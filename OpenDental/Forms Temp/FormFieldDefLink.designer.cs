namespace Imedisoft.Forms
{
	partial class FormFieldDefLink
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
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.displayedGrid = new OpenDental.UI.ODGrid();
            this.hiddenGrid = new OpenDental.UI.ODGrid();
            this.rightButton = new OpenDental.UI.Button();
            this.leftButton = new OpenDental.UI.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.locationComboBox = new System.Windows.Forms.ComboBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(456, 484);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 8;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(542, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // displayedGrid
            // 
            this.displayedGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.displayedGrid.Location = new System.Drawing.Point(12, 89);
            this.displayedGrid.Name = "displayedGrid";
            this.displayedGrid.Size = new System.Drawing.Size(270, 382);
            this.displayedGrid.TabIndex = 4;
            this.displayedGrid.Title = "Visible Fields";
            this.displayedGrid.TranslationName = "TableVisible";
            // 
            // hiddenGrid
            // 
            this.hiddenGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hiddenGrid.Location = new System.Drawing.Point(352, 89);
            this.hiddenGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.hiddenGrid.Name = "hiddenGrid";
            this.hiddenGrid.Size = new System.Drawing.Size(270, 382);
            this.hiddenGrid.TabIndex = 7;
            this.hiddenGrid.Title = "Hidden Fields ";
            this.hiddenGrid.TranslationName = "TableHidden";
            // 
            // rightButton
            // 
            this.rightButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightButton.Image = global::Imedisoft.Properties.Resources.IconArrowRight;
            this.rightButton.Location = new System.Drawing.Point(302, 210);
            this.rightButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(30, 25);
            this.rightButton.TabIndex = 5;
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.leftButton.Image = global::Imedisoft.Properties.Resources.IconArrowLeft;
            this.leftButton.Location = new System.Drawing.Point(302, 248);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(30, 25);
            this.leftButton.TabIndex = 6;
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 9);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(610, 50);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "Select fields on the left and move them to the right in order to hide the selecte" +
    "d field from the specified location in the software.";
            // 
            // locationComboBox
            // 
            this.locationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locationComboBox.FormattingEnabled = true;
            this.locationComboBox.Location = new System.Drawing.Point(110, 62);
            this.locationComboBox.Name = "locationComboBox";
            this.locationComboBox.Size = new System.Drawing.Size(172, 21);
            this.locationComboBox.TabIndex = 3;
            this.locationComboBox.SelectionChangeCommitted += new System.EventHandler(this.LocationComboBox_SelectionChangeCommitted);
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(32, 65);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(72, 13);
            this.locationLabel.TabIndex = 2;
            this.locationLabel.Text = "Field Location";
            // 
            // FormFieldDefLink
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(634, 521);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.locationComboBox);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.leftButton);
            this.Controls.Add(this.rightButton);
            this.Controls.Add(this.hiddenGrid);
            this.Controls.Add(this.displayedGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFieldDefLink";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Field Display";
            this.Load += new System.EventHandler(this.FormFieldDefLink_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid displayedGrid;
		private OpenDental.UI.ODGrid hiddenGrid;
		private OpenDental.UI.Button rightButton;
		private OpenDental.UI.Button leftButton;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.ComboBox locationComboBox;
		private System.Windows.Forms.Label locationLabel;
	}
}