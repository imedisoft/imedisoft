using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNoteResponsePicker : FormBase
	{
		public string AutoNoteResponseText { get; private set; }

		public FormAutoNoteResponsePicker()
		{
			InitializeComponent();
		}

		private void FormAutoNoteResponsePicker_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			AutoNotes.RefreshCache();

			autoNotesGrid.BeginUpdate();
			autoNotesGrid.Columns.Clear();
			autoNotesGrid.Columns.Add(new GridColumn("", 100));
			autoNotesGrid.Rows.Clear();
	
			foreach (var autoNote in AutoNotes.GetAll())
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(autoNote.Name);
				gridRow.Tag = autoNote;

				autoNotesGrid.Rows.Add(gridRow);
			}

			autoNotesGrid.EndUpdate();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var responseText = responseTextTextBox.Text.Trim();
			if (responseText.Length == 0)
			{
				ShowError(Translation.Common.PleaseEnterResponseText);

				return;
			}

			var autoNote = autoNotesGrid.SelectedTag<AutoNote>();
			if (autoNote == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			AutoNoteResponseText = responseTextTextBox.Text + " : {" + autoNote.Name + "}";

			DialogResult = DialogResult.OK;
		}
	}
}
