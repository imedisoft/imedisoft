using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormDunningSetup:ODForm {
		private List<Dunning> _listAllDunnings;
		private List<Definition> _listBillingTypeDefs;

		public FormDunningSetup() {
			InitializeComponent();
			
		}

		private void FormDunningSetup_Load(object sender,EventArgs e) {
			_listBillingTypeDefs=Definitions.GetDefsForCategory(DefinitionCategory.BillingTypes,true);
			listBill.Items.Add("("+"all"+")");
			listBill.SetSelected(0,true);
			listBill.Items.AddRange(_listBillingTypeDefs.Select(x => x.Name).ToArray());
			comboClinics.SelectedClinicNum=Clinics.Active.Id;
			FillGrids(true);
		}

		private void FillGrids(bool doRefreshList=false) {
			if(doRefreshList) {
				_listAllDunnings=Dunnings.Refresh(Clinics.GetByCurrentUser().Select(x => x.Id).ToList());
			}
			List<Dunning> listSubDunnings=_listAllDunnings.FindAll(x => ValidateDunningFilters(x));
			if(!Prefs.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				listSubDunnings.RemoveAll(x => x.IsSuperFamily);
			}
			gridDunning.BeginUpdate();
			gridDunning.ListGridColumns.Clear();
			gridDunning.ListGridColumns.Add(new GridColumn("Billing Type",80));
			gridDunning.ListGridColumns.Add(new GridColumn("Aging",70));
			gridDunning.ListGridColumns.Add(new GridColumn("Ins",40));
			gridDunning.ListGridColumns.Add(new GridColumn("Message",150));
			gridDunning.ListGridColumns.Add(new GridColumn("Bold Message",150));
			gridDunning.ListGridColumns.Add(new GridColumn("Email",35,HorizontalAlignment.Center));
			if(Prefs.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				gridDunning.ListGridColumns.Add(new GridColumn("SF",30,HorizontalAlignment.Center));
			}
			if(PrefC.HasClinicsEnabled) {
				gridDunning.ListGridColumns.Add(new GridColumn("Clinic",50));
			}
			gridDunning.ListGridRows.Clear();
			GridRow row;
			foreach(Dunning dunnCur in listSubDunnings) {
				row=new GridRow();
				if(dunnCur.BillingType==0){
					row.Cells.Add("all");
				}
				else{
					row.Cells.Add(Definitions.GetName(DefinitionCategory.BillingTypes,dunnCur.BillingType));
				}
				if(dunnCur.AgeAccount==0){
					row.Cells.Add("any");
				}
				else{
					row.Cells.Add("Over "+dunnCur.AgeAccount.ToString());
				}
				if(dunnCur.InsIsPending==YN.Yes) {
					row.Cells.Add("Y");
				}
				else if(dunnCur.InsIsPending==YN.No) {
					row.Cells.Add("N");
				}
				else {//YN.Unknown
					row.Cells.Add("any");
				}
				row.Cells.Add(dunnCur.DunMessage);
				row.Cells.Add(new GridCell(dunnCur.MessageBold) { Bold= true, ForeColor=Color.DarkRed });
				row.Cells.Add((!string.IsNullOrEmpty(dunnCur.EmailBody) || !string.IsNullOrEmpty(dunnCur.EmailSubject))?"X":"");
				if(Prefs.GetBool(PrefName.ShowFeatureSuperfamilies)) {
					row.Cells.Add(dunnCur.IsSuperFamily?"X":"");
				}
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(dunnCur.ClinicNum));
				}
				row.Tag=dunnCur;
				gridDunning.ListGridRows.Add(row);
			}
			gridDunning.EndUpdate();
		}

		private bool ValidateDunningFilters(Dunning dunning) {
			if((!comboClinics.IsAllSelected && comboClinics.SelectedClinicNum!=dunning.ClinicNum)
				||(!listBill.SelectedIndices.Contains(0) && !listBill.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x-1].Id).Contains(dunning.BillingType))
				||(!radioAny.Checked && dunning.AgeAccount!=(byte)(30*new List<RadioButton> { radioAny,radio30,radio60,radio90 }.FindIndex(x => x.Checked)))//0, 30, 60, or 90
				||(!string.IsNullOrWhiteSpace(textAdv.Text) && dunning.DaysInAdvance!=PIn.Int(textAdv.Text,false))//blank=0
				||(!radioU.Checked && dunning.InsIsPending!=(YN)new List<RadioButton> { radioU,radioY,radioN }.FindIndex(x => x.Checked)))//0=Unknown, 1=Yes, 2=No+
			{
				return false;
			}
			return true;
		}

		private void OnFilterChanged(object sender,EventArgs e) {
		 if(_listAllDunnings==null) {//Not initialized yet
			return;
		 }
			FillGrids();
		}

		private void gridDunning_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormDunningEdit formD=new FormDunningEdit((Dunning)gridDunning.ListGridRows[e.Row].Tag);
			if(formD.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillGrids(true);
		}

		private void butAdd_Click(object sender,EventArgs e) {
			Dunning dun=new Dunning();
			long clinicNum=0;
			if(!comboClinics.IsAllSelected) {
				clinicNum=comboClinics.SelectedClinicNum;
			}
			dun.ClinicNum=clinicNum;
			FormDunningEdit FormD=new FormDunningEdit(dun);
			FormD.IsNew=true;
			if(FormD.ShowDialog()==DialogResult.OK) {
				FillGrids(true);
			}
		}
		
		private void butDuplicate_Click(object sender,EventArgs e) {
			if(gridDunning.SelectedIndices.Count()==0) {
				MessageBox.Show("Please select a message to duplicate first.");
				return;
			}
			Dunning dun=((Dunning)gridDunning.ListGridRows[gridDunning.GetSelectedIndex()].Tag).Copy();
			Dunnings.Insert(dun);
			FillGrids(true);
			gridDunning.SetSelected(gridDunning.ListGridRows.OfType<GridRow>().ToList().FindIndex(x => ((Dunning)x.Tag).DunningNum==dun.DunningNum),true);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
		
	}
}
