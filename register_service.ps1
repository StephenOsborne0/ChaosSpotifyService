$ServiceName = "ChaosSpotifyService"
$ExeName = "ChaosSpotifyService.exe"
$ExePath = Join-Path -Path $PSScriptRoot -ChildPath $ExeName

if (!(Test-Path $ExePath)) {
    Write-Host "Executable not found. Please build the project first (dotnet build)." -ForegroundColor Red
    exit
}

New-Service -Name $ServiceName -BinaryPathName $ExePath -DisplayName "Chaos Spotify Monitor Service" -Description "Monitors Spotify window title and writes currently playing track to song.txt" -StartupType Automatic
Start-Service -Name $ServiceName

Write-Host "Service $ServiceName registered and started successfully." -ForegroundColor Green
