using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormImageSelect : FormBase
	{
		private readonly long patientId;

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the only display images.
		///		</para>
		///		<para>
		///			When true PDF files are excluded from the displayed list of documents.
		///		</para>
		/// </summary>
		public bool OnlyShowImages { get; set; }

		/// <summary>
		/// Gets the ID of the selected document. Returns 0 if no document was selected.
		/// </summary>
		public long SelectedDocumentId
        {
            get
            {
				if (imagesGrid.SelectedGridRows.Count > 0 &&
					imagesGrid.SelectedGridRows[0].Tag is Document document)
                {
					return document.DocNum;
                }

				return 0;
            }
        }

		public FormImageSelect(long patientId)
		{
			InitializeComponent();

			this.patientId = patientId;
		}

		private void FormImageSelect_Load(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void FillGrid()
		{
			imagesGrid.BeginUpdate();
			imagesGrid.ListGridColumns.Clear();
			imagesGrid.ListGridColumns.Add(new GridColumn("Date", 100));
			imagesGrid.ListGridColumns.Add(new GridColumn("Category", 120));
			imagesGrid.ListGridColumns.Add(new GridColumn("Description", 300));
			imagesGrid.ListGridRows.Clear();

			var documents = Documents.GetAllWithPat(patientId).ToList();
			if (OnlyShowImages)
			{
				documents = documents.Where(
					doc => Path.GetExtension(doc.FileName).ToLower() != ".pdf").ToList();
			}

			foreach (var document in documents)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(document.DateCreated.ToString());
				gridRow.Cells.Add(Defs.GetName(DefCat.ImageCats, document.DocCategory));
				gridRow.Cells.Add(document.Description);
				gridRow.Tag = document;

				imagesGrid.ListGridRows.Add(gridRow);
			}

			imagesGrid.EndUpdate();
		}

		private void ImagesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (imagesGrid.SelectedGridRows.Count > 0)
            {
				AcceptButton_Click(sender, EventArgs.Empty);
            }
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (imagesGrid.GetSelectedIndex() == -1)
			{
				MessageBox.Show("Please select an image first.");

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
