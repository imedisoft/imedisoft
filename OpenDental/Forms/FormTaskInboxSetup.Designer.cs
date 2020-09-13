namespace Imedisoft.Forms
{
	partial class FormTaskInboxSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskInboxSetup));
            this.label1 = new System.Windows.Forms.Label();
            this.usersGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.trunkListBox = new System.Windows.Forms.ListBox();
            this.setButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(600, 130);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // usersGrid
            // 
            this.usersGrid.Location = new System.Drawing.Point(12, 142);
            this.usersGrid.Name = "usersGrid";
            this.usersGrid.Size = new System.Drawing.Size(300, 280);
            this.usersGrid.TabIndex = 2;
            this.usersGrid.Title = null;
            this.usersGrid.TranslationName = "TableInboxAssignments";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(446, 464);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(532, 464);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // trunkListBox
            // 
            this.trunkListBox.FormattingEnabled = true;
            this.trunkListBox.IntegralHeight = false;
            this.trunkListBox.Location = new System.Drawing.Point(462, 142);
            this.trunkListBox.Name = "trunkListBox";
            this.trunkListBox.Size = new System.Drawing.Size(150, 280);
            this.trunkListBox.TabIndex = 4;
            this.trunkListBox.SelectedIndexChanged += new System.EventHandler(this.TrunkListBox_SelectedIndexChanged);
            // 
            // setButton
            // 
            this.setButton.Enabled = false;
            this.setButton.Image = global::Imedisoft.Properties.Resources.IconArrowLeft;
            this.setButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setButton.Location = new System.Drawing.Point(350, 260);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(80, 25);
            this.setButton.TabIndex = 3;
            this.setButton.Text = "Set";
            this.setButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // FormTaskInboxSetup
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(624, 501);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.trunkListBox);
            this.Controls.Add(this.usersGrid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTaskInboxSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task Inbox Setup";
            this.Load += new System.EventHandler(this.FormTaskInboxSetup_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.ODGrid usersGrid;
		private System.Windows.Forms.ListBox trunkListBox;
		private OpenDental.UI.Button setButton;
	}
}
