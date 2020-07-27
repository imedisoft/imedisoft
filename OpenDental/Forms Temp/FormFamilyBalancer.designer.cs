namespace OpenDental{
	partial class FormFamilyBalancer {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFamilyBalancer));
			this.datePicker = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butStartPause = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.progressBarTransfer = new System.Windows.Forms.ProgressBar();
			this.timerProgress = new System.Windows.Forms.Timer(this.components);
			this.labelProgress = new System.Windows.Forms.Label();
			this.labelPayments = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// datePicker
			// 
			this.datePicker.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.datePicker.Location = new System.Drawing.Point(162, 41);
			this.datePicker.Name = "datePicker";
			this.datePicker.Size = new System.Drawing.Size(132, 20);
			this.datePicker.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label1.Location = new System.Drawing.Point(16, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 13);
			this.label1.TabIndex = 134;
			this.label1.Text = "Transfer Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label15.Location = new System.Drawing.Point(13, 9);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(436, 29);
			this.label15.TabIndex = 135;
			this.label15.Text = "This tool performs an income transfer for every family. This process can take a l" +
    "ong time.";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(379, 174);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butStartPause
			// 
			this.butStartPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butStartPause.Location = new System.Drawing.Point(195, 173);
			this.butStartPause.Name = "butStartPause";
			this.butStartPause.Size = new System.Drawing.Size(75, 26);
			this.butStartPause.TabIndex = 149;
			this.butStartPause.Text = "Start";
			this.butStartPause.Click += new System.EventHandler(this.butStartPause_Click);
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label2.Location = new System.Drawing.Point(300, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(154, 13);
			this.label2.TabIndex = 150;
			this.label2.Text = "(Payment Date)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// progressBarTransfer
			// 
			this.progressBarTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBarTransfer.Location = new System.Drawing.Point(16, 97);
			this.progressBarTransfer.Name = "progressBarTransfer";
			this.progressBarTransfer.Size = new System.Drawing.Size(438, 23);
			this.progressBarTransfer.TabIndex = 153;
			// 
			// timerProgress
			// 
			this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
			// 
			// labelProgress
			// 
			this.labelProgress.Location = new System.Drawing.Point(13, 123);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(176, 13);
			this.labelProgress.TabIndex = 154;
			this.labelProgress.Text = "labelProgress";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPayments
			// 
			this.labelPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPayments.Location = new System.Drawing.Point(278, 123);
			this.labelPayments.Name = "labelPayments";
			this.labelPayments.Size = new System.Drawing.Size(176, 13);
			this.labelPayments.TabIndex = 155;
			this.labelPayments.Text = "labelPayments";
			this.labelPayments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormFamilyBalancer
			// 
			this.ClientSize = new System.Drawing.Size(464, 211);
			this.Controls.Add(this.labelPayments);
			this.Controls.Add(this.labelProgress);
			this.Controls.Add(this.progressBarTransfer);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.datePicker);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butStartPause);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(480, 250);
			this.Name = "FormFamilyBalancer";
			this.Text = "Family Balancer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFamilyBalancer_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion
		private UI.Button butCancel;
		private System.Windows.Forms.DateTimePicker datePicker;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label15;
		private UI.Button butStartPause;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ProgressBar progressBarTransfer;
		private System.Windows.Forms.Timer timerProgress;
		private System.Windows.Forms.Label labelProgress;
		private System.Windows.Forms.Label labelPayments;
	}
}