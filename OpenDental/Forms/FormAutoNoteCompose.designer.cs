namespace Imedisoft.Forms
{
	partial class FormAutoNoteCompose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoNoteCompose));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.autoNotesTreeView = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.insertButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "Select Auto Note";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(351, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 112;
            this.label2.Text = "Note";
            // 
            // noteTextBox
            // 
            this.noteTextBox.AcceptsReturn = true;
            this.noteTextBox.AcceptsTab = true;
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.Location = new System.Drawing.Point(354, 25);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.noteTextBox.Size = new System.Drawing.Size(478, 513);
            this.noteTextBox.TabIndex = 113;
            this.noteTextBox.TextChanged += new System.EventHandler(this.NoteTextBox_TextChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Enabled = false;
            this.acceptButton.Location = new System.Drawing.Point(666, 544);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(752, 544);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            // 
            // autoNotesTreeView
            // 
            this.autoNotesTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.autoNotesTreeView.HideSelection = false;
            this.autoNotesTreeView.ImageIndex = 0;
            this.autoNotesTreeView.ImageList = this.imageListTree;
            this.autoNotesTreeView.Indent = 12;
            this.autoNotesTreeView.Location = new System.Drawing.Point(12, 25);
            this.autoNotesTreeView.Name = "autoNotesTreeView";
            this.autoNotesTreeView.SelectedImageIndex = 0;
            this.autoNotesTreeView.Size = new System.Drawing.Size(250, 513);
            this.autoNotesTreeView.TabIndex = 114;
            this.autoNotesTreeView.DoubleClick += new System.EventHandler(this.AutoNotesTreeView_DoubleClick);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "imageFolder");
            this.imageListTree.Images.SetKeyName(1, "imageText");
            // 
            // insertButton
            // 
            this.insertButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.insertButton.Image = global::Imedisoft.Properties.Resources.IconArrowRight;
            this.insertButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.insertButton.Location = new System.Drawing.Point(268, 250);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(80, 25);
            this.insertButton.TabIndex = 115;
            this.insertButton.Text = "&Insert";
            this.insertButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.insertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // FormAutoNoteCompose
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(844, 581);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.autoNotesTreeView);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNoteCompose";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compose Auto Note";
            this.Load += new System.EventHandler(this.FormAutoNoteCompose_Load);
            this.Shown += new System.EventHandler(this.FormAutoNoteCompose_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox noteTextBox;
		private System.Windows.Forms.TreeView autoNotesTreeView;
		private System.Windows.Forms.ImageList imageListTree;
		private OpenDental.UI.Button insertButton;
	}
}
