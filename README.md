# godot-hub 

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mattiascibien_godot-hub&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mattiascibien_godot-hub)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=mattiascibien_godot-hub&metric=bugs)](https://sonarcloud.io/summary/new_code?id=mattiascibien_godot-hub)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=mattiascibien_godot-hub&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=mattiascibien_godot-hub)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=mattiascibien_godot-hub&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=mattiascibien_godot-hub)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=mattiascibien_godot-hub&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=mattiascibien_godot-hub)
[![CI](https://github.com/mattiascibien/godot-hub/actions/workflows/ci.yml/badge.svg)](https://github.com/mattiascibien/godot-hub/actions/workflows/ci.yml) 
![GitHub](https://img.shields.io/github/license/mattiascibien/godot-hub) 
![GitHub release (latest by date)](https://img.shields.io/github/v/release/mattiascibien/godot-hub)

Command line tool to install and use godot versions

# Installation

## Standalone

Download the appropriate file from the GitHub releases (or artifact from CI) for your desired platform
and extract it on some directory available on your `$PATH`.

## As a `dotnet` tool

You need to have the `dotnet` CLI installed on your system and the [.NET 6.0 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0).

From your command line run

```bash
dotnet tool install --global godot-hub
```

or, if you want to use a CI version:

```bash
dotnet tool install --global godot-hub --prerelease
```

# Usage

After installing the tool, you can use it from the command line and see what you can do with it:

```bash
godot-hub --help # (or godot-hub -h)
```

# Localization

You can help localizing this software by translating the messages in the resx files: a [Transifex Project](https://www.transifex.com/mattias-cibien/godot-hub/) is set up containing the main languages
that the software supports. You can also request new languages to be added by using [Github Issues](https://github.com/mattiascibien/godot-hub/issues) using the `localization` label.

# Development

The project is developed using C#, [.NET](https://dotnet.microsoft.com/) 6.0 and [Visual Studio 2022](https://visualstudio.microsoft.com/) but it can be also developed
using [Visual Studio Code](https://code.visualstudio.com/) or [Jetbrains Rider](https://www.jetbrains.com/rider/). The only requirement is the [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0).

Building the Windows Store version can be built only on Windows and with Visual Studio.
