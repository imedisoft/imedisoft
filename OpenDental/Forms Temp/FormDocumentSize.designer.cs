namespace OpenDental{
	partial class FormDocumentSize {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDocumentSize));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textTime = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelCropInfo = new System.Windows.Forms.Label();
			this.butCropReset = new OpenDental.UI.Button();
			this.labelCrop = new System.Windows.Forms.Label();
			this.button1 = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(429, 214);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(429, 244);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.Location = new System.Drawing.Point(40, 70);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(152, 18);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "Is Flipped Horizontally";
			this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(198, 71);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(252, 18);
			this.label1.TabIndex = 5;
			this.label1.Text = "(to flip vertically, combine this with a 180 rotation)";
			// 
			// textTime
			// 
			this.textTime.Location = new System.Drawing.Point(177, 94);
			this.textTime.Name = "textTime";
			this.textTime.Size = new System.Drawing.Size(43, 20);
			this.textTime.TabIndex = 131;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(68, 95);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(106, 16);
			this.label4.TabIndex = 130;
			this.label4.Text = "Degrees Rotated";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(225, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(252, 18);
			this.label2.TabIndex = 132;
			this.label2.Text = "(Only allowed 0,90,180, and 270)";
			// 
			// labelCropInfo
			// 
			this.labelCropInfo.Location = new System.Drawing.Point(247, 127);
			this.labelCropInfo.Name = "labelCropInfo";
			this.labelCropInfo.Size = new System.Drawing.Size(250, 18);
			this.labelCropInfo.TabIndex = 135;
			this.labelCropInfo.Text = "(this image has no crop applied)";
			// 
			// butCropReset
			// 
			this.butCropReset.Location = new System.Drawing.Point(177, 121);
			this.butCropReset.Name = "butCropReset";
			this.butCropReset.Size = new System.Drawing.Size(64, 24);
			this.butCropReset.TabIndex = 134;
			this.butCropReset.Text = "Reset";
			// 
			// labelCrop
			// 
			this.labelCrop.Location = new System.Drawing.Point(94, 126);
			this.labelCrop.Name = "labelCrop";
			this.labelCrop.Size = new System.Drawing.Size(82, 18);
			this.labelCrop.TabIndex = 133;
			this.labelCrop.Text = "Crop";
			this.labelCrop.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(177, 152);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 24);
			this.button1.TabIndex = 136;
			this.button1.Text = "Reset All";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(247, 157);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(250, 18);
			this.label3.TabIndex = 137;
			this.label3.Text = "(flip, rotate, and crop)";
			// 
			// FormDocumentSize
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(516, 280);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.labelCropInfo);
			this.Controls.Add(this.butCropReset);
			this.Controls.Add(this.labelCrop);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textTime);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormDocumentSize";
			this.Text = "Item Size";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textTime;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelCropInfo;
		private UI.Button butCropReset;
		private System.Windows.Forms.Label labelCrop;
		private UI.Button button1;
		private System.Windows.Forms.Label label3;
	}
}