using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using RadioPlayer.WinForms.Properties;
using System.Xml.Serialization;

namespace RadioPlayer.WinForms
{
    public partial class FormPlayer : Form
    {
        IPlayer player;
        //ILyricsService lyricsService = new ChartLyricsService();
        string oldTitle, newTitle;

        public FormPlayer()
        {
            InitializeComponent();
            SetupForm();
            SetupPlayer();
        }

        public FormPlayer(string file)
        {
            InitializeComponent();
            SetupForm();
            SetupPlayer();
            try
            {
                ChannelInfo ch = ChannelImporter.ImportFromFile(file);
                textBoxUrl.Text = ch.Url;
                player.StartPlayback(ch.Url);

                // ask user if we want to bookmark the channel
                if (!Settings.Default.ChannelList.Exists(ch))
                {
                    DialogResult result = MessageBox.Show("Do you want to bookmark this channel?" + Environment.NewLine + ch.Name, 
                        "Add to bookmarks", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Settings.Default.ChannelList.Add(ch);
                        ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Unable to parse file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupForm()
        {
            labelTitle.Text = "";

            if (Settings.Default.ScrapList == null)
                Settings.Default.ScrapList = new ScrapList();
            if (Settings.Default.ChannelList == null)
            {
                Settings.Default.ChannelList = new ChannelList();
                Settings.Default.ChannelList.Add(new ChannelInfo("1.FM - Absolute Country Hits", "http://205.164.62.22:7800/"));
                Settings.Default.ChannelList.Add(new ChannelInfo("Smooth Jazz Florida", "http://us1.internet-radio.com:11094/"));
                Settings.Default.ChannelList.Add(new ChannelInfo("KCSN 88.5 FM - Smart Rock", "http://130.166.82.184:8000/"));
            }

            Settings.Default.Save();

            // Bound channel listbox with channel list
            ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
            ReloadListBoxWithScrapList(listBoxScraps, Settings.Default.ScrapList);

            trackBarVolume.Value = (int)(Settings.Default.Volume * trackBarVolume.Maximum);
        }

        private void SetupPlayer()
        {
            player = new ShoutcastPlayer();
            player.OnException += OnPlayerException;
            player.Volume = Settings.Default.Volume;
        }

        private void OnTitleChanged()
        {
            //ShowLyrics();
        }

        //private void ShowLyrics()
        //{
        //    try
        //    {
        //        string title = "", artist = "";

        //        if (!newTitle.Contains('-'))
        //        {
        //            title = newTitle;
        //        }
        //        else
        //        {
        //            artist = newTitle.Substring(0, newTitle.IndexOf('-')).Trim();
        //            title = newTitle.Substring(newTitle.IndexOf('-') + 1).Trim();
        //        }

        //        //textBoxLyrics.Text = "Title: " + title + Environment.NewLine + "Artist: " + artist;

        //        //textBoxLyrics.Text = lyricsService.GetLyrics(title, artist);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void ReloadListBoxWithScrapList(ListBox listBox, ScrapList scrapList)
        {
            string[] scraps = scrapList.GetArray();
            listBox.Items.Clear();

            foreach (string scrap in scraps)
            {
                listBox.Items.Add(scrap);
            }

            // save to config
            Properties.Settings.Default.Save();
        }

        private void ReloadListBoxWithChannelList(ListBox listBox, ChannelList channelList)
        {
            listBox.DataSource = null;
            listBox.Items.Clear();
            listBox.DataSource = Settings.Default.ChannelList.Items;
            listBox.DisplayMember = "Name";
            listBox.ValueMember = "Url";

            Settings.Default.Save();
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
            if (!String.IsNullOrWhiteSpace(textBoxUrl.Text))
            {
                player.StartPlayback(textBoxUrl.Text);
            }
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            player.PausePlayback();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            player.StopPlayback();
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
                    labelState.Text += " - " + player.StreamBitrate / 1000 + " K";
                }
                
                // Enables scrap button if title is available
                buttonScrap.Visible = !String.IsNullOrWhiteSpace(player.StreamTitle);
                // Enable / disable scrap button
                buttonScrap.Enabled = !Settings.Default.ScrapList.Exists(player.StreamTitle);

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
                    OnTitleChanged();
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
                //Clipboard.SetText(labelTitle.Text);
                DialogResult result = MessageBox.Show(labelTitle.Text + Environment.NewLine + "Do you want to search for lyrics?",
                    labelTitle.Text, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(LyricsWeb.GetRequestUrl(Settings.Default.LyricsWebUrl, labelTitle.Text));
                }
            }
        }

        private void buttonScrap_Click(object sender, EventArgs e)
        {
            Settings.Default.ScrapList.Add(player.StreamTitle);

            // Reload listBoxScraps
            ReloadListBoxWithScrapList(listBoxScraps, Settings.Default.ScrapList);
        }

        private void editScrapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new FormScrapEditor(scrapList).ShowDialog();
            //ReloadListBoxScraps();
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            player.Volume = trackBarVolume.Value / (float)trackBarVolume.Maximum;
            Settings.Default.Volume = player.Volume;
            Settings.Default.Save();
        }

        private void listBoxChannels_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listBoxChannels.SelectedIndex = listBoxChannels.IndexFromPoint(e.Location);
                contextMenuStripChannels.Show(Control.MousePosition);
            }
        }

        private void listBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxChannels.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                listBoxChannels.SelectedIndex = index;
                if (listBoxChannels.SelectedItem != null && (player.State == StreamingPlaybackState.Stopped || textBoxUrl.Text != listBoxChannels.SelectedValue.ToString()))
                {
                    if (player.State == StreamingPlaybackState.Playing || player.State == StreamingPlaybackState.Paused)
                    {
                        player.StopPlayback();
                    }

                    textBoxUrl.Text = listBoxChannels.SelectedValue.ToString();

                    while (player.State != StreamingPlaybackState.Stopped) { }
                    player.StartPlayback(textBoxUrl.Text);
                }
            }
        }

        private void listBoxScraps_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                listBoxScraps.SelectedIndex = listBoxScraps.IndexFromPoint(e.Location);
                contextMenuStripScraps.Show(Control.MousePosition);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBoxScraps.SelectedItem != null)
            {
                Clipboard.SetText(listBoxScraps.SelectedItem.ToString());
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listBoxScraps.SelectedItem != null)
            {
                Settings.Default.ScrapList.Remove(listBoxScraps.SelectedItem.ToString());
                ReloadListBoxWithScrapList(listBoxScraps, Settings.Default.ScrapList);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ReloadListBoxWithScrapList(listBoxScraps, Settings.Default.ScrapList);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormChannelEditor(null).ShowDialog();
            ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
        }

        private void configFilepathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Application.LocalUserAppDataPath + Environment.NewLine +
                "Do you want to open the folder in Windows Explorer?",
                "Config files are stored in...", MessageBoxButtons.YesNo) == 
                System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(Application.LocalUserAppDataPath);
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Radio Player (Windows Forms) is part of Project NetRadio" + Environment.NewLine + 
                "Licensed under GPLv2"
            , "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxChannels.SelectedItem != null)
            {
                Settings.Default.ChannelList.Remove((ChannelInfo)listBoxChannels.SelectedItem);
                ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxChannels.SelectedItem != null)
            {
                new FormChannelEditor((ChannelInfo)listBoxChannels.SelectedItem).ShowDialog();
                ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
            }
        }

        private void lyricsServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormLyricsOptions().ShowDialog();
        }

        private void listBoxChannels_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listBoxChannels_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    ChannelInfo imported = ChannelImporter.ImportFromFile(files[0]);
                    Settings.Default.ChannelList.Add(imported);
                    ReloadListBoxWithChannelList(listBoxChannels, Settings.Default.ChannelList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to import channel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void importFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please drag .pls or .m3u file to channels panel", "Not implemented");
        }
    }
}
