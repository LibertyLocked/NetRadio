using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadioPlayer
{
    public class StreamTitleChangedEventArgs : EventArgs
    {
        public readonly string Title;

        public StreamTitleChangedEventArgs(string title)
        {
            this.Title = title;
        }
    }
}
