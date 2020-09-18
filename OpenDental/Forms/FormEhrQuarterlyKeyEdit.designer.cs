namespace Imedisoft.Forms
{
	partial class FormEhrQuarterlyKeyEdit
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
            this.yearLabel = new System.Windows.Forms.Label();
            this.quarterLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.yearTextBox = new System.Windows.Forms.TextBox();
            this.quarterTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(236, 125);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 24);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(317, 125);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 24);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            // 
            // yearLabel
            // 
            this.yearLabel.AutoSize = true;
            this.yearLabel.Location = new System.Drawing.Point(55, 22);
            this.yearLabel.Name = "yearLabel";
            this.yearLabel.Size = new System.Drawing.Size(29, 13);
            this.yearLabel.TabIndex = 0;
            this.yearLabel.Text = "Year";
            // 
            // quarterLabel
            // 
            this.quarterLabel.AutoSize = true;
            this.quarterLabel.Location = new System.Drawing.Point(39, 48);
            this.quarterLabel.Name = "quarterLabel";
            this.quarterLabel.Size = new System.Drawing.Size(45, 13);
            this.quarterLabel.TabIndex = 2;
            this.quarterLabel.Text = "Quarter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Key";
            // 
            // keyTextBox
            // 
            this.keyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyTextBox.Location = new System.Drawing.Point(90, 71);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(302, 20);
            this.keyTextBox.TabIndex = 5;
            // 
            // yearTextBox
            // 
            this.yearTextBox.Location = new System.Drawing.Point(90, 19);
            this.yearTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.yearTextBox.MaxLength = 4;
            this.yearTextBox.Name = "yearTextBox";
            this.yearTextBox.Size = new System.Drawing.Size(50, 20);
            this.yearTextBox.TabIndex = 1;
            this.yearTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.YearTextBox_KeyPress);
            this.yearTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.YearTextBox_Validating);
            // 
            // quarterTextBox
            // 
            this.quarterTextBox.Location = new System.Drawing.Point(90, 45);
            this.quarterTextBox.MaxLength = 1;
            this.quarterTextBox.Name = "quarterTextBox";
            this.quarterTextBox.Size = new System.Drawing.Size(30, 20);
            this.quarterTextBox.TabIndex = 3;
            this.quarterTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuarterTextBox_KeyPress);
            // 
            // FormEhrQuarterlyKeyEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 161);
            this.Controls.Add(this.quarterTextBox);
            this.Controls.Add(this.yearTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.keyTextBox);
            this.Controls.Add(this.quarterLabel);
            this.Controls.Add(this.yearLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrQuarterlyKeyEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EHR Quarterly Key";
            this.Load += new System.EventHandler(this.FormEhrQuarterlyKeyEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label yearLabel;
		private System.Windows.Forms.Label quarterLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox keyTextBox;
		private System.Windows.Forms.TextBox yearTextBox;
		private System.Windows.Forms.TextBox quarterTextBox;
	}
}
