namespace OpenDental {
	partial class UserControlReminderMessage {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.textTemplateSms = new System.Windows.Forms.RichTextBox();
			this.groupSms = new System.Windows.Forms.GroupBox();
			this.groupEmail = new System.Windows.Forms.GroupBox();
			this.butEditEmail = new OpenDental.UI.Button();
			this.browserEmailBody = new System.Windows.Forms.WebBrowser();
			this.textTemplateSubject = new System.Windows.Forms.RichTextBox();
			this.groupSms.SuspendLayout();
			this.groupEmail.SuspendLayout();
			this.SuspendLayout();
			// 
			// textTemplateSms
			// 
			this.textTemplateSms.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textTemplateSms.Location = new System.Drawing.Point(3, 16);
			this.textTemplateSms.Name = "textTemplateSms";
			this.textTemplateSms.Size = new System.Drawing.Size(698, 59);
			this.textTemplateSms.TabIndex = 107;
			this.textTemplateSms.Text = "";
			// 
			// groupSms
			// 
			this.groupSms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupSms.Controls.Add(this.textTemplateSms);
			this.groupSms.Location = new System.Drawing.Point(0, 0);
			this.groupSms.Name = "groupSms";
			this.groupSms.Size = new System.Drawing.Size(704, 78);
			this.groupSms.TabIndex = 122;
			this.groupSms.TabStop = false;
			this.groupSms.Text = "Text Message";
			// 
			// groupEmail
			// 
			this.groupEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupEmail.Controls.Add(this.butEditEmail);
			this.groupEmail.Controls.Add(this.browserEmailBody);
			this.groupEmail.Controls.Add(this.textTemplateSubject);
			this.groupEmail.Location = new System.Drawing.Point(0, 82);
			this.groupEmail.Name = "groupEmail";
			this.groupEmail.Size = new System.Drawing.Size(705, 221);
			this.groupEmail.TabIndex = 123;
			this.groupEmail.TabStop = false;
			this.groupEmail.Text = "Email Subject and Body";
			// 
			// butEditEmail
			// 
			this.butEditEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEditEmail.Location = new System.Drawing.Point(624, 189);
			this.butEditEmail.Name = "butEditEmail";
			this.butEditEmail.Size = new System.Drawing.Size(75, 26);
			this.butEditEmail.TabIndex = 126;
			this.butEditEmail.Text = "&Edit";
			this.butEditEmail.UseVisualStyleBackColor = true;
			this.butEditEmail.Click += new System.EventHandler(this.butEditEmail_Click);
			// 
			// browserEmailBody
			// 
			this.browserEmailBody.AllowWebBrowserDrop = false;
			this.browserEmailBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.browserEmailBody.Location = new System.Drawing.Point(6, 42);
			this.browserEmailBody.MinimumSize = new System.Drawing.Size(20, 20);
			this.browserEmailBody.Name = "browserEmailBody";
			this.browserEmailBody.Size = new System.Drawing.Size(693, 144);
			this.browserEmailBody.TabIndex = 114;
			this.browserEmailBody.WebBrowserShortcutsEnabled = false;
			// 
			// textTemplateSubject
			// 
			this.textTemplateSubject.Dock = System.Windows.Forms.DockStyle.Top;
			this.textTemplateSubject.Location = new System.Drawing.Point(3, 16);
			this.textTemplateSubject.Name = "textTemplateSubject";
			this.textTemplateSubject.Size = new System.Drawing.Size(699, 22);
			this.textTemplateSubject.TabIndex = 113;
			this.textTemplateSubject.Text = "";
			// 
			// UserControlReminderMessage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.groupEmail);
			this.Controls.Add(this.groupSms);
			this.MinimumSize = new System.Drawing.Size(702, 303);
			this.Name = "UserControlReminderMessage";
			this.Size = new System.Drawing.Size(705, 303);
			this.groupSms.ResumeLayout(false);
			this.groupEmail.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.RichTextBox textTemplateSms;
		private System.Windows.Forms.GroupBox groupSms;
		private System.Windows.Forms.GroupBox groupEmail;
		private UI.Button butEditEmail;
		private System.Windows.Forms.WebBrowser browserEmailBody;
		private System.Windows.Forms.RichTextBox textTemplateSubject;
	}
}
