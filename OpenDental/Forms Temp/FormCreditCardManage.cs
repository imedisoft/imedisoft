using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Text.RegularExpressions;
using OpenDental.Bridges;
using CodeBase;
using OpenDentBusiness.IO;
using Imedisoft.Forms;
using Imedisoft.UI;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormCreditCardManage:ODForm {
		private Patient PatCur;
		private List<CreditCard> _listCreditCards;

		public FormCreditCardManage(Patient pat) {
			InitializeComponent();
			
			PatCur=pat;
		}
		
		private void FormCreditCardManage_Load(object sender,EventArgs e) {
			if(Preferences.GetBool(PreferenceName.StoreCCnumbers)
				&& (Programs.IsEnabled(ProgramName.Xcharge) 
					|| Programs.IsEnabled(ProgramName.PayConnect) 
					|| Programs.IsEnabled(ProgramName.PaySimple)))//tokens supported by Xcharge and PayConnect
			{
				labelStoreCCNumWarning.Visible=true;
			}
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new GridColumn("Card Number",140));
			if(Programs.IsEnabled(ProgramName.Xcharge)) {
				gridMain.Columns.Add(new GridColumn("X-Charge",70,HorizontalAlignment.Center));
			}
			if(Programs.IsEnabled(ProgramName.PayConnect)) {
				gridMain.Columns.Add(new GridColumn("PayConnect",85,HorizontalAlignment.Center));
			}
			if(Programs.IsEnabled(ProgramName.PaySimple)) {
				gridMain.Columns.Add(new GridColumn("PaySimple",80,HorizontalAlignment.Center));
				gridMain.Columns.Add(new GridColumn("ACH",40,HorizontalAlignment.Center));
			}
			if(PrefC.HasOnlinePaymentEnabled(out ProgramName progNameForPayments)) {
				if(progNameForPayments==ProgramName.Xcharge) {
					gridMain.Columns.Add(new GridColumn("XWeb",45,HorizontalAlignment.Center));
				}
				else {
					gridMain.Columns.Add(new GridColumn("PayConnect\r\nPortal",85,HorizontalAlignment.Center));
				}
			}
			if(gridMain.Columns.Sum(x => x.ColumnWidth) > gridMain.Width) {
				gridMain.HScrollVisible=true;
			}
			gridMain.Rows.Clear();
			GridRow row;
			_listCreditCards=CreditCards.Refresh(PatCur.PatNum);
			foreach(CreditCard cc in _listCreditCards) {
				row=new GridRow();
				string ccNum=cc.CCNumberMasked;
				if(Regex.IsMatch(ccNum,"^\\d{12}(\\d{0,7})")) {	//Credit cards can have a minimum of 12 digits, maximum of 19
					int idxLast4Digits=(ccNum.Length-4);
					ccNum=(new string('X',12))+ccNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
				}
				row.Cells.Add(ccNum);
				if(Programs.IsEnabled(ProgramName.Xcharge)) {
					row.Cells.Add(!string.IsNullOrEmpty(cc.XChargeToken) && !cc.IsXWeb()?"X":"");
				}
				if(Programs.IsEnabled(ProgramName.PayConnect)) {
					row.Cells.Add(!string.IsNullOrEmpty(cc.PayConnectToken) && !cc.IsPayConnectPortal() ? "X" : "");
				}
				if(Programs.IsEnabled(ProgramName.PaySimple)) {
					row.Cells.Add(!string.IsNullOrEmpty(cc.PaySimpleToken) ? "X" : "");
					row.Cells.Add(cc.CCSource==CreditCardSource.PaySimpleACH ? "X" : "");
				}
				if(PrefC.HasOnlinePaymentEnabled(out progNameForPayments)) {
					if(progNameForPayments==ProgramName.Xcharge) {
						row.Cells.Add(!string.IsNullOrEmpty(cc.XChargeToken) && cc.IsXWeb() ? "X" : "");
					}
					else {//PayConnectPortal
						row.Cells.Add(!string.IsNullOrEmpty(cc.PayConnectToken) && cc.IsPayConnectPortal() ? "X" : "");
					}
				}
				row.Tag=cc;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormCreditCardEdit FormCCE=new FormCreditCardEdit(PatCur);
			FormCCE.CreditCardCur=(CreditCard)gridMain.Rows[e.Row].Tag;
			FormCCE.ShowDialog();
			FillGrid();
			if(gridMain.Rows.Count>0) {//could have deleted the only CC, make sure there's at least one row
				int indexCC=gridMain.Rows.OfType<GridRow>().ToList().FindIndex(x => ((CreditCard)x.Tag).CreditCardNum==FormCCE.CreditCardCur.CreditCardNum);
				gridMain.SetSelected(Math.Max(0,indexCC),true);
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(!Preferences.GetBool(PreferenceName.StoreCCnumbers)) {
				bool hasXCharge=false;
				bool hasPayConnect=false;
				bool hasPaySimple=false;
				Dictionary<string,int> dictEnabledProcessors=new Dictionary<string,int>();
				int idx=0;
				bool hasXChargePreventCcAdd=PIn.Bool(ProgramProperties.GetPropVal(Programs.GetCur(ProgramName.Xcharge).Id,
					ProgramProperties.PropertyDescs.XCharge.XChargePreventSavingNewCC,Clinics.Active.Id));
				if(Programs.IsEnabled(ProgramName.Xcharge) && !hasXChargePreventCcAdd) {
					dictEnabledProcessors["X-Charge"]=idx++;
				}
				bool hasPayConnectPreventCcAdd=PIn.Bool(ProgramProperties.GetPropVal(Programs.GetCur(ProgramName.PayConnect).Id,
					PayConnect.ProgramProperties.PayConnectPreventSavingNewCC,Clinics.Active.Id));
				if(Programs.IsEnabled(ProgramName.PayConnect) && !hasPayConnectPreventCcAdd) {
					dictEnabledProcessors["PayConnect"]=idx++;
				}
				bool hasPaySimplePreventCCAdd=PIn.Bool(ProgramProperties.GetPropVal(Programs.GetCur(ProgramName.PaySimple).Id,
					PaySimple.PropertyDescs.PaySimplePreventSavingNewCC,Clinics.Active.Id));
				if(Programs.IsEnabled(ProgramName.PaySimple) && !hasPaySimplePreventCCAdd) {
					dictEnabledProcessors["PaySimple"]=idx++;
				}
				if(dictEnabledProcessors.Count>1) {
					List<string> listCCProcessors=dictEnabledProcessors.Select(x => x.Key).ToList();
					InputBox chooseProcessor=
						new InputBox("For which credit card processing company would you like to add this card?",listCCProcessors,true);
					if(chooseProcessor.ShowDialog()==DialogResult.Cancel) {
						return;
					}
					hasXCharge=dictEnabledProcessors.ContainsKey("X-Charge") && chooseProcessor.SelectedIndices.Contains(dictEnabledProcessors["X-Charge"]);
					hasPayConnect=dictEnabledProcessors.ContainsKey("PayConnect") && chooseProcessor.SelectedIndices.Contains(dictEnabledProcessors["PayConnect"]);
					hasPaySimple=dictEnabledProcessors.ContainsKey("PaySimple") && chooseProcessor.SelectedIndices.Contains(dictEnabledProcessors["PaySimple"]);
				}
				else if(Programs.IsEnabled(ProgramName.Xcharge) && !hasXChargePreventCcAdd) {
					hasXCharge=true;
				}
				else if(Programs.IsEnabled(ProgramName.PayConnect) && !hasPayConnectPreventCcAdd) {
					hasPayConnect=true;
				}
				else if(Programs.IsEnabled(ProgramName.PaySimple) && !hasPaySimplePreventCCAdd) {
					hasPaySimple=true;
				}
				else {//not storing CC numbers and both PayConnect and X-Charge are disabled
					MessageBox.Show("Not allowed to store credit cards.");
					return;
				}
				CreditCard creditCardCur=null;
				if(hasXCharge) {
					Program prog=Programs.GetCur(ProgramName.Xcharge);
					string path=Programs.GetProgramPath(prog);
					string xUsername=ProgramProperties.GetPropVal(prog.Id,"Username",Clinics.Active.Id).Trim();
					string xPassword=ProgramProperties.GetPropVal(prog.Id,"Password",Clinics.Active.Id).Trim();
					//Force user to retry entering information until it's correct or they press cancel
					while(!File.Exists(path) || string.IsNullOrEmpty(xPassword) || string.IsNullOrEmpty(xUsername)) {
						MessageBox.Show("The Path, Username, and/or Password for X-Charge have not been set or are invalid.");
						if(!Security.IsAuthorized(Permissions.Setup)) {
							return;
						}
						FormXchargeSetup FormX=new FormXchargeSetup();//refreshes program and program property caches on OK click
						FormX.ShowDialog();
						if(FormX.DialogResult!=DialogResult.OK) {//if user presses cancel, return
							return;
						}
						prog=Programs.GetCur(ProgramName.Xcharge);//refresh local variable prog to reflect any changes made in setup window
						path=Programs.GetProgramPath(prog);
						xUsername=ProgramProperties.GetPropVal(prog.Id,"Username",Clinics.Active.Id).Trim();
						xPassword=ProgramProperties.GetPropVal(prog.Id,"Password",Clinics.Active.Id).Trim();
					}
					xPassword=CodeBase.MiscUtils.Decrypt(xPassword);
					ProcessStartInfo info=new ProcessStartInfo(path);
					string resultfile=Storage.GetTempFileName(".txt");
					try {
						File.Delete(resultfile);//delete the old result file.
					}
					catch {
						MessageBox.Show("Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have sufficient permissions.");
						return;
					}
					info.Arguments="";
					info.Arguments+="/TRANSACTIONTYPE:ArchiveVaultAdd /LOCKTRANTYPE ";
					info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
					info.Arguments+="/USERID:"+xUsername+" ";
					info.Arguments+="/PASSWORD:"+xPassword+" ";
					info.Arguments+="/VALIDATEARCHIVEVAULTACCOUNT ";
					info.Arguments+="/STAYONTOP ";
					info.Arguments+="/SMARTAUTOPROCESS ";
					info.Arguments+="/AUTOCLOSE ";
					info.Arguments+="/HIDEMAINWINDOW ";
					info.Arguments+="/SMALLWINDOW ";
					info.Arguments+="/NORESULTDIALOG ";
					info.Arguments+="/TOOLBAREXITBUTTON ";
					Cursor=Cursors.WaitCursor;
					Process process=new Process();
					process.StartInfo=info;
					process.EnableRaisingEvents=true;
					process.Start();
					while(!process.HasExited) {
						Application.DoEvents();
					}
					Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
					Cursor=Cursors.Default;
					string resulttext="";
					string line="";
					string xChargeToken="";
					string accountMasked="";
					string exp="";;
					bool insertCard=false;
					try {
						using(TextReader reader=new StreamReader(resultfile)) {
							line=reader.ReadLine();
							while(line!=null) {
								if(resulttext!="") {
									resulttext+="\r\n";
								}
								resulttext+=line;
								if(line.StartsWith("RESULT=")) {
									if(line!="RESULT=SUCCESS") {
										throw new Exception();
									}
									insertCard=true;
								}
								if(line.StartsWith("XCACCOUNTID=")) {
									xChargeToken=PIn.String(line.Substring(12));
								}
								if(line.StartsWith("ACCOUNT=")) {
									accountMasked=PIn.String(line.Substring(8));
								}
								if(line.StartsWith("EXPIRATION=")) {
									exp=PIn.String(line.Substring(11));
								}
								line=reader.ReadLine();
							}
							if(insertCard && xChargeToken!="") {//Might not be necessary but we've had successful charges with no tokens returned before.
								creditCardCur=new CreditCard();
								List<CreditCard> itemOrderCount=CreditCards.Refresh(PatCur.PatNum);
								creditCardCur.PatientId=PatCur.PatNum;
								creditCardCur.ItemOrder=itemOrderCount.Count;
								creditCardCur.CCNumberMasked=accountMasked;
								creditCardCur.XChargeToken=xChargeToken;
								creditCardCur.CCExpiration=new DateTime(Convert.ToInt32("20"+PIn.String(exp.Substring(2,2))),Convert.ToInt32(PIn.String(exp.Substring(0,2))),1);
								creditCardCur.Procedures=Preferences.GetString(PreferenceName.DefaultCCProcs);
								creditCardCur.CCSource=CreditCardSource.XServer;
								creditCardCur.ClinicNum=Clinics.Active.Id;
								CreditCards.Insert(creditCardCur);
							}
						}
					}
					catch(Exception) {
						MessageBox.Show("There was a problem adding the credit card.  Please try again.");
					}
				}
				if(hasPayConnect) {
					FormPayConnect FormPC=new FormPayConnect(Clinics.Active.Id, PatCur,(decimal)0.01,creditCardCur,true);
					FormPC.ShowDialog();
				}
				if(hasPaySimple) {
					FormPaySimple formPS=new FormPaySimple(Clinics.Active.Id, PatCur,(decimal)0.01,creditCardCur,true);
					formPS.ShowDialog();
				}
				FillGrid();
				if(gridMain.Rows.Count>0 && creditCardCur!=null) {
					gridMain.SetSelected(gridMain.Rows.Count-1,true);
				}
				return;
			}
			//storing CC numbers allowed from here down
			FormCreditCardEdit FormCCE=new FormCreditCardEdit(PatCur);
			FormCCE.CreditCardCur=new CreditCard();
			FormCCE.CreditCardCur.IsNew=true;
			FormCCE.CreditCardCur.Procedures=Preferences.GetString(PreferenceName.DefaultCCProcs);
			FormCCE.ShowDialog();
			if(FormCCE.DialogResult==DialogResult.OK) {
				FillGrid();
				if(gridMain.Rows.Count>0) {
					gridMain.SetSelected(gridMain.Rows.Count-1,true);
				}
			}
		}

		private void butMoveTo_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()<0) {
				MessageBox.Show("Please select a card first.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Move this credit card information to a different patient account?")) {
				return;
			}
			FormPatientSelect form=new FormPatientSelect();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			int selected=gridMain.GetSelectedIndex();
			CreditCard creditCard=_listCreditCards[selected];
			long patNumOrig=creditCard.PatientId;
			creditCard.PatientId=form.SelectedPatientId;
			CreditCards.Update(creditCard);
			FillGrid();
			MessageBox.Show("Credit card moved successfully");
			SecurityLogs.MakeLogEntry(Permissions.CreditCardMove,patNumOrig,$"Credit card moved to PatNum: {form.SelectedPatientId}");
			SecurityLogs.MakeLogEntry(Permissions.CreditCardMove,form.SelectedPatientId, $"Credit card moved from PatNum: {patNumOrig}");
		}

		private void butUp_Click(object sender,EventArgs e) {
			int placement=gridMain.GetSelectedIndex();
			if(placement==-1) {
				MessageBox.Show("Please select a card first.");
				return;
			}
			if(placement==0) {
				return;//can't move up any more
			}
			int oldIdx;
			int newIdx;
			CreditCard oldItem;
			CreditCard newItem;
			oldIdx=_listCreditCards[placement].ItemOrder;
			newIdx=oldIdx+1; 
			for(int i=0;i<_listCreditCards.Count;i++) {
				if(_listCreditCards[i].ItemOrder==oldIdx) {
					oldItem=_listCreditCards[i];
					newItem=_listCreditCards[i-1];
					oldItem.ItemOrder=newItem.ItemOrder;
					newItem.ItemOrder-=1;
					CreditCards.Update(oldItem);
					CreditCards.Update(newItem);
				}
			}
			FillGrid();
			gridMain.SetSelected(placement-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			int placement=gridMain.GetSelectedIndex();
			if(placement==-1) {
				MessageBox.Show("Please select a card first.");
				return;
			}
			if(placement==_listCreditCards.Count-1) {
				return;//can't move down any more
			}
			int oldIdx;
			int newIdx;
			CreditCard oldItem;
			CreditCard newItem;
			oldIdx=_listCreditCards[placement].ItemOrder;
			newIdx=oldIdx-1;
			for(int i=0;i<_listCreditCards.Count;i++) {
				if(_listCreditCards[i].ItemOrder==newIdx) {
					newItem=_listCreditCards[i];
					oldItem=_listCreditCards[i-1];
					newItem.ItemOrder=oldItem.ItemOrder;
					oldItem.ItemOrder-=1;
					CreditCards.Update(oldItem);
					CreditCards.Update(newItem);
				}
			}
			FillGrid();
			gridMain.SetSelected(placement+1,true);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}