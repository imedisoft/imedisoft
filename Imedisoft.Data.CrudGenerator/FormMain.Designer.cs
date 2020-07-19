namespace Imedisoft.Data.CrudGenerator
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.exportButton = new System.Windows.Forms.Button();
            this.snippetTextBox = new System.Windows.Forms.TextBox();
            this.typesComboBox = new System.Windows.Forms.ListBox();
            this.iconsImageList = new System.Windows.Forms.ImageList(this.components);
            this.assemblyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(12, 438);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(110, 30);
            this.exportButton.TabIndex = 2;
            this.exportButton.Text = "&Generate All";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // snippetTextBox
            // 
            this.snippetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.snippetTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.snippetTextBox.Location = new System.Drawing.Point(198, 12);
            this.snippetTextBox.Multiline = true;
            this.snippetTextBox.Name = "snippetTextBox";
            this.snippetTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.snippetTextBox.Size = new System.Drawing.Size(774, 637);
            this.snippetTextBox.TabIndex = 3;
            this.snippetTextBox.WordWrap = false;
            // 
            // typesComboBox
            // 
            this.typesComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.typesComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typesComboBox.FormattingEnabled = true;
            this.typesComboBox.IntegralHeight = false;
            this.typesComboBox.ItemHeight = 18;
            this.typesComboBox.Location = new System.Drawing.Point(12, 39);
            this.typesComboBox.Name = "typesComboBox";
            this.typesComboBox.Size = new System.Drawing.Size(180, 393);
            this.typesComboBox.TabIndex = 1;
            this.typesComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TypesComboBox_DrawItem);
            this.typesComboBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TypesComboBox_MouseDoubleClick);
            // 
            // iconsImageList
            // 
            this.iconsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconsImageList.ImageStream")));
            this.iconsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconsImageList.Images.SetKeyName(0, "class");
            // 
            // assemblyTextBox
            // 
            this.assemblyTextBox.Location = new System.Drawing.Point(12, 12);
            this.assemblyTextBox.Name = "assemblyTextBox";
            this.assemblyTextBox.ReadOnly = true;
            this.assemblyTextBox.Size = new System.Drawing.Size(180, 21);
            this.assemblyTextBox.TabIndex = 0;
            this.assemblyTextBox.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.assemblyTextBox);
            this.Controls.Add(this.typesComboBox);
            this.Controls.Add(this.snippetTextBox);
            this.Controls.Add(this.exportButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crud Generator";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.TextBox snippetTextBox;
        private System.Windows.Forms.ListBox typesComboBox;
        private System.Windows.Forms.ImageList iconsImageList;
        private System.Windows.Forms.TextBox assemblyTextBox;
    }
}
