using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RadioPlayer
{
    public partial class FormPlayer : Form
    {
        public static WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();

        public static void PlayMusicFromURL(string url)
        {
            player.URL = url;

            player.settings.volume = 100;

            player.controls.play();
        }

        public static void PlayerStop(string url)
        {
            player.controls.stop();
        }

        public static void SetPlayerVolume(int volume)
        {
            player.settings.volume = volume;
        }


        public FormPlayer()
        {
            InitializeComponent();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            PlayMusicFromURL(textBoxUrl.Text);
        }

        private void textBoxVolume_TextChanged(object sender, EventArgs e)
        {
            SetPlayerVolume(Int32.Parse(textBoxVolume.Text));
        }
    }
}
