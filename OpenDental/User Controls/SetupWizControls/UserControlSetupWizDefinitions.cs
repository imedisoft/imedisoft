using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;
using System.IO;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizDefinitions:SetupWizControl {
		private List<Definition> _listDefsAll;
		private bool _isDefChanged;

		///<summary>Gets the currently selected DefCat along with its options.</summary>
		private DefCatOptions _selectedDefCatOpt {
			get { return listCategory.GetSelected<DefCatOptions>(); }
		}

		///<summary>All definitions for the current category, hidden and non-hidden.</summary>
		private List<Definition> _listDefsCur {
			get { return _listDefsAll.Where(x => x.Category == _selectedDefCatOpt.DefCat).OrderBy(x => x.SortOrder).ToList(); }
		}

		public UserControlSetupWizDefinitions() {
			InitializeComponent();
			this.OnControlDone += ControlDone;
		}

		private void UserControlSetupWizDefinitions_Load(object sender,EventArgs e) {
			IsDone=true;//this is optional, so the user is done whenever they choose
			List<string> listDefCats=new List<string>();
			//Only including the most important categories so the user is not intimidated with all the options.
			listDefCats.Add(DefinitionCategory.AccountColors);
			listDefCats.Add(DefinitionCategory.AdjTypes);
			listDefCats.Add(DefinitionCategory.AppointmentColors);
			listDefCats.Add(DefinitionCategory.ApptConfirmed);
			listDefCats.Add(DefinitionCategory.ApptProcsQuickAdd);
			listDefCats.Add(DefinitionCategory.AutoNoteCats);
			listDefCats.Add(DefinitionCategory.BillingTypes);
			listDefCats.Add(DefinitionCategory.BlockoutTypes);
			listDefCats.Add(DefinitionCategory.ChartGraphicColors);
			listDefCats.Add(DefinitionCategory.CommLogTypes);
			listDefCats.Add(DefinitionCategory.ImageCats);
			listDefCats.Add(DefinitionCategory.PaymentTypes);
			listDefCats.Add(DefinitionCategory.ProcCodeCats);
			listDefCats.Add(DefinitionCategory.RecallUnschedStatus);
			listDefCats.Add(DefinitionCategory.TxPriorities);
			List<DefCatOptions> listDefCatsOrdered = new List<DefCatOptions>();
			listDefCatsOrdered=DefL.GetOptionsForDefCats(listDefCats.ToArray());
			listDefCatsOrdered = listDefCatsOrdered.OrderBy(x => x.Description).ToList(); //orders alphabetically.
			ODBoxItem<DefCatOptions> defCatItem;
			foreach(DefCatOptions defCOpt in listDefCatsOrdered) {
				defCatItem=new ODBoxItem<DefCatOptions>(defCOpt.Description,defCOpt);
				listCategory.Items.Add(defCatItem);
				if(defCOpt.DefCat==listDefCatsOrdered[0].DefCat) {
					listCategory.SelectedItem=defCatItem;
				}
			}
		}
		
		private void FillGridDefs(){
			if(_listDefsAll == null || _listDefsAll.Count == 0) {
				RefreshDefs();
			}
			DefL.FillGridDefs(gridDefs,_selectedDefCatOpt,_listDefsCur);
			//the following do not require a refresh of the table:
			if(_selectedDefCatOpt.CanHide) {
				butHide.Visible=true;
			}
			else {
				butHide.Visible=false;
			}
			if(_selectedDefCatOpt.CanEditName) {
				groupEdit.Enabled=true;
				groupEdit.Text="Edit Items";
			}
			else {
				groupEdit.Enabled=false;
				groupEdit.Text="Not allowed";
			}
			textGuide.Text=_selectedDefCatOpt.HelpText;
		}

		private void gridDefs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Definition selectedDef = (Definition)gridDefs.ListGridRows[e.Row].Tag;
			_isDefChanged=DefL.GridDefsDoubleClick(selectedDef,_selectedDefCatOpt,_listDefsCur,_listDefsAll,_isDefChanged);
			if(_isDefChanged) {
				RefreshDefs();
				FillGridDefs();
			}
		}
			

		private void butAdd_Click(object sender,EventArgs e) {
			if(DefL.AddDef(gridDefs,_selectedDefCatOpt)) {
				RefreshDefs();
				FillGridDefs();
				_isDefChanged=true;
			}
		}

		private void listCategory_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridDefs();
		}

		private void RefreshDefs() {
			Definitions.RefreshCache();
			_listDefsAll = Definitions.GetAll();
		}

		private void butHide_Click(object sender,EventArgs e) {
			if(DefL.TryHideDefSelectedInGrid(gridDefs,_selectedDefCatOpt)) {
				RefreshDefs();
				FillGridDefs();
				_isDefChanged=true;
			}
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(DefL.UpClick(gridDefs)) {
				_isDefChanged=true;
				FillGridDefs();
			}
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(DefL.DownClick(gridDefs)){
				_isDefChanged=true;
				FillGridDefs();
			}
		}
		
		private void ControlDone(object sender, EventArgs e) {
			//Correct the item orders of all definition categories.
			List<Definition> listDefUpdates=new List<Definition>();
			foreach(KeyValuePair<string,List<Definition>> kvp in Definitions.GetDictionaryNoCache()) {
				for(int i=0;i<kvp.Value.Count;i++) {
					if(kvp.Value[i].SortOrder!=i) {
						kvp.Value[i].SortOrder=i;
						listDefUpdates.Add(kvp.Value[i]);
					}
				}
			}
			listDefUpdates.ForEach(x => Definitions.Update(x));
			if(_isDefChanged || listDefUpdates.Count>0) {
				DataValid.SetInvalid(InvalidType.Defs);
			}
		}
		
	}
}
