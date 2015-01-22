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
        ILyricsService lyricsService = new ChartLyricsService();
        string oldTitle, newTitle;

        public FormPlayer()
        {
            InitializeComponent();
            labelTitle.Text = "";
            player.OnException += OnPlayerException;
        }

        private void SetupPlayer()
        {
            player = new ShoutcastPlayer();
            player.OnException += OnPlayerException;
        }

        private void ShowLyrics()
        {
            try
            {
                string title = "", artist = "";

                if (!newTitle.Contains('-'))
                {
                    title = newTitle;
                }
                else
                {
                    artist = newTitle.Substring(0, newTitle.IndexOf('-')).Trim();
                    title = newTitle.Substring(newTitle.IndexOf('-') + 1).Trim();
                }

                //textBoxLyrics.Text = "Title: " + title + Environment.NewLine + "Artist: " + artist;

                //textBoxLyrics.Text = lyricsService.GetLyrics(title, artist);
            }
            catch (Exception ex)
            {
                textBoxLyrics.Text = ex.ToString();
            }
        }

        private void OnPlayerException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "player just threw an exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Dispose the old player, and create a new one
            player.Dispose();
            SetupPlayer();
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
                labelTitle.Text = player.StreamTitle;
                labelState.Text = player.State.ToString();

                if (player.State == StreamingPlaybackState.Playing)
                {
                    labelState.Text += " - " + player.StreamBitrate / 1000 + " Kbps";
                }
                

                // Enable or disable buttons and textboxes based on state
                if (player.State == StreamingPlaybackState.Buffering)
                {
                    // turn all the buttons off when buffering
                    buttonPlay.Enabled = false;
                    buttonPause.Enabled = false;
                    buttonStop.Enabled = false;
                }
                else
                {
                    buttonPlay.Enabled = player.State == StreamingPlaybackState.Paused || player.State == StreamingPlaybackState.Stopped;
                    buttonPause.Enabled = player.State == StreamingPlaybackState.Playing;
                    buttonStop.Enabled = player.State == StreamingPlaybackState.Paused || player.State == StreamingPlaybackState.Playing;
                }
                textBoxUrl.ReadOnly = player.State != StreamingPlaybackState.Stopped;
                listBoxChannels.Enabled = player.State != StreamingPlaybackState.Buffering;

                // Check if stream title is changed
                newTitle = player.StreamTitle;
                if (oldTitle != newTitle && !String.IsNullOrWhiteSpace(newTitle))
                {
                    ShowLyrics();
                }
                oldTitle = newTitle;
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
            if (!String.IsNullOrWhiteSpace(labelTitle.Text))
            {
                Clipboard.SetText(labelTitle.Text);
                MessageBox.Show("Stream title copied to clipboard." + Environment.NewLine + labelTitle.Text);
            }
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBoxUrl.Text != (string)listBoxChannels.SelectedItem)
            {
                if (player.State == StreamingPlaybackState.Playing || player.State == StreamingPlaybackState.Paused)
                {
                    player.StopPlayback();
                }

                textBoxUrl.Text = (string)listBoxChannels.SelectedItem;

                while (player.State != StreamingPlaybackState.Stopped) { }
                player.StartPlayback(textBoxUrl.Text);
            }
        }
    }
}
