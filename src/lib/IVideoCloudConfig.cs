using System;

namespace BrightcoveVideoCloudIntegration
{
    public interface IVideoCloudConfig
    {
        string PublisherId { get; set; }
        string ReadToken { get; set; }
        string WriteToken { get; set; }
        string ReadUrl { get; set; }
        string WriteUrl { get; set; }
        string DefaultVideoPlayerId { get; set; }
        string DefaultPlaylistPlayerId { get; set; }
    }
}
