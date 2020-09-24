using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Imedisoft.Forms
{
    public partial class FormEhrSummaryOfCare : FormBase
	{
		private readonly Patient patient;

		public FormEhrSummaryOfCare(Patient patient)
		{
			InitializeComponent();

			this.patient = patient;
		}

		private void FormSummaryOfCare_Load(object sender, EventArgs e)
		{
			FillGridSent();
			FillGridReceived();
		}

		private void FillGridSent()
		{
			sentGrid.BeginUpdate();
			sentGrid.Columns.Clear();
			sentGrid.Columns.Add(new GridColumn(Translation.Common.Timestamp, 130, HorizontalAlignment.Center));
			sentGrid.Columns.Add(new GridColumn(Translation.Common.Meets, 140, HorizontalAlignment.Center));
			sentGrid.Rows.Clear();

			var refAttaches = RefAttaches.GetRefAttachesForSummaryOfCareForPat(patient.PatNum);

			foreach (var measureEvent in EhrMeasureEvents.GetByPatient(patient.PatNum, EhrMeasureEventType.SummaryOfCareProvidedToDr))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(measureEvent.Date.ToString());
				gridRow.Cells.Add(refAttaches.Any(x => x.RefAttachNum == measureEvent.ObjectId) ? "X" : "");
				gridRow.Tag = measureEvent;

				sentGrid.Rows.Add(gridRow);
			}

			sentGrid.EndUpdate();
		}

		private void FillGridReceived()
		{
			receivedGrid.BeginUpdate();
			receivedGrid.Columns.Clear();
			receivedGrid.Columns.Add(new GridColumn(Translation.Common.Timestamp, 140, HorizontalAlignment.Center));
			receivedGrid.Rows.Clear();

			foreach (var ehrSummaryCcd in EhrSummaryCcds.Refresh(patient.PatNum))
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(ehrSummaryCcd.Date?.ToShortDateString());

				receivedGrid.Rows.Add(gridRow);
			}

			receivedGrid.EndUpdate();
		}

		private void SentGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var measureEvent = sentGrid.SelectedTag<EhrMeasureEvent>();
			if (measureEvent == null || measureEvent.ObjectId == null)
            {
				return;
            }

            using var formReferralsPatient = new FormReferralsPatient
            {
                DefaultRefAttachNum = measureEvent.ObjectId.Value,
                PatNum = patient.PatNum,
                IsSelectionMode = true
            };

            if (formReferralsPatient.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			measureEvent.ObjectId = formReferralsPatient.RefAttachNum;

			EhrMeasureEvents.Save(measureEvent);

			FillGridSent();
		}

		public static bool DisplayCCD(string ccdXml, Patient patient = null, string alternativeXslPath = null)
		{
			var document = new XmlDocument();
			try
			{
				document.LoadXml(ccdXml);
			}
			catch (Exception exception)
			{
				throw new Exception(Translation.Common.InvalidXml, exception);
			}

			string xmlFileName;
			string xslFileName;
			string xslContents;

			if (document.DocumentElement.Name.ToLower() == "clinicaldocument") // CCD, CCDA, and C32.
			{
				xmlFileName = "ccd.xml";
				xslFileName = "ccd.xsl";
				xslContents = FormEHR.GetEhrResource("CCD");

				if (string.IsNullOrEmpty(xslContents))
				{
					if (!string.IsNullOrEmpty(alternativeXslPath))
					{
						xslContents = File.ReadAllText(alternativeXslPath);
					}
				}

				if (string.IsNullOrEmpty(xslContents))
				{
					throw new Exception(Translation.Common.NoStylesheetFound);
				}
			}
			else if (document.DocumentElement.Name.ToLower() == "continuityofcarerecord" || document.DocumentElement.Name.ToLower() == "ccr:continuityofcarerecord")
			{
				// CCR
				xmlFileName = "ccr.xml";
				xslFileName = "ccr.xsl";
				xslContents = FormEHR.GetEhrResource("CCR");
			}
			else
			{
				System.Windows.MessageBox.Show(
					Translation.Ehr.InvalidSummaryOfCareOnlyRawTextWillBeShown, 
					Translation.Ehr.SummaryOfCare, 
					System.Windows.MessageBoxButton.OK, 
					System.Windows.MessageBoxImage.Error);

				using var msgBoxCopyPaste = new MsgBoxCopyPaste(ccdXml);

				msgBoxCopyPaste.ShowDialog();

				return false;
			}

			var node = document.SelectSingleNode("/processing-instruction(\"xml-stylesheet\")");
			if (node == null) // document does not contain any stylesheet instruction, so add one
			{
				document.InsertAfter(
					document.CreateProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"" + xslFileName + "\""),
					document.ChildNodes[0]);
			}
			else // alter the existing instruction
			{
				var processingInstruction = (XmlProcessingInstruction)node;
				processingInstruction.Value = "type=\"text/xsl\" href=\"" + xslFileName + "\"";
			}

			var pathTemp = Path.GetTempPath();
			var pathXml = Path.Combine(pathTemp, xmlFileName);
			var pathXsl = Path.Combine(pathTemp, xslFileName);

			File.WriteAllText(pathXml, document.InnerXml.ToString());
			File.WriteAllText(pathXsl, xslContents);

			using var formEhrSummaryOfCareEdit = new FormEhrSummaryOfCareEdit(pathXml, patient);

			formEhrSummaryOfCareEdit.ShowDialog();

			var fileNames = new string[] { pathXml, pathXsl };
			foreach (var fileName in fileNames)
			{
				try
				{
					File.Delete(fileName);
				}
				catch
				{
				}
			}

			return formEhrSummaryOfCareEdit.DidPrint;
		}

		private bool TryGenerateSummaryOfCare(Patient patient, out string ccd)
		{
			try
			{
				ccd = EhrCCD.GenerateSummaryOfCare(patient);

				return true;
			}
			catch (Exception exception)
			{
				ccd = "";

				ShowError(exception.Message);

				return false;
			}
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			if (!TryGenerateSummaryOfCare(patient, out var ccd))
            {
				return;
            }

            using var formReferralsPatient = new FormReferralsPatient
            {
                PatNum = patient.PatNum,
                IsSelectionMode = true
            };

            if (formReferralsPatient.ShowDialog() != DialogResult.OK)
			{
				ShowError(Translation.Ehr.SummaryOfCareNotExported);

				return;
			}

            using var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = ImageStore.GetPatientFolder(patient, OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath())
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

			EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.Now,
				Type = EhrMeasureEventType.SummaryOfCareProvidedToDr,
				PatientId = patient.PatNum,
				ObjectId = formReferralsPatient.RefAttachNum
			});

			EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.Now,
				Type = EhrMeasureEventType.SummaryOfCareProvidedToDrElectronic,
				PatientId = patient.PatNum,
				ObjectId = formReferralsPatient.RefAttachNum
			});

			FillGridSent();

			ShowInfo(Translation.Common.Exported + ".");
		}

		private void SendEmailButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.EmailSend))
			{
				return;
			}

			if (!TryGenerateSummaryOfCare(patient, out var ccd))
			{
				return;
			}

			using var formReferralsPatient = new FormReferralsPatient
            {
                PatNum = patient.PatNum,
                IsSelectionMode = true
            };

            if (formReferralsPatient.ShowDialog(this) != DialogResult.OK)
			{
				ShowError(Translation.Ehr.SummaryOfCareNotExported);

				return;
			}

			var emailAddress = EmailAddresses.GetByClinic(Clinics.Active.Id);

			var emailMessage = new EmailMessage
            {
                PatNum = patient.PatNum,
                MsgDateTime = DateTime.Now,
                SentOrReceived = EmailSentOrReceived.Neither,
                FromAddress = emailAddress?.SmtpUsername,
                ToAddress = "",
                Subject = Translation.Ehr.SummaryOfCare,
                BodyText = Translation.Ehr.SummaryOfCare
			};

            try
			{
				emailMessage.Attachments.Add(
					EmailAttaches.CreateAttach("ccd.xml", Encoding.UTF8.GetBytes(ccd)));

				emailMessage.Attachments.Add(
					EmailAttaches.CreateAttach("ccd.xsl", Encoding.UTF8.GetBytes(FormEHR.GetEhrResource("CCD"))));
			}
			catch (Exception exception)
			{
				Cursor = Cursors.Default;

				ShowError(exception.Message);

				return;
			}

			EmailMessages.Insert(emailMessage);

			using var formEmailMessageEdit = new FormEmailMessageEdit(emailMessage, emailAddress);
			if (formEmailMessageEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.Now,
				Type = EhrMeasureEventType.SummaryOfCareProvidedToDr,
				PatientId = patient.PatNum,
				ObjectId = formReferralsPatient.RefAttachNum
			});

			EhrMeasureEvents.Save(new EhrMeasureEvent
			{
				Date = DateTime.Now,
				Type = EhrMeasureEventType.SummaryOfCareProvidedToDrElectronic,
				PatientId = patient.PatNum,
				ObjectId = formReferralsPatient.RefAttachNum
			});

			FillGridSent();
		}

		private void ShowXhtmlButton_Click(object sender, EventArgs e)
		{
            using var formReferralsPatient = new FormReferralsPatient
            {
                PatNum = patient.PatNum,
                IsSelectionMode = true
            };

            if (formReferralsPatient.ShowDialog(this) != DialogResult.OK)
			{
				ShowError(Translation.Ehr.SummaryOfCareNotShown);

				return;
			}

			if (!TryGenerateSummaryOfCare(patient, out var ccd))
			{
				return;
			}

			bool didPrint = DisplayCCD(ccd);

			if (didPrint)
			{
                EhrMeasureEvents.Save(new EhrMeasureEvent
				{
					Date = DateTime.Now,
					Type = EhrMeasureEventType.SummaryOfCareProvidedToDr,
					ObjectId = formReferralsPatient.RefAttachNum,
					PatientId = patient.PatNum
				});

				FillGridSent();
			}
		}

		private void ShowXmlButton_Click(object sender, EventArgs e)
		{
			if (!TryGenerateSummaryOfCare(patient, out var ccd))
			{
				return;
			}

			using var msgBoxCopyPaste = new MsgBoxCopyPaste(ccd);

			msgBoxCopyPaste.ShowDialog(this);
		}

		private void ReceiveEmailButton_Click(object sender, EventArgs e)
		{
			using var formEmailInbox = new FormEmailInbox();

			formEmailInbox.ShowDialog(this);
		}

		private void ImportButton_Click(object sender, EventArgs e)
		{
			using var openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			var content = File.ReadAllText(openFileDialog.FileName);

            EhrSummaryCcds.Save(new EhrSummaryCcd
			{
				Content = content,
				Date = DateTime.Today,
				PatientId = patient.PatNum
			});

			FillGridReceived();

			DisplayCCD(content, patient);
		}

		private void SentGrid_SelectionCommitted(object sender, EventArgs e)
		{
			deleteButton.Enabled = sentGrid.SelectedRows.Count > 0;
		}

		private void ReceivedGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var ehrSummaryCcd = receivedGrid.SelectedTag<EhrSummaryCcd>();
			if (ehrSummaryCcd == null)
			{
				return;
			}

			DisplayCCD(ehrSummaryCcd.Content, patient);
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var ehrMeasureEvents = sentGrid.SelectedTags<EhrMeasureEvent>();
			if (ehrMeasureEvents.Count == 0)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItems))
            {
				return;
            }

			foreach (var ehrMeasureEvent in ehrMeasureEvents)
            {
				EhrMeasureEvents.Delete(ehrMeasureEvent.Id);
			}

			FillGridSent();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
