using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using Imedisoft.Data;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormWikiListHistory:ODForm {
		///<summary>Set from outside this form to load all appropriate data into the form during Form_Load().</summary>
		public string ListNameCur;
		public bool IsReverted;

		public FormWikiListHistory() {
			InitializeComponent();
			
		}

		private void FormWikiListHistory_Load(object sender,EventArgs e) {
			FillGridMain();
			if(gridMain.Rows.Count>0) {
				gridMain.SetSelected(gridMain.Rows.Count-1,true);
				gridMain.ScrollToEnd();
			}
			Text="Wiki List History - "+ListNameCur;
			gridOld.Title="Old Revision";
			gridCur.Title="Current Revision";
			FillGridOld();
			FillGridCur();
		}

		private void FormWikiListHistory_Resize(object sender,EventArgs e) {
			int workingWidth=butRevert.Left-gridMain.Right-18;//18 for 3x6 px between gridMain-gridOld, gridOld-gridCur, and gridCur-butRevert
			gridOld.Width=workingWidth/2;
			gridCur.Left=gridOld.Right+6;
			gridCur.Width=workingWidth-gridOld.Width;
		}

		/// <summary></summary>
		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			gridMain.Columns.AddRange(new[] { new GridColumn("User",70),new GridColumn("Saved",80) });
			gridMain.Rows.AddRange(WikiListHists.GetByNameNoContent(ListNameCur)
				.Select(x => new GridRow(Users.GetUserName(x.UserNum),x.DateTimeSaved.ToString()) { Tag=x }));
			gridMain.EndUpdate();
		}

		/// <summary></summary>
		private void FillGridOld() {
			gridOld.BeginUpdate();
			gridOld.Columns.Clear();
			gridOld.Rows.Clear();
			if(gridMain.GetSelectedIndex()<0) {
				gridOld.EndUpdate();
				return;
			}
			if(string.IsNullOrEmpty(gridMain.SelectedTag<WikiListHist>()?.ListHeaders)) {
				gridMain.SelectedRows[0].Tag=WikiListHists.SelectOne(gridMain.SelectedTag<WikiListHist>()?.WikiListHistNum??0);
			}
			Dictionary<string,int> dictColWidths=WikiListHeaderWidths.GetFromListHist(gridMain.SelectedTag<WikiListHist>())
				.ToDictionary(x => x.ColName,x => x.ColWidth);
			using(DataTable table=new DataTable()) {
				using(StringReader sr=new StringReader(gridMain.SelectedTag<WikiListHist>().ListContent))
				using(XmlReader xmlReader=XmlReader.Create(sr)) {
					try {
						table.ReadXml(xmlReader);
					}
					catch(Exception) {
						MessageBox.Show("Corruption detected in the Old Revision table.  Partial data will be displayed.  Please call us for support.");
						gridOld.EndUpdate();
						return;
					}
				}
				gridOld.Columns.AddRange(
					table.Columns.OfType<DataColumn>().Select(x => new GridColumn(x.ColumnName,dictColWidths.TryGetValue(x.ColumnName,out int width)?width:100)));
				gridOld.Rows.AddRange(table.Select().Select(x => new GridRow(x.ItemArray.Select(y => y.ToString()).ToArray())));
			}
			gridOld.EndUpdate();
		}

		/// <summary></summary>
		private void FillGridCur() {
			gridCur.BeginUpdate();
			gridCur.Columns.Clear();
			gridCur.Rows.Clear();
			Dictionary<string,int> dictColWidths=WikiListHeaderWidths.GetForList(ListNameCur).ToDictionary(x => x.ColName,x => x.ColWidth);
			using(DataTable table=WikiLists.GetByName(ListNameCur)) {
				gridCur.Columns.AddRange(
					table.Columns.OfType<DataColumn>().Select(x => new GridColumn(x.ColumnName,dictColWidths.TryGetValue(x.ColumnName,out int width)?width:100)));
				gridCur.Rows.AddRange(table.Select().Select(x => new GridRow(x.ItemArray.Select(y => y.ToString()).ToArray())));
			}			
			gridCur.EndUpdate();
		}

		private void gridMain_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			FillGridOld();
			gridMain.Focus();
		}

		private void butRevert_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Revert list to currently selected revision?")) {
				return;
			}
			try {
				WikiListHists.RevertFrom(gridMain.SelectedTag<WikiListHist>(),Security.CurrentUser.Id);
			}
			catch(Exception) {
				MessageBox.Show("There was an error when trying to revert changes.  Please call us for support.");
				return;
			}
			FillGridMain();
			gridMain.SetSelected(gridMain.Rows.Count-1,true);//select the new revision.
			gridMain.ScrollToEnd();//in case there are LOTS of revisions. Should this go in the fill grid code? 
			FillGridOld();
			FillGridCur();
			gridMain.Focus();
			IsReverted=true;
		}

	}
}