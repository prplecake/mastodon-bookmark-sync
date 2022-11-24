package main

import (
	"log"
	"os"
	"time"

	"gopkg.in/yaml.v2"
)

const (
	HttpUserAgent = "mastodon-bookmark-sync/1.0"
)

var (
	configFile string
)

type config struct {
	Instances  []instanceConfig
	Pinboard   pinboardConfig
	LastSynced time.Time
	HttpConfig httpConfig
}

type instanceConfig struct {
	AccessToken, InstanceURL string
	DeleteBookmarks          bool
}

type pinboardConfig struct {
	APIToken string
}

type httpConfig struct {
	UserAgent string
}

func readConfig(fileName string) *config {
	log.Println("reading config...")
	configFile = fileName
	config := new(config)
	cf, err := os.ReadFile(configFile)
	if err != nil {
		log.Fatalln("Failed to read config: ", err)
	}
	err = yaml.Unmarshal(cf, &config)
	if err != nil {
		log.Panic(err)
	}
	if debug {
		log.Printf("Config:\n\n%v", config)
	}
	config.HttpConfig.UserAgent = HttpUserAgent
	return config
}

func (cf *config) updateLastSynced() {
	log.Println("updating LastSynced key...")
	cf.LastSynced = time.Now()
}

func (cf *config) Save() error {
	log.Println("saving config to file...")
	cfBytes, err := yaml.Marshal(cf)
	if err != nil {
		log.Fatalln("Failed to marshal config: ", err.Error())
	}
	err = os.WriteFile(configFile, cfBytes, 0644)
	if err != nil {
		log.Fatalf("Failed to save config to file. Error: %s", err.Error())
	}

	return nil
}
