using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

using NAudio;
using NAudio.Wave;

namespace RadioPlayer.WinForms
{
    public partial class FormPlayer : Form
    {
        IPlayer player = new ShoutcastPlayer();
        ILyricsService lyricsService;

        public FormPlayer()
        {
            InitializeComponent();
            textBoxUrl.Text = "http://205.164.62.22:7800/";
            labelTitle.Text = "";
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            player.StartPlayback(textBoxUrl.Text);
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            player.PausePlayback();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            player.StopPlayback();
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            labelVolume.Text = trackBarVolume.Value.ToString();

            player.Volume = trackBarVolume.Value / (float)trackBarVolume.Maximum;
        }

        private void timerPlayer_Tick(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Update();
                labelState.Text = player.State.ToString();
                labelTitle.Text = player.StreamTitle;
            }

        }

        private void FormPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            player.StopPlayback();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelTitle_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(labelTitle.Text);
            MessageBox.Show("Stream title copied to clipboard.");
        }
    }
}
