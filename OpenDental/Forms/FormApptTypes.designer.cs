namespace Imedisoft.Forms
{
	partial class FormApptTypes
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
            this.addButton = new OpenDental.UI.Button();
            this.closeButton = new OpenDental.UI.Button();
            this.appointmentTypesGrid = new OpenDental.UI.ODGrid();
            this.downButton = new OpenDental.UI.Button();
            this.upButton = new OpenDental.UI.Button();
            this.promptCheckBox = new System.Windows.Forms.CheckBox();
            this.warnCheckBox = new System.Windows.Forms.CheckBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(352, 65);
            this.addButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(352, 504);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // appointmentTypesGrid
            // 
            this.appointmentTypesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentTypesGrid.Location = new System.Drawing.Point(12, 65);
            this.appointmentTypesGrid.Name = "appointmentTypesGrid";
            this.appointmentTypesGrid.Size = new System.Drawing.Size(334, 464);
            this.appointmentTypesGrid.TabIndex = 0;
            this.appointmentTypesGrid.Title = "Appointment Types";
            this.appointmentTypesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.AppointmentTypesGrid_CellDoubleClick);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = global::Imedisoft.Properties.Resources.IconArrowDown;
            this.downButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downButton.Location = new System.Drawing.Point(352, 153);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(80, 25);
            this.downButton.TabIndex = 3;
            this.downButton.Text = "&Down";
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = global::Imedisoft.Properties.Resources.IconArrowUp;
            this.upButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upButton.Location = new System.Drawing.Point(352, 123);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(80, 25);
            this.upButton.TabIndex = 2;
            this.upButton.Text = "&Up";
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // promptCheckBox
            // 
            this.promptCheckBox.AutoSize = true;
            this.promptCheckBox.Location = new System.Drawing.Point(12, 12);
            this.promptCheckBox.Name = "promptCheckBox";
            this.promptCheckBox.Size = new System.Drawing.Size(257, 17);
            this.promptCheckBox.TabIndex = 6;
            this.promptCheckBox.Text = "New appointments prompt for appointment type";
            this.promptCheckBox.UseVisualStyleBackColor = true;
            // 
            // warnCheckBox
            // 
            this.warnCheckBox.AutoSize = true;
            this.warnCheckBox.Location = new System.Drawing.Point(12, 35);
            this.warnCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.warnCheckBox.Name = "warnCheckBox";
            this.warnCheckBox.Size = new System.Drawing.Size(345, 17);
            this.warnCheckBox.TabIndex = 7;
            this.warnCheckBox.Text = "Warn users before disassociating procedures from an appointment";
            this.warnCheckBox.UseVisualStyleBackColor = true;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(355, 473);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormApptTypes
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(444, 541);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.warnCheckBox);
            this.Controls.Add(this.promptCheckBox);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.appointmentTypesGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptTypes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Types";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptTypes_FormClosing);
            this.Load += new System.EventHandler(this.FormApptTypes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button addButton;
		private OpenDental.UI.ODGrid appointmentTypesGrid;
		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.Button downButton;
		private OpenDental.UI.Button upButton;
		private System.Windows.Forms.CheckBox promptCheckBox;
		private System.Windows.Forms.CheckBox warnCheckBox;
		private OpenDental.UI.Button acceptButton;
	}
}
