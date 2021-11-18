# godot-hub 

[![CI](https://github.com/mattiascibien/godot-hub/actions/workflows/ci.yml/badge.svg)](https://github.com/mattiascibien/godot-hub/actions/workflows/ci.yml) ![GitHub](https://img.shields.io/github/license/mattiascibien/godot-hub) ![GitHub release (latest by date)](https://img.shields.io/github/v/release/mattiascibien/godot-hub)

Command line tool to install and use godot versions

# Installation

## Standalone

Download the appropriate file from the GitHub releases (or artifact from CI) for your desired platform
and extract it on some directory available on your `$PATH`.

## Windows Store

The app will be published on the Windows Store soon. It will be installable form there and will act like a tool installed in your path `$PATH`.

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

# Development

The project is developed using C#, [.NET](https://dotnet.microsoft.com/) 6.0 and [Visual Studio 2022](https://visualstudio.microsoft.com/) but it can be also developed
using [Visual Studio Code](https://code.visualstudio.com/) or [Jetbrains Rider](https://www.jetbrains.com/rider/). The only requirement is the [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0).

Building the Windows Store version can be built only on Windows and with Visual Studio.
