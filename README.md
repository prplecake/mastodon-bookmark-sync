[![Dotnet](https://github.com/prplecake/mastodon-bookmark-sync/actions/workflows/dotnet.yml/badge.svg)](https://github.com/prplecake/mastodon-bookmark-sync/actions/workflows/dotnet.yml)
[![Dotnet Release](https://github.com/prplecake/mastodon-bookmark-sync/actions/workflows/dotnet-release.yml/badge.svg)](https://github.com/prplecake/mastodon-bookmark-sync/actions/workflows/dotnet-release.yml)
[![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/prplecake/mastodon-bookmark-sync?include_prereleases)](https://github.com/prplecake/mastodon-bookmark-sync/releases/latest)

# mastodon-bookmark-sync

mastodon-bookmark-sync is a command-line utility to synchronize Mastodon
bookmarks with Pinboard.

mastodon-bookmark-sync supports multiple fediverse accounts.

**Supported bookmarking services:**

- LinkAce
- ~~Pinboard~~ *(deprecated)*

## getting started

You probably just want to grab an executable from the [Releases][releases] page.

[releases]:https://github.com/prplecake/mastodon-bookmark-sync/releases

Before you can start using mastodon-bookmark-sync, you'll need to configure
it. An example configuration can be found [here][config-blob]. You can also
just copy the example:

```shell
cp appsettings.Example.json appsettings.json
vim appsettings.json # don't forget to edit it!
```

You'll need an access token from your Mastodon server.
i.e. `your.instance/settings/applications`

[fediverse-access-token]:https://tools.splat.soy/fediverse-access-token/

And you'll need your
[Pinboard API token](https://pinboard.in/settings/password).

Once you've got it configured, just run it. You might want to add it to your
crontab, or your other favorite task scheduler:

```text
0 */6 * * * cd /path/to/mastodon-bookmark-sync; ./mastodon-bookmark-sync
```

[config-blob]:https://github.com/prplecake/mastodon-bookmark-sync/blob/master/BookmarkSync.CLI/appsettings.Example.json

## questions

* [Help! I can't run this on my Mac.](https://github.com/prplecake/mastodon-bookmark-sync/wiki/Questions#help-i-cant-run-this-on-my-mac)