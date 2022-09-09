package main

import (
	"log"
	"os"
	"time"

	"gopkg.in/yaml.v2"
)

var (
	configFile string
)

type config struct {
	Instances  []instanceConfig
	Pinboard   pinboardConfig
	LastSynced time.Time
}

type instanceConfig struct {
	AccessToken, InstanceURL string
	DeleteBookmarks          bool
}

type pinboardConfig struct {
	APIToken string
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
	return config
}

func (cf *config) updateLastSynced() {
	log.Println("updating lastsynced key...")
	cf.LastSynced = time.Now()
}

func (cf *config) Save() error {
	log.Println("saving config to file...")
	cfbytes, err := yaml.Marshal(cf)
	if err != nil {
		log.Fatalln("Failed to marshal config: ", err.Error())
	}
	err = os.WriteFile(configFile, cfbytes, 0644)
	if err != nil {
		log.Fatalf("Failed to save config to file. Error: %s", err.Error())
	}

	return nil
}
