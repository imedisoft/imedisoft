namespace Imedisoft.Forms
{
    partial class FormDefinitionEdit
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.valueLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.colorButton = new System.Windows.Forms.Button();
            this.labelColor = new System.Windows.Forms.Label();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new OpenDental.UI.Button();
            this.checkExcludeSend = new System.Windows.Forms.CheckBox();
            this.checkExcludeConfirm = new System.Windows.Forms.CheckBox();
            this.groupEConfirm = new System.Windows.Forms.GroupBox();
            this.selectButton = new OpenDental.UI.Button();
            this.clearButton = new OpenDental.UI.Button();
            this.groupBoxEReminders = new System.Windows.Forms.GroupBox();
            this.checkExcludeRemind = new System.Windows.Forms.CheckBox();
            this.noColorCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBoxEThanks = new System.Windows.Forms.GroupBox();
            this.checkExcludeThanks = new System.Windows.Forms.CheckBox();
            this.groupBoxArrivals = new System.Windows.Forms.GroupBox();
            this.checkExcludeArrivalSend = new System.Windows.Forms.CheckBox();
            this.checkExcludeArrivalResponse = new System.Windows.Forms.CheckBox();
            this.groupEConfirm.SuspendLayout();
            this.groupBoxEReminders.SuspendLayout();
            this.groupBoxEThanks.SuspendLayout();
            this.groupBoxArrivals.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 48);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(34, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(188, 48);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(33, 13);
            this.valueLabel.TabIndex = 1;
            this.valueLabel.Text = "Value";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(12, 64);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(178, 64);
            this.nameTextBox.TabIndex = 0;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(190, 64);
            this.valueTextBox.MaxLength = 256;
            this.valueTextBox.Multiline = true;
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(178, 64);
            this.valueTextBox.TabIndex = 1;
            // 
            // colorButton
            // 
            this.colorButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colorButton.Location = new System.Drawing.Point(371, 64);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(30, 20);
            this.colorButton.TabIndex = 2;
            this.colorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // labelColor
            // 
            this.labelColor.AutoSize = true;
            this.labelColor.Location = new System.Drawing.Point(371, 46);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(32, 13);
            this.labelColor.TabIndex = 5;
            this.labelColor.Text = "Color";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(264, 305);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(350, 305);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hiddenCheckBox.Location = new System.Drawing.Point(12, 12);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(65, 18);
            this.hiddenCheckBox.TabIndex = 3;
            this.hiddenCheckBox.Text = "Hidden";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrashRed;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 305);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // checkExcludeSend
            // 
            this.checkExcludeSend.AutoSize = true;
            this.checkExcludeSend.Location = new System.Drawing.Point(6, 19);
            this.checkExcludeSend.Name = "checkExcludeSend";
            this.checkExcludeSend.Size = new System.Drawing.Size(132, 17);
            this.checkExcludeSend.TabIndex = 7;
            this.checkExcludeSend.Text = "Exclude when sending";
            // 
            // checkExcludeConfirm
            // 
            this.checkExcludeConfirm.AutoSize = true;
            this.checkExcludeConfirm.Location = new System.Drawing.Point(6, 40);
            this.checkExcludeConfirm.Name = "checkExcludeConfirm";
            this.checkExcludeConfirm.Size = new System.Drawing.Size(144, 17);
            this.checkExcludeConfirm.TabIndex = 8;
            this.checkExcludeConfirm.Text = "Exclude when confirming";
            // 
            // groupEConfirm
            // 
            this.groupEConfirm.Controls.Add(this.checkExcludeSend);
            this.groupEConfirm.Controls.Add(this.checkExcludeConfirm);
            this.groupEConfirm.Location = new System.Drawing.Point(12, 130);
            this.groupEConfirm.Name = "groupEConfirm";
            this.groupEConfirm.Size = new System.Drawing.Size(177, 64);
            this.groupEConfirm.TabIndex = 9;
            this.groupEConfirm.TabStop = false;
            this.groupEConfirm.Text = "eConfirmations";
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(371, 106);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(25, 20);
            this.selectButton.TabIndex = 200;
            this.selectButton.Text = "...";
            this.selectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.clearButton.Location = new System.Drawing.Point(402, 106);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(25, 20);
            this.clearButton.TabIndex = 201;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // groupBoxEReminders
            // 
            this.groupBoxEReminders.Controls.Add(this.checkExcludeRemind);
            this.groupBoxEReminders.Location = new System.Drawing.Point(12, 197);
            this.groupBoxEReminders.Name = "groupBoxEReminders";
            this.groupBoxEReminders.Size = new System.Drawing.Size(177, 46);
            this.groupBoxEReminders.TabIndex = 202;
            this.groupBoxEReminders.TabStop = false;
            this.groupBoxEReminders.Text = "eReminders";
            // 
            // checkExcludeRemind
            // 
            this.checkExcludeRemind.AutoSize = true;
            this.checkExcludeRemind.Location = new System.Drawing.Point(6, 19);
            this.checkExcludeRemind.Name = "checkExcludeRemind";
            this.checkExcludeRemind.Size = new System.Drawing.Size(132, 17);
            this.checkExcludeRemind.TabIndex = 7;
            this.checkExcludeRemind.Text = "Exclude when sending";
            // 
            // noColorCheckBox
            // 
            this.noColorCheckBox.AutoSize = true;
            this.noColorCheckBox.Location = new System.Drawing.Point(363, 12);
            this.noColorCheckBox.Name = "noColorCheckBox";
            this.noColorCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.noColorCheckBox.Size = new System.Drawing.Size(67, 17);
            this.noColorCheckBox.TabIndex = 203;
            this.noColorCheckBox.Text = "No Color";
            this.noColorCheckBox.Visible = false;
            this.noColorCheckBox.CheckedChanged += new System.EventHandler(this.NoColorCheckBox_CheckedChanged);
            // 
            // groupBoxEThanks
            // 
            this.groupBoxEThanks.Controls.Add(this.checkExcludeThanks);
            this.groupBoxEThanks.Location = new System.Drawing.Point(191, 197);
            this.groupBoxEThanks.Name = "groupBoxEThanks";
            this.groupBoxEThanks.Size = new System.Drawing.Size(177, 46);
            this.groupBoxEThanks.TabIndex = 203;
            this.groupBoxEThanks.TabStop = false;
            this.groupBoxEThanks.Text = "Automated Thank-You";
            // 
            // checkExcludeThanks
            // 
            this.checkExcludeThanks.AutoSize = true;
            this.checkExcludeThanks.Location = new System.Drawing.Point(6, 19);
            this.checkExcludeThanks.Name = "checkExcludeThanks";
            this.checkExcludeThanks.Size = new System.Drawing.Size(132, 17);
            this.checkExcludeThanks.TabIndex = 7;
            this.checkExcludeThanks.Text = "Exclude when sending";
            // 
            // groupBoxArrivals
            // 
            this.groupBoxArrivals.Controls.Add(this.checkExcludeArrivalSend);
            this.groupBoxArrivals.Controls.Add(this.checkExcludeArrivalResponse);
            this.groupBoxArrivals.Location = new System.Drawing.Point(191, 130);
            this.groupBoxArrivals.Name = "groupBoxArrivals";
            this.groupBoxArrivals.Size = new System.Drawing.Size(177, 64);
            this.groupBoxArrivals.TabIndex = 204;
            this.groupBoxArrivals.TabStop = false;
            this.groupBoxArrivals.Text = "Arrivals";
            // 
            // checkExcludeArrivalSend
            // 
            this.checkExcludeArrivalSend.AutoSize = true;
            this.checkExcludeArrivalSend.Location = new System.Drawing.Point(6, 19);
            this.checkExcludeArrivalSend.Name = "checkExcludeArrivalSend";
            this.checkExcludeArrivalSend.Size = new System.Drawing.Size(132, 17);
            this.checkExcludeArrivalSend.TabIndex = 7;
            this.checkExcludeArrivalSend.Text = "Exclude when sending";
            // 
            // checkExcludeArrivalResponse
            // 
            this.checkExcludeArrivalResponse.AutoSize = true;
            this.checkExcludeArrivalResponse.Location = new System.Drawing.Point(6, 40);
            this.checkExcludeArrivalResponse.Name = "checkExcludeArrivalResponse";
            this.checkExcludeArrivalResponse.Size = new System.Drawing.Size(148, 17);
            this.checkExcludeArrivalResponse.TabIndex = 8;
            this.checkExcludeArrivalResponse.Text = "Exclude when responding";
            // 
            // FormDefEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(442, 342);
            this.Controls.Add(this.groupBoxArrivals);
            this.Controls.Add(this.groupBoxEThanks);
            this.Controls.Add(this.noColorCheckBox);
            this.Controls.Add(this.groupBoxEReminders);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.groupEConfirm);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.labelColor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(439, 322);
            this.Name = "FormDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Definition";
            this.Load += new System.EventHandler(this.FormDefEdit_Load);
            this.groupEConfirm.ResumeLayout(false);
            this.groupEConfirm.PerformLayout();
            this.groupBoxEReminders.ResumeLayout(false);
            this.groupBoxEReminders.PerformLayout();
            this.groupBoxEThanks.ResumeLayout(false);
            this.groupBoxEThanks.PerformLayout();
            this.groupBoxArrivals.ResumeLayout(false);
            this.groupBoxArrivals.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.TextBox valueTextBox;
		private System.Windows.Forms.Button colorButton;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button selectButton;
		private System.Windows.Forms.Label labelColor;
		private System.Windows.Forms.CheckBox hiddenCheckBox;
		private OpenDental.UI.Button deleteButton;
		private System.Windows.Forms.CheckBox checkExcludeSend;
		private System.Windows.Forms.CheckBox checkExcludeConfirm;
		private System.Windows.Forms.GroupBox groupEConfirm;
		private System.Windows.Forms.GroupBox groupBoxEReminders;
		private System.Windows.Forms.CheckBox checkExcludeRemind;
		private System.Windows.Forms.CheckBox noColorCheckBox;
		private System.Windows.Forms.GroupBox groupBoxEThanks;
		private System.Windows.Forms.CheckBox checkExcludeThanks;
		private System.Windows.Forms.GroupBox groupBoxArrivals;
		private System.Windows.Forms.CheckBox checkExcludeArrivalSend;
		private System.Windows.Forms.CheckBox checkExcludeArrivalResponse;
		private OpenDental.UI.Button clearButton;
	}
}
