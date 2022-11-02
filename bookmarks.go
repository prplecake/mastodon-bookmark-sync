package main

import (
	"bytes"
	"encoding/json"
	"log"
)

type bookmark struct {
	ID         string
	URL        string
	Content    string
	Account    account
	Visibility string
}

type account struct {
	Acct string
}

func processBookmarks(data []byte) ([]bookmark, error) {
	var rawBookmarks []bookmark
	if err := json.Unmarshal(data, &rawBookmarks); err != nil {
		return nil, err
	}
	if debug {
		var jsonResult bytes.Buffer
		_ = json.Indent(&jsonResult, data, "", "  ")
		log.Print(jsonResult.String())
	}

	var bookmarks []bookmark
	for _, bookmark := range rawBookmarks {
		if bookmark.Visibility == "private" || bookmark.Visibility == "direct" {
			if debug {
				log.Printf("Removing bookmark %s due to visibility setting", bookmark.URL)
			}
		} else {
			bookmarks = append(bookmarks, bookmark)
		}
	}

	return bookmarks, nil
}
