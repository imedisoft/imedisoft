namespace Imedisoft.Forms
{
    partial class FormComputers
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

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormComputers));
            this.computersListBox = new System.Windows.Forms.ListBox();
            this.closeButton = new OpenDental.UI.Button();
            this.computersInfoLabel = new System.Windows.Forms.Label();
            this.deleteButton = new OpenDental.UI.Button();
            this.fixGraphicsButton = new OpenDental.UI.Button();
            this.graphicsLabel = new System.Windows.Forms.Label();
            this.graphicsGroupBox = new System.Windows.Forms.GroupBox();
            this.workstationTextBox = new System.Windows.Forms.TextBox();
            this.workstationLabel = new System.Windows.Forms.Label();
            this.workstationGroupBox = new System.Windows.Forms.GroupBox();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.versionTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.hostnameTextBox = new System.Windows.Forms.TextBox();
            this.commentLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.hostnameLabel = new System.Windows.Forms.Label();
            this.graphicsGroupBox.SuspendLayout();
            this.workstationGroupBox.SuspendLayout();
            this.databaseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // computersListBox
            // 
            this.computersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.computersListBox.IntegralHeight = false;
            this.computersListBox.Items.AddRange(new object[] {
            ""});
            this.computersListBox.Location = new System.Drawing.Point(6, 108);
            this.computersListBox.Name = "computersListBox";
            this.computersListBox.Size = new System.Drawing.Size(242, 248);
            this.computersListBox.TabIndex = 3;
            this.computersListBox.SelectedIndexChanged += new System.EventHandler(this.ComputersListBox_SelectedIndexChanged);
            this.computersListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ComputersListBox_MouseDoubleClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(392, 574);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // computersInfoLabel
            // 
            this.computersInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.computersInfoLabel.Location = new System.Drawing.Point(6, 61);
            this.computersInfoLabel.Name = "computersInfoLabel";
            this.computersInfoLabel.Size = new System.Drawing.Size(448, 44);
            this.computersInfoLabel.TabIndex = 2;
            this.computersInfoLabel.Text = "Computers are added to this list every time you use Imedisoft. You can safely del" +
    "ete unused computer names from this list to speed up messaging.";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(6, 362);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // fixGraphicsButton
            // 
            this.fixGraphicsButton.Location = new System.Drawing.Point(37, 176);
            this.fixGraphicsButton.Name = "fixGraphicsButton";
            this.fixGraphicsButton.Size = new System.Drawing.Size(120, 25);
            this.fixGraphicsButton.TabIndex = 1;
            this.fixGraphicsButton.Text = "Use Simple Graphics";
            this.fixGraphicsButton.Click += new System.EventHandler(this.FixGraphicsButton_Click);
            // 
            // graphicsLabel
            // 
            this.graphicsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphicsLabel.Location = new System.Drawing.Point(6, 23);
            this.graphicsLabel.Name = "graphicsLabel";
            this.graphicsLabel.Size = new System.Drawing.Size(188, 150);
            this.graphicsLabel.TabIndex = 0;
            this.graphicsLabel.Text = resources.GetString("graphicsLabel.Text");
            // 
            // graphicsGroupBox
            // 
            this.graphicsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.graphicsGroupBox.Controls.Add(this.fixGraphicsButton);
            this.graphicsGroupBox.Controls.Add(this.graphicsLabel);
            this.graphicsGroupBox.Location = new System.Drawing.Point(254, 108);
            this.graphicsGroupBox.Name = "graphicsGroupBox";
            this.graphicsGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.graphicsGroupBox.Size = new System.Drawing.Size(200, 220);
            this.graphicsGroupBox.TabIndex = 4;
            this.graphicsGroupBox.TabStop = false;
            this.graphicsGroupBox.Text = "Fix a Workstation";
            // 
            // workstationTextBox
            // 
            this.workstationTextBox.Enabled = false;
            this.workstationTextBox.Location = new System.Drawing.Point(120, 26);
            this.workstationTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.workstationTextBox.Name = "workstationTextBox";
            this.workstationTextBox.ReadOnly = true;
            this.workstationTextBox.Size = new System.Drawing.Size(280, 20);
            this.workstationTextBox.TabIndex = 1;
            // 
            // workstationLabel
            // 
            this.workstationLabel.AutoSize = true;
            this.workstationLabel.Location = new System.Drawing.Point(20, 29);
            this.workstationLabel.Name = "workstationLabel";
            this.workstationLabel.Size = new System.Drawing.Size(94, 13);
            this.workstationLabel.TabIndex = 0;
            this.workstationLabel.Text = "Current Computer";
            // 
            // workstationGroupBox
            // 
            this.workstationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workstationGroupBox.Controls.Add(this.graphicsGroupBox);
            this.workstationGroupBox.Controls.Add(this.workstationTextBox);
            this.workstationGroupBox.Controls.Add(this.computersListBox);
            this.workstationGroupBox.Controls.Add(this.deleteButton);
            this.workstationGroupBox.Controls.Add(this.workstationLabel);
            this.workstationGroupBox.Controls.Add(this.computersInfoLabel);
            this.workstationGroupBox.Location = new System.Drawing.Point(12, 168);
            this.workstationGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.workstationGroupBox.Name = "workstationGroupBox";
            this.workstationGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.workstationGroupBox.Size = new System.Drawing.Size(460, 393);
            this.workstationGroupBox.TabIndex = 1;
            this.workstationGroupBox.TabStop = false;
            this.workstationGroupBox.Text = "Workstation";
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseGroupBox.Controls.Add(this.commentTextBox);
            this.databaseGroupBox.Controls.Add(this.versionTextBox);
            this.databaseGroupBox.Controls.Add(this.nameTextBox);
            this.databaseGroupBox.Controls.Add(this.hostnameTextBox);
            this.databaseGroupBox.Controls.Add(this.commentLabel);
            this.databaseGroupBox.Controls.Add(this.versionLabel);
            this.databaseGroupBox.Controls.Add(this.nameLabel);
            this.databaseGroupBox.Controls.Add(this.hostnameLabel);
            this.databaseGroupBox.Location = new System.Drawing.Point(12, 12);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.databaseGroupBox.Size = new System.Drawing.Size(460, 150);
            this.databaseGroupBox.TabIndex = 0;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "Database Server";
            // 
            // commentTextBox
            // 
            this.commentTextBox.Enabled = false;
            this.commentTextBox.Location = new System.Drawing.Point(120, 104);
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.ReadOnly = true;
            this.commentTextBox.Size = new System.Drawing.Size(280, 20);
            this.commentTextBox.TabIndex = 7;
            // 
            // versionTextBox
            // 
            this.versionTextBox.Enabled = false;
            this.versionTextBox.Location = new System.Drawing.Point(120, 78);
            this.versionTextBox.Name = "versionTextBox";
            this.versionTextBox.ReadOnly = true;
            this.versionTextBox.Size = new System.Drawing.Size(280, 20);
            this.versionTextBox.TabIndex = 5;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Enabled = false;
            this.nameTextBox.Location = new System.Drawing.Point(120, 52);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(280, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // hostnameTextBox
            // 
            this.hostnameTextBox.Enabled = false;
            this.hostnameTextBox.Location = new System.Drawing.Point(120, 26);
            this.hostnameTextBox.Name = "hostnameTextBox";
            this.hostnameTextBox.ReadOnly = true;
            this.hostnameTextBox.Size = new System.Drawing.Size(280, 20);
            this.hostnameTextBox.TabIndex = 1;
            // 
            // commentLabel
            // 
            this.commentLabel.AutoSize = true;
            this.commentLabel.Location = new System.Drawing.Point(24, 107);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(90, 13);
            this.commentLabel.TabIndex = 6;
            this.commentLabel.Text = "Service Comment";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(34, 81);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(80, 13);
            this.versionLabel.TabIndex = 4;
            this.versionLabel.Text = "Service Version";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(42, 55);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(72, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Service Name";
            // 
            // hostnameLabel
            // 
            this.hostnameLabel.AutoSize = true;
            this.hostnameLabel.Location = new System.Drawing.Point(45, 29);
            this.hostnameLabel.Name = "hostnameLabel";
            this.hostnameLabel.Size = new System.Drawing.Size(69, 13);
            this.hostnameLabel.TabIndex = 0;
            this.hostnameLabel.Text = "Server Name";
            // 
            // FormComputers
            // 
            this.AcceptButton = this.closeButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(484, 611);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.workstationGroupBox);
            this.Controls.Add(this.databaseGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormComputers";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Computers";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormComputers_Closing);
            this.Load += new System.EventHandler(this.FormComputers_Load);
            this.graphicsGroupBox.ResumeLayout(false);
            this.workstationGroupBox.ResumeLayout(false);
            this.workstationGroupBox.PerformLayout();
            this.databaseGroupBox.ResumeLayout(false);
            this.databaseGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button closeButton;
		private System.Windows.Forms.Label computersInfoLabel;
		private System.Windows.Forms.ListBox computersListBox;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.UI.Button fixGraphicsButton;
		private System.Windows.Forms.Label graphicsLabel;
		private System.Windows.Forms.GroupBox graphicsGroupBox;
		private System.Windows.Forms.Label workstationLabel;
		private System.Windows.Forms.TextBox workstationTextBox;
		private System.Windows.Forms.GroupBox workstationGroupBox;
		private System.Windows.Forms.GroupBox databaseGroupBox;
		private System.Windows.Forms.TextBox commentTextBox;
		private System.Windows.Forms.TextBox versionTextBox;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.TextBox hostnameTextBox;
		private System.Windows.Forms.Label commentLabel;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label hostnameLabel;
	}
}
