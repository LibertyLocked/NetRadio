namespace RadioPlayer.WinForms
{
    partial class FormLyricsOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxLyrics = new System.Windows.Forms.GroupBox();
            this.radioButtonMusix = new System.Windows.Forms.RadioButton();
            this.radioButtonGenius = new System.Windows.Forms.RadioButton();
            this.radioButtonDirect = new System.Windows.Forms.RadioButton();
            this.radioButtonMetro = new System.Windows.Forms.RadioButton();
            this.radioButtonAz = new System.Windows.Forms.RadioButton();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBoxLyrics.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLyrics
            // 
            this.groupBoxLyrics.Controls.Add(this.radioButtonMusix);
            this.groupBoxLyrics.Controls.Add(this.radioButtonGenius);
            this.groupBoxLyrics.Controls.Add(this.radioButtonDirect);
            this.groupBoxLyrics.Controls.Add(this.radioButtonMetro);
            this.groupBoxLyrics.Controls.Add(this.radioButtonAz);
            this.groupBoxLyrics.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLyrics.Name = "groupBoxLyrics";
            this.groupBoxLyrics.Size = new System.Drawing.Size(152, 149);
            this.groupBoxLyrics.TabIndex = 0;
            this.groupBoxLyrics.TabStop = false;
            this.groupBoxLyrics.Text = "Lyrics Service";
            // 
            // radioButtonMusix
            // 
            this.radioButtonMusix.AutoSize = true;
            this.radioButtonMusix.Location = new System.Drawing.Point(6, 111);
            this.radioButtonMusix.Name = "radioButtonMusix";
            this.radioButtonMusix.Size = new System.Drawing.Size(81, 17);
            this.radioButtonMusix.TabIndex = 4;
            this.radioButtonMusix.TabStop = true;
            this.radioButtonMusix.Text = "Musixmatch";
            this.radioButtonMusix.UseVisualStyleBackColor = true;
            // 
            // radioButtonGenius
            // 
            this.radioButtonGenius.AutoSize = true;
            this.radioButtonGenius.Location = new System.Drawing.Point(6, 65);
            this.radioButtonGenius.Name = "radioButtonGenius";
            this.radioButtonGenius.Size = new System.Drawing.Size(58, 17);
            this.radioButtonGenius.TabIndex = 2;
            this.radioButtonGenius.TabStop = true;
            this.radioButtonGenius.Text = "Genius";
            this.radioButtonGenius.UseVisualStyleBackColor = true;
            // 
            // radioButtonDirect
            // 
            this.radioButtonDirect.AutoSize = true;
            this.radioButtonDirect.Location = new System.Drawing.Point(6, 42);
            this.radioButtonDirect.Name = "radioButtonDirect";
            this.radioButtonDirect.Size = new System.Drawing.Size(76, 17);
            this.radioButtonDirect.TabIndex = 1;
            this.radioButtonDirect.TabStop = true;
            this.radioButtonDirect.Text = "Directlyrics";
            this.radioButtonDirect.UseVisualStyleBackColor = true;
            // 
            // radioButtonMetro
            // 
            this.radioButtonMetro.AutoSize = true;
            this.radioButtonMetro.Location = new System.Drawing.Point(6, 88);
            this.radioButtonMetro.Name = "radioButtonMetro";
            this.radioButtonMetro.Size = new System.Drawing.Size(79, 17);
            this.radioButtonMetro.TabIndex = 3;
            this.radioButtonMetro.TabStop = true;
            this.radioButtonMetro.Text = "MetroLyrics";
            this.radioButtonMetro.UseVisualStyleBackColor = true;
            // 
            // radioButtonAz
            // 
            this.radioButtonAz.AutoSize = true;
            this.radioButtonAz.Location = new System.Drawing.Point(6, 19);
            this.radioButtonAz.Name = "radioButtonAz";
            this.radioButtonAz.Size = new System.Drawing.Size(66, 17);
            this.radioButtonAz.TabIndex = 0;
            this.radioButtonAz.TabStop = true;
            this.radioButtonAz.Text = "AZLyrics";
            this.radioButtonAz.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(114, 167);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(50, 22);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // FormLyricsOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 201);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxLyrics);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormLyricsOptions";
            this.Text = "FormLyricsOptions";
            this.Load += new System.EventHandler(this.FormLyricsOptions_Load);
            this.groupBoxLyrics.ResumeLayout(false);
            this.groupBoxLyrics.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLyrics;
        private System.Windows.Forms.RadioButton radioButtonAz;
        private System.Windows.Forms.RadioButton radioButtonMetro;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.RadioButton radioButtonDirect;
        private System.Windows.Forms.RadioButton radioButtonGenius;
        private System.Windows.Forms.RadioButton radioButtonMusix;

    }
}