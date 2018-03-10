namespace MarkdownToRW
{
    partial class PreviewWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewWindow));
            this.WebPreview = new Awesomium.Windows.Forms.WebControl();
            this.SuspendLayout();
            // 
            // WebPreview
            // 
            this.WebPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebPreview.Location = new System.Drawing.Point(0, 0);
            this.WebPreview.Name = "WebPreview";
            this.WebPreview.Size = new System.Drawing.Size(904, 612);
            this.WebPreview.TabIndex = 0;
            // 
            // PreviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 612);
            this.Controls.Add(this.WebPreview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(920, 0);
            this.Name = "PreviewWindow";
            this.Text = "Preview";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PreviewWindow_FormClosed);
            this.Load += new System.EventHandler(this.PreviewWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Awesomium.Windows.Forms.WebControl WebPreview;
    }
}