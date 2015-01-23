using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RadioPlayer
{
    [Serializable]
    public class ChannelInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public ChannelInfo()
        {
        }

        public ChannelInfo(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
