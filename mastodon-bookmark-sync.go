package main

import (
	"flag"
	"fmt"
	"io"
	"log"
	"net/http"
	"net/url"
	"strings"

	"github.com/microcosm-cc/bluemonday"
	"golang.org/x/net/html"
)

var (
	conf                     *config
	debug                    bool
	PINBOARD_DESC_MAX_LENGTH = 255
	PINBOARD_API_URL         = "https://api.pinboard.in/v1/posts/add"
)

type article struct {
	Title, URL, Summary string
}

func main() {
	var (
		configFile string
	)
	flag.StringVar(&configFile, "c", "config.yaml", "the configuration file to use")
	flag.BoolVar(&debug, "debug", false, "enable debugging")
	flag.Parse()

	conf = readConfig(configFile)

	if debug {
		log.Println(conf)
	}

	log.Println("mastodon-bookmark-sync starting up...")
	log.Println("initializing bluemonday")
	p := bluemonday.StripTagsPolicy()

	for _, instance := range conf.Instances {
		log.Printf("Fetching bookmarks from instance: [%s]...", instance.InstanceURL)
		apiURL := instance.InstanceURL + "/api/v1/bookmarks"

		var req *http.Request
		req, err := http.NewRequest("GET", apiURL, nil)
		if err != nil {
			log.Fatal(err)
		}
		req.Header.Set("Authorization", "Bearer "+instance.AccessToken)

		c := &http.Client{}

		resp, err := c.Do(req)
		if err != nil {
			log.Fatal(err)
		}
		defer resp.Body.Close()

		body, err := io.ReadAll(resp.Body)
		if err != nil {
			log.Fatal(err)
		}
		bookmarks, err := processBookmarks(body)
		if err != nil {
			log.Fatal(err)
		}

		for i := len(bookmarks) - 1; i >= 0; i-- {
			bookmarks[i].Content = html.UnescapeString(p.Sanitize(
				strings.Replace(bookmarks[i].Content, "</p><p>", "\n\n", -1),
			))
			if debug {
				log.Printf("%d:\n%s\n%s\n%s\n\n",
					i, bookmarks[i].URL,
					bookmarks[i].Content,
					bookmarks[i].Account.Acct,
				)
			}

			log.Printf("Saving URL: %s", bookmarks[i].URL)

			var (
				descriptionTrimmed = false
				trimmedDescription string
				extended           string
				tags               = fmt.Sprintf(
					"%s %s",
					"via:@"+bookmarks[i].Account.Acct,
					"via:mastodon-bookmark-sync",
				)
			)

			if len(bookmarks[i].Content) > PINBOARD_DESC_MAX_LENGTH {
				trimmedDescription = bookmarks[i].Content[PINBOARD_DESC_MAX_LENGTH:]
				descriptionTrimmed = true
			}

			if descriptionTrimmed {
				extended = bookmarks[i].Content
				bookmarks[i].Content = trimmedDescription
			}

			data := url.Values{}
			data.Set("description", bookmarks[i].Content)
			data.Set("url", bookmarks[i].URL)
			data.Set("shared", "no")
			data.Set("extended", extended)
			data.Set("tags", string(tags))
			data.Set("auth_token", conf.Pinboard.APIToken)
			fullURL := PINBOARD_API_URL + "?" + data.Encode()

			if debug {
				log.Print(fullURL)
			}

			req, err := http.NewRequest("GET", fullURL, nil)
			if err != nil {
				log.Fatal(err)
			}

			c := &http.Client{}

			resp, err := c.Do(req)
			if err != nil {
				log.Fatal(err)
			}
			defer resp.Body.Close()

			log.Printf("resp.StatusCode: %d", resp.StatusCode)

			if debug {
				buf := new(strings.Builder)
				_, err = io.Copy(buf, resp.Body)
				if err != nil {
					log.Fatal(err)
				}
				log.Printf("resp.Body: %s", buf.String())
			}

			if instance.DeleteBookmarks && resp.StatusCode == 200 && bookmarks[i].Visibility != "private" && bookmarks[i].Visibility != "direct" {
				// only delete bookmarks if successfully saved by Pinboard and
				// don't delete bookmarks of private or direct statuses
				instance.deleteBookmark(bookmarks[i])
			}
		}
	}
}

func (instance *instanceConfig) deleteBookmark(status bookmark) {
	apiURL := instance.InstanceURL + "/api/v1/statuses/" + status.ID + "/unbookmark"

	req, err := http.NewRequest("POST", apiURL, nil)
	if err != nil {
		log.Fatal(err)
	}

	req.Header.Set("Authorization", "Bearer "+instance.AccessToken)

	c := &http.Client{}

	resp, err := c.Do(req)
	if err != nil {
		log.Fatal(err)
	}
	defer resp.Body.Close()

	if debug {
		buf := new(strings.Builder)
		_, err = io.Copy(buf, resp.Body)
		if err != nil {
			log.Fatal(err)
		}
		log.Printf("resp.Body: %s", buf.String())
	}
}
