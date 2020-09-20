namespace Imedisoft.Forms
{
	partial class FormAutoNotePromptEdit
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
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.labelTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.labelLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.optionsTextBox = new System.Windows.Forms.TextBox();
            this.optionsLabel = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.downButton = new OpenDental.UI.Button();
            this.upButton = new OpenDental.UI.Button();
            this.autoNoteResponseButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(180, 38);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(200, 21);
            this.typeComboBox.TabIndex = 3;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);
            // 
            // labelTextBox
            // 
            this.labelTextBox.Location = new System.Drawing.Point(180, 65);
            this.labelTextBox.MaxLength = 255;
            this.labelTextBox.Multiline = true;
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(370, 80);
            this.labelTextBox.TabIndex = 5;
            this.labelTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LabelTextBox_KeyDown);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(180, 12);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(200, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(143, 41);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(31, 13);
            this.typeLabel.TabIndex = 2;
            this.typeLabel.Text = "Type";
            // 
            // labelLabel
            // 
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new System.Drawing.Point(110, 68);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(64, 13);
            this.labelLabel.TabIndex = 4;
            this.labelLabel.Text = "Prompt text";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(114, 15);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // optionsTextBox
            // 
            this.optionsTextBox.AcceptsReturn = true;
            this.optionsTextBox.HideSelection = false;
            this.optionsTextBox.Location = new System.Drawing.Point(180, 151);
            this.optionsTextBox.Multiline = true;
            this.optionsTextBox.Name = "optionsTextBox";
            this.optionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.optionsTextBox.Size = new System.Drawing.Size(370, 240);
            this.optionsTextBox.TabIndex = 7;
            // 
            // optionsLabel
            // 
            this.optionsLabel.Location = new System.Drawing.Point(12, 154);
            this.optionsLabel.Name = "optionsLabel";
            this.optionsLabel.Size = new System.Drawing.Size(162, 60);
            this.optionsLabel.TabIndex = 6;
            this.optionsLabel.Text = "Possible responses\r\n(one line per item)";
            this.optionsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(612, 424);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(526, 424);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 11;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // downButton
            // 
            this.downButton.Image = global::Imedisoft.Properties.Resources.IconArrowDown;
            this.downButton.Location = new System.Drawing.Point(556, 220);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(30, 25);
            this.downButton.TabIndex = 10;
            this.downButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // upButton
            // 
            this.upButton.Image = global::Imedisoft.Properties.Resources.IconArrowUp;
            this.upButton.Location = new System.Drawing.Point(556, 189);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(30, 25);
            this.upButton.TabIndex = 9;
            this.upButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // autoNoteResponseButton
            // 
            this.autoNoteResponseButton.Location = new System.Drawing.Point(556, 151);
            this.autoNoteResponseButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.autoNoteResponseButton.Name = "autoNoteResponseButton";
            this.autoNoteResponseButton.Size = new System.Drawing.Size(120, 25);
            this.autoNoteResponseButton.TabIndex = 8;
            this.autoNoteResponseButton.Text = "Auto Note Response";
            this.autoNoteResponseButton.Visible = false;
            this.autoNoteResponseButton.Click += new System.EventHandler(this.AutoNoteResponseButton_Click);
            // 
            // FormAutoNotePromptEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(704, 461);
            this.Controls.Add(this.autoNoteResponseButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.optionsLabel);
            this.Controls.Add(this.optionsTextBox);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.labelLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.labelTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNotePromptEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Note Prompt";
            this.Load += new System.EventHandler(this.FormAutoNotePromptEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox typeComboBox;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.TextBox labelTextBox;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.Label labelLabel;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox optionsTextBox;
		private System.Windows.Forms.Label optionsLabel;
		private OpenDental.UI.Button downButton;
		private OpenDental.UI.Button upButton;
		private OpenDental.UI.Button autoNoteResponseButton;
	}
}
