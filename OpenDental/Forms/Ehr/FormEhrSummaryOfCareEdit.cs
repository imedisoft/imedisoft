using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Imedisoft.Forms
{
    public partial class FormEhrSummaryOfCareEdit : FormBase
	{
		private readonly Patient patient;
		private readonly string xmlFilePath;

		public bool DidPrint { get; private set; }

		public FormEhrSummaryOfCareEdit(string xmlFilePath, Patient patient)
		{
			InitializeComponent();

			this.xmlFilePath = xmlFilePath;
			this.patient = patient;
		}

		private void FormEhrSummaryOfCareEdit_Load(object sender, EventArgs e)
		{
			if (patient == null || patient.PatNum == 0) // No patient is currently selected. Do not show reconcile UI.
			{
				reconcileLabel.Visible = false;
				reconcileAllergiesButton.Visible = false;
				reconcileMedicationsButton.Visible = false;
				reconcileProblemsButton.Visible = false;
			}

			Cursor = Cursors.WaitCursor;

			webBrowser.Url = new Uri(xmlFilePath);

			Cursor = Cursors.Default;
		}

		private bool TryLoadXml(out XmlDocument document)
        {
			document = new XmlDocument();

			try
			{
				document.LoadXml(File.ReadAllText(xmlFilePath));

				return true;
			}
			catch (Exception ex)
			{
				ShowError(Translation.Common.ErrorReadingFile + " " + ex.Message);

				return false;
			}
		}

		private void ReconcileMedicationsButton_Click(object sender, EventArgs e)
		{
			if (!TryLoadXml(out var xmlDocCcd)) return;

            using var formReconcileMedication = new FormReconcileMedication(patient)
            {
                ListMedicationPatNew = new List<MedicationPat>()
            };

            EhrCCD.GetListMedicationPats(xmlDocCcd, formReconcileMedication.ListMedicationPatNew);

			formReconcileMedication.ShowDialog(this);
		}

		private void ReconcileProblemsButton_Click(object sender, EventArgs e)
		{
			if (!TryLoadXml(out var xmlDocCcd)) return;

            using var formReconcileProblem = new FormReconcileProblem(patient)
            {
                ListProblemNew = new List<Problem>(),
                ListProblemDefNew = new List<ProblemDefinition>()
            };

            EhrCCD.GetListDiseases(xmlDocCcd, formReconcileProblem.ListProblemNew, formReconcileProblem.ListProblemDefNew);

			formReconcileProblem.ShowDialog(this);
		}

		private void ReconcileAllergiesButton_Click(object sender, EventArgs e)
		{
			if (!TryLoadXml(out var xmlDocCcd)) return;

            using var formReconcileAllergy = new FormReconcileAllergy(patient)
            {
                ListAllergyNew = new List<Allergy>(),
                ListAllergyDefNew = new List<AllergyDef>()
            };

            EhrCCD.GetListAllergies(xmlDocCcd, 
				formReconcileAllergy.ListAllergyNew, 
				formReconcileAllergy.ListAllergyDefNew);

			formReconcileAllergy.ShowDialog(this);
		}

		private void ShowXmlButton_Click(object sender, EventArgs e)
		{
			var ccd = File.ReadAllText(xmlFilePath)
				.Replace("\r\n", "")
				.Replace("\n", "")
				.Replace("\r", "")
				.Replace(">", ">\r\n");

			using var msgBoxCopyPaste = new MsgBoxCopyPaste(ccd);
			msgBoxCopyPaste.ShowDialog();
		}

		private void PrintButton_Click(object sender, EventArgs e)
		{
			webBrowser.ShowPrintDialog();

			DidPrint = true;
		}
	}
}
