using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadioPlayer
{
    public interface IPlayer : IDisposable
    {
        StreamingPlaybackState State { get; }
        string StreamTitle { get; }
        int StreamBitrate { get; }
        float Volume { get; set; }
        event UnhandledExceptionEventHandler OnException;

        void StartPlayback(string url);
        void PausePlayback();
        void StopPlayback();
        void Update();
    }
}
