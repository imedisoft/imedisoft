using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.IO;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrVaccines : FormBase
	{
		private readonly Patient patient;
		private EhrPatient ehrPatient;

		public FormEhrVaccines(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormVaccines_Load(object sender, EventArgs e)
		{
			ehrPatient = EhrPatients.GetById(patient.PatNum);

			switch (ehrPatient.AllowShareVaccines)
            {
				case true:
					exportYesRadioButton.Checked = true;
					break;

				case false:
					exportNoRadioButton.Checked = true;
					break;

				default:
					exportUnknownRadioButton.Checked = true;
					break;
            }

			FillGridVaccine();
		}

		private void FillGridVaccine()
		{
			vaccinesGrid.BeginUpdate();
			vaccinesGrid.Columns.Clear();
			vaccinesGrid.Columns.Add(new GridColumn(Translation.Common.Date, 90));
			vaccinesGrid.Columns.Add(new GridColumn(Translation.Ehr.Vaccine, 100));
			vaccinesGrid.Rows.Clear();

			foreach (var patientVaccine in EhrPatientVaccines.GetByPatient(patient.PatNum))
			{
				string vaccineName;
				if (patientVaccine.VaccineId.HasValue)
				{
					vaccineName = string.Format(Translation.Ehr.NotAdministeredName, patientVaccine.Note);
				}
				else
				{
					vaccineName = EhrVaccines.GetById(patientVaccine.VaccineId.Value).Name;
				}

				var gridRow = new GridRow();
				gridRow.Cells.Add(patientVaccine.DateStart?.ToShortDateString());
				gridRow.Cells.Add(vaccineName);
				gridRow.Tag = patientVaccine;

				vaccinesGrid.Rows.Add(gridRow);
			}

			vaccinesGrid.EndUpdate();
		}

		private void ExportRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			bool? allowShare = null;
			if (exportYesRadioButton.Checked) allowShare = true;
			if (exportNoRadioButton.Checked) allowShare = false;

			if (ehrPatient.AllowShareVaccines != allowShare)
			{
				ehrPatient.AllowShareVaccines = allowShare;

				EhrPatients.Update(ehrPatient);
			}
		}

		private void VaccinesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void VaccinesGrid_SelectionCommitted(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled = vaccinesGrid.SelectedRows.Count > 0;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var patientVaccine = new EhrPatientVaccine
            {
                PatientId = patient.PatNum,
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now
            };

            using var formEhrVaccinePatEdit = new FormEhrPatientVaccineEdit(patientVaccine);
			if (formEhrVaccinePatEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGridVaccine();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var patientVaccine = vaccinesGrid.SelectedTag<EhrPatientVaccine>();
			if (patientVaccine == null)
			{
				return;
			}

			using var formEhrVaccinePatEdit = new FormEhrPatientVaccineEdit(patientVaccine);
			if (formEhrVaccinePatEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGridVaccine();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var patientVaccines = vaccinesGrid.SelectedTags<EhrPatientVaccine>();
			if (patientVaccines.Count == 0)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItems))
            {
				return;
            }

			foreach (var patientVaccine in patientVaccines)
            {
				EhrPatientVaccines.Delete(patientVaccine);
            }
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			var patientVaccines = vaccinesGrid.SelectedTags<EhrPatientVaccine>();
			if (patientVaccines.Count == 0)
			{
				ShowError(Translation.Ehr.PleaseSelectAtLeastOneVaccine);

				return;
			}

			OpenDentBusiness.HL7.EhrVXU vxu;
			try
			{
				vxu = new OpenDentBusiness.HL7.EhrVXU(patient, patientVaccines);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			string outputStr = vxu.GenerateMessage();

            using var saveFileDialog = new SaveFileDialog
            {
                FileName = "vxu.txt"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			if (File.Exists(saveFileDialog.FileName))
			{
				if (Prompt(Translation.Common.ConfirmOverwriteFile, MessageBoxButtons.OKCancel) != DialogResult.OK)
				{
					return;
				}
			}

			File.WriteAllText(saveFileDialog.FileName, outputStr);

			ShowInfo(Translation.Common.Saved);
		}

		private void SubmitButton_Click(object sender, EventArgs e)
		{
			var patientVaccines = vaccinesGrid.SelectedTags<EhrPatientVaccine>();
			if (patientVaccines.Count == 0)
			{
				ShowError(Translation.Ehr.PleaseSelectAtLeastOneVaccine);

				return;
			}

			OpenDentBusiness.HL7.EhrVXU vxu;
			try
			{
				vxu = new OpenDentBusiness.HL7.EhrVXU(patient, patientVaccines);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			string outputStr = vxu.GenerateMessage();

			Cursor = Cursors.WaitCursor;

			try
			{
				EmailMessages.SendTestUnsecure(Translation.Ehr.ImmunizationSubmission, "vxu.txt", outputStr);
			}
			catch (Exception exception)
			{
				Cursor = Cursors.Default;

				ShowError(exception.Message);

				return;
			}

			Cursor = Cursors.Default;

			ShowInfo(Translation.Common.Sent);
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
