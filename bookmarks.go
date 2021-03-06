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
	bookmarks := []bookmark{}
	if err := json.Unmarshal(data, &bookmarks); err != nil {
		return nil, err
	}
	if debug {
		var jsonResult bytes.Buffer
		json.Indent(&jsonResult, data, "", "  ")
		log.Print(jsonResult.String())
	}

	return bookmarks, nil
}
