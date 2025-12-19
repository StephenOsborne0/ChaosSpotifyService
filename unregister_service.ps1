$ServiceName = "ChaosSpotifyService"

if (Get-Service -Name $ServiceName -ErrorAction SilentlyContinue) {
    Stop-Service -Name $ServiceName
    Remove-Service -Name $ServiceName
    Write-Host "Service $ServiceName unregistered successfully." -ForegroundColor Green
} else {
    Write-Host "Service $ServiceName not found." -ForegroundColor Yellow
}
