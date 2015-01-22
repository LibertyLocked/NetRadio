using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RadioPlayer.WinForms
{
    public partial class FormScrapEditor : Form
    {
        ScrapList scrapList;

        public FormScrapEditor(ScrapList scrapList)
        {
            InitializeComponent();
            this.scrapList = scrapList;

            ReloadListBox();
        }

        private void ReloadListBox()
        {
            // clear the listbox
            listBoxScraps.Items.Clear();

            // get the scraps
            string[] scraps = scrapList.GetList();

            foreach (string scrap in scraps)
            {
                listBoxScraps.Items.Add(scrap);
            }

            // save the scraplist
            scrapList.SaveToFile("scraps.txt");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBoxScraps.SelectedItem != null)
            {
                scrapList.Remove(listBoxScraps.SelectedItem.ToString());
                ReloadListBox();
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (listBoxScraps.SelectedItem != null)
            {
                Clipboard.SetText(listBoxScraps.SelectedItem.ToString());
                MessageBox.Show("Copied to clipboard." + Environment.NewLine + listBoxScraps.SelectedItem.ToString());
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ReloadListBox();
        }

        private void listBoxScraps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                buttonDelete_Click(sender, e);
        }
    }
}
