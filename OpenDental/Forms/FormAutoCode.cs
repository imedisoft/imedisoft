using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoCode : FormBase
	{
		private bool hasChanges;

		public FormAutoCode() 
			=> InitializeComponent();

		private void FormAutoCode_Load(object sender, EventArgs e) 
			=> FillList();

		private void FillList()
		{
			AutoCodes.RefreshCache();

			var autoCodes = AutoCodes.GetListDeep();

			autoCodesListBox.Items.Clear();
			foreach (var autoCode in autoCodes)
			{
				autoCodesListBox.Items.Add(autoCode);
			}
		}

		private void AutoCodesListBox_DoubleClick(object sender, EventArgs e)
		{
			EditButton_Click(this, EventArgs.Empty);
		}

		private void AutoCodesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			editButton.Enabled = deleteButton.Enabled 
				= autoCodesListBox.SelectedItem != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var autoCode = new AutoCode();

			using var formAutoCodeEdit = new FormAutoCodeEdit
			{
				IsNew = true,
				AutoCodeCur = autoCode
			};

			AutoCodes.Save(formAutoCodeEdit.AutoCodeCur);

			if (formAutoCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillList();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			if (!(autoCodesListBox.SelectedItem is AutoCode autoCode))
			{
				return;
			}

			using var formAutoCodeEdit = new FormAutoCodeEdit
			{
				AutoCodeCur = autoCode
			};

			if (formAutoCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillList();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
            if (!(autoCodesListBox.SelectedItem is AutoCode autoCode))
            {
                return;
            }

            try
			{
				AutoCodes.Delete(autoCode);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			hasChanges = true;

			FillList();
		}

		private void CloseButton_Click(object sender, EventArgs e)
			=> Close();

		private void FormAutoCode_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.AutoCodes));
			}
		}
    }
}
