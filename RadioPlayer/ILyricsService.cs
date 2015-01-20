using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadioPlayer
{
    public interface ILyricsService
    {
        string Name { get; }
        string GetLyrics(string title, string artist);
    }
}
