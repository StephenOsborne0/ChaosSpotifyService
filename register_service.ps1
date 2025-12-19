$ServiceName = "ChaosSpotifyService"
$ExeName = "ChaosSpotifyService.exe"
$Tfm = "net10.0-windows10.0.19041.0"
$ExePath = Join-Path -Path $PSScriptRoot -ChildPath "ChaosSpotifyService\bin\Debug\$Tfm\$ExeName"

if (!(Test-Path $ExePath)) {
    # Try release path if debug not found
    $ExePath = Join-Path -Path $PSScriptRoot -ChildPath "ChaosSpotifyService\bin\Release\$Tfm\$ExeName"
}

if (!(Test-Path $ExePath)) {
    Write-Host "Executable not found. Please build the project first (dotnet build)." -ForegroundColor Red
    exit
}

New-Service -Name $ServiceName -BinaryPathName $ExePath -DisplayName "Chaos Spotify Monitor Service" -Description "Monitors Spotify window title and writes currently playing track to song.txt" -StartupType Automatic
Start-Service -Name $ServiceName

Write-Host "Service $ServiceName registered and started successfully." -ForegroundColor Green
