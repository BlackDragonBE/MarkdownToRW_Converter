namespace MarkdownToRW
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtMarkdown = new System.Windows.Forms.RichTextBox();
            this.txtHtml = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowPreview = new System.Windows.Forms.Button();
            this.btnOpenMarkdown = new System.Windows.Forms.Button();
            this.btnCopyClipboard = new System.Windows.Forms.Button();
            this.btnWordpress = new System.Windows.Forms.Button();
            this.lblMarkdownPath = new System.Windows.Forms.LinkLabel();
            this.openMarkdownDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMarkdown
            // 
            this.txtMarkdown.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.txtMarkdown, 2);
            this.txtMarkdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMarkdown.Location = new System.Drawing.Point(3, 33);
            this.txtMarkdown.Name = "txtMarkdown";
            this.txtMarkdown.ReadOnly = true;
            this.txtMarkdown.Size = new System.Drawing.Size(422, 450);
            this.txtMarkdown.TabIndex = 3;
            this.txtMarkdown.Text = "";
            // 
            // txtHtml
            // 
            this.txtHtml.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.txtHtml, 2);
            this.txtHtml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHtml.Location = new System.Drawing.Point(431, 33);
            this.txtHtml.Name = "txtHtml";
            this.txtHtml.ReadOnly = true;
            this.txtHtml.Size = new System.Drawing.Size(424, 450);
            this.txtHtml.TabIndex = 4;
            this.txtHtml.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Markdown";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtMarkdown, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtHtml, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnShowPreview, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenMarkdown, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCopyClipboard, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnWordpress, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblMarkdownPath, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(858, 526);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(431, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 30);
            this.label2.TabIndex = 6;
            this.label2.Text = "RW Wordpress Ready HTML";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowPreview
            // 
            this.btnShowPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowPreview.Location = new System.Drawing.Point(645, 3);
            this.btnShowPreview.Name = "btnShowPreview";
            this.btnShowPreview.Size = new System.Drawing.Size(210, 24);
            this.btnShowPreview.TabIndex = 7;
            this.btnShowPreview.Text = "Show Preview...";
            this.btnShowPreview.UseVisualStyleBackColor = true;
            this.btnShowPreview.Click += new System.EventHandler(this.btnShowPreview_Click);
            // 
            // btnOpenMarkdown
            // 
            this.btnOpenMarkdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenMarkdown.Location = new System.Drawing.Point(217, 3);
            this.btnOpenMarkdown.Name = "btnOpenMarkdown";
            this.btnOpenMarkdown.Size = new System.Drawing.Size(208, 24);
            this.btnOpenMarkdown.TabIndex = 8;
            this.btnOpenMarkdown.Text = "Open Markdown File...";
            this.btnOpenMarkdown.UseVisualStyleBackColor = true;
            this.btnOpenMarkdown.Click += new System.EventHandler(this.btnOpenMarkdown_Click);
            // 
            // btnCopyClipboard
            // 
            this.btnCopyClipboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopyClipboard.Location = new System.Drawing.Point(645, 489);
            this.btnCopyClipboard.Name = "btnCopyClipboard";
            this.btnCopyClipboard.Size = new System.Drawing.Size(210, 34);
            this.btnCopyClipboard.TabIndex = 9;
            this.btnCopyClipboard.Text = "Copy HTML to clipboard";
            this.btnCopyClipboard.UseVisualStyleBackColor = true;
            this.btnCopyClipboard.Click += new System.EventHandler(this.btnCopyClipboard_Click);
            // 
            // btnWordpress
            // 
            this.btnWordpress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWordpress.Location = new System.Drawing.Point(431, 489);
            this.btnWordpress.Name = "btnWordpress";
            this.btnWordpress.Size = new System.Drawing.Size(208, 34);
            this.btnWordpress.TabIndex = 10;
            this.btnWordpress.Text = "Upload Images...";
            this.btnWordpress.UseVisualStyleBackColor = true;
            this.btnWordpress.Click += new System.EventHandler(this.btnWordpress_Click);
            // 
            // lblMarkdownPath
            // 
            this.lblMarkdownPath.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMarkdownPath, 2);
            this.lblMarkdownPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMarkdownPath.Location = new System.Drawing.Point(3, 486);
            this.lblMarkdownPath.Name = "lblMarkdownPath";
            this.lblMarkdownPath.Size = new System.Drawing.Size(422, 40);
            this.lblMarkdownPath.TabIndex = 11;
            this.lblMarkdownPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblMarkdownPath_LinkClicked);
            // 
            // openMarkdownDialog
            // 
            this.openMarkdownDialog.FileName = "tutorial.md";
            this.openMarkdownDialog.Filter = "Markdown Files|*.md;*.markdown;*.mdown;*.mkdn;*.mkd;*.mdwn;*.mdtxt;*.mdtext;*.tex" +
    "t;*.txt;*.rmd";
            this.openMarkdownDialog.Title = "Select the markdown file.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 526);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "MainForm";
            this.Text = "Markdown To Wordpress HTML Converter v0.84";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox txtMarkdown;
        private System.Windows.Forms.RichTextBox txtHtml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnShowPreview;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOpenMarkdown;
        private System.Windows.Forms.OpenFileDialog openMarkdownDialog;
        private System.Windows.Forms.Button btnCopyClipboard;
        private System.Windows.Forms.Button btnWordpress;
        private System.Windows.Forms.LinkLabel lblMarkdownPath;
    }
}

