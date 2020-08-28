namespace Imedisoft.UI
{
    partial class InputBox
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
            this.components = new System.ComponentModel.Container();
            this.labelPrompt = new System.Windows.Forms.Label();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // labelPrompt
            // 
            this.labelPrompt.Location = new System.Drawing.Point(31, 39);
            this.labelPrompt.Name = "labelPrompt";
            this.labelPrompt.Size = new System.Drawing.Size(387, 41);
            this.labelPrompt.TabIndex = 2;
            this.labelPrompt.Text = "Input controls can be added dynamically to this form. Types that can be added are" +
    " TextBoxes, ComboBoxes (mult and single), ValidDates,ValidTimes, ValidPhones, an" +
    "d Checkboxes.";
            this.labelPrompt.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labelPrompt.Visible = false;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(275, 137);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 1000;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.butOK_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(361, 137);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 1001;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // _timer
            // 
            this._timer.Interval = 15000;
            this._timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(12, 137);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 1002;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Visible = false;
            this.deleteButton.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // InputBox
            // 
            this.AcceptButton = this.acceptButton;
            this.ClientSize = new System.Drawing.Size(453, 174);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.labelPrompt);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.InputBox_Shown);
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button acceptButton;
		public System.Windows.Forms.Label labelPrompt;
		private System.Windows.Forms.Timer _timer;
		private OpenDental.UI.Button deleteButton;
	}
}
