using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormClinics : FormBase
	{
		private readonly List<Clinic> clinics = new List<Clinic>();
		private Dictionary<long, int> clinicPatientCounts;
		private long? moveDestClinicId = null;

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the form is in selection mode.
		///		</para>
		///		<para>
		///			In selection mode certain UI elements are hidden.
		///		</para>
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user can select multiple clinics.
		/// </summary>
		public bool IsMultiSelect { get; set; }

		/// <summary>
		/// Gets the selected clinic.
		/// </summary>
		public Clinic SelectedClinic 
			=> clinicsGrid.SelectedTag<Clinic>();

		/// <summary>
		/// Gets the selected clinics.
		/// </summary>
		public List<Clinic> ListSelectedClinicNums 
			=> clinicsGrid.SelectedTags<Clinic>();

		public FormClinics(List<Clinic> clinics = null)
		{
			InitializeComponent();

			this.clinics.AddRange(clinics ?? Clinics.GetByCurrentUser());
		}

		private void FormClinics_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				addButton.Visible = false;
				acceptButton.Visible = true;
				moveGroupBox.Visible = false;

				int diff = moveGroupBox.Width - acceptButton.Width;

				MinimumSize = new Size(MinimumSize.Width - diff, MinimumSize.Height);
				Width -= diff;
				clinicsGrid.Width += diff;

				showHiddenCheckBox.Visible = false;
				showHiddenCheckBox.Checked = false;
			}
			else
			{
				clinicPatientCounts = Clinics.GetClinicalPatientCount();
			}

			if (IsMultiSelect)
			{
				selectAllButton.Visible = true;
				selectNoneButton.Visible = true;

				clinicsGrid.SelectionMode = GridSelectionMode.MultiExtended;
			}

			FillGrid();
		}

		private void FillGrid()
		{
			var selectedClinicsIds = clinicsGrid.SelectedTags<Clinic>().Select(x => x.Id).ToList();
			var selectedClinicIndices = new List<int>();

			clinicsGrid.BeginUpdate();
			clinicsGrid.Columns.Clear();
			clinicsGrid.Columns.Add(new GridColumn("Abbr", 120));
			clinicsGrid.Columns.Add(new GridColumn(Translation.Common.Description, 200));
			clinicsGrid.Columns.Add(new GridColumn("Specialty", 150));

			if (!IsSelectionMode)
			{
				clinicsGrid.Columns.Add(new GridColumn("Patients", 80, HorizontalAlignment.Center));
				clinicsGrid.Columns.Add(new GridColumn("Hidden", 40, HorizontalAlignment.Center) { IsWidthDynamic = true });
			}

			clinicsGrid.Rows.Clear();

			var clinicSpecialityDescriptions = Definitions.GetByCategory(DefinitionCategory.ClinicSpecialty, true).ToDictionary(x => x.Id, x => x.Name);
			var clinicSpecialityLinks = DefLinks.GetListByFKeys(clinics.Select(x => x.Id).ToList(), DefLinkType.Clinic);

			foreach (var clinic in clinics)
			{
				if (!showHiddenCheckBox.Checked && clinic.IsHidden)
				{
					continue;
				}

				var specialities = 
					clinicSpecialityLinks
						.Where(defLink => defLink.FKey == clinic.Id)
						.Select(defLink => clinicSpecialityDescriptions.TryGetValue(defLink.DefinitionId, out var speciality) ? speciality : "")
						.Where(speciality => !string.IsNullOrWhiteSpace(speciality));

				var gridRow = new GridRow();
				gridRow.Cells.Add(clinic.Abbr);
				gridRow.Cells.Add(clinic.Description);
				gridRow.Cells.Add(string.Join(",", specialities));

				if (!IsSelectionMode)
				{
					clinicPatientCounts.TryGetValue(clinic.Id, out var patientCount);

					gridRow.Cells.Add(patientCount.ToString());
					gridRow.Cells.Add(clinic.IsHidden ? "X" : "");
				}

				gridRow.Tag = clinic;

				clinicsGrid.Rows.Add(gridRow);

				if (selectedClinicsIds.Contains(clinic.Id))
				{
					selectedClinicIndices.Add(clinicsGrid.Rows.Count - 1);
				}
			}

			clinicsGrid.EndUpdate();

			if (selectedClinicIndices.Count > 0)
			{
				clinicsGrid.SetSelected(selectedClinicIndices.ToArray(), true);
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var clinic = new Clinic
            {
				IsMedicalOnly = Preferences.GetBool(PreferenceName.PracticeIsMedicalOnly)
			};

			using var formClinicEdit = new FormClinicEdit(clinic);
			if (formClinicEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			Clinics.Insert(clinic);

			clinics.Add(clinic);
			clinicPatientCounts[clinic.Id] = 0;

			FillGrid();
		}

		private void ClinicsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var clinic = clinicsGrid.SelectedTag<Clinic>();
			if (clinic == null)
            {
				return;
            }

			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);

				return;
			}

			using var formClinicEdit = new FormClinicEdit(clinic);
			if (formClinicEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			Clinics.Update(clinic);

			FillGrid();
		}

		private void PickMoveDestButton_Click(object sender, EventArgs e)
		{
            using var formClinics = new FormClinics(clinics)
            {
                IsSelectionMode = true
            };

            if (formClinics.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			moveDestClinicId = formClinics.SelectedClinic?.Id;
			moveDestTextBox.Text = clinics.FirstOrDefault(x => x.Id == moveDestClinicId)?.Abbr ?? "";
		}

		private void MoveButton_Click(object sender, EventArgs e)
		{
			var sourceClinics = clinicsGrid.SelectedTags<Clinic>();
			if (sourceClinics.Count == 0)
			{
				ShowError("You must select at least one clinic to move patients from.");

				return;
			}

			if (moveDestClinicId == null)
			{
				ShowError("You must pick a 'To' clinic in the box above to move patients to.");

				return;
			}

			var destClinic = clinics.FirstOrDefault(x => x.Id == moveDestClinicId);
			if (destClinic == null)
			{
				ShowError("The clinic could not be found.");
				return;
			}

			if (sourceClinics.Exists(x => x.Id == moveDestClinicId))
			{
				ShowError("The 'To' clinic should not also be one of the 'From' clinics.");

				return;
			}

			var sourceClinicDict = sourceClinics.ToDictionary(x => x.Id);
			var sourceClinicPatientCounts = Clinics.GetClinicalPatientCount(true).Where(x => sourceClinicDict.ContainsKey(x.Key)).ToList();

			var patientsToMove = sourceClinicPatientCounts.Sum(x => x.Value);
			if (patientsToMove == 0)
			{
				ShowError("There are no patients assigned to the selected clinics.");
				return;
			}

			var sourceClinicAbbrs = sourceClinicPatientCounts.Where(x => x.Value > 0).Select(x => sourceClinicDict[x.Key].Abbr);

			var prompt = 
				"This will move all patients to " + destClinic.Abbr + " from the following clinics:\r\n" + 
				string.Join("\r\n", sourceClinicAbbrs) + 
				"\r\nContinue?";
			
			if (!Confirm(prompt))
			{
				return;
			}

			int GetPatientCount(long clinicId)
            {
				lock (sourceClinicPatientCounts)
                {
					return sourceClinicPatientCounts.FirstOrDefault(x => x.Key == clinicId).Value;
				}
            }

			var patientsMoved = 0;

			ODProgress.ShowAction(() =>
				{
					var actions = sourceClinicDict.Select(x => x.Value).Select(sourceClinic => new Action(() =>
					{
						var patients = GetPatientCount(sourceClinic.Id);
						if (patients == 0)
						{
							return;
						}

						Patients.ChangeClinicsForAll(sourceClinic.Id, destClinic.Id);

						SecurityLogs.MakeLogEntry(Permissions.PatientEdit, 0, 
							"Clinic changed for patients from " + sourceClinic.Abbr + " to " + destClinic.Abbr + ".");

						patientsMoved += patients;

						ClinicEvent.Fire(EventCategory.Clinic,
							"Moved " + patientsMoved + " out of " + patientsToMove + " patients...");
					}));

					ODThread.RunParallel(actions.ToList(), TimeSpan.FromMinutes(2));
				},
				startingMessage: "Moving patients...",
				eventType: typeof(ClinicEvent),
				odEventType: EventCategory.Clinic);

			clinicPatientCounts = Clinics.GetClinicalPatientCount();

			FillGrid();

			ShowInfo("Done.");
		}

		private void ShowHiddenCheckBox_CheckedChanged(object sender, EventArgs e) 
			=> FillGrid();

		private void SelectAllButton_Click(object sender, EventArgs e)
		{
			clinicsGrid.SetSelected(true);
			clinicsGrid.Focus();
		}

		private void SelectNoneButton_Click(object sender, EventArgs e)
		{
			clinicsGrid.SetSelected(false);
			clinicsGrid.Focus();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				if (clinicsGrid.SelectedIndices.Length > 0)
				{
					DialogResult = DialogResult.OK;
				}

				return;
			}

			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e) 
			=> Close();
	}
}
