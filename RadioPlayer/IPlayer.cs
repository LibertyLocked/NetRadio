using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadioPlayer
{
    public interface IPlayer
    {
        StreamingPlaybackState State { get; }
        string StreamTitle { get; }
        float Volume { get; set; }
        event EventHandler StreamTitleChanged;

        void StartPlayback(string url);
        void PausePlayback();
        void StopPlayback();
        void Update();
    }
}
