using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

namespace RadioPlayer
{
    public class ChartLyricsService : ILyricsService
    {
        private const string STR_XNAMESPACE = "http://api.chartlyrics.com/";

        public string Name
        {
            get { return "Chart Lyrics"; }
        }

        public string GetLyrics(string title, string artist)
        {
            try
            {
                title = RemoveIllegalChars(title);
                artist = RemoveIllegalChars(artist);

                WebClient client = new WebClient();
                string queryUrl = "http://api.chartlyrics.com/apiv1.asmx/SearchLyric?artist=" + artist + "&song=" + title;
                string result = client.DownloadString(queryUrl);
                client.Dispose();
                // Construct a XDocument out of it
                XElement root = XElement.Parse(result);

                XNamespace xns = STR_XNAMESPACE;
                string lyricChecksum = "", lyricId = "";

                var query = from lyricResult in root.Elements()
                            where (string)lyricResult.Element(xns + "Artist") == artist
                            select lyricResult;
                if (query.Count() > 0)
                {
                    lyricId = query.First().Element(xns + "LyricId").Value;
                    if (lyricId == "0")
                    {
                        return "No lyrics found for this song.";
                    }
                    else
                    {
                        lyricChecksum = query.First().Element(xns + "LyricChecksum").Value;

                        // Now that we have both checksum and lyric, we can get it from chartlyrics
                        return GetLyricFromChartLyrics(lyricId, lyricChecksum);
                    }
                }
                else
                {
                    return "Song does not exist in database.";
                }
            }
            catch (Exception ex)
            {
                return "An exception occurred when searching lyric." + Environment.NewLine + ex.Message;
            }
        }

        private string GetLyricFromChartLyrics(string id, string checksum)
        {
            try
            {
                WebClient webClient = new WebClient();
                string queryUrl = "http://api.chartlyrics.com/apiv1.asmx/GetLyric?lyricId=" + id + "&lyricCheckSum=" + checksum;

                string result = webClient.DownloadString(queryUrl);
                webClient.Dispose();

                XElement root = XElement.Parse(result);
                XNamespace xns = STR_XNAMESPACE;

                return root.Element(xns + "Lyric").Value;
            }
            catch (Exception ex)
            {
                return "An exception occurred when getting lyric." + Environment.NewLine + ex.Message;
            }
        }

        private string RemoveIllegalChars(string input)
        {
            return input.Replace("\'", "").Replace("&", "");
        }
    }
}
