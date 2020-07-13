namespace OpenDental.Forms
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
            this.fb = new System.Windows.Forms.FolderBrowserDialog();
            this.exportPathLabel = new System.Windows.Forms.Label();
            this.storageInfoLabel = new System.Windows.Forms.Label();
            this.letterMergePathLabel = new System.Windows.Forms.Label();
            this.letterMergeButton = new OpenDental.UI.Button();
            this.letterMergePathTextBox = new System.Windows.Forms.TextBox();
            this.storageGroupBox = new System.Windows.Forms.GroupBox();
            this.localPathButton = new OpenDental.UI.Button();
            this.localPathTextBox = new System.Windows.Forms.TextBox();
            this.labelLocalPath = new System.Windows.Forms.Label();
            this.storageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(476, 393);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 26);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(557, 393);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 26);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(6, 69);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(526, 20);
            this.pathTextBox.TabIndex = 1;
            // 
            // exportPathTextBox
            // 
            this.exportPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathTextBox.Location = new System.Drawing.Point(12, 238);
            this.exportPathTextBox.Name = "exportPathTextBox";
            this.exportPathTextBox.Size = new System.Drawing.Size(538, 20);
            this.exportPathTextBox.TabIndex = 1;
            // 
            // exportPathButton
            // 
            this.exportPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathButton.Location = new System.Drawing.Point(556, 235);
            this.exportPathButton.Name = "exportPathButton";
            this.exportPathButton.Size = new System.Drawing.Size(76, 25);
            this.exportPathButton.TabIndex = 91;
            this.exportPathButton.Text = "Browse";
            this.exportPathButton.Click += new System.EventHandler(this.ExportPathButton_Click);
            // 
            // pathButton
            // 
            this.pathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pathButton.Location = new System.Drawing.Point(538, 66);
            this.pathButton.Name = "pathButton";
            this.pathButton.Size = new System.Drawing.Size(76, 25);
            this.pathButton.TabIndex = 2;
            this.pathButton.Text = "&Browse";
            this.pathButton.Click += new System.EventHandler(this.PathButton_Click);
            // 
            // fb
            // 
            this.fb.SelectedPath = "C:\\";
            // 
            // exportPathLabel
            // 
            this.exportPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPathLabel.Location = new System.Drawing.Point(12, 185);
            this.exportPathLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.exportPathLabel.Name = "exportPathLabel";
            this.exportPathLabel.Size = new System.Drawing.Size(620, 50);
            this.exportPathLabel.TabIndex = 92;
            this.exportPathLabel.Text = resources.GetString("exportPathLabel.Text");
            // 
            // storageInfoLabel
            // 
            this.storageInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.storageInfoLabel.Location = new System.Drawing.Point(6, 26);
            this.storageInfoLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.storageInfoLabel.Name = "storageInfoLabel";
            this.storageInfoLabel.Size = new System.Drawing.Size(608, 40);
            this.storageInfoLabel.TabIndex = 93;
            this.storageInfoLabel.Text = resources.GetString("storageInfoLabel.Text");
            // 
            // letterMergePathLabel
            // 
            this.letterMergePathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergePathLabel.Location = new System.Drawing.Point(12, 281);
            this.letterMergePathLabel.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.letterMergePathLabel.Name = "letterMergePathLabel";
            this.letterMergePathLabel.Size = new System.Drawing.Size(620, 50);
            this.letterMergePathLabel.TabIndex = 96;
            this.letterMergePathLabel.Text = resources.GetString("letterMergePathLabel.Text");
            // 
            // letterMergeButton
            // 
            this.letterMergeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergeButton.Location = new System.Drawing.Point(556, 331);
            this.letterMergeButton.Name = "letterMergeButton";
            this.letterMergeButton.Size = new System.Drawing.Size(76, 25);
            this.letterMergeButton.TabIndex = 95;
            this.letterMergeButton.Text = "Browse";
            this.letterMergeButton.Click += new System.EventHandler(this.LetterMergeButton_Click);
            // 
            // letterMergePathTextBox
            // 
            this.letterMergePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.letterMergePathTextBox.Location = new System.Drawing.Point(12, 334);
            this.letterMergePathTextBox.Name = "letterMergePathTextBox";
            this.letterMergePathTextBox.Size = new System.Drawing.Size(538, 20);
            this.letterMergePathTextBox.TabIndex = 94;
            // 
            // storageGroupBox
            // 
            this.storageGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.storageGroupBox.Controls.Add(this.localPathButton);
            this.storageGroupBox.Controls.Add(this.storageInfoLabel);
            this.storageGroupBox.Controls.Add(this.pathButton);
            this.storageGroupBox.Controls.Add(this.localPathTextBox);
            this.storageGroupBox.Controls.Add(this.pathTextBox);
            this.storageGroupBox.Controls.Add(this.labelLocalPath);
            this.storageGroupBox.Location = new System.Drawing.Point(12, 12);
            this.storageGroupBox.Name = "storageGroupBox";
            this.storageGroupBox.Size = new System.Drawing.Size(620, 160);
            this.storageGroupBox.TabIndex = 0;
            this.storageGroupBox.TabStop = false;
            this.storageGroupBox.Text = "Folder for storing images and documents";
            // 
            // localPathButton
            // 
            this.localPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.localPathButton.Location = new System.Drawing.Point(538, 115);
            this.localPathButton.Name = "localPathButton";
            this.localPathButton.Size = new System.Drawing.Size(76, 25);
            this.localPathButton.TabIndex = 103;
            this.localPathButton.Text = "Browse";
            this.localPathButton.Click += new System.EventHandler(this.LocalPathButton_Click);
            // 
            // localPathTextBox
            // 
            this.localPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localPathTextBox.Location = new System.Drawing.Point(6, 118);
            this.localPathTextBox.Name = "localPathTextBox";
            this.localPathTextBox.Size = new System.Drawing.Size(526, 20);
            this.localPathTextBox.TabIndex = 102;
            // 
            // labelLocalPath
            // 
            this.labelLocalPath.AutoSize = true;
            this.labelLocalPath.Location = new System.Drawing.Point(6, 102);
            this.labelLocalPath.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelLocalPath.Name = "labelLocalPath";
            this.labelLocalPath.Size = new System.Drawing.Size(252, 13);
            this.labelLocalPath.TabIndex = 104;
            this.labelLocalPath.Text = "Path override for this computer. Usually leave blank.";
            // 
            // FormPath
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(644, 431);
            this.Controls.Add(this.storageGroupBox);
            this.Controls.Add(this.letterMergeButton);
            this.Controls.Add(this.exportPathButton);
            this.Controls.Add(this.letterMergePathLabel);
            this.Controls.Add(this.letterMergePathTextBox);
            this.Controls.Add(this.exportPathLabel);
            this.Controls.Add(this.exportPathTextBox);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TextBox exportPathTextBox;
		private System.Windows.Forms.TextBox pathTextBox;
		private OpenDental.UI.Button exportPathButton;
		private OpenDental.UI.Button pathButton;
		private System.Windows.Forms.Label exportPathLabel;
		private System.Windows.Forms.Label storageInfoLabel;
		private System.Windows.Forms.Label letterMergePathLabel;
		private OpenDental.UI.Button letterMergeButton;
		private System.Windows.Forms.TextBox letterMergePathTextBox;
		private System.Windows.Forms.FolderBrowserDialog fb;
		private System.Windows.Forms.Label labelLocalPath;
		private System.Windows.Forms.TextBox localPathTextBox;
		private OpenDental.UI.Button localPathButton;
		private System.Windows.Forms.GroupBox storageGroupBox;
	}
}
