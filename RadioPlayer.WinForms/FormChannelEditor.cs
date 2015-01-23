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
    }
}
