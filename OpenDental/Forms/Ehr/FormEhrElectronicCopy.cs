using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEhrElectronicCopy : FormBase
	{
		private readonly Patient patient;

		public FormEhrElectronicCopy(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormElectronicCopy_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			measureEventsGrid.BeginUpdate();
			measureEventsGrid.Columns.Clear();
			measureEventsGrid.Columns.Add(new GridColumn(Translation.Common.Timestamp, 140));
			measureEventsGrid.Columns.Add(new GridColumn(Translation.Common.Type, 600));
			measureEventsGrid.Rows.Clear();

			var measureEvents = EhrMeasureEvents.GetByPatient(patient.PatNum, 
				EhrMeasureEventType.ElectronicCopyRequested, 
				EhrMeasureEventType.ElectronicCopyProvidedToPt);
			
			foreach (var measureEvent in measureEvents)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(measureEvent.Date.ToString());

				switch (measureEvent.Type)
				{
					case EhrMeasureEventType.ElectronicCopyRequested:
						gridRow.Cells.Add(Translation.Ehr.RequestedByPatient);
						break;
					case EhrMeasureEventType.ElectronicCopyProvidedToPt:
						gridRow.Cells.Add(Translation.Ehr.ProvidedToPatient);
						break;
				}

				gridRow.Tag = measureEvent;

				measureEventsGrid.Rows.Add(gridRow);
			}

			measureEventsGrid.EndUpdate();
		}

		private void RequestButton_Click(object sender, EventArgs e)
		{
            EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.Now.AddMinutes(-1),
				Type = EhrMeasureEventType.ElectronicCopyRequested,
				PatientId = patient.PatNum,
				MoreInfo = ""
			});

			FillGrid();
		}

		private void RecordRequestAndProvide()
		{
			var requestMinDate = DateTime.Today.AddDays(-5);
			var requestExists = measureEventsGrid.GetTags<EhrMeasureEvent>().Any(
				evt => evt.Type == EhrMeasureEventType.ElectronicCopyRequested && evt.Date.Date >= requestMinDate);

			if (!requestExists)
			{
                EhrMeasureEvents.Save(new EhrMeasureEvent
				{
					Date = DateTime.Now.AddMinutes(-1),
					Type = EhrMeasureEventType.ElectronicCopyRequested,
					PatientId = patient.PatNum,
					MoreInfo = ""
				});
			}

            EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.Now,
				Type = EhrMeasureEventType.ElectronicCopyProvidedToPt,
				PatientId = patient.PatNum,
				MoreInfo = ""
			});

			FillGrid();
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			string ccd;

			try
			{
				ccd = EhrCCD.GenerateElectronicCopy(patient);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

            using var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = ImageStore.GetPatientFolder(patient, OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath())//Default to patient image folder.
            };

			if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			if (File.Exists(Path.Combine(folderBrowserDialog.SelectedPath, "ccd.xml")))
			{
				if (Prompt(Translation.Common.ConfirmOverwriteFile, MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
					return;
                }
			}

			File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, "ccd.xml"), ccd);
			File.WriteAllText(Path.Combine(folderBrowserDialog.SelectedPath, "ccd.xsl"), FormEHR.GetEhrResource("CCD"));

			RecordRequestAndProvide();

			ShowInfo("Exported.");
		}

		private void EmailButton_Click(object sender, EventArgs e)
		{
			ShowInfo(
				"Electronic copies cannot be emailed to patients due to security concerns.\r\n" +
				"Instruct the patient to access their information in the patient portal.\r\n" +
				"If you are trying to send the patient information directly to another provider, then go to Chart | EHR | Send/Receive summary of care.");
		}

		private void ShowXhtml_Click(object sender, EventArgs e)
		{
			string ccd;

			try
			{
				ccd = EhrCCD.GenerateElectronicCopy(patient);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			FormEhrSummaryOfCare.DisplayCCD(ccd);
		}

		private void ShowXml_Click(object sender, EventArgs e)
		{
			string ccd;

			try
			{
				ccd = EhrCCD.GenerateElectronicCopy(patient);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			using var msgBoxCopyPaste = new MsgBoxCopyPaste(ccd);

			msgBoxCopyPaste.ShowDialog(this);
		}

		private void MeasureEventsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			deleteButton.Enabled = measureEventsGrid.SelectedRows.Count > 0;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var measureEvents = measureEventsGrid.SelectedTags<EhrMeasureEvent>();
			if (measureEvents.Count == 0)
			{
				return;
			}

			foreach (var measureEvent in measureEvents)
            {
				EhrMeasureEvents.Delete(measureEvent.Id);
			}

			FillGrid();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
