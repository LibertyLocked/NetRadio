using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RadioPlayer.WinForms.Properties;
using RadioPlayer;

namespace RadioPlayer.WinForms
{
    public partial class FormLyricsOptions : Form
    {
        List<RadioButton> radioButtons;

        public FormLyricsOptions()
        {
            InitializeComponent();
            // add all radio buttons to list and group box
            radioButtons = new List<RadioButton>();
            int posY = 20;
            foreach (LyricsWebProvider p in Enum.GetValues(typeof(LyricsWebProvider)))
            {
                string pName = Enum.GetName(typeof(LyricsWebProvider), p);
                RadioButton pRadioButton = new RadioButton();
                pRadioButton.Text = pName;
                pRadioButton.Location = new Point(6, posY);
                posY += 24;
                radioButtons.Add(pRadioButton);
                groupBoxLyrics.Controls.Add(pRadioButton);
            }
        }

        private void FormLyricsOptions_Load(object sender, EventArgs e)
        {
            radioButtons[Settings.Default.LyricsWebProvider].Select();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var selected = groupBoxLyrics.Controls.OfType<RadioButton>()
                           .FirstOrDefault(n => n.Checked);
            Settings.Default.LyricsWebProvider = radioButtons.IndexOf(selected);
            Settings.Default.LyricsWebUrl = LyricsWeb.GetLyricsWebServiceUrl((LyricsWebProvider)Settings.Default.LyricsWebProvider);
            Settings.Default.Save();
            this.Close();
        }
    }
}
