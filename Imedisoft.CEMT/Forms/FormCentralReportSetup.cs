using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralReportSetup : FormBase
	{
		private readonly long userGroupId;
		private readonly bool isPermissionMode;

		/// <summary>
		/// Gets a value indicating whether the user group has report permissions.
		/// </summary>
		public bool HasReportPermissions { get; private set; }

		public FormCentralReportSetup(long userGroupId, bool isPermissionMode)
		{
			InitializeComponent();

			this.userGroupId = userGroupId;
			this.isPermissionMode = isPermissionMode;
		}

		private void FormCentralReportSetup_Load(object sender, EventArgs e)
		{
			reportSetupUserControl.InitializeOnStartup(true, userGroupId, isPermissionMode, true);

			if (isPermissionMode)
			{
				reportSetupTabControl.SelectedIndex = 1;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			GroupPermissions.Sync(reportSetupUserControl.ListGroupPermissionsForReports, reportSetupUserControl.ListGroupPermissionsOld);

			if (reportSetupUserControl.ListGroupPermissionsForReports.Exists(x => x.UserGroupNum == userGroupId))
			{
				HasReportPermissions = true;
			}

			GroupPermissions.RefreshCache();

			DialogResult = DialogResult.OK;
		}
	}
}
