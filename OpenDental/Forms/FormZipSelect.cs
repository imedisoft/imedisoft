using Imedisoft.Data.Cache;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormZipSelect : FormBase
	{
		private string zipCodeDigits;
		private bool hasChanges;

		/// <summary>
		/// Gets the selected zip code.
		/// </summary>
		public ZipCode SelectedZipCode 
			=> zipCodesListBox.SelectedItem as ZipCode;

		public FormZipSelect() 
			=> InitializeComponent();

		public FormZipSelect(string zipCodeDigits) : this()
		{
			this.zipCodeDigits = zipCodeDigits;
		}

		private void FormZipSelect_Load(object sender, EventArgs e) 
			=> FillList();

		private void FillList()
		{
			ZipCodes.RefreshCache();

			zipCodesListBox.Items.Clear();

			var zipCodes = ZipCodes.GetDeepCopy();
			if (!string.IsNullOrWhiteSpace(zipCodeDigits))
			{
				zipCodes.RemoveAll(x => x.Digits != zipCodeDigits);
			}

			foreach (var zipCode in zipCodes)
			{
				zipCodesListBox.Items.Add(zipCode);
			}

			if (string.IsNullOrEmpty(zipCodeDigits) && zipCodes.Count > 0)
			{
				zipCodeDigits = zipCodes[0].Digits;
			}

			zipCodesListBox.SelectedIndex = -1;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
            var zipCode = new ZipCode
            {
                Digits = zipCodeDigits
            };

            using var formZipCodeEdit = new FormZipCodeEdit(zipCode);

			if (formZipCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			FillList();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var zipCode = SelectedZipCode;
			if (zipCode == null)
            {
                MessageBox.Show(Translation.Common.PleaseSelectItemFirst);

                return;
            }

            using var formZipCodeEdit = new FormZipCodeEdit(zipCode);

			if (formZipCodeEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			hasChanges = true;

			zipCodesListBox.Invalidate(
				zipCodesListBox.GetItemRectangle(
					zipCodesListBox.SelectedIndex));
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var zipCode = SelectedZipCode;
			if (zipCode == null)
			{
                MessageBox.Show(Translation.Common.PleaseSelectItemFirst);

                return;
            }

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
			{
				return;
			}

			ZipCodes.Delete(zipCode);

			zipCodesListBox.Items.Remove(zipCode);

			hasChanges = true;
		}

		private void ZipCodesListBox_DoubleClick(object sender, EventArgs e)
		{
			if (zipCodesListBox.SelectedIndex == -1)
			{
				return;
			}

			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void ZipCodesListBox_SelectedIndexChanged(object sender, EventArgs e) 
			=> editButton.Enabled = deleteButton.Enabled 
				= zipCodesListBox.SelectedItem != null;

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var zipCode = SelectedZipCode;
			if (zipCode == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void FormZipSelect_Closing(object sender, CancelEventArgs e)
		{
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.ZipCodes));
			}
		}
    }
}
