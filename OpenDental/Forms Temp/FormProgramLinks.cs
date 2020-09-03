using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProgramLinks : FormBase
	{
		private bool changed;
		private List<Program> programs;

		public FormProgramLinks()
		{
			InitializeComponent();
		}

		private void FormProgramLinks_Load(object sender, EventArgs e)
		{
			FillList();
		}

		private void FillList()
		{
			Programs.RefreshCache();

			programs = Programs.GetListDeep();
			programs.RemoveAll(x => x.Name == ProgramName.AvaTax.ToString());
			programs.RemoveAll(x => x.Name == ProgramName.CareCredit.ToString());

			programsGrid.BeginUpdate();
			programsGrid.ListGridColumns.Clear();
			programsGrid.ListGridColumns.Add(new GridColumn("Enabled", 55, HorizontalAlignment.Center));
			programsGrid.ListGridColumns.Add(new GridColumn("Program Name", 100) { IsWidthDynamic = true });
			programsGrid.ListGridRows.Clear();

			foreach (var program in programs)
			{
				var gridRow = new GridRow();

				gridRow.BackColor = program.Enabled ? Color.FromArgb(230, 255, 238) : gridRow.BackColor;
				gridRow.Cells.Add(program.Enabled ? "X" : "");
				gridRow.Cells.Add(program.Description);
				gridRow.Tag = program;

				programsGrid.ListGridRows.Add(gridRow);
			}

			programsGrid.EndUpdate();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			using (var formProgramLinkEdit = new FormProgramLinkEdit())
			{
				formProgramLinkEdit.IsNew = true;
				formProgramLinkEdit.ProgramCur = new Program();
				formProgramLinkEdit.ShowDialog();

				changed = true; // because we don't really know what they did, so assume changed.
			}

			FillList();
		}

		private void ProgramsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var dialogResult = DialogResult.None;

			Program program = programs[programsGrid.GetSelectedIndex()].Copy();
			switch (program.Name)
			{
				case "UAppoint":
					FormUAppoint FormU = new FormUAppoint();
					FormU.ProgramCur = program;
					dialogResult = FormU.ShowDialog();
					break;

				case "eRx":
					FormErxSetup FormES = new FormErxSetup();
					dialogResult = FormES.ShowDialog();
					break;

				case "Mountainside":
					FormMountainside FormM = new FormMountainside();
					FormM.ProgramCur = program;
					dialogResult = FormM.ShowDialog();
					break;

				case "PayConnect":
					FormPayConnectSetup fpcs = new FormPayConnectSetup();
					dialogResult = fpcs.ShowDialog();
					break;

				case "Podium":
					FormPodiumSetup FormPS = new FormPodiumSetup();
					dialogResult = FormPS.ShowDialog();
					break;
				case "Xcharge":
					FormXchargeSetup fxcs = new FormXchargeSetup();
					dialogResult = fxcs.ShowDialog();
					break;

				case "FHIR":
					FormFHIRSetup FormFS = new FormFHIRSetup();
					dialogResult = FormFS.ShowDialog();
					break;

				case "Transworld":
					FormTransworldSetup FormTs = new FormTransworldSetup();
					dialogResult = FormTs.ShowDialog();
					break;

				case "PaySimple":
					FormPaySimpleSetup formPS = new FormPaySimpleSetup();
					dialogResult = formPS.ShowDialog();
					break;

				case "XDR":
					FormXDRSetup FormXS = new FormXDRSetup();
					dialogResult = FormXS.ShowDialog();
					break;

				case "TrojanExpressCollect":
					using (FormTrojanCollectSetup FormTro = new FormTrojanCollectSetup())
					{
						dialogResult = FormTro.ShowDialog();
					}
					break;

				case "BencoPracticeManagement":
					FormBencoSetup FormBPM = new FormBencoSetup();
					dialogResult = FormBPM.ShowDialog();
					break;

				default:
					using (var formProgramLinkEdit = new FormProgramLinkEdit())
					{
						if (Programs.IsStatic(program))
						{
							formProgramLinkEdit.AllowToolbarChanges = false;
						}
						formProgramLinkEdit.ProgramCur = program;

						dialogResult = formProgramLinkEdit.ShowDialog(this);
					}
					break;
			}

			if (dialogResult == DialogResult.OK)
			{
				changed = true;

				FillList();
			}
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormProgramLinks_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (changed)
			{
				DataValid.SetInvalid(InvalidType.Programs, InvalidType.ToolButsAndMounts);
			}
		}
	}
}
