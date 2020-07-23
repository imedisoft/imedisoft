using OpenDental.UI;
using OpenDentBusiness.Services.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormFeatureRequest : FormBase
	{
		private List<FeatureRequestDto> featureRequests;
		private List<FeatureRequestDto> featureRequestsFiltered;

		public FormFeatureRequest() => InitializeComponent();

		private void FormFeatureRequest_Load(object sender, EventArgs e)
		{
			editButton.Visible = false;

			FillGrid();
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			requestsGrid.SetSelected(false);

			Search();
		}

		private void Search()
		{
			// TODO: Make web service call the retreive the requests...

			FillGrid();
		}

		private void FillGrid()
		{
			featureRequestsFiltered = new List<FeatureRequestDto>(featureRequests);
			featureRequestsFiltered.Sort(SortFeatureRequests);

			long selectedRequestId = 0;

			int selectedIndex = requestsGrid.GetSelectedIndex();
			if (selectedIndex != -1)
			{
				if (requestsGrid.ListGridRows[selectedIndex].Tag is FeatureRequestDto featureRequest)
				{
					selectedRequestId = featureRequest.Id;
				}
			}

			requestsGrid.BeginUpdate();
			requestsGrid.ListGridColumns.Clear();
			requestsGrid.ListGridColumns.Add(new GridColumn("Req#", 40, GridSortingStrategy.AmountParse));
			requestsGrid.ListGridColumns.Add(new GridColumn("Mine", 40, GridSortingStrategy.StringCompare));
			requestsGrid.ListGridColumns.Add(new GridColumn("Comments", 70, GridSortingStrategy.AmountParse));
			requestsGrid.ListGridColumns.Add(new GridColumn("Version", 75, GridSortingStrategy.VersionNumber));
			requestsGrid.ListGridColumns.Add(new GridColumn("Diff", 40, GridSortingStrategy.AmountParse));
			requestsGrid.ListGridColumns.Add(new GridColumn("Weight", 45, HorizontalAlignment.Right, GridSortingStrategy.AmountParse));
			requestsGrid.ListGridColumns.Add(new GridColumn("Status", 90, GridSortingStrategy.StringCompare));
			requestsGrid.ListGridColumns.Add(new GridColumn("Description", 500, GridSortingStrategy.StringCompare));
			requestsGrid.ListGridRows.Clear();

			foreach (var featureRequest in featureRequestsFiltered)
            {
				if (mineCheckBox.Checked && !featureRequest.IsMine)
					continue;

				var gridRow = new GridRow();
				gridRow.Cells.Add(featureRequest.Id.ToString());
				gridRow.Cells.Add(featureRequest.IsMine ? "X" : "");
				gridRow.Cells.Add(featureRequest.TotalComments.ToString());
				gridRow.Cells.Add(featureRequest.VersionCompleted);
				gridRow.Cells.Add(featureRequest.Difficulty.ToString());
				gridRow.Cells.Add(featureRequest.Weight.ToString("F"));
				gridRow.Cells.Add(FeatureRequestDto.GetStatusString(featureRequest.Status));
				gridRow.Cells.Add(featureRequest.Description);

				// If they voted or pledged on this feature, mark it so they can see.
				if (featureRequest.IsMine && !featureRequest.IsCritical && featureRequest.Pledge == 0 &&
					featureRequest.Status != FeatureRequestStatus.Complete)
				{
					gridRow.ColorBackG = Color.FromArgb(255, 255, 230); // Light yellow.
				}

				requestsGrid.ListGridRows.Add(gridRow);
				gridRow.Tag = featureRequest;
			}

			requestsGrid.EndUpdate();

			if (selectedRequestId > 0)
            {
				for (int i = 0; i < requestsGrid.ListGridRows.Count; ++i)
                {
					if (requestsGrid.ListGridRows[i].Tag is FeatureRequestDto featureRequest && featureRequest.Id == selectedRequestId)
                    {
						requestsGrid.SetSelected(i, true);

						break;
                    }
                }
            }
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			string warning = 
				"The majority of feature requests that users submit are duplicates of existing requests. " +
				"Please take the time to do a thorough search for different keywords and become familiar with similar requests before adding one of your own. " +
				"Continue?";

			if (Prompt(warning) != DialogResult.No)
			{
				return;
			}

			var featureRequest = new FeatureRequestDto();
			using (var formRequestEdit = new FormRequestEdit(featureRequest))
			{
				formRequestEdit.ShowDialog(this);
			}

			searchTextBox.Text = "";

			Search();
		}

		private void RequestsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
            if (!(requestsGrid.ListGridRows[e.Row].Tag is FeatureRequestDto featureRequest)) return;

			using (var formRequestEdit = new FormRequestEdit(featureRequest))
			{
				formRequestEdit.ShowDialog(this);
			}

			Search();
		}

		private void MineCheckBox_CheckedChanged(object sender, EventArgs e) 
			=> FillGrid();

		private void MyVotesCheckBox_CheckedChanged(object sender, EventArgs e) 
			=> FillGrid();

		private void EditButton_Click(object sender, EventArgs e)
		{
			var index = requestsGrid.GetSelectedIndex();
			if (index == -1)
			{
				ShowInfo("Please select a feature request.");

				return;
			}

            if (!(requestsGrid.ListGridRows[index].Tag is FeatureRequestDto featureRequest))
            {
                return;
            }

            using (var formRequestEdit = new FormRequestEdit(featureRequest))
			{
				formRequestEdit.ShowDialog(this);
			}

			Search();
		}

		private void CancelButton_Click(object sender, EventArgs e) => Close();

		private int SortFeatureRequests(FeatureRequestDto x, FeatureRequestDto y)
		{
			// Sort by status...
			if (x.Status != y.Status)
            {
				return x.Status.CompareTo(y.Status);
            }

			// Move personal items towards the top.
			var xPledge = x.Pledge > 0 || x.IsCritical;
			var yPledge = y.Pledge > 0 || y.IsCritical;
			if (xPledge != yPledge) return xPledge.CompareTo(yPledge);

			// Move items with larger number of votes to to the top.
			if (xPledge && yPledge)
            {
				if (x.Weight != y.Weight)
                {
					return -x.Weight.CompareTo(y.Weight);
                }
            }

			if (x.IsMine != y.IsMine) return x.IsMine ? 1 : -1;

			// Sort by weight...
			if (x.Weight != y.Weight)
			{
				return -x.Weight.CompareTo(y.Weight);
			}

			// Finally sort by ID.
			return x.Id.CompareTo(y.Id);
		}
    }
}
