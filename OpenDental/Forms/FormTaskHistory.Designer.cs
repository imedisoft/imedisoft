namespace Imedisoft.Forms
{
	partial class FormTaskHistory
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
            this.closeButton = new OpenDental.UI.Button();
            this.taskHistoryGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(712, 384);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // taskHistoryGrid
            // 
            this.taskHistoryGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskHistoryGrid.Location = new System.Drawing.Point(12, 12);
            this.taskHistoryGrid.Name = "taskHistoryGrid";
            this.taskHistoryGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.taskHistoryGrid.Size = new System.Drawing.Size(780, 366);
            this.taskHistoryGrid.TabIndex = 1;
            this.taskHistoryGrid.Title = "History";
            this.taskHistoryGrid.TranslationName = "TableHist";
            // 
            // FormTaskHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(804, 421);
            this.Controls.Add(this.taskHistoryGrid);
            this.Controls.Add(this.closeButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "FormTaskHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task History";
            this.Load += new System.EventHandler(this.FormTaskHistory_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.ODGrid taskHistoryGrid;
	}
}
