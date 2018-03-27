namespace MarkdownToRW
{
    partial class ImageUploadWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageUploadWindow));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSaveCredentials = new System.Windows.Forms.CheckBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressUpload = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkUpdateMarkdown = new System.Windows.Forms.CheckBox();
            this.listPreviews = new System.Windows.Forms.ListBox();
            this.chkOptimizeImages = new System.Windows.Forms.CheckBox();
            this.btnMacPasteUsername = new System.Windows.Forms.Button();
            this.btnMacPastePassword = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 346);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(306, 37);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Enabled = false;
            this.btnUpload.Location = new System.Drawing.Point(12, 303);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(306, 37);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload and update Markdown";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(200, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnMacPastePassword);
            this.groupBox1.Controls.Add(this.btnMacPasteUsername);
            this.groupBox1.Controls.Add(this.chkSaveCredentials);
            this.groupBox1.Controls.Add(this.btnVerify);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 131);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RW Wordpress Login Info";
            // 
            // chkSaveCredentials
            // 
            this.chkSaveCredentials.AutoSize = true;
            this.chkSaveCredentials.Location = new System.Drawing.Point(9, 77);
            this.chkSaveCredentials.Name = "chkSaveCredentials";
            this.chkSaveCredentials.Size = new System.Drawing.Size(106, 17);
            this.chkSaveCredentials.TabIndex = 10;
            this.chkSaveCredentials.Text = "Save Credentials";
            this.chkSaveCredentials.UseVisualStyleBackColor = true;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(9, 100);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(291, 23);
            this.btnVerify.TabIndex = 9;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(200, 45);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(324, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Image Paths (double click to view image)";
            // 
            // progressUpload
            // 
            this.progressUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressUpload.Location = new System.Drawing.Point(15, 438);
            this.progressUpload.Name = "progressUpload";
            this.progressUpload.Size = new System.Drawing.Size(1018, 23);
            this.progressUpload.TabIndex = 9;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Location = new System.Drawing.Point(12, 408);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1021, 20);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Please verify account";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(12, 388);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1021, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Status:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkUpdateMarkdown
            // 
            this.chkUpdateMarkdown.Checked = true;
            this.chkUpdateMarkdown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateMarkdown.Location = new System.Drawing.Point(12, 149);
            this.chkUpdateMarkdown.Name = "chkUpdateMarkdown";
            this.chkUpdateMarkdown.Size = new System.Drawing.Size(306, 31);
            this.chkUpdateMarkdown.TabIndex = 12;
            this.chkUpdateMarkdown.Text = "Update original markdown file with uploaded image URLs";
            this.chkUpdateMarkdown.UseVisualStyleBackColor = true;
            // 
            // listPreviews
            // 
            this.listPreviews.FormattingEnabled = true;
            this.listPreviews.HorizontalScrollbar = true;
            this.listPreviews.Location = new System.Drawing.Point(327, 28);
            this.listPreviews.Name = "listPreviews";
            this.listPreviews.ScrollAlwaysVisible = true;
            this.listPreviews.Size = new System.Drawing.Size(706, 355);
            this.listPreviews.TabIndex = 13;
            this.listPreviews.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listPreviews_MouseDoubleClick);
            // 
            // chkOptimizeImages
            // 
            this.chkOptimizeImages.Checked = true;
            this.chkOptimizeImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOptimizeImages.Location = new System.Drawing.Point(12, 186);
            this.chkOptimizeImages.Name = "chkOptimizeImages";
            this.chkOptimizeImages.Size = new System.Drawing.Size(306, 31);
            this.chkOptimizeImages.TabIndex = 14;
            this.chkOptimizeImages.Text = "Optimize copies of PNG images";
            this.chkOptimizeImages.UseVisualStyleBackColor = true;
            // 
            // btnMacPasteUsername
            // 
            this.btnMacPasteUsername.Location = new System.Drawing.Point(67, 19);
            this.btnMacPasteUsername.Name = "btnMacPasteUsername";
            this.btnMacPasteUsername.Size = new System.Drawing.Size(127, 20);
            this.btnMacPasteUsername.TabIndex = 11;
            this.btnMacPasteUsername.Text = "Paste from clipboard";
            this.btnMacPasteUsername.UseVisualStyleBackColor = true;
            this.btnMacPasteUsername.Visible = false;
            this.btnMacPasteUsername.Click += new System.EventHandler(this.btnMacPasteUsername_Click);
            // 
            // btnMacPastePassword
            // 
            this.btnMacPastePassword.Location = new System.Drawing.Point(67, 45);
            this.btnMacPastePassword.Name = "btnMacPastePassword";
            this.btnMacPastePassword.Size = new System.Drawing.Size(127, 20);
            this.btnMacPastePassword.TabIndex = 12;
            this.btnMacPastePassword.Text = "Paste from clipboard";
            this.btnMacPastePassword.UseVisualStyleBackColor = true;
            this.btnMacPastePassword.Visible = false;
            this.btnMacPastePassword.Click += new System.EventHandler(this.btnMacPastePassword_Click);
            // 
            // ImageUploadWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 467);
            this.Controls.Add(this.chkOptimizeImages);
            this.Controls.Add(this.listPreviews);
            this.Controls.Add(this.chkUpdateMarkdown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressUpload);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageUploadWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Image Uploader BETA";
            this.Load += new System.EventHandler(this.ImageUploadWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressUpload;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkSaveCredentials;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkUpdateMarkdown;
        private System.Windows.Forms.ListBox listPreviews;
        private System.Windows.Forms.CheckBox chkOptimizeImages;
        private System.Windows.Forms.Button btnMacPastePassword;
        private System.Windows.Forms.Button btnMacPasteUsername;
    }
}