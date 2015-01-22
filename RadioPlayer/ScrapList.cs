using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RadioPlayer
{
    public class ScrapList
    {
        List<String> items;

        public ScrapList()
        {
            items = new List<string>();
        }

        public ScrapList(List<String> items)
        {
            this.items = items;
        }

        public ScrapList(string plainTextFile)
        {
            items = new List<string>();
            using (StreamReader sr = new StreamReader(plainTextFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        items.Add(line);
                    }
                }
            }
        }

        public bool Exists(string item)
        {
            return items.Contains(item);
        }

        public bool Add(string item)
        {
            if (Exists(item))
            {
                return false;
            }
            else
            {
                items.Add(item);
                return true;
            }
        }

        public bool Remove(string item)
        {
            if (Exists(item))
            {
                items.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveToFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                foreach (string item in items)
                {
                    if (!String.IsNullOrWhiteSpace(item))
                        sw.WriteLine(item);
                }
            }
        }

        /// <summary>
        /// Gets a copy of the scraps.
        /// </summary>
        /// <returns></returns>
        public String[] GetList()
        {
            string[] arr = new string[items.Count];
            items.CopyTo(arr);

            return arr;
        }
    }
}
