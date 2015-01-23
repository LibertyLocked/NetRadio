using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RadioPlayer
{
    [Serializable]
    public class ScrapList
    {
        [XmlArray]
        public List<string> Items { get; set; }

        public ScrapList()
        {
            Items = new List<string>();
        }

        public bool Exists(string item)
        {
            return Items.Contains(item);
        }

        public bool Add(string item)
        {
            if (Exists(item))
            {
                return false;
            }
            else
            {
                Items.Add(item);
                return true;
            }
        }

        public bool Remove(string item)
        {
            if (Exists(item))
            {
                Items.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a copy of the scraps.
        /// </summary>
        /// <returns></returns>
        public String[] GetArray()
        {
            string[] arr = new string[Items.Count];
            Items.CopyTo(arr);

            return arr;
        }
    }
}
