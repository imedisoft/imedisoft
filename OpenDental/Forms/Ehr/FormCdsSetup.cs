using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCdsSetup : FormBase
	{
		private readonly List<CdsPermissionState> cdsPermissionStates = new List<CdsPermissionState>();

		public FormCdsSetup()
		{
			InitializeComponent();
		}

		private void FormCDSSetup_Load(object sender, EventArgs e)
		{
			var users = Users.GetAll(true);

			foreach (var cdsPermission in CdsPermissions.GetAll())
            {
				var user = users.FirstOrDefault(user => user.Id == cdsPermission.UserId);
				if (user == null)
                {
					continue;
                }

				cdsPermissionStates.Add(new CdsPermissionState(user, cdsPermission));
            }

			FillGrid();
		}

		class CdsPermissionState
		{
			public readonly User User;
			public readonly CdsPermission CdsPermission;
			public readonly bool[] Permissions = new bool[10];

			public CdsPermissionState(User user, CdsPermission cdsPermission)
			{
				User = user;

				CdsPermission = cdsPermission ?? new CdsPermission
				{
					UserId = user.Id
				};

				Permissions[0] = cdsPermission.ShowCDS;
				Permissions[1] = cdsPermission.SetupCDS;
				Permissions[2] = cdsPermission.ShowInfoButton;
				Permissions[3] = cdsPermission.EditBibliography;
				Permissions[4] = cdsPermission.ProblemCDS;
				Permissions[5] = cdsPermission.MedicationCDS;
				Permissions[6] = cdsPermission.AllergyCDS;
				Permissions[7] = cdsPermission.DemographicCDS;
				Permissions[8] = cdsPermission.LabTestCDS;
				Permissions[9] = cdsPermission.VitalCDS;
			}

			private bool HasChanges => 
				Permissions[0] != CdsPermission.ShowCDS ||
				Permissions[1] != CdsPermission.SetupCDS ||
				Permissions[2] != CdsPermission.ShowInfoButton ||
				Permissions[3] != CdsPermission.EditBibliography ||
				Permissions[4] != CdsPermission.ProblemCDS ||
				Permissions[5] != CdsPermission.MedicationCDS ||
				Permissions[6] != CdsPermission.AllergyCDS ||
				Permissions[7] != CdsPermission.DemographicCDS ||
				Permissions[8] != CdsPermission.LabTestCDS ||
				Permissions[9] != CdsPermission.VitalCDS;

			public void Save()
            {
				if (HasChanges)
                {
					CdsPermissions.Save(CdsPermission);
                }
            }
		}

		private void CdsPermissionsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			if (e.Col < 1) return;

			var index = e.Col - 2;
			if (index <0 || index > 9)
            {
				return;
            }

			var cdsPermissionState = cdsPermissionsGrid.SelectedTag<CdsPermissionState>();
			if (cdsPermissionState == null)
			{
				return;
			}

			cdsPermissionState.Permissions[index] = !cdsPermissionState.Permissions[index];

			FillGrid();
		}

		private void FillGrid()
		{
			cdsPermissionsGrid.BeginUpdate();
			cdsPermissionsGrid.Columns.Clear();
			cdsPermissionsGrid.Columns.Add(new GridColumn(Translation.Common.User, 120));
			cdsPermissionsGrid.Columns.Add(new GridColumn("Show CDS", 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn("Show i", 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn("Edit CDS", 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn(Translation.Common.Source, 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn(Translation.Common.Problem, 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn(Translation.Common.Medication, 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn(Translation.Common.Allergy, 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn("Demographic", 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn("Labs", 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Columns.Add(new GridColumn("Vitals", 80, HorizontalAlignment.Center));
			cdsPermissionsGrid.Rows.Clear();

			foreach (var cdsPermissionState in cdsPermissionStates)
            {
				var gridRow = new GridRow();
				gridRow.Cells.Add(cdsPermissionState.User?.UserName);
				gridRow.Tag = cdsPermissionState;

				foreach (var value in cdsPermissionState.Permissions)
                {
					gridRow.Cells.Add(value ? "X" : "");
                }

				cdsPermissionsGrid.Rows.Add(gridRow);
            }

			cdsPermissionsGrid.EndUpdate();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			foreach (var cdsPermission in cdsPermissionStates)
			{
				cdsPermission.Save();
			}

			DialogResult = DialogResult.OK;
		}
	}
}
