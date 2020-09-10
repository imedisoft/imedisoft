namespace Imedisoft.Forms
{
	partial class FormAllergyEdit
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
            this.isActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.reactionTextBox = new System.Windows.Forms.TextBox();
            this.reactionLabel = new System.Windows.Forms.Label();
            this.allergyDefLabel = new System.Windows.Forms.Label();
            this.allergyDefComboBox = new System.Windows.Forms.ComboBox();
            this.adverseReactionDateTextBox = new System.Windows.Forms.TextBox();
            this.adverseReactionDateLabel = new System.Windows.Forms.Label();
            this.reactionSnomedCodeLabel = new System.Windows.Forms.Label();
            this.reactionSnomedCodeTextBox = new System.Windows.Forms.TextBox();
            this.reactionSnomedCodeNoneButton = new OpenDental.UI.Button();
            this.reactionSnomedCodeSelectButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // isActiveCheckBox
            // 
            this.isActiveCheckBox.AutoSize = true;
            this.isActiveCheckBox.Checked = true;
            this.isActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isActiveCheckBox.Location = new System.Drawing.Point(150, 163);
            this.isActiveCheckBox.Name = "isActiveCheckBox";
            this.isActiveCheckBox.Size = new System.Drawing.Size(68, 17);
            this.isActiveCheckBox.TabIndex = 12;
            this.isActiveCheckBox.Text = "Is Active";
            this.isActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // reactionTextBox
            // 
            this.reactionTextBox.Location = new System.Drawing.Point(150, 71);
            this.reactionTextBox.Multiline = true;
            this.reactionTextBox.Name = "reactionTextBox";
            this.reactionTextBox.Size = new System.Drawing.Size(270, 60);
            this.reactionTextBox.TabIndex = 9;
            // 
            // reactionLabel
            // 
            this.reactionLabel.AutoSize = true;
            this.reactionLabel.Location = new System.Drawing.Point(39, 74);
            this.reactionLabel.Name = "reactionLabel";
            this.reactionLabel.Size = new System.Drawing.Size(105, 13);
            this.reactionLabel.TabIndex = 8;
            this.reactionLabel.Text = "Reaction Description";
            // 
            // allergyDefLabel
            // 
            this.allergyDefLabel.AutoSize = true;
            this.allergyDefLabel.Location = new System.Drawing.Point(104, 22);
            this.allergyDefLabel.Name = "allergyDefLabel";
            this.allergyDefLabel.Size = new System.Drawing.Size(40, 13);
            this.allergyDefLabel.TabIndex = 2;
            this.allergyDefLabel.Text = "Allergy";
            // 
            // allergyDefComboBox
            // 
            this.allergyDefComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.allergyDefComboBox.FormattingEnabled = true;
            this.allergyDefComboBox.Location = new System.Drawing.Point(150, 19);
            this.allergyDefComboBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.allergyDefComboBox.Name = "allergyDefComboBox";
            this.allergyDefComboBox.Size = new System.Drawing.Size(270, 21);
            this.allergyDefComboBox.TabIndex = 3;
            // 
            // adverseReactionDateTextBox
            // 
            this.adverseReactionDateTextBox.Location = new System.Drawing.Point(150, 137);
            this.adverseReactionDateTextBox.Name = "adverseReactionDateTextBox";
            this.adverseReactionDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.adverseReactionDateTextBox.TabIndex = 11;
            // 
            // adverseReactionDateLabel
            // 
            this.adverseReactionDateLabel.AutoSize = true;
            this.adverseReactionDateLabel.Location = new System.Drawing.Point(26, 140);
            this.adverseReactionDateLabel.Name = "adverseReactionDateLabel";
            this.adverseReactionDateLabel.Size = new System.Drawing.Size(118, 13);
            this.adverseReactionDateLabel.TabIndex = 10;
            this.adverseReactionDateLabel.Text = "Adverse Reaction Date";
            // 
            // reactionSnomedCodeLabel
            // 
            this.reactionSnomedCodeLabel.AutoSize = true;
            this.reactionSnomedCodeLabel.Location = new System.Drawing.Point(34, 49);
            this.reactionSnomedCodeLabel.Name = "reactionSnomedCodeLabel";
            this.reactionSnomedCodeLabel.Size = new System.Drawing.Size(110, 13);
            this.reactionSnomedCodeLabel.TabIndex = 4;
            this.reactionSnomedCodeLabel.Text = "SNOMED CT Reaction";
            // 
            // reactionSnomedCodeTextBox
            // 
            this.reactionSnomedCodeTextBox.Location = new System.Drawing.Point(150, 46);
            this.reactionSnomedCodeTextBox.Name = "reactionSnomedCodeTextBox";
            this.reactionSnomedCodeTextBox.ReadOnly = true;
            this.reactionSnomedCodeTextBox.Size = new System.Drawing.Size(270, 20);
            this.reactionSnomedCodeTextBox.TabIndex = 5;
            // 
            // reactionSnomedCodeNoneButton
            // 
            this.reactionSnomedCodeNoneButton.Location = new System.Drawing.Point(457, 46);
            this.reactionSnomedCodeNoneButton.Name = "reactionSnomedCodeNoneButton";
            this.reactionSnomedCodeNoneButton.Size = new System.Drawing.Size(60, 20);
            this.reactionSnomedCodeNoneButton.TabIndex = 7;
            this.reactionSnomedCodeNoneButton.Text = "None";
            this.reactionSnomedCodeNoneButton.Click += new System.EventHandler(this.ReactionSnomedCodeNoneButton_Click);
            // 
            // reactionSnomedCodeSelectButton
            // 
            this.reactionSnomedCodeSelectButton.Location = new System.Drawing.Point(426, 46);
            this.reactionSnomedCodeSelectButton.Name = "reactionSnomedCodeSelectButton";
            this.reactionSnomedCodeSelectButton.Size = new System.Drawing.Size(25, 20);
            this.reactionSnomedCodeSelectButton.TabIndex = 6;
            this.reactionSnomedCodeSelectButton.Text = "...";
            this.reactionSnomedCodeSelectButton.Click += new System.EventHandler(this.ReactionSnomedCodeSelectButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(442, 214);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(356, 214);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 214);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 13;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAllergyEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(534, 251);
            this.Controls.Add(this.reactionSnomedCodeNoneButton);
            this.Controls.Add(this.reactionSnomedCodeSelectButton);
            this.Controls.Add(this.reactionSnomedCodeLabel);
            this.Controls.Add(this.reactionSnomedCodeTextBox);
            this.Controls.Add(this.adverseReactionDateLabel);
            this.Controls.Add(this.adverseReactionDateTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.allergyDefComboBox);
            this.Controls.Add(this.allergyDefLabel);
            this.Controls.Add(this.isActiveCheckBox);
            this.Controls.Add(this.reactionTextBox);
            this.Controls.Add(this.reactionLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.deleteButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAllergyEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allergy";
            this.Load += new System.EventHandler(this.FormAllergyEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button deleteButton;
		private System.Windows.Forms.CheckBox isActiveCheckBox;
		private System.Windows.Forms.TextBox reactionTextBox;
		private System.Windows.Forms.Label reactionLabel;
		private System.Windows.Forms.Label allergyDefLabel;
		private System.Windows.Forms.ComboBox allergyDefComboBox;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TextBox adverseReactionDateTextBox;
		private System.Windows.Forms.Label adverseReactionDateLabel;
		private OpenDental.UI.Button reactionSnomedCodeNoneButton;
		private OpenDental.UI.Button reactionSnomedCodeSelectButton;
		private System.Windows.Forms.Label reactionSnomedCodeLabel;
		private System.Windows.Forms.TextBox reactionSnomedCodeTextBox;
	}
}
