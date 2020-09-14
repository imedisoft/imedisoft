namespace Imedisoft.Forms
{
    partial class FormPerioEdit
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.examDateTextBox = new System.Windows.Forms.TextBox();
            this.examDateLabel = new System.Windows.Forms.Label();
            this.providerListBox = new System.Windows.Forms.ComboBox();
            this.providerLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(332, 114);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(246, 114);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // examDateTextBox
            // 
            this.examDateTextBox.Location = new System.Drawing.Point(120, 19);
            this.examDateTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.examDateTextBox.Name = "examDateTextBox";
            this.examDateTextBox.Size = new System.Drawing.Size(70, 20);
            this.examDateTextBox.TabIndex = 2;
            // 
            // examDateLabel
            // 
            this.examDateLabel.AutoSize = true;
            this.examDateLabel.Location = new System.Drawing.Point(51, 22);
            this.examDateLabel.Name = "examDateLabel";
            this.examDateLabel.Size = new System.Drawing.Size(63, 13);
            this.examDateLabel.TabIndex = 1;
            this.examDateLabel.Text = "Exam. Date";
            // 
            // providerListBox
            // 
            this.providerListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.providerListBox.Location = new System.Drawing.Point(120, 45);
            this.providerListBox.Name = "providerListBox";
            this.providerListBox.Size = new System.Drawing.Size(240, 21);
            this.providerListBox.TabIndex = 4;
            // 
            // providerLabel
            // 
            this.providerLabel.AutoSize = true;
            this.providerLabel.Location = new System.Drawing.Point(67, 48);
            this.providerLabel.Name = "providerLabel";
            this.providerLabel.Size = new System.Drawing.Size(47, 13);
            this.providerLabel.TabIndex = 3;
            this.providerLabel.Text = "Provider";
            // 
            // FormPerioEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 151);
            this.Controls.Add(this.providerLabel);
            this.Controls.Add(this.providerListBox);
            this.Controls.Add(this.examDateLabel);
            this.Controls.Add(this.examDateTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPerioEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Perio Exam";
            this.Load += new System.EventHandler(this.FormPerioEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		private System.Windows.Forms.TextBox examDateTextBox;
		private System.Windows.Forms.Label examDateLabel;
		private System.Windows.Forms.ComboBox providerListBox;
		private System.Windows.Forms.Label providerLabel;
	}
}
