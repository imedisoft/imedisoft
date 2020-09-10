namespace Imedisoft.Forms
{
    partial class FormPath
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPath));
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.exportPathTextBox = new System.Windows.Forms.TextBox();
            this.exportPathButton = new OpenDental.UI.Button();
            this.pathButton = new OpenDental.UI.Button();
            this.exportPathLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.letterMergePathLabel = new System.Windows.Forms.Label();
            this.letterMergeButton = new OpenDental.UI.Button();
            this.letterMergePathTextBox = new System.Windows.Forms.TextBox();
            this.storageGroupBox = new System.Windows.Forms.GroupBox();
            this.localPathButton = new OpenDental.UI.Button();
            this.localPathTextBox = new System.Windows.Forms.TextBox();
            this.localPathLabel = new System.Windows.Forms.Label();
            this.exportPathGroupBox = new System.Windows.Forms.GroupBox();
            this.letterMergePathGroupBox = new System.Windows.Forms.GroupBox();
            this.storageGroupBox.SuspendLayout();
            this.exportPathGroupBox.SuspendLayout();
            this.letterMergePathGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(426, 474);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(512, 474);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(6, 69);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(537, 20);
            this.pathTextBox.TabIndex = 1;
            // 
            // exportPathTextBox
            // 
            this.exportPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathTextBox.Location = new System.Drawing.Point(6, 89);
            this.exportPathTextBox.Name = "exportPathTextBox";
            this.exportPathTextBox.Size = new System.Drawing.Size(537, 20);
            this.exportPathTextBox.TabIndex = 1;
            // 
            // exportPathButton
            // 
            this.exportPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathButton.Image = ((System.Drawing.Image)(resources.GetObject("exportPathButton.Image")));
            this.exportPathButton.Location = new System.Drawing.Point(549, 89);
            this.exportPathButton.Name = "exportPathButton";
            this.exportPathButton.Size = new System.Drawing.Size(25, 20);
            this.exportPathButton.TabIndex = 2;
            this.exportPathButton.Click += new System.EventHandler(this.ExportPathButton_Click);
            // 
            // pathButton
            // 
            this.pathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pathButton.Image = ((System.Drawing.Image)(resources.GetObject("pathButton.Image")));
            this.pathButton.Location = new System.Drawing.Point(549, 69);
            this.pathButton.Name = "pathButton";
            this.pathButton.Size = new System.Drawing.Size(25, 20);
            this.pathButton.TabIndex = 2;
            this.pathButton.Click += new System.EventHandler(this.PathButton_Click);
            // 
            // exportPathLabel
            // 
            this.exportPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathLabel.Location = new System.Drawing.Point(6, 26);
            this.exportPathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.exportPathLabel.Name = "exportPathLabel";
            this.exportPathLabel.Size = new System.Drawing.Size(568, 60);
            this.exportPathLabel.TabIndex = 0;
            this.exportPathLabel.Text = resources.GetString("exportPathLabel.Text");
            // 
            // pathLabel
            // 
            this.pathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathLabel.Location = new System.Drawing.Point(6, 26);
            this.pathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(568, 40);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = resources.GetString("pathLabel.Text");
            // 
            // letterMergePathLabel
            // 
            this.letterMergePathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergePathLabel.Location = new System.Drawing.Point(6, 26);
            this.letterMergePathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.letterMergePathLabel.Name = "letterMergePathLabel";
            this.letterMergePathLabel.Size = new System.Drawing.Size(568, 60);
            this.letterMergePathLabel.TabIndex = 0;
            this.letterMergePathLabel.Text = resources.GetString("letterMergePathLabel.Text");
            // 
            // letterMergeButton
            // 
            this.letterMergeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergeButton.Image = ((System.Drawing.Image)(resources.GetObject("letterMergeButton.Image")));
            this.letterMergeButton.Location = new System.Drawing.Point(549, 89);
            this.letterMergeButton.Name = "letterMergeButton";
            this.letterMergeButton.Size = new System.Drawing.Size(25, 20);
            this.letterMergeButton.TabIndex = 2;
            this.letterMergeButton.Click += new System.EventHandler(this.LetterMergeButton_Click);
            // 
            // letterMergePathTextBox
            // 
            this.letterMergePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergePathTextBox.Location = new System.Drawing.Point(6, 89);
            this.letterMergePathTextBox.Name = "letterMergePathTextBox";
            this.letterMergePathTextBox.Size = new System.Drawing.Size(537, 20);
            this.letterMergePathTextBox.TabIndex = 1;
            // 
            // storageGroupBox
            // 
            this.storageGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.storageGroupBox.Controls.Add(this.localPathButton);
            this.storageGroupBox.Controls.Add(this.pathLabel);
            this.storageGroupBox.Controls.Add(this.pathButton);
            this.storageGroupBox.Controls.Add(this.localPathTextBox);
            this.storageGroupBox.Controls.Add(this.pathTextBox);
            this.storageGroupBox.Controls.Add(this.localPathLabel);
            this.storageGroupBox.Location = new System.Drawing.Point(12, 12);
            this.storageGroupBox.Name = "storageGroupBox";
            this.storageGroupBox.Size = new System.Drawing.Size(580, 160);
            this.storageGroupBox.TabIndex = 0;
            this.storageGroupBox.TabStop = false;
            this.storageGroupBox.Text = "Folder for storing images and documents";
            // 
            // localPathButton
            // 
            this.localPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.localPathButton.Image = ((System.Drawing.Image)(resources.GetObject("localPathButton.Image")));
            this.localPathButton.Location = new System.Drawing.Point(549, 118);
            this.localPathButton.Name = "localPathButton";
            this.localPathButton.Size = new System.Drawing.Size(25, 20);
            this.localPathButton.TabIndex = 5;
            this.localPathButton.Click += new System.EventHandler(this.LocalPathButton_Click);
            // 
            // localPathTextBox
            // 
            this.localPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localPathTextBox.Location = new System.Drawing.Point(6, 118);
            this.localPathTextBox.Name = "localPathTextBox";
            this.localPathTextBox.Size = new System.Drawing.Size(537, 20);
            this.localPathTextBox.TabIndex = 4;
            // 
            // localPathLabel
            // 
            this.localPathLabel.AutoSize = true;
            this.localPathLabel.Location = new System.Drawing.Point(6, 102);
            this.localPathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.localPathLabel.Name = "localPathLabel";
            this.localPathLabel.Size = new System.Drawing.Size(259, 13);
            this.localPathLabel.TabIndex = 3;
            this.localPathLabel.Text = "Path override for this computer. Usually leave blank.";
            // 
            // exportPathGroupBox
            // 
            this.exportPathGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathGroupBox.Controls.Add(this.exportPathLabel);
            this.exportPathGroupBox.Controls.Add(this.exportPathTextBox);
            this.exportPathGroupBox.Controls.Add(this.exportPathButton);
            this.exportPathGroupBox.Location = new System.Drawing.Point(12, 181);
            this.exportPathGroupBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.exportPathGroupBox.Name = "exportPathGroupBox";
            this.exportPathGroupBox.Size = new System.Drawing.Size(580, 130);
            this.exportPathGroupBox.TabIndex = 1;
            this.exportPathGroupBox.TabStop = false;
            this.exportPathGroupBox.Text = "Export Path (Optional)";
            // 
            // letterMergePathGroupBox
            // 
            this.letterMergePathGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergePathGroupBox.Controls.Add(this.letterMergePathLabel);
            this.letterMergePathGroupBox.Controls.Add(this.letterMergePathTextBox);
            this.letterMergePathGroupBox.Controls.Add(this.letterMergeButton);
            this.letterMergePathGroupBox.Location = new System.Drawing.Point(12, 320);
            this.letterMergePathGroupBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.letterMergePathGroupBox.Name = "letterMergePathGroupBox";
            this.letterMergePathGroupBox.Size = new System.Drawing.Size(580, 130);
            this.letterMergePathGroupBox.TabIndex = 2;
            this.letterMergePathGroupBox.TabStop = false;
            this.letterMergePathGroupBox.Text = "Letter Merge Path (Optional)";
            // 
            // FormPath
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(604, 511);
            this.Controls.Add(this.letterMergePathGroupBox);
            this.Controls.Add(this.exportPathGroupBox);
            this.Controls.Add(this.storageGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPath";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Paths";
            this.Load += new System.EventHandler(this.FormPath_Load);
            this.storageGroupBox.ResumeLayout(false);
            this.storageGroupBox.PerformLayout();
            this.exportPathGroupBox.ResumeLayout(false);
            this.exportPathGroupBox.PerformLayout();
            this.letterMergePathGroupBox.ResumeLayout(false);
            this.letterMergePathGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TextBox exportPathTextBox;
		private System.Windows.Forms.TextBox pathTextBox;
		private OpenDental.UI.Button exportPathButton;
		private OpenDental.UI.Button pathButton;
		private System.Windows.Forms.Label exportPathLabel;
		private System.Windows.Forms.Label pathLabel;
		private System.Windows.Forms.Label letterMergePathLabel;
		private OpenDental.UI.Button letterMergeButton;
		private System.Windows.Forms.TextBox letterMergePathTextBox;
		private System.Windows.Forms.Label localPathLabel;
		private System.Windows.Forms.TextBox localPathTextBox;
		private OpenDental.UI.Button localPathButton;
		private System.Windows.Forms.GroupBox storageGroupBox;
        private System.Windows.Forms.GroupBox exportPathGroupBox;
        private System.Windows.Forms.GroupBox letterMergePathGroupBox;
    }
}
