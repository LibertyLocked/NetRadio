using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RadioPlayer.WinForms.Properties;

namespace RadioPlayer.WinForms
{
    public partial class FormLyricsOptions : Form
    {
        List<RadioButton> radioButtons;

        public FormLyricsOptions()
        {
            InitializeComponent();
            // add all radio buttons to array
            radioButtons = new List<RadioButton>() { radioButtonAz, radioButtonDirect, radioButtonGenius, radioButtonMetro, radioButtonMusix };
        }

        private void FormLyricsOptions_Load(object sender, EventArgs e)
        {
            radioButtons[Settings.Default.LyricsServiceIndex].Select();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var selected = groupBoxLyrics.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);
            Settings.Default.LyricsServiceIndex = radioButtons.IndexOf(selected);
            Settings.Default.Save();
            this.Close();
        }

        public static string GetRequestUrl(string req)
        {
            int svcIndex = Settings.Default.LyricsServiceIndex;
            // sanitize request
            req = Sanitize(req);
            if (svcIndex == 0) return ("http://search.azlyrics.com/search.php?q=" + req);
            else if (svcIndex == 1) return ("http://www.directlyrics.com/search/?q=" + req);
            else if (svcIndex == 2) return ("http://genius.com/search?q=" + req);
            else if (svcIndex == 3) return ("http://www.metrolyrics.com/search.html?search=" + req);
            else if (svcIndex == 4) return ("https://www.musixmatch.com/search/" + req);
            else throw new Exception("Invalid lyrics service index");
        }

        private static string Sanitize(string req)
        {
            req = req.Replace('-', ' ');
            return req;
        }
    }
}
