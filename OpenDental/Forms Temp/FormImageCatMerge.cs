using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Forms;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
	public partial class FormImageCatMerge : ODForm
	{
		private long _defNumInto;
		private long _defNumFrom;

		public FormImageCatMerge()
		{
			InitializeComponent();

		}

		private void buttonChangeInto_Click(object sender, EventArgs e)
		{
			using var formDefinitionPicker = new FormDefinitionPicker(DefinitionCategory.ImageCats);

			if (formDefinitionPicker.ShowDialog(this) == DialogResult.OK)
			{
				textBoxInto.Text = formDefinitionPicker.SelectedDefinition.Name;

				_defNumInto = formDefinitionPicker.SelectedDefinition.Id;

				CheckUIState();
			}
		}

		private void buttonChangeFrom_Click(object sender, EventArgs e)
		{
			using var formDefinitionPicker = new FormDefinitionPicker(DefinitionCategory.ImageCats);

			if (formDefinitionPicker.ShowDialog(this) == DialogResult.OK)
			{
				textBoxFrom.Text = formDefinitionPicker.SelectedDefinition.Name;

				_defNumFrom = formDefinitionPicker.SelectedDefinition.Id;

				CheckUIState();
			}
		}

		//Double check the state and status of the form.
		private void CheckUIState()
		{
			butMerge.Enabled = (textBoxFrom.Text.Trim() != "" && textBoxInto.Text.Trim() != "");
		}

		private void butMerge_Click(object sender, EventArgs e)
		{
			if (_defNumInto == _defNumFrom)
			{
				MessageBox.Show("Cannot merge the same Image Category. Please update either the merge Into field or the merge From field.");
				return;
			}
			if (!MsgBox.Show(MsgBoxButtons.YesNo, "Are you sure? The results are permanent and cannot be undone."))
			{
				return;
			}
			try
			{
				Definitions.MergeImageCatDefNums(_defNumFrom, _defNumInto);
			}
			catch (Exception ex)
			{
				FriendlyException.Show("Image Categories failed to merge.", ex);
				return;
			}
			Definitions.Hide(Definitions.GetDef(DefinitionCategory.ImageCats, _defNumFrom));
			DataValid.SetInvalid(InvalidType.Defs);
			MessageBox.Show("Image Categories merged successfully.");
			string logText = "Image Category Merge from"
				+ " " + Definitions.GetName(DefinitionCategory.ImageCats, _defNumFrom) + " " + "to" + " " + Definitions.GetName(DefinitionCategory.ImageCats, _defNumInto);
			//Make log entry here.
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, logText);
			textBoxFrom.Clear();
			textBoxInto.Clear();
			CheckUIState();
		}

		private void butClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
