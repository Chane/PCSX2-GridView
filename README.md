# PCSX2-GridView

![.NET](https://github.com/Chane/PCSX2-GridView/actions/workflows/dotnet.yml/badge.svg)

PCSX2-GridView is a desktop library browser for PlayStation 2 ISO files.

It scans a game library folder for `.iso` files, attempts to match each game with a `.jpg` cover file (by filename), displays results in a grid/list, and can launch a selected game with `PCSX2 --nogui`.

## Repository Layout

- `PCSX2GridView/`: Avalonia desktop application (UI + view models)
- `PCSX2GridView.Backend/`: media discovery and cover-art services
- `PCSX2GridView.Backend.Tests/`: backend unit tests
- `PCSX2GridView.Tests/`: UI/view-model focused unit tests

## Requirements

- .NET SDK 5.0.x
- PCSX2 available on your `PATH` as `PCSX2`

## Library Path Configuration

Set `PCSX2_GRIDVIEW_LIBRARY_PATH` to your PS2 library root.

Expected structure:

```text
<library-root>/
	Game One.iso
	Game Two.iso
	Covers/
		Game One.jpg
		Game Two.jpg
```

If the variable is not set, the app falls back to:

`<user-home>/Games/ROMs/PlayStation2`

### Linux/macOS

```bash
export PCSX2_GRIDVIEW_LIBRARY_PATH="$HOME/Games/ROMs/PlayStation2"
```

### Windows (PowerShell)

```powershell
$env:PCSX2_GRIDVIEW_LIBRARY_PATH = "$HOME\Games\ROMs\PlayStation2"
```

## Build and Test

```bash
dotnet restore PCSX2GridView.sln
dotnet build PCSX2GridView.sln --no-restore
dotnet test PCSX2GridView.sln --no-build --verbosity minimal
```

## Run

```bash
dotnet run --project PCSX2GridView/PCSX2GridView.csproj
```

## CI

GitHub Actions workflow `.github/workflows/dotnet.yml` runs restore, build, and tests for pushes and PRs targeting `main`.
