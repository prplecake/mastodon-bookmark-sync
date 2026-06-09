[![builds.sr.ht status](https://builds.sr.ht/~prplecake/mastodon-bookmark-sync/commits/master.svg)](https://builds.sr.ht/~prplecake/mastodon-bookmark-sync/commits/master?)

# mastodon-bookmark-sync

mastodon-bookmark-sync is a command-line utility to synchronize Mastodon
bookmarks with LinkAce or other bookmarking services.

mastodon-bookmark-sync supports multiple Mastodon accounts.

**Supported bookmarking services:**

- [Briefkasten]
- [LinkAce]
- [linkding]
- [Pinboard]

[Briefkasten]:https://github.com/ndom91/briefkasten
[LinkAce]:https://linkace.org/
[linkding]:https://github.com/sissbruecker/linkding
[Pinboard]:https://pinboard.in/

## resources

This repository recently moved from GitHub to Sourcehut. If you're viewing this
on GitHub, feel free to head over to our new project home:
[~prplecake/mastodon-bookmark-sync].

Planning, feature requests, and bugs live at our issue tracker,
[~prplecake/mastodon-bookmark-sync][tracker]. 

[Send patches](https://git-send-email.io/) to our development list,
[~prplecake/mastodon-bookmark-sync-devel@lists.sr.ht]. Questions and other
non-development related discussions are had on our discuss list,
[~prplecake/mastodon-bookmark-sync-discuss@lists.sr.ht]. Feel free to subscribe
to our low-volume announce list,
[~prplecake/mastodon-bookmark-sync-announce@lists.sr.ht], for release
announcements. 

[~prplecake/mastodon-bookmark-sync]:https://sr.ht/~prplecake/mastodon-bookmark-sync
[tracker]:https://todo.sr.ht/~prplecake/mastodon-bookmark-sync
[~prplecake/mastodon-bookmark-sync-devel@lists.sr.ht]:https://lists.sr.ht/~prplecake/mastodon-bookmark-sync-devel
[~prplecake/mastodon-bookmark-sync-discuss@lists.sr.ht]:https://lists.sr.ht/~prplecake/mastodon-bookmark-sync-discuss
[~prplecake/mastodon-bookmark-sync-announce@lists.sr.ht]:https://lists.sr.ht/~prplecake/mastodon-bookmark-sync-announce

## getting started

You probably just want to grab an executable from the [Releases][releases] page.

[releases]:https://git.sr.ht/~prplecake/mastodon-bookmark-sync/refs

Before you can start using mastodon-bookmark-sync, you'll need to configure
it. An example configuration can be found [here][config-blob]. You can also
just copy the example:

```shell
cp appsettings.Example.json appsettings.Production.json
vim appsettings.Production.json # don't forget to edit it!
```

You'll need an access token from your Mastodon server.
i.e. `your.instance/settings/applications`

And you'll need an API token for your bookmarking service of choice.

See the wiki for [configuration examples][config-examples].

[config-examples]:https://man.sr.ht/~prplecake/mastodon-bookmark-sync/configuration-examples.md

Once you've got it configured, just run it. You might want to add it to your
crontab, or your other favorite task scheduler:

```text
0 */6 * * * cd /path/to/mastodon-bookmark-sync; ./mastodon-bookmark-sync
```

[config-blob]:https://git.sr.ht/~prplecake/mastodon-bookmark-sync/tree/master/item/src/BookmarkSync.CLI/config.Example.yaml

## questions

* [Help! I can't run this on my Mac.](https://man.sr.ht/~prplecake/mastodon-bookmark-sync/questions.md#help-i-cant-run-this-on-my-mac)
