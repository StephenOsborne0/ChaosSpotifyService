# Chaos Spotify Monitor Service

A Windows Service that monitors what you are currently playing on Spotify and writes it to a text file (`song.txt`). This is useful for OBS or other streaming software to display the current track.

## Features

- Uses Windows Media Control APIs for robust track detection.
- Automatically clears the output file when Spotify is paused or closed.
- Runs as a background Windows Service.
- No Spotify API keys or login required.

## Requirements

- Windows 10 (Build 19041) or later.
- .NET 10 SDK or later.

## Setup and Installation

### 1. Build the project

Open a terminal (PowerShell or Command Prompt) in the project root and run:

```powershell
dotnet build
```

### 2. Install the Service

The project includes PowerShell scripts to register and start the application as a Windows Service. You can run them from the project root or from the build output directory.

1. Open **PowerShell as Administrator**.
2. Navigate to the build output directory (e.g., `ChaosSpotifyService\bin\Debug\net10.0-windows10.0.19041.0\`).
3. Run the registration script:

```powershell
.\register_service.ps1
```

The service `ChaosSpotifyService` will be created and started automatically. It is configured to start automatically with Windows.

> **Note:** If you run the script from the project root, it will automatically look for the built executable in the debug/release folders.

## Usage

Once the service is running, it will create a file named `song.txt` in the same directory as the executable.

- When a song is playing: `song.txt` contains `Artist - Title`.
- When paused or Spotify is closed: `song.txt` is cleared (empty).

## Uninstallation

To stop and remove the service, run the unregistration script from the same directory:

```powershell
.\unregister_service.ps1
```

## Logs

The service logs its activity to the Windows Event Viewer. You can find logs under:
`Windows Logs -> Application` with the source `Chaos Spotify Monitor Service`.
