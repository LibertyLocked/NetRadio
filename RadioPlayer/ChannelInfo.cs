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

        //public static bool operator ==(ChannelInfo c1, ChannelInfo c2)
        //{
        //    return (c1.Name == c2.Name) && (c1.Url == c2.Url);
        //}

        //public static bool operator !=(ChannelInfo c1, ChannelInfo c2)
        //{
        //    return (c1.Name != c2.Name) || (c1.Url != c2.Url);
        //}
    }
}
