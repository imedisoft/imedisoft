using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCdsTriggers : FormBase
	{
		public FormCdsTriggers()
		{
			InitializeComponent();
		}

		private void FormCdsTriggers_Load(object sender, EventArgs e)
		{
			if (CdsPermissions.GetByUser(Security.CurrentUser.Id).SetupCDS || Security.IsAuthorized(Permissions.SecurityAdmin, true))
			{
				setupButton.Enabled = addButton.Enabled = 
					ehrTriggersGrid.Enabled = true;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			ehrTriggersGrid.BeginUpdate();
			ehrTriggersGrid.Columns.Clear();
			ehrTriggersGrid.Columns.Add(new GridColumn(Translation.Common.Description, 200));
			ehrTriggersGrid.Columns.Add(new GridColumn(Translation.Ehr.Cardinality, 140));
			ehrTriggersGrid.Columns.Add(new GridColumn(Translation.Ehr.TriggerCategories, 200));
			ehrTriggersGrid.Rows.Clear();

			foreach (var erhTrigger in EhrTriggers.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(erhTrigger.Description);
				gridRow.Cells.Add(erhTrigger.Cardinality.ToString());
				gridRow.Cells.Add(erhTrigger.GetTriggerCategories());
				gridRow.Tag = erhTrigger;

				ehrTriggersGrid.Rows.Add(gridRow);
			}

			ehrTriggersGrid.EndUpdate();
		}

		private void EhrTriggersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var ehrTrigger = ehrTriggersGrid.SelectedTag<EhrTrigger>();
			if (ehrTrigger == null)
            {
				return;
            }

            using var formCdsTriggerEdit = new FormCdsTriggerEdit
            {
                EhrTriggerCur = ehrTrigger
            };

            if (formCdsTriggerEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            using var formCdsTriggerEdit = new FormCdsTriggerEdit
            {
                EhrTriggerCur = new EhrTrigger(),
                IsNew = true
            };

            if (formCdsTriggerEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var ehrTrigger = ehrTriggersGrid.SelectedTag<EhrTrigger>();
			if (ehrTrigger == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			EhrTriggers.Delete(ehrTrigger.EhrTriggerNum);

			FillGrid();
		}

		private void SettingsButton_Click(object sender, EventArgs e)
		{
			using var formCdsSetup = new FormCdsSetup();

			formCdsSetup.ShowDialog();
		}

		private void FormCdsTriggers_FormClosing(object sender, FormClosingEventArgs e)
		{
			EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.UtcNow,
				Type = EhrMeasureEventType.ClinicalInterventionRules,
				MoreInfo = Translation.Ehr.TriggersEnabled + ": " + EhrTriggers.GetAll().Count
			});
		}
    }
}
