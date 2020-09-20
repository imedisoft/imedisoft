namespace Imedisoft.Forms
{
    partial class FormAutoNoteExport
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
            this.autoNotesTreeView = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.labelExportSelect = new System.Windows.Forms.Label();
            this.collapseCheckBox = new System.Windows.Forms.CheckBox();
            this.exportButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.clearButton = new OpenDental.UI.Button();
            this.selectAllButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // autoNotesTreeView
            // 
            this.autoNotesTreeView.AllowDrop = true;
            this.autoNotesTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoNotesTreeView.CheckBoxes = true;
            this.autoNotesTreeView.ImageIndex = 0;
            this.autoNotesTreeView.ImageList = this.imageListTree;
            this.autoNotesTreeView.Indent = 12;
            this.autoNotesTreeView.Location = new System.Drawing.Point(12, 35);
            this.autoNotesTreeView.Name = "autoNotesTreeView";
            this.autoNotesTreeView.SelectedImageIndex = 0;
            this.autoNotesTreeView.Size = new System.Drawing.Size(324, 554);
            this.autoNotesTreeView.TabIndex = 0;
            this.autoNotesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.AutoNotesTreeView_AfterCheck);
            // 
            // imageListTree
            // 
            this.imageListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListTree.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // labelExportSelect
            // 
            this.labelExportSelect.AutoSize = true;
            this.labelExportSelect.Location = new System.Drawing.Point(12, 9);
            this.labelExportSelect.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.labelExportSelect.Name = "labelExportSelect";
            this.labelExportSelect.Size = new System.Drawing.Size(242, 13);
            this.labelExportSelect.TabIndex = 1;
            this.labelExportSelect.Text = "Select the Auto Notes you would like to export...";
            // 
            // collapseCheckBox
            // 
            this.collapseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.collapseCheckBox.AutoSize = true;
            this.collapseCheckBox.Location = new System.Drawing.Point(342, 35);
            this.collapseCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.collapseCheckBox.Name = "collapseCheckBox";
            this.collapseCheckBox.Size = new System.Drawing.Size(80, 17);
            this.collapseCheckBox.TabIndex = 3;
            this.collapseCheckBox.Text = "Collapse All";
            this.collapseCheckBox.CheckedChanged += new System.EventHandler(this.CollapseCheckBox_CheckedChanged);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Location = new System.Drawing.Point(342, 533);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(90, 25);
            this.exportButton.TabIndex = 2;
            this.exportButton.Text = "&Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(342, 564);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.Location = new System.Drawing.Point(342, 106);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(90, 25);
            this.clearButton.TabIndex = 6;
            this.clearButton.Text = "Clear Selection";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAllButton.Location = new System.Drawing.Point(342, 75);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(90, 25);
            this.selectAllButton.TabIndex = 7;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // FormAutoNoteExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 601);
            this.Controls.Add(this.selectAllButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.collapseCheckBox);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.labelExportSelect);
            this.Controls.Add(this.autoNotesTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoNoteExport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Note Export";
            this.Load += new System.EventHandler(this.FormAutoNoteExport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView autoNotesTreeView;
        private System.Windows.Forms.Label labelExportSelect;
        private System.Windows.Forms.ImageList imageListTree;
        private OpenDental.UI.Button exportButton;
        private System.Windows.Forms.CheckBox collapseCheckBox;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button clearButton;
        private OpenDental.UI.Button selectAllButton;
    }
}
