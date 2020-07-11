using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental
{
	public class FormTasks : ODForm
	{
		//private System.ComponentModel.IContainer components;
		/////<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		//public TaskObjectType GotoType;
		private UserControlTasks userControlTasks1;
		private IContainer components = null;
		/////<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		//public long GotoKeyNum;
		//private bool IsTriage;
		private SplitContainer splitter;
		private FormWindowState windowStateOld;

		public UserControlTasksTab TaskTab
		{
			get
			{
				return userControlTasks1.TaskTab;
			}
			set
			{
				userControlTasks1.TaskTab = value;
			}
		}

		///<summary></summary>
		public FormTasks()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//Lan.F(this);
		}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTasks));
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.userControlTasks1 = new OpenDental.UserControlTasks();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.Panel2.SuspendLayout();
			this.splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitter
			// 
			this.splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.splitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitter.Location = new System.Drawing.Point(0, 0);
			this.splitter.Name = "splitter";
			this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.userControlTasks1);
			this.splitter.Panel1MinSize = 150;
			// 
			// splitter.Panel2
			// 
			this.splitter.Panel2MinSize = 150;
			this.splitter.Size = new System.Drawing.Size(1230, 696);
			this.splitter.SplitterDistance = 540;
			this.splitter.TabIndex = 0;
			this.splitter.TabStop = false;
			// 
			// userControlTasks1
			// 
			this.userControlTasks1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlTasks1.Location = new System.Drawing.Point(0, 0);
			this.userControlTasks1.Name = "userControlTasks1";
			this.userControlTasks1.Size = new System.Drawing.Size(1228, 538);
			this.userControlTasks1.TabIndex = 0;
			this.userControlTasks1.TaskTab = OpenDental.UserControlTasksTab.ForUser;
			this.userControlTasks1.FillGridEvent += new OpenDental.UserControlTasks.FillGridEventHandler(this.UserControlTasks1_FillGridEvent);
			this.userControlTasks1.Resize += new System.EventHandler(this.userControlTasks1_Resize);
			// 
			// FormTasks
			// 
			this.ClientSize = new System.Drawing.Size(1230, 696);
			this.Controls.Add(this.splitter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(175, 100);
			this.Name = "FormTasks";
			this.Text = "Tasks";
			this.Load += new System.EventHandler(this.FormTasks_Load);
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormTasks_Load(object sender, EventArgs e)
		{
			windowStateOld = WindowState;
			userControlTasks1.InitializeOnStartup();
			splitter.Panel2Collapsed = true;
		}

		private void userControlTasks1_GoToChanged(object sender, EventArgs e)
		{
			TaskObjectType gotoType = userControlTasks1.GotoType;
			long gotoKeyNum = userControlTasks1.GotoKeyNum;
			if (gotoType == TaskObjectType.Patient)
			{
				if (gotoKeyNum != 0)
				{
					Patient pat = Patients.GetPat(gotoKeyNum);
					//OnPatientSelected(pat);

					GotoModule.GotoAccount(pat.PatNum);

				}
			}
			if (gotoType == TaskObjectType.Appointment)
			{
				if (gotoKeyNum != 0)
				{
					Appointment apt = Appointments.GetOneApt(gotoKeyNum);
					if (apt == null)
					{
						MessageBox.Show( "Appointment has been deleted, so it's not available.");
						return;
						//this could be a little better, because window has closed, but they will learn not to push that button.
					}
					DateTime dateSelected = DateTime.MinValue;
					if (apt.AptStatus == ApptStatus.Planned || apt.AptStatus == ApptStatus.UnschedList)
					{
						//I did not add feature to put planned or unsched apt on pinboard.
						MessageBox.Show( "Cannot navigate to appointment.  Use the Other Appointments button.");
						//return;
					}
					else
					{
						dateSelected = apt.AptDateTime;
					}
					Patient pat = Patients.GetPat(apt.PatNum);
					//OnPatientSelected(pat);
					GotoModule.GotoAppointment(dateSelected, apt.AptNum);
				}
			}
			//DialogResult=DialogResult.OK;
		}

		private void userControlTasks1_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{//Form currently minimized.
				windowStateOld = WindowState;
				return;//The window is invisble when minimized, so no need to refresh.
			}
			if (windowStateOld == FormWindowState.Minimized)
			{//Form was previously minimized (invisible) and is now in normal state or maximized state.
				windowStateOld = WindowState;
				return;
			}
			windowStateOld = WindowState;//Set the window state after every resize.
		}

		private void UserControlTasks1_FillGridEvent(object sender, EventArgs e)
		{
			Text = userControlTasks1.ControlParentTitle;
		}
	}
}