using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RadioPlayer.WinForms
{
    class ChannelImporter
    {
        public static ChannelInfo ImportFromFile(string file)
        {
            if (file.EndsWith(".pls")) return parsePls(file);
            else if (file.EndsWith(".m3u")) return parseM3u(file);
            else if (file.EndsWith(".xspf")) return parseXspf(file);
            else throw new IOException("Unknown file format!");
        }

        private static ChannelInfo parsePls(string file)
        {
            IniFile plsFile = new IniFile(file);
            string url = plsFile.GetValue("playlist", "File1", "url");
            string title = plsFile.GetValue("playlist", "Title1", "title");
            return new ChannelInfo(title, url);
        }

        private static ChannelInfo parseM3u(string file)
        {
            bool readUrl = false;
            string url = "url", title = "title";
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith(@"#EXTINF"))
                    {
                        title = line.Substring(line.IndexOf(',') + 1);
                        readUrl = true;
                    }
                    else if (readUrl)
                    {
                        url = line;
                        break;
                    }
                }
            }
            return new ChannelInfo(title, url);
        }

        private static ChannelInfo parseXspf(string file)
        {
            throw new NotImplementedException("xspf parsing not implemented");
        }
    }
}
