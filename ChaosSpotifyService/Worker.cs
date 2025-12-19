using Windows.Media.Control;

namespace ChaosSpotifyService;

/// <summary>
///     A background service that monitors the currently playing track on Spotify using Windows Media Controls.
///     This approach is more consistent than reading window titles and allows detecting when music is paused.
/// </summary>
public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly string _outputFilePath = Path.Combine(AppContext.BaseDirectory, "song.txt");
    private bool _isCleared;
    private string _lastTrack = string.Empty;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Service starting. Output file path: {FilePath}", Path.GetFullPath(_outputFilePath));
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                var sessions = sessionManager.GetSessions();
                
                string currentTrack = string.Empty;
                bool spotifyFound = false;

                foreach (var session in sessions)
                {
                    if (session.SourceAppUserModelId.Contains("Spotify", StringComparison.OrdinalIgnoreCase))
                    {
                        spotifyFound = true;
                        var playbackInfo = session.GetPlaybackInfo();
                        logger.LogDebug("Spotify session found. Status: {Status}", playbackInfo.PlaybackStatus);

                        if (playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        {
                            var mediaProperties = await session.TryGetMediaPropertiesAsync();
                            if (mediaProperties != null && !string.IsNullOrWhiteSpace(mediaProperties.Title))
                            {
                                currentTrack = $"{mediaProperties.Artist} - {mediaProperties.Title}";
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(currentTrack))
                {
                    if (!_isCleared)
                    {
                        await File.WriteAllTextAsync(_outputFilePath, string.Empty, stoppingToken);
                        _isCleared = true;
                        _lastTrack = string.Empty;
                        
                        if (spotifyFound)
                        {
                            logger.LogInformation("Spotify is paused or inactive. Cleared {FilePath}", _outputFilePath);
                        }
                        else
                        {
                            logger.LogInformation("Spotify session not found. Cleared {FilePath}", _outputFilePath);
                        }
                    }
                }
                else
                {
                    if (currentTrack != _lastTrack)
                    {
                        await File.WriteAllTextAsync(_outputFilePath, currentTrack, stoppingToken);
                        _lastTrack = currentTrack;
                        _isCleared = false;
                        logger.LogInformation("Currently playing: {Track} (Written to {FilePath})", currentTrack, _outputFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while checking Spotify status via Media Controls");
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}
