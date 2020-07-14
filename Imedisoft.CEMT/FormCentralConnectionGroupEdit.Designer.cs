namespace Imedisoft.CEMT.Forms
{
	partial class FormCentralConnectionGroupEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralConnectionGroupEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.butOK = new OpenDental.UI.Button();
            this.gridMain = new OpenDental.UI.ODGrid();
            this.butDelete = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.removeButton = new OpenDental.UI.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gridAvail = new OpenDental.UI.ODGrid();
            this.butCancel = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group Description";
            // 
            // textDescription
            // 
            this.textDescription.Location = new System.Drawing.Point(120, 12);
            this.textDescription.Name = "textDescription";
            this.textDescription.Size = new System.Drawing.Size(237, 23);
            this.textDescription.TabIndex = 1;
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(906, 529);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(80, 25);
            this.butOK.TabIndex = 6;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // gridMain
            // 
            this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridMain.Location = new System.Drawing.Point(12, 41);
            this.gridMain.Name = "gridMain";
            this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.gridMain.Size = new System.Drawing.Size(480, 482);
            this.gridMain.TabIndex = 2;
            this.gridMain.Title = "Connections for this Group";
            this.gridMain.TranslationName = "TableConnections";
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Image = ((System.Drawing.Image)(resources.GetObject("butDelete.Image")));
            this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butDelete.Location = new System.Drawing.Point(12, 529);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(80, 25);
            this.butDelete.TabIndex = 8;
            this.butDelete.Text = "Delete";
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // addButton
            // 
            this.addButton.Image = ((System.Drawing.Image)(resources.GetObject("addButton.Image")));
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(498, 214);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(90, 30);
            this.addButton.TabIndex = 219;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Image = ((System.Drawing.Image)(resources.GetObject("removeButton.Image")));
            this.removeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeButton.Location = new System.Drawing.Point(498, 250);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(90, 30);
            this.removeButton.TabIndex = 220;
            this.removeButton.Text = "Remove";
            this.removeButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(363, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 15);
            this.label2.TabIndex = 221;
            this.label2.Text = "Connections can belong to multiple groups";
            // 
            // gridAvail
            // 
            this.gridAvail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAvail.Location = new System.Drawing.Point(592, 41);
            this.gridAvail.Name = "gridAvail";
            this.gridAvail.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.gridAvail.Size = new System.Drawing.Size(480, 482);
            this.gridAvail.TabIndex = 222;
            this.gridAvail.Title = "Available Connections";
            this.gridAvail.TranslationName = "TableConnections";
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(992, 529);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(80, 25);
            this.butCancel.TabIndex = 223;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // FormCentralConnectionGroupEdit
            // 
            this.AcceptButton = this.butOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1084, 566);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.gridAvail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.gridMain);
            this.Controls.Add(this.textDescription);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralConnectionGroupEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection Group Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCentralConnectionGroupEdit_FormClosing);
            this.Load += new System.EventHandler(this.FormCentralConnectionGroupEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button removeButton;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.ODGrid gridAvail;
		private OpenDental.UI.Button butCancel;
	}
}
