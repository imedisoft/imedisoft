using Imedisoft.Data.Cache;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormApptTypes : FormBase
	{
		private List<AppointmentType> appointmentTypes;


		///<summary>Stale deep copy of _listApptTypes to use with sync.</summary>
		private List<AppointmentType> _listApptTypesOld;


		/// <summary>
		/// Gets or sets a value indicating whether 'None' is allowed option.
		/// </summary>
		public bool IsNoneAllowed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the form is in select mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether multiple appointment types may be selected.
		///		</para>
		///		<para>
		///			Only applicable if <see cref="IsSelectionMode"/> is true.
		///		</para>
		/// </summary>
		public bool AllowMultipleSelections { get; set; }




		///<summary>The appointment type that was selected if IsSelectionMode is true.
		///If IsSelectionMode is true and this object is prefilled with an appointment type then the grid will preselect that type if possible.
		///It is not guaranteed that the appointment type will be selected.
		///This object should only be read from externally after DialogResult.OK has been returned.  Can be null.</summary></summary>
		public AppointmentType SelectedAptType;

		///<summary>Contains all of the selected appointment types if IsSelectionMode is true.
		///If IsSelectionMode and AllowMultiple are true, this object can be prefilled with appointment types which will be preselected if possible.
		///It is not guaranteed that all appointment types will be selected (due to hidden).
		///This list should only be read from externally after DialogResult.OK has been returned.</summary>
		public List<AppointmentType> ListSelectedApptTypes = new List<AppointmentType>();



		public FormApptTypes()
		{
			InitializeComponent();

			appointmentTypes = new List<AppointmentType>();
		}

		private void FormApptTypes_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				acceptButton.Visible = true;
				addButton.Visible = false;
				downButton.Visible = false;
				upButton.Visible = false;
				warnCheckBox.Visible = false;
				promptCheckBox.Visible = false;

				if (AllowMultipleSelections)
				{
					Text = "Select Appointment Types";

					appointmentTypesGrid.SelectionMode = GridSelectionMode.MultiExtended;
				}
				else
				{
					Text = "Select Appointment Type";
				}
			}

			promptCheckBox.Checked = Prefs.GetBool(PrefName.AppointmentTypeShowPrompt);
			warnCheckBox.Checked = Prefs.GetBool(PrefName.AppointmentTypeShowWarning);

			// Don't show hidden appointment types in selection mode
			appointmentTypes = AppointmentTypes.GetDeepCopy(IsSelectionMode);
			_listApptTypesOld = AppointmentTypes.GetDeepCopy();
			FillMain();

			// Preselect the corresponding appointment type(s) once on load.  Do not do this within FillMain().
			if (IsSelectionMode)
			{
				if (SelectedAptType != null)
				{
					ListSelectedApptTypes.Add(SelectedAptType);
				}

				for (int i = 0; i < appointmentTypesGrid.ListGridRows.Count; i++)
				{
					if (((AppointmentType)appointmentTypesGrid.ListGridRows[i].Tag) != null //The "None" option will always be null
						&& ListSelectedApptTypes.Any(x => x.Id == ((AppointmentType)appointmentTypesGrid.ListGridRows[i].Tag).Id))
					{
						appointmentTypesGrid.SetSelected(i, true);
					}
				}
			}
		}

		private void FillMain()
		{
			appointmentTypesGrid.BeginUpdate();
			appointmentTypesGrid.ListGridColumns.Clear();
			appointmentTypesGrid.ListGridColumns.Add(new GridColumn("Name", 200));
			appointmentTypesGrid.ListGridColumns.Add(new GridColumn("Color", 50, HorizontalAlignment.Center));
			appointmentTypesGrid.ListGridColumns.Add(new GridColumn("Hidden", 60, HorizontalAlignment.Center) { IsWidthDynamic = true });
			appointmentTypesGrid.ListGridRows.Clear();

			appointmentTypes.Sort(AppointmentTypes.SortItemOrder);
			foreach (var appointmentType in appointmentTypes)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(appointmentType.Name);
				gridRow.Cells.Add("");
				gridRow.Cells[1].BackColor = appointmentType.Color;
				gridRow.Cells.Add(appointmentType.Hidden ? "X" : "");
				gridRow.Tag = appointmentType;

				appointmentTypesGrid.ListGridRows.Add(gridRow);
			}

			// Always add a None option to the end of the list when in selection mode.
			if (IsNoneAllowed)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add("None");
				gridRow.Cells.Add("");
				gridRow.Cells.Add("");
				appointmentTypesGrid.ListGridRows.Add(gridRow);
			}

			appointmentTypesGrid.EndUpdate();
		}

		private void UpButton_Click(object sender, EventArgs e)
		{
			if (appointmentTypesGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select an item in the grid first.");

				return;
			}

			if (appointmentTypesGrid.GetSelectedIndex() == 0)
			{
				// Do nothing, the item is at the top of the list.
				return;
			}

			int index = appointmentTypesGrid.GetSelectedIndex();

			appointmentTypes[index - 1].ItemOrder += 1;
			appointmentTypes[index].ItemOrder -= 1;

			FillMain();

			index -= 1;
			appointmentTypesGrid.SetSelected(index, true);
		}

		private void DownButton_Click(object sender, EventArgs e)
		{
			if (appointmentTypesGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select an item in the grid first.");

				return;
			}

			if (appointmentTypesGrid.GetSelectedIndex() == appointmentTypes.Count - 1)
			{
				// Do nothing, the item is at the bottom of the list.
				return;
			}

			int index = appointmentTypesGrid.GetSelectedIndex();

			appointmentTypes[index + 1].ItemOrder -= 1;
			appointmentTypes[index].ItemOrder += 1;
			FillMain();

			index += 1;

			appointmentTypesGrid.SetSelected(index, true);
		}

		private void AppointmentTypesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				AcceptButton_Click(sender, EventArgs.Empty);
			}
			else
			{
				var appointmentType = appointmentTypes[e.Row];

				using var formApptTypeEdit = new FormApptTypeEdit(appointmentType);

				if (formApptTypeEdit.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				if (appointmentType.Id == 0)
				{
					appointmentTypes.RemoveAt(e.Row);
				}
				else
				{
					appointmentTypes[e.Row] = appointmentType;
				}

				FillMain();
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var appointmentType = new AppointmentType
			{
				ItemOrder = appointmentTypes.Count,
				IsNew = true,
				Color = Color.White
			};

			using var formApptTypeEdit = new FormApptTypeEdit(appointmentType);

            if (formApptTypeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			appointmentTypes.Add(appointmentType);

			FillMain();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			ListSelectedApptTypes = appointmentTypesGrid.SelectedTags<AppointmentType>();

			SelectedAptType = ListSelectedApptTypes.FirstOrDefault();

			DialogResult = DialogResult.OK;
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				DialogResult = DialogResult.Cancel;
			}

			Close();
		}

		private async void FormApptTypes_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!IsSelectionMode)
			{
				Prefs.Set(PrefName.AppointmentTypeShowPrompt, promptCheckBox.Checked);
				Prefs.Set(PrefName.AppointmentTypeShowWarning, warnCheckBox.Checked);

				for (int i = 0; i < appointmentTypes.Count; i++)
				{
					appointmentTypes[i].ItemOrder = i;
				}

				AppointmentTypes.Sync(appointmentTypes, _listApptTypesOld);

				await CacheManager.RefreshAsync(
					nameof(InvalidType.AppointmentTypes),
					nameof(InvalidType.Prefs));

				DialogResult = DialogResult.OK;
			}
		}
	}
}
