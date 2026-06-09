## Briefkasten
```yaml
...
App:
  Bookmarking:
    Service: Briefkasten
    ApiToken:
    BriefkastenUri: https://briefkastenhq.com
...
```

## LinkAce
```yaml
...
App:
  Bookmarking:
    Service: LinkAce
    ApiToken: 
    LinkAceUri: https://links.example.com
    ApiVersion: v1
...
```

* ApiVersion should be one of: "v1" or "v2". v2 is used if this key is not specified. 

## linkding
```yaml
...
App:
  Bookmarking:
    Service: linkding
    ApiToken:
    LinkdingUri: https://links.example.com
...
```

## Pinboard
```yaml
...
App:
  Bookmarking:
    Service: Pinboard
    ApiToken:
...
```