using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace Imedisoft.Forms
{
    /// <summary>
    /// This form shows all of the security log entries for one fKey item. 
    /// So far this only applies to a single appointment or a single procedure code.
    /// </summary>
    public partial class FormAuditOneType : FormBase
	{
		private readonly long patientId;
		private readonly List<Permissions> permissions;
		private readonly long foreignKey;

		public FormAuditOneType(long patientId, List<Permissions> permissions, string title, long foreignKey)
		{
			InitializeComponent();

			Text = title;

			this.patientId = patientId;
			this.permissions = new List<Permissions>(permissions);
			this.foreignKey = foreignKey;
		}

		private void FormAuditOneType_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			var securityLogs = new List<SecurityLog>();
			try
			{
				securityLogs.AddRange(
					SecurityLogs.Refresh(patientId, permissions, foreignKey));
			}
			catch (Exception ex)
			{
				FriendlyException.Show(
					"There was a problem refreshing the Audit Trail with the current filters.", ex);
			}

			grid.BeginUpdate();
			grid.ListGridColumns.Clear();
			grid.ListGridColumns.Add(new GridColumn("Date Time", 120));
			grid.ListGridColumns.Add(new GridColumn("User", 70));
			grid.ListGridColumns.Add(new GridColumn("Permission", 170));
			grid.ListGridColumns.Add(new GridColumn("Log Text", 510));
			grid.ListGridRows.Clear();

			foreach (var securityLog in securityLogs)
			{
				var user = Userods.GetUser(securityLog.UserNum);

				var gridRow = new GridRow();
				gridRow.Cells.Add(securityLog.LogDateTime.ToShortDateString() + " " + securityLog.LogDateTime.ToShortTimeString());
				gridRow.Cells.Add(user?.UserName ?? "unknown");
				gridRow.Cells.Add(securityLog.PermType.ToString());
				gridRow.Cells.Add(securityLog.LogText);

				grid.ListGridRows.Add(gridRow);
			}

			grid.EndUpdate();
			grid.ScrollToEnd();
		}
	}
}
