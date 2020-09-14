namespace Imedisoft.Forms
{
	partial class FormTimePick
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
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.pmRadioButton = new System.Windows.Forms.RadioButton();
            this.amRadioButton = new System.Windows.Forms.RadioButton();
            this.seperatorLabel = new System.Windows.Forms.Label();
            this.minuteComboBox = new System.Windows.Forms.ComboBox();
            this.hourComboBox = new System.Windows.Forms.ComboBox();
            this.dateGroupBox = new System.Windows.Forms.GroupBox();
            this.timeGroupBox = new System.Windows.Forms.GroupBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.dateGroupBox.SuspendLayout();
            this.timeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker.Location = new System.Drawing.Point(6, 19);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(188, 20);
            this.dateTimePicker.TabIndex = 0;
            // 
            // pmRadioButton
            // 
            this.pmRadioButton.AutoSize = true;
            this.pmRadioButton.Location = new System.Drawing.Point(139, 32);
            this.pmRadioButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pmRadioButton.Name = "pmRadioButton";
            this.pmRadioButton.Size = new System.Drawing.Size(39, 17);
            this.pmRadioButton.TabIndex = 4;
            this.pmRadioButton.Text = "PM";
            this.pmRadioButton.UseVisualStyleBackColor = true;
            // 
            // amRadioButton
            // 
            this.amRadioButton.AutoSize = true;
            this.amRadioButton.Checked = true;
            this.amRadioButton.Location = new System.Drawing.Point(139, 13);
            this.amRadioButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.amRadioButton.Name = "amRadioButton";
            this.amRadioButton.Size = new System.Drawing.Size(40, 17);
            this.amRadioButton.TabIndex = 3;
            this.amRadioButton.TabStop = true;
            this.amRadioButton.Text = "AM";
            this.amRadioButton.UseVisualStyleBackColor = true;
            // 
            // seperatorLabel
            // 
            this.seperatorLabel.AutoSize = true;
            this.seperatorLabel.Location = new System.Drawing.Point(59, 26);
            this.seperatorLabel.Margin = new System.Windows.Forms.Padding(0);
            this.seperatorLabel.Name = "seperatorLabel";
            this.seperatorLabel.Size = new System.Drawing.Size(11, 13);
            this.seperatorLabel.TabIndex = 1;
            this.seperatorLabel.Text = ":";
            // 
            // minuteComboBox
            // 
            this.minuteComboBox.FormattingEnabled = true;
            this.minuteComboBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.minuteComboBox.Location = new System.Drawing.Point(70, 23);
            this.minuteComboBox.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.minuteComboBox.MaxDropDownItems = 48;
            this.minuteComboBox.MaxLength = 2;
            this.minuteComboBox.Name = "minuteComboBox";
            this.minuteComboBox.Size = new System.Drawing.Size(50, 21);
            this.minuteComboBox.TabIndex = 2;
            // 
            // hourComboBox
            // 
            this.hourComboBox.FormattingEnabled = true;
            this.hourComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.hourComboBox.Location = new System.Drawing.Point(6, 23);
            this.hourComboBox.MaxDropDownItems = 48;
            this.hourComboBox.MaxLength = 2;
            this.hourComboBox.Name = "hourComboBox";
            this.hourComboBox.Size = new System.Drawing.Size(50, 21);
            this.hourComboBox.TabIndex = 0;
            this.hourComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TimeComboBox_KeyPress);
            // 
            // dateGroupBox
            // 
            this.dateGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateGroupBox.Controls.Add(this.dateTimePicker);
            this.dateGroupBox.Location = new System.Drawing.Point(12, 12);
            this.dateGroupBox.Name = "dateGroupBox";
            this.dateGroupBox.Size = new System.Drawing.Size(200, 47);
            this.dateGroupBox.TabIndex = 1;
            this.dateGroupBox.TabStop = false;
            this.dateGroupBox.Text = "Pick Date";
            // 
            // timeGroupBox
            // 
            this.timeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeGroupBox.Controls.Add(this.hourComboBox);
            this.timeGroupBox.Controls.Add(this.pmRadioButton);
            this.timeGroupBox.Controls.Add(this.minuteComboBox);
            this.timeGroupBox.Controls.Add(this.seperatorLabel);
            this.timeGroupBox.Controls.Add(this.amRadioButton);
            this.timeGroupBox.Location = new System.Drawing.Point(12, 65);
            this.timeGroupBox.Name = "timeGroupBox";
            this.timeGroupBox.Size = new System.Drawing.Size(200, 56);
            this.timeGroupBox.TabIndex = 2;
            this.timeGroupBox.TabStop = false;
            this.timeGroupBox.Text = "Pick Time";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(46, 144);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(132, 144);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // FormTimePick
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(224, 181);
            this.Controls.Add(this.timeGroupBox);
            this.Controls.Add(this.dateGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTimePick";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pick Time";
            this.Load += new System.EventHandler(this.FormTimePick_Load);
            this.dateGroupBox.ResumeLayout(false);
            this.timeGroupBox.ResumeLayout(false);
            this.timeGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.ComboBox hourComboBox;
		private System.Windows.Forms.ComboBox minuteComboBox;
		private System.Windows.Forms.Label seperatorLabel;
		private System.Windows.Forms.RadioButton amRadioButton;
		private System.Windows.Forms.RadioButton pmRadioButton;
		private System.Windows.Forms.DateTimePicker dateTimePicker;
		private System.Windows.Forms.GroupBox dateGroupBox;
		private System.Windows.Forms.GroupBox timeGroupBox;
	}
}
