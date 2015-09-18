using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadioPlayer
{
    public class LyricsWeb
    {
        public static string GetRequestUrl(string svc, string title)
        {
            return (svc + SanitizeRequest(title));
        }

        public static string GetRequestUrl(LyricsWebProvider provider, string title)
        {
            return (GetLyricsWebServiceUrl(provider) + SanitizeRequest(title));
        }

        private static string SanitizeRequest(string req)
        {
            return req.Replace(@"'", "").Replace(@"&", "").Replace(@"-", "");
        }

        public static string GetLyricsWebServiceUrl(LyricsWebProvider provider)
        {
            if (provider == LyricsWebProvider.AZLyrics) return "http://search.azlyrics.com/search.php?q=";
            else if (provider == LyricsWebProvider.Directlyrics) return "http://www.directlyrics.com/search/?q=";
            else if (provider == LyricsWebProvider.Genius) return "http://genius.com/search?q=";
            else if (provider == LyricsWebProvider.MetroLyrics) return "http://www.metrolyrics.com/search.html?search=";
            else if (provider == LyricsWebProvider.Musixmatch) return "https://www.musixmatch.com/search/";
            else throw new Exception("Unknown provider!");
        }
    }
}
