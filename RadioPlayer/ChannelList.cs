using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RadioPlayer
{
    [Serializable]
    public class ChannelList
    {
        [XmlArray("Items"), XmlArrayItem(typeof(ChannelInfo), ElementName = "Item")]
        public List<ChannelInfo> Items { get; set; }

        public ChannelList()
        {
            Items = new List<ChannelInfo>();
        }

        public bool Add(ChannelInfo channel)
        {
            if (this.Exists(channel) || String.IsNullOrWhiteSpace(channel.Url) || String.IsNullOrWhiteSpace(channel.Name)) return false;
            else
            {
                Items.Add(channel);
                return true;
            }
        }

        public bool Remove(ChannelInfo channel)
        {
            return Items.Remove(channel);
        }

        public bool Exists(ChannelInfo channel)
        {
            // search the list for a channel with same url
            foreach (ChannelInfo info in Items)
            {
                if (info.Url == channel.Url) return true;
            }
            return false;
        }
    }
}
