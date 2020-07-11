namespace Imedisoft.CEMT.Forms
{
	partial class FormCentralConnectionGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralConnectionGroups));
            this.gridMain = new OpenDental.UI.ODGrid();
            this.closeButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.connectionGroupComboBox = new System.Windows.Forms.ComboBox();
            this.connectionGroupLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gridMain
            // 
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMain.Location = new System.Drawing.Point(12, 41);
            this.gridMain.Name = "gridMain";
            this.gridMain.Size = new System.Drawing.Size(334, 368);
            this.gridMain.TabIndex = 0;
            this.gridMain.Title = "Groups";
            this.gridMain.TranslationName = "TableGroups";
            this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(352, 384);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(352, 41);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 218;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // connectionGroupComboBox
            // 
            this.connectionGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionGroupComboBox.FormattingEnabled = true;
            this.connectionGroupComboBox.Location = new System.Drawing.Point(12, 12);
            this.connectionGroupComboBox.MaxDropDownItems = 20;
            this.connectionGroupComboBox.Name = "connectionGroupComboBox";
            this.connectionGroupComboBox.Size = new System.Drawing.Size(169, 23);
            this.connectionGroupComboBox.TabIndex = 221;
            this.connectionGroupComboBox.SelectionChangeCommitted += new System.EventHandler(this.comboConnectionGroup_SelectionChangeCommitted);
            // 
            // connectionGroupLabel
            // 
            this.connectionGroupLabel.AutoSize = true;
            this.connectionGroupLabel.Location = new System.Drawing.Point(187, 15);
            this.connectionGroupLabel.Name = "connectionGroupLabel";
            this.connectionGroupLabel.Size = new System.Drawing.Size(139, 15);
            this.connectionGroupLabel.TabIndex = 220;
            this.connectionGroupLabel.Text = "Default Group on Startup";
            // 
            // FormCentralConnectionGroups
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(444, 421);
            this.Controls.Add(this.connectionGroupComboBox);
            this.Controls.Add(this.connectionGroupLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.gridMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(460, 460);
            this.Name = "FormCentralConnectionGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection Groups";
            this.Load += new System.EventHandler(this.FormCentralConnectionGroups_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.ComboBox connectionGroupComboBox;
		private System.Windows.Forms.Label connectionGroupLabel;
	}
}
