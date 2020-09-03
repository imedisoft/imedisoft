using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormSetupWizard : ODForm
	{
		private List<SetupWizard.SetupWizClass> setupWizardItems;

		public FormSetupWizard()
		{
			InitializeComponent();
		}

		private void FormSetupWizard_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillListSetupItems()
		{
			setupWizardItems = new List<SetupWizard.SetupWizClass>();
			setupWizardItems.Add(new SetupWizard.FeatureSetup());
			setupWizardItems.Add(new SetupWizard.ProvSetup());
			setupWizardItems.Add(new SetupWizard.EmployeeSetup());
			setupWizardItems.Add(new SetupWizard.FeeSchedSetup());
			if (PrefC.HasClinicsEnabled)
			{
				setupWizardItems.Add(new SetupWizard.ClinicSetup());
			}
			setupWizardItems.Add(new SetupWizard.OperatorySetup());
			setupWizardItems.Add(new SetupWizard.PracticeSetup());
			setupWizardItems.Add(new SetupWizard.PrinterSetup());
		}

		private void FillGrid()
		{
			FillListSetupItems();

			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn("Setup Item", 250));
			gridMain.ListGridColumns.Add(new GridColumn("Status", 100, HorizontalAlignment.Center));
			gridMain.ListGridColumns.Add(new GridColumn("?", 35, HorizontalAlignment.Center) { ImageList = imageList1 });
			gridMain.ListGridRows.Clear();

			var gridRows = ConstructGridRows();
			foreach (var gridRow in gridRows)
			{
				gridMain.ListGridRows.Add(gridRow);
			}

			gridMain.EndUpdate();
		}

		private List<GridRow> ConstructGridRows()
		{
			//the Tag of a Parent Row is its ODSetupCategory.
			//the Tag of a Child Row is a SetupWizClass
			List<GridRow> listSetupRows = new List<GridRow>();
			List<GridRow> listCategoryRows = new List<GridRow>();
			List<GridRow> listRowsAll = new List<GridRow>();
			int statusCellNum = 0;
			foreach (SetupWizard.SetupWizClass setupItem in setupWizardItems)
			{
				GridRow row = new GridRow();
				row.Cells.Add("     " + setupItem.Name);
				row.Cells.Add(setupItem.Status.GetDescription());
				statusCellNum = row.Cells.Count - 1;
				row.Cells[statusCellNum].BackColor = SetupWizard.GetColor(setupItem.Status);
				row.Cells.Add("0");
				//row.ColorBackG=SetupWizard.GetColor(setupItem.GetStatus);
				row.Tag = setupItem;
				listSetupRows.Add(row);
			}
			//now add parent rows to the list
			foreach (GridRow rowCur in listSetupRows)
			{
				ODSetupCategory catCur = ((SetupWizard.SetupWizClass)rowCur.Tag).Category;
				//bool exists = false;
				////if the parent row doesn't exist..
				//foreach(ODGridRow parentRow in listCategoryRows) {
				//	if(parentRow.Tag.GetType() == typeof(ODSetupCategory)
				//		&& ((ODSetupCategory)parentRow.Tag) == catCur) {
				//		exists=true;
				//		break;
				//	}
				//}
				if (listCategoryRows.Any(x => x.Tag is ODSetupCategory && (ODSetupCategory)x.Tag == catCur))
				{
					continue;
				}
				//add the parent row.
				GridRow row = new GridRow();
				row.Cells.Add("\r\n" + catCur.GetDescription() + "\r\n");
				row.Cells.Add("");
				row.Cells.Add("");
				row.Tag = catCur;
				row.Bold = true;
				//row.ColorLborder=Color.Black;
				listCategoryRows.Add(row);
				//}
				////for all children rows, find the parent row -- set it to the proper parent row.
				//foreach(ODGridRow parentRow in listParentRows) {
				//	if(parentRow.Tag.GetType() == typeof(ODSetupCategory)
				//		&& ((ODSetupCategory)parentRow.Tag) == catCur) {
				//		rowCur.DropDownParent=parentRow;
				//		break;
				//	}
				//}
			}
			//Assign colors to parent rows.
			foreach (GridRow rowCur in listCategoryRows)
			{
				if (listSetupRows.Where(x => ((SetupWizard.SetupWizClass)x.Tag).Category == ((ODSetupCategory)rowCur.Tag))
					.All(x => ((SetupWizard.SetupWizClass)x.Tag).Status == ODSetupStatus.Complete || ((SetupWizard.SetupWizClass)x.Tag).Status == ODSetupStatus.Optional))
				{
					//rowCur.ColorBackG = SetupWizard.GetColor(ODSetupStatus.Complete);
					rowCur.Cells[statusCellNum].Text = "\r\n" + ODSetupStatus.Complete.GetDescription();
					rowCur.Cells[statusCellNum].BackColor = SetupWizard.GetColor(ODSetupStatus.Complete);

				}
				else
				{
					//rowCur.ColorBackG = SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
					rowCur.Cells[statusCellNum].Text = "\r\n" + ODSetupStatus.NeedsAttention.GetDescription();
					rowCur.Cells[statusCellNum].BackColor = SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
				}
			}
			foreach (GridRow rowCur in listCategoryRows)
			{
				listRowsAll.Add(rowCur);
				listSetupRows.Where(x => ((SetupWizard.SetupWizClass)x.Tag).Category == ((ODSetupCategory)rowCur.Tag)).DefaultIfEmpty(new GridRow()).LastOrDefault().LowerBorderColor = Color.Black;
				listRowsAll.AddRange(listSetupRows.Where(x => ((SetupWizard.SetupWizClass)x.Tag).Category == ((ODSetupCategory)rowCur.Tag)));
			}
			return listRowsAll;
		}

		private void gridMain_CellClick(object sender, ODGridClickEventArgs e)
		{
			GridRow clickedRow = gridMain.ListGridRows[e.Row];
			GridColumn clickedCol = gridMain.ListGridColumns[e.Col];
			if (clickedRow.Tag.GetType() == typeof(ODSetupCategory))
			{
				for (int i = 0; i < gridMain.ListGridRows.Count; i++)
				{
					GridRow row = gridMain.ListGridRows[i];
					if (row.Tag is SetupWizard.SetupWizClass
						&& ((SetupWizard.SetupWizClass)row.Tag).Category == (ODSetupCategory)clickedRow.Tag)
					{
						gridMain.SetSelected(i, true);
					}
				}
				return;
			}
			if (clickedRow.Tag.GetType().BaseType != typeof(SetupWizard.SetupWizClass)
				|| clickedCol.ImageList == null)
			{
				return;
			}
			MessageBox.Show(((SetupWizard.SetupWizClass)clickedRow.Tag).Description);
		}

		private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			//Show a "Congatulations, you've already finished this!" section for finished sections.
			GridRow clickedRow = gridMain.ListGridRows[e.Row];
			FormSetupWizardProgress FormSWP;
			List<SetupWizard.SetupWizClass> listSetupClasses = new List<SetupWizard.SetupWizClass>();
			if (clickedRow.Tag.GetType().BaseType != typeof(SetupWizard.SetupWizClass))
			{ //category clicked
				foreach (SetupWizard.SetupWizClass setupWizClass in setupWizardItems)
				{ //for each row, add the row and an intro and complete class.
					if (setupWizClass.Category == (ODSetupCategory)clickedRow.Tag)
					{
						SetupWizard.SetupIntro intro = new SetupWizard.SetupIntro(setupWizClass.Name, setupWizClass.Description);
						SetupWizard.SetupComplete complete = new SetupWizard.SetupComplete(setupWizClass.Name);
						listSetupClasses.Add(intro);
						listSetupClasses.Add(setupWizClass);
						listSetupClasses.Add(complete);
					}
				}
				FormSWP = new FormSetupWizardProgress(listSetupClasses, true);
				FormSWP.ShowDialog();
			}
			else
			{ //single row clicked
				SetupWizard.SetupWizClass setupWizClass = (SetupWizard.SetupWizClass)clickedRow.Tag;
				SetupWizard.SetupIntro intro = new SetupWizard.SetupIntro(setupWizClass.Name, setupWizClass.Description);
				SetupWizard.SetupComplete complete = new SetupWizard.SetupComplete(setupWizClass.Name);
				listSetupClasses.Add(intro);
				listSetupClasses.Add(setupWizClass);
				listSetupClasses.Add(complete);
				FormSWP = new FormSetupWizardProgress(listSetupClasses, false);
				FormSWP.ShowDialog();
			}
			FillGrid();
		}

		private void butAll_Click(object sender, EventArgs e)
		{
			List<SetupWizard.SetupWizClass> listSetupClasses = new List<OpenDental.SetupWizard.SetupWizClass>();
			foreach (SetupWizard.SetupWizClass setupWizClass in setupWizardItems)
			{ //for each row, add the row and an intro and complete class.
				SetupWizard.SetupIntro intro = new SetupWizard.SetupIntro(setupWizClass.Name, setupWizClass.Description);
				SetupWizard.SetupComplete complete = new SetupWizard.SetupComplete(setupWizClass.Name);
				listSetupClasses.Add(intro);
				listSetupClasses.Add(setupWizClass);
				listSetupClasses.Add(complete);
			}
			FormSetupWizardProgress FormSWP = new FormSetupWizardProgress(listSetupClasses, true);
			FormSWP.ShowDialog();
			FillGrid();
		}

		private void butSelected_Click(object sender, EventArgs e)
		{
			List<SetupWizard.SetupWizClass> listSetupClasses = new List<SetupWizard.SetupWizClass>();
			foreach (int rowNum in gridMain.SelectedIndices)
			{
				GridRow selectedRow = gridMain.ListGridRows[rowNum];
				if (selectedRow.Tag.GetType().BaseType != typeof(OpenDental.SetupWizard.SetupWizClass))
				{
					continue;
				}
				SetupWizard.SetupWizClass setupWizClass = (SetupWizard.SetupWizClass)selectedRow.Tag;
				SetupWizard.SetupIntro intro = new SetupWizard.SetupIntro(setupWizClass.Name, setupWizClass.Description);
				SetupWizard.SetupComplete complete = new SetupWizard.SetupComplete(setupWizClass.Name);
				listSetupClasses.Add(intro);
				listSetupClasses.Add(setupWizClass);
				listSetupClasses.Add(complete);
			}
			FormSetupWizardProgress FormSWP = new FormSetupWizardProgress(listSetupClasses, false);
			FormSWP.ShowDialog();
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
