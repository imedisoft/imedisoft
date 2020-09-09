using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormFieldDefLink : FormBase
	{
		private List<FieldDefLink> _listFieldDefLinks;
		private List<AppointmentFieldDefinition> _listApptFieldDefs;
		private List<PatFieldDef> _listPatFieldDefs;
		private FieldLocations _fieldLocation;

		public FormFieldDefLink(FieldLocations fieldLocation = FieldLocations.Account)
		{
			InitializeComponent();

			_fieldLocation = fieldLocation;
		}

		private void FormFieldDefLink_Load(object sender, EventArgs e)
		{
			string[] arrayFieldLocations = Enum.GetNames(typeof(FieldLocations));
			for (int i = 0; i < arrayFieldLocations.Length; i++)
			{
				locationComboBox.Items.Add(arrayFieldLocations[i]);
				if (i == (int)_fieldLocation)
				{
					locationComboBox.SelectedIndex = i;
				}
			}
			_listFieldDefLinks = FieldDefLinks.GetAll();
			_listApptFieldDefs = AppointmentFieldDefinitions.All;
			_listPatFieldDefs = PatFieldDefs.GetDeepCopy(true);
			FillGridDisplayed();
			FillGridHidden();
		}

		///<summary>Fills the displayed grid with all field defs that should show for the location selected.
		///This should be enhanced in the future to include indication rows when there is a potential for one location to serve multiple types.</summary>
		private void FillGridDisplayed()
		{
			//Find all FieldDefLinks for the currently selected location, then find the apptfield/patfield for each, to display.
			int selectedIdx = displayedGrid.GetSelectedIndex();
			displayedGrid.BeginUpdate();
			displayedGrid.Columns.Clear();
			displayedGrid.Columns.Add(new GridColumn("", 20) { IsWidthDynamic = true });
			displayedGrid.Rows.Clear();

			GridRow row;
			switch ((FieldLocations)locationComboBox.SelectedIndex)
			{
				case FieldLocations.Account:
				case FieldLocations.Chart:
				case FieldLocations.Family:
				case FieldLocations.GroupNote:
				case FieldLocations.OrthoChart:
					foreach (PatFieldDef patField in _listPatFieldDefs)
					{
						if (_listFieldDefLinks.Exists(x => x.FieldDefNum == patField.Id
							 && x.FieldLocation == (FieldLocations)locationComboBox.SelectedIndex
							 && x.FieldDefType == FieldDefTypes.Patient))
						{
							continue; //If there is already a link for the patfield for the selected location, don't display the patfield in "display" grid.
						}
						row = new GridRow();
						row.Cells.Add(patField.FieldName);
						row.Tag = patField;
						displayedGrid.Rows.Add(row);
					}
					break;

				case FieldLocations.AppointmentEdit://AppointmentEdit is the only place where ApptFields are used.
					foreach (AppointmentFieldDefinition apptField in _listApptFieldDefs)
					{
						if (_listFieldDefLinks.Exists(x => x.FieldDefNum == apptField.Id
							 && x.FieldLocation == (FieldLocations)locationComboBox.SelectedIndex
							 && x.FieldDefType == FieldDefTypes.Appointment))
						{
							continue; //If there is already a link for the apptfield for the selected location, don't display the apptfield in "display" grid.
						}
						row = new GridRow();
						row.Cells.Add(apptField.Name);
						row.Tag = apptField;
						displayedGrid.Rows.Add(row);
					}
					break;
			}
			displayedGrid.EndUpdate();
			if (displayedGrid.Rows.Count - 1 >= selectedIdx)
			{
				displayedGrid.SetSelected(selectedIdx, true);
			}
		}

		///<summary>Fills the hidden grid with all hidden field defs for the location selected.
		///This should be enhanced in the future to include indication rows when there is a potential for one location to serve multiple types.</summary>
		private void FillGridHidden()
		{
			int selectedIdx = hiddenGrid.GetSelectedIndex();
			hiddenGrid.BeginUpdate();
			hiddenGrid.Columns.Clear();
			hiddenGrid.Columns.Add(new GridColumn("", 20) { IsWidthDynamic = true });
			hiddenGrid.Rows.Clear();
			GridRow row;
			List<FieldDefLink> listFieldDefLinksForLoc = _listFieldDefLinks.FindAll(x => x.FieldLocation == (FieldLocations)locationComboBox.SelectedIndex);
			List<FieldDefLink> listLinksToDelete = new List<FieldDefLink>();//Some links could exists to deleted FieldDefs prior to 18.1
			foreach (FieldDefLink fieldDefLink in listFieldDefLinksForLoc)
			{
				switch (fieldDefLink.FieldDefType)
				{
					case FieldDefTypes.Patient:
						PatFieldDef patFieldDef = _listPatFieldDefs.Find(x => x.Id == fieldDefLink.FieldDefNum);
						if (patFieldDef == null)
						{//orphanded FK link to deleted PatFieldDef
							listLinksToDelete.Add(fieldDefLink);//orphanded FK link to deleted PatFieldDef
							continue;
						}
						row = new GridRow();
						row.Cells.Add(patFieldDef.FieldName);
						row.Tag = fieldDefLink;
						hiddenGrid.Rows.Add(row);
						break;

					case FieldDefTypes.Appointment:
						AppointmentFieldDefinition apptFieldDef = _listApptFieldDefs.Find(x => x.Id == fieldDefLink.FieldDefNum);
						if (apptFieldDef == null)
						{//orphaned FK link to deleted ApptFieldDef
							listLinksToDelete.Add(fieldDefLink);//orphaned FK link to deleted ApptFieldDef
							continue;
						}
						row = new GridRow();
						row.Cells.Add(apptFieldDef.Name);
						row.Tag = fieldDefLink;
						hiddenGrid.Rows.Add(row);
						break;
				}
			}

			//Remove all orphaned links
			foreach (FieldDefLink fieldDefLink in listLinksToDelete)
			{
				_listFieldDefLinks.Remove(fieldDefLink);
			}

			hiddenGrid.EndUpdate();
			if (hiddenGrid.Rows.Count - 1 >= selectedIdx)
			{
				hiddenGrid.SetSelected(selectedIdx, true);
			}
		}

		private void LocationComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGridDisplayed();
			FillGridHidden();
		}

		private void RightButton_Click(object sender, EventArgs e)
		{
			// Add the selected field def from the display grid to the _listFieldDefLinks as a new link. Refill grid.
			if (displayedGrid.GetSelectedIndex() < 0)
			{
				return; // Button pressed with nothing selected.
			}

			FieldDefLink fieldDefLink = new FieldDefLink();
			switch ((FieldLocations)locationComboBox.SelectedIndex)
			{
				case FieldLocations.Account:
				case FieldLocations.Chart:
				case FieldLocations.Family:
				case FieldLocations.GroupNote:
				case FieldLocations.OrthoChart:
					PatFieldDef patFieldDef = (PatFieldDef)displayedGrid.Rows[displayedGrid.GetSelectedIndex()].Tag;
					fieldDefLink = new FieldDefLink();
					fieldDefLink.FieldDefNum = patFieldDef.Id;
					fieldDefLink.FieldDefType = FieldDefTypes.Patient;
					fieldDefLink.FieldLocation = (FieldLocations)locationComboBox.SelectedIndex;
					_listFieldDefLinks.Add(fieldDefLink);
					break;

				case FieldLocations.AppointmentEdit://AppointmentEdit is the only place where ApptFields are used.
					AppointmentFieldDefinition apptFieldDef = (AppointmentFieldDefinition)displayedGrid.Rows[displayedGrid.GetSelectedIndex()].Tag;
					fieldDefLink = new FieldDefLink();
					fieldDefLink.FieldDefNum = apptFieldDef.Id;
					fieldDefLink.FieldDefType = FieldDefTypes.Appointment;
					fieldDefLink.FieldLocation = (FieldLocations)locationComboBox.SelectedIndex;
					_listFieldDefLinks.Add(fieldDefLink);
					break;
			}
			FillGridDisplayed();
			FillGridHidden();
		}

		private void LeftButton_Click(object sender, EventArgs e)
		{
			// Remove the selected field def from the hidden grid.
			if (hiddenGrid.SelectedIndices.Length < 1)
			{
				// Nothing selected.
				return;
			}

			_listFieldDefLinks.Remove((FieldDefLink)hiddenGrid.Rows[hiddenGrid.GetSelectedIndex()].Tag);
			FillGridDisplayed();
			FillGridHidden();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			FieldDefLinks.Sync(_listFieldDefLinks);

			DialogResult = DialogResult.OK;
		}
	}
}
