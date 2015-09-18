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
    public partial class FormChannelEditor : Form
    {
        ChannelInfo editInfo;

        public FormChannelEditor(ChannelInfo editInfo)
        {
            InitializeComponent();
            this.editInfo = editInfo;

            if (editInfo != null)
            {
                textBoxDescription.Text = editInfo.Name;
                textBoxUrl.Text = editInfo.Url;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // dont add blanks
            if (!String.IsNullOrWhiteSpace(textBoxDescription.Text) && !String.IsNullOrWhiteSpace(textBoxUrl.Text))
            {
                if (editInfo == null)
                {
                    // create new
                    Properties.Settings.Default.ChannelList.Add(new ChannelInfo(textBoxDescription.Text, textBoxUrl.Text));
                }
                else
                {
                    // edit exisiting
                    editInfo.Name = textBoxDescription.Text;
                    editInfo.Url = textBoxUrl.Text;
                }
                Properties.Settings.Default.Save();
                this.Close();
            }
        }

        private void FormChannelEditor_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void FormChannelEditor_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    ChannelInfo imported = ChannelImporter.ImportFromFile(files[0]);
                    textBoxDescription.Text = imported.Name;
                    textBoxUrl.Text = imported.Url;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to import channel");
            }
        }
    }
}
