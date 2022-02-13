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

		for i, bkmk := range bookmarks {
			bkmk.Content = html.UnescapeString(p.Sanitize(
				strings.Replace(bkmk.Content, "</p><p>", "\n\n", -1),
			))
			if debug {
				log.Printf("%d:\n%s\n%s\n%s\n\n",
					i, bkmk.URL,
					bkmk.Content,
					bkmk.Account.Acct,
				)
			}

			log.Printf("Saving URL: %s", bkmk.URL)

			var (
				descriptionTrimmed = false
				trimmedDescription string
				extended           string
				tags               = fmt.Sprintf(
					"%s %s",
					"@"+bkmk.Account.Acct,
					"via:mastodon-bookmark-sync",
				)
			)

			if len(bkmk.Content) > PINBOARD_DESC_MAX_LENGTH {
				trimmedDescription = bkmk.Content[PINBOARD_DESC_MAX_LENGTH:]
				descriptionTrimmed = true
			}

			if descriptionTrimmed {
				extended = bkmk.Content
				bkmk.Content = trimmedDescription
			}

			data := url.Values{}
			data.Set("description", bkmk.Content)
			data.Set("url", bkmk.URL)
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
		}
	}
}
