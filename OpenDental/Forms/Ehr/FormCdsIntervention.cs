using Imedisoft.Data;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCdsIntervention : FormBase
	{
        private readonly List<CdsIntervention> cdsInterventions;
		private readonly bool showCancelButton;
		private bool showInfoButton;

		FormCdsIntervention(List<CdsIntervention> cdsInterventions, bool showCancelButton = true)
		{
			InitializeComponent();

			this.cdsInterventions = cdsInterventions;
			this.showCancelButton = showCancelButton;
		}

		private void FormCDSIntervention_Load(object sender, EventArgs e)
		{
			cancelButton.Visible = showCancelButton;

			if (!cancelButton.Visible)
            {
				acceptButton.Left = cancelButton.Right - acceptButton.Width;
            }

			FillGrid();
		}

		private void FillGrid()
		{
			showInfoButton = CdsPermissions.GetByUser(Security.CurrentUser.Id).ShowInfoButton;

			cdsInterventionsGrid.BeginUpdate();
			cdsInterventionsGrid.Columns.Clear();

			if (showInfoButton)
			{
				cdsInterventionsGrid.Columns.Add(new GridColumn("", 18) { ImageList = imageListInfoButton });
			}

			cdsInterventionsGrid.Columns.Add(new GridColumn(Translation.Ehr.Conditions, 300));
			cdsInterventionsGrid.Columns.Add(new GridColumn(Translation.Ehr.Instructions, 400));
			cdsInterventionsGrid.Columns.Add(new GridColumn(Translation.Ehr.Bibliography, 120));
			cdsInterventionsGrid.Rows.Clear();

			foreach (var cdsIntervention in cdsInterventions)
            {
				var gridRow = new GridRow();
				if (showInfoButton)
				{
					gridRow.Cells.Add("");
				}

				gridRow.Cells.Add(cdsIntervention.InterventionMessage);
				gridRow.Cells.Add(cdsIntervention.EhrTrigger.Instructions);
				gridRow.Cells.Add(cdsIntervention.EhrTrigger.Bibliography);
				gridRow.Tag = cdsIntervention;

				cdsInterventionsGrid.Rows.Add(gridRow);
			}

			cdsInterventionsGrid.EndUpdate();
		}

		private void CdsInterventionsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			if (showInfoButton == false || e.Col != 0) return;

			var cdsIntervention = cdsInterventionsGrid.SelectedTag<CdsIntervention>();
			if (cdsIntervention == null)
            {
				return;
            }

			using var formInfobutton = new FormInfobutton(cdsIntervention.TriggerObjects);

			formInfobutton.ShowDialog(this);
		}

		public static DialogResult ShowIfRequired(IEnumerable<CdsIntervention> cdsInterventions, bool showCancelButton = true)
        {
			if (cdsInterventions == null)
            {
				return DialogResult.Cancel;
            }

			var cdsInterventionsList = cdsInterventions.ToList();
			if (cdsInterventionsList.Count == 0)
            {
				return DialogResult.Cancel;
            }

			using var formCdsIntervention = new FormCdsIntervention(cdsInterventionsList, showCancelButton);

			return formCdsIntervention.ShowDialog();
        }
	}
}
