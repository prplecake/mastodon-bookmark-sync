[![Go](https://github.com/prplecake/mastodon-bookmark-sync/actions/workflows/go.yml/badge.svg)](https://github.com/prplecake/mastodon-bookmark-sync/actions/workflows/go.yml)
[![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/prplecake/mastodon-bookmark-sync?include_prereleases)](https://github.com/prplecake/mastodon-bookmark-sync/releases/latest)

# mastodon-bookmark-sync

mastodon-bookmark-sync is a command-line utility to synchronize Mastodon
bookmarks with Pinboard.

mastodon-bookmark-sync supports multiple fediverse accounts.

## requirements

* Go 1.17

## installation

Download the latest release for your system from the
[Releases page](https://github.com/prplecake/mastodon-bookmark-sync/releases/latest).

## usage

Before you can start using mastodon-bookmark-sync, you'll need to configure
it. An example configuration can be found [here][config-blob]. You can also
just copy the example:

```shell
cp config.example.yaml config.yaml
vim config.yaml # don't forget to edit it!
```

You'll need an access token as well. You can get on from the [Fediverse
Instance Access Token Generator][fediverse-access-token].

[fediverse-access-token]:https://tools.splat.soy/fediverse-access-token/

And you'll need your
[Pinboard API token](https://pinboard.in/settings/password).

## developing

Build the thing:

```shell
go build
```

Then you can use it:

```shell
./mastodon-bookmark-sync
```

You could also specify the configuration file to use via the command
line:

```shell
./mastodon-bookmark-sync -c /path/to/your/config.yaml
```

This would allow you to place the executable (and configuration)
anywhere on your system. Once gof is configured, you might want to add it to
your crontab, or your other favorite task scheduler:

```text
0 */6 * * * cd /path/to/mastodon-bookmark-sync; ./mastodon-bookmark-sync
```

or:

```text
0 */6 * * * /path/to/mastodon-bookmark-sync -c /path/to/config.yaml
```

[config-blob]:https://github.com/prplecake/mastodon-bookmark-sync/blob/master/config.example.yaml
