#region using
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using SHDocVw;
using SparksToothChart;
using OpenDental.Bridges;
using System.Drawing.Imaging;
using System.Threading;
using SharpDX;
using OpenDentBusiness.IO;
using Imedisoft.Forms;
using Imedisoft.UI;
using Imedisoft.X12.Codes;
using Imedisoft.Data.Models;
using Imedisoft.Data;
#if EHRTEST
using EHR;
#endif
#endregion using

namespace OpenDental {
	///<summary></summary>
	public partial class ContrChart:UserControl	{
		#region Fields - Public
		///<summary>public for plugins</summary>
		public bool TreatmentNoteChanged;
		#endregion Fields - Public

		#region Fields - Private
		///<summary>Locker for Apteryx Thumbnail downloads</summary>
		private object _apteryxLocker=new object();
		///<summary>For one patient. Allows highlighting rows.</summary>
		private Appointment[] _arrayAppts;
		private Document[] _arrayDocuments;
		///<summary>a list of the hidden teeth as strings. Includes "1"-"32", and "A"-"Z"</summary>
		private ArrayList _arrayListHiddenTeeth;
		///<summary>The indices within Documents.List[i] of docs which are visible in Chart.</summary>
		private ArrayList _arrayListVisImages;
		///<summary>The indices of the image categories which are visible in Chart.</summary>
		private ArrayList _arrayListVisImageCats;
		private PatField[] _arrayPatFields;
		private ProcButton[] _arrayProcButtons;
		///<summary>This is a filtered list containing only TP procedures.  It's also already sorted by priority and tooth number.</summary>
		private Procedure[] _arrayProceduresTp;
		///<summary>A specific reference to the "eRx" button.  This special reference helps us refresh the notification text on the button after the user changes.</summary>
		private ODToolBarButton _butErx;
		private int _chartScrollVal;
		///<summary>Can be null if user has not set up any views.  Defaults to first in list when starting up.</summary>
		private ChartView _chartViewCurDisplay;
		///<summary>The time that we started our last prog note search.</summary>
		private DateTime _dateTimeLastSearch;
		///<summary>Dictionary of every open FormErx per PatNum.  This is to ensure we only open 1 FormErx per PatNum.</summary>
		private Dictionary<long,FormErx> _dictFormErxSessions=new Dictionary<long,FormErx>();
		///<summary>Dictionary linking a TreatPlanNum key to the list of TpRows for that TP.</summary>
		private Dictionary<long,List<TpRow>> _dictTpNumListTpRows;
		private Family _famCur;
		private FormImageViewer _formImageViewer;
		private int _headingPrintH;
		private bool _headingPrinted;
		private int _imageSplitterOriginalY;
		private bool _initializedOnStartup;
		[DllImport("wininet.dll",CharSet = CharSet.Auto,SetLastError = true)]
		static extern bool InternetSetCookie(string lpszUrlName,string lbszCookieName,string lpszCookieData);

		///<summary>This keeps track of if the progress notes are being filled.</summary>
		private bool _isFillingProgNotes;
		///<summary>List of images that have been downloaded from Apteryx.</summary>
		private List<ApteryxThumbnail> _listApteryxThumbnails;
		private List <Benefit> _listBenefits;
		private List<ChartView> _listChartViews;
		///<summary>List of procedures added via quick add or procedure button.  Cleared for each user click before adding.</summary>
		private List<Procedure> _listChartedProcs=null;
		///<summary>Used for calculating insurance information. It might be able to remove this without much refactoring.</summary>
		private List<ClaimProcHist> _listClaimProcHists;
		private List <InsurancePlan> _listInsPlans;
		private List<InsSub> _listInsSubs;
		private List <PatPlan> _listPatPlans;
		///<summary>List of all procedures (except deleted status) for the current patient.</summary>
		private List<Procedure> _listPatProcs;
		///<summary>List containing only rows showing in gridPlanned, can be the same as _tablePlannedAll</summary>
		private List<DataRow> _listPlannedAppt;
		private List<ProcButtonQuick> _listProcButtonQuicks;
		///<summary>Used to determine what date the Tooth Chart should display.  Each unique date will have a tick on the time bar.</summary>
		private List<DateTime> _listProcDates;
		///<summary>A subset of DataSetMain.  The procedures with valid dates that were skipped when drawing the graphical toothchart</summary>
		private List<DataRow> _listProcsSkipped;
		///<summary>A copy of ProcList used to revert list of DataRows back to normal ChartModule after switching to IsTpCharting view.</summary>
		private List<DataRow> _listProcListOrig;
		///<summary>A subset of DataSetMain.  The procedures that need to be drawn in the graphical tooth chart.</summary>
		private List<DataRow> _listRowsProcForGraphical;
		///<summary>The rows in the prog notes table that matched the last time we searched.</summary>
		private List<DataRow> _listSearchResults=new List<DataRow>();
		///<summary>Used to cache the selected AptNums of the items in the main grid, to reselect them after a refresh.</summary>
		private List<long> _listSelectedAptNums=new List<long>();
		///<summary>This gets lazy loaded from db the first time a user clicks to add a procedure.  That way, it can be reused a few times for each new procedure added without going back to db.  Needed in Procedures.ComputeEstimates and Fees.GetListFromObjects.</summary>
		private List<SubstitutionLink> _listSubstitutionLinks;
		private List<ToothInitial> _listToothInitials;
		///<summary>Deep copy of ToothInitials that is used to fill the Tooth Chart when the track bar date changes. Currently only adds / removes missing teeth due to extractions.</summary>
		private List<ToothInitial> _listToothInitialsCopy;
		///<summary>List of all TPs for the current patient.  Does not include Saved status.</summary>
		private List<TreatPlan> _listTreatPlans;
		///<summary>Most if not all the data needed to load the module.</summary>
		private ChartModules.LoadData _loadDataNotThreadSafe;
		///<summary>Lock object used to lock _loadData.</summary>
		private ReaderWriterLockSlim _lockLoadData=new ReaderWriterLockSlim();
		private bool _mouseIsDownOnImageSplitter;
		private int _originalImageMousePos;
		private int _pagesPrinted;
		private Patient _patCur;
		///<summary>Full path to the patient folder, including \ on the end.  Could be null if a patient folder could not be created / found.</summary>
		private string _patFolder;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		private PatientNote _patientNoteCur;
		///<summary>Used for MenuItemPopup() to tell which row the user clicked on.  Currently only for gridPtInfo</summary>
		private Point _pointLastClicked;
		///<summary>The previous text we used when searching prog notes. Used in optimizing the search results</summary>
		private string _previousSearchText="";
		private long _prevPatNum;
		private ProcStat _procStatusNew;
		///<summary>Keeps track of which tab is selected. It's the index of the selected tab.</summary>
		private int _selectedImageTab=0;
		private int _selectedProcTab;
		///<summary>The class that controls all child control placement and anchoring based off of selected sheetDef layout.</summary>
		private SheetLayoutController _sheetLayoutController;
		private DateTime _dateTimeShowDateEnd;
		private DateTime _dateTimeShowDateStart;
		private DataTable _tablePlannedAll;
		///<summary>Thread that downloads Apteryx images.</summary>
		private ODThread _threadImageRequest;
		///<summary>This is the new Sparks3D toothChart.</summary>
		private Control toothChart;
		///<summary>Relays commands to either the old SparksToothChart.ToothChartWrapper or the new Sparks3d.ToothChart.</summary>
		private ToothChartRelay _toothChartRelay;
		#endregion Fields - Private

		#region Constructor
		///<summary></summary>
		public ContrChart() {
			Logger.LogInfo("Initializing chart module...");
			InitializeComponent();
			tabControlImages.DrawItem += new DrawItemEventHandler(OnDrawItem);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//panelQuickButtons.Enabled=false;
				butBF.Text="B/V";//vestibular instead of facial
				butV.Text="5";
			}
			else {
				menuItemLabFee.Visible=false;
				menuItemLabFeeDetach.Visible=false;
			}
			//no need to remove event handler... ContrChart always exists 1:1 per instance of the program.
			ODEvent.Fired+=ErxBrowserClosed;
		}
		#endregion Constructor

		#region Properties
		///<summary>List of tab titles for the TabProc control. Used to get accurate preview in sheet layout design view. Returns a list of one item called "Tab" if something goes wrong.</summary>
		public List<string> ListTabProcPageTitles {
			get {
				List<string> listTabTitles=tabProc.TabPages.OfType<TabPage>().Select(x => x.Text).ToList();
				if(listTabTitles.IsNullOrEmpty()) {
					listTabTitles=new List<string>() { "Tab" };
				}
				return listTabTitles;
			}
		}
		///<summary>Most if not all the data needed to load the module.</summary>
		private ChartModules.LoadData LoadData {
			get {
				try {
					_lockLoadData.EnterReadLock();
					return _loadDataNotThreadSafe;
				}
				finally {
					_lockLoadData.ExitReadLock();
				}
			}
			set {
				try {
					_lockLoadData.EnterWriteLock();
					_loadDataNotThreadSafe=value;
				}
				finally {
					_lockLoadData.ExitWriteLock();
				}
			}
		}
		///<summary>True if the chart view allows TP Charting or "Is TP View" is checked.</summary>
		private bool IsTPChartingAvailable {
			get { return (_chartViewCurDisplay!=null && _chartViewCurDisplay.IsTpCharting) || checkTPChart.Checked; }
		}
		#endregion Properties

		#region Methods - Event Handlers - Buttons
		private void butAddKey_Click(object sender,EventArgs e) {
			RegistrationKey key=new RegistrationKey();
			key.PatNum=_patCur.PatNum;
			//Notes are not commonly necessary, because most customers have only one office (thus only 1 key is necessary).
			//A tech can edit the note later after it is added if necessary.
			key.Note="";
			key.DateStarted=DateTime.Today;
			key.IsForeign=false;
			key.VotesAllotted=100;
			RegistrationKeys.Insert(key);
			FillPtInfo();//Refresh registration key list in patient info grid.
		}

		private void butChartViewAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			int count=gridChartViews.Rows.Count;
			int selectedIndex=gridChartViews.GetSelectedIndex();
			FormChartView formChartView=new FormChartView();
			formChartView.ChartViewCur=new ChartView();
			formChartView.ChartViewCur.IsNew=true;
			formChartView.ChartViewCur.ItemOrder=count;
			if(checkAppt.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=1;
			}
			if(checkComm.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=2;
			}
			if(checkCommFamily.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=4;
			}
			if(checkTasks.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=8;
			}
			if(checkEmail.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=16;
			}
			if(checkLabCase.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=32;
			}
			if(checkRx.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=64;
			}
			if(checkSheets.Checked) {
				formChartView.ChartViewCur.ObjectTypes+=128;
			}
			if(checkShowTP.Checked) {
				formChartView.ChartViewCur.ProcStatuses+=1;
			}
			if(checkShowC.Checked) {
				formChartView.ChartViewCur.ProcStatuses+=2;
			}
			if(checkShowE.Checked) {
				formChartView.ChartViewCur.ProcStatuses+=4;
			}
			if(checkShowR.Checked) {
				formChartView.ChartViewCur.ProcStatuses+=16;
			}
			if(checkShowCn.Checked) {
				formChartView.ChartViewCur.ProcStatuses+=64;
			}
			if(formChartView.ChartViewCur.IsNew) {
				formChartView.ChartViewCur.IsTpCharting=true;//default to TP view for new chart views
			}
			formChartView.ChartViewCur.SelectedTeethOnly=checkShowTeeth.Checked;
			formChartView.ChartViewCur.ShowProcNotes=checkNotes.Checked;
			formChartView.ChartViewCur.IsAudit=checkAudit.Checked;
			formChartView.ShowDialog();
			FillChartViewsGrid();
			int count2=gridChartViews.Rows.Count;
			if(count2==0) { 
				return; 
			}
			if(count2==count) {
				if(selectedIndex!=-1) {
					gridChartViews.SetSelected(selectedIndex,true);
					SetChartView(_listChartViews[selectedIndex]);
				}
			}
			else {
				formChartView.ChartViewCur.ItemOrder=count;
				ChartViews.Update(formChartView.ChartViewCur);
				FillChartViewsGrid();
				SetChartView(_listChartViews[count]);
				gridChartViews.SetSelected(count,true);
			}
		}

		private void butChartViewDown_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			if(gridChartViews.SelectedIndices.Length==0) {
				MessageBox.Show("Please select a view first.");
				return;
			}
			int oldIdx;
			int newIdx;
			ChartView oldChartView;
			ChartView newChartView;
			if(gridChartViews.GetSelectedIndex()!=-1) {
				oldIdx=gridChartViews.GetSelectedIndex();
				if(oldIdx==_listChartViews.Count-1) {
					return;//can't move down any more
				}
				newIdx=oldIdx+1;
				for(int i=0;i<_listChartViews.Count;i++) {
					if(_listChartViews[i].ItemOrder==newIdx) {
						newChartView=_listChartViews[i];
						oldChartView=_listChartViews[oldIdx];
						newChartView.ItemOrder=oldChartView.ItemOrder;
						oldChartView.ItemOrder+=1;
						ChartViews.Update(oldChartView);
						ChartViews.Update(newChartView);
					}
				}
				FillChartViewsGrid();
				gridChartViews.SetSelected(newIdx,true);
				SetChartView(_listChartViews[newIdx]);
			}
		}

		private void butChartViewUp_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			if(gridChartViews.SelectedIndices.Length==0) {
				MessageBox.Show("Please select a view first.");
				return;
			}
			int oldIdx;
			int newIdx;
			ChartView oldChartView;
			ChartView newChartView;
			if(gridChartViews.GetSelectedIndex()!=-1) {
				oldIdx=gridChartViews.GetSelectedIndex();
				if(oldIdx==0) {
					return;//can't move up any more
				}
				newIdx=oldIdx-1; 
				for(int i=0;i<_listChartViews.Count;i++) {
					if(_listChartViews[i].ItemOrder==oldIdx) {
						oldChartView=_listChartViews[i];
						newChartView=_listChartViews[newIdx];
						oldChartView.ItemOrder=newChartView.ItemOrder;
						newChartView.ItemOrder+=1;
						ChartViews.Update(oldChartView);
						ChartViews.Update(newChartView);
					}
				}
				FillChartViewsGrid();
				gridChartViews.SetSelected(newIdx,true);
				SetChartView(_listChartViews[newIdx]);
			}
		}

		private void butErxAccess_Click(object sender,EventArgs e) {
			FormErxAccess formErxAccess=new FormErxAccess(_patCur);
			formErxAccess.ShowDialog();
		}

		private void butForeignKey_Click(object sender,EventArgs e) {
			RegistrationKey key=new RegistrationKey();
			key.PatNum=_patCur.PatNum;
			key.Note="";
			key.DateStarted=DateTime.Today;
			key.IsForeign=true;
			key.VotesAllotted=100;
			RegistrationKeys.Insert(key);
			FillPtInfo();
		}

		private void butNewTP_Click(object sender, EventArgs e)
		{
			FormTreatPlanCurEdit formTPCE = new FormTreatPlanCurEdit();
			formTPCE.TreatPlanCur = new TreatPlan()
			{
				Heading = "Inactive Treatment Plan",
				Note = Preferences.GetString(PreferenceName.TreatmentPlanNote),
				PatNum = _patCur.PatNum,
				TPStatus = TreatPlanStatus.Inactive,
			};
			formTPCE.ShowDialog();
			if (formTPCE.DialogResult != DialogResult.OK)
			{
				return;
			}
			FillTreatPlans();
			_listTreatPlans.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum == x.TreatPlanNum)),
				formTPCE.TreatPlanCur.TreatPlanNum == x.TreatPlanNum));
			if (gridTreatPlans.GetSelectedIndex() > -1)
			{
				gridTreatPlans.ScrollToIndex(gridTreatPlans.GetSelectedIndex());
			}
			ModuleSelected(_patCur.PatNum);//refreshes TPs
		}

		private void butPhoneNums_Click(object sender, EventArgs e)
		{
			if (FormOpenDental.CurPatNum == 0)
			{
				MessageBox.Show( "Please select a patient first.");
				return;
			}
			FormPhoneNumbersManage formPNM = new FormPhoneNumbersManage();
			formPNM.PatNum = FormOpenDental.CurPatNum;
			formPNM.ShowDialog();
		}

		private void butShowDateRange_Click(object sender,EventArgs e) {
			FormChartViewDateFilter formCVDF=new FormChartViewDateFilter();
			formCVDF.DateStart=_dateTimeShowDateStart;
			formCVDF.DateEnd=_dateTimeShowDateEnd;
			formCVDF.ShowDialog();
			if(formCVDF.DialogResult!=DialogResult.OK) {
				return;
			}
			_dateTimeShowDateStart=formCVDF.DateStart;
			_dateTimeShowDateEnd=formCVDF.DateEnd;
			if(gridChartViews.Rows.Count>0) {//enable custom view label
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true; 
			FillDateRange();
			FillProgNotes();
		}
		#endregion Methods - Event Handlers - Buttons

		#region Methods - Event Handlers - ChartViews Clicked
		private void ChartViewsCellClicked(ODGridClickEventArgs e) {
			SetChartView(_listChartViews[e.Row]);
			gridChartViews.SetSelected(e.Row,true);
			RefreshModuleScreen(false);//Update UI to reflect any changed dynamic SheetDefs.
			ReloadSheetLayout();//Changes the progress notes grid. This changes the sheet layout.
		}

		private void ChartViewsDoubleClicked(ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			int count=gridChartViews.Rows.Count;
			FormChartView formChartView=new FormChartView();
			formChartView.ChartViewCur=_listChartViews[e.Row];
			formChartView.ShowDialog();
			FillChartViewsGrid();
			if(gridChartViews.Rows.Count==0) {
				FillProgNotes();
				return;//deleted last view, so display default
			}
			if(gridChartViews.Rows.Count==count) {
				gridChartViews.SetSelected(formChartView.ChartViewCur.ItemOrder,true);
				SetChartView(_listChartViews[formChartView.ChartViewCur.ItemOrder]);
			}
			else if(gridChartViews.Rows.Count>0) {
				for(int i=0;i<_listChartViews.Count;i++) {
					_listChartViews[i].ItemOrder=i;
					ChartViews.Update(_listChartViews[i]);
				}
				if(formChartView.ChartViewCur.ItemOrder!=0) {
					gridChartViews.SetSelected(formChartView.ChartViewCur.ItemOrder-1,true);
					SetChartView(_listChartViews[formChartView.ChartViewCur.ItemOrder-1]);
				}
				else {
					gridChartViews.SetSelected(0,true);
					SetChartView(_listChartViews[0]);
				}
			}
			RefreshModuleScreen(false);//Update UI to reflect any changed dynamic SheetDefs.
			ReloadSheetLayout();//Changes the progress notes grid. This changes the sheet layout.
		}
		#endregion Methods - Event Handlers - ChartViews Clicked

		#region Methods - Event Handlers - Checkboxes
		private void checkToday_CheckedChanged(object sender,EventArgs e) {
			if(checkToday.Checked) {
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			else {
				//
			}
		}

		private void checkTPChart_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			if(_chartViewCurDisplay!=null) {
				_chartViewCurDisplay.IsTpCharting=((CheckBox)sender).Checked;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}
		#endregion Methods - Event Handlers - Checkboxes

		#region Methods - Event Handlers - ErxBrowsers
		///<summary>CRITICAL: If we ever decide to launch eRx in an eternal browser window again, then we will need another way to sync the medications from eRx.  If we use an external browser window, then we have no way to know when the user is done with the exernal browser, and therefore we would not know when to sync.  Currently this event function knows when the browser closes, so we know when to sync.</summary>
		private void ErxBrowserClosed(ODEventArgs e) {
			if(e.EventType!=EventCategory.ErxBrowserClosed) {
				return;
			}
			Patient pat=(Patient)e.Tag;
			if(pat==null) {
				return;//A subwindow of FormErx was opened due to a link being clicked into a new window instance.
			}
			//Remove the FormErx session from the dictionary so that another FormErx can be opened for this patient if the user tries again.
			_dictFormErxSessions.Remove(pat.PatNum);
			if(_patCur==null || _patCur.PatNum!=pat.PatNum) {
				return;//FormErx was closed for another patient.
			}
			//Refresh prescriptions from NewCrop, since the user probably just added at least one.
			this.Cursor=Cursors.WaitCursor;
			Application.DoEvents();
			if(NewCropRefreshPrescriptions()) {
				ModuleSelected(_patCur.PatNum);
			}
			RefreshDoseSpotNotifications();
			this.Cursor=Cursors.Default;
		}
		#endregion Methods - Event Handlers - ErxBrowsers

		#region Methods - Event Handlers - Forms
		/// <summary>Event handler for closing FormExamSheets when it is non-modal.</summary>
		private void FormExamSheets_FormClosing(object sender,FormClosingEventArgs e) {
			long formPatNum=((FormExamSheets)sender).PatNum;
			if(_patCur!=null && _patCur.PatNum==formPatNum) { //Only refresh if we have a patient selected and are currently on the chart module matching this exam sheet.
				LoadData.TableProgNotes=ChartModules.GetProgNotes(formPatNum,checkAudit.Checked,GetChartModuleComponents());
				RefreshModuleScreen();
			}
		}

		/// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
		private void FormSheetFillEdit_FormClosing(object sender,FormClosingEventArgs e) {
			FormSheetFillEdit formSFE=((FormSheetFillEdit)sender);
			if(formSFE.DialogResult==DialogResult.OK && _patCur!=null) {
				//If the user deleted the sheet, forcefully refresh the chart module regardless of what patient is selected.
				//Otherwise; only refresh the chart module if the same patient is selected.
				if(formSFE.SheetCur==null || formSFE.SheetCur.PatNum==_patCur.PatNum) {
					ModuleSelected(_patCur.PatNum);
				}
			}
		}
		#endregion Methods - Event Handlers - Forms

		#region Methods - Event Handlers - Grids
		private void gridChartViews_CellClick(object sender,ODGridClickEventArgs e) {
			ChartViewsCellClicked(e);
		}

		private void gridChartViews_DoubleClick(object sender,ODGridClickEventArgs e) {
			ChartViewsDoubleClicked(e);
		}

		private void gridCustomerViews_CellClick(object sender,ODGridClickEventArgs e) {
			ChartViewsCellClicked(e);
		}

		private void gridCustomerViews_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			ChartViewsDoubleClicked(e);
		}

		private void gridProg_CellClick(object sender,ODGridClickEventArgs e) {
			DataTable tableProgNotes=LoadData.TableProgNotes;
			//DataRow rowClicked=progNotes.Rows[e.Row];
			DataRow rowClicked=(DataRow)gridProg.Rows[e.Row].Tag;
			long procNum=PIn.Long(rowClicked["ProcNum"].ToString());
			if(procNum==0) {//if not a procedure
				return;
			}
			long codeNum=PIn.Long(rowClicked["CodeNum"].ToString());
			if(ProcedureCodes.GetStringProcCode(codeNum,doThrowIfMissing:false)!=ProcedureCodes.GroupProcCode) {//if not a group note
				return;
			}
			List<ProcGroupItem> listGroupItems=ProcGroupItems.GetForGroup(procNum);
			//for(int i=0;i<progNotes.Rows.Count;i++){
			for(int i=0;i<gridProg.Rows.Count;i++) {
				DataRow row=(DataRow)gridProg.Rows[i].Tag;
				if(row["ProcNum"].ToString()=="0") {
					continue;
				}
				long procNum2=PIn.Long(row["ProcNum"].ToString());
				for(int j=0;j<listGroupItems.Count;j++) {
					if(procNum2==listGroupItems[j].ProcNum) {
						gridProg.SetSelected(i,true);
					}
				}
			}
		}

		private void gridProg_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right) {
				if(Preferences.GetBool(PreferenceName.EasyHideHospitals)) {
					menuItemPrintDay.Visible=false;
				}
				else {
					menuItemPrintDay.Visible=true;
				}
				//This hook was here before we changed this method to be MouseDown instead of MouseUp
				Plugins.HookAddCode(this,"ContrChart.gridProg_MouseUp_end",menuProgRight,gridProg,_patCur);
			}
		}

		private void gridPtInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(Plugins.HookMethod(this,"ContrChart.gridPtInfo_CellDoubleClick",_patCur,_famCur,e,_patientNoteCur)) {
				return;
			}
			if(TerminalActives.PatIsInUse(_patCur.PatNum)) {
				MessageBox.Show("Patient is currently entering info at a reception terminal.  Please try again later.");
				return;
			}
			if(gridPtInfo.Rows[e.Row].Tag!=null && gridPtInfo.Rows[e.Row].Tag.ToString()!="DOB") {
				if(new[] { "tabMedical","tabProblems","tabMedications","tabAllergies","tabTobaccoUse" }.Contains(gridPtInfo.Rows[e.Row].Tag.ToString())) {
					FormMedical formMedical=new FormMedical(_patientNoteCur,_patCur,gridPtInfo.Rows[e.Row].Tag.ToString());
					formMedical.ShowDialog();
					ModuleSelected(_patCur.PatNum);
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Referral") {
					//RefAttach refattach=(RefAttach)gridPat.Rows[e.Row].Tag;
					FormReferralsPatient formRP=new FormReferralsPatient();
					formRP.PatNum=_patCur.PatNum;
					formRP.ShowDialog();
					ModuleSelected(_patCur.PatNum);
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="References") {
					FormReference formReference=new FormReference();
					formReference.ShowDialog();
					if(formReference.GotoPatNum!=0) {
						Patient pat=Patients.GetPat(formReference.GotoPatNum);
						FormOpenDental.S_Contr_PatientSelected(pat,false);
						GotoModule.GotoFamily(formReference.GotoPatNum);
						return;
					}
					if(formReference.DialogResult!=DialogResult.OK) {
						return;
					}
					for(int i=0;i<formReference.SelectedCustRefs.Count;i++) {
						CustRefEntry custRefEntry=new CustRefEntry();
						custRefEntry.DateEntry=DateTime.Now;
						custRefEntry.PatNumCust=_patCur.PatNum;
						custRefEntry.PatNumRef=formReference.SelectedCustRefs[i].PatNum;
						CustRefEntries.Insert(custRefEntry);
					}
					FillPtInfo();
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Patient Portal") {
					FormPatientPortal formPP=new FormPatientPortal(_patCur);
					formPP.ShowDialog();
					if(formPP.DialogResult==DialogResult.OK) {
						FillPtInfo();
					}
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Payor Types") {
					FormPayorTypes formPayorTypes=new FormPayorTypes();
					formPayorTypes.PatCur=_patCur;
					formPayorTypes.ShowDialog();
					if(formPayorTypes.DialogResult==DialogResult.OK) {
						FillPtInfo();
					}
					return;
				}
				if(gridPtInfo.Rows[e.Row].Tag.ToString()=="Broken Appts") {					
					return;//This row is just for display; it can't be edited.
				}
				if(gridPtInfo.Rows[e.Row].Tag.GetType()==typeof(CustRefEntry)) {
					FormReferenceEntryEdit formREE=new FormReferenceEntryEdit((CustRefEntry)gridPtInfo.Rows[e.Row].Tag);
					formREE.ShowDialog();
					FillPtInfo();
					return;
				}
				else if(gridPtInfo.Rows[e.Row].Tag is PatFieldDef) {//patfield for an existing PatFieldDef
					PatFieldDef patFieldDef=(PatFieldDef)gridPtInfo.Rows[e.Row].Tag;
					PatField patField=PatFields.GetByName(patFieldDef.FieldName,_arrayPatFields);
					PatFieldL.OpenPatField(patField,patFieldDef,_patCur.PatNum);
				}
				else if(gridPtInfo.Rows[e.Row].Tag is PatField) {//PatField for a PatFieldDef that no longer exists
					PatField patField=(PatField)gridPtInfo.Rows[e.Row].Tag;
					FormPatFieldEdit formPatFieldEdit=new FormPatFieldEdit(patField);
					formPatFieldEdit.ShowDialog();
				}
			}
			else {
				FormPatientEdit formPatientEdit=new FormPatientEdit(_patCur,_famCur);
				formPatientEdit.IsNew=false;
				formPatientEdit.ShowDialog();
				if(formPatientEdit.DialogResult==DialogResult.OK) {
					FormOpenDental.S_Contr_PatientSelected(_patCur,false);
				}
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void gridPtInfo_MouseDown(object sender,MouseEventArgs e) {
			_pointLastClicked=e.Location;
		}

		private void gridTpProcs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridTpProcs.Rows[e.Row].Tag==null) {
				return;//clicked on header row
			}
			ProcTP procTpCur=(ProcTP)gridTpProcs.Rows[e.Row].Tag;
			Procedure procCur=Procedures.GetOneProc(procTpCur.ProcNumOrig,true);
			List<ClaimProc> listClaimProc=ClaimProcs.RefreshForTP(_patCur.PatNum);
			//generate a new loop list containing only the procs before this one in it
			List<ClaimProcHist> listClaimProcHistsLoop=new List<ClaimProcHist>();
			for(int i=0;i<_arrayProceduresTp.Length;i++) {
				if(_arrayProceduresTp[i].ProcNum==procCur.ProcNum) {
					break;
				}
				listClaimProcHistsLoop.AddRange(ClaimProcs.GetHistForProc(listClaimProc,_arrayProceduresTp[i].ProcNum,_arrayProceduresTp[i].CodeNum));
			}
			FormProcEdit formProcEdit=new FormProcEdit(procCur,_patCur,_famCur,listPatToothInitials:_listToothInitials);
			formProcEdit.LoopList=listClaimProcHistsLoop;
			formProcEdit.HistList=_listClaimProcHists;
			formProcEdit.ShowDialog();
			List<long> listSelectedTpNums=gridTreatPlans.SelectedIndices.Select(x => _listTreatPlans[x].TreatPlanNum).ToList();
			RefreshModuleData(_patCur.PatNum,true);
			FillProgNotes();
			gridTreatPlans.SetSelected(false);
			listSelectedTpNums.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x)),true));
			FillTpProcs();
			for(int i=0;i<gridTpProcs.Rows.Count;i++) {
				if(gridTpProcs.Rows[i].Tag==null) {
					continue;
				}
				ProcTP procTp=(ProcTP)gridTpProcs.Rows[i].Tag;
				gridTpProcs.SetSelected(i,(procTp.ProcNumOrig==procTpCur.ProcNumOrig && procTp.TreatPlanNum==procTpCur.TreatPlanNum));
			}
		}

		private void gridTreatPlans_CellClick(object sender,ODGridClickEventArgs e) {
			gridTpProcs.SetSelected(false);
			FillTpProcs();
			FillToothChart(false);
		}

		private void gridTreatPlans_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			TreatPlan treatPlanSelected=_listTreatPlans[e.Row];
			FormTreatPlanCurEdit formTPCE=new FormTreatPlanCurEdit();
			formTPCE.TreatPlanCur=treatPlanSelected;
			formTPCE.ShowDialog();
			if(formTPCE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillTreatPlans();
			_listTreatPlans.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x.TreatPlanNum)),
				formTPCE.TreatPlanCur.TreatPlanNum==x.TreatPlanNum));
			if(gridTreatPlans.GetSelectedIndex()>-1) {
				gridTreatPlans.ScrollToIndex(gridTreatPlans.GetSelectedIndex());
			}
			ModuleSelected(_patCur.PatNum);
		}
		#endregion Methods - Event Handlers - Grids

		#region Methods - Event Handlers - LayoutMenuItems
		private void LayoutMenuItem_Click(object sender,EventArgs e) {
			MenuItem menuItem=(sender as MenuItem);
			if(menuItem.Tag is SheetDef) {
				menuItem.Parent.MenuItems.OfType<MenuItem>().ForEach(x => x.Checked=(x==menuItem));
				SheetDef sheetDefCur=menuItem.Tag as SheetDef;
				_sheetLayoutController.InitLayoutForSheetDef(sheetDefCur,GetSheetLayoutMode(),GetSheetFieldDefControlDict(),isUserSelection:true);
			}
			else {
				Tool_Layout_Click();
			}
		}
		#endregion Methods - Event Handlers - LayoutMenuItems

		#region Methods - Event Handlers - Lists
		private void listCommonProcs_MouseDown(object sender,MouseEventArgs e) {
			if(listCommonProcs.SelectedIndex==-1) {
				MessageBox.Show("Please select a procedure.");
				return;
			}
			string procCode="";
			double procFee=0;
			//Hard coded internal procedures.
			switch(listCommonProcs.SelectedIndex) {
				case 0://Monthly Maintenance
					procCode="001";
					procFee=149;
					break;
				case 1://Monthly Mobile
					procCode="027";
					procFee=10;
					break;
				case 2://Monthly E-Mail Support
					procCode="008";
					procFee=89;
					break;
				case 3://Monthly EHR
					procCode="029";
					break;
				case 4://Data Conversion
					procCode="005";
					procFee=700;
					break;
				case 5://Trial Conversion
					procCode="N5641";
					break;
				case 6://Demo
					procCode="018";
					break;
				case 7://Online Training
					procCode="N1254";
					break;
				case 8://Additional Online Training
					procCode="N8989";
					procFee=50;
					break;
				case 9://eCW Online Training
					procCode="eCW1";
					break;
				case 10://eCW Installation Verification
					procCode="eCW2";
					break;
				case 11://Programming
					procCode="007";
					break;
				case 12://Query Programming
					procCode="023";
					procFee=90;
					break;
			}
			//Simply add the procedure to the customers account.
			Procedure proc=new Procedure();
			proc.CodeNum=ProcedureCodes.GetCodeNum(procCode);
			proc.DateEntryC=DateTimeOD.Today;
			proc.PatNum=_patCur.PatNum;
			proc.ProcDate=DateTime.Now;
			proc.DateTP=DateTime.Now;
			proc.ProcFee=procFee;
			proc.ProcStatus=ProcStat.TP;
			proc.ProvNum=Preferences.GetLong(PreferenceName.PracticeDefaultProv);
			proc.MedicalCode=ProcedureCodes.GetById(proc.CodeNum).MedicalCode;
			proc.BaseUnits=ProcedureCodes.GetById(proc.CodeNum).BaseUnits;
			proc.PlaceService=Preferences.GetString(PreferenceName.DefaultProcedurePlaceService, PlaceOfService.Office);//Default Proc Place of Service for the Practice is used. 
			Procedures.Insert(proc);//no recall synch needed because dental offices don't use this feature
			listCommonProcs.SelectedIndex=-1;
			FillProgNotes();
		}

		///<summary>Updates priority of all selected procedures to the selected priority.</summary>
		private void listPriorities_MouseDown(object sender,MouseEventArgs e) {
			int clickedRow=listPriorities.IndexFromPoint(e.X,e.Y);
			if(clickedRow==-1) {
				return;//nothing clicked, do nothing.
			}
			List<long> listSelectedTpNums=new List<long>();
			listSelectedTpNums.AddRange(gridTreatPlans.SelectedIndices.Select(x => _listTreatPlans[x].TreatPlanNum));
			//Priority of Procedures is dependent on which TP it is attached to. Track selected procedures by TPNum and ProcNum
			List<Tuple<long,long>> listSelectedTpNumProcNums=new List<Tuple<long,long>>();
			listSelectedTpNumProcNums.AddRange(gridTpProcs.SelectedIndices.Where(x => gridTpProcs.Rows[x].Tag!=null).Select(x => (ProcTP)gridTpProcs.Rows[x].Tag)
				.Select(x => new Tuple<long,long>(x.TreatPlanNum,x.ProcNumOrig)));
			List<TreatPlanAttach> listAllTpAttaches=gridTreatPlans.SelectedIndices.ToList().SelectMany(x => (List<TreatPlanAttach>)gridTreatPlans.Rows[x].Tag).ToList();
			foreach(int selectedIdx in gridTpProcs.SelectedIndices) {
				if(gridTpProcs.Rows[selectedIdx].Tag==null) {
					continue;//must be a header row.
				}
				ProcTP procTpCur=(ProcTP)gridTpProcs.Rows[selectedIdx].Tag;
				TreatPlanAttach treatPlanAttach=listAllTpAttaches.FirstOrDefault(x => x.ProcNum==procTpCur.ProcNumOrig && x.TreatPlanNum==procTpCur.TreatPlanNum);
				if(treatPlanAttach==null) {
					continue;//should never happen.
				}
				treatPlanAttach.Priority=0;
				if(clickedRow>0) {//Not 'no priority'
					treatPlanAttach.Priority=(listPriorities.Items[clickedRow] as ODBoxItem<Definition>).Tag.Id;
				}
			}
			listAllTpAttaches.Select(x => x.TreatPlanNum).Distinct().ToList()
				.ForEach(x => TreatPlanAttaches.Sync(listAllTpAttaches.FindAll(y => y.TreatPlanNum==x),x));//sync each TP seperately
			TreatPlanType treatPlanTypeCur=(_patCur.DiscountPlanNum==0?TreatPlanType.Insurance:TreatPlanType.Discount);
			TreatPlans.AuditPlans(_patCur.PatNum,treatPlanTypeCur);//consider adding logic here to update active plan priorities instead of calling the entire AuditPlans function
			listSelectedTpNums.ForEach(x => gridTreatPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(y => y.TreatPlanNum==x)),true));
			FillTpProcs();
			//Reselect TPs and Procs.
			for(int i=0;i<gridTpProcs.Rows.Count;i++) {
				if(gridTpProcs.Rows[i].Tag==null) {
					continue;
				}
				ProcTP procTpCur=(ProcTP)gridTpProcs.Rows[i].Tag;
				gridTpProcs.SetSelected(i,listSelectedTpNumProcNums.Contains(new Tuple<long,long>(procTpCur.TreatPlanNum,procTpCur.ProcNumOrig)));
			}
		}

		private void listProcStatusCodes_MouseUp(object sender,MouseEventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void listViewImages_DoubleClick(object sender, EventArgs e) {
			if(listViewImages.SelectedIndices.Count==0) {
				return;//clicked on white space.
			}
			ApteryxImage apteryxImg=listViewImages.SelectedItems[0].Tag as ApteryxImage;
			if(apteryxImg!=null) {//can be other images or documents here. XVWeb downloads have tags to store info.
				string text=listViewImages.SelectedItems[0].Text;
				Bitmap bitmapApiImage=null;
				double fileSizeMB=(double)apteryxImg.FileSize/1024/1024;
				FormProgress formProgress=new FormProgress(maxVal:fileSizeMB);
				formProgress.DisplayText="?currentVal MB of ?maxVal MB copied";
				ODThread ODThreadGetBitmap=new ODThread(new ODThread.WorkerDelegate((o) => {
					bitmapApiImage=XVWeb.GetBitmap(apteryxImg,formProgress);
				}));
				ODThreadGetBitmap.Name="DownloadApteryxImage"+apteryxImg.Id;
				ODThreadGetBitmap.Start(true);
				//display the progress dialog to the user:
				formProgress.ShowDialog();
				if(formProgress.DialogResult==DialogResult.Cancel) {
					ODThreadGetBitmap.QuitAsync();
					return;
				}
				ODThreadGetBitmap.Join(2000);//give thread some time to finish before trying to display the image.
				if(_formImageViewer==null || !_formImageViewer.Visible) {
					_formImageViewer=new FormImageViewer();
					_formImageViewer.Show();
				}
				if(_formImageViewer.WindowState==FormWindowState.Minimized) {
					_formImageViewer.WindowState=FormWindowState.Normal;
				}
				_formImageViewer.BringToFront();
				_formImageViewer.SetImage(bitmapApiImage,text);
				//if they want to save in the db & image doesn't already exist
				Document docSavedImage=XVWeb.SaveApteryxImageToDoc(apteryxImg,bitmapApiImage,_patCur);
				if(docSavedImage!=null) {
					listViewImages.SelectedItems[0].Tag=null;
					_listApteryxThumbnails.Remove(_listApteryxThumbnails.Find(x => x.Image.Id==apteryxImg.Id));//So that this image is not displayed twice
					int docListIndex=_arrayDocuments.GetLength(0);
					Array.Resize(ref _arrayDocuments,docListIndex+1);
					_arrayDocuments[docListIndex]=Documents.GetByNum(docSavedImage.Id);
					FillImages();
				}
				bitmapApiImage.Dispose();
				return;
			}
			Document docCur=_arrayDocuments[(int)_arrayListVisImages[listViewImages.SelectedIndices[0]]];
			if(!ImageHelper.HasImageExtension(docCur.FileName)) {
				try {
					Storage.Run(Storage.CombinePaths(_patFolder,docCur.FileName));
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
				}
				return;
			}
			if(_formImageViewer==null || !_formImageViewer.Visible) {
				_formImageViewer=new FormImageViewer();
				_formImageViewer.Show();
			}
			if(_formImageViewer.WindowState==FormWindowState.Minimized) {
				_formImageViewer.WindowState=FormWindowState.Normal;
			}
			_formImageViewer.BringToFront();
			_formImageViewer.SetImage(docCur,_patCur.GetNameLF()+" - "
				+docCur.AddedOnDate.ToShortDateString()+": "+docCur.Description);
		}

		private void listViewImages_ItemMouseHover(object sender,ListViewItemMouseHoverEventArgs e) {
			Cursor=Cursors.Default;
		}
		#endregion Methods - Event Handlers - Lists

		#region Methods - Event Handlers - Menu Items
		private void menuItemChartBig_Click(object sender,EventArgs e) {
			//Check for patient because the tooth chart will be expecting data from a patient's chart.
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			FormToothChartingBig formTCB=new FormToothChartingBig(checkShowTeeth.Checked,_listToothInitials,_listRowsProcForGraphical);
			formTCB.Show();
		}

		private void menuItemChartSave_Click(object sender,EventArgs e) {
			//Check for patient because the tooth chart will be expecting data from a patient's chart.
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			long defNum=Definitions.GetImageCat(ImageCategorySpecial.T);
			if(defNum==0) {//no category set for Tooth Charts.
				MessageBox.Show("No Def set for Tooth Charts.");
				return;
			}
			Bitmap bitmapChart=null;
			try {
				bitmapChart=_toothChartRelay.GetBitmap();
				ImageStore.Import(bitmapChart,defNum,ImageType.Photo,_patCur);
			}
			catch(SharpDXException sdxEx) {
				MsgBoxCopyPaste errorMsg=new MsgBoxCopyPaste("Failed to capture tooth chart image from graphics card. \r\n"
					+"Please contact support to help with graphics troubleshooting:\r\n"
					+sdxEx.Message+"\r\n"
					+sdxEx.StackTrace
				);
				errorMsg.ShowDialog();
				return;
			}
			catch(Exception ex) {
				MessageBox.Show("Unable to save file: "+ex.Message);
				return;
			}
			finally {//Executes regardles of above returns in the catches, "Saved." msgbox will not show.
				if(bitmapChart!=null) {
					bitmapChart.Dispose();
					bitmapChart=null;
				}
			}
			MessageBox.Show("Saved.");
		}

		private void menuItemDelete_Click(object sender,EventArgs e) {
			DeleteRows();
		}

		private void menuItemDoseSpotPendingPescr_Click(object sender,EventArgs e) {
			Tool_eRx_Click(true);
		}

		private void menuItemDoseSpotRefillReqs_Click(object sender,EventArgs e) {
			Tool_eRx_Click(true);
		}

		private void menuItemDoseSpotTransactionErrors_Click(object sender,EventArgs e) {
			Tool_eRx_Click(true);
		}

		private void menuItemEditSelected_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0) {
				MessageBox.Show("Please select procedures first.");
				return;
			}
			DataRow row;
			List<Procedure> listProcs=new List<Procedure>();
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(!CanEditRow(row,doCheckDb:true,isSilent:false,listProcs)) {
					return;
				}
			}
			FormProcEditAll formProcEditAll=new FormProcEditAll();
			formProcEditAll.ProcList=listProcs;
			formProcEditAll.ShowDialog();
			if(formProcEditAll.DialogResult==DialogResult.OK) {
				ModuleSelected(_patCur.PatNum);
			}
		}

		///<summary>Manuall refresh prescriptions from eRx.</summary>
		private void menuItemErxRefresh_Click(object sender,EventArgs e) {
			this.Cursor=Cursors.WaitCursor;
			Application.DoEvents();
			if(NewCropRefreshPrescriptions()) {
				ModuleSelected(_patCur.PatNum);
			}
			RefreshDoseSpotNotifications();
			this.Cursor=Cursors.Default;
		}

		private void menuItemGroupMultiVisit_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0) {
				return;
			}
			List<Procedure> listProcs=new List<Procedure>();
			foreach(int i in gridProg.SelectedIndices) {
				DataRow row=(DataRow)gridProg.Rows[i].Tag;
				if(!CanGroupMultiVisit(row,doCheckDb:true,isSilent:false)) {
					return;
				}
				Procedure proc=new Procedure();
				proc.ProcNum=PIn.Long(row["ProcNum"].ToString());
				proc.ProcStatus=PIn.Enum<ProcStat>(row["ProcStatus"].ToString());
				listProcs.Add(proc);
			}
			ProcMultiVisits.CreateGroup(listProcs);
			LoadData.ListProcMultiVisits.AddRange(ProcMultiVisits.GetGroupsForProcsFromDb(listProcs.Select(x => x.ProcNum).ToArray()));
			FillProgNotes(retainToothSelection:true,doRefreshData:false);//Refresh to show potential status change.
			MessageBox.Show("Done.");
		}

		private void menuItemGroupSelected_Click(object sender,EventArgs e) {
			List<Procedure> listProcs=new List<Procedure>();
			if(!CanAddGroupNote(doCheckDb:true,isSilent:false,listProcs)) {
				return;
			}
			DateTime procDate=listProcs[0].ProcDate;
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=listProcs[0].ClinicNum;
			}
			long provNum=listProcs[0].ProvNum;
			Procedure procGroup=new Procedure();
			procGroup.PatNum=_patCur.PatNum;
			procGroup.ProcStatus=ProcStat.EC;
			procGroup.DateEntryC=DateTime.Now;
			procGroup.ProcDate=procDate;
			procGroup.ProvNum=provNum;
			procGroup.ClinicNum=clinicNum;//Will be 0 above if clinics disabled.
			procGroup.CodeNum=ProcedureCodes.GetCodeNum(ProcedureCodes.GroupProcCode);
			if(Preferences.GetBool(PreferenceName.ProcGroupNoteDoesAggregate)) {
				string aggNote="";
				for(int i=0;i<listProcs.Count;i++) {
					if(i>0 && listProcs[i-1].Note!="") {
						aggNote+="\r\n";
					}
					aggNote+=listProcs[i].Note;
				}
				procGroup.Note=aggNote;
			}
			else {
				//group notes are special; they have a status of EC but still get their procedure notes populated.
				procGroup.Note=ProcCodeNotes.GetNote(procGroup.ProvNum,procGroup.CodeNum,procGroup.ProcStatus,true); 
				if(!Preferences.GetBool(PreferenceName.ProcPromptForAutoNote)) {
					//Users do not want to be prompted for auto notes, so remove them all from the procedure note.
					procGroup.Note=Regex.Replace(procGroup.Note,@"\[\[.+?\]\]","");
				}
			}
			procGroup.IsNew=true;
			Procedures.Insert(procGroup);
			List<ProcGroupItem> listProcGroupItems=new List<ProcGroupItem>();
			ProcGroupItem procGroupItem;
			for(int i=0;i<listProcs.Count;i++){
				procGroupItem=new ProcGroupItem();
				procGroupItem.ProcNum=listProcs[i].ProcNum;
				procGroupItem.GroupNum=procGroup.ProcNum;
				ProcGroupItems.Insert(procGroupItem);
				listProcGroupItems.Add(procGroupItem);
			}
			if(Programs.UsingOrion) {
				OrionProc orionProc=new OrionProc();
				orionProc.ProcNum=procGroup.ProcNum;
				orionProc.Status2=OrionStatus.C;
				OrionProcs.Insert(orionProc);
			}
			FormProcGroup formProcGroup=new FormProcGroup();
			formProcGroup.GroupCur=procGroup;
			formProcGroup.GroupItemList=listProcGroupItems;
			formProcGroup.ProcList=listProcs;
			formProcGroup.ShowDialog();
			if(formProcGroup.DialogResult!=DialogResult.OK){
				return;
			}
			if(Preferences.GetBool(PreferenceName.ProcGroupNoteDoesAggregate)) {
				//remove the notes from all the attached procs
				for(int i=0;i<listProcs.Count;i++) {
					Procedure oldProc=listProcs[i].Copy();
					Procedure changedProc=listProcs[i].Copy();
					changedProc.Note="";
					Procedures.Update(changedProc,oldProc);
				}
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void menuItemLabFee_Click(object sender,EventArgs e) {
			List<long> listProcNumsReg=new List<long>();
			List<long> listProcNumsLab=new List<long>();
			if(!CanAttachLabFee(isSilent:false,listProcNumsReg,listProcNumsLab)) {
				return;
			}
			//We only alter the lab procedure(s), not the regular procedure.
			Procedure procLab=null;
			Procedure procOld;
			for(int i=0;i<listProcNumsLab.Count;i++) {
				procLab=Procedures.GetOneProc(listProcNumsLab[i],false);
				procOld=procLab.Copy();
				procLab.ProcNumLab=listProcNumsReg[0];
				Procedures.Update(procLab,procOld);
			}
			if(procLab!=null) {
				CanadianLabFeeHelper(procLab.ProcNumLab);
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void menuItemLabFeeDetach_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length!=1) {
				MessageBox.Show("Please select exactly one lab procedure first.");
				return;
			}
			DataRow row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag;
			if(!CanDetachLabFee(row,isSilent:false)) {
				return;
			}
			Procedure procLab=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),false);
			Procedure procOld=procLab.Copy();
			procLab.ProcNumLab=0;
			Procedures.Update(procLab,procOld);
			CanadianLabFeeHelper(procOld.ProcNumLab);
			ModuleSelected(_patCur.PatNum);
		}

		///<summary>Just prior to displaying the context menu, enable or disables the UnmaskSSN option</summary>
		private void MenuItemPopupUnmaskDOB(object sender,EventArgs e) {
			MenuItem menuItemDOB=gridPtInfo.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Name=="ViewDOB");
			if(menuItemDOB==null) { 
				return;//Should not happen
			}
			MenuItem menuItemSeperator=gridPtInfo.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Text=="-");
			if(menuItemSeperator==null) { 
				return;//Should not happen
			}
			int idxRowClick=gridPtInfo.PointToRow(_pointLastClicked.Y);
			int idxColClick=gridPtInfo.PointToCol(_pointLastClicked.X);//Make sure the user clicked within the bounds of the grid.
			if(idxRowClick>-1 && idxColClick>-1 && (gridPtInfo.Rows[idxRowClick].Tag!=null)
				&& gridPtInfo.Rows[idxRowClick].Tag is string
				&& ((string)gridPtInfo.Rows[idxRowClick].Tag=="DOB")) 
			{
				if(Security.IsAuthorized(Permissions.PatientDOBView,true) 
					&& gridPtInfo.Rows[idxRowClick].Cells[gridPtInfo.Rows[idxRowClick].Cells.Count-1].Text!="") 
				{
					menuItemDOB.Visible=true;
					menuItemDOB.Enabled=true;
				}
				else {
					menuItemDOB.Visible=true;
					menuItemDOB.Enabled=false;
				}
				menuItemSeperator.Visible=true;
				menuItemSeperator.Enabled=true;
			}
			else {
				menuItemDOB.Visible=false;
				menuItemDOB.Enabled=false;
				if(gridPtInfo.ContextMenu.MenuItems.OfType<MenuItem>().Count(x => x.Visible==true && x.Text!="-")>1) {
					//There is more than one item showing, we want the seperator.
					menuItemSeperator.Visible=true;
					menuItemSeperator.Enabled=true;
				}
				else {
					//We dont want the seperator to be there with only one option.
					menuItemSeperator.Visible=false;
					menuItemSeperator.Enabled=false;
				}
			}
		}

		private void menuItemPrintDay_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0) {
				MessageBox.Show("Please select at least one item first.");
				return;
			}
			DataRow row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag;
			//hospitalDate=PIn.Date(row["ProcDate"].ToString());
			//Store the state of all checkboxes in temporary variables
			bool showRx=this.checkRx.Checked;
			bool showComm=this.checkComm.Checked;
			bool showApt=this.checkAppt.Checked;
			bool showEmail=this.checkEmail.Checked;
			bool showTask=this.checkTasks.Checked;
			bool showLab=this.checkLabCase.Checked;
			bool showSheets=this.checkSheets.Checked;
			bool showTeeth=this.checkShowTeeth.Checked;
			bool showAudit=this.checkAudit.Checked;
			DateTime showDateStart=_dateTimeShowDateStart;
			DateTime showDateEnd=_dateTimeShowDateEnd;
			bool showTP=this.checkShowTP.Checked;
			bool showComplete=this.checkShowC.Checked;
			bool showExist=this.checkShowE.Checked;
			bool showRefer=this.checkShowR.Checked;
			bool showCond=this.checkShowCn.Checked;
			bool showProcNote=this.checkNotes.Checked;
			bool customView=this.chartCustViewChanged;
			//Set the checkboxes to desired values for print out
			checkRx.Checked=false;
			checkComm.Checked=false;
			checkAppt.Checked=false;
			checkEmail.Checked=false;
			checkTasks.Checked=false;
			checkLabCase.Checked=false;
			checkSheets.Checked=false;
			checkShowTeeth.Checked=false;
			checkAudit.Checked=false;
			_dateTimeShowDateStart=PIn.Date(row["ProcDate"].ToString());
			_dateTimeShowDateEnd=PIn.Date(row["ProcDate"].ToString());
			checkShowTP.Checked=false;
			checkShowC.Checked=true;
			checkShowE.Checked=false;
			checkShowR.Checked=false;
			checkShowCn.Checked=false;
			checkNotes.Checked=true;
			chartCustViewChanged=true;//custom view will not reset the check boxes so we force it true.
			//Fill progress notes with only desired rows to be printed, then print.
			FillProgNotes();
			if(gridProg.Rows.Count==0) {
				MessageBox.Show("No completed procedures or notes to print");
			}
			else {
				try {
					_pagesPrinted=0;
					_headingPrinted=false;
					PrinterL.TryPrintOrDebugRpPreview(pd2_PrintPageDay,
						"Day report for hospital printed",
						auditPatNum:_patCur.PatNum,
						margins:new Margins(0,0,0,0),
						printoutOrigin:PrintoutOrigin.AtMargin
					);
				}
				catch {

				}
			}
			//Set Date values and checkboxes back to original values, then refill progress notes.
			//hospitalDate=DateTime.MinValue;
			checkRx.Checked=showRx;
			checkComm.Checked=showComm;
			checkAppt.Checked=showApt;
			checkEmail.Checked=showEmail;
			checkTasks.Checked=showTask;
			checkLabCase.Checked=showLab;
			checkSheets.Checked=showSheets;
			checkShowTeeth.Checked=showTeeth;
			checkAudit.Checked=showAudit;
			_dateTimeShowDateStart=showDateStart;
			_dateTimeShowDateEnd=showDateEnd;
			checkShowTP.Checked=showTP;
			checkShowC.Checked=showComplete;
			checkShowE.Checked=showExist;
			checkShowR.Checked=showRefer;
			checkShowCn.Checked=showCond;
			checkNotes.Checked=showProcNote;
			chartCustViewChanged=customView;
			FillProgNotes();
		}

		private void menuItemPrintProg_Click(object sender,EventArgs e) {
			_pagesPrinted=0;
			_headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd2_PrintPage,
				"Progress notes printed",
				auditPatNum:_patCur.PatNum
			);
		}

		private void menuItemPrintRouteSlip_Click(object sender,EventArgs e) {
			if(!CanPrintRoutingSlip(isSilent: false)) {
				return;
			}
			Appointment apt=Appointments.GetOneApt(PIn.Long(((DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag)["AptNum"].ToString()));
			//for now, this only allows one type of routing slip.  But it could be easily changed.
			FormRpRouting formRpRouting=new FormRpRouting();
			formRpRouting.AptNum=apt.AptNum;
			List<SheetDef> listCustomSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.RoutingSlip);
			if(listCustomSheetDefs.Count==0) {
				formRpRouting.SheetDefNum=0;
			}
			else {
				formRpRouting.SheetDefNum=listCustomSheetDefs[0].SheetDefNum;
			}
			formRpRouting.ShowDialog();
		}

		private void menuItemRxManage_Click(object sender,EventArgs e) {
			FormRxManage formRxManage=new FormRxManage(_patCur);
			formRxManage.ShowDialog();
			ModuleSelected(_patCur.PatNum);
		}

		private void menuItemSetComplete_Click(object sender,EventArgs e) {
			if(!IsAuditMode(isSilent:false)) {
				return;
			}
			//get list of DataRows from the selected row's tags in gridProg, in case the grid is refilled and our selections are cleared
			//(happens if right-clicking and marking a task done and with the prompt message box up another task is edited before pressing the OK button)
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			long patNum=_patCur.PatNum;//local patNum variable to make sure the patient hasn't changed since the above list was created
			#region One Appointment
			if(CanCompleteAppointment(doCheckDb:true,isSilent:!CanDisplayAppointment())) {
				DataRow rowApt=listSelectedRows.First();
				Appointment apt=Appointments.GetOneApt(PIn.Long(rowApt["AptNum"].ToString()));
				DateTime datePrevious=apt.DateTStamp;
				InsSub insSub1=InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans,PatPlans.GetOrdinal(PriSecMed.Primary,_listPatPlans,_listInsPlans,_listInsSubs)),_listInsSubs);
				InsSub insSub2=InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans,PatPlans.GetOrdinal(PriSecMed.Secondary,_listPatPlans,_listInsPlans,_listInsSubs)),_listInsSubs);
				Appointments.SetAptStatusComplete(apt,insSub1.PlanNum,insSub2.PlanNum);
				AppointmentEvent.Fire(EventCategory.AppointmentEdited,apt);
				List<Procedure> listProcsForAppt=Procedures.GetProcsForSingle(apt.AptNum,false);
				bool removeCompletedProcs=ProcedureL.DoRemoveCompletedProcs(apt,listProcsForAppt.FindAll(x => x.AptNum==apt.AptNum && x.ProcStatus==ProcStat.C));
				ProcedureL.SetCompleteInAppt(apt,_listInsPlans,_listPatPlans,_patCur,_listInsSubs,removeCompletedProcs);//loops through each proc, also makes completed security logs
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit, apt.PatNum,
					apt.ProcDescript+", "+apt.AptDateTime.ToString()+", Set Complete",
					apt.AptNum,datePrevious);
				//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
				if(HL7Defs.IsExistingHL7Enabled()) {
					//S14 - Appt Modification event
					MessageHL7 messageHL7=MessageConstructor.GenerateSIU(_patCur,_famCur.GetPatient(_patCur.Guarantor),EventTypeHL7.S14,apt);
					//Will be null if there is no outbound SIU message defined, so do nothing
					if(messageHL7!=null) {
						HL7Msg hl7Msg=new HL7Msg();
						hl7Msg.AptNum=apt.AptNum;
						hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
						hl7Msg.MsgText=messageHL7.ToString();
						hl7Msg.PatNum=_patCur.PatNum;
						HL7Msgs.Insert(hl7Msg);
#if DEBUG
						MessageBox.Show(this,messageHL7.ToString());
#endif
					}
				}
				Recalls.Synch(_patCur.PatNum);
				Recalls.SynchScheduledApptFull(_patCur.PatNum);
				ModuleSelected(_patCur.PatNum);
				//If necessary, prompt the user to ask the patient to opt in to using Short Codes.
				FormShortCodeOptIn.PromptIfNecessary(_patCur,apt.ClinicNum);
				return;
			}
			#endregion One Appointment
			#region One task
			if(CanCompleteTask(doCheckDb:true,isSilent:!CanDisplayTask())) {
				long taskNum=PIn.Long(listSelectedRows[0]["TaskNum"].ToString());
				Task taskCur=Tasks.GetOne(taskNum);
				Task taskOld=taskCur.Copy();
				taskCur.Status=TaskStatus.Done;//global even if new status is tracked by user
				if(taskOld.Status!=TaskStatus.Done) {
					taskCur.DateCompleted=DateTime.Now;
				}
				TaskUnreads.DeleteForTask(taskCur);//clear out taskunreads. We have too many tasks to read the done ones.
				Tasks.Update(taskCur,taskOld);
				TaskHistory taskHist=new TaskHistory(taskOld);
				taskHist.HistoryUserId=Security.CurrentUser.Id;
				TaskHists.Insert(taskHist);
				long signalNum=Signalods.SetInvalid(InvalidType.Task,KeyType.Task,taskCur.Id);
				UserControlTasks.RefillLocalTaskGrids(taskCur,new List<long>() { signalNum });
				ModuleSelected(_patCur.PatNum);
				return;
			}
			#endregion One task
			#region Multiple procedures
			MenuItemSetSelectedProcsStatus(ProcStat.C);
			#endregion
		}

		private void menuItemSetEC_Click(object sender,EventArgs e) {
			MenuItemSetSelectedProcsStatus(ProcStat.EC);
		}

		private void menuItemSetEO_Click(object sender,EventArgs e) {
			MenuItemSetSelectedProcsStatus(ProcStat.EO);
		}

		private void menuItemUngroupMultiVisit_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0) {
				return;
			}
			List <long> listProcNums=new List<long>();
			foreach(int i in gridProg.SelectedIndices) {
				DataRow row=(DataRow)gridProg.Rows[i].Tag;
				if(!CanUngroupMultiVisit(row,doCheckDb: true,isSilent: false)) {
					return;
				}
				long procNum=PIn.Long(row["ProcNum"].ToString());
				listProcNums.Add(procNum);
			}
			List <ProcMultiVisit> listProcMVs=ProcMultiVisits.GetGroupsForProcsFromDb(listProcNums.ToArray());
			bool hasDeletedRows=false;
			foreach(long procNum in listProcNums) {
				ProcMultiVisit pmv=listProcMVs.FirstOrDefault(x => x.ProcNum==procNum);
				if(pmv!=null) {//The procedure might not belong to a multiple visit group currently.
					ProcMultiVisits.Delete(pmv.ProcMultiVisitNum);
					hasDeletedRows=true;
					LoadData.ListProcMultiVisits.RemoveAll(x => x.ProcMultiVisitNum==pmv.ProcMultiVisitNum);
				}
			}
			if(hasDeletedRows) {
				Signalods.SetInvalid(InvalidType.ProcMultiVisits);
				ProcMultiVisits.RefreshCache();
				FillProgNotes(retainToothSelection:true,doRefreshData:false);//Refresh to show potential status change.
			}
			MessageBox.Show("Done.");
		}

		private void MenuItemUnmaskDOB_Click(object sender,EventArgs e) {
			//Preference and permissions check has already happened by this point.
			//Guaranteed to be clicking on a valid row & column.
			int rowClick = gridPtInfo.PointToRow(_pointLastClicked.Y);
			gridPtInfo.BeginUpdate();
			GridRow row=gridPtInfo.Rows[rowClick];
			row.Cells[row.Cells.Count-1].Text=Patients.DOBFormatHelper(_patCur.Birthdate,false);
			gridPtInfo.EndUpdate();
			string logText="Date of birth unmasked in Chart Module";
			SecurityLogs.MakeLogEntry(Permissions.PatientDOBView,_patCur.PatNum,logText);
		}
		#endregion Methods - Event Handlers - Menu Items

		#region Methods - Event Handlers - Menu Other
		private void menuConsent_Click(object sender,EventArgs e) {
			SheetDef sheetDef=(SheetDef)(((MenuItem)sender).Tag);
			SheetDefs.GetFieldsAndParameters(sheetDef);
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,_patCur.PatNum);
			SheetParameter.SetParameter(sheet,"PatNum",_patCur.PatNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			FormSheetFillEdit.ShowForm(sheet,FormSheetFillEdit_FormClosing);
		}

		private void menuConsent_Popup(object sender,EventArgs e) {
			menuConsent.MenuItems.Clear();
			List<SheetDef> listSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.Consent);
			MenuItem menuItem;
			for(int i=0;i<listSheetDefs.Count;i++) {
				menuItem=new MenuItem(listSheetDefs[i].Description);
				menuItem.Tag=listSheetDefs[i];
				menuItem.Click+=new EventHandler(menuConsent_Click);
				menuConsent.MenuItems.Add(menuItem);
			}
		}

		private void menuOrthoChart_Click(object sender,EventArgs e) {
			int orthoChartTabIndex=(int)(((MenuItem)sender).Tag);
			FormOrthoChart formOrthoChart=new FormOrthoChart(_patCur,orthoChartTabIndex);
			formOrthoChart.ShowDialog();
			ModuleSelected(_patCur.PatNum);
		}

		private void menuOrthoChart_Popup(object sender,EventArgs e) {
			menuOrthoChart.MenuItems.Clear();
			List<OrthoChartTab> listOrthoChartTabs=OrthoChartTabs.GetDeepCopy(true);
			for(int i=1;i<listOrthoChartTabs.Count;i++) {//Start i at 1 so tha we do not duplicate the first ortho tab (the main button)
				MenuItem menuItem=new MenuItem(listOrthoChartTabs[i].TabName);
				menuItem.Tag=i;
				menuItem.Click+=new EventHandler(menuOrthoChart_Click);
				menuOrthoChart.MenuItems.Add(menuItem);
			}
		}

		///<summary>The menu will display all items that are relevant to at least one selected row. If there is at least one selected row that is not relevant, the menu item will be disabled but visible.</summary>
		private void menuProgRight_Popup(object sender,EventArgs e) {
			//Permissions on some menu items will not get checked until clicking on the menu item which will then stop the user if not allowed. 
			ShowMenuItemHelper(menuItemDelete,x => CanDeleteRow(x.Row));
			ShowMenuItemHelper(menuItemSetEO,x => CanChangeProcsStatus(ProcStat.EO,x.Row,doCheckDb:false,isSilent:true));
			ShowMenuItemHelper(menuItemSetEC,x => CanChangeProcsStatus(ProcStat.EC,x.Row,doCheckDb:false,isSilent:true));
			ShowMenuItemHelper(menuItemSetComplete,x => CanChangeProcsStatus(ProcStat.C,x.Row,doCheckDb:false,isSilent:true));
			ShowMenuItemHelper(menuItemEditSelected,x => CanEditRow(x.Row,doCheckDb:false,isSilent:true,new List<Procedure> { }));
			ShowMenuItemHelper(menuItemGroupSelected,x => CanGroupRow(x.Row,doCheckDb:false,isSilent:true,new List<Procedure> { }));
			ShowMenuItemHelper(menuItemPrintDay,x => CanPrintDay(x.Row,x.Index));
			ShowMenuItemHelper(menuItemLabFeeDetach,x => CanDetachLabFee(x.Row,isSilent:true));
			ShowMenuItemHelper(menuItemLabFee,x => CanAttachLabFee(isSilent:true,new List<long> { },new List<long> { }));
			if(CanDisplayRoutingSlip()) {
				menuItemPrintRouteSlip.Visible=true;
				menuItemPrintRouteSlip.Enabled=CanPrintRoutingSlip(isSilent:true);
			}
			else {
				menuItemPrintRouteSlip.Visible=false;
			}
			menuItemGroupSelected.Enabled=CanAddGroupNote(doCheckDb:false,isSilent:true,new List<Procedure>());
			if(CanDisplayAppointment() || CanDisplayTask()) {
				menuItemSetComplete.Visible=true;
				menuItemSetComplete.Enabled=(CanCompleteAppointment(doCheckDb:false,isSilent:true) || CanCompleteTask(doCheckDb:false,isSilent:true));
			}
			ShowMenuItemHelper(menuItemGroupMultiVisit,x => CanGroupMultiVisit(x.Row,doCheckDb:false,isSilent:true));
			ShowMenuItemHelper(menuItemUngroupMultiVisit,x => CanUngroupMultiVisit(x.Row,doCheckDb:false,isSilent:true));
		}

		private void menuToothChart_Popup(object sender,EventArgs e) {
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			//only enable the big button if 3D graphics
			/*if(computerPref.GraphicsSimple) {
				menuItemChartBig.Enabled=false;
			}
			else {
				menuItemChartBig.Enabled=true;
			}*/
		}
		#endregion Methods - Event Handlers - Menu Other

		#region Methods - Event Handlers - OnDrawItems
		///<summary>Draws one button for the tabControlImages.</summary>
		private void OnDrawItem(object sender,DrawItemEventArgs e) {
      Graphics g=e.Graphics;
      Pen penBlue=new Pen(Color.FromArgb(97,136,173));
			Pen penRed=new Pen(Color.FromArgb(140,51,46));
			Pen penOrange=new Pen(Color.FromArgb(250,176,3),2);
			Pen penDkOrange=new Pen(Color.FromArgb(227,119,4));
			SolidBrush brBlack=new SolidBrush(Color.Black);
			int selected=-1;
			if(tabControlImages.SelectedTab!=null) {
				selected=tabControlImages.TabPages.IndexOf(tabControlImages.SelectedTab);
			}
			Rectangle bounds=e.Bounds;
			Rectangle rect=new Rectangle(bounds.X+2,bounds.Y+1,bounds.Width-5,bounds.Height-4);
			if(e.Index==selected) {
				g.FillRectangle(new SolidBrush(Color.White),rect);
				//g.DrawRectangle(penBlue,rect);
				g.DrawLine(penOrange,rect.X,rect.Bottom-1,rect.Right,rect.Bottom-1);
				g.DrawLine(penDkOrange,rect.X+1,rect.Bottom,rect.Right-2,rect.Bottom);
				g.DrawString(tabControlImages.TabPages[e.Index].Text,Font,brBlack,bounds.X+3,bounds.Y+6);
			}
			else {
				g.DrawString(tabControlImages.TabPages[e.Index].Text,Font,brBlack,bounds.X,bounds.Y);
			}
    }
		#endregion Methods - Event Handlers - OnDrawItems

		#region Methods - Event Handlers - Panels
		private void panelImages_MouseDown(object sender,MouseEventArgs e) {
			if(e.Y>3) {
				return;
			}
			_mouseIsDownOnImageSplitter=true;
			_imageSplitterOriginalY=panelImages.Top;
			_originalImageMousePos=panelImages.Top+e.Y;
		}

		private void panelImages_MouseLeave(object sender,EventArgs e) {
			//not needed.
		}

		private void panelImages_MouseMove(object sender,MouseEventArgs e) {
			if(!_mouseIsDownOnImageSplitter) {
				if(e.Y<=3) {
					panelImages.Cursor=Cursors.HSplit;
				}
				else {
					panelImages.Cursor=Cursors.Default;
				}
				return;
			}
			//panelNewTop
			int panelNewH=panelImages.Bottom
				-(_imageSplitterOriginalY+(panelImages.Top+e.Y)-_originalImageMousePos);//-top
			if(panelNewH<10)//cTeeth.Bottom)
				panelNewH=10;//cTeeth.Bottom;//keeps it from going too low
			if(panelNewH>panelImages.Bottom-_toothChartRelay.Bottom)
				panelNewH=panelImages.Bottom-_toothChartRelay.Bottom;//keeps it from going too high
			panelImages.Height=panelNewH;
		}

		private void panelImages_MouseUp(object sender,MouseEventArgs e) {
			if(!_mouseIsDownOnImageSplitter) {
				return;
			}
			_mouseIsDownOnImageSplitter=false;
		}

		///<summary>Handles single clicks that occur on button items. Not double clicks, not labels, and not empty space.</summary>
		private void panelQuickButtons_ItemClickBut(object sender,ODButtonPanelEventArgs e) {
			ProcButtonQuick procButtonQuick=null;
			for(int i=0;i<e.Item.Tags.Count;i++) {
				if(e.Item.Tags[i].GetType()!=typeof(ProcButtonQuick)) {
					continue;
				}
				procButtonQuick=(ProcButtonQuick)e.Item.Tags[i];//should always happen
			}
			if(procButtonQuick==null) {
				return;//should never happen.
			}
			ProcButtonClicked(null,procButtonQuick);
		}
		#endregion Methods - Event Handlers - Panels

		#region Methods - Event Handlers - Printing
		private void pd2_PrintPage(object sender,PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(25,40,800,1000);//1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font fontHeading=new Font("Arial",13,FontStyle.Bold);
			Font fontSubHeading=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			text="Chart Progress Notes";//heading
			g.DrawString(text,fontHeading,Brushes.Black,center-g.MeasureString(text,fontHeading).Width/2,yPos);
			text=DateTime.Today.ToShortDateString();//date
			g.DrawString(text,fontSubHeading,Brushes.Black,bounds.Right-g.MeasureString(text,fontSubHeading).Width,yPos);
			yPos+=(int)g.MeasureString(text,fontHeading).Height;
			text=_patCur.GetNameFL();//name
			if(g.MeasureString(text,fontSubHeading).Width>700) {
				//extremely long name
				text=_patCur.GetNameFirst()[0]+". "+_patCur.LName;//example: J. Sparks
			}
			string[] arrayHeaderText={ text };
			Plugins.HookAddCode(this,"ContrChart.pd2_PrintPage_middle",_patCur,e,g,arrayHeaderText);
			text=arrayHeaderText[0];
			g.DrawString(text,fontSubHeading,Brushes.Black,center-g.MeasureString(text,fontSubHeading).Width/2,yPos);
			text="Page "+(_pagesPrinted+1);
			g.DrawString(text,fontSubHeading,Brushes.Black,bounds.Right-g.MeasureString(text,fontSubHeading).Width,yPos);
			yPos+=30;
			_headingPrinted=true;
			_headingPrintH=yPos;
			#endregion
			yPos=gridProg.PrintPage(g,_pagesPrinted,bounds,_headingPrintH,true);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void pd2_PrintPageDay(object sender,PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(25,40,800,1000);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font fontHeading=new Font("Arial",13,FontStyle.Bold);
			Font fontSubHeading=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!_headingPrinted) {
				text="Chart Progress Notes";
				g.DrawString(text,fontHeading,Brushes.Black,center-g.MeasureString(text,fontHeading).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,fontHeading).Height;
				//practice
				text=Preferences.GetString(PreferenceName.PracticeTitle);
				if(PrefC.HasClinicsEnabled) {
					DataRow row;
					long procNum;
					long clinicNum;
					for(int i=0;i<gridProg.Rows.Count;i++) {
						row=(DataRow)gridProg.Rows[i].Tag;
						procNum=PIn.Long(row["ProcNum"].ToString());
						if(procNum==0) {
							continue;
						}
						clinicNum=Procedures.GetClinicNum(procNum);
						if(clinicNum!=0) {//The first clinicNum that's encountered
							//Description is used here because it can be printed and shown to the patient.
							text=Clinics.GetDescription(clinicNum);
							break;
						}
					}
				}
				g.DrawString(text,fontSubHeading,Brushes.Black,center-g.MeasureString(text,fontSubHeading).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,fontSubHeading).Height;
				//name
				text=_patCur.GetNameFL();
				g.DrawString(text,fontSubHeading,Brushes.Black,center-g.MeasureString(text,fontSubHeading).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,fontSubHeading).Height;
				text="Birthdate: "+_patCur.Birthdate.ToShortDateString();
				g.DrawString(text,fontSubHeading,Brushes.Black,center-g.MeasureString(text,fontSubHeading).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,fontSubHeading).Height;
				text="Printed: "+DateTime.Today.ToShortDateString();
				g.DrawString(text,fontSubHeading,Brushes.Black,center-g.MeasureString(text,fontSubHeading).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,fontSubHeading).Height;
				text="Ward: "+_patCur.Ward;
				g.DrawString(text,fontSubHeading,Brushes.Black,center-g.MeasureString(text,fontSubHeading).Width/2,yPos);
				yPos+=20;
				//Patient images are not shown when the A to Z folders are disabled.
	
					Bitmap patPicture;
					bool patientPictExists=Documents.GetPatPict(_patCur.PatNum,ImageStore.GetPatientFolder(_patCur, OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath()),out patPicture);
					if(patPicture!=null) {//Successfully loaded a patient picture?
						Bitmap thumbnail=ImageHelper.GetThumbnail(patPicture,80);
						g.DrawImage(thumbnail,center-40,yPos);
					}
					if(patientPictExists) {
						yPos+=80;
					}
					yPos+=30;
					_headingPrinted=true;
					_headingPrintH=yPos;
				
			}
			#endregion
			yPos=gridProg.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				g.DrawString("Signature_________________________________________________________",
								fontSubHeading,Brushes.Black,160,yPos+20);
				e.HasMorePages=false;
			}
			g.Dispose();
		}
		#endregion Methods - Event Handlers - Printing

		#region Methods - Event Handlers - Tabs General
		private void tabControlImages_MouseDown(object sender,MouseEventArgs e) {
			if(_selectedImageTab==-1) {
				_selectedImageTab=tabControlImages.SelectedIndex;
				return;
			}
			bool isTabAlreadySelected=false;
			if(_selectedImageTab.Between(0,tabControlImages.TabCount-1)) {
				Rectangle rect=tabControlImages.GetTabRect(_selectedImageTab);
				isTabAlreadySelected=rect.Contains(e.X,e.Y);
			}
			if(isTabAlreadySelected) {
				if(panelImages.Visible) {
					panelImages.Visible=false;
				}
				else {
					panelImages.Visible=true;
				}
			}
			else {//clicked on a new tab
				if(!panelImages.Visible) {
					panelImages.Visible=true;
				}
			}
			_selectedImageTab=tabControlImages.SelectedIndex;
			FillImages();//it will not actually fill the images unless panelImages is visible.
			RefreshModuleScreen(false);//Update UI to reflect any changed dynamic SheetDefs.
			ReloadSheetLayout();//Shows or hides the images bar. This changes the sheet layout.
		}

		private void tabProc_MouseDown(object sender,MouseEventArgs e) {
			ToggleCheckTreatPlans();
			if(Programs.UsingOrion) {
				return;//tabs never minimize
			}
			if(tabProc.Tag==null) {
				//Save original height, will get reset to null every time it is expanded.
				//We reset it in case user changes layouts with differenct tabProc heights.
				tabProc.Tag=tabProc.Height;
			}
			//selected tab will have changed, so we need to test the original selected tab:
			Rectangle rect=tabProc.GetTabRect(_selectedProcTab);
			if(rect.Contains(e.X,e.Y) && tabProc.Height>27) {//clicked on the already selected tab which was maximized
				tabProc.Height=27;
				tabProc.Refresh();
			}
			else if(tabProc.Height==27) {//clicked on a minimized tab
				tabProc.Height=((int)tabProc.Tag);
				tabProc.Tag=null;//Set null so that we save height in tag again, layout could change with different height.
				tabProc.Refresh();
			}
			else {//clicked on a new tab
				//height will have already been set, so do nothing
			}
			_selectedProcTab=tabProc.SelectedIndex;
			//Now that tabProc has collapsed or expanded, we need to refresh the layout to account for Chart Module sheet growth behavior of other controls
			_sheetLayoutController.RefreshGridVerticalSpace(GetSheetLayoutMode(),GetSheetFieldDefControlDict(),_sheetLayoutController
				.GetPertinentSheetFieldDefs(GetSheetLayoutMode()));
		}
		#endregion Methods - Event Handlers - Tabs General

		#region Methods - Event Handlers - Tab EnterTx
		private void butAddProc_Click(object sender,EventArgs e) {
			if(_procStatusNew==ProcStat.C) {
				if(!Preferences.GetBool(PreferenceName.AllowSettingProcsComplete)) {
					MsgBox.Show("Set the procedure complete by setting the appointment complete.  "
						+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
					return;
				}
				//We will call Security.IsAuthorized again once we know the ProcCode and the ProcFee.
				if(!ProcedureCodes.DoAnyBypassLockDate() && !Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))) {
					return;
				}
			}
			bool isValid;
			ProcedureTreatmentArea treatmentArea;
			FormProcCodes formProcCodes=new FormProcCodes();
			formProcCodes.IsSelectionMode=true;
			formProcCodes.ShowDialog();
			if(formProcCodes.DialogResult!=DialogResult.OK) {
				return;
			}
			ProcedureCode procedureCode=ProcedureCodes.GetById(formProcCodes.SelectedCodeNum);
			if(_listSubstitutionLinks==null){
				_listSubstitutionLinks=SubstitutionLinks.GetAllForPlans(_listInsPlans);
			}
			List<Fee> listFees=Fees.GetListFromObjects(new List<ProcedureCode>(){procedureCode },null,//no proc, so medical code won't have changed
				null,//listProvNumsTreat: providers will instead be set from other places
				_patCur.PriProv,_patCur.SecProv,_patCur.FeeSched,_listInsPlans,new List<long>(){_patCur.ClinicNum},_arrayAppts.ToList(),_listSubstitutionLinks,_patCur.DiscountPlanNum);
			List<string> listProcCodes=new List<string>();
			//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
			if(ProcedureCodes.GetStringProcCode(formProcCodes.SelectedCodeNum)!="D9986" && ProcedureCodes.GetStringProcCode(formProcCodes.SelectedCodeNum)!="D9987") {
				Procedures.SetDateFirstVisit(DateTimeOD.Today,1,_patCur);
			}
			Procedure procCur;
			for(int n=0;n==0 || n<_toothChartRelay.SelectedTeeth.Count;n++) {
				isValid=true;
				procCur=new Procedure();//going to be an insert, so no need to set Procedures.CurOld
				//Procedure
				procCur.CodeNum = formProcCodes.SelectedCodeNum;
				//Procedures.Cur.ProcCode=ProcButtonItems.CodeList[i];
				treatmentArea=procedureCode.TreatmentArea;
				if((treatmentArea==ProcedureTreatmentArea.Arch
					|| treatmentArea==ProcedureTreatmentArea.Mouth
					|| treatmentArea==ProcedureTreatmentArea.Quad
					|| treatmentArea==ProcedureTreatmentArea.Sextant
					|| treatmentArea==ProcedureTreatmentArea.ToothRange)
					&& n>0) {//the only two left are tooth and surf
					continue;//only entered if n=0, so they don't get entered more than once.
				}
				else if(treatmentArea==ProcedureTreatmentArea.Quad) {
					AddQuadProcs(procCur,listFees);
				}
				else if(treatmentArea==ProcedureTreatmentArea.Surface) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						isValid=false;
					}
					else {
						procCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
						//Procedures.Cur=ProcCur;
					}
					if(textSurf.Text=="") {
						isValid=false;
					}
					else {
						procCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,procCur.ToothNum);
					}
					if(isValid) {
						AddQuick(procCur,listFees);
					}
					else {
						AddProcedure(procCur,listFees);
					}
				}
				else if(treatmentArea==ProcedureTreatmentArea.Tooth) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						//Procedures.Cur=ProcCur;
						AddProcedure(procCur,listFees);
					}
					else {
						procCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
						//Procedures.Cur=ProcCur;
						AddQuick(procCur,listFees);
					}
				}
				else if(treatmentArea==ProcedureTreatmentArea.ToothRange) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						//Procedures.Cur=ProcCur;
						AddProcedure(procCur,listFees);
					}
					else {
						procCur.ToothRange="";
						for(int b=0;b<_toothChartRelay.SelectedTeeth.Count;b++) {
							if(b!=0) procCur.ToothRange+=",";
							procCur.ToothRange+=_toothChartRelay.SelectedTeeth[b];
						}
						//Procedures.Cur=ProcCur;
						AddProcedure(procCur,listFees);//it's nice to see the procedure to verify the range
					}
				}
				else if(treatmentArea==ProcedureTreatmentArea.Arch) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						procCur.Surf=CodeMapping.GetArchSurfaceFromProcCode(ProcedureCodes.GetById(procCur.CodeNum));
						//Procedures.Cur=ProcCur;
						AddProcedure(procCur,listFees);
						continue;
					}
					foreach(string arch in Tooth.GetArchesForTeeth(_toothChartRelay.SelectedTeeth)) {
						Procedure proc=procCur.Copy();
						proc.Surf=arch;
						AddQuick(proc,listFees);
					}
				}
				else if(treatmentArea==ProcedureTreatmentArea.Sextant) {
					//Procedures.Cur=ProcCur;
					AddProcedure(procCur,listFees);
				}
				else {//mouth
					//Procedures.Cur=ProcCur;
					AddQuick(procCur,listFees);
				}
				listProcCodes.Add(ProcedureCodes.GetById(procCur.CodeNum).Code);
			}//for n
			//this was requiring too many irrelevant queries and going too slowly   //ModuleSelected(PatCur.PatNum);
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
			ClearButtons();
			FillProgNotes();
			if(_procStatusNew==ProcStat.C) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,listProcCodes,_patCur.PatNum);
			}
		}

		private void butBF_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butBF.BackColor==Color.White) {
				butBF.BackColor=SystemColors.Control;
			}
			else {
				butBF.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butD_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butD.BackColor==Color.White) {
				butD.BackColor=SystemColors.Control;
			}
			else {
				butD.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butL_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butL.BackColor==Color.White) {
				butL.BackColor=SystemColors.Control;
			}
			else {
				butL.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butM_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butM.BackColor==Color.White) {
				butM.BackColor=SystemColors.Control;
			}
			else {
				butM.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butOI_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butOI.BackColor==Color.White) {
				butOI.BackColor=SystemColors.Control;
			}
			else {
				butOI.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butOK_Click(object sender,EventArgs e) {
			EnterTypedCode();
		}

		private void butV_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butV.BackColor==Color.White) {
				butV.BackColor=SystemColors.Control;
			}
			else {
				butV.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void checkTreatPlans_CheckedChanged(object sender,EventArgs e) {
			gridTreatPlans.SetSelected(false);
			if(_listTreatPlans!=null && _listTreatPlans.Count>0) {
				gridTreatPlans.SetSelected(0,true);
			}
			FillProgNotes();
			RefreshModuleScreen(false);//Update UI to reflect any changed dynamic SheetDefs.
			ReloadSheetLayout();//Shows or hides the TP UI. This changes the sheet layout selected.
		}

		private void ComboPrognosis_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboPrognosis.SelectedIndex<1//0 index is 'no prognosis' and does not need to be verified.yy
				|| Definitions.GetByCategory(DefinitionCategory.Prognosis).Any(x => x.Id==comboPrognosis.GetSelectedDefNum()))
			{
				return;
			}
			MessageBox.Show("The selected Prognosis has been hidden.");
			ModuleSelected(_patCur.PatNum);
		}

		private void comboPriority_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboPriority.SelectedIndex<1//0 is 'no priority'
				|| Definitions.GetByCategory(DefinitionCategory.TxPriorities).Any(x => x.Id==comboPriority.GetSelectedDefNum())) {
				return;
			}
			//this only happens if the user selects a priority that was just hidden by someone else
			MessageBox.Show("The selected Priority has been hidden.");
			ModuleSelected(_patCur.PatNum);
		}

		private void gridProg_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			_chartScrollVal=gridProg.ScrollValue;
			DataRow row=(DataRow)gridProg.Rows[e.Row].Tag;
			switch(ChartModules.GetRowType(row,out long rowPk)){
				case ProgNotesRowType.Proc:
					if(checkAudit.Checked){
						MessageBox.Show("Not allowed to edit procedures when in audit mode.");
						return;
					}
					Procedure proc=Procedures.GetOneProc(rowPk,true);
					if(ProcedureCodes.GetStringProcCode(proc.CodeNum,doThrowIfMissing:false)==ProcedureCodes.GroupProcCode) {
						FormProcGroup formProcGroup=new FormProcGroup();		
						List<ProcGroupItem> listProcGroupItems=ProcGroupItems.GetForGroup(proc.ProcNum);
						List<Procedure> listProcs=new List<Procedure>();
						for(int i=0;i<listProcGroupItems.Count;i++) {
							listProcs.Add(Procedures.GetOneProc(listProcGroupItems[i].ProcNum,false));
						}
						formProcGroup.GroupCur=proc;
						formProcGroup.GroupItemList=listProcGroupItems;
						formProcGroup.ProcList=listProcs;
						formProcGroup.ShowDialog();
						if(formProcGroup.DialogResult==DialogResult.OK) {
							ModuleSelected(_patCur.PatNum);
						}
						return;
					}
					else {
						FormProcEdit formProcEdit=new FormProcEdit(proc,_patCur,_famCur,listPatToothInitials:_listToothInitials);
						Plugins.HookAddCode(this, "ContrChart.gridProg_CellDoubleClick_proc", proc, formProcEdit);
						if(!formProcEdit.IsDisposed) { //Form might be disposed by the above hook.
							formProcEdit.ShowDialog();
						} 
						Plugins.HookAddCode(this, "ContrChart.gridProg_CellDoubleClick_proc2", proc, formProcEdit);
						if(formProcEdit.DialogResult!=DialogResult.OK) {
							return;
						}
					}
					break;
				case ProgNotesRowType.CommLog:
					Commlog commlog=Commlogs.GetOne(rowPk);
					if(commlog==null) {
						MessageBox.Show("This commlog has been deleted by another user.");
					}
					else {
						FormCommItem formCommItem=new FormCommItem(commlog);
						if(formCommItem.ShowDialog()!=DialogResult.OK) {
							return;
						}
					}
					break;
				case ProgNotesRowType.Rx:
					RxPat rx=RxPats.GetRx(rowPk);
					if(rx==null) {
						MessageBox.Show("This prescription has been deleted by another user.");
					}
					else {
						FormRxEdit formRxEdit=new FormRxEdit(_patCur,rx);
						formRxEdit.ShowDialog();
						if(formRxEdit.DialogResult!=DialogResult.OK) {
							return;
						}
					}
					break;
				case ProgNotesRowType.LabCase:
					LabCase labCase=LabCases.GetOne(rowPk);
					if(labCase==null) {
						MessageBox.Show("This LabCase has been deleted by another user.");
					}
					else {
						FormLabCaseEdit formLabCaseEdit=new FormLabCaseEdit();
						formLabCaseEdit.CaseCur=labCase;
						formLabCaseEdit.ShowDialog();
						//needs to always refresh due to complex ok/cancel
					}
					break;
				case ProgNotesRowType.Task:
					Task task=Tasks.GetOne(rowPk);
					if(task==null) {
						MessageBox.Show("This task has been deleted by another user.");
					}
					else {
						FormTaskEdit formTaskEdit=new FormTaskEdit(task);
						formTaskEdit.Show();//non-modal
					}
					break;
				case ProgNotesRowType.Apt:
					FormApptEdit formApptEdit=new FormApptEdit(rowPk);
					//PinIsVisible=false
					formApptEdit.IsInChartModule=true;
					formApptEdit.ShowDialog();
					if(formApptEdit.CloseOD) {//Only true when using ECW and user clicks FormApptEdit.butComplete.
						this.FindForm().Close();
						return;
					}
					if(formApptEdit.DialogResult!=DialogResult.OK) {
						return;
					}
					if(_patCur==null) {
						//For this to be null we changed the current module (Sent the appt to the pinboard from the chart)
						//There is no need to perform a module selected at the end of this method since the module has been refreshed already.
						return;
					}
					break;
				case ProgNotesRowType.EmailMessage:
					EmailMessage emailMsg=EmailMessages.GetOne(rowPk);
					if(emailMsg.SentOrReceived==EmailSentOrReceived.WebMailReceived
						|| emailMsg.SentOrReceived==EmailSentOrReceived.WebMailRecdRead
						|| emailMsg.SentOrReceived==EmailSentOrReceived.WebMailSent
						|| emailMsg.SentOrReceived==EmailSentOrReceived.WebMailSentRead) 
					{
						//web mail uses special secure messaging portal
						FormWebMailMessageEdit formWMME=new FormWebMailMessageEdit(_patCur.PatNum,emailMsg);
						if(formWMME.ShowDialog()==DialogResult.Cancel) {//This will cause an unneccesary refresh in the case of a validation error with the webmail
							return;
						}
					}
					else {
						FormEmailMessageEdit formEmailMessageEdit=new FormEmailMessageEdit(emailMsg);
						formEmailMessageEdit.ShowDialog();
						if(formEmailMessageEdit.DialogResult!=DialogResult.OK) {
							return;
						}
					}
					break;
				case ProgNotesRowType.Sheet:
					Sheet sheet=Sheets.GetSheet(rowPk);
					SheetUtilL.ShowSheet(sheet,_patCur,FormSheetFillEdit_FormClosing);
					break;
				case ProgNotesRowType.FormPat:
					FormPat formPat=FormPats.GetOne(rowPk);
					FormFormPatEdit formFormPatEdit=new FormFormPatEdit();
					formFormPatEdit.FormPatCur=formPat;
					formFormPatEdit.ShowDialog();
					if(formFormPatEdit.DialogResult==DialogResult.OK)
					{
						ModuleSelected(_patCur.PatNum);//Why is this called here and down 3 lines? Do we need the Allocator, or should we return here?
					}
					break;
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void gridProg_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back) {
				DeleteRows();
			}
		}

		private void listButtonCats_Click(object sender,EventArgs e) {
			FillProcButtons();
		}

		private void listDx_SelectedValueChanged(object sender,EventArgs e) {
			if(listDx.SelectedIndex==-1 //Just in case
				|| Definitions.GetDefsForCategory(DefinitionCategory.Diagnosis,true).Any(x => x.Id==listDx.GetSelected<Definition>().Id))//Cached def was hidden by another user.
			{
				return;
			}
			MessageBox.Show("The selected Diagnosis has been hidden.");
			ModuleSelected(_patCur.PatNum);
		}

		private void listViewButtons_Click(object sender,EventArgs e) {
			if(_procStatusNew==ProcStat.C) {
				if(!Preferences.GetBool(PreferenceName.AllowSettingProcsComplete)) {
					MessageBox.Show("Set the procedure complete by setting the appointment complete.  "
						+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
					return;
				}
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))) {
					return;
				}
			}
			if(listViewButtons.SelectedIndices.Count==0) {
				return;
			}
			ProcButton procButtonCur=_arrayProcButtons[listViewButtons.SelectedIndices[0]];
			ProcButtonClicked(procButtonCur);
		}

		private void radioEntryC_CheckedChanged(object sender,EventArgs e) {
			_procStatusNew=ProcStat.C;
		}

		private void radioEntryCn_CheckedChanged(object sender,EventArgs e) {
			_procStatusNew=ProcStat.Cn;
		}

		private void radioEntryEC_CheckedChanged(object sender,EventArgs e) {
			_procStatusNew=ProcStat.EC;
		}

		private void radioEntryEO_CheckedChanged(object sender,EventArgs e) {
			_procStatusNew=ProcStat.EO;
		}

		private void radioEntryR_CheckedChanged(object sender,EventArgs e) {
			_procStatusNew=ProcStat.R;
		}

		private void radioEntryTP_CheckedChanged(object sender,EventArgs e) {
			_procStatusNew=ProcStat.TP;
		}

		public void TaskGoToEvent(object sender,CancelEventArgs e) {
			if(_patCur==null) {
				return;
			}
			//FormTaskEdit formTaskEdit=(FormTaskEdit)sender;
			//TaskObjectType gotoType=formTaskEdit.GotoType;
			//long keyNum=formTaskEdit.GotoKeyNum;
			//if(gotoType==TaskObjectType.None) {
			//	ModuleSelected(_patCur.PatNum);
			//	return;
			//}
			//if(gotoType==TaskObjectType.Patient) {
			//	if(keyNum!=0) {
			//		Patient pat=Patients.GetPat(keyNum);
			//		FormOpenDental.S_Contr_PatientSelected(pat,false);
			//		ModuleSelected(pat.PatNum);
			//		return;
			//	}
			//}
			//if(gotoType==TaskObjectType.Appointment) {
			//	//There's nothing to do here, since we're not in the appt module.
			//	return;
			//}
		}

		private void textProcCode_Enter(object sender,EventArgs e) {
			if(textProcCode.Text=="Type Proc Code") {
				textProcCode.Text="";
			}
		}

		private void textProcCode_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				EnterTypedCode();
				e.Handled=true;
				e.SuppressKeyPress=true;
			}
		}

		private void textProcCode_TextChanged(object sender,EventArgs e) {
			if(textProcCode.Text=="d") {
				textProcCode.Text="D";
				textProcCode.SelectionStart=1;
			}
		}
		#endregion Methods - Event Handlers - Tab EnterTx

		#region Methods - Event Handlers - Tab MissingTeeth
		private void butEdentulous_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.Missing);
			for(int i=1;i<=32;i++) {
				ToothInitials.SetValueQuick(_patCur.PatNum,i.ToString(),ToothInitialType.Missing,0);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butHidden_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Hidden);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butMissing_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Missing);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butNotMissing_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Missing);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butUnhide_Click(object sender,EventArgs e) {
			if(listHidden.SelectedIndex==-1) {
				MessageBox.Show("Please select an item from the list first.");
				return;
			}
			ToothInitials.ClearValue(_patCur.PatNum,(string)_arrayListHiddenTeeth[listHidden.SelectedIndex],ToothInitialType.Hidden);
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}		
		#endregion Methods - Event Handlers - Tab MissingTeeth

		#region Methods - Event Handlers - Tab Movements
		private void butApplyMovements_Click(object sender,EventArgs e) {
			if(textShiftM.errorProvider1.GetError(textShiftM)!=""
				|| textShiftO.errorProvider1.GetError(textShiftO)!=""
				|| textShiftB.errorProvider1.GetError(textShiftB)!=""
				|| textRotate.errorProvider1.GetError(textRotate)!=""
				|| textTipM.errorProvider1.GetError(textTipM)!=""
				|| textTipB.errorProvider1.GetError(textTipB)!="")
			{
				MessageBox.Show("Please fix errors first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				if(textShiftM.Text!=""){
					ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftM,PIn.Float(textShiftM.Text));
				}
				if(textShiftO.Text!="") {
					ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftO,PIn.Float(textShiftO.Text));
				}
				if(textShiftB.Text!="") {
					ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftB,PIn.Float(textShiftB.Text));
				}
				if(textRotate.Text!="") {
					ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Rotate,PIn.Float(textRotate.Text));
				}
				if(textTipM.Text!="") {
					ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipM,PIn.Float(textTipM.Text));
				}
				if(textTipB.Text!="") {
					ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipB,PIn.Float(textTipB.Text));
				}
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butClearAllMovements_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Clear all movements on all teeth for this patient?")) {
				return;
			}
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.Rotate);
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.ShiftB);
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.ShiftM);
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.ShiftO);
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.TipB);
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.TipM);
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butClearSelectedMovements_Click(object sender, EventArgs e) {
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Rotate);
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftB);
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftM);
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftO);
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipB);
				ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipM);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butRotateMinus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Rotate,-20);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butRotatePlus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Rotate,20);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftBminus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftB,-2);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftBplus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftB,2);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftMminus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftM,-2);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftMplus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftM,2);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftOminus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftO,-2);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftOplus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftO,2);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butTipBminus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipB,-10);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butTipBplus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipB,10);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butTipMminus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipM,-10);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}

		private void butTipMplus_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.AddMovement(_listToothInitials,_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipM,10);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(true);
		}
		#endregion Methods - Event Handlers - Tab Movements

		#region Methods - Event Handlers - Tab Primary
		private void butAllPerm_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.Primary);
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butAllPrimary_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.Primary);
			for(int i=1;i<=32;i++){
				ToothInitials.SetValueQuick(_patCur.PatNum,i.ToString(),ToothInitialType.Primary,0);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butMixed_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(_patCur.PatNum,ToothInitialType.Primary);
			string[] arrayPriTeeth=new string[] 
				{"1","2","4","5","6","11","12","13","15","16","17","18","20","21","22","27","28","29","31","32"};
			for(int i=0;i<arrayPriTeeth.Length;i++) {
				ToothInitials.SetValueQuick(_patCur.PatNum,arrayPriTeeth[i],ToothInitialType.Primary,0);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butPerm_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				if(Tooth.IsPrimary(_toothChartRelay.SelectedTeeth[i])){
					ToothInitials.ClearValue(_patCur.PatNum,Tooth.PriToPerm(_toothChartRelay.SelectedTeeth[i])
						,ToothInitialType.Primary);
				}
				else{
					ToothInitials.ClearValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i]
						,ToothInitialType.Primary);
				}
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}

		private void butPrimary_Click(object sender,EventArgs e) {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				MessageBox.Show("Please select teeth first.");
				return;
			}
			for(int i=0;i<_toothChartRelay.SelectedTeeth.Count;i++) {
				ToothInitials.SetValue(_patCur.PatNum,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Primary);
			}
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			FillToothChart(false);
		}
		#endregion Methods - Event Handlers - Tab Primary

		#region Methods - Event Handlers - Tab Planned Appts
		///<summary>This is the listener for the Delete button.</summary>
		private void butClear_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0) {
				MessageBox.Show("Please select an item first");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Delete planned appointment(s)?")) {
				return;
			}
			for(int i=0;i<gridPlanned.SelectedIndices.Length;i++) {			
				Appointments.Delete(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["AptNum"].ToString()),true);
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0) {
				MessageBox.Show("Please select an item first.");
				return;
			}
			if(gridPlanned.SelectedIndices.Length>1) {
				MessageBox.Show("Please only select one item first.");
				return;
			}
			int idx=gridPlanned.SelectedIndices[0];
			if(idx==_listPlannedAppt.Count-1) {
				return;
			}
			DataRow rowSelectedAppt=_listPlannedAppt[idx];//Get selected data row
			DataRow rowBelowSelectedAppt=_listPlannedAppt[idx+1];//Get data row below the selected, since we are moving down we are going to need it to adjust its item order
			moveItemOrderHelper(rowSelectedAppt,PIn.Int(rowBelowSelectedAppt["ItemOrder"].ToString()));//Sets the selected rows item order = the above rows and adjust everything inbetween
			saveChangesToDBHelper();//Loops through list, gets PlannedAppt, sets the new ItemOrder and then updates if needed
			LoadData.TableProgNotes=ChartModules.GetProgNotes(_patCur.PatNum,checkAudit.Checked);
			LoadData.TablePlannedAppts=ChartModules.GetPlannedApt(_patCur.PatNum);
			_listSelectedAptNums.Clear();
			FillPlanned();
			gridPlanned.SetSelected(idx+1,true);
		}

		private void butNew_Click(object sender,EventArgs e) {
			if(_patCur==null) {
				MessageBox.Show("Please select a Patient.");
				return;
			}
			List<long> listProcNums=null;
			if(checkTreatPlans.Checked && gridTpProcs.SelectedRows.Count>0) {//Showing TPs and user has proc selections.
				if( _listTreatPlans[gridTreatPlans.GetSelectedIndex()].TPStatus!=TreatPlanStatus.Active) {//Only allow pre selecting procs on active TP.
					string msgText="Planned appointments can only be created using an Active treatment plan when selecting Procedures."+"\r\n"
						+"Continue without selections?";
					if(MessageBox.Show(msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
						return;	
					}
				}
				else {
					listProcNums=gridTpProcs.SelectedRows
						.FindAll(x => x.Tag!=null && x.Tag.GetType()==typeof(ProcTP))//ProcTP's only
						.Select(x => ((ProcTP)(x.Tag)).ProcNumOrig).ToList();//get ProcNums
				}
			}
			int itemOrder=LoadData.TablePlannedAppts.Rows.Count+1;
			if(AppointmentL.CreatePlannedAppt(_patCur,itemOrder,listProcNums)==PlannedApptStatus.FillGridNeeded) {
				FillPlanned();
			}
		}

		///<summary></summary>
		private void butPin_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0) {
				MessageBox.Show("Please select an item first");
				return;
			}
			if(_patCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
				MessageBox.Show("Appointments cannot be scheduled for "+_patCur.PatStatus.ToString().ToLower()+" patients.");
				return;
			}
			List<long> listAptNums=new List<long>();
			for(int i=0;i<gridPlanned.SelectedIndices.Length;i++) {
				long aptNum=PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["AptNum"].ToString());
				if(Procedures.GetProcsForSingle(aptNum,true).Count(x => x.ProcStatus==ProcStat.C)>0) {
					MessageBox.Show("Not allowed to send a planned appointment to the pinboard if completed procedures are attached. Edit the planned "
						+"appointment first.");
					return;
				}
				ApptStatus aptStatus=(ApptStatus)(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["AptStatus"].ToString()));
				if(aptStatus==ApptStatus.Complete) {
					//Warn the user they are moving a completed appointment.
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"You are about to move an already completed appointment.  Continue?")) {
						return;
					}
					listAptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["SchedAptNum"].ToString()));
				}
				else if(aptStatus==ApptStatus.Scheduled) {
					//Warn the user they are moving an already scheduled appointment.
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"You are about to move an appointment already on the schedule.  Continue?")) {
						return;
					}
					listAptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["SchedAptNum"].ToString()));
				}
				else if(aptStatus==ApptStatus.UnschedList || aptStatus==ApptStatus.Broken) {
					//Dont need to warn user, just put onto the pinboard.
					listAptNums.Add(PIn.Long(_listPlannedAppt[gridPlanned.SelectedIndices[i]]["SchedAptNum"].ToString())); 
				}
				else { //No appointment
					listAptNums.Add(aptNum);
				}
			}
			GotoModule.PinToAppt(listAptNums,0);
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridPlanned.SelectedIndices.Length==0) {
				MessageBox.Show("Please select an item first.");
				return;
			}
			if(gridPlanned.SelectedIndices.Length>1) {
				MessageBox.Show("Please only select one item first.");
				return;
			}
			int idx=gridPlanned.SelectedIndices[0];
			if(idx==0) {
				return;
			}
			DataRow rowSelectedAppt=_listPlannedAppt[idx];//Get selected data row
			//Get data row above the selected, since we are moving up we are going to need it to adjust its item order
			DataRow rowAboveSelectedAppt=_listPlannedAppt[idx-1];//idx guaranteed to be >0
			moveItemOrderHelper(rowSelectedAppt,PIn.Int(rowAboveSelectedAppt["ItemOrder"].ToString()));//Sets the selected rows item order = the above rows and adjust everything inbetween
			saveChangesToDBHelper();//Loops through list, gets PlannedAppt, sets the new ItemOrder and then updates if needed
			LoadData.TableProgNotes=ChartModules.GetProgNotes(_patCur.PatNum,checkAudit.Checked);
			LoadData.TablePlannedAppts=ChartModules.GetPlannedApt(_patCur.PatNum);
			_listSelectedAptNums.Clear();
			FillPlanned();
			gridPlanned.SetSelected(idx-1,true);
		}

		private void checkDone_Click(object sender,EventArgs e) {
			Patient patOld=_patCur.Copy();
			if(checkDone.Checked) {
				if(_tablePlannedAll.Rows.Count>0) {
					if(!MsgBox.Show(MsgBoxButtons.YesNo,"ALL planned appointment(s) will be deleted. Continue?")) {
						checkDone.Checked=false;
						return; 
					}
					for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
						Appointments.Delete(PIn.Long(_tablePlannedAll.Rows[i]["AptNum"].ToString()),true);
					}
				}
				_patCur.PlannedIsDone=true;
				Patients.Update(_patCur,patOld);
			}
			else{
				_patCur.PlannedIsDone=false;
				Patients.Update(_patCur,patOld);
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void checkShowCompleted_CheckedChanged(object sender,EventArgs e) {
			_listSelectedAptNums.Clear();
			foreach(int index in gridPlanned.SelectedIndices) {
				_listSelectedAptNums.Add(PIn.Long(_listPlannedAppt[index]["AptNum"].ToString()));
			}
			FillPlanned();
		}

		private void gridPlanned_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			long aptNum=(long)gridPlanned.Rows[e.Row].Tag;
			FormApptEdit formApptEdit=new FormApptEdit(aptNum);
			formApptEdit.ShowDialog();
			if(Programs.UsingOrion) {
				if(formApptEdit.DialogResult==DialogResult.OK) {
					ModuleSelected(_patCur.PatNum);//if procs were added in appt, then this will display them*/
				}
			}
			else {
				ModuleSelected(_patCur.PatNum);//if procs were added in appt, then this will display them*/
			}
			gridPlanned.SetSelected(false);
			for(int i=0;i<gridPlanned.Rows.Count;i++) {
				if((long)gridPlanned.Rows[i].Tag==aptNum) {
					gridPlanned.SetSelected(i,true);
				}
			}
		}
		#endregion Methods - Event Handlers - Tab Planned Appts

		#region Methods - Event Handlers - Tab Show
		private void button1_Click(object sender, System.EventArgs e) {
			//sometimes used for testing purposes
		}

		private void butShowAll_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			checkShowTP.Checked=true;
			checkShowC.Checked=true;
			checkShowE.Checked=true;
			checkShowR.Checked=true;
			checkShowCn.Checked=true;
			checkNotes.Checked=true;
			checkAppt.Checked=true;
			checkComm.Checked=true;
			checkCommFamily.Checked=true;
			checkLabCase.Checked=true;
			checkRx.Checked=true;
			checkShowTeeth.Checked=false;
			checkTasks.Checked=true;
			checkEmail.Checked=true;
			checkSheets.Checked=true;
			FillProgNotes();
		}

		private void butShowNone_Click(object sender,System.EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			checkShowTP.Checked=false;
			checkShowC.Checked=false;
			checkShowE.Checked=false;
			checkShowR.Checked=false;
			checkShowCn.Checked=false;
			checkNotes.Checked=false;
			checkAppt.Checked=false;
			checkComm.Checked=false;
			checkCommFamily.Checked=false;
			checkLabCase.Checked=false;
			checkRx.Checked=false;
			checkShowTeeth.Checked=false;
			checkTasks.Checked=false;
			checkEmail.Checked=false;
			checkSheets.Checked=false;
			FillProgNotes();
		}

		private void checkAppt_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkAudit_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkComm_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkCommFamily_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkEmail_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkLabCase_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkNotes_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkRx_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkSheets_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowC_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowCn_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowE_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowR_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowTP_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void checkShowTeeth_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			if(checkShowTeeth.Checked) {
				checkShowTP.Checked=true;
				checkShowC.Checked=true;
				checkShowE.Checked=true;
				checkShowR.Checked=true;
				checkShowCn.Checked=true;
				checkNotes.Checked=true;
				checkAppt.Checked=false;
				checkComm.Checked=false;
				checkCommFamily.Checked=false;
				checkLabCase.Checked=false;
				checkRx.Checked=false;
				checkEmail.Checked=false;
				checkTasks.Checked=false;
				checkSheets.Checked=false;
			}
			else {
				checkShowTP.Checked=true;
				checkShowC.Checked=true;
				checkShowE.Checked=true;
				checkShowR.Checked=true;
				checkShowCn.Checked=true;
				checkNotes.Checked=true;
				checkAppt.Checked=true;
				checkComm.Checked=true;
				checkCommFamily.Checked=true;
				checkLabCase.Checked=true;
				checkRx.Checked=true;
				checkEmail.Checked=true;
				checkTasks.Checked=true;
				checkSheets.Checked=true;
			}
			FillProgNotes(true);
		}

		private void checkTasks_Click(object sender,EventArgs e) {
			if(gridChartViews.Rows.Count>0) {
				labelCustView.Visible=true;
			}
			chartCustViewChanged=true;
			FillProgNotes();
		}

		private void labelSearchClear_Click(object sender,EventArgs e) {
			textSearch.Text="";
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			labelSearchClear.Visible=(textSearch.Text!="");
			if(textSearch.Text=="") {
				_dateTimeLastSearch=DateTime.Now;//This is so that if a previous search is running, it won't fill the grid when it finishes.
				_previousSearchText="";
				_listSearchResults?.Clear();
				_listSearchResults=null;
				if(!_isFillingProgNotes) {//if we are currently filling the progress notes, we do not want to do so again
					FillProgNotes();
				}
				return;
			}
			SearchProgNotes();
		}
		#endregion Methods - Event Handlers - Tab Show

		#region Methods - Event Handlers - Tab Draw
		private void butColorOther_Click(object sender,EventArgs e) {
			ColorDialog colorDialog=new ColorDialog();
			colorDialog.Color=butColorOther.BackColor;
			if(colorDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			panelDrawColor.BackColor=colorDialog.Color;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelBlack_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=Color.Black;
			_toothChartRelay.ColorDrawing=Color.Black;
		}

		private void panelCdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelCdark.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelClight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelClight.BackColor;
		}		
		
		private void panelDrawColor_DoubleClick(object sender,EventArgs e) {
			//do nothing
		}

		private void panelECdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelECdark.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelEClight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelEClight.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelEOdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelEOdark.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelEOlight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelEOlight.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelRdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelRdark.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelRlight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelRlight.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelTPdark_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelTPdark.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void panelTPlight_Click(object sender,EventArgs e) {
			panelDrawColor.BackColor=panelTPlight.BackColor;
			_toothChartRelay.ColorDrawing=panelDrawColor.BackColor;
		}

		private void radioColorChanger_Click(object sender,EventArgs e) {
			_toothChartRelay.CursorTool=CursorTool.ColorChanger;
		}

		private void radioEraser_Click(object sender,EventArgs e) {
			_toothChartRelay.CursorTool=CursorTool.Eraser;
		}

		private void radioPen_Click(object sender,EventArgs e) {
			_toothChartRelay.CursorTool=CursorTool.Pen;
		}

		private void radioPointer_Click(object sender,EventArgs e) {
			_toothChartRelay.CursorTool=CursorTool.Pointer;
		}
		#endregion Methods - Event Handlers - Tab Draw

		#region Methods - Event Handlers - Text Fields
		private void textTreatmentNotes_TextChanged(object sender,EventArgs e) {
			TreatmentNoteChanged=true;
		}

		private void textTreatmentNotes_Leave(object sender,EventArgs e) {
			UpdateTreatmentNote();
		}

		private void textTreatmentNotes_MouseLeave(object sender,EventArgs e) {
			UpdateTreatmentNote();
		}
		#endregion Methods - Event Handlers - Text Fields

		#region Methods - Event Handlers - Toolbars
		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				//standard predefined button
				switch(e.Button.Tag.ToString()) {
					case "Rx":
						Tool_Rx_Click();
						break;
					case "eRx":
						Tool_eRx_Click();
						break;
					case "LabCase":
						Tool_LabCase_Click();
						break;
					case "Perio":
						Tool_Perio_Click();
						break;
					case "Ortho":
						Tool_Ortho_Click();
						break;
					case "Anesthesia":
						Tool_Anesthesia_Click();
						break;
					case "Consent":
						Tool_Consent_Click();
						break;
					case "Commlog"://only for eCW
						Tool_Commlog_Click();
						break;
					case "ToothChart":
						Tool_ToothChart_Click();
						break;
					case "ExamSheet":
						Tool_ExamSheet_Click();
						break;
					case "EHR":
						Tool_EHR_Click(false);
						break;
					case "HL7":
						Tool_HL7_Click();
						break;
					case "MedLab":
						Tool_MedLab_Click();
						break;
					case "Layout":
						Tool_Layout_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).Id,_patCur);
			}
		}
		#endregion Methods - Event Handlers - Toolbars

		#region Methods - Event Handlers - Tooth
		private void toothChart_Click(object sender,EventArgs e) {
			textSurf.Text="";
			//if(toothChart.SelectedTeeth.Length==1) {
				//butO.BackColor=SystemColors.Control;
				//butB.BackColor=SystemColors.Control;
				//butF.BackColor=SystemColors.Control;
				//if(Tooth.IsAnterior(toothChart.SelectedTeeth[0])) {
					//butB.Text="";
					//butO.Text="";
					//butB.Enabled=false;
					//butO.Enabled=false;
					//butF.Text="F";
					//butI.Text="I";
					//butF.Enabled=true;
					//butI.Enabled=true;
				//}
				//else {
					//butB.Text="B";
					//butO.Text="O";
					//butB.Enabled=true;
					//butO.Enabled=true;
					//butF.Text="";
					//butI.Text="";
					//butF.Enabled=false;
					//butI.Enabled=false;
				//}
			//}
			if(checkShowTeeth.Checked) {
				FillProgNotes();
			}
			FillMovementsAndHidden();
		}
		
		private void toothChart_SegmentDrawn(object sender,ToothChartDrawEventArgs e) {
			string stringSegment=e.DrawingSegement;
			if(radioPen.Checked) {
				ToothInitial toothInitial=new ToothInitial();
				toothInitial.DrawingSegment=stringSegment;
				toothInitial.InitialType=ToothInitialType.Drawing;
				toothInitial.PatNum=_patCur.PatNum;
				toothInitial.ColorDraw=panelDrawColor.BackColor;
				ToothInitials.Insert(toothInitial);
				_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
				FillToothChart(true);
			}
			else if(radioEraser.Checked) {
				//for(int i=0;i<ToothInitialList.Count;i++) {
				for(int i=_listToothInitials.Count-1;i>=0;i--) {//go backwards
					if(_listToothInitials[i].InitialType!=ToothInitialType.Drawing) {
						continue;
					}
					if(_listToothInitials[i].DrawingSegment!=stringSegment) {
						continue;
					}
					ToothInitials.Delete(_listToothInitials[i]);
					_listToothInitials.RemoveAt(i);
					//no need to refresh since that's handled by the toothchart.
				}
			}
			else if(radioColorChanger.Checked) {
				for(int i=0;i<_listToothInitials.Count;i++) {
					if(_listToothInitials[i].InitialType!=ToothInitialType.Drawing) {
						continue;
					}
					if(_listToothInitials[i].DrawingSegment!=stringSegment) {
						continue;
					}
					_listToothInitials[i].ColorDraw=panelDrawColor.BackColor;
					ToothInitials.Update(_listToothInitials[i]);
					FillToothChart(true);
				}
			}
		}

		private void toothChart_ToothSelectionsChanged(object sender) {
			if(checkShowTeeth.Checked) {
				FillProgNotes(true,false);
			}
			FillMovementsAndHidden();
		}

		private void trackToothProcDates_ValueChanged(object sender,EventArgs e) {
			textToothProcDate.Text=_listProcDates[trackToothProcDates.Value].ToShortDateString();
			FillToothChart(true,_listProcDates[trackToothProcDates.Value]);
		}
		#endregion Methods - Event Handlers - Tooth

		#region Methods - Public
		public void FillProgNotes(bool retainToothSelection=false,bool doRefreshData=true,bool isSearch=false,bool isForceFirstPage=false) {
			_isFillingProgNotes=true;
			FillProgNotesInternal(retainToothSelection,doRefreshData,isSearch,isForceFirstPage);
			_isFillingProgNotes=false;
		}

		///<summary>Do not call this method. Call FillProgNotes instead.</summary>
		public void FillProgNotesInternal(bool retainToothSelection=false,bool doRefreshData=true,bool isSearch=false,bool isForceFirstPage=false) {
			Plugins.HookAddCode(this,"ContrChart.FillProgNotes_begin");
			//Make a reference to all of the tags (custom DataRow) that are currently selected within gridProg for reselecting.
			List<DataRow> listSelectedDataRows=gridProg.SelectedTags<DataRow>();
			gridProg.BeginUpdate();
			gridProg.Columns.Clear();
			GridColumn col;
			List<DisplayField> listDisplayFields;
			//DisplayFields.RefreshCache();
			if(gridChartViews.Rows.Count==0) {//No chart views, Use default values.
				listDisplayFields=DisplayFields.GetDefaultList(DisplayFieldCategory.None);
				gridProg.Title="Progress Notes";
				if(!chartCustViewChanged) {
					checkSheets.Checked=true;
					checkTasks.Checked=true;
					checkEmail.Checked=true;
					checkCommFamily.Checked=true;
					checkAppt.Checked=true;
					checkLabCase.Checked=true;
					checkRx.Checked=true;
					checkComm.Checked=true;
					checkShowTP.Checked=true;
					checkShowC.Checked=true;
					checkShowE.Checked=true;
					checkShowR.Checked=true;
					checkShowCn.Checked=true;
					checkNotes.Checked=true;
					checkShowTeeth.Checked=false;
					checkAudit.Checked=false;
					textShowDateRange.Text="All Dates";
				}
			}
			else {
				if(_chartViewCurDisplay==null) {
					_chartViewCurDisplay=ChartViews.GetFirst();
				}
				listDisplayFields=DisplayFields.GetForChartView(_chartViewCurDisplay.Id);
				gridProg.Title=_chartViewCurDisplay.Description;
				if(!chartCustViewChanged) {
					checkSheets.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.Sheets)==ChartViewObjs.Sheets;
					checkTasks.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.Tasks)==ChartViewObjs.Tasks;
					checkEmail.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.Email)==ChartViewObjs.Email;
					checkCommFamily.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.CommLogFamily)==ChartViewObjs.CommLogFamily;
					checkAppt.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.Appointments)==ChartViewObjs.Appointments;
					checkLabCase.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.LabCases)==ChartViewObjs.LabCases;
					checkRx.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.Rx)==ChartViewObjs.Rx;
					checkComm.Checked=(_chartViewCurDisplay.ObjectTypes & ChartViewObjs.CommLog)==ChartViewObjs.CommLog;
					checkShowTP.Checked=(_chartViewCurDisplay.ProcStatuses & ChartViewProcStat.TP)==ChartViewProcStat.TP;
					checkShowC.Checked=(_chartViewCurDisplay.ProcStatuses & ChartViewProcStat.C)==ChartViewProcStat.C;
					checkShowE.Checked=(_chartViewCurDisplay.ProcStatuses & ChartViewProcStat.EC)==ChartViewProcStat.EC;
					checkShowR.Checked=(_chartViewCurDisplay.ProcStatuses & ChartViewProcStat.R)==ChartViewProcStat.R;
					checkShowCn.Checked=(_chartViewCurDisplay.ProcStatuses & ChartViewProcStat.Cn)==ChartViewProcStat.Cn;
					checkShowTeeth.Checked=_chartViewCurDisplay.SelectedTeethOnly;
					checkNotes.Checked=_chartViewCurDisplay.ShowProcNotes;
					checkAudit.Checked=_chartViewCurDisplay.IsAudit;
					checkTPChart.Checked=_chartViewCurDisplay.IsTpCharting;
					SetDateRange();
					FillDateRange();
					gridChartViews.SetSelected(_chartViewCurDisplay.ItemOrder,true);
				}
				else {
					gridChartViews.SetSelected(false);
				}
			}
			bool showSelectedTeeth=checkShowTeeth.Checked;
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				checkShowTeeth.Checked=false;
			}
			if(_patCur!=null && doRefreshData) {
				LoadData.TableProgNotes=ChartModules.GetProgNotes(_patCur.PatNum,checkAudit.Checked,GetChartModuleComponents());
				LoadData.TablePlannedAppts=ChartModules.GetPlannedApt(_patCur.PatNum);
			}
			if(checkTreatPlans.Checked) {
				checkShowTP.Enabled=false;
				checkShowC.Enabled=false;
				checkShowE.Enabled=false;
				checkShowR.Enabled=false;
				checkShowCn.Enabled=false;
				checkNotes.Enabled=false;
			}
			else {
				checkShowTP.Enabled=true;
				checkShowC.Enabled=true;
				checkShowE.Enabled=true;
				checkShowR.Enabled=true;
				checkShowCn.Enabled=true;
				checkNotes.Enabled=true;
			}
			for(int i=0;i<listDisplayFields.Count;i++) {
				if(listDisplayFields[i].Description=="") {
					col=new GridColumn(listDisplayFields[i].InternalName,listDisplayFields[i].ColumnWidth);
				}
				else {
					col=new GridColumn(listDisplayFields[i].Description,listDisplayFields[i].ColumnWidth);
				}
				if(listDisplayFields[i].InternalName=="Th") {
					col.SortingStrategy=GridSortingStrategy.ToothNumberParse;
				}
				if(listDisplayFields[i].InternalName=="Date") {
					col.SortingStrategy=GridSortingStrategy.DateParse;
				}
				if(listDisplayFields[i].InternalName=="Amount") {
					col.SortingStrategy=GridSortingStrategy.AmountParse;
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(listDisplayFields[i].InternalName=="Proc Code"
					|| listDisplayFields[i].InternalName=="User"
					|| listDisplayFields[i].InternalName=="Signed"
					|| listDisplayFields[i].InternalName=="Locked"
					|| listDisplayFields[i].InternalName=="HL7 Sent")
				{
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridProg.Columns.Add(col);
			}
			if(gridProg.Columns.Count<3) {//0 wouldn't be possible.
				gridProg.NoteSpanStart=0;
				gridProg.NoteSpanStop=gridProg.Columns.Count-1;
			}
			else {
				gridProg.NoteSpanStart=2;
				if(gridProg.Columns.Count>7) {
					gridProg.NoteSpanStop=7;
				}
				else {
					gridProg.NoteSpanStop=gridProg.Columns.Count-1;
				}
			}
			gridProg.Rows.Clear();//Needed even when paging handles rows for us due to the change in columns and EndUpdate() logic.
			gridProg.EndUpdate();
			//Type type;
			if(LoadData==null) {
				FillToothChart(false);//?
				if(IsTPChartingAvailable) {
					FillListPriorities();//Mimics old ChartLayoutHelper logic
				}
				ToggleCheckTreatPlans();//Mimics old ChartLayoutHelper logic
				FillTreatPlans();
				FillTpProcs();
				return;
			}
			DataTable table=new DataTable();
			if(isSearch && _listSearchResults != null) {
				table=LoadData.TableProgNotes.Clone();
				for(int i=0;i<_listSearchResults.Count;i++) {
					table.Rows.Add(_listSearchResults[i].ItemArray);
				}
			}
			else {
				table=LoadData.TableProgNotes;
				textSearch.Text="";
			}
			if(doRefreshData || LoadData.ListProcGroupItems==null) {
				if(_patCur!=null) {
					LoadData.ListProcGroupItems=ProcGroupItems.Refresh(_patCur.PatNum);
				}
				else {
					LoadData.ListProcGroupItems=new List<ProcGroupItem>();
				}
			}
			_listRowsProcForGraphical=new List<DataRow>();
			List<long> listProcNums=new List<long>();//a list of all procNums of procs that will be visible
			if(checkShowTeeth.Checked) {
				//we will want to see groupnotes that are attached to any procs that should be visible.
				foreach(DataRow rowCur in table.Rows) {
					string procNumStr=rowCur["ProcNum"].ToString();
					if(procNumStr=="0") {//if this is not a procedure
						continue;
					}
					if(rowCur["ProcCode"].ToString()==ProcedureCodes.GroupProcCode) {
						continue;//skip procgroups
					}
					if(ShouldDisplayProc(rowCur)) {
						listProcNums.Add(PIn.Long(procNumStr));//remember that procnum
					}
				}
			}
			bool isSamePat=false;
			if(_patCur!=null) {
				if(gridProg.Tag!=null && ((long)gridProg.Tag)==_patCur.PatNum) {
					isSamePat=true;
				}
				gridProg.Tag=_patCur.PatNum;
			}
			List<int> listSelectedIndicies;
			//Filter out any DataRows that should not be displayed to the user.
			List<DataRow> listDataRows=table.Select()
				.Where(x => DoesGridProgRowPassFilter(x,LoadData.ListProcGroupItems,listProcNums))
				.ToList();
			if(isSamePat && listSelectedDataRows.Count==0 && !isForceFirstPage) {//Same patient without previously selected rows.
				listSelectedIndicies=new List<int>();//By passing in an empty list gridProg paging system will attempt to select the current page.
			}
			else if(isSamePat && listSelectedDataRows.Count>0 && !isForceFirstPage) {
				ODDataRowComparer comparer=new ODDataRowComparer();
				//Compare the current tag of every row in our grid to the previously selected rows.
				listSelectedIndicies=listSelectedDataRows.Select(x =>
					listDataRows.FindIndex(y => comparer.Equals(y,x))
				).ToList();
				//Remove any rows that were not found so that we either select a known row or try our best to preserve the current page.
				listSelectedIndicies.RemoveAll(x => x<0);
			}
			else {//New patient selected or same patient and forcing to go to first page, do not maintain page or selection.
				listSelectedIndicies=null;//null list will result in gridProd paging to go to first page.
			}
			gridProg.FuncConstructGridRow=((t) => GridProgRowConstruction((t as DataRow),listDisplayFields));
			gridProg.SetPagingData(listDataRows,listSelectedIndicies);
			if(IsTPChartingAvailable) {
				FillListPriorities();//Mimics old ChartLayoutHelper logic
			}
			ToggleCheckTreatPlans();//Mimics old ChartLayoutHelper logic
			List<long> listTreatPlanNums=new List<long>();
			if(_patCur!=null && gridTreatPlans.SelectedIndices.Length>0) {
				listTreatPlanNums=gridTreatPlans.SelectedIndices
					.Where(x => _listTreatPlans[x].PatNum==_patCur.PatNum)//must check PatNum because _listTreatPlans might be from previous patient
					.Select(x => _listTreatPlans[x].TreatPlanNum).ToList();
			}
			FillTreatPlans();
			for(int i=0;i<gridTreatPlans.Rows.Count && listTreatPlanNums.Count>0;i++) {
				gridTreatPlans.SetSelected(i,listTreatPlanNums.Contains(_listTreatPlans[i].TreatPlanNum));
			}
			if(gridTreatPlans.GetSelectedIndex()>-1) {
				gridTreatPlans.ScrollToIndex(gridTreatPlans.GetSelectedIndex());
			}
			FillTpProcs();
			//create a copy of the original _procList for filling the tooth chart with all procs. Deep copy of filtered list required.
			List<string> listProcNumStrs=_listRowsProcForGraphical.Select(x => x["ProcNum"].ToString()).ToList();
			_listProcListOrig=LoadData.TableProgNotes.Copy().Select().Where(x => listProcNumStrs.Contains(x["ProcNum"].ToString())).ToList();
			FillTrackSlider();
			FillToothChart(retainToothSelection);
			checkShowTeeth.Checked=showSelectedTeeth;
		}

		public void FillPtInfo(bool doRefreshData=true) {
			if(Plugins.HookMethod(this,"ContrChart.FillPtInfo",_patCur)) {
				return;
			}
			textTreatmentNotes.Text="";
			if(_patCur==null) {
				gridPtInfo.BeginUpdate();
				gridPtInfo.Rows.Clear();
				gridPtInfo.Columns.Clear();
				gridPtInfo.EndUpdate();
				TreatmentNoteChanged=false;
				return;
			}
			else {
				textTreatmentNotes.Text=_patientNoteCur.Treatment;
				textTreatmentNotes.Enabled=true;
				textTreatmentNotes.Select(textTreatmentNotes.Text.Length+2,1);
				textTreatmentNotes.ScrollToCaret();
				TreatmentNoteChanged=false;
			}
			gridPtInfo.BeginUpdate();
			gridPtInfo.Columns.Clear();
			GridColumn col=new GridColumn("",100);//"",);
			gridPtInfo.Columns.Add(col);
			col=new GridColumn("",50){ IsWidthDynamic=true };//HScrollVisible is false, dynamic col width.
			gridPtInfo.Columns.Add(col);
			gridPtInfo.Rows.Clear();
			GridCell cell;
			GridRow row;
			List<Definition> listMiscColorDefs=Definitions.GetDefsForCategory(DefinitionCategory.MiscColors);
			List<Definition> listMiscColorShortDefs=Definitions.GetDefsForCategory(DefinitionCategory.MiscColors,true);//Preserving old behavior.
			List<DisplayField> listDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.ChartPatientInformation);
			DisplayField fieldCur;
			for(int f=0;f<listDisplayFields.Count;f++) {
				fieldCur=listDisplayFields[f];
				row=new GridRow();
				//within a case statement, the row may be re-instantiated if needed, effectively removing the first cell added here:
				if(fieldCur.Description=="") {
					row.Cells.Add(fieldCur.InternalName);
				}
				else {
					row.Cells.Add(fieldCur.Description);
				}
				int ordinal=0;
				switch(fieldCur.InternalName) {
					#region ABC0
					case "ABC0":
						row.Cells.Add(_patCur.CreditType);
						break;
					#endregion ABC0
					#region Age
					case "Age":
						row.Cells.Add(PatientLogic.DateToAgeString(_patCur.Birthdate,_patCur.DateTimeDeceased));
						break;
					#endregion Age
					#region Allergies
					case "Allergies":
						if(doRefreshData || LoadData.ListAllergies==null) {
							LoadData.ListAllergies=Allergies.GetByPatient(_patCur.PatNum,false).ToList();
						}
						List<Allergy> listAllergies=LoadData.ListAllergies;
						row=new GridRow();
						cell=new GridCell();
						if(fieldCur.Description=="") {
							cell.Text=fieldCur.InternalName;
						}
						else {
							cell.Text=fieldCur.Description;
						}
						cell.Bold= true;
						row.Cells.Add(cell);
						row.BackColor=listMiscColorDefs[3].Color;
						row.Tag="tabAllergies";
						if(listAllergies.Count>0) {
							row.Cells.Add("");
							gridPtInfo.Rows.Add(row);
						}
						else {
							row.Cells.Add("none");
						}
						for(int i=0;listAllergies.Count>i;i++) {
							row=new GridRow();
							//In the instance that an AllergyDef is somehow deleted/removed from the DB, create an AllergyDef - display only - based upon the selected 
							//number. Populate it with a description that tells the user that the Allergy is missing. 
							//The user can click on the allergy in FormMedical and remedy the issue on their own.
							AllergyDef allergyDef=AllergyDefs.GetById(listAllergies[i].AllergyDefId);
							if(allergyDef==null) {
								allergyDef=new AllergyDef() {
									Id=listAllergies[i].AllergyDefId,
									Description="MISSING ALLERGY"
								};
							}
							cell=new GridCell(allergyDef.Description);
							cell.Bold= true;
							cell.ForeColor=Color.Red;
							row.Cells.Add(cell);
							row.Cells.Add(listAllergies[i].Reaction);
							row.BackColor=listMiscColorDefs[3].Color;
							row.Tag="tabAllergies";
							if(i!=listAllergies.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					#endregion Allergies
					#region AskToArriveEarly
					case "AskToArriveEarly":
						if(_patCur.AskToArriveEarly==0) {
							row.Cells.Add("");
						}
						else {
							row.Cells.Add(_patCur.AskToArriveEarly.ToString()+" minute(s)");
						}
						break;
					#endregion AskToArriveEarly
					#region Billing Type
					case "Billing Type":
						row.Cells.Add(Definitions.GetName(DefinitionCategory.BillingTypes,_patCur.BillingType));
						break;
					#endregion Billing Type
					#region Birthdate
					case "Birthdate":
						if(Preferences.GetBool(PreferenceName.PatientDOBMasked)) {
							row.Cells.Add(Patients.DOBFormatHelper(_patCur.Birthdate,true));
							row.Tag="DOB";//Used later to tell if we're right clicking on the DOB row
						}
						else {
							row.Cells.Add(Patients.DOBFormatHelper(_patCur.Birthdate,false));
						}
						break;
					#endregion Birthdate
					#region Broken Appts
					case "Broken Appts":
						row.Tag="Broken Appts";
						int count=0;
						DataTable table=LoadData.TableProgNotes;
						if(ProcedureCodes.IsValidCode("D9986")) {
							foreach(DataRow rowCur in table.Rows.OfType<DataRow>().Where(x => x["ProcNum"].ToString()!="0")) {
								if(PIn.String(rowCur["ProcCode"].ToString())=="D9986") {
									count++;
								}
							}
						}
						else {
							count=Adjustments.GetAdjustForPatByType(_patCur.PatNum,Preferences.GetLong(PreferenceName.BrokenAppointmentAdjustmentType)).Count;
						}
						row.Cells.Add(count.ToString());
						break;
					#endregion Broken Appts
					#region City
					case "City":
						row.Cells.Add(_patCur.City);
						break;
					#endregion City
					#region Date First Visit
					case "Date First Visit":
						if(_patCur.DateFirstVisit.Year<1880) {
							row.Cells.Add("??");
						}
						else if(_patCur.DateFirstVisit==DateTime.Today) {
							row.Cells.Add("NEW PAT");
						}
						else {
							row.Cells.Add(_patCur.DateFirstVisit.ToShortDateString());
						}
						row.Tag=null;
						break;
					#endregion Date First Visit
					#region Med Urgent
					case "Med Urgent":
						cell=new GridCell();
						cell.Text=_patCur.MedUrgNote;
						cell.ForeColor=Color.Red;
						cell.Bold= true;
						row.Cells.Add(cell);
						row.BackColor=listMiscColorDefs[3].Color;
						row.Tag="tabMedical";
						break;
					#endregion Med Urgent
					#region Medical Summary
					case "Medical Summary":
						row.Cells.Add(_patientNoteCur.Medical);
						row.BackColor=listMiscColorDefs[3].Color;
						row.Tag="tabMedical";
						break;
					#endregion Medical Summary
					#region Medications
					case "Medications":
						if(doRefreshData || LoadData.TableMeds==null) {
							Medications.RefreshCache();
						}
						else {
							Medications.FillCacheFromTable(LoadData.TableMeds);
						}
						if(doRefreshData || LoadData.ListMedPats==null) {
							LoadData.ListMedPats=MedicationPats.Refresh(_patCur.PatNum,false);
						}
						List<MedicationPat> listMeds=LoadData.ListMedPats;
						row=new GridRow();
						cell=new GridCell();
						if(fieldCur.Description=="") {
							cell.Text=fieldCur.InternalName;
						}
						else {
							cell.Text=fieldCur.Description;
						}
						cell.Bold= true;
						row.Cells.Add(cell);
						row.BackColor=listMiscColorDefs[3].Color;
						row.Tag="tabMedications";
						if(listMeds.Count>0) {
							row.Cells.Add("");
							gridPtInfo.Rows.Add(row);
						}
						else {
							row.Cells.Add("none");
						}
						string text;
						Medication medication;
						for(int i=0;i<listMeds.Count;i++) {
							row=new GridRow();
							if(listMeds[i].MedicationNum==0) {//NewCrop medication order.
								row.Cells.Add(listMeds[i].MedDescript);
							}
							else {
								medication=Medications.GetById(listMeds[i].MedicationNum);
								text=medication.Name;
								if(medication.Id != medication.GenericId && medication.GenericId.HasValue) {
									text+="("+Medications.GetById(medication.GenericId.Value).Name+")";
								}
								row.Cells.Add(text);
							}
							text=listMeds[i].PatNote;
							string noteMedGeneric="";
							if(listMeds[i].MedicationNum!=0) {
								noteMedGeneric=Medications.GetGeneric(listMeds[i].MedicationNum).Notes;
							}
							if(noteMedGeneric!="") {
								text+="("+noteMedGeneric+")";
							}
							row.Cells.Add(text);
							row.BackColor=listMiscColorDefs[3].Color;
							row.Tag="tabMedications";
							if(i!=listMeds.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					#endregion Medications
					#region PatFields
					case "PatFields":
						PatFieldL.AddPatFieldsToGrid(gridPtInfo,_arrayPatFields.ToList(),FieldLocations.Chart,(doRefreshData ? null : LoadData.ListFieldDefLinks));
						break;
					#endregion PatFields
					#region Pat Restrictions
					case "Pat Restrictions":
						if(doRefreshData || LoadData.ListPatRestricts==null) {
							LoadData.ListPatRestricts=PatRestrictions.GetAllForPat(_patCur.PatNum);
						}
						List<PatRestriction> listPatRestricts=LoadData.ListPatRestricts;
						if(listPatRestricts.Count==0) {
							row.Cells.Add("None");//row added outside of switch statement
						}
						for(int i=0;i<listPatRestricts.Count;i++) {
							row=new GridRow();
							if(string.IsNullOrWhiteSpace(fieldCur.Description)) {
								row.Cells.Add(fieldCur.InternalName);
							}
							else {
								row.Cells.Add(fieldCur.Description);
							}
							row.Cells.Add(PatRestrictions.GetPatRestrictDesc(listPatRestricts[i].PatRestrictType));
							row.BackColor=listMiscColorShortDefs[10].Color;//index 10 is Patient Restrictions (hard coded in convertdatabase4)
							if(i==listPatRestricts.Count-1) {//last row added outside of switch statement
								break;
							}
							gridPtInfo.Rows.Add(row);
						}
						break;
					#endregion Pat Restrictions
					#region Patient Portal
					case "Patient Portal":
						row.Tag="Patient Portal";
						bool hasAccess;
						if(doRefreshData) {
							hasAccess=Patients.HasPatientPortalAccess(_patCur.PatNum);
						}
						else {
							hasAccess=LoadData.HasPatientPortalAccess;
						}
						if(!hasAccess) {
							row.Cells.Add("No access");
						}
						else {
							row.Cells.Add("Online");
						}
						break;
					#endregion Patient Portal
					#region Payor Types
					case "Payor Types":
						row.Tag="Payor Types";
						if(doRefreshData || LoadData.PayorType==null) {
							LoadData.PayorType=PayorTypes.GetCurrentDescription(_patCur.PatNum);
						}
						row.Cells.Add(LoadData.PayorType);
						break;
					#endregion Payor Types
					#region Premedicate
					case "Premedicate":
						if(_patCur.Premed) {
							row=new GridRow();
							row.Cells.Add("");
							cell=new GridCell();
							if(fieldCur.Description=="") {
								cell.Text=fieldCur.InternalName;
							}
							else {
								cell.Text=fieldCur.Description;
							}
							cell.ForeColor=Color.Red;
							cell.Bold= true;
							row.Cells.Add(cell);
							row.BackColor=listMiscColorDefs[3].Color;
							row.Tag="tabMedical";
							gridPtInfo.Rows.Add(row);
						}
						break;
					#endregion Premedicate
					#region Pri Ins
					case "Pri Ins":
						string name;
						ordinal=PatPlans.GetOrdinal(PriSecMed.Primary,_listPatPlans,_listInsPlans,_listInsSubs);
						if(ordinal>0) {
							InsSub insSub=InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans,ordinal),_listInsSubs);
							name=InsPlans.GetCarrierName(insSub.PlanNum,_listInsPlans);
							if(_listPatPlans[0].IsPending) {
								name+=" (pending)";
							}
							row.Cells.Add(name);
						}
						else {
							row.Cells.Add("");
						}
						row.Tag=null;
						break;
					#endregion Pri Ins
					#region Problems
					case "Problems":
						if(doRefreshData || LoadData.ListDiseases==null) {
							LoadData.ListDiseases=Problems.GetByPatient(_patCur.PatNum,true).ToList();
						}
						List<Problem> listDiseases=LoadData.ListDiseases;
						row=new GridRow();
						cell=new GridCell();
						if(fieldCur.Description=="") {
							cell.Text=fieldCur.InternalName;
						}
						else {
							cell.Text=fieldCur.Description;
						}
						cell.Bold= true;
						row.Cells.Add(cell);
						row.BackColor=listMiscColorDefs[3].Color;
						row.Tag="tabProblems";
						if(listDiseases.Count>0) {
							row.Cells.Add("");
							gridPtInfo.Rows.Add(row);
						}
						else {
							row.Cells.Add("none");
						}
						//Add a new row for each med.
						for(int i=0;i<listDiseases.Count;i++) {
							row=new GridRow(); 
							if(listDiseases[i].ProblemDefId!=0) {
								cell=new GridCell(ProblemDefinitions.GetName(listDiseases[i].ProblemDefId));
								cell.ForeColor=Color.Red;
								cell.Bold= true;
								row.Cells.Add(cell);
								row.Cells.Add(listDiseases[i].PatientNote);
							}
							else {
								row.Cells.Add("");
								cell=new GridCell(ProblemDefinitions.GetItem(listDiseases[i].ProblemDefId)?.Description??"INVALID PROBLEM");
								cell.ForeColor=Color.Red;
								cell.Bold= true;
								row.Cells.Add(cell);
								//row.Cells.Add(DiseaseList[i].PatNote);//no place to show a pat note
							}
							row.BackColor=listMiscColorDefs[3].Color;
							row.Tag="tabProblems";
							if(i!=listDiseases.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					#endregion Problems
					#region Prov. (Pri, Sec)
					case "Prov. (Pri, Sec)":
						string provText="";
						if(_patCur.PriProv!=0) {
							provText+=Providers.GetAbbr(_patCur.PriProv)+", ";							
						}
						else {
							provText+="None"+", ";
						}
						if(_patCur.SecProv != 0) {
							provText+=Providers.GetAbbr(_patCur.SecProv);
						}
						else {
							provText+="None";
						}
						row.Cells.Add(provText);
						row.Tag = null;
						break;
					#endregion Prov. (Pri, Sec)
					#region References
					case "References":
						List<CustRefEntry> listCustRefEntries=CustRefEntries.GetEntryListForCustomer(_patCur.PatNum);
						if(listCustRefEntries.Count==0) {
							row.Cells.Add("None");
							row.Tag="References";
							row.BackColor=listMiscColorShortDefs[8].Color;
						}
						else {
							row.Cells.Add("");
							row.Tag="References";
							row.BackColor=listMiscColorShortDefs[8].Color;
							gridPtInfo.Rows.Add(row);
						}
						for(int i=0;i<listCustRefEntries.Count;i++) {
							row=new GridRow();
							row.Cells.Add(listCustRefEntries[i].DateEntry.ToShortDateString());
							row.Cells.Add(CustReferences.GetCustNameFL(listCustRefEntries[i].PatNumRef));
							row.Tag=listCustRefEntries[i];
							row.BackColor=listMiscColorShortDefs[8].Color;
							if(i<listCustRefEntries.Count-1) {
								gridPtInfo.Rows.Add(row);
							}
						}
						break;
					#endregion References
					#region Referred From
					case "Referred From":
						if(doRefreshData || LoadData.ListRefAttaches==null) {
							LoadData.ListRefAttaches=RefAttaches.Refresh(_patCur.PatNum).DistinctBy(x => x.ReferralNum).ToList();
						}
						List<RefAttach> listRefAttach=LoadData.ListRefAttaches;
						string referral="";
						for(int i=0;i<listRefAttach.Count;i++) {
							if(listRefAttach[i].RefType==ReferralType.RefFrom) {
								referral=Referrals.GetNameLF(listRefAttach[i].ReferralNum);
								break;
							}
						}
						if(referral=="") {
							referral="??";
						}
						row.Cells.Add(referral);
						row.Tag="Referral";
						break;
					#endregion Referred From
					#region Registration Keys
					case "Registration Keys":
						//Not even available to most users.
						RegistrationKey[] arrayKeys=RegistrationKeys.GetForPatient(_patCur.PatNum);
						for(int i=0;i<arrayKeys.Length;i++) {
							//For non-guarantors with reseller keys, we do not want to show other family member reseller keys (there will be a lot of them).
							if(_patCur.PatNum!=_patCur.Guarantor
								&& arrayKeys[i].IsResellerCustomer 
								&& arrayKeys[i].PatNum!=_patCur.PatNum) 
							{
								//The current patient selected is not the guarantor and this is a reseller key for another family member.  Do not show it in this patient's chart module.
								continue;
							}
							row=new GridRow();
							row.Cells.Add("Registration Key");
							string str=arrayKeys[i].RegKey.Substring(0,4)+"-"+arrayKeys[i].RegKey.Substring(4,4)+"-"
								+arrayKeys[i].RegKey.Substring(8,4)+"-"+arrayKeys[i].RegKey.Substring(12,4);
							str+="  |  PatNum: "+arrayKeys[i].PatNum.ToString();//Always show the PatNum
							if(arrayKeys[i].IsForeign) {
								str+="\r\nForeign";
							}
							else {
								str+="\r\nUSA";
							}
							str+="\r\nStarted: "+arrayKeys[i].DateStarted.ToShortDateString();
							if(arrayKeys[i].DateDisabled.Year>1880) {
								str+="\r\nDisabled: "+arrayKeys[i].DateDisabled.ToShortDateString();
							}
							if(arrayKeys[i].DateEnded.Year>1880) {
								str+="\r\nEnded: "+arrayKeys[i].DateEnded.ToShortDateString();
							}
							if(arrayKeys[i].Note!="") {
								str+=arrayKeys[i].Note;
							}
							row.Cells.Add(str);
							row.Tag=arrayKeys[i].Copy();
							gridPtInfo.Rows.Add(row);
						}
						break;
					#endregion Registration Keys
					#region Sec Ins
					case "Sec Ins":
						ordinal=PatPlans.GetOrdinal(PriSecMed.Secondary,_listPatPlans,_listInsPlans,_listInsSubs);
						if(ordinal>0) {
							InsSub insSub=InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans,ordinal),_listInsSubs);
							name=InsPlans.GetCarrierName(insSub.PlanNum,_listInsPlans);
							if(_listPatPlans[1].IsPending) {
								name+=" (pending)";
							}
							row.Cells.Add(name);
						}
						else {
							row.Cells.Add("");
						}
						row.Tag=null;
						break;
					#endregion Sec Ins
					#region Service Notes
					case "Service Notes":
						row.Cells.Add(_patientNoteCur.Service);
						row.BackColor=listMiscColorDefs[3].Color;
						row.Tag="tabMedical";
						break;
					#endregion Service Notes
					#region Specialty
					case "Specialty":
						row.Cells.Add(Patients.GetPatientSpecialtyDef(_patCur.PatNum)?.Name??"");
						row.Tag=null;
						break;
					#endregion Specialty
					#region Super Head
					case "Super Head":
						if(doRefreshData || LoadData.SuperFamHead==null) {
							LoadData.SuperFamHead=Patients.GetPat(_patCur.SuperFamily);
						}
						if(_patCur.SuperFamily!=0) {
							Patient patTempSuper=LoadData.SuperFamHead;
							row.Cells.Add(patTempSuper.GetNameLF()+" ("+patTempSuper.PatNum+")");
						}
						else {
							continue;//do not allow this row to be added if there is no data to in the row.
						}
						break;
					#endregion Super Head
					#region Tobacco Use (Patient Smoking Status)
					case "Tobacco Use":
						if(!Preferences.GetBool(PreferenceName.ShowFeatureEhr)) {
							continue;
						}
						if(doRefreshData || LoadData.ListTobaccoStatuses==null) {
							LoadData.ListTobaccoStatuses=EhrMeasureEvents.GetByPatient(_patCur.PatNum,EhrMeasureEventType.TobaccoUseAssessed).ToList();

						}
						List<EhrMeasureEvent> listTobaccoStatuses=LoadData.ListTobaccoStatuses
							.OrderByDescending(x => x.Date).Take(3).ToList();//only display the last three assessments at most
						row=new GridRow() { BackColor=listMiscColorDefs[3].Color,Tag="tabTobaccoUse" };
						row.Cells.Add(new GridCell(Text=fieldCur.Description==""?fieldCur.InternalName:fieldCur.Description) { Bold= true });
						row.Cells.Add(listTobaccoStatuses.Count>0?"":"none");
						if(listTobaccoStatuses.Count>0) {
							gridPtInfo.Rows.Add(row);
						}
						Snomed snmCur;
						for(int i=0;i<listTobaccoStatuses.Count;i++) {//show the last three tobacco use assessments at most
							EhrMeasureEvent ehrCur=listTobaccoStatuses[i];
							row=new GridRow() { BackColor=listMiscColorDefs[3].Color,Tag="tabTobaccoUse" };
							snmCur=Snomeds.GetByCode(ehrCur.ResultCode);
							row.Cells.Add(snmCur!=null?snmCur.Description:"");
							row.Cells.Add(ehrCur.Date.ToShortDateString()+(ehrCur.MoreInfo==""?"":(" - "+ehrCur.MoreInfo)));
							if(i==listTobaccoStatuses.Count-1) {
								break;//don't add last row here, handled outside of switch statement
							}
							gridPtInfo.Rows.Add(row);
						}
						break;
						#endregion Tobacco Use (Patient Smoking Status)
				}
				if(new[] { "PatFields","Premedicate","Registration Keys" }.Contains(fieldCur.InternalName)) {
					//For fields that might have zero rows, we can't add the row here.  Adding rows is instead done in the case clause.
					//But some fields that are based on lists will always have one row, even if there are no items in the list.
					//Do not add those kinds here.
				}
				else {
					gridPtInfo.Rows.Add(row);
				}
			}
			gridPtInfo.EndUpdate();
		}

		public void FunctionKeyPressContrChart(Keys keys) {
			List<ChartView> listChartViews=ChartViews.GetDeepCopy();
			switch(keys) {
				case Keys.F1: 
					if(gridChartViews.Rows.Count>0) {
						gridChartViews.SetSelected(0,true);
						SetChartView(listChartViews[0]);
					}
					break;
				case Keys.F2:
					if(gridChartViews.Rows.Count>1) {
						gridChartViews.SetSelected(1,true);
						SetChartView(listChartViews[1]);
					}
					break;
				case Keys.F3:
					if(gridChartViews.Rows.Count>2) {
						gridChartViews.SetSelected(2,true);
						SetChartView(listChartViews[2]);
					}
					break;
				case Keys.F4:
					if(gridChartViews.Rows.Count>3) {
						gridChartViews.SetSelected(3,true);
						SetChartView(listChartViews[3]);
					}
					break;
				case Keys.F5:
					if(gridChartViews.Rows.Count>4) {
						gridChartViews.SetSelected(4,true);
						SetChartView(listChartViews[4]);
					}
					break;
				case Keys.F6:
					if(gridChartViews.Rows.Count>5) {
						gridChartViews.SetSelected(5,true);
						SetChartView(listChartViews[5]);
					}
					break;
				case Keys.F7:
					if(gridChartViews.Rows.Count>6) {
						gridChartViews.SetSelected(6,true);
						SetChartView(listChartViews[6]);
					}
					break;
				case Keys.F8:
					if(gridChartViews.Rows.Count>7) {
						gridChartViews.SetSelected(7,true);
						SetChartView(listChartViews[7]);
					}
					break;
				case Keys.F9:
					if(gridChartViews.Rows.Count>8) {
						gridChartViews.SetSelected(8,true);
						SetChartView(listChartViews[8]);
					}
					break;
				case Keys.F10:
					if(gridChartViews.Rows.Count>9) {
						gridChartViews.SetSelected(9,true);
						SetChartView(listChartViews[9]);
					}
					break;
				case Keys.F11:
					if(gridChartViews.Rows.Count>10) {
						gridChartViews.SetSelected(10,true);
						SetChartView(listChartViews[10]);
					}
					break;
				case Keys.F12:
					if(gridChartViews.Rows.Count>11) {
						gridChartViews.SetSelected(11,true);
						SetChartView(listChartViews[11]);
					}
					break;
			}
		}

		///<summary>Called every time prefs are changed from any workstation.</summary>
		public void InitializeLocalData()
		{
			butAddKey.Visible = false;
			butForeignKey.Visible = false;
			butPhoneNums.Visible = false;
			butErxAccess.Visible = false;
			tabProc.TabPages.Remove(tabCustomer);

				//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
				toothChartWrapper.UseHardware = ComputerPrefs.LocalComputer.GraphicsUseHardware;
				toothChartWrapper.PreferredPixelFormatNumber = ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
				toothChartWrapper.DeviceFormat = new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
				//Must be last preference set here, because this causes the pixel format to be recreated.																											
				toothChartWrapper.DrawMode = ComputerPrefs.LocalComputer.GraphicsSimple;
				//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
				ComputerPrefs.LocalComputer.PreferredPixelFormatNum = toothChartWrapper.PreferredPixelFormatNumber;
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			
			if (_patCur != null)
			{
				FillToothChart(true);
			}
			//if(Prefs.GetBool(PrefName.ChartQuickAddHideAmalgam,true)){ //Preference is Deprecated.
			//	panelQuickPasteAmalgam.Visible=false;
			//}
			//else{
			//	panelQuickPasteAmalgam.Visible=true;
			//}
			if (!ToolButItems.GetCacheIsNull())
			{
				LayoutToolBar();
				if (_patCur == null)
				{
					if (HasHideRxButtonsEcw())
					{
						//Don't show the Rx and eRx buttons.
					}
					else
					{
						ToolBarMain.Buttons["Rx"].Enabled = false;
						ToolBarMain.Buttons["eRx"].Enabled = false;
					}
					ToolBarMain.Buttons["LabCase"].Enabled = false;
					if (ToolBarMain.Buttons["Perio"] != null)
					{
						ToolBarMain.Buttons["Perio"].Enabled = false;
					}
					if (ToolBarMain.Buttons["Ortho"] != null)
					{
						ToolBarMain.Buttons["Ortho"].Enabled = false;
					}
					ToolBarMain.Buttons["Consent"].Enabled = false;
					if (ToolBarMain.Buttons["ToothChart"] != null)
					{
						ToolBarMain.Buttons["ToothChart"].Enabled = false;
					}
					ToolBarMain.Buttons["ExamSheet"].Enabled = false;
					if (Preferences.GetBool(PreferenceName.ShowFeatureEhr))
					{
						ToolBarMain.Buttons["EHR"].Enabled = false;
					}
					if (ToolBarMain.Buttons["HL7"] != null)
					{
						ToolBarMain.Buttons["HL7"].Enabled = false;
					}
				}
			}
		}

		///<summary></summary>
		public void InitializeOnStartup() {
			if(_initializedOnStartup) {
				return;
			}
			_initializedOnStartup=true;

			_toothChartRelay= new ToothChartRelay();//IsSparks3DPresent could have been set back to false here
			_toothChartRelay.SetToothChartWrapper(toothChartWrapper);


				toothChartWrapper.Visible=true;
				//ComputerPref localComputerPrefs=ComputerPrefs.GetForLocalComputer();
				this.toothChartWrapper.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
				this.toothChartWrapper.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;//triggers ResetControls.
			
			_procStatusNew=ProcStat.TP;
			if(IsTPChartingAvailable) {
				FillListPriorities();//Mimics old ChartLayoutHelper logic
			}
			ToggleCheckTreatPlans();//Mimics old ChartLayoutHelper logic
			LayoutToolBar();
			//Passed-in controls will maintain their location and be shown but are not part of the dynamic layout fields.
			_sheetLayoutController=new SheetLayoutController(this,ToolBarMain,tabControlImages,panelImages);
			ReloadSheetLayout();//First time loading.
			if(Programs.UsingOrion) {
				tabProc.SelectedTab=tabPatInfo;
			}
			Plugins.HookAddCode(this,"ContrChart.InitializeOnStartup_end",_patCur);
		}

		///<summary>Causes the toolbars to be laid out again.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			if (HasHideRxButtonsEcw())
			{
				//Don't show the Rx and eRx buttons.
			}
			else
			{

				//ToolBarMain.Buttons.Add(new ODToolBarButton("New Rx",1,"","Rx"));
				button = new ODToolBarButton("New Rx", 1, "", "Rx");
				button.Style = ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu = _contextMenuRxManage;
				ToolBarMain.Buttons.Add(button);
				if (_butErx == null)
				{
					_butErx = new ODToolBarButton("eRx", 1, "", "eRx");
					_butErx.Style = ODToolBarButtonStyle.DropDownButton;
					_butErx.DropDownMenu = menuErx;
				}
				ToolBarMain.Buttons.Add(_butErx);

			}
			ToolBarMain.Buttons.Add(new ODToolBarButton("LabCase",-1,"","LabCase"));
			if(!Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton("Perio Chart",2,"","Perio"));
			}
			button=new ODToolBarButton(OrthoChartTabs.GetFirst(true).TabName,-1,"","Ortho");
			if(OrthoChartTabs.GetCount(true)>1) {
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuOrthoChart;
			}
			ToolBarMain.Buttons.Add(button);
			button=new ODToolBarButton("Consent",-1,"","Consent");
			if(SheetDefs.GetCustomForType(SheetTypeEnum.Consent).Count>0) {
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuConsent;
			}
			ToolBarMain.Buttons.Add(button);
			//if(Prefs.GetBool(PrefName.ToothChartMoveMenuToRight)) {
			//	ToolBarMain.Buttons.Add(new ODToolBarButton(".",-1,"",""));
			//}
			if(!Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				button=new ODToolBarButton("Tooth Chart",-1,"","ToothChart");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuToothChart;
				ToolBarMain.Buttons.Add(button);
			}
			button=new ODToolBarButton("Exam Sheet",-1,"","ExamSheet");
			button.Style=ODToolBarButtonStyle.PushButton;
			ToolBarMain.Buttons.Add(button);
			if(Preferences.GetBool(PreferenceName.ShowFeatureEhr)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton("EHR",-1,"","EHR"));
			}
			HL7Def hl7Def=HL7Defs.GetOneDeepEnabled();
			if(hl7Def!=null) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(hl7Def.Description,-1,"","HL7"));
			}
			HL7Def hl7DefMedLab=HL7Defs.GetOneDeepEnabled(true);
			if(hl7DefMedLab!=null) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(hl7DefMedLab.Description,-1,"","MedLab"));
			}
			if(_sheetLayoutController!=null && _sheetLayoutController.ListSheetDefsLayout.Count>0) {
				button=new ODToolBarButton("Layout",-1,"","Layout");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				List<MenuItem> listMenuItems=new List<MenuItem>(new[] { new MenuItem("Add/Edit Layouts",LayoutMenuItem_Click),new MenuItem("-") });
				long selectedLayoutSheetDefNum=_sheetLayoutController.GetLayoutForUser().SheetDefNum;
				listMenuItems.AddRange(
					_sheetLayoutController.ListSheetDefsLayout.FindAll(x => x.SheetDefNum>0)//add all custom SheetDefs
						.Select(x => new MenuItem(x.Description,LayoutMenuItem_Click) { Tag=x,Checked=(selectedLayoutSheetDefNum==x.SheetDefNum) })
				);
				if(_sheetLayoutController.ListSheetDefsLayout.Any(x => x.SheetDefNum>0)) {//Menu has at least one custom layout def
					listMenuItems.Add(new MenuItem("-"));//add separator between custom and internal
				}
				SheetDef sheetDefInternalLayout=_sheetLayoutController.ListSheetDefsLayout.FirstOrDefault(x => x.SheetDefNum==0);
				if(sheetDefInternalLayout!=null) {//Not sure how this could be null, but we've had bug submissions for it.
					listMenuItems.Add(
						new MenuItem(sheetDefInternalLayout.Description,LayoutMenuItem_Click) { Tag=sheetDefInternalLayout,Checked=(selectedLayoutSheetDefNum==0) }
					);
				}
				button.DropDownMenu=new ContextMenu(listMenuItems.ToArray());
				ToolBarMain.Buttons.Add(button);
			}
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.ChartModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrChart.LayoutToolBar_end",_patCur);
		}

		/// <summary></summary>
		public void ModuleSelected(long patNum) {
			ModuleSelected(patNum,true,false);
		}

		/// <summary>Only use this overload when isClinicRefresh is set to true.  This is only used when calling ModuleSelected from FromOpenDental. When isClinicRefresh is true the tab control tabProc is redrawn and only needs to be done when the clinic is changed or the module is selected for the first time.</summary>
		public void ModuleSelected(long patNum,bool isClinicRefresh) {
			ModuleSelected(patNum,true,isClinicRefresh);
		}

		///<summary>Only use this overload when isFullRefresh is set to false.  This is ONLY in a few places and only for eCW at this point.  Speeds things up by refreshing less data.</summary>
		public void ModuleSelected(long patNum,bool isFullRefresh,bool isClinicRefresh) {
			EasyHideClinicalData();
			Logger.LogAction("RefreshModuleData",() => RefreshModuleData(patNum,isFullRefresh));
			Logger.LogAction("RefreshModuleScreen",() => RefreshModuleScreen(isClinicRefresh));//Update UI to reflect any changed dynamic SheetDefs.
			ReloadSheetLayout();//Module selected
			Plugins.HookAddCode(this,"ContrChart.ModuleSelected_end",patNum);
		}

		///<summary>This function does not follow our usual pattern. This function is just like ModuleSelected() but also syncs any eRx data which needs to be checked frequently.  Only called from FormOpenDental when the Chart module button is clicked or a new patient is selected while inside the Chart.</summary>
		public void ModuleSelectedErx(long patNum) {
			ModuleSelected(patNum,true);
			RefreshDoseSpotNotifications();
		}

		///<summary></summary>
		public void ModuleUnselected(bool isLoggingOff=false) {
			//toothChart.Dispose();?
			UpdateTreatmentNote();
			if(!isLoggingOff) {
				PlannedApptPromptHelper();
			}
			_famCur=null;
			_patCur=null;
			_listInsPlans=null;
			_listSubstitutionLinks=null;
			_listInsSubs=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			gridPtInfo.ContextMenu=new ContextMenu();//This module is never really disposed. Get rid of any menu options we added, to avoid duplicates.
			Plugins.HookAddCode(this,"ContrChart.ModuleUnselected_end");
		}

		public void RefreshModuleScreen(bool isClinicRefresh = false)
		{
			//ParentForm.Text=Patients.GetMainTitle(PatCur);
			LayoutToolBar();
			if (_patCur == null)
			{
				//groupShow.Enabled=false;
				gridPtInfo.Enabled = false;
				//tabPlanned.Enabled=false;
				_toothChartRelay.Enabled = false;
				gridProg.Enabled = false;
				if (HasHideRxButtonsEcw())
				{
					//Don't show the Rx and eRx buttons.
				}
				else
				{
					ToolBarMain.Buttons["Rx"].Enabled = false;
					ToolBarMain.Buttons["eRx"].Enabled = false;
				}
				ToolBarMain.Buttons["LabCase"].Enabled = false;
				if (ToolBarMain.Buttons["Perio"] != null)
				{
					ToolBarMain.Buttons["Perio"].Enabled = false;
				}
				if (ToolBarMain.Buttons["Ortho"] != null)
				{
					ToolBarMain.Buttons["Ortho"].Enabled = false;
				}
				ToolBarMain.Buttons["Consent"].Enabled = false;
				if (ToolBarMain.Buttons["ToothChart"] != null)
				{
					ToolBarMain.Buttons["ToothChart"].Enabled = false;
				}
				ToolBarMain.Buttons["ExamSheet"].Enabled = false;
				//if(FormOpenDental.ObjSomeEhrSuperClass!=null) {//didn't work
				if (ToolBarMain.Buttons["EHR"] != null)
				{
					ToolBarMain.Buttons["EHR"].Enabled = false;
				}
				if (ToolBarMain.Buttons["HL7"] != null)
				{
					ToolBarMain.Buttons["HL7"].Enabled = false;
				}
				tabProc.Enabled = false;
				butAddKey.Enabled = false;
				butForeignKey.Enabled = false;
				butPhoneNums.Enabled = false;
				butErxAccess.Enabled = false;
				trackToothProcDates.Enabled = false;
				textToothProcDate.Enabled = false;
				textSearch.Text = "";
			}
			else
			{
				trackToothProcDates.Enabled = true;
				textToothProcDate.Enabled = true;
				//groupShow.Enabled=true;
				gridPtInfo.Enabled = true;
				//groupPlanned.Enabled=true;
				_toothChartRelay.Enabled = true;
				gridProg.Enabled = true;
				if (HasHideRxButtonsEcw())
				{
					//Don't show the Rx and eRx buttons.
				}
				else
				{
					ToolBarMain.Buttons["Rx"].Enabled = true;
					ToolBarMain.Buttons["eRx"].Enabled = true;
				}
				ToolBarMain.Buttons["LabCase"].Enabled = true;
				if (ToolBarMain.Buttons["Perio"] != null)
				{
					ToolBarMain.Buttons["Perio"].Enabled = true;
				}
				if (ToolBarMain.Buttons["Ortho"] != null)
				{
					ToolBarMain.Buttons["Ortho"].Enabled = true;
				}
				ToolBarMain.Buttons["Consent"].Enabled = true;
				if (ToolBarMain.Buttons["ToothChart"] != null)
				{
					ToolBarMain.Buttons["ToothChart"].Enabled = true;
				}
				ToolBarMain.Buttons["ExamSheet"].Enabled = true;

				if (Preferences.GetBool(PreferenceName.ShowFeatureEhr))
				{ //didn't work either
				  //if(ToolBarMain.Buttons["EHR"]!=null) {
					ToolBarMain.Buttons["EHR"].Enabled = true;
				}
				if (ToolBarMain.Buttons["HL7"] != null)
				{
					ToolBarMain.Buttons["HL7"].Enabled = true;
				}
				tabProc.Enabled = true;
				butAddKey.Enabled = true;
				butForeignKey.Enabled = true;
				butPhoneNums.Enabled = true;
				butErxAccess.Enabled = true;
				if (_prevPatNum != _patCur.PatNum)
				{//reset to TP status on every new patient selected
					if (Preferences.GetBool(PreferenceName.AutoResetTPEntryStatus))
					{
						radioEntryTP.Select();
					}
					textSearch.Text = "";
					_prevPatNum = _patCur.PatNum;
				}
				if (Preferences.GetBool(PreferenceName.PatientDOBMasked))
				{
					//Add "View DOB" right click option, MenuItemPopupUnmaskDOB will show and hide it as needed.
					if (gridPtInfo.ContextMenu == null)
					{
						gridPtInfo.ContextMenu = new ContextMenu();//ODGrid will automatically attach the defaut Popups
					}
					ContextMenu contextMenu = gridPtInfo.ContextMenu;
					MenuItem menuItemUnmaskDOB = new MenuItem();
					menuItemUnmaskDOB.Enabled = false;
					menuItemUnmaskDOB.Visible = false;
					menuItemUnmaskDOB.Name = "ViewDOB";
					menuItemUnmaskDOB.Text = "View DOB";
					menuItemUnmaskDOB.Click += new System.EventHandler(this.MenuItemUnmaskDOB_Click);
					contextMenu.MenuItems.Add(menuItemUnmaskDOB);
					contextMenu.Popup += MenuItemPopupUnmaskDOB;
				}
			}
			if (Programs.UsingOrion)
			{
				radioEntryC.Visible = false;
				radioEntryEC.Visible = false;
				radioEntryR.Visible = false;
				radioEntryCn.Visible = false;
				radioEntryEO.Location = new Point(radioEntryEO.Location.X, 31);
				groupBox2.Height = 54;
				menuItemSetComplete.Visible = false;
				menuItemSetEC.Visible = false;
				menuItemSetEO.Visible = false;
			}

			if (isClinicRefresh)
			{
				if (Clinics.IsMedicalClinic(Clinics.ClinicId))
				{
					tabProc.TabPages.Remove(tabMissing);
					tabProc.TabPages.Remove(tabMovements);
					tabProc.TabPages.Remove(tabPrimary);
					if (tabProc.SelectedTab == tabMissing || tabProc.SelectedTab == tabMovements || tabProc.SelectedTab == tabPrimary)
					{
						tabProc.SelectedTab = tabEnterTx;
					}
					_selectedProcTab = tabProc.SelectedIndex;
					textSurf.Visible = false;
					butBF.Visible = false;
					butOI.Visible = false;
					butV.Visible = false;
					butM.Visible = false;
					butD.Visible = false;
					butL.Visible = false;
					checkShowTeeth.Visible = false;
				}
				else
				{
					TabPage selectedTab = tabProc.SelectedTab;
					tabProc.TabPages.Remove(tabMissing);
					tabProc.TabPages.Remove(tabMovements);
					tabProc.TabPages.Remove(tabPrimary);
					tabProc.TabPages.Remove(tabPlanned);
					tabProc.TabPages.Remove(tabShow);
					tabProc.TabPages.Remove(tabDraw);
					tabProc.TabPages.Add(tabMissing);
					tabProc.TabPages.Add(tabMovements);
					tabProc.TabPages.Add(tabPrimary);
					tabProc.TabPages.Add(tabPlanned);
					tabProc.TabPages.Add(tabShow);
					tabProc.TabPages.Add(tabDraw);
					tabProc.SelectedTab = selectedTab;
					_selectedProcTab = tabProc.SelectedIndex;
					textSurf.Visible = true;
					butBF.Visible = true;
					butOI.Visible = true;
					butV.Visible = true;
					butM.Visible = true;
					butD.Visible = true;
					butL.Visible = true;
					checkShowTeeth.Visible = true;
				}
			}
			ToolBarMain.Invalidate();
			ClearButtons();
			FillMovementsAndHidden();

			Logger.LogAction("FillChartViewsGrid", () => FillChartViewsGrid(false));
			if (textSearch.Text != "")
			{
				_listSearchResults?.Clear();
				_listSearchResults = null;
				SearchProgNotes();
			}
			else
			{
				Logger.LogAction("FillProgNotes", () => FillProgNotes(doRefreshData: false));
			}

			Logger.LogAction("FillPlanned", () => FillPlanned());
			Logger.LogAction("FillPtInfo", () => FillPtInfo(false));
			Logger.LogAction("FillDxProcImage", () => FillDxProcImage(false));
			Logger.LogAction("FillImages", () => FillImages());
		}

		public void UpdateTreatmentNote() {
			if(_famCur==null) {
				return;
			}
			if(TreatmentNoteChanged) {
				_patientNoteCur.Treatment=textTreatmentNotes.Text;
				PatientNotes.Update(_patientNoteCur,_patCur.Guarantor);
				TreatmentNoteChanged=false;
			}
		}
		#endregion Methods - Public

		#region Methods - Private - General
		///<summary>Inserts TreatPlanAttaches that allows the procedure to be charted to one or more treatment plans at the same time.</summary>
		private void AttachProcToTPs(Procedure proc) {
			if(proc.ProcStatus!=ProcStat.TP && proc.ProcStatus!=ProcStat.TPi) {
				return;
			}
			List<long> listTpNums=new List<long>();
			if(gridTreatPlans.GetSelectedIndex()>=0) {
				listTpNums=gridTreatPlans.SelectedIndices.Select(x => _listTreatPlans[x].TreatPlanNum).ToList();
			}
			_listTreatPlans=TreatPlans.GetAllCurrentForPat(_patCur.PatNum);
			//If there is no active TP, make sure to add an active treatment plan.
			if(_listTreatPlans.All(x => x.TPStatus!=TreatPlanStatus.Active)) {
				TreatPlan activeTreatPlan=new TreatPlan() {
					Heading="Active Treatment Plan",
					Note=Preferences.GetString(PreferenceName.TreatmentPlanNote),
					TPStatus=TreatPlanStatus.Active,
					PatNum=_patCur.PatNum,
					TPType=_patCur.DiscountPlanNum==0 ? TreatPlanType.Insurance : TreatPlanType.Discount
				};
				activeTreatPlan.TreatPlanNum=TreatPlans.Insert(activeTreatPlan);
				_listTreatPlans=new List<TreatPlan>() { activeTreatPlan };
				listTpNums.Add(activeTreatPlan.TreatPlanNum);
			}
			else if(listTpNums.Count==0) {//NOT treat plan charting so no TP selected, active plan exists so get TPNum from active plan
				listTpNums.Add(_listTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Active).TreatPlanNum);
			}
			long priorityNum=0;
			if(comboPriority.SelectedIndex>0) {
				priorityNum=comboPriority.GetSelectedDefNum();
			}
			listTpNums.ForEach(x => TreatPlanAttaches.Insert(new TreatPlanAttach() { TreatPlanNum=x,ProcNum=proc.ProcNum,Priority=priorityNum }));
			//if all treatplans selected are not the active treatplan, then chart proc as status TPi
			//we know there is an active plan for the patient at this point
			if(_listTreatPlans.FindAll(x => listTpNums.Contains(x.TreatPlanNum)).All(x => x.TPStatus!=TreatPlanStatus.Active)) {
				Procedure procOld=proc.Copy();
				proc.ProcStatus=ProcStat.TPi;//change proc status to TPi if all selected plans are Inactive status
				Procedures.Update(proc,procOld);
			}
		}

		///<summary>Updates estimates for given parent procedure.</summary
		private void CanadianLabFeeHelper(long procNumParent) {
			if(procNumParent==0){
				return;//Should not happen.
			}
			if(_listPatProcs==null) {
				_listPatProcs=Procedures.Refresh(_patCur.PatNum);
			}
			Procedure procParent=Procedures.GetProcFromList(_listPatProcs,procNumParent);
			if(procParent==null) {//A null parent proc could happen in rare cases for older databases.
				return;
			}
			if(procParent.ProcNum==0) {//Should never happen.
				return;
			}
			if(Procedures.IsAttachedToClaim(procParent.ProcNum)) {//If attached to a claim, then user should recreate claim because estimates will be inaccurate not matter what.
				return;
			}
			Procedures.ComputeEstimates(procParent,_patCur.PatNum,ClaimProcs.RefreshForProc(procParent.ProcNum),false,_listInsPlans,_listPatPlans,_listBenefits,_patCur.Age,_listInsSubs);
		}

		private bool CanAddGroupNote(bool doCheckDb,bool isSilent,List<Procedure> listProcs) {
			DataRow row;
			if(gridProg.SelectedIndices.Length==0) {
				if(!isSilent) {
					MessageBox.Show("Please select procedures to attach a group note to."); 
				}
				return false;
			}
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(!CanGroupRow(row,doCheckDb:doCheckDb,isSilent:isSilent,listProcs)) {
					return false;
				}
			}
			//Validate the list of procedures------------------------------------------------------------------------------------
			DateTime procDate=listProcs[0].ProcDate;
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=listProcs[0].ClinicNum;
			}
			long provNum=listProcs[0].ProvNum;
			for(int i=0;i<listProcs.Count;i++) {//starts at 0 to check procStatus
				if(listProcs[i].ProcDate!=procDate) {
					if(!isSilent) {
						MessageBox.Show("Procedures must have the same date to attach a group note."); 
					}
					return false;
				}
				if(PrefC.HasClinicsEnabled && listProcs[i].ClinicNum!=clinicNum) {
					if(!isSilent) {
						MessageBox.Show("Procedures must have the same clinic to attach a group note."); 
					}
					return false;
				}
				if(listProcs[i].ProvNum!=provNum) {
					if(!isSilent) {
						MessageBox.Show("Procedures must have the same provider to attach a group note."); 
					}
					return false;
				}
			}
			return true;
		}

		///<summary>Returns true if the 'Attach Lab Fee' menu item is applicable. Adds selected regular procedures to procNumsReg. Adds selected lab procedures to procNumsLab.</summary>
		private bool CanAttachLabFee(bool isSilent,List<long> procNumsReg,List<long> procNumsLab) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				return false;
			}
			if(gridProg.SelectedIndices.Length<2 || gridProg.SelectedIndices.Length>3) {
				if(!isSilent) {
					MessageBox.Show("Please select two or three procedures, one regular and the other one or two lab.");
				}
				return false;
			}
			//One check that is not made is whether a lab proc is already attached to a different proc.
			DataRow row1=(DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag;
			DataRow row2=(DataRow)gridProg.Rows[gridProg.SelectedIndices[1]].Tag;
			DataRow row3=null;
			if(gridProg.SelectedIndices.Length==3) {
				row3=(DataRow)gridProg.Rows[gridProg.SelectedIndices[2]].Tag;
			}
			if(row1["ProcNum"].ToString()=="0" || row2["ProcNum"].ToString()=="0" || (row3!=null && row3["ProcNum"].ToString()=="0")) {
				if(!isSilent) {
					MessageBox.Show("All selected items must be procedures.");
				}
				return false;
			}
			if(ProcedureCodes.GetProcCode(row1["ProcCode"].ToString()).IsCanadianLab) {
				procNumsLab.Add(PIn.Long(row1["ProcNum"].ToString()));
			}
			else {
				procNumsReg.Add(PIn.Long(row1["ProcNum"].ToString()));
			}
			if(ProcedureCodes.GetProcCode(row2["ProcCode"].ToString()).IsCanadianLab) {
				procNumsLab.Add(PIn.Long(row2["ProcNum"].ToString()));
			}
			else {
				procNumsReg.Add(PIn.Long(row2["ProcNum"].ToString()));
			}
			if(row3!=null) {
				if(ProcedureCodes.GetProcCode(row3["ProcCode"].ToString()).IsCanadianLab) {
					procNumsLab.Add(PIn.Long(row3["ProcNum"].ToString()));
				}
				else {
					procNumsReg.Add(PIn.Long(row3["ProcNum"].ToString()));
				}
			}
			if(procNumsReg.Count==0) {
				if(!isSilent) {
					MessageBox.Show("One of the selected procedures must be a regular non-lab procedure as defined in Procedure Codes.");
				}
				return false;
			}
			if(procNumsReg.Count>1) {
				if(!isSilent) {
					MessageBox.Show("Only one of the selected procedures may be a regular non-lab procedure as defined in Procedure Codes.");
				}
				return false;
			}
			return true;
		}

		///<summary>Checks if the procedure can be changed to newProcStatus.  If doCheckDb, it will get fresh data from the database when checking, otherwise, it will use data from LoadData.  If isSilent, a message box will popup if the status cannot be changed</summary>
		private bool CanChangeProcsStatus(ProcStat newProcStatus,DataRow row,bool doCheckDb,bool isSilent) {
			if(!IsAuditMode(isSilent)) {
				return false;
			}
			if(newProcStatus==ProcStat.C && !Preferences.GetBool(PreferenceName.AllowSettingProcsComplete)) {
				if(!isSilent) {
					MessageBox.Show("Only single appointments and tasks may be set complete.  If you want to be able to set procedures complete, you must turn "
						+"on that option in Setup | Chart | Chart Preferences.");
				}
				return false;
			}
			//check to make sure we don't have non-procedures
			if(row["ProcNum"].ToString()=="0" || row["ProcCode"].ToString()=="~GRP~") {
				if(!isSilent) {
					MessageBox.Show("Only procedures, single appointments, or single tasks may be set complete.");
				}
				return false;
			}
			List<ClaimProc> listClaimProcs=doCheckDb ? ClaimProcs.Refresh(_patCur.PatNum) : LoadData.ListClaimProcs;
			PaySplit[] arrayPaySplits=doCheckDb ? PaySplits.Refresh(_patCur.PatNum) : LoadData.ArrPaySplits;
			Adjustment[] arrayAdjustments=doCheckDb ? Adjustments.Refresh(_patCur.PatNum) : LoadData.ArrAdjustments;
			long procNum=PIn.Long(row["ProcNum"].ToString());
			Procedure procOld=doCheckDb ? Procedures.GetOneProc(procNum,true) : LoadData.ListProcs.FirstOrDefault(x => x.ProcNum==procNum)
				?? Procedures.GetOneProc(procNum,true);
			OrthoProcLink orthoProcLink=doCheckDb ? OrthoProcLinks.GetByProcNum(procNum) : LoadData.ListOrthoProcLinks
				.FirstOrDefault(x => x.ProcNum==procNum);
			if(procOld.IsLocked) {
				if(!isSilent) {
					MessageBox.Show("Locked procedures cannot be edited.");
				}
				return false;
			}
			#region Validation
			if(procOld.ProcStatus==newProcStatus) {
				if(!isSilent) {
					MessageBox.Show("Procedure's status already is "+newProcStatus);
				}
				return false;
			}
			if(ProcedureCodes.GetWhere(x => x.Id==procOld.CodeNum).Count==0) {
				if(!isSilent) {
					MessageBox.Show($"Missing codenum. Please run database maintenance method {nameof(DatabaseMaintenances.ProcedurelogCodeNumInvalid)}.");
				}
				return false;
			}
			Procedure procCur=procOld.Copy();
			procCur.ProcStatus=newProcStatus;
			DateTime procDate=PIn.Date(textDate.Text);//Mimics how procCur.ProcDate would be changed after validation below.
			Appointment appt=null;
			if(procCur.AptNum!=0) {//if attached to an appointment
				appt=doCheckDb ? Appointments.GetOneApt(procCur.AptNum) : LoadData.ArrAppts.FirstOrDefault(x => x.AptNum==procCur.AptNum);
				DateTime dateNow=doCheckDb ? MiscData.GetNowDateTime() : DateTime.Now;
				if(appt.AptDateTime.Date > dateNow.Date) {
					if(!isSilent) {
						MessageBox.Show("Not allowed because a procedure is attached to a future appointment with a date of "
							+appt.AptDateTime.ToShortDateString());
					}
					return false;
				}
				procDate=appt.AptDateTime;
			}
			if(procDate.Date.Year<1880) {//If not textDate entered or if appt.AptDateTime is invalid.
				procDate=doCheckDb ? MiscData.GetNowDateTime() : DateTime.Now;
			}
			if(procOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {
				if(doCheckDb && !ProcedureL.CheckPermissionsAndGlobalLockDate(procOld,procCur,procDate)) {
					return false;
				}
			}
			//At this point we do not need to check if a procedure status is changing. FormProcEdit.EntriesAreValid() does however.
			if(procOld.ProcStatus==ProcStat.C) {
				#region Changing from completed to something else.
				if(Adjustments.GetForProc(procCur.ProcNum,arrayAdjustments).Count!=0 && !isSilent
					&& !MsgBox.Show(MsgBoxButtons.YesNo,"This procedure has adjustments attached to it. Changing the status from completed will delete any "
						+"adjustments for the procedure. Continue?")) 
				{
					return false;
				}
				double sumPaySplits=PaySplits.GetForProc(procCur.ProcNum,arrayPaySplits).ToArray().ToList().Sum(x => ((PaySplit)x).SplitAmt);
				if(sumPaySplits!=0) {
					if(!isSilent) {
						MessageBox.Show("Not allowed to modify the status of a procedure that has payments attached to it. Detach payments from the procedure first.");
					}
					return false;
				}
				//Cannot set EC or EO on completed procs on a claim.
				if(ProcedureL.IsProcCompleteAttachedToClaim(procOld,listClaimProcs,isSilent)) {
					return false;
				}
				if(orthoProcLink!=null) {
					if(!isSilent) {
						MessageBox.Show("Cannot change the status of completed procedures that are linked to an ortho cases. Detach the procedure from the ortho case first.");
					}
					return false;
				}
				#endregion
			}
			if(procCur.ProcStatus==ProcStat.C) {//User is trying to change status to complete.
				#region Setting proc to complete
				if(ProcedureL.IsProcCompleteAttachedToClaim(procCur,listClaimProcs,isSilent)) {
					return false;
				}
				if(appt!=null) {
					if(doCheckDb && !Security.IsAuthorized(Permissions.ProcComplCreate,appt.AptDateTime,procCur.CodeNum,procCur.ProcFee)) {
						return false;
					}
				}
				else if(doCheckDb && !Security.IsAuthorized(Permissions.ProcComplCreate,procDate,procCur.CodeNum,procCur.ProcFee)) {
					return false;
				}
				if(procDate.Date > DateTime.Today.Date && !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed)) {
					MessageBox.Show("Completed procedures cannot be set for future dates.");
					return false;
				}
				#endregion
			}
			#endregion
			return true;
		}

		///<summary>Returns true if the row is an appointment and can be set complete.</summary>
		private bool CanCompleteAppointment(bool doCheckDb,bool isSilent) {
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			if(!CanDisplayAppointment() || listSelectedRows.Count!=1) {
				if(!isSilent) {
					MessageBox.Show("Only procedures, single appointments, or single tasks may be set complete.");
				}
				//Row selected is not an appoitment or the rows selected != 1
				return false;
			}
			//Only one appointment row selected at this point.
			DataRow rowApt=listSelectedRows.First();
			if(doCheckDb && !Security.IsAuthorized(Permissions.AppointmentEdit)) {
				return false;
			}
			long aptNum=PIn.Long(rowApt["AptNum"].ToString());
			Appointment apt=doCheckDb ? Appointments.GetOneApt(aptNum) : LoadData.ArrAppts.FirstOrDefault(x => x.AptNum==aptNum);
			if(apt==null) {
				if(!isSilent) {
					MessageBox.Show("Appointment does not exist.");
				}
				return false;
			}
			if(apt.AptStatus==ApptStatus.Complete) {
				if(!isSilent) {
					MessageBox.Show("Already complete.");
				}
				return false;
			}
			if(apt.AptStatus==ApptStatus.PtNote
				|| apt.AptStatus==ApptStatus.PtNoteCompleted
				|| apt.AptStatus==ApptStatus.Planned
				|| apt.AptStatus==ApptStatus.UnschedList) 
			{
				if(!isSilent) {
					MessageBox.Show("Not allowed for that status.");
				}
				return false;
			}
			if(doCheckDb) { 
				if(ProcedureCodes.DoAnyBypassLockDate()) {
					List<Procedure> listProcs=Procedures.GetProcsMultApts(new List<long> { apt.AptNum });
					foreach(Procedure proc in listProcs) {
						if(!Security.IsAuthorized(Permissions.ProcComplCreate,apt.AptDateTime,proc.CodeNum,proc.ProcFee)) {
							return false;
						}
					}
				}
				else if(!Security.IsAuthorized(Permissions.ProcComplCreate,apt.AptDateTime)) {
					return false;
				}
			}
			if(apt.AptDateTime.Date>DateTime.Today.Date) {
				if(!Preferences.GetBool(PreferenceName.ApptAllowFutureComplete)) {
					if(!isSilent) {
						MessageBox.Show("Not allowed to set future appointments complete."); 
					}
					return false;
				}
				if(!Preferences.GetBool(PreferenceName.FutureTransDatesAllowed)) {
					if(!isSilent) {
						MessageBox.Show("Not allowed to set procedures complete with future dates."); 
					}
					return false;
				}
			}
			bool hasProcsAttached=doCheckDb ? Appointments.HasProcsAttached(apt.AptNum) : LoadData.ListProcs.Any(x => x.AptNum==apt.AptNum);
			if(!apt.AptStatus.In(ApptStatus.PtNote,ApptStatus.PtNoteCompleted)  //PtNote blocked above, added here in case we ever enhance
				&& !Preferences.GetBool(PreferenceName.ApptAllowEmptyComplete)
				&& !hasProcsAttached)
			{
				if(!isSilent) {
					MessageBox.Show("Appointments without procedures attached can not be set complete."); 
				}
				return false;
			}
			#region Provider Term Date Check
			string message=Providers.CheckApptProvidersTermDates(apt);
			if(message!="") {
				if(!isSilent) {
					MessageBox.Show(this,message);//translated in Providers S class method 
				}
				return false;
			}
			#endregion Provider Term Date Check
			else if(!isSilent && !MsgBox.Show(MsgBoxButtons.OKCancel,"Set appointment complete?")) {
				return false;
			}
			return true;//Appointment row can be completed
		}

		///<summary>Returns true if the selected task row can be set complete.</summary>
		private bool CanCompleteTask(bool doCheckDb,bool isSilent) {
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			if(!CanDisplayTask() || listSelectedRows.Count!=1) {
				if(!isSilent) {
					MessageBox.Show("Only procedures, single appointments, or single tasks may be set complete.");
				}
				return false;
			}
			if(!isSilent && !MsgBox.Show(MsgBoxButtons.OKCancel,"The selected task will be marked Done and will affect all users.")) {
				return false;
			}
			if(listSelectedRows.Count!=gridProg.SelectedIndices.Length) {
				return false;
			}
			if(doCheckDb) {
				long taskNum=PIn.Long(listSelectedRows[0]["TaskNum"].ToString());
				Task taskCur=Tasks.GetOne(taskNum);
				if(taskCur==null) {
					if(!isSilent) {
						MessageBox.Show("The task has been deleted or moved.  Try again.");
					}
					return false;
				} 
			}
			return true;
		}

		///<summary>Returns true if the row can be deleted.</summary>
		private bool CanDeleteRow(DataRow row) {
			int skippedSecurity=0;
			int skippedRxSecurity=0;
			int skippedC=0;
			int skippedAttached=0;
			int skippedLinkedToOrthoCase=0;
			return CanDeleteRow(row,doCheckDb:false,ref skippedSecurity,ref skippedRxSecurity,ref skippedC,ref skippedAttached,
				ref skippedLinkedToOrthoCase,null);
		}

		///<summary>Returns true if the row can be deleted. This overload increments the arguments keeping track of skip reasons.</summary>
		private bool CanDeleteRow(DataRow row,bool doCheckDb,ref int skippedSecurity,ref int skippedRxSecurity,ref int skippedC,ref int skippedAttached,
			ref int skippedLinkedToOrthoCase,OrthoProcLink orthoProcLink) 
		{
			long procNum=PIn.Long(row["ProcNum"].ToString(),false);
			if(procNum!=0) {
				if(PIn.Enum<ProcStat>(PIn.Int(row["ProcStatus"].ToString())).In(ProcStat.C,ProcStat.EC,ProcStat.EO)
					|| PIn.Bool(row["IsLocked"].ToString()))//takes care of locked group notes and invalidated (deleted and locked) procs
				{
					skippedC++;
					return false;
				}
				if(orthoProcLink!=null) {
					skippedLinkedToOrthoCase++;
					return false;
				}
				DateTime procDate=PIn.Date(row["ProcDate"].ToString());
				long codeNum=PIn.Long(row["CodeNum"].ToString());
				if(ProcedureCodes.GetStringProcCode(codeNum)==ProcedureCodes.GroupProcCode) {//If a group note
					//Check DB to see if attached to any completed procedures. This isn't pulled from datasetmain because we want to be 100% up to date.
					//Note that if multiple rows were selected it might have already deleted some procedures, but we do not delete completed
					//procedures in this loop.
					if(doCheckDb) {
						if(ProcGroupItems.GetCountCompletedProcsForGroup(procNum)==0) { //If not attached to completed procs
							if(!Security.IsAuthorized(Permissions.ProcDelete,procDate)) {
								skippedSecurity++;
								return false;
							}
						}
						else {
							skippedC++;
							return false;
						}
					}
				}
				else {//Not a group note
					if(doCheckDb && !Security.IsAuthorized(Permissions.ProcDelete,procDate)) {
						skippedSecurity++;
						return false;
					}
				}
				if(PIn.Enum<ProcStat>(PIn.Int(row["ProcStatus"].ToString()))==ProcStat.TP) {//check if there is an allocated payment to the TP proc. 
					double totForProc=doCheckDb ? PIn.Double(PaySplits.GetTotForProc(procNum)) 
						: LoadData.ArrPaySplits.Where(x => x.ProcNum==procNum).Sum(x => x.SplitAmt);
					if(!totForProc.IsEqual(0)) {
						skippedAttached++;
						return false;
					}
				}
				return true;
			}
			else if(row["RxNum"].ToString()!="0") {
				if(doCheckDb && !Security.IsAuthorized(Permissions.RxEdit)) {
					skippedRxSecurity++;
					return false;
				}
				return true;
			}
			//Not a proc or a prescription
			return false;
		}

		///<summary>Returns true if the 'Detach Lab Fee' menu item is applicable.</summary>
		private bool CanDetachLabFee(DataRow row,bool isSilent) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				return false;
			}
			if(row["ProcNum"].ToString()=="0") {
				if(!isSilent) {
					MessageBox.Show("Please select a lab procedure first.");
				}
				return false;
			}
			if(row["ProcNumLab"].ToString()=="0") {
				if(!isSilent) {
					MessageBox.Show("The selected procedure is not attached as a lab procedure.");
				}
				return false;
			}
			return true;
		}

		///<summary>Returns true if at least one appointment row is selected.</summary>
		private bool CanDisplayAppointment() {
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			if(listSelectedRows.Any(x => x["AptNum"].ToString()!="0")) {
				return true;
			}
			return false;
		}

		///<summary>Returns true if the 'Print Routing Slip' menu item should be displayed.</summary>
		private bool CanDisplayRoutingSlip() {
			if(checkAudit.Checked) {
				return false;
			}
			return gridProg.SelectedIndices.Any(x => ((DataRow)gridProg.Rows[x].Tag)["AptNum"].ToString()!="0");
		}

		///<summary>Returns true if at least one task is selected.</summary>
		private bool CanDisplayTask() {
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			if(listSelectedRows.Any(x => x["TaskNum"].ToString()=="0")) {
				//Row selected is not a task.
				return false;
			}
			return true;
		}

		///<summary>Returns true if the 'Edit All' menu item is applicable to this row. For the procedures that can be edited, adds them to listProcsToEdit.</summary>
		private bool CanEditRow(DataRow row,bool doCheckDb,bool isSilent,List<Procedure> listProcsToEdit) {
			if(checkAudit.Checked) {
				if(!isSilent) {
					MessageBox.Show("Not allowed to edit procedures when in audit mode.");
				}
				return false;
			}
			long procNum=PIn.Long(row["ProcNum"].ToString());
			if(procNum==0) {
				if(!isSilent) {
					MessageBox.Show("Only procedures may be edited.");
				}
				return false;
			}
			Procedure proc=doCheckDb ? Procedures.GetOneProc(procNum,true) : LoadData.ListProcs.FirstOrDefault(x => x.ProcNum==procNum);
			if(proc==null) {
				if(!isSilent) {
					MessageBox.Show("Procedure does not exist.");
				}
				return false;
			}
			if(proc.IsLocked) {
				if(!isSilent) {
					MessageBox.Show("Locked procedures cannot be edited.");
				}
				return false;
			}
			listProcsToEdit.Add(proc);
			return true;
		}

		///<summary>Returns true if the 'Group for Multi Visit' menu item is applicable.</summary>
		private bool CanGroupMultiVisit(DataRow row,bool doCheckDb,bool isSilent) {
			long procNum=PIn.Long(row["ProcNum"].ToString());
			if(procNum==0) {
				if(!isSilent) {
					MessageBox.Show("Some of the selected items are not procedures.\r\n"
					+"Select only procedures and try again.");
				}
				return false;
			}
			if(gridProg.SelectedTags<DataRow>().Count(x => x["ProcNum"].ToString()!="0")<2) {
				if(!isSilent) {
					MessageBox.Show("At least two procedures must be selected to create a multiple visit group.");
				}
				return false;
			}
			List<ProcMultiVisit> listProcMVs=doCheckDb ? ProcMultiVisits.GetGroupsForProcsFromDb(procNum)
				: LoadData.ListProcMultiVisits.FindAll(x => x.ProcNum==procNum);
			if(listProcMVs.Count>0) {
				if(!isSilent) {
					MessageBox.Show("Some of the selected items belong to existing multiple visit groups.\r\n"
					+"Select only procedures which are not part of a multiple visit group and try again.\r\n"
					+"Consider ungrouping an existing group before adding new procedures to the group if needed.");
				}
				return false;
			}
			return true;
		}		
		
		///<summary>Returns true if the row can be put into a group note. Adds procedure to listProcsToGroup if it can be.</summary>
		private bool CanGroupRow(DataRow row,bool doCheckDb,bool isSilent,List<Procedure> listProcsToGroup) {
			long procNum=PIn.Long(row["ProcNum"].ToString());
			if(procNum==0) { //This is not a procedure.
				if(!isSilent) {
					MessageBox.Show("You may only attach a group note to procedures.");
				}
				return false;
			}
			Procedure proc=doCheckDb ? Procedures.GetOneProc(procNum,true) : LoadData.ListProcs.FirstOrDefault(x => x.ProcNum==procNum);
			if(proc==null) {
				return false;
			}
			if(ProcedureCodes.GetStringProcCode(proc.CodeNum,doThrowIfMissing: false)==ProcedureCodes.GroupProcCode) {
				if(!isSilent) {
					MessageBox.Show("You cannot attach a group note to another group note.");
				}
				return false;
			}
			if(proc.IsLocked) {
				if(!isSilent) {
					MessageBox.Show("Locked procedures cannot be attached to a group note.");
				}
				return false;
			}
			if(proc.ProcStatus!=ProcStat.C) {
				if(!isSilent) {
					MessageBox.Show("Procedures must be complete to attach a group note.");
				}
				return false;
			}
			if(proc.ProcStatus==ProcStat.C && proc.ProcDate.Date>DateTime.Today.Date && !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed)) {
				if(!isSilent) {
					MessageBox.Show("Completed procedures cannot be set complete for days in the future.");
				}
				return false;
			}
			listProcsToGroup.Add(proc);
			return true;
		}

		///<summary>Returns true if the 'Print Day for Hospital' menu item is applicable. This method returns true if there is at least one completed procedure with the same date as the row.</summary>
		private bool CanPrintDay(DataRow row,int rowIdx) {
			bool isCompletedProc(DataRow rowToCheck) {
				return PIn.Long(rowToCheck["ProcNum"].ToString())!=0 && PIn.Enum<ProcStat>(rowToCheck["ProcStatus"].ToString())==ProcStat.C;
			}
			if(isCompletedProc(row)) {
				return true;
			}
			DateTime dateRow=PIn.Date(row["ProcDate"].ToString()).Date;
			//Look at all the rows of the same date before this row.
			for(int i=rowIdx-1;i>=0;i--) {
				DataRow otherRow=(DataRow)gridProg.Rows[i].Tag;
				DateTime dateOtherRow=PIn.Date(otherRow["ProcDate"].ToString());
				if(dateRow!=dateOtherRow) {
					break;
				}
				if(isCompletedProc(otherRow)) {
					return true;
				}
			}
			//Look at all the rows of the same date after this row.
			for(int i=rowIdx+1;i<gridProg.Rows.Count;i++) {
				DataRow otherRow=(DataRow)gridProg.Rows[i].Tag;
				DateTime dateOtherRow=PIn.Date(otherRow["ProcDate"].ToString()).Date;
				if(dateRow!=dateOtherRow) {
					break;
				}
				if(isCompletedProc(otherRow)) {
					return true;
				}
			}
			return false;//No completed procs found for this date.
		}

		///<summary>Returns true if the 'Print Routing Slip' menu item should be enabled.</summary>
		private bool CanPrintRoutingSlip(bool isSilent) {
			if(gridProg.SelectedIndices.Length==0) {
				if(!isSilent) {
					MessageBox.Show("Please select an appointment first.");
				}
				return false;
			}
			if(checkAudit.Checked) {
				if(!isSilent) {
					MessageBox.Show("Not allowed in audit mode.");
				}
				return false;
			}
			if(gridProg.SelectedIndices.Length!=1
				|| ((DataRow)gridProg.Rows[gridProg.SelectedIndices[0]].Tag)["AptNum"].ToString()=="0") 
			{
				if(!isSilent) {
					MessageBox.Show("Routing slips can only be printed for single appointments.");
				}
				return false;
			}
			return true;
		}

		///<summary>Returns true if the 'Ungroup for Multi Visit' menu item is applicable.</summary>
		private bool CanUngroupMultiVisit(DataRow row,bool doCheckDb,bool isSilent) {
			long procNum=PIn.Long(row["ProcNum"].ToString());
			if(procNum==0) {
				if(!isSilent) {
					MessageBox.Show("Some of the selected items are not procedures.\r\n"
					+"Select only procedures and try again.");
				}
				return false;
			}
			if(!doCheckDb) {//We'll check the db later before ungrouping.
				List<ProcMultiVisit> listProcMVs=LoadData.ListProcMultiVisits.FindAll(x => x.ProcNum==procNum);
				if(listProcMVs.Count==0) {
					if(!isSilent) {
						MessageBox.Show("Selected procedures are not part of multi visit group.");
					}
					return false;
				}
			}
			return true;
		}

		///<summary>Displays all XVWeb images to the chart module for the patient selected and the thumbnails we have in our list.</summary>
		private void DisplayXVWebImages(long patNum) {
			if(InvokeRequired) {
				Invoke((Action)(() => DisplayXVWebImages(patNum)));
				return;
			}
			if(_listApteryxThumbnails==null || _patCur==null
				|| patNum!=_patCur.PatNum) //In case the patient was changed while we were downloading images
			{
				return;
			}
			long imageCatNum=PIn.Long(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.XVWeb),XVWeb.ProgramProps.ImageCategory));
			if(tabControlImages.SelectedIndex>0 //any category except 'all'
				&& imageCatNum!=Definitions.GetByCategory(DefinitionCategory.ImageCats)[(int)_arrayListVisImageCats[tabControlImages.SelectedIndex-1]].Id) 
			{
				return;//if the currently selected tab is not for XVWeb
			}
			for(int i=listViewImages.Items.Count-1;i>=0;i--) {
				ApteryxImage imageTag=listViewImages.Items[i].Tag as ApteryxImage;
				if(imageTag!=null) {
					imageListThumbnails.Images.RemoveAt(i);
					listViewImages.Items.RemoveAt(i);
				}
			}
			lock(_apteryxLocker) {
				for(int i=0;i<_listApteryxThumbnails.Count;i++) {
					ApteryxImage imgCur=_listApteryxThumbnails[i].Image;
					imageListThumbnails.Images.Add(_listApteryxThumbnails[i].Thumbnail);
					ListViewItem item=new ListViewItem(imgCur.AcquisitionDate.ToShortDateString()+": "+imgCur.FormattedTeeth,imageListThumbnails.Images.Count-1);
					item.Tag=imgCur;
					listViewImages.Items.Add(item);
				}
			}
		}

		///<summary>Returns true if the row passed in should be displayed.  Otherwise; false.</summary>
		private bool DoesGridProgRowPassFilter(DataRow rowCur,List<ProcGroupItem> procGroupItems,List<long> procNumList) {
			long procNumCur=PIn.Long(rowCur["ProcNum"].ToString());//increase code efficiency
			long patNumCur=PIn.Long(rowCur["PatNum"].ToString());//increase code efficiency
			if(procNumCur!=0) {//if this is a procedure 
				//if it's a group note and we are viewing by tooth number
				if(rowCur["ProcCode"].ToString()==ProcedureCodes.GroupProcCode && checkShowTeeth.Checked) {
					//consult the list of previously obtained procedures and ProcGroupItems to see if this procgroup should be visible.
					bool showGroupNote=false;
					for(int j=0;j<procGroupItems.Count;j++) {//loop through all procGroupItems for the patient. 
						if(procGroupItems[j].GroupNum==procNumCur) {//if this item is associated with this group note
							for(int k=0;k<procNumList.Count;k++) {//check all of the visible procs
								if(procNumList[k]==procGroupItems[j].ProcNum) {//if this group note is associated with a visible proc
									showGroupNote=true;
								}
							}
						}
					}
					if(!showGroupNote) {
						return false;//don't show it in the grid
					}
				}
				else {//procedure or group note, not viewing by tooth number
					if(ShouldDisplayProc(rowCur)) {
						_listRowsProcForGraphical.Add(rowCur);//show it in the graphical tooth chart
						//show it in the grid below
					}
					else {
						return false;//don't show it in the grid
					}
				}
			}
			else if(rowCur["CommlogNum"].ToString()!="0") {//if this is a commlog
				if(!checkComm.Checked) {
					return false;
				}
				if(_patCur!=null&&patNumCur!=_patCur.PatNum) {//if this is a different family member
					if(!checkCommFamily.Checked) {
						return false;
					}
				}
			}
			//ODHQ only - rows should only exist (have WebChatNums) if at HQ
			else if(rowCur["WebChatSessionNum"].ToString()!="0") {//if this is a web chat
				if(!checkComm.Checked) {
					return false;
				}
				if(patNumCur!=_patCur.PatNum) {//if this is a different family member
					return false;
				}
			}
			else if(rowCur["RxNum"].ToString()!="0") {//if this is an Rx
				if(!checkRx.Checked) {
					return false;
				}
			}
			else if(rowCur["LabCaseNum"].ToString()!="0") {//if this is a LabCase
				if(!checkLabCase.Checked) {
					return false;
				}
			}
			else if(rowCur["TaskNum"].ToString()!="0") {//if this is a TaskItem
				if(!checkTasks.Checked) {
					return false;
				}
				if(_patCur!=null&&patNumCur!=_patCur.PatNum) {//if this is a different family member
					if(!checkCommFamily.Checked) { //uses same check box as commlog
						return false;
					}
				}
			}
			else if(rowCur["EmailMessageNum"].ToString()!="0") {//if this is an Email
				if(!checkEmail.Checked || ((HideInFlags)PIn.Int(rowCur["EmailMessageHideIn"].ToString())).HasFlag(HideInFlags.ChartProgNotes)) {
					return false;
				}
				EmailType type=(EmailType)PIn.Int(rowCur["EmailMessageHtmlType"].ToString());
				if(type==EmailType.Html) {
					rowCur["note"]=MarkupEdit.ConvertToPlainText(rowCur["note"].ToString());
				}
				else if(type==EmailType.RawHtml) {
					rowCur["note"]="Raw HTML Email";//user needs to double click to see contents. Currently no way to strip out all code.
				}
			}
			else if(rowCur["AptNum"].ToString()!="0") {//if this is an Appointment
				if(!checkAppt.Checked) {
					return false;
				}
			}
			else if(rowCur["SheetNum"].ToString()!="0") {//if this is a sheet
				if(!checkSheets.Checked) {
					return false;
				}
			}
			if(_dateTimeShowDateStart.Year>1880 && PIn.Date(rowCur["ProcDate"].ToString()).Date<_dateTimeShowDateStart.Date) {
				return false;
			}
			if(_dateTimeShowDateEnd.Year>1880 && PIn.Date(rowCur["ProcDate"].ToString()).Date>_dateTimeShowDateEnd.Date) {
				return false;
			}
			return true;
		}		
		
		private void DrawProcGraphics(DateTime dateLimit) {
			//this requires: ProcStatus, ProcCode, ToothNum, HideGraphics, Surf, and ToothRange.  All need to be raw database values.
			string[] arrayTeeth;
			Color cLight=Color.White;
			Color cDark=Color.White;
			List<Definition> listDefs=Definitions.GetByCategory(DefinitionCategory.ChartGraphicColors);
			for(int i=0;i<_listRowsProcForGraphical.Count;i++) {
				if(_listRowsProcForGraphical[i]["HideGraphics"].ToString()=="1") {
					continue;
				}
				ProcedureCode procCode=ProcedureCodes.GetProcCode(_listRowsProcForGraphical[i]["ProcCode"].ToString());
				if(procCode.PaintType==ToothPaintingType.None || procCode.TreatmentArea==ProcedureTreatmentArea.Mouth) {
					continue;
				}
				if(procCode.PaintType==ToothPaintingType.Extraction && (
					PIn.Long(_listRowsProcForGraphical[i]["ProcStatus"].ToString())==(int)ProcStat.C
					|| PIn.Long(_listRowsProcForGraphical[i]["ProcStatus"].ToString())==(int)ProcStat.EC
					|| PIn.Long(_listRowsProcForGraphical[i]["ProcStatus"].ToString())==(int)ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(procCode.GraphicColor.ToArgb()==Color.FromArgb(0).ToArgb()) {
					switch((ProcStat)PIn.Long(_listRowsProcForGraphical[i]["ProcStatus"].ToString())) {
						case ProcStat.C:
							cDark=listDefs[1].Color;
							cLight=listDefs[6].Color;
							break;
						case ProcStat.TP:
						case ProcStat.TPi:// TPi color should be the same as TP color.
							cDark=listDefs[0].Color;
							cLight=listDefs[5].Color;
							break;
						case ProcStat.EC:
							cDark=listDefs[2].Color;
							cLight=listDefs[7].Color;
							break;
						case ProcStat.EO:
							cDark=listDefs[3].Color;
							cLight=listDefs[8].Color;
							break;
						case ProcStat.R:
							cDark=listDefs[4].Color;
							cLight=listDefs[9].Color;
							break;
						case ProcStat.Cn:
							cDark=listDefs[16].Color;
							cLight=listDefs[17].Color;
							break;
						case ProcStat.D:  //Can happen with invalidated locked procs.
						default:
							continue;  //Don't draw.
					}
				}
				else {
					cDark=procCode.GraphicColor;
					cLight=procCode.GraphicColor;
				}

				switch(procCode.PaintType) {
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(_listToothInitialsCopy,_listRowsProcForGraphical[i]["ToothNum"].ToString())){
							_toothChartRelay.SetPontic(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						}
						else{
							_toothChartRelay.SetCrown(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(_listToothInitialsCopy,_listRowsProcForGraphical[i]["ToothNum"].ToString())) {
							_toothChartRelay.SetPontic(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cLight);
						}
						else {
							_toothChartRelay.SetCrown(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						_toothChartRelay.SetCrown(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.CrownLight:
						_toothChartRelay.SetCrown(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(_listRowsProcForGraphical[i]["Surf"].ToString()=="U") {
							arrayTeeth=new string[14];
							for(int t=0;t<14;t++) {
								arrayTeeth[t]=(t+2).ToString();
							}
						}
						else if(_listRowsProcForGraphical[i]["Surf"].ToString()=="L") {
							arrayTeeth=new string[14];
							for(int t=0;t<14;t++) {
								arrayTeeth[t]=(t+18).ToString();
							}
						}
						else {
							arrayTeeth=_listRowsProcForGraphical[i]["ToothRange"].ToString().Split(new char[] {','});
						}
						for(int t=0;t<arrayTeeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(_listToothInitialsCopy,arrayTeeth[t])) {
								_toothChartRelay.SetPontic(arrayTeeth[t],cDark);
							}
							else {
								_toothChartRelay.SetCrown(arrayTeeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(_listRowsProcForGraphical[i]["Surf"].ToString()=="U") {
							arrayTeeth=new string[14];
							for(int t=0;t<14;t++) {
								arrayTeeth[t]=(t+2).ToString();
							}
						}
						else if(_listRowsProcForGraphical[i]["Surf"].ToString()=="L") {
							arrayTeeth=new string[14];
							for(int t=0;t<14;t++) {
								arrayTeeth[t]=(t+18).ToString();
							}
						}
						else {
							arrayTeeth=_listRowsProcForGraphical[i]["ToothRange"].ToString().Split(new char[] { ',' });
						}
						for(int t=0;t<arrayTeeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(_listToothInitialsCopy,arrayTeeth[t])) {
								_toothChartRelay.SetPontic(arrayTeeth[t],cLight);
							}
							else {
								_toothChartRelay.SetCrown(arrayTeeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						_toothChartRelay.SetBigX(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.FillingDark:
						_toothChartRelay.SetSurfaceColors(_listRowsProcForGraphical[i]["ToothNum"].ToString(),_listRowsProcForGraphical[i]["Surf"].ToString(),cDark);
						break;
				  case ToothPaintingType.FillingLight:
						_toothChartRelay.SetSurfaceColors(_listRowsProcForGraphical[i]["ToothNum"].ToString(),_listRowsProcForGraphical[i]["Surf"].ToString(),cLight);
						break;
					case ToothPaintingType.Implant:
						_toothChartRelay.SetImplant(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.PostBU:
						_toothChartRelay.SetBU(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.RCT:
						_toothChartRelay.SetRCT(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Sealant:
						_toothChartRelay.SetSealant(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Veneer:
						_toothChartRelay.SetVeneer(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.Watch:
						_toothChartRelay.SetWatch(_listRowsProcForGraphical[i]["ToothNum"].ToString(),cDark);
						break;
				}
			}
		}

		private void EasyHideClinicalData() {
			if(Preferences.GetBool(PreferenceName.EasyHideClinical)) {
				gridPtInfo.Visible=false;
				checkShowE.Visible=false;
				checkShowR.Visible=false;
				checkRx.Visible=false;
				checkComm.Visible=false;
				checkNotes.Visible=false;
				butShowNone.Visible=false;
				butShowAll.Visible=false;
				//panelEnterTx.Visible=false;//next line changes it, though
				radioEntryEC.Visible=false;
				radioEntryEO.Visible=false;
				radioEntryR.Visible=false;
				labelDx.Visible=false;
				listDx.Visible=false;
				labelPrognosis.Visible=false;
				comboPrognosis.Visible=false;
			}
			else {
				gridPtInfo.Visible=true;
				checkShowE.Visible=true;
				checkShowR.Visible=true;
				checkRx.Visible=true;
				checkComm.Visible=true;
				checkNotes.Visible=true;
				butShowNone.Visible=true;
				butShowAll.Visible=true;
				radioEntryEC.Visible=true;
				radioEntryEO.Visible=true;
				radioEntryR.Visible=true;
				labelDx.Visible=true;
				listDx.Visible=true;
				labelPrognosis.Visible=true;
				comboPrognosis.Visible=true;
			}
		}

		private void FillChartViewsGrid(bool doRefreshViews=true) {
			if(_patCur==null) {
				butChartViewAdd.Enabled=false;
				butChartViewDown.Enabled=false;
				butChartViewUp.Enabled=false;
				gridChartViews.Enabled=false;
				return;
			}
			else {
				butChartViewAdd.Enabled=true;
				butChartViewDown.Enabled=true;
				butChartViewUp.Enabled=true;
				gridChartViews.Enabled=true;
			}
			if(doRefreshViews) {
				ChartViews.RefreshCache();//Ideally this would use signals to refresh
			}
			gridChartViews.BeginUpdate();
			gridChartViews.Columns.Clear();
			GridColumn col=new GridColumn("F#",25);
			gridChartViews.Columns.Add(col);
			col=new GridColumn("View",50){ IsWidthDynamic=true };
			gridChartViews.Columns.Add(col);
			gridChartViews.Rows.Clear();
			GridRow row;
			_listChartViews=ChartViews.GetDeepCopy();
			for(int i=0;i<_listChartViews.Count;i++) {
				row=new GridRow();
				//assign hot keys F1-F12
				if(i<11) {
					row.Cells.Add("F"+(i+1));
				}
				row.Cells.Add(_listChartViews[i].Description);
				gridChartViews.Rows.Add(row);
			}
			gridChartViews.EndUpdate();
		}

		///<summary>This method is used to set the Date Range filter start and stop dates based on either a custom date range or DatesShowing property of chart view.</summary>
		private void FillDateRange() {
			textShowDateRange.Text="";
			if(_dateTimeShowDateStart.Year>1880) {
				textShowDateRange.Text+=_dateTimeShowDateStart.ToShortDateString();
			}
			if(_dateTimeShowDateEnd.Year>1880 && _dateTimeShowDateStart!=_dateTimeShowDateEnd) {
				if(textShowDateRange.Text!="") {
					textShowDateRange.Text+="-";
				}
				textShowDateRange.Text+=_dateTimeShowDateEnd.ToShortDateString();
			}
			if(textShowDateRange.Text=="") {
				textShowDateRange.Text="All Dates";
			}
		}

		///<summary>Gets run with each ModuleSelected.  Fills Dx, Prognosis, Priorities, ProcButtons, Date, and Image categories</summary>
		private void FillDxProcImage(bool doRefreshData=true) {
			//if(textDate.errorProvider1.GetError(textDate)==""){
			if(checkToday.Checked) {//textDate.Text=="" || 
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			//}
			List<Definition> listChartGraphicColorDefs=Definitions.GetByCategory(DefinitionCategory.ChartGraphicColors, true);
			List<Definition> listProcButtonCatDefs=Definitions.GetDefsForCategory(DefinitionCategory.ProcButtonCats,true);
			List<Definition> listDiagnosisDefs=Definitions.GetDefsForCategory(DefinitionCategory.Diagnosis,true);
			List<Definition> listPrognosisDefs=Definitions.GetDefsForCategory(DefinitionCategory.Prognosis,true);
			List<Definition> listTxPrioritiesDefs=Definitions.GetDefsForCategory(DefinitionCategory.TxPriorities,true);
			listDx.Items.Clear();
			foreach(Definition diagnosisDef in listDiagnosisDefs) {
				listDx.Items.Add(new ODBoxItem<Definition>(diagnosisDef.Name,diagnosisDef));
			}
			int selectedPrognosis=comboPrognosis.SelectedIndex;//retain prognosis selection
			comboPrognosis.Items.Clear();
			comboPrognosis.Items.AddDefNone("no prognosis");
			comboPrognosis.Items.AddDefs(listPrognosisDefs);
			int selectedPriority=comboPriority.SelectedIndex;//retain current selection
			comboPriority.Items.Clear();
			comboPriority.Items.AddDefNone("no priority");//0
			comboPriority.Items.AddDefs(listTxPrioritiesDefs);
			if(selectedPrognosis>0 && selectedPrognosis<comboPrognosis.Items.Count) {
				comboPrognosis.SelectedIndex=selectedPrognosis;
			}
			else {
				comboPrognosis.SelectedIndex=0;
			}
			if(selectedPriority>0 && selectedPriority<comboPriority.Items.Count) {
				//set the selected to what it was before. Don't let the combo remember the old one, in case defs were just edited.
				comboPriority.SelectedIndex=selectedPriority;
			}
			else {
				comboPriority.SelectedIndex=0;
				//or just set to no priority
			}
			int selectedButtonCat=listButtonCats.SelectedIndex;
			listButtonCats.Items.Clear();
			listButtonCats.Items.Add("Quick Buttons");
			foreach(Definition procButtonCatDef in listProcButtonCatDefs) {
				listButtonCats.Items.Add(new ODBoxItem<Definition>(procButtonCatDef.Name,procButtonCatDef));
			}
			if(selectedButtonCat<listButtonCats.Items.Count) {
				listButtonCats.SelectedIndex=selectedButtonCat;
			}
			if(listButtonCats.SelectedIndex==-1	&& listButtonCats.Items.Count>0) {
				listButtonCats.SelectedIndex=0;
			}
			FillProcButtons(doRefreshData);
			int selectedImageTab=tabControlImages.SelectedIndex;//retains current selection
			tabControlImages.TabPages.Clear();
			TabPage tabPage;
			tabPage=new TabPage();
			tabPage.Text="All";
			tabControlImages.TabPages.Add(tabPage);
			_arrayListVisImageCats=new ArrayList();
			List<Definition> listImageCatDefs=Definitions.GetDefsForCategory(DefinitionCategory.ImageCats,true);
			for(int i=0;i<listImageCatDefs.Count;i++) {
				if(listImageCatDefs[i].Value.Contains("X")) {//if tagged to show in Chart
					_arrayListVisImageCats.Add(i);
					tabPage=new TabPage();
					tabPage.Text=listImageCatDefs[i].Name;
					tabControlImages.TabPages.Add(tabPage);
				}
			}
			if(selectedImageTab<tabControlImages.TabCount) {
				tabControlImages.SelectedIndex=selectedImageTab;
			}
			panelTPdark.BackColor=listChartGraphicColorDefs[0].Color;
			panelCdark.BackColor=listChartGraphicColorDefs[1].Color;
			panelECdark.BackColor=listChartGraphicColorDefs[2].Color;
			panelEOdark.BackColor=listChartGraphicColorDefs[3].Color;
			panelRdark.BackColor=listChartGraphicColorDefs[4].Color;
			panelTPlight.BackColor=listChartGraphicColorDefs[5].Color;
			panelClight.BackColor=listChartGraphicColorDefs[6].Color;
			panelEClight.BackColor=listChartGraphicColorDefs[7].Color;
			panelEOlight.BackColor=listChartGraphicColorDefs[8].Color;
			panelRlight.BackColor=listChartGraphicColorDefs[9].Color;
    }

		///<summary>Gets run on ModuleSelected and each time a different images tab is selected. It first creates any missing thumbnails, then displays them. So it will be faster after the first time.</summary>
		private void FillImages() {
			_arrayListVisImages=new ArrayList();
			listViewImages.Items.Clear();
			imageListThumbnails.Images.Clear();
			if(_patCur==null) {
				return;
			}
			if(string.IsNullOrEmpty(_patFolder)) {
				return;
			}
			if(!panelImages.Visible) {
				return;
			}
			List<Definition> listImageCatDefs=Definitions.GetByCategory(DefinitionCategory.ImageCats);
			for(int i=0;i<_arrayDocuments.Length;i++) {
				if(!_arrayListVisImageCats.Contains(listImageCatDefs.FindIndex(x => x.Id==_arrayDocuments[i].Category))) {
					continue;//if category not visible, continue
				}
				if(tabControlImages.SelectedIndex>0) {//any category except 'all'
					if(_arrayDocuments[i].Category!=listImageCatDefs[(int)_arrayListVisImageCats[tabControlImages.SelectedIndex-1]].Id)
					{
						continue;//if not in category, continue
					}
				}
				//Documents.Cur=DocumentList[i];
				imageListThumbnails.Images.Add(Documents.GetThumbnail(_arrayDocuments[i],_patFolder,
					imageListThumbnails.ImageSize.Width));
				_arrayListVisImages.Add(i);
				ListViewItem item=new ListViewItem(_arrayDocuments[i].AddedOnDate.ToShortDateString()+": "
					+_arrayDocuments[i].Description,imageListThumbnails.Images.Count-1);
				//item.ToolTipText=patFolder+DocumentList[i].FileName;//not supported by Mono
				listViewImages.Items.Add(item);
			}//for
			DisplayXVWebImages(_patCur.PatNum);	
		}

		private void FillListPriorities() {
			listPriorities.Items.Clear();
			listPriorities.Items.Add("No Priority");
			List<Definition> listDefs=Definitions.GetByCategory(DefinitionCategory.TxPriorities);
			foreach(Definition txPriorityDef in listDefs) {
				listPriorities.Items.Add(new ODBoxItem<Definition>(txPriorityDef.Name,txPriorityDef));
			}
		}

		private void fillPanelQuickButtons(bool doRefreshData=true) {
			panelQuickButtons.BeginUpdate();
			panelQuickButtons.ListODPanelItems.Clear();
			if(doRefreshData || _listProcButtonQuicks==null) {
				_listProcButtonQuicks=ProcButtonQuicks.GetAll();
			}
			_listProcButtonQuicks.Sort(ProcButtonQuicks.sortYX);
			ODPanelItem panelItem;
			for(int i=0;i<_listProcButtonQuicks.Count;i++) {
				panelItem=new ODPanelItem();
				panelItem.Text=_listProcButtonQuicks[i].Description;
				panelItem.YPos=_listProcButtonQuicks[i].YPos;
				panelItem.ItemOrder=i;
				panelItem.ItemType=(_listProcButtonQuicks[i].IsLabel?ODPanelItemType.Label:ODPanelItemType.Button);
				panelItem.Tags.Add(_listProcButtonQuicks[i]);
				panelQuickButtons.ListODPanelItems.Add(panelItem);
			}
			panelQuickButtons.EndUpdate();
		}

		private void FillProcButtons(bool doRefreshData=true) {
			listViewButtons.Items.Clear();
			imageListProcButtons.Images.Clear();
			panelQuickButtons.Visible=false;
			if(listButtonCats.SelectedIndex==-1) {
				_arrayProcButtons=new ProcButton[0];
				return;
			}
			if(listButtonCats.SelectedIndex==0) {
				panelQuickButtons.Visible=true;
				panelQuickButtons.Location=listViewButtons.Location;
				panelQuickButtons.Size=listViewButtons.Size;
				fillPanelQuickButtons(doRefreshData);
				panelQuickButtons.Visible=true;
				panelQuickButtons.Location=listViewButtons.Location;
				panelQuickButtons.Size=listViewButtons.Size;
				_arrayProcButtons=new ProcButton[0];
				return;
			}
			if(doRefreshData) {
				ProcButtons.RefreshCache();
			}
			Definition selectedButtonCatDef=listButtonCats.GetSelected<Definition>();//Will not be null if 'Quick Buttons' is selected due to above if statement
			List<long> listProcButtonDefNums=Definitions.GetByCategory(DefinitionCategory.ProcButtonCats).Select(x => x.Id).ToList();
			if(!listProcButtonDefNums.Contains(selectedButtonCatDef.Id)) {
				MessageBox.Show("The Procedue Button Category has been hidden.");
				ModuleSelected(_patCur.PatNum);
				return;
			}
			_arrayProcButtons=ProcButtons.GetForCat(selectedButtonCatDef.Id);
			ListViewItem item;
			for(int i=0;i<_arrayProcButtons.Length;i++) {
				if(_arrayProcButtons[i].ButtonImage!="") {
					//image keys are simply the ProcButtonNum
					imageListProcButtons.Images.Add(_arrayProcButtons[i].ProcButtonNum.ToString(),PIn.Bitmap(_arrayProcButtons[i].ButtonImage));
				}
				item=new ListViewItem(new string[] {_arrayProcButtons[i].Description},_arrayProcButtons[i].ProcButtonNum.ToString());
				listViewButtons.Items.Add(item);
			}
    }

		private void FillToothChart(bool retainSelection) {
			if(_patCur==null) {
				FillToothChart(retainSelection,DateTime.Today);
			}
			else {
				FillToothChart(retainSelection,_listProcDates[trackToothProcDates.Value]);
			}
		}

		///<summary>This is, of course, called when module refreshed.  But it's also called when user sets missing teeth or tooth movements.  In that case, the Progress notes are not refreshed, so it's a little faster.  This also fills in the movement amounts.</summary>
		private void FillToothChart(bool retainSelection,DateTime dateLimit) {
			Cursor=Cursors.WaitCursor;
			_toothChartRelay.BeginUpdate();

			List<Definition> listChartGraphicColorDefs=Definitions.GetByCategory(DefinitionCategory.ChartGraphicColors, true);
			_toothChartRelay.ColorBackgroundMain=listChartGraphicColorDefs[10].Color;
			_toothChartRelay.ColorText=listChartGraphicColorDefs[11].Color;
			_toothChartRelay.ColorTextHighlightFore=listChartGraphicColorDefs[12].Color;
			_toothChartRelay.ColorTextHighlightBack=listChartGraphicColorDefs[13].Color;
			_toothChartRelay.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PreferenceName.UseInternationalToothNumbers));
			//remember which teeth were selected
			List<string> selectedTeeth=new List<string>(_toothChartRelay.SelectedTeeth);
			_toothChartRelay.ResetTeeth();
			if(_patCur==null) {
				_toothChartRelay.EndUpdate();
				FillMovementsAndHidden();
				Cursor=Cursors.Default;
				return;
			}
			//primary teeth need to be set before resetting selected teeth, because some of them might be primary.
			//primary teeth also need to be set before initial list so that we can set a primary tooth missing.
			for(int i=0;i<_listToothInitials.Count;i++) {
				if(_listToothInitials[i].InitialType==ToothInitialType.Primary) {
					_toothChartRelay.SetPrimary(_listToothInitials[i].ToothNum);
				}
			}
			if(checkShowTeeth.Checked || retainSelection) {
				for(int i=0;i<selectedTeeth.Count;i++) {
					_toothChartRelay.SetSelected(selectedTeeth[i],true);
				}
			}
			DataTable tableCur=LoadData.TableProgNotes.Copy();
			if(checkTreatPlans.Checked) {
				//filter list of DataRows to only include completed work and work for the selected treatment plans
				List<long> listProcNumsAll=gridTreatPlans.SelectedIndices.SelectMany(x => _listTreatPlans[x].ListProcTPs).Select(x => x.ProcNumOrig).ToList();
				_listRowsProcForGraphical=new List<DataRow>();
				_listProcsSkipped=new List<DataRow>();
				foreach(DataRow rowCur in tableCur.Rows) {
					//If proc status is anything except TP and TPi
					if(new[] { ProcStat.C,ProcStat.Cn,ProcStat.EC,ProcStat.EO,ProcStat.R }.Contains((ProcStat)PIn.Long(rowCur["ProcStatus"].ToString()))
						|| listProcNumsAll.Contains(PIn.Long(rowCur["ProcNum"].ToString())))
					{
						if(!ShouldRowShowGraphical(rowCur,dateLimit)) {
							continue;
						}
						_listRowsProcForGraphical.Add(rowCur);
					}
				}
			}
			else {
				//put list back to the original list of DataRows
				_listProcsSkipped=new List<DataRow>();
				List<string> listOrigProcNumStrs=_listProcListOrig.Select(x => x["ProcNum"].ToString()).ToList();
				_listRowsProcForGraphical=tableCur.Select().Where(x => listOrigProcNumStrs.Contains(x["ProcNum"].ToString()) && ShouldRowShowGraphical(x,dateLimit)).ToList();
			}
			_listToothInitialsCopy=_listToothInitials.Select(x => x.Copy()).ToList();
			foreach(DataRow row in _listProcsSkipped) {
				if(((ProcStat)PIn.Long(row["ProcStatus"].ToString())).In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(row["ProcCode"].ToString());
					if(procCode.PaintType==ToothPaintingType.Extraction) {
						_listToothInitialsCopy.RemoveAll(x => x.InitialType==ToothInitialType.Missing && x.ToothNum==row["ToothNum"].ToString());
					}
				}
			}
			//Also remove any extractions for TP procs that were set to TP by the ShouldRowShowGraphical
			foreach(DataRow row in _listRowsProcForGraphical) {
				DateTime dateTP;
				DateTime dateComplete;
				ProcStat procStatus=(ProcStat)PIn.Int(row["ProcStatus"].ToString());
				if(!procStatus.In(ProcStat.TP)) {
					continue;
				}
				if(!DateTime.TryParse(row["DateTP"].ToString(),out dateTP)) {
					continue;
				}
				if(!DateTime.TryParse(row["DateEntryC"].ToString(),out dateComplete)) {
					continue;
				}
				ProcedureCode procCode=ProcedureCodes.GetProcCode(row["ProcCode"].ToString());
				if(procCode.PaintType==ToothPaintingType.Extraction 
						&& dateLimit<dateComplete && dateLimit>=dateTP) 
				{//Procedure is C and the slider date is after or equal to the TP date, but before the completion date
					_listToothInitialsCopy.RemoveAll(x => x.InitialType==ToothInitialType.Missing && x.ToothNum==row["ToothNum"].ToString());//Pretend the row is TP for the tooth chart
				}
			}
			for(int i=0;i<_listToothInitialsCopy.Count;i++) {
				switch(_listToothInitialsCopy[i].InitialType) {
					case ToothInitialType.Missing:
						_toothChartRelay.SetMissing(_listToothInitialsCopy[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						_toothChartRelay.SetHidden(_listToothInitialsCopy[i].ToothNum);
						break;
					//case ToothInitialType.Primary:
					//	break;
					case ToothInitialType.Rotate:
						_toothChartRelay.MoveTooth(_listToothInitialsCopy[i].ToothNum,_listToothInitialsCopy[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						_toothChartRelay.MoveTooth(_listToothInitialsCopy[i].ToothNum,0,_listToothInitialsCopy[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						_toothChartRelay.MoveTooth(_listToothInitialsCopy[i].ToothNum,0,0,_listToothInitialsCopy[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						_toothChartRelay.MoveTooth(_listToothInitialsCopy[i].ToothNum,0,0,0,_listToothInitialsCopy[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						_toothChartRelay.MoveTooth(_listToothInitialsCopy[i].ToothNum,0,0,0,0,_listToothInitialsCopy[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						_toothChartRelay.MoveTooth(_listToothInitialsCopy[i].ToothNum,0,0,0,0,0,_listToothInitialsCopy[i].Movement);
						break;
					case ToothInitialType.Drawing:
						_toothChartRelay.AddDrawingSegment(_listToothInitialsCopy[i].Copy());
						break;
				}
			}
			DrawProcGraphics(dateLimit);
			_toothChartRelay.EndUpdate();
			FillMovementsAndHidden();
			if(dateLimit==DateTime.Today) {
				try {
					LoadData.ToothChartBM=_toothChartRelay.GetBitmap();
				}
				catch {
					//Failing to get the toothchart bitmap would only mean being unable to update the Patient Dashboard.  Since this is a fairly
					//rare exception, we can consider this to be not important enough to crash the program; the next module refresh should update the view.
				}
			}
			//By firing this event here, we propogate any charted procs/missing/movements immediately to the Patient Dashboard.  FillToothChart() is always
			//invoked via FillProgNotes() when ModuleSelected() is invoked, therefore, this event will fire when ModuleSelected() is invoked.  By only 
			//firing this event here, we avoid duplicate events.
			PatientDashboardDataEvent.Fire(EventCategory.ModuleSelected,LoadData);
			Cursor=Cursors.Default;
		}

		private void FillTrackSlider() {
			//This method can be called from many places and it would be annoying to the user if their slider always reset to today's date, so allow retaining selection.
			trackToothProcDates.Minimum=0;
			//FillToothChart is called after FillTrackSlider.  We don't need to fire the ValueChanged event, otherwise it calls FillToothChart unnecessarily
			trackToothProcDates.ValueChanged-=trackToothProcDates_ValueChanged;
			trackToothProcDates.Value=0;
			List<string> listProcNumStrsOrig=_listProcListOrig.Select(y => y["ProcNum"].ToString()).ToList();
			//Fill the list of unique procedure dates with the new values found in the ProgNotes data table.
			//The proc dates can include TP, Completed, and Scheduled dates.  Each of these dates are significant in the patients history (visually).
			_listProcDates=LoadData.TableProgNotes.Select().Where(x => listProcNumStrsOrig.Contains(x["ProcNum"].ToString()))
				.SelectMany(x => new[] { x["DateTP"].ToString(),x["DateEntryC"].ToString(),((DateTime)x["ProcDate"]).ToShortDateString() }.Distinct())
				.Concat(new[] { DateTime.Today.ToShortDateString() })
				.Distinct()
				.Where(x => x!=DateTime.MinValue.ToShortDateString())
				.Select(x => PIn.Date(x))
				.OrderBy(x => x)
				.ToList();
			trackToothProcDates.Maximum=_listProcDates.Count()-1;
			trackToothProcDates.Value=_listProcDates.FindIndex(x => x==DateTime.Today);//Default to today's date which is guaranteed to be in our track bar
			trackToothProcDates.ValueChanged+=trackToothProcDates_ValueChanged;//Add the ValueChanged event handler back after setting the track bar value
			textToothProcDate.Text=_listProcDates[trackToothProcDates.Value].ToShortDateString();
		}

		private void FillTreatPlans() {
			gridTreatPlans.BeginUpdate();
			gridTreatPlans.Columns.Clear();
			gridTreatPlans.Columns.Add(new GridColumn("Status",50));
			gridTreatPlans.Columns.Add(new GridColumn("Heading",60){ IsWidthDynamic=true });
			gridTreatPlans.Columns.Add(new GridColumn("Procs",50,HorizontalAlignment.Center));
			gridTreatPlans.Rows.Clear();
			if(_patCur==null || !checkTreatPlans.Checked) {
				gridTreatPlans.EndUpdate();
				return;
			}
			_listPatProcs=Procedures.Refresh(_patCur.PatNum);
			_arrayProceduresTp=Procedures.GetListTPandTPi(_listPatProcs);//sorted by priority, then toothnum
			_listTreatPlans=TreatPlans.GetAllCurrentForPat(_patCur.PatNum);
			GridRow row;
			List<TreatPlanAttach> listTpAttaches=TreatPlanAttaches.GetAllForPatNum(_patCur.PatNum);
			for(int i=0;i<_listTreatPlans.Count;i++) {
				row=new GridRow();
				row.Cells.Add(_listTreatPlans[i].TPStatus.ToString());
				row.Cells.Add(_listTreatPlans[i].Heading);
				//if(_listTreatPlans[i].ResponsParty!=0) {
				//	//This should never be used for Active or Inactive treatment plans. Saved TPs only.
				//	str+="\r\n"+"Responsible Party: "+Patients.GetLim(_listTreatPlans[i].ResponsParty).GetNameLF();
				//}
				row.Cells.Add(listTpAttaches.FindAll(x=>x.TreatPlanNum==_listTreatPlans[i].TreatPlanNum).Count.ToString());
				row.Tag=listTpAttaches.FindAll(x => x.TreatPlanNum==_listTreatPlans[i].TreatPlanNum);
				gridTreatPlans.Rows.Add(row);
			}
			gridTreatPlans.EndUpdate();
			gridTreatPlans.SetSelected(0,true);
		}

		///<summary>Calls FillTpProcData and FillTpProcDisplay as well as showing checkTreatPlans and filling the priority list.</summary>
		private void FillTpProcs() {
			if(!checkTreatPlans.Checked) {
				return;
			}
			FillTpProcData();
			FillTpProcDisplay();
		}
		
		/// <summary>Fills _dictTpNumListTpRows with TreatPlanNums linked to the list of TpRows for the TP, used to fill gridTpProcs.</summary>
		private void FillTpProcData() {
			_dictTpNumListTpRows=new Dictionary<long,List<TpRow>>();
			List<TpRow> listTpRows;
			for(int i=0;i<gridTreatPlans.SelectedIndices.Length;i++) {
				listTpRows=new List<TpRow>();
				long treatPlanNumCur=_listTreatPlans[gridTreatPlans.SelectedIndices[i]].TreatPlanNum;
				List<TreatPlanAttach> listTreatPlanAttaches=(List<TreatPlanAttach>)gridTreatPlans.Rows[gridTreatPlans.SelectedIndices[i]].Tag;
				List<Procedure> listProcsForTP=Procedures.GetManyProc(listTreatPlanAttaches.Select(x => x.ProcNum).ToList(),false)
					.OrderBy(x => Definitions.GetOrder(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==x.ProcNum).Priority)<0)
					.ThenBy(x => Definitions.GetOrder(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==x.ProcNum).Priority))
					.ThenBy(x => Tooth.ToInt(x.ToothNum))
					.ThenBy(x => x.ProcDate).ToList();
				List<ProcTP> listProcTPsCur=new List<ProcTP>();
				TpRow row;
				for(int j=0;j<listProcsForTP.Count;j++) {
					row=new TpRow();
					//Fill TpRow object with information.
					row.Priority=Definitions.GetName(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==listProcsForTP[j].ProcNum).Priority);
					row.Tth=Tooth.ToInternat(listProcsForTP[j].ToothNum);
					if(ProcedureCodes.GetById(listProcsForTP[j].CodeNum).TreatmentArea==ProcedureTreatmentArea.Surface) {
						row.Surf=Tooth.SurfTidyFromDbToDisplay(listProcsForTP[j].Surf,listProcsForTP[j].ToothNum);
					}
					else if(ProcedureCodes.GetById(listProcsForTP[j].CodeNum).TreatmentArea==ProcedureTreatmentArea.Sextant) {
						row.Surf=Tooth.GetSextant(listProcsForTP[j].Surf,(ToothNumberingNomenclature)PrefC.GetInt(PreferenceName.UseInternationalToothNumbers));
					}
					else {
						row.Surf=listProcsForTP[j].Surf; //I think this will properly allow UR, L, etc.
					}
					row.Code=ProcedureCodes.GetById(listProcsForTP[j].CodeNum).Code;//returns new ProcedureCode if not found
					string descript=ProcedureCodes.GetLaymanTerm(listProcsForTP[j].CodeNum);
					if(listProcsForTP[j].ToothRange!="") {
						descript+=" #"+Tooth.FormatRangeForDisplay(listProcsForTP[j].ToothRange);
					}
					row.Description=descript;
					row.ColorText=Definitions.GetColor(DefinitionCategory.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==listProcsForTP[j].ProcNum).Priority);
					if(row.ColorText==System.Drawing.Color.White) {
						row.ColorText=System.Drawing.Color.Black;
					}
					Procedure proc=listProcsForTP[j];
					ProcTP procTP=new ProcTP();//dummy ProcTP for local list listProcTPsCur, used as the tag on this TP grid row
					procTP.PatNum=_patCur.PatNum;
					procTP.TreatPlanNum=treatPlanNumCur;
					procTP.ProcNumOrig=proc.ProcNum;
					procTP.ItemOrder=i;
					procTP.Priority=listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==proc.ProcNum).Priority;
					procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
					if(ProcedureCodes.GetById(proc.CodeNum).TreatmentArea==ProcedureTreatmentArea.Surface) {
						procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
					}
					else {
						procTP.Surf=proc.Surf;//for UR, L, etc.
					}
					procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
					procTP.Descript=row.Description;
					procTP.Prognosis=row.Prognosis;
					procTP.Dx=row.Dx;
					listProcTPsCur.Add(procTP);
					row.Tag=procTP;
					listTpRows.Add(row);
				}
				//if there is another treatment plan after this one, add a row with just the TP Header and a bold lower line
				if(i<gridTreatPlans.SelectedIndices.Length-1 && listTpRows.Count>0) {
					listTpRows[listTpRows.Count-1].ColorLborder=Color.FromArgb(102,102,122);
				}
				_listTreatPlans[gridTreatPlans.SelectedIndices[i]].ListProcTPs=listProcTPsCur;
				_dictTpNumListTpRows[_listTreatPlans[gridTreatPlans.SelectedIndices[i]].TreatPlanNum]=listTpRows;
			}
		}

		///<summary>Fills gridTpProcs with data in _dictTpNumListTpRows.  Could be filled with procs from more than one TP.</summary>
		private void FillTpProcDisplay() {
			gridTpProcs.BeginUpdate();
			gridTpProcs.Columns.Clear();
			gridTpProcs.Columns.Add(new GridColumn("Priority",50));
			gridTpProcs.Columns.Add(new GridColumn("Tth",35));
			gridTpProcs.Columns.Add(new GridColumn("Surf",40));
			gridTpProcs.Columns.Add(new GridColumn("Code",50));
			gridTpProcs.Columns.Add(new GridColumn("Description",50){ IsWidthDynamic=true });
			gridTpProcs.Rows.Clear();
			if(_patCur==null || _dictTpNumListTpRows==null || gridTreatPlans.Rows.Count==0) {
				gridTpProcs.EndUpdate();
				return;
			}
			GridRow row;
			foreach(KeyValuePair<long,List<TpRow>> kvPair in _dictTpNumListTpRows) {
				row=new GridRow();
				if(_dictTpNumListTpRows.Count>1) {
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add(_listTreatPlans.FindAll(x => x.TreatPlanNum==kvPair.Key).DefaultIfEmpty(new TreatPlan() { Heading="" }).FirstOrDefault().Heading);
					row.Bold=true;
					row.LowerBorderColor=Color.FromArgb(102,102,122);//from odGrid painting logic
					row.BackColor=Color.FromArgb(224,223,227);//from odGrid painting logic
					gridTpProcs.Rows.Add(row);
					row=new GridRow();
				}
				foreach(TpRow tpRow in kvPair.Value) {
					ProcTP procTp=new ProcTP();
					if(tpRow.Tag!=null) {
						procTp=(ProcTP)tpRow.Tag;
					}
					row.Cells.Add(tpRow.Priority??"");
					row.Cells.Add(tpRow.Tth??"");
					row.Cells.Add(tpRow.Surf??"");
					row.Cells.Add(tpRow.Code??"");
					row.Cells.Add(tpRow.Description??"");
					row.ForeColor=tpRow.ColorText;
					row.LowerBorderColor=tpRow.ColorLborder;
					row.Tag=tpRow.Tag;//Tag is a ProcTP
					row.Bold=tpRow.Bold;
					gridTpProcs.Rows.Add(row);
					row=new GridRow();
				}
			}
			gridTpProcs.EndUpdate();
		}

		///<summary>Returns the appropriate ChartModuleComponentsToLoad.</summary>
		private ChartModuleComponentsToLoad GetChartModuleComponents() {
			return new ChartModuleComponentsToLoad(
				checkAppt.Checked,        //showAppointments
				checkComm.Checked,        //showCommLog
				checkShowC.Checked,       //showCompleted
				checkShowCn.Checked,      //showConditions
				checkEmail.Checked,       //showEmail
				checkShowE.Checked,       //showExisting
				checkCommFamily.Checked,  //showFamilyCommLog
				true,                     //showFormPat
				checkLabCase.Checked,     //showLabCases
				checkNotes.Checked,       //showProcNotes
				checkShowR.Checked,       //showReferred
				checkRx.Checked,          //showRX
				checkSheets.Checked,      //showSheets, consent
				checkTasks.Checked,       //showTasks
				checkShowTP.Checked);     //showTreatPlan
		}

		///<summary>Returns a dictionary such that the key is a sheetFieldDef.FieldName and the value is a corresponding control from this instance of ContrChart. Dictionary values can be null if control is not matched or it is matched to an HQ only control and not at HQ.</summary>
		private Dictionary<string,Control> GetSheetFieldDefControlDict() {
			SheetFieldLayoutMode layoutModeCur=GetSheetLayoutMode();
			Dictionary<string,Control> dictControls=new Dictionary<string,Control>();
			//Use internal list because it implements all sheetFieldDefs.  We expect the def to always match the controls in the Chart.
			List<SheetFieldDef> listAllSheetFieldDefs=SheetsInternal.GetSheetDef(SheetInternalType.ChartModule).SheetFieldDefs;
			foreach(SheetFieldDef fieldDef in listAllSheetFieldDefs) {
				if(fieldDef.LayoutMode!=layoutModeCur) {
					continue;
				}
				Control control=null;
				//bool isHqOrDistibutorControl=false;
				switch(fieldDef.FieldName) {
					#region Set control based on matching FieldName
					case "PatientInfo":
						control=this.gridPtInfo;
						if(control.Parent!=this) {//Depending on the mode this control can be moved into tabProc
							this.Controls.Add(control);
						}
						break;
					case "ProgressNotes":
						control=this.panelGridProg;
						break;
					case "ChartModuleTabs":
						control=this.tabProc;
						break;
					case "TreatmentNotes":
						control=textTreatmentNotes;
						break;
					case "TrackToothProcDates":
						control=panelToothTrackBar;
						break;
					case "toothChart":
							control=toothChartWrapper;
						
						break;
					case "PanelEcw":
						control=panelEcw;
						break;
					case "ButtonNewTP":
						control=butNewTP;
						break;
					case "Procedures":
						control=gridTpProcs;
						break;
					case "TreatmentPlans":
						control=gridTreatPlans;
						break;
					case "SetPriorityListBox":
						control=panelTPpriority;
						break;
					#endregion
					default:
						//Control not matched
						break;
				}

				dictControls.Add(fieldDef.FieldName,control);//Can add null
			}
			return dictControls;
		}

		///<summary>This mimics FormSheetDefEdit.InitLayoutModes() logic.</summary>
		private SheetFieldLayoutMode GetSheetLayoutMode() {
			SheetFieldLayoutMode sheetLayoutModeCur;
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				if(checkTreatPlans.Checked) {
					sheetLayoutModeCur=SheetFieldLayoutMode.MedicalPracticeTreatPlan;
				}
				else {
					sheetLayoutModeCur=SheetFieldLayoutMode.MedicalPractice;
				}
			}
			else if(Programs.UsingOrion) {
				if(checkTreatPlans.Checked) {
					sheetLayoutModeCur=SheetFieldLayoutMode.OrionTreatPlan;
				}
				else {
					sheetLayoutModeCur=SheetFieldLayoutMode.Orion;
				}
				gridPtInfo.Visible=true;
				//Logic below mimics old ChartLayoutHelper
				gridPtInfo.Location=new Point(0,0);
				gridPtInfo.Size=new Size(tabPatInfo.ClientSize.Width,tabPatInfo.ClientSize.Height);
				gridPtInfo.Anchor=AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
				tabPatInfo.Controls.Add(gridPtInfo);
				if(!tabProc.TabPages.Contains(tabPatInfo)) {
					tabProc.TabPages.Insert(1,tabPatInfo);
				}
			}
			else if(checkTreatPlans.Checked) {
				sheetLayoutModeCur=SheetFieldLayoutMode.TreatPlan;
			}
			else {
				sheetLayoutModeCur=SheetFieldLayoutMode.Default;
				tabProc.TabPages.Remove(tabPatInfo);
			}
			return sheetLayoutModeCur;
		}

		private void GetXVWebImages(ODThread thread) {
			Patient patient=(Patient)thread.Parameters[0];
			List<string> listIdsToExclude=(List<string>)thread.Parameters[1];
			lock(_apteryxLocker) {
				if(_listApteryxThumbnails==null) {
					_listApteryxThumbnails=new List<ApteryxThumbnail>();
				}
				_listApteryxThumbnails.RemoveAll(x => x.PatNum!=patient.PatNum);
				listIdsToExclude.AddRange(_listApteryxThumbnails.Select(x => x.Image.Id.ToString()));
			}
			bool doDisplayXVWebInChart=XVWeb.IsDisplayingImagesInProgram
				&& Definitions.GetDefsForCategory(DefinitionCategory.ImageCats,true).Any(x => x.Value.Contains("X") //if tagged to show in Chart
				&& x.Id==PIn.Long(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.XVWeb),XVWeb.ProgramProps.ImageCategory)));
			if(!doDisplayXVWebInChart) {
				return;
			}
			//make requests to the XVWeb Api to get a list of images for this patient.
			List<ApteryxThumbnail> listAT=new List<ApteryxThumbnail>();
			foreach(ApteryxThumbnail thumbnail in XVWeb.GetListThumbnails(patient,listIdsToExclude)) {
				lock(_apteryxLocker) {
					_listApteryxThumbnails.Add(thumbnail);
				}
				DisplayXVWebImages(patient.PatNum);
			}
		}

		///<summary>Returns an ODGridRow object which dictates how the row passed in should be displayed.</summary>
		private GridRow GridProgRowConstruction(DataRow rowCur,List<DisplayField> fields) {
			long procNumCur=PIn.Long(rowCur["ProcNum"].ToString());//increase code efficiency
			GridRow row=new GridRow();
			row.LowerBorderColor=Color.Black;
			//remember that columns that start with lowercase are already altered for display rather than being raw data.
			for(int f=0;f<fields.Count;f++) {
				switch(fields[f].InternalName) {
					case "Date":
						row.Cells.Add(rowCur["procDate"].ToString());
						break;
					case "Time":
						row.Cells.Add(rowCur["procTime"].ToString());
						break;
					case "Th":
						row.Cells.Add(rowCur["toothNum"].ToString());
						break;
					case "Surf":
						row.Cells.Add(rowCur["surf"].ToString());
						break;
					case "Dx":
						row.Cells.Add(rowCur["dx"].ToString());
						break;
					case "Description":
						row.Cells.Add(rowCur["description"].ToString());
						break;
					case "Stat":
						long procNum=PIn.Long(rowCur["ProcNum"].ToString());
						if(ProcMultiVisits.IsProcInProcess(procNum)) {
							row.Cells.Add(ProcStatExt.InProcess);
						}
						else {
							row.Cells.Add(rowCur["procStatus"].ToString());//Already translated for display
						}
						break;
					case "Prov":
						row.Cells.Add(rowCur["prov"].ToString());
						break;
					case "Amount":
						row.Cells.Add(rowCur["procFee"].ToString());
						break;
					case "Proc Code":
						row.Cells.Add(rowCur["ProcCode"].ToString());
						break;
					case "User":
						row.Cells.Add(rowCur["user"].ToString());
						break;
					case "Signed":
						row.Cells.Add(rowCur["signature"].ToString());
						break;
					case "Priority":
						row.Cells.Add(rowCur["priority"].ToString());
						break;
					case "Date Entry":
						row.Cells.Add(rowCur["dateEntryC"].ToString());
						break;
					case "Prognosis":
						row.Cells.Add(rowCur["prognosis"].ToString());
						break;
					case "Date TP":
						row.Cells.Add(rowCur["dateTP"].ToString());
						break;
					case "End Time":
						row.Cells.Add(rowCur["procTimeEnd"].ToString());
						break;
					case "Quadrant":
						row.Cells.Add(rowCur["quadrant"].ToString());
						break;
					case "Schedule By":
						row.Cells.Add(rowCur["orionDateScheduleBy"].ToString());
						break;
					case "Stop Clock":
						row.Cells.Add(rowCur["orionDateStopClock"].ToString());
						break;
					case "DPC":
						row.Cells.Add(rowCur["orionDPC"].ToString());
						break;
					case "Effective Comm":
						row.Cells.Add(rowCur["orionIsEffectiveComm"].ToString());
						break;
					case "On Call":
						row.Cells.Add(rowCur["orionIsOnCall"].ToString());
						break;
					case "Stat 2":
						row.Cells.Add(rowCur["orionStatus2"].ToString());
						break;
					case "DPCpost":
						row.Cells.Add(rowCur["orionDPCpost"].ToString());
						break;
					case "Length":
						row.Cells.Add(rowCur["length"].ToString());
						break;
					case "Abbr": //abbreviation for procedures
						row.Cells.Add(rowCur["AbbrDesc"].ToString());
						break;
					case "Locked":
						row.Cells.Add(rowCur["isLocked"].ToString());
						break;
					case "HL7 Sent":
						row.Cells.Add(rowCur["hl7Sent"].ToString());
						break;
					case "Clinic":
						row.Cells.Add(Clinics.GetAbbr(PIn.Long(rowCur["ClinicNum"].ToString())));
						break;
					case "ClinicDesc":
						row.Cells.Add(Clinics.GetDescription(PIn.Long(rowCur["ClinicNum"].ToString())));
						break;
					//If you add something here, you should also add it to SearchProgNotesMethod.
					default:
						row.Cells.Add("");
						break;
				}
			}
			if(checkNotes.Checked) {
				row.Note=rowCur["note"].ToString();
			}
			row.ForeColor=Color.FromArgb(PIn.Int(rowCur["colorText"].ToString()));
			long provNum=PIn.Long(rowCur["ProvNum"].ToString());
			if(Preferences.GetBool(PreferenceName.UseProviderColorsInChart)
					&& procNumCur>0
					&& provNum>0
					&& new[] { ProcStat.C,ProcStat.EC }.Contains((ProcStat)PIn.Int(rowCur["ProcStatus"].ToString())))
			{
				row.BackColor=Providers.GetColor(provNum);
			}
			else {
				row.BackColor=Color.FromArgb(PIn.Int(rowCur["colorBackG"].ToString()));
			}
			row.Tag=rowCur;
			return row;
		}

		///<summary>Returns true if eCW is enabled and they turned on the Hide Chart Rx Buttons setting within the program link.</summary>
		private bool HasHideRxButtonsEcw() {
			if(Programs.IsEnabled(ProgramName.eClinicalWorks) 
				&& ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"HideChartRxButtons")=="1") 
			{
				return true;
			}
			return false;
		}

		private bool IsAuditMode(bool isSilent) {
			if(gridProg.SelectedIndices.Count(x => x>-1 && x<gridProg.Rows.Count)==0) {
				if(!isSilent) {
					MessageBox.Show("Please select an item first."); 
				}
				return false;
			}
			if(checkAudit.Checked) {
				if(!isSilent) {
					MessageBox.Show("Not allowed in audit mode."); 
				}
				return false;
			}
			return true;
		}

		///<summary>Creates log entries for completed procedures</summary>
		private void logComplCreate(Procedure procCur) {
			if(_procStatusNew!=ProcStat.C) {
				return;
			}
			string teeth=String.Join(", ",_toothChartRelay.SelectedTeeth);
			Procedures.LogProcComplCreate(_patCur.PatNum,procCur,teeth);
		}

		///<summary>Sets the selected rows to the ProcStatus passed in.</summary>
		private void MenuItemSetSelectedProcsStatus(ProcStat newProcStatus) {
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			if(!listSelectedRows.All(x => CanChangeProcsStatus(newProcStatus,x,doCheckDb:true,isSilent:false))) {
				return;
			}
			List<string> listProcCodes=new List<string>();//for automation
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(_patCur.PatNum) ;
			OrthoCaseProcLinkingData orthoCaseProcLinkingData=new OrthoCaseProcLinkingData(_patCur.PatNum);
			foreach(DataRow row in listSelectedRows) {
				Procedure procOld=Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),true);
				if(procOld.ProcStatus==newProcStatus) {
					continue;
				}
				Procedure procNew=procOld.Copy();
				procNew.ProcStatus=newProcStatus;
				if(procNew.ProcStatus==ProcStat.C) {//Proc set complete.
					#region Setting proc complete
					#region Proc note
					//if procedure was already complete, then don't add more notes.
					//Prompt for default note if the preference is true.
					string procNoteDefault=ProcCodeNotes.GetNote(procNew.ProvNum,procNew.CodeNum,ProcStat.C);
					if(procNew.Note!="" && procNoteDefault!="") {
						procNew.Note+="\r\n"; //add a new line if there was already a ProcNote on the procedure.
					}
					procNew.Note+=procNoteDefault;
					if(!Preferences.GetBool(PreferenceName.ProcPromptForAutoNote)) {
						//Users do not want to be prompted for auto notes, so remove them all from the procedure note.
						procNew.Note=Regex.Replace(procNew.Note,@"\[\[.+?\]\]","");
					}
					#endregion
					procNew.DateEntryC=DateTime.Now;//Should this be server date?
					if(procNew.DiagnosticCode=="") {
						procNew.DiagnosticCode=Preferences.GetString(PreferenceName.ICD9DefaultForNewProcs);
						procNew.IcdVersion=Preferences.GetByte(PreferenceName.DxIcdVersion);
					}
					//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
					if(ProcedureCodes.GetStringProcCode(procNew.CodeNum).In("D9986","D9987")) {
						Procedures.SetDateFirstVisit(procNew.ProcDate,2,_patCur);
					}
					listProcCodes.Add(ProcedureCodes.GetStringProcCode(procNew.CodeNum));
					#endregion
				}
				if(procNew.AptNum!=0) {//if attached to an appointment
					Appointment appt=Appointments.GetOneApt(procNew.AptNum);
					procNew.ClinicNum=appt.ClinicNum;
					procNew.ProcDate=appt.AptDateTime;
					procNew.PlaceService=Clinics.GetPlaceService(appt.ClinicNum);
				}
				else {
					procNew.ProcDate=PIn.Date(textDate.Text);
					procNew.PlaceService=Preferences.GetString(PreferenceName.DefaultProcedurePlaceService, PlaceOfService.Office);
				}
				if(procNew.ProcDate.Year<1880) {
					procNew.ProcDate=MiscData.GetNowDateTime();
				}
				procNew.SiteNum=_patCur.SiteNum;
				if(procNew.ProcStatus.In(ProcStat.EC,ProcStat.EO)) {
					procNew.DiagnosticCode="";
				}
				if(Users.IsUserCpoe(Security.CurrentUser)) {
					//Only change the status of IsCpoe to true.  Never set it back to false for any reason.  Once true, always true.
					procNew.IsCpoe=true;
				}
				Plugins.HookAddCode(this,"ContrChart.menuItemSetComplete_Click_procLoop",procNew,procOld);
				ProcedureCode procCode=ProcedureCodes.GetById(procNew.CodeNum);
				OrthoProcLink orthoProcLink=OrthoProcLinks.TryLinkProcForActiveOrthoCase(orthoCaseProcLinkingData,procNew);
				bool isProcLinkedToOrthoCase=orthoProcLink!=null;
				Procedures.FormProcEditUpdate(procNew,procOld,procCode,isProcLinkedToOrthoCase);
				if(isProcLinkedToOrthoCase) {//If proc was linked to an ortho case, pass ortho case data to ComputeEstimates.
					Procedures.ComputeEstimates(procNew,procNew.PatNum,listClaimProcs,false,_listInsPlans,_listPatPlans,_listBenefits,_patCur.Age,_listInsSubs,
						orthoProcLink,orthoCaseProcLinkingData.ActiveOrthoCase,orthoCaseProcLinkingData.OrthoSchedule,orthoCaseProcLinkingData.ListProcLinksForCase);
				}
				else {
					Procedures.ComputeEstimates(procNew,procNew.PatNum,listClaimProcs,false,_listInsPlans,_listPatPlans,_listBenefits,_patCur.Age,_listInsSubs);
				}
			}
			//Mimics FormProcEdit.SaveAndClose()
			Recalls.Synch(_patCur.PatNum);
			if(listProcCodes.Count>0) {//Only do if we are completing the procedures. 
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,listProcCodes,_patCur.PatNum);
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void ModuleSelectedDoseSpot() {
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)delegate () {
					ModuleSelectedDoseSpot();
				});
				return;
			}
			if(_patCur!=null) {//If a user switches to another module, PatCur can be null
				ModuleSelected(_patCur.PatNum);//Always use PatCur because by the time this gets called the patient has been changed to reflect in PatCur.
			}
		}

		///<summary>Returns false if account ID is blank or not in format of 1 or more digits, followed by 3 random alpha-numberic characters, followed by a 2 digit checksum. Only returns true when the NewCrop Account ID is one that was created by OD.</summary>
 		private bool NewCropIsAccountIdValid() {
			bool validKey=false;
			string newCropAccountId=Preferences.GetString(PreferenceName.NewCropAccountId);
			if(Regex.IsMatch(newCropAccountId,"[0-9]+\\-[0-9A-Za-z]{3}[0-9]{2}")) { //Must contain at least 1 digit for patnum, 1 dash, 3 random alpha-numeric characters, then 2 digits for checksum.
				//Verify key checksum to make certain that this key was generated by OD and not a reseller.
				long patNum=PIn.Long(newCropAccountId.Substring(0,newCropAccountId.IndexOf('-')));
				long checkSum=patNum;
				checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+1])*3;
				checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+2])*5;
				checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+3])*7;
				if((checkSum%100).ToString().PadLeft(2,'0')==newCropAccountId.Substring(newCropAccountId.Length-2)) {
					validKey=true;
				}
			}
			return validKey;
		}

		///<summary>Returns true if new information was pulled back from NewCrop.</summary>
		private bool NewCropRefreshPrescriptions() {
			Program programNewCrop=Programs.GetCur(ProgramName.eRx);
			if(ToolBarMain.Buttons["eRx"]!=null) {//Hidden for eCW
				ToolBarMain.Buttons["eRx"].IsRed=false; //Set the eRx button back to default color.
				ToolBarMain.Invalidate();
			}
			if(!programNewCrop.Enabled) {
				return false;
			}
			if(_patCur==null) {
				return false;
			}
			ErxOption erxOption=PIn.Enum<ErxOption>(ProgramProperties.GetPropForProgByDesc(programNewCrop.Id,Erx.PropertyDescs.ErxOption).Value);
			if(erxOption!=ErxOption.Legacy && erxOption!=ErxOption.DoseSpotWithLegacy) {
				return false;
			}
			string newCropAccountId=Preferences.GetString(PreferenceName.NewCropAccountId);
			if(newCropAccountId=="") {//We check for NewCropAccountID validity below, but we also need to be sure to exit this check for resellers if blank.
				return false;
			}
			if(!NewCropIsAccountIdValid()) {
				//The NewCropAccountID will be invalid for resellers, because the checksum will be wrong.
				//Therefore, resellers should be allowed to continue if both the NewCropName and NewCropPassword are specified. NewCrop does not allow blank passwords.
				if(Preferences.GetString(PreferenceName.NewCropName)=="" || Preferences.GetString(PreferenceName.NewCropPassword)=="") {
					return false;
				}
			}
			Imedisoft.NewCrop.Update1 wsNewCrop=new Imedisoft.NewCrop.Update1();//New Crop web services interface.
			Imedisoft.NewCrop.Credentials credentials=new Imedisoft.NewCrop.Credentials();
			Imedisoft.NewCrop.AccountRequest accountRequest=new Imedisoft.NewCrop.AccountRequest();
			Imedisoft.NewCrop.PatientRequest patientRequest=new Imedisoft.NewCrop.PatientRequest();
			Imedisoft.NewCrop.PrescriptionHistoryRequest prescriptionHistoryRequest=new Imedisoft.NewCrop.PrescriptionHistoryRequest();
			Imedisoft.NewCrop.PatientInformationRequester patientInfoRequester=new Imedisoft.NewCrop.PatientInformationRequester();
			Imedisoft.NewCrop.Result response=new Imedisoft.NewCrop.Result();
#if DEBUG
			wsNewCrop.Url="https://preproduction.newcropaccounts.com/v7/WebServices/Update1.asmx";
#endif
			credentials.PartnerName=OpenDentBusiness.NewCrop.NewCropPartnerName;
			credentials.Name=OpenDentBusiness.NewCrop.NewCropAccountName;
			credentials.Password=OpenDentBusiness.NewCrop.NewCropAccountPasssword;
			accountRequest.AccountId=newCropAccountId;
			accountRequest.SiteId="1";//Accounts are always created with SiteId=1.
			patientRequest.PatientId=POut.Long(_patCur.PatNum);
			prescriptionHistoryRequest.StartHistory=new DateTime(2012,11,2);//Only used for archived prescriptions. This is the date of first release for NewCrop integration.
			prescriptionHistoryRequest.EndHistory=DateTime.Now;//Only used for archived prescriptions.
			//Prescription Archive Status Values:
			//N = Not archived (i.e. Current Medication) 
			//Y = Archived (i.e. Previous Mediation)
			//% = Both Not Archived and Archived
			//Note: This field will contain values other than Y,N in future releases.
			prescriptionHistoryRequest.PrescriptionArchiveStatus="N";
			//Prescription Status Values:
			//C = Completed Prescription
			//P = Pending Medication
			//% = Both C and P.
			prescriptionHistoryRequest.PrescriptionStatus="C";
			//Prescription Sub Status Values:
			//% = All meds (Returns all meds regardless of the sub status)
			//A = NS (Returns only meds that have a 'NS' - Needs staff sub status)
			//U = DR (Returns only meds that have a 'DR' - Needs doctor review sub status)
			//P = Renewal Request that has been selected for processing on the NewCrop screens - it has not yet been denied, denied and re-written or accepted
			//S = Standard Rx (Returns only meds that have an 'InProc' - InProcess sub status)
			//D = DrugSet source - indicates the prescription was created by selecting the medication from the DrugSet selection box on the ComposeRx page
			//O = Outside Prescription - indicates the prescription was created on the MedEntry page, not prescribed.
			prescriptionHistoryRequest.PrescriptionSubStatus="S";
			patientInfoRequester.UserType="Staff";//Allowed values: Doctor,Staff
			if(Security.CurrentUser.ProviderId!=0) {//If the current OD user is associated to a doctor, then the request is from a doctor, otherwise from a staff member.
			  patientInfoRequester.UserType="Doctor";
			}
			patientInfoRequester.UserId=POut.Long(Security.CurrentUser.Id);
			//Send the request to NewCrop. Always returns all current medications, and returns medications between the StartHistory and EndHistory dates if requesting archived medications.
			//The patientIdType parameter was added for another vendor and is not often used. We do not use this field. We must pass empty string.
			//The includeSchema parameter is useful for first-time debugging, but in release mode, we should pass N for no.
			wsNewCrop.Timeout=3000;//3 second. The default is 100 seconds, but we cannot wait that long, because prescriptions are checked each time the Chart is refreshed. 1 second is too little, 2 seconds works most of the time. 3 seconds is safe.
			try {
				//throw new Exception("Test communication error in debug mode.");
				response=wsNewCrop.GetPatientFullMedicationHistory6(credentials,accountRequest,patientRequest,prescriptionHistoryRequest,patientInfoRequester,"","N");
			}
			catch { //An exception is thrown when the timeout is reached, or when the NewCrop servers are not accessible (because the servers are down, or because local internet is down).
				//We used to show a popup here each time the refresh failed, but users found it annoying when the NewCrop severs were down, because the popup would show each time they visited the Chart and impeded user workflow.
				//We tried silently logging a warning message into the Application log within system Event Viewer, but we found out that a decent number of users do not have permission to write to the Application log, which causes UEs sometimes.
				//We tried showing a popup exactly 1 time for each instance of OD launched, to avoid the permission issue, but users were still complaining about it popping up and they didn't know what to do to fix it.
				//We now change the background color of the eRx button red, and show an error message when user click the eRx button to alert them that interactions may be out of date.
				if(ToolBarMain.Buttons["eRx"]!=null) {//Hidden for eCW
					ToolBarMain.Buttons["eRx"].IsRed=true; //Marks the eRx button to be drawn with a red color.
					ToolBarMain.Invalidate();
				}
				return false;
			}
			
			//response.Message = Error message if error.
			//response.RowCount = Number of prescription records returned.
			//response.Status = Status of request. "OK" = success.
			//response.Timing = Not sure what this is for. Tells us how quickly the server responded to the request?
			//response.XmlResponse = The XML data returned, encoded in base 64.
			if (response.Status!= Imedisoft.NewCrop.StatusType.OK) {//Other statuses include Fail (ex if credentials are invalid), NotFound (ex if patientId invalid or accoundId invalid), Unknown (no known examples yet)
				//For now we simply abort gracefully.
				return false;
			}
			byte[] xmlResponseBytes=Convert.FromBase64String(response.XmlResponse);
			string xmlResponse=Encoding.UTF8.GetString(xmlResponseBytes);
			if(xmlResponse=="") {//An empty result means that the patient does not currently have any active medications in eRx.
				xmlResponse="<emptyResult/>";//At least one node is needed below to prevent crashing.
				//We need to continue to the bottom of this function even when there are no active medications,
				//so that we can discontinue any medications in the database which were active that are now discontinued in eRx.
			}
#if DEBUG//For capturing the xmlReponse with the newlines properly showing.
			string tempFile= Storage.GetTempFileName(".txt");
			File.WriteAllText(tempFile,xmlResponse);
#endif
			XmlDocument xml=new XmlDocument();
			try {
				xml.LoadXml(xmlResponse);
			}
			catch { //In case NewCrop returns invalid XML.
				return false;//abort gracefully
			}
			DateTime rxStartDateT=PrefC.GetDate(PreferenceName.ElectronicRxDateStartedUsing131);
			XmlNode nodeNewDataSet=xml.FirstChild;
			List <long> listActiveMedicationPatNums=new List<long>();
			List<RxPat> listNewRx=new List<RxPat>();
			foreach(XmlNode nodeTable in nodeNewDataSet.ChildNodes) {
				RxPat rxOld=null;
				MedicationPat medOrderOld=null;
				RxPat rx=new RxPat();
				//rx.IsControlled not important.  Only used in sending, but this Rx was already sent.
				rx.Disp="";
				rx.DosageCode="";
				rx.Drug="";
				rx.Notes="";
				rx.Refills="";
				rx.SendStatus=RxSendStatus.Unsent;
				rx.Sig="";
				rx.ErxPharmacyInfo="";
				string additionalSig="";
				bool isProv=true;
				long rxCui=0;
				string strDrugName="";
				string strGenericName="";
				string strProvNumOrNpi="";//We used to send ProvNum in LicensedPrescriber.ID to NewCrop, but now we send NPI. We will receive ProvNum for older prescriptions.
				string drugInfo="";
				string externalDrugConcept="";
				foreach(XmlNode nodeRxFieldParent in nodeTable.ChildNodes) {
					XmlNode nodeRxField=nodeRxFieldParent.FirstChild;
					if(nodeRxField==null) {
						continue;
					}
					switch(nodeRxFieldParent.Name.ToLower()) {
						case "deaclasscode":
							//According to Brian from NewCrop:
							//"Possible values are 0 = unscheduled, schedules 1-5, and 9 = unknown.
							//Some states categorize a drug as scheduled, but do not assign a particular level."
							rx.IsControlled=false;
							if(nodeRxField.Value!="0") {
								rx.IsControlled=true;
							}
							break;
						case "dispense"://ex 5.555
							rx.Disp=nodeRxField.Value;
							break;
						case "druginfo"://ex lisinopril 5 mg Tab
							drugInfo=nodeRxField.Value;
							break;
						case "drugname"://ex lisinopril
							strDrugName=nodeRxField.Value;
							break;
						case "externaldrugconcept":
							externalDrugConcept=nodeRxField.Value;//ex "ingredient1, ingredient 2"
							break;
						case "externalpatientid"://patnum passed back from the compose request that initiated this prescription
							rx.PatNum=PIn.Long(nodeRxField.Value);
							break;
						case "externalphysicianid"://NPI passed back from the compose request that initiated this prescription.  For older prescriptions, this will be ProvNum.
							strProvNumOrNpi=nodeRxField.Value;
							break;
						case "externaluserid"://The person who ordered the prescription. Is a ProvNum when provider, or an EmployeeNum when an employee. If EmployeeNum, then is prepended with "emp" because of how we sent it to NewCrop in the first place.
							if(nodeRxField.Value.StartsWith("emp")) {
								isProv=false;
							}
							break;
						case "finaldestinationtype":
							//According to Brian from NewCrop:
							//FinalDestinationType - Indicates the transmission method from NewCrop to the receiving entity.
							//0=Not Transmitted
							//1=Print
							//2=Fax
							//3=Electronic/Surescripts Retail
							//4=Electronic/Surescripts Mail Order
							//5=Test
							if(nodeRxField.Value=="0") {//Not Transmitted
								rx.SendStatus=RxSendStatus.Unsent;
							}
							else if(nodeRxField.Value=="1") {//Print
								rx.SendStatus=RxSendStatus.Printed;
							}
							else if(nodeRxField.Value=="2") {//Fax
								rx.SendStatus=RxSendStatus.Faxed;
							}
							else if(nodeRxField.Value=="3") {//Electronic/Surescripts Retail
								rx.SendStatus=RxSendStatus.SentElect;
							}
							else if(nodeRxField.Value=="4") {//Electronic/Surescripts Mail Order
								rx.SendStatus=RxSendStatus.SentElect;
							}
							else if(nodeRxField.Value=="5") {//Test
								rx.SendStatus=RxSendStatus.Unsent;
							}
							break;
						case "genericname":
							strGenericName=nodeRxField.Value;
							break;
						case "patientfriendlysig"://The concat of all the codified fields.
							rx.Sig=nodeRxField.Value;
							break;
						case "pharmacyncpdp"://ex 9998888
							//We will use this information in the future to find a pharmacy already entered into OD, or to create one dynamically if it does not exist.
							//rx.PharmacyNum;//Get the pharmacy where pharmacy.PharmID = node.Value
							break;
						case "prescriptiondate":
							rx.RxDate=PIn.Date(nodeRxField.Value);
							break;
						case "prescriptionguid"://32 characters with 4 hyphens. ex ba4d4a84-af0a-4cbf-9437-36feda97d1b6
							rx.ErxGuid=nodeRxField.Value;
							rxOld=RxPats.GetErxByIdForPat(nodeRxField.Value);
							medOrderOld=MedicationPats.GetMedicationOrderByErxIdAndPat(nodeRxField.Value,_patCur.PatNum);
							break;
						case "prescriptionnotes"://from the Additional Sig box at the bottom
							additionalSig=nodeRxField.Value;
							break;
						case "refills"://ex 1
							rx.Refills=nodeRxField.Value;
							break;
						case "rxcui"://ex 311354
							rxCui=PIn.Long(nodeRxField.Value);//The RxCui is not returned with all prescriptions, so it can be zero (not set).
							break;
						case "pharmacyfullinfo":
							rx.ErxPharmacyInfo=nodeRxField.Value;	
							break;					
					}
				}//end inner foreach
				if(rx.RxDate<rxStartDateT) {//Ignore prescriptions created before version 13.1.14, because those prescriptions were entered manually by the user.
					continue;
				}
				if(additionalSig!="") {
					if(rx.Sig!="") {//If patient friend SIG is present.
						rx.Sig+=" ";
					}
					rx.Sig+=additionalSig;
				}
				rx.Drug=drugInfo;
				if((drugInfo=="" || drugInfo.ToLower()=="none") && externalDrugConcept!="") {
					rx.Drug=externalDrugConcept;
				}
				//Determine the provider. This is a mess, because we used to send ProvNum in the outgoing XML LicensedPrescriber.ID,
				//but now we send NPI to avoid multiple billing charges for two provider records with the same NPI
				//(the same doctor entered multiple times, for example, one provider for each clinic).
				ErxLog erxLog=ErxLogs.GetLatestForPat(rx.PatNum,rx.RxDate);//Locate the original request corresponding to this prescription.
				if(erxLog!=null && erxLog.ProviderId!=0 && erxLog.LastModifiedDate.Date==rx.RxDate.Date) {
					Provider provErxLog=Providers.FirstOrDefault(x => x.Id==erxLog.ProviderId);
					if((strProvNumOrNpi.Length==10 && provErxLog.NationalProviderID==strProvNumOrNpi) || erxLog.ProviderId.ToString()==strProvNumOrNpi) {
						rx.ProvNum=erxLog.ProviderId;
					}
				}
				if(rx.ProvNum==0) {//Not found or the provnum is unknown.
					//The erxLog.ProvNum will be 0 for prescriptions fetched from NewCrop before version 13.3. Could also happen if
					//prescriptions were created when NewCrop was brand new (right before ErxLog was created),
					//or if someone lost a database and they are downloading all the prescriptions from scratch again.
					if(rxOld==null) {//The prescription is being dowloaded for the first time, or is being downloaded again after it was deleted manually by the user.
						List<Provider> listProviders=Providers.GetAll(true);
						for(int j=0;j<listProviders.Count;j++) {//Try to locate a visible provider matching the NPI on the prescription.
							if(strProvNumOrNpi.Length==10 && listProviders[j].NationalProviderID==strProvNumOrNpi) {
								rx.ProvNum=listProviders[j].Id;
								break;
							}
						}
						if(rx.ProvNum==0) {//No visible provider found matching the NPI on the prescription.
							//Try finding a hidden provider matching the NPI on the prescription, or a matching provnum.
							Provider provider=Providers.FirstOrDefault(x => x.NationalProviderID==strProvNumOrNpi);
							if(provider==null) {
								provider=Providers.FirstOrDefault(x => x.Id.ToString()==strProvNumOrNpi);
							}
							if(provider!=null) {
								rx.ProvNum=provider.Id;
							}
						}
						//If rx.ProvNum is still zero, then that means the provider NPI/ProvNum has been modified or somehow deleted (for example, database was lost) for the provider record originally used.
						if(rx.ProvNum==0) {//Catch all
							Provider provUnknown=Providers.FirstOrDefault(x => x.FirstName=="ERX" && x.LastName=="UNKNOWN");
							if(provUnknown!=null) {
								rx.ProvNum=provUnknown.Id;
							}
							if(provUnknown==null) {
								provUnknown=new Provider();
								provUnknown.Abbr="UNK";
								provUnknown.FirstName="ERX";
								provUnknown.LastName="UNKNOWN";
								provUnknown.IsHidden=true;
								rx.ProvNum=Providers.Insert(provUnknown);
								Providers.RefreshCache();
							}
						}
					}
					else {//The prescription has already been downloaded in the past.
						rx.ProvNum=rxOld.ProvNum;//Preserve the provnum if already in the database, because it may have already been corrected by the user after the previous download.
					}
				}
				long medicationPatNum=Erx.InsertOrUpdateErxMedication(rxOld,rx,rxCui,strDrugName,strGenericName,isProv);
				listActiveMedicationPatNums.Add(medicationPatNum);
				if(rxOld==null) {//Only add the rx if it is new.  We don't want to trigger automation for existing prescriptions.
					listNewRx.Add(rx);
				}
			}//end foreach
			List<MedicationPat> listAllMedicationsForPatient=MedicationPats.Refresh(_patCur.PatNum,false);
			foreach(MedicationPat medication in listAllMedicationsForPatient) {
				if(!Erx.IsFromNewCrop(medication.ErxGuid)) {
					continue;//This medication is not an eRx medicaiton.  It was entered manually inside OD.
				}
				if(listActiveMedicationPatNums.Contains(medication.MedicationPatNum)) {
					continue;//The medication is still active.
				}
				//The medication was discontinued inside the eRx interface.
				medication.DateStop=DateTime.Today.AddDays(-1);//Discontinue the medication as of yesterday so that it will immediately show as discontinued.
				MedicationPats.Update(medication,false);//Discontinue the medication inside OD to match what shows in the eRx interface.
			}//end foreach
			if(listNewRx.Count>0) {
				AutomationL.Trigger(AutomationTrigger.RxCreate,new List<string>(),_patCur.PatNum,0,listNewRx);
			}
			return true;
		}

		private bool OrionProcStatDesired(string status2) {
			//We ought to include procs with no status2 in case one slips through the cracks and for testing.
			if(status2==OrionStatus.None.ToString()) {
				return true;
			}
			//Convert the graphical status "os" into a single string status "status2".
			//Not needed because we never translate orion fields to other languages.
			/*
			string status2="";
			if(os==Lans.g("enumStatus2",OrionStatus.TP.ToString())) {
				status2=OrionStatus.TP.ToString();
			}
			 * etc*/
			return false;
		}

		private void PlannedApptPromptHelper() {
			if(_patCur==null || !Preferences.GetBool(PreferenceName.ShowPlannedAppointmentPrompt)) {
				return;
			}
			List<string> listExcludedCodes=CovCats.GetValidCodesForEbenCat(EbenefitCategory.Diagnostic)
				.Union(CovCats.GetValidCodesForEbenCat(EbenefitCategory.DiagnosticXRay))
				.Union(CovCats.GetValidCodesForEbenCat(EbenefitCategory.RoutinePreventive)).ToList();
			List<Procedure> listEligibleProcs=Procedures.RefreshForStatus(_patCur.PatNum,ProcStat.TP)
				.Where(x => !listExcludedCodes.Contains(ProcedureCodes.GetById(x.CodeNum).Code))
				.ToList();
			if(listEligibleProcs.Count==0 || listEligibleProcs.Any(x => x.PlannedAptNum!=0)) {//No eligible procs or already an existing planned appt
				return;
			}
			if(!Procedures.RefreshForStatus(_patCur.PatNum,ProcStat.TP,false).Any(x => x.DateTP==DateTime.Now.Date)) {
				return;//Patient does not have any work that was TP today
			}
			//Make sure patient has no future scheduled non-recall appointment
			List<Appointment> listAppts=Appointments.GetFutureSchedApts(_patCur.PatNum).FindAll(x => x.AptDateTime.Date>DateTime.Now.Date);
			foreach(Appointment apt in listAppts) {
				List<Procedure> listProcsOnAppt=Procedures.GetProcsForSingle(apt.AptNum,false);
				if(listProcsOnAppt.Any(x => !listExcludedCodes.Contains(ProcedureCodes.GetById(x.CodeNum).Code))) {
					return;//Patient has a future scheduled appt that is not Diagnostic,Xray,or Preventative
				}
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Create Planned Appointment with highest priority planned treatment selected?")) {
				return;
			}
			List<Definition> listTreatPlanPriorities=Definitions.GetDefsForCategory(DefinitionCategory.TxPriorities,true);
			List<Procedure> listProcsHighestPriority=listEligibleProcs
				.GroupBy(x => listTreatPlanPriorities.Find(y => y.Id==x.Priority)?.SortOrder??int.MaxValue,x => x)
				.OrderBy(x => x.Key).First()?.ToList();
			int itemOrder=LoadData.TablePlannedAppts.Rows.Count+1;
			List<long> listProcNums=listProcsHighestPriority.Select(x => x.ProcNum).ToList();
			PlannedApptStatus plannedApptResult=AppointmentL.CreatePlannedAppt(_patCur,itemOrder,listProcNums);
			if(plannedApptResult==PlannedApptStatus.FillGridNeeded) {
				FillPlanned();
			}
		}

		/// <summary> Checks ProcStat passed to see if one of the check boxes on the form contains a check for the ps passed. For example if ps is TP and the checkShowTP.Checked is true it will return true.</summary>
		private bool ProcStatDesired(ProcStat ps,bool isLocked) {
			switch(ps) {
				case ProcStat.TP:
					if(checkShowTP.Checked) {
						return true;
					}
					break;
				case ProcStat.C:
					if(checkShowC.Checked) {
						return true;
					}
					break;
				case ProcStat.EC:
					if(checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.EO:
					if(checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.R:
					if(checkShowR.Checked) {
						return true;
					}
					break;
				case ProcStat.D:
					if(checkAudit.Checked || (checkShowC.Checked && isLocked)) {
						return true;
					}
					break;
				case ProcStat.Cn:
					if(checkShowCn.Checked) {
						return true;
					}
					break;
				case ProcStat.TPi:
					if(checkTreatPlans.Checked) {
						return true;
					}
					break;
			}
			//TODO: if proc Date is within show date range; return true;
			return false;
		}

		private void RefreshDoseSpotNotifications() {
			if(_butErx==null || _patCur==null) {
				return;
			}
			Program progErx=Programs.GetCur(ProgramName.eRx);
			if(progErx==null || !progErx.Enabled) {
				return;
			}
			ErxOption erxOption=PIn.Enum<ErxOption>(ProgramProperties.GetPropForProgByDesc(progErx.Id,Erx.PropertyDescs.ErxOption).Value);
			if(erxOption!=ErxOption.DoseSpot && erxOption!=ErxOption.DoseSpotWithLegacy) {
				return;
			}
			//Set the menu items for DoseSpot to visible.
			//Setting the menu items visible before this doesn't matter because this method is the only way to make the menu items valid any ways.
			menuItemDoseSpotPendingPescr.Visible=true;
			menuItemDoseSpotRefillReqs.Visible=true;
			menuItemDoseSpotTransactionErrors.Visible=true;
			ODThread thread=new ODThread((odThread) => {
				long? clinicNum=Clinics.ClinicId;
				if(!Preferences.GetBool(PreferenceName.ElectronicRxClinicUseSelected)) {
					clinicNum=_patCur.ClinicNum;
				}
				string doseSpotClinicID="";
				string doseSpotClinicKey="";
				string doseSpotUserID="";
				int countRefillRequests=0;
				int countErrors=0;
				int countPendingPrescriptions=0;
				try {
					doseSpotUserID=DoseSpot.GetUserID(Security.CurrentUser,clinicNum ?? 0);
					DoseSpot.GetClinicIdAndKey(clinicNum??0,doseSpotUserID,null,null,out doseSpotClinicID,out doseSpotClinicKey);
				}
				catch{
					SetErxButtonNotification(-1,-1,-1,true);
					return;
				}
				//We have valid DoseSpot credentials.  Try to access information from DoseSpot's API.  Catch independently to ensure as much data is gathered as possible.
				try {
					DoseSpot.GetPrescriberNotificationCounts(doseSpotClinicID,doseSpotClinicKey,doseSpotUserID,out countRefillRequests,out countErrors,out countPendingPrescriptions);
					SetErxButtonNotification(countRefillRequests,countErrors,countPendingPrescriptions,false);
				}
				catch (Exception ex) {
					SetErxButtonNotification(-1,-1,-1,true,(ex is ODException odex && odex.ErrorCodeAsEnum==ODException.ErrorCodes.DoseSpotNotAuthorized));
				}
				try {
					//Consent for DoseSpot to share medication history must be renewed every 24 hours. Once we have patient's consent stored in DB, we renew
					//consent each time we refresh notifications.
					if(_patientNoteCur.Consent.HasFlag(PatConsentFlags.ShareMedicationHistoryErx)) {
						DoseSpot.SetMedicationHistConsent(_patCur,clinicNum??0);
					}
				}
				catch {
					SetErxButtonNotification(countRefillRequests,countErrors,countPendingPrescriptions,true);
				}
				try {
					Action<List<RxPat>> onRxAdd=new Action<List<RxPat>>((listRx) => {
						AutomationL.Trigger(AutomationTrigger.RxCreate,new List<string>(),_patCur.PatNum,0,listRx);
					});
					if(DoseSpot.SyncPrescriptionsFromDoseSpot(doseSpotClinicID,doseSpotClinicKey,doseSpotUserID,_patCur.PatNum,onRxAdd)) {
						ModuleSelectedDoseSpot();
					}
				}
				catch {
					SetErxButtonNotification(countRefillRequests,countErrors,countPendingPrescriptions,true);
				}
			});
			thread.Start();
		}

		///<summary>isFullRefresh is ONLY for eCW at this point.</summary>
		private void RefreshModuleData(long patNum,bool isFullRefresh) {
			UpdateTreatmentNote();
			if(patNum==0) {
				LoadData?.ClearData();
				_patCur=null;
				_famCur=null;
				return;
			}
			if(!isFullRefresh) {
				return;
			}
			bool doMakeSecLog=false;
			if(_patNumLast!=patNum) {
				doMakeSecLog=true;
				_patNumLast=patNum;
			}
			try {
				Logger.LogAction("GetAll",() => LoadData=ChartModules.GetAll(patNum,checkAudit.Checked,GetChartModuleComponents(), doMakeSecLog));
			}
			catch(ApplicationException ex) {
				if(ex.Message=="Missing codenum") {
					MessageBox.Show($"Missing codenum. Please run database maintenance method {nameof(DatabaseMaintenances.ProcedurelogCodeNumInvalid)}.");
					_patCur=null;
					LoadData=null;
					return;
				}
				throw;
			}
			_famCur=LoadData.Fam;
			_patCur=LoadData.Pat;
			_listInsSubs=LoadData.ListInsSubs;
			_listInsPlans=LoadData.ListInsPlans;
			//_listSubstitutionLinks not filled here.
			_listPatPlans=LoadData.ListPatPlans;
			_listBenefits=LoadData.ListBenefits;
			_listClaimProcHists=LoadData.ListClaimProcHists;
//todo: track down where this is altered.  Optimize for eCW:
			_patientNoteCur=LoadData.PatNote;

				ODException.SwallowAnyException(() => {
					_patFolder=ImageStore.GetPatientFolder(_patCur, OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath());
				});
			
			_arrayDocuments=LoadData.ArrDocuments;			
			StartXVWebThread();
//todo: might change for planned appt:
			_arrayAppts=LoadData.ArrAppts;
//todo: refresh as needed elsewhere:
			_listToothInitials=LoadData.ListToothInitials;
//todo: optimize for Full mode:
			_arrayPatFields=LoadData.ArrPatFields;
			ChartViews.FillCacheFromTable(LoadData.TableChartViews);
			_listProcButtonQuicks=LoadData.ListProcButtonQuicks;
		}		

		///<summary>Local helper method that retrieves the current layout mode and sheetFieldDef information prior to refreshing the sheet layout.</summary>
		private void ReloadSheetLayout() {
			_sheetLayoutController.ReloadSheetLayout(GetSheetLayoutMode(),GetSheetFieldDefControlDict());
		}

		private void SetChartView(ChartView chartView) {
			_chartViewCurDisplay=chartView;
			labelCustView.Visible=false;
			chartCustViewChanged=false;
			FillProgNotes(isForceFirstPage:true);
			ReloadSheetLayout();//Progress Notes columns may have changed.  Recalculate grid control width.
		}

		///<summary>This does not currently handle custom views.</summary>
		private void SetDateRange() {
			switch(_chartViewCurDisplay.DatesShowing) {
				case ChartViewDates.All:
					_dateTimeShowDateStart=DateTime.MinValue;
					_dateTimeShowDateEnd=DateTime.MinValue;//interpreted as empty.  We want to show all future dates.
					break;
				case ChartViewDates.Today:
					_dateTimeShowDateStart=DateTime.Today;
					_dateTimeShowDateEnd=DateTime.Today;
					break;
				case ChartViewDates.Yesterday:
					_dateTimeShowDateStart=DateTime.Today.AddDays(-1);
					_dateTimeShowDateEnd=DateTime.Today.AddDays(-1);
					break;
				case ChartViewDates.ThisYear:
					_dateTimeShowDateStart=new DateTime(DateTime.Today.Year,1,1);
					_dateTimeShowDateEnd=new DateTime(DateTime.Today.Year,12,31);
					break;
				case ChartViewDates.LastYear:
					_dateTimeShowDateStart=new DateTime(DateTime.Today.Year-1,1,1);
					_dateTimeShowDateEnd=new DateTime(DateTime.Today.Year-1,12,31);
					break;
			}
		}

		///<summary>Currently only used for DoseSpot.</summary>
		private void SetErxButtonNotification(int countRefillRequests,int countErrors,int countPendingPrescriptions,bool isError,bool wasNotAuthorized=false) {
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)delegate () {
					SetErxButtonNotification(countRefillRequests,countErrors,countPendingPrescriptions,isError,wasNotAuthorized);
				});
				return;
			}
			menuItemDoseSpotPendingPescr.Enabled=(!isError);
			menuItemDoseSpotRefillReqs.Enabled=(!isError);
			menuItemDoseSpotTransactionErrors.Enabled=(!isError);
			if(wasNotAuthorized) {
				_butErx.IsRed=false;//Not authorized errors shouldn't make the button red.
			}
			else {
				_butErx.IsRed=isError;//Set the eRx button back to default color.
			}
			_butErx.NotificationText="";
			menuItemDoseSpotPendingPescr.Text="Pending Prescriptions";
			menuItemDoseSpotRefillReqs.Text="Refill Requests";
			menuItemDoseSpotTransactionErrors.Text="Transaction Errors";
			//Has valid counts to display to the user.  There may have been an error, but if we have valid counts we should show them to the user.
			if(countRefillRequests>=0 && countErrors>=0 && countPendingPrescriptions>=0) {
				int numberOfNotifications=Math.Min(99,countRefillRequests+countErrors+countPendingPrescriptions);
				_butErx.NotificationText=(numberOfNotifications==0) ? "" : numberOfNotifications.ToString();
				menuItemDoseSpotPendingPescr.Text+=" ("+countPendingPrescriptions+")";
				menuItemDoseSpotRefillReqs.Text+=" ("+countRefillRequests+")";
				menuItemDoseSpotTransactionErrors.Text+=" ("+countErrors+")";
			}
			ToolBarMain.Invalidate();//Cause the notification text on the eRx button to update as soon as possible.
		}

		///<summary>The supplied procedure row must include these columns: isLocked,ProcDate,ProcStatus,ProcCode,Surf,ToothNum, and ToothRange, all in raw database format.</summary>
		private bool ShouldDisplayProc(DataRow row) {
			//if printing for hospital
			/*
			if(hospitalDate.Year > 1880) {
				if(hospitalDate.Date != PIn.Date(row["ProcDate"].ToString()).Date) {
					return false;
				}
				if(row["ProcStatus"].ToString() != ((int)ProcStat.C).ToString()) {
					return false;
				}
			}*/
			if(checkShowTeeth.Checked) {//Only show selected teeth
				bool showProc=false;
				//ArrayList selectedTeeth = new ArrayList();//integers 1-32
				//for(int i = 0;i < toothChart.SelectedTeeth.Count;i++) {
				//	selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
				//}
				switch(ProcedureCodes.GetProcCode(row["ProcCode"].ToString()).TreatmentArea) {
					case ProcedureTreatmentArea.Arch:
						for(int s=0;s<_toothChartRelay.SelectedTeeth.Count;s++) {
							if(row["Surf"].ToString()=="U" && Tooth.IsMaxillary(_toothChartRelay.SelectedTeeth[s])) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="L" && !Tooth.IsMaxillary(_toothChartRelay.SelectedTeeth[s])) {
								showProc = true;
							}
						}
						break;
					case ProcedureTreatmentArea.Mouth:
					case ProcedureTreatmentArea.None:
					case ProcedureTreatmentArea.Sextant://nobody will miss it
						showProc=false;
						break;
					case ProcedureTreatmentArea.Quad:
						for(int s=0;s<_toothChartRelay.SelectedTeeth.Count;s++) {
							if(row["Surf"].ToString()=="UR" && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])>=1 && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])<=8) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="UL" && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])>=9 && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])<= 16) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="LL" && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])>=17 && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])<=24) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="LR" && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])>=25 && Tooth.ToInt(_toothChartRelay.SelectedTeeth[s])<=32) {
								showProc=true;
							}
						}
						break;
					case ProcedureTreatmentArea.Surface:
					case ProcedureTreatmentArea.Tooth:
						for(int s=0;s<_toothChartRelay.SelectedTeeth.Count;s++) {
							if(row["ToothNum"].ToString()==_toothChartRelay.SelectedTeeth[s]) {
								showProc=true;
							}
						}
						break;
					case ProcedureTreatmentArea.ToothRange:
						string[] range=row["ToothRange"].ToString().Split(',');
						for(int s=0;s<_toothChartRelay.SelectedTeeth.Count;s++) {
							for(int r=0;r<range.Length;r++) {
								if(range[r]==_toothChartRelay.SelectedTeeth[s]) {
									showProc=true;
								}
							}
						}
						break;
				}
				if(!showProc) {
					return false;
				}
			}
			bool isLocked=(row["isLocked"].ToString()=="X");
			if(!ProcStatDesired((ProcStat)PIn.Long(row["ProcStatus"].ToString()),isLocked)) {
				return false;
			}
			if(Programs.IsEnabled(ProgramName.Orion)) {
				if(!OrionProcStatDesired((row["orionStatus2"].ToString()))) {
					return false;
				}
			}
			// Put check for showing hygine in here
			// Put check for showing films in here
			return true;
		}		

		///<summary>Returns true if rowCur represents a valid procedure that should be shown on the tooth chart based on the current track bar date. Returns false if rowCur does not have a valid DateTP, DateEntryC, and ProcDate set. Will also return false if rowCur should not be drawn on the tooth chart (based on the track bar's currently selected date / status of rowCur). If the row needs to be skipped, it gets added to _listProcsSkipped so make sure to manage it correctly before calling this method.</summary>
		private bool ShouldRowShowGraphical(DataRow rowCur,DateTime dateLimit) {
			DateTime dateTP;
			DateTime dateComplete;
			DateTime dateScheduled;
			ProcStat procStatus=(ProcStat)PIn.Int(rowCur["ProcStatus"].ToString());
			if(!DateTime.TryParse(rowCur["DateTP"].ToString(),out dateTP)) {
				return false;
			}
			if(!DateTime.TryParse(rowCur["DateEntryC"].ToString(),out dateComplete)) {
				return false;
			}
			if(!DateTime.TryParse(rowCur["aptDateTime"].ToString(),out dateScheduled)) {
				return false;
			}
			if(dateLimit<dateTP) {//slider date is before the TP date
				_listProcsSkipped.Add(rowCur);
				return false;//Skip the proc
			}
			if(!procStatus.In(ProcStat.C,ProcStat.TP) && dateLimit<dateComplete) {//slider date is before the completion date and the procedure is not C or TP
				_listProcsSkipped.Add(rowCur);
				return false;//Skip the proc
			}
			if(procStatus==ProcStat.C && dateLimit<dateComplete && dateLimit>=dateTP) {//Procedure is C and the slider date is after or equal to the TP date, but before the completion date
				rowCur["ProcStatus"]=POut.Int((int)ProcStat.TP);//Pretend the row is TP for the tooth chart
			}
			else if(procStatus==ProcStat.TP && dateLimit>=dateScheduled.Date //Procedure is TP and the slider date is after or equal to the Scheduled date
				&& dateScheduled.Year>1880 && dateLimit!=DateTimeOD.Today) //If the slider is at today, then we want to show what things are like at the current time.
			{
				rowCur["ProcStatus"]=POut.Int((int)ProcStat.C);//Pretend the row is C for the tooth chart
			}
			return true;
		}

		///<summary>Displays the menu item as enabled if all the selected rows return true for isRowRelevant. Displays the menu item as disabled if at least one but not all return true. Hides the menu item if no rows return true.</summary>
		private void ShowMenuItemHelper(MenuItem menuItem,Func<DataRowWithIdx,bool> isRowRelevant) {
			List<DataRowWithIdx> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => new DataRowWithIdx((DataRow)gridProg.Rows[x].Tag,x)).ToList();
			int countRelevant=listSelectedRows.Count(x => isRowRelevant(x));
			if(countRelevant==0) {
				menuItem.Visible=false;
			}
			else {
				menuItem.Visible=true;
				menuItem.Enabled=listSelectedRows.Count==countRelevant;
			}
		}
		
		private void StartXVWebThread() {
			if(_threadImageRequest!=null) {
				return;
			}
			listViewImages.Scrollable=false;//Setting the scroll bar invisible in order to reduce the amount of blinking that goes on when filling the view.
			long patNum=_patCur.PatNum;
			_threadImageRequest=new ODThread(GetXVWebImages,_patCur.Copy()
				,_arrayDocuments.Where(x => x.ExternalSource==ExternalSourceType.XVWeb).Select(x => x.ExternalGUID).ToList());
			_threadImageRequest.AddExitHandler(o => {
				_threadImageRequest=null;
				this.Invoke(() => listViewImages.Scrollable=true);					
			});
			_threadImageRequest.AddExceptionHandler(e => {
				if(e is ApplicationException) {
					FriendlyException.Show(e.Message,e);
				}
				else {
					FriendlyException.Show("Unable to download images from XVWeb.",e);
				}
			});
			_threadImageRequest.Name="XVWebImageDownload";
			_threadImageRequest.Start(true);
		}

		///<summary>Mimics old ChartLayoutHelper logic. Called in various places when we want to ensure that checkTreatPlans is shown when it needs to.</summary>
		private void ToggleCheckTreatPlans() {
			listButtonCats.BringToFront();
			if(IsTPChartingAvailable) {
				//adjust listBtnCats height so it will be 1 pixel above checkTPs Y pos
				listButtonCats.Height=checkTreatPlans.Location.Y-listButtonCats.Location.Y-1;
			}
			else {
				checkTreatPlans.Checked=false;//TP charting not avaliable, make sure TP sheet layout is not selected.
				//set listBtnCats height so it will be 2 pixels below checkTPs Y pos + checkTPs height
				listButtonCats.Height=checkTreatPlans.Location.Y+checkTreatPlans.Height+2-listButtonCats.Location.Y;
			}
		}

		private void Tool_Anesthesia_Click() {
			/*
			AnestheticData AnestheticDataCur;
			AnestheticDataCur = new AnestheticData();
			FormAnestheticRecord FormAR = new FormAnestheticRecord(PatCur, AnestheticDataCur);
			FormAR.ShowDialog();

			PatCur = Patients.GetPat(Convert.ToInt32(PatCur.PatNum));
			OnPatientSelected(Convert.ToInt32(PatCur.PatNum), Convert.ToString(PatCur), true, Convert.ToString(PatCur));
			FillPtInfo();
			return;*/
		}

		///<summary>Only used for eCW tight.  Everyone else has the commlog button up in the main toolbar.</summary>
		private void Tool_Commlog_Click() {
			Commlog commlogCur=new Commlog();
			commlogCur.PatNum=_patCur.PatNum;
			commlogCur.CommDateTime=DateTime.Now;
			commlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			commlogCur.Mode_=CommItemMode.Phone;
			commlogCur.SentOrReceived=CommSentOrReceived.Received;
			commlogCur.UserNum=Security.CurrentUser.Id;
			commlogCur.IsNew=true;
			FormCommItem formCommItem=new FormCommItem(commlogCur);
			if(formCommItem.ShowDialog()==DialogResult.OK) {
				ModuleSelected(_patCur.PatNum);
			}
		}

		private void Tool_Consent_Click() {
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			List<SheetDef> listSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.Consent);
			if(listSheetDefs.Count>0) {
				MessageBox.Show("Please use dropdown list.");
				return;
			}
			SheetDef sheetDef=SheetsInternal.GetSheetDef(SheetInternalType.Consent);
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,_patCur.PatNum);
			SheetParameter.SetParameter(sheet,"PatNum",_patCur.PatNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			FormSheetFillEdit.ShowForm(sheet,FormSheetFillEdit_FormClosing);
		}

		private void Tool_EHR_Click(bool onLoadShowOrders) {
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			//Quarterly key check was removed from here so that any customer can use EHR tools
			//But we require a EHR subscription for them to obtain their MU reports.
			if(Providers.GetById(_patCur.PriProv)==null) {
				MessageBox.Show("Please set the patient's primary provider first.");
				return;
			}
			FormEHR formEHR=new FormEHR();
			formEHR.PatNum=_patCur.PatNum;
			formEHR.PatNotCur=_patientNoteCur;
			formEHR.PatFamCur=_famCur;
			formEHR.ShowDialog();
			if(formEHR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(formEHR.ResultOnClosing==EhrFormResult.PatientSelect) {
				FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(formEHR.PatNum),false);
				ModuleSelected(formEHR.PatNum);
			}
		}

		private void Tool_eRx_Click(bool isShowRefillsAndErrors=false) {
			if(!Security.IsAuthorized(Permissions.RxCreate)) {
				return;
			}
			if(_dictFormErxSessions.ContainsKey(_patCur.PatNum) && _dictFormErxSessions[_patCur.PatNum]!=null) {
				_dictFormErxSessions[_patCur.PatNum].Restore();
				_dictFormErxSessions[_patCur.PatNum].BringToFront();
				return;//FormErx is already open for this patient.  Simply bring it to the front to make the user aware that it is still there.
			}
			Program programErx=Programs.GetCur(ProgramName.eRx);
			ProgramProperty ppErxOption=ProgramProperties.GetPropForProgByDesc(programErx.Id,Erx.PropertyDescs.ErxOption);
			ErxOption erxOption=PIn.Enum<ErxOption>(ppErxOption.Value);
			string doseSpotClinicID="";
			string doseSpotClinicKey="";
			string doseSpotUserID="";
			bool isEmp=Erx.IsUserAnEmployee(Security.CurrentUser);
			Provider prov=null;
			if(!isEmp && Security.CurrentUser.ProviderId.HasValue) {
				prov=Providers.GetById(Security.CurrentUser.ProviderId.Value);
			}
			else {
				prov=Providers.GetById(_patCur.PriProv);
			}
			if(erxOption==ErxOption.DoseSpotWithLegacy) {
				if(prov.IsErxEnabled==ErxEnabledStatus.EnabledWithLegacy) {
					InputBox pickErxOption=
						new InputBox("Which eRx option would you like to use?",new List<string>() { "Legacy","DoseSpot"},false);
					if(pickErxOption.ShowDialog()==DialogResult.Cancel) {
						return;
					}
					if(pickErxOption.SelectedIndex==0) {//Legacy
						erxOption=ErxOption.Legacy;
					}
					else {
						erxOption=ErxOption.DoseSpot;
					}
				}
				else {//It's fine that the provider might not be enabled.  We will check it later and they will be blocked.
					erxOption=ErxOption.DoseSpot;
				}
			}
			#region Provider Term Date Check
			//Prevents prescriptions from being added that have a provider selected that is past their term date
			string message="";
			List<long> listInvalidProvs=Providers.GetInvalidProvsByTermDate(new List<long> { prov.Id },DateTime.Now);
			if(listInvalidProvs.Count>0) {
				if(!isEmp && Security.CurrentUser.ProviderId!=0) {
					message="The provider attached to this user has a Term Date that has expired. "
						+"Please select another user or change the provider's term date.";
				}
				else {
					message="The primary provider for this patient has a Term Date that has expired. "
						+"Please change the primary provider for this patient or change the provider's term date.";
				}
				MessageBox.Show(message);
				return;
			}
			#endregion Provider Term Date Check
			if(erxOption==ErxOption.Legacy) {
				string newCropAccountId=Preferences.GetString(PreferenceName.NewCropAccountId);
				if(newCropAccountId=="") {//NewCrop has not been enabled yet.
					if(!MsgBox.Show(MsgBoxButtons.YesNo,"Continuing will enable basic Electronic Rx (eRx).  Fees are associated with this secure e-prescribing system.  See our online manual for details.  At this time, eRx only works for the United States and its territories, including Puerto Rico.  Continue?")) {
						return;
					}
					//prepare the xml document to send--------------------------------------------------------------------------------------
					XmlWriterSettings xmlWSettings=new XmlWriterSettings();
					xmlWSettings.Indent=true;
					xmlWSettings.IndentChars=("    ");
					StringBuilder strBuild=new StringBuilder();
					using(XmlWriter xmlWriter=XmlWriter.Create(strBuild,xmlWSettings)) {
						xmlWriter.WriteStartElement("CustomerIdRequest");
						xmlWriter.WriteStartElement("RegistrationKey");
						xmlWriter.WriteString(Preferences.GetString(PreferenceName.RegistrationKey));
						xmlWriter.WriteEndElement();
						xmlWriter.WriteEndElement();
					}
#if DEBUG
					Imedisoft.localhost.Service1 updateService=new Imedisoft.localhost.Service1();
#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
					updateService.Url=Prefs.GetString(PrefName.UpdateServerAddress);
#endif
					if(Preferences.GetString(PreferenceName.UpdateWebProxyAddress)!="") {
						IWebProxy proxy=new WebProxy(Preferences.GetString(PreferenceName.UpdateWebProxyAddress));
						ICredentials cred=new NetworkCredential(Preferences.GetString(PreferenceName.UpdateWebProxyUserName),Preferences.GetString(PreferenceName.UpdateWebProxyPassword));
						proxy.Credentials=cred;
						updateService.Proxy=proxy;
					}
					string patNum="";
					try {
						string result=updateService.RequestCustomerID(strBuild.ToString());//may throw error
						XmlDocument xmlDoc=new XmlDocument();
						xmlDoc.LoadXml(result);
						XmlNode node=xmlDoc.SelectSingleNode("//CustomerIdResponse");
						if(node!=null) {
							patNum=node.InnerText;
						}
						if(patNum=="") {
							throw new ApplicationException("Failed to validate registration key.");
						}
						newCropAccountId=patNum;
						newCropAccountId+="-"+CodeBase.MiscUtils.CreateRandomAlphaNumericString(3);
						long checkSum=PIn.Long(patNum);
						checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+1])*3;
						checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+2])*5;
						checkSum+=Convert.ToByte(newCropAccountId[newCropAccountId.IndexOf('-')+3])*7;
						newCropAccountId+=(checkSum%100).ToString().PadLeft(2,'0');
						Preferences.Set(PreferenceName.NewCropAccountId,newCropAccountId);
						programErx.Enabled=true;
						Programs.Update(programErx);
					}
					catch(Exception ex) {
						MessageBox.Show(ex.Message);
						return;
					}
				}
				else { //newCropAccountId!=""
					if(!programErx.Enabled) {
						MessageBox.Show("eRx is currently disabled."+"\r\n"+"To enable, see our online manual for instructions.");
						return;
					}
					if(!NewCropIsAccountIdValid()) {
						string newCropName=Preferences.GetString(PreferenceName.NewCropName);
						string newCropPassword=Preferences.GetString(PreferenceName.NewCropPassword);
						if(newCropName=="" || newCropPassword=="") { //NewCrop does not allow blank passwords.
							MessageBox.Show("NewCropName preference and NewCropPassword preference must not be blank when using a NewCrop AccountID provided by a reseller.");
							return;
						}
					}
				}
			}
			else if(erxOption==ErxOption.DoseSpot) {
				if(!programErx.Enabled) {
					MessageBox.Show("eRx is currently disabled."+"\r\n"+"To enable, see our online manual for instructions.");
					return;
				}
				if(Security.CurrentUser.EmployeeId==0 && Security.CurrentUser.ProviderId==0) {
					MessageBox.Show("This user must be associated with either a provider or an employee.  The security admin must make this change before this user can submit prescriptions.");
					return;
				}
				//clinicNum should be 0 for offices not using clinics.
				//This will work properly when retreiving the clinicKey and clinicID

				long? clinicNum=Clinics.ClinicId;
				if (!Preferences.GetBool(PreferenceName.ElectronicRxClinicUseSelected))
				{
					clinicNum = _patCur.ClinicNum;
				}
				
				List<ProgramProperty> listDoseSpotProperties=ProgramProperties.GetForProgram(programErx.Id)
					.FindAll(x => x.ClinicId==clinicNum 
						&& (x.Description==Erx.PropertyDescs.ClinicID || x.Description==Erx.PropertyDescs.ClinicKey));
				byte[] arrayPostData=new byte[1];
				string queryString="";
				bool isDoseSpotAccessAllowed=true;
				try {
					doseSpotUserID=DoseSpot.GetUserID(Security.CurrentUser,clinicNum??0);
					DoseSpot.GetClinicIdAndKey(clinicNum??0,doseSpotUserID,programErx,listDoseSpotProperties,out doseSpotClinicID,out doseSpotClinicKey);
					//BuildDoseSpotPostDataBytes will validate patient information and throw exceptions.
					OIDExternal oIdExternal=DoseSpot.GetDoseSpotPatID(_patCur.PatNum);
					if(oIdExternal==null) {
						DoseSpot.ValidatePatientData(_patCur);
						string token=DoseSpotREST.GetToken(doseSpotUserID,doseSpotClinicID,doseSpotClinicKey);
						DoseSpot.CreateOIDForPatient(PIn.Int(DoseSpotREST.AddPatient(token,_patCur)),_patCur.PatNum);
					}
					if(isShowRefillsAndErrors) {
						arrayPostData=ErxXml.BuildDoseSpotPostDataBytesRefillsErrors(doseSpotClinicID,doseSpotClinicKey,doseSpotUserID,out queryString);
					}
					else {
						string onBehalfOfUserId="";
						if(isEmp) {
							List<Provider> listProviders=Providers.GetProvsScheduledToday(clinicNum??0);
							if(!listProviders.Any(x => x.Id==prov.Id)) {
								listProviders.Add(prov);
							}
							FormProviderPick formProviderPick=new FormProviderPick(listProviders);
							formProviderPick.SelectedProviderId=prov.Id;
							formProviderPick.IsNoneAvailable=false;
							formProviderPick.IsShowAllAvailable=true;
							formProviderPick.ShowDialog();
							if(formProviderPick.DialogResult==DialogResult.Cancel) {
								return;
							}
							List<User> listDoseUsers=Users.Find(x => !x.IsHidden && x.ProviderId==formProviderPick.SelectedProviderId);//Only consider non-hidden users.
							listDoseUsers=listDoseUsers.FindAll(x => {//Finds users that have a DoseSpot ID
								try {
									return !string.IsNullOrWhiteSpace(DoseSpot.GetUserID(x,clinicNum??0));
								}
								catch(Exception) {
									return false;
								}
							});
							User userOnBehalfOf=null;
							if(listDoseUsers.Count==1) {
								userOnBehalfOf=listDoseUsers[0];
							}
							else if(listDoseUsers.Count==0) {
								throw new ODException("Could not find DoseSpot User ID for the selected provider.");
							}
							else {
								throw new ODException("There are too many Open Dental users associated to the selected provider.");
							}
							prov=Providers.GetById(formProviderPick.SelectedProviderId);
							#region Provider Term Date Check
							//Prevents prescriptions from being added that have a provider selected that is past their term date
							listInvalidProvs=Providers.GetInvalidProvsByTermDate(new List<long> { prov.Id },DateTime.Now);
							if(listInvalidProvs.Count>0) {
								message="The provider selected has a Term Date that has expired. Please select another provider.";
								MessageBox.Show(message);
								return;
							}
							#endregion Provider Term Date Check
							onBehalfOfUserId=(DoseSpot.GetUserID(userOnBehalfOf,clinicNum??0));
						}
						arrayPostData=ErxXml.BuildDoseSpotPostDataBytes(doseSpotClinicID,doseSpotClinicKey,doseSpotUserID,onBehalfOfUserId,_patCur,out queryString);
					}
					//Running this block in debug won't work.
					// TODO: This how do we test this?
#if !DEBUG
					if(!isEmp && Security.CurUser.ProvNum!=0) {//Not a proxy clinician, so we want to validate that they are allowed access.
							DoseSpot.ValidateProvider(prov,clinicNum);
							//hook for additional authorization before prescription is saved
							bool[] arrayAuthorized=new bool[1] { false };
							if(Plugins.HookMethod(this,"ContrChart.Tool_eRx_Click_Authorize",arrayAuthorized,prov)) {
								if(!arrayAuthorized[0]) {
									isDoseSpotAccessAllowed=false;
								}
							}
							string provNpi=Regex.Replace(prov.NationalProvID,"[^0-9]*","");//NPI with all non-numeric characters removed.
							UpdateErxAccess(provNpi,doseSpotUserID,clinicNum,doseSpotClinicID,doseSpotClinicKey,erxOption);
							ProviderErx provErxDoseSpot=ProviderErxs.GetOneForNpiAndOption(provNpi,erxOption);
							if(provErxDoseSpot.IsEnabled!=ErxStatus.Enabled) {
								MessageBox.Show("Contact support to enable eRx for provider"+" "+prov.Abbr);
								isDoseSpotAccessAllowed=false;
							}
						}
						else {
							//Proxy users still need to have their clinic synced with ODHQ.
							//This call mimics what would happen in UpdateErxAccess above
							DoseSpot.SyncClinicErxsWithHQ();
						}
						ClinicErx clinicErxCur=ClinicErxs.GetByClinicIdAndKey(doseSpotClinicID,doseSpotClinicKey);
						if(clinicErxCur.EnabledStatus!=ErxStatus.Enabled) {
							string clinicAbbr="";
							if(clinicErxCur.ClinicNum==-1) {//ClinicErx was inserted from ODHQ, use the ClinicDesc given by an ODHQ staff
								clinicAbbr=clinicErxCur.ClinicDesc;
							}
							else if(clinicErxCur.ClinicNum==0) {//Office Headquarters
								clinicAbbr="Headquarters";
							}
							else {
								clinicAbbr=Clinics.GetAbbr(clinicErxCur.ClinicNum);
							}
							MessageBox.Show("Contact support to enable eRx for clinic"+" "+clinicAbbr);
							isDoseSpotAccessAllowed=false;
						}
#endif
					//Try to add any self reported medications to DoseSpot before the user gets views their list.
					DoseSpot.SyncPrescriptionsToDoseSpot(doseSpotClinicID,doseSpotClinicKey,doseSpotUserID,_patCur.PatNum);
				}
				catch(ODException odException) {
					MessageBox.Show(odException.Message);//The ODExceptions thrown in this context have already been translated.
					return;
				}
				catch(Exception ex) {
					MessageBox.Show("Error: "+ex.Message);
					return;
				}
				if(isDoseSpotAccessAllowed) {
					//The user is either a provider with granted access, or a proxy clinician
					FormErx formErx=new FormErx(false);
					formErx.PatCur=_patCur;
					formErx.PostDataBytes=arrayPostData;
					formErx.ErxOptionCur=erxOption;
					formErx.Show();//Non-modal so user can browse OD while writing prescription.  When form is closed, ErxBrowserClosed() is called below.
					_dictFormErxSessions[_patCur.PatNum]=formErx;
				}
				ErxLog erxDoseSpotLog=new ErxLog();
				erxDoseSpotLog.PatientId=_patCur.PatNum;
				erxDoseSpotLog.MsgText=queryString;
				erxDoseSpotLog.ProviderId=prov.Id;
				erxDoseSpotLog.UserId=Security.CurrentUser.Id;
				SecurityLogs.MakeLogEntry(Permissions.RxCreate,erxDoseSpotLog.PatientId,"eRx DoseSpot entry made for provider"+" "+Providers.GetAbbr(erxDoseSpotLog.ProviderId));
				ErxLogs.Insert(erxDoseSpotLog);
				return;
			}
			//Validation------------------------------------------------------------------------------------------------------------------------------------------------------
			if(Security.CurrentUser.EmployeeId==0 && Security.CurrentUser.ProviderId==0) {
				MessageBox.Show("This user must be associated with either a provider or an employee.  The security admin must make this change before this user can submit prescriptions.");
				return;
			}
			if(_patCur==null) {
				MessageBox.Show("No patient selected.");
				return;
			}
			Employee emp=null;
			Clinic clinic=null;
			try
			{
				Erx.ValidatePracticeInfo();
				//Clinic Validation

				if (Preferences.GetBool(PreferenceName.ElectronicRxClinicUseSelected))
				{
					clinic = Clinics.Active;
				}
				else if (_patCur.ClinicNum != 0)
				{//Use patient default clinic if the patient has one.
					clinic = Clinics.GetById(_patCur.ClinicNum);
				}
				if (clinic != null)
				{
					Erx.ValidateClinic(clinic);
				}

				if (isEmp)
				{
					emp = Employees.GetEmp(Security.CurrentUser.EmployeeId.Value);
					if (emp.LastName == "")
					{//Checked in UI, but check here just in case this database was converted from another software.
						MessageBox.Show("Employee last name missing for user: " + Security.CurrentUser.UserName);
						return;
					}

					if (emp.FirstName == "")
					{//Not validated in UI.
						MessageBox.Show("Employee first name missing for user: " + Security.CurrentUser.UserName);
						return;
					}
				}
				Erx.ValidateProv(prov, clinic);
				//hook for additional authorization before prescription is saved
				bool[] arrayAuthorized = new bool[1] { false };
				if (Plugins.HookMethod(this, "ContrChart.Tool_eRx_Click_Authorize2", arrayAuthorized, prov))
				{
					if (!arrayAuthorized[0])
					{
						throw new ODException("Provider is not authenticated");
					}
				}
				Erx.ValidatePat(_patCur);
			}
			catch (ODException ex)
			{//Purposefully only catch exceptions we throw due to validation
				MessageBox.Show(ex.Message);//All ODExceptions thrown in this context should have already been translated.
				return;
			}
#region ProviderErx Validation
			string npi=Regex.Replace(prov.NationalProviderID,"[^0-9]*","");//NPI with all non-numeric characters removed.
			bool isAccessAllowed=true;
			UpdateErxAccess(npi,"",0,"","",erxOption);//0/blank/blank for clinicNum/clinicid/clinickey is fine because we don't enable/disable the clinic for NewCrop.
			ProviderErx provErx=ProviderErxs.GetOneForNpiAndOption(npi,erxOption);
			if(!Preferences.GetBool(PreferenceName.NewCropIsLegacy) && !provErx.IsIdentifyProofed) {
				if(Preferences.GetString(PreferenceName.NewCropPartnerName)!="" || Preferences.GetString(PreferenceName.NewCropPassword)!="") {//Customer of a distributor
					MessageBox.Show("Provider"+" "+prov.Abbr+" "
						+"must complete Identity Proofing (IDP) before using eRx.  Call support for details.");
				}
				else {//Customer of OD proper or customer of a reseller
					MessageBox.Show("Provider"+" "+prov.Abbr+" "+"must complete Identity Proofing (IDP) before using eRx.  "
						+"Please call support to schedule an IDP appointment.");
				}
				isAccessAllowed=false;
			}
			if(provErx.IsEnabled!=ErxStatus.Enabled) {
				MessageBox.Show("Contact support to enable eRx for provider"+" "+prov.Abbr);
				isAccessAllowed=false;
			}
#endregion ProviderErx Validation
			string clickThroughXml="";
			byte[] arrayPostDataBytes=ErxXml.BuildNewCropPostDataBytes(prov,emp,_patCur,out clickThroughXml);
#region Launch eRx in external browser window.
//			string xmlBase64=System.Web.HttpUtility.HtmlEncode(Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(clickThroughXml)));
//			xmlBase64=xmlBase64.Replace("+","%2B");//A common base 64 character which needs to be escaped within URLs.
//			xmlBase64=xmlBase64.Replace("/","%2F");//A common base 64 character which needs to be escaped within URLs.
//			xmlBase64=xmlBase64.Replace("=","%3D");//Base 64 strings usually end in '=', which could mean a new parameter definition within the URL so we escape.
//			String postdata="RxInput=base64:"+xmlBase64;
//			byte[] PostDataBytes=System.Text.Encoding.UTF8.GetBytes(postdata);
//			string additionalHeaders="Content-Type: application/x-www-form-urlencoded\r\n";
//			IWebBrowserApp IE=(IWebBrowserApp)IEControl;
//			IE.Visible=true;
//#if DEBUG
//			string newCropUrl="http://preproduction.newcropaccounts.com/interfaceV7/rxentry.aspx";
//#else //Debug
//			string newCropUrl="https://secure.newcropaccounts.com/interfacev7/rxentry.aspx";
//#endif
//			IE.Navigate(newCropUrl,null,null,PostDataBytes,additionalHeaders);
#endregion Launch eRx in external browser window.
			try {
				//Enforce Latest IE Version Available.
				if(MiscUtils.TryUpdateIeEmulation()) {
					MessageBox.Show("Browser emulation version updated.\r\nYou must restart this application before using eRx.");
					return;
				}
				FormErx formErx=new FormErx();
				formErx.PatCur=_patCur;
				formErx.PostDataBytes=arrayPostDataBytes;
				formErx.ErxOptionCur=erxOption;
				if(isAccessAllowed) {
					formErx.Show();//Non-modal so user can browse OD while writing prescription.  When form is closed, ErxBrowserClosed() is called below.
					_dictFormErxSessions[_patCur.PatNum]=formErx;
				}
				else {
					//This is how we send the provider information to NewCrop without allowing the provider to use NewCrop.
					//NewCrop requires the provider information on their server in order to complete Identity Proofing (IDP).
					formErx.ComposeNewRxLegacy();
				}
			}
			catch(Exception ex) {
				MessageBox.Show("Error launching browser window.  Internet Explorer might not be installed.  "+ex.Message);
				return;
			}
			ErxLog erxLog=new ErxLog();
			erxLog.PatientId=_patCur.PatNum;
			erxLog.MsgText=clickThroughXml;
			erxLog.ProviderId=prov.Id;
			erxLog.UserId=Security.CurrentUser.Id;
			SecurityLogs.MakeLogEntry(Permissions.RxCreate,erxLog.PatientId,"eRx entry made for provider"+" "+Providers.GetAbbr(erxLog.ProviderId));
			ErxLogs.Insert(erxLog);
		}

		private void Tool_ExamSheet_Click() {
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			FormExamSheets formExamSheets=new FormExamSheets();
			formExamSheets.PatNum=_patCur.PatNum;
			formExamSheets.FormClosing+=FormExamSheets_FormClosing;
			formExamSheets.Show();
		}

		private void Tool_HL7_Click()
		{
			DataRow row;
			if (gridProg.SelectedIndices.Length == 0)
			{
				//autoselect procedures
				for (int i = 0; i < gridProg.Rows.Count; i++)
				{//loop through every line showing in progress notes
					row = (DataRow)gridProg.Rows[i].Tag;
					if (row["ProcNum"].ToString() == "0")
					{
						continue;//ignore non-procedures
					}
					//May want to ignore procs with zero fee?
					//if((decimal)row["chargesDouble"]==0) {
					//  continue;//ignore zero fee procedures, but user can explicitly select them
					//}
					if (PIn.Date(row["ProcDate"].ToString()) == DateTime.Today && PIn.Int(row["ProcStatus"].ToString()) == (int)ProcStat.C)
					{
						gridProg.SetSelected(i, true);
					}
				}
				if (gridProg.SelectedIndices.Length == 0)
				{//if still none selected
					MessageBox.Show("Please select procedures first.");
					return;
				}
			}
			List<Procedure> listProcs = new List<Procedure>();
			bool allAreProcedures = true;
			for (int i = 0; i < gridProg.SelectedIndices.Length; i++)
			{
				row = (DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if (row["ProcNum"].ToString() == "0")
				{
					allAreProcedures = false;
				}
				else
				{
					listProcs.Add(Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()), false));
				}
			}
			if (!allAreProcedures)
			{
				MessageBox.Show("You can only select procedures.");
				return;
			}
			long aptNum = 0;
			for (int i = 0; i < listProcs.Count; i++)
			{
				if (listProcs[i].AptNum == 0)
				{
					continue;
				}
				aptNum = listProcs[i].AptNum;
				break;
			}
			if (HL7Defs.GetOneDeepEnabled().IsProcApptEnforced && listProcs.Any(x => x.AptNum == 0))
			{
				if (!MsgBox.Show(MsgBoxButtons.YesNo, "At least one of these procedures is not attached to an appointment. Send anyway?"))
				{
					return;
				}
			}
			//todo: compare with: Bridges.ECW.AptNum, no need to generate PDF segment, pdfs only with eCW and this button not available with eCW integration
			MessageHL7 messageHL7 = MessageConstructor.GenerateDFT(listProcs, EventTypeHL7.P03, _patCur, _famCur.ListPats[0], aptNum, "treatment", "PDF Segment");
			if (messageHL7 == null)
			{
				MessageBox.Show("There is no DFT message type defined for the enabled HL7 definition.");
				return;
			}
			HL7Msg hl7Msg = new HL7Msg();
			hl7Msg.AptNum = aptNum;
			hl7Msg.HL7Status = HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
			hl7Msg.MsgText = messageHL7.ToString();
			hl7Msg.PatNum = _patCur.PatNum;
			HL7ProcAttach hl7ProcAttach = new HL7ProcAttach();
			hl7ProcAttach.HL7MsgNum = HL7Msgs.Insert(hl7Msg);
			foreach (Procedure proc in listProcs)
			{
				hl7ProcAttach.ProcNum = proc.ProcNum;
				HL7ProcAttaches.Insert(hl7ProcAttach);
			}

#if DEBUG
			MessageBox.Show(messageHL7.ToString());
#endif

			MessageBox.Show(listProcs.Count + " " + (listProcs.Count == 1 ? "procedure" : "procedures")
					+ " " + "queued to be sent by the HL7 service.");

		}

		private void Tool_LabCase_Click() {
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			LabCase labCase=new LabCase();
			labCase.PatNum=_patCur.PatNum;
			labCase.ProvNum=Patients.GetProvNum(_patCur);
			labCase.DateTimeCreated=MiscData.GetNowDateTime();
			LabCases.Insert(labCase);//it will be deleted inside the form if user clicks cancel.
			//We need the primary key in order to attach lab slip.
			FormLabCaseEdit formLabCaseEdit=new FormLabCaseEdit();
			formLabCaseEdit.CaseCur=labCase;
			formLabCaseEdit.IsNew=true;
			formLabCaseEdit.ShowDialog();
			//needs to always refresh due to complex ok/cancel
			ModuleSelected(_patCur.PatNum);
		}

		private void Tool_Layout_Click() {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormSheetDefs formSheetDefs=new FormSheetDefs(SheetTypeEnum.ChartModule);
			formSheetDefs.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sheets");
			RefreshModuleScreen(false);//Update UI to reflect any changed dynamic SheetDefs.
			ReloadSheetLayout();//Could have added or deleted layouts, refresh list.
		}

		private void Tool_MedLab_Click() {
			FormMedLabs formMedLabs=new FormMedLabs();
			formMedLabs.PatCur=_patCur;
			formMedLabs.Show();
		}

		private void Tool_Perio_Click() {
			if(_patCur==null || LoadData==null || LoadData.TableProgNotes==null) {
				MessageBox.Show("Please select a patient and try again.");
				return;
			}
			List<Procedure> listProcedures=new List<Procedure>();
			DataTable table=LoadData.TableProgNotes;
			//Find rows which are procedures (ProcNum!=0) and use the CodeNum and ToothNum columns to create a list of pseudo "Procedures".
			//We pull the procedures from the ProgNotes in memory so that we do not have to run a query to get the procedure data.
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["ProcNum"].ToString()=="0") {//jsalmon - this might need to be enhanced to consider proc status?
					continue;//Not a procedure row.
				}
				Procedure procTemp=new Procedure();
				procTemp.ToothNum=PIn.String(table.Rows[i]["ToothNum"].ToString());
				procTemp.CodeNum=PIn.Long(table.Rows[i]["CodeNum"].ToString());
				listProcedures.Add(procTemp);
			}

				FormPerio formPerio=new FormPerio(_patCur,listProcedures);
				formPerio.ShowDialog();
			
		}

		private void Tool_Ortho_Click() {
			if(_patCur==null) {
				MessageBox.Show("Please select a patient.");
				return;
			}
			//We store the current patNum because previously we've seen PatCur become null prior to ModuleSelected(...) being called somehow.
			long curPatNum=_patCur.PatNum;
			FormOrthoChart formOrthoChart=new FormOrthoChart(_patCur);
			formOrthoChart.ShowDialog();
			ModuleSelected(curPatNum);
		}

		private void Tool_Rx_Click()
		{
			//This code is a copy of FormRxManage.butRxNew_Click().  Any changes to this code need to be changed there too.
			if (!Security.IsAuthorized(Permissions.RxCreate))
			{
				return;
			}

			FormRxSelect formRxSelect = new FormRxSelect(_patCur);
			formRxSelect.ShowDialog();
			if (formRxSelect.DialogResult != DialogResult.OK) return;
			ModuleSelected(_patCur.PatNum);
			SecurityLogs.MakeLogEntry(Permissions.RxCreate, _patCur.PatNum, "Created prescription.");
		}
		
		private void Tool_ToothChart_Click() {
			if(Programs.UsingOrion) {
				menuItemChartSave_Click(this,new EventArgs());
				return;
			}
			MessageBox.Show("Please use dropdown list.");
			return;
		}

		///<summary>Returns an error message upon error.  Otherwise returns empty string.</summary>
		private void UpdateErxAccess(string npi,string userId,long clinicNum,string clinicId,string clinicKey,ErxOption erxOption) {
			ProviderErx provErxCur=ProviderErxs.GetOneForNpiAndOption(npi,erxOption);
			if(provErxCur==null) {
				//The provider is not yet part of the providererx table.  This extra refresh will only happen one time for each new provider.
				//First refresh cache to verify the provider was not added within the last signal interval.  Prevents duplicates for long signal intervals.
				ProviderErxs.RefreshCache();
				provErxCur=ProviderErxs.GetOneForNpiAndOption(npi,erxOption);
			}
			if(provErxCur==null) {
				provErxCur=new ProviderErx();
				provErxCur.PatNum=0;
				provErxCur.NationalProviderID=npi;
				if(erxOption==ErxOption.Legacy) {
					provErxCur.IsEnabled=ErxStatus.Disabled;
					if(Preferences.GetBool(PreferenceName.NewCropIsLegacy)) {
						provErxCur.IsEnabled=ErxStatus.Enabled;
					}
				}
				else {//DoseSpot
					provErxCur.IsEnabled=ErxStatus.PendingAccountId;
				}
				provErxCur.IsIdentifyProofed=false;
				provErxCur.IsSentToHq=false;
				provErxCur.ErxType=erxOption;
				provErxCur.UserId=userId;
				ProviderErxs.Insert(provErxCur);
				DataValid.SetInvalid(InvalidType.ProviderErxs);
			}
			//Make sure that there is a UserId associated to the providererx if the erx option utilized these ids (DoseSpot)
			ProviderErx provOld=provErxCur.Clone();
			provErxCur.UserId=userId;
			if(ProviderErxs.Update(provErxCur,provOld)) {
				DataValid.SetInvalid(InvalidType.ProviderErxs);
			}
			if(erxOption==ErxOption.DoseSpot) {
				DoseSpot.SyncClinicErxsWithHQ();
			}
			bool isDistributorCustomer=false;
			if(Preferences.GetString(PreferenceName.NewCropPartnerName)!="" || Preferences.GetString(PreferenceName.NewCropPassword)!="") {
				isDistributorCustomer=true;
			}
			bool isOdUpdateAddress=false;
			if(Preferences.GetString(PreferenceName.UpdateServerAddress).ToLower().Contains("opendentalsoft.com") || 
				Preferences.GetString(PreferenceName.UpdateServerAddress).ToLower().Contains("open-dent.com"))
			{
				isOdUpdateAddress=true;
			}
			DateTime dateLastAccessMonth=DateTime.MinValue;
			if(erxOption==ErxOption.Legacy) {
				dateLastAccessMonth=PrefC.GetDate(PreferenceName.NewCropDateLastAccessCheck);
			}
			else {//DoseSpot
				dateLastAccessMonth=PrefC.GetDate(PreferenceName.DoseSpotDateLastAccessCheck);
			}
			dateLastAccessMonth=new DateTime(dateLastAccessMonth.Year,dateLastAccessMonth.Month,1);
			if(erxOption==ErxOption.Legacy && isDistributorCustomer && isOdUpdateAddress) {
				//The distributor forgot to change the "Server Address for Updates" inside of the Update Setup window for this customer.
				//Do not contact the OD web service.
			}
			else if(provErxCur.IsEnabled!=ErxStatus.Enabled //If prov is not yet enabled, always check with OD HQ to see if the prov has been enabled yet.
				|| (erxOption==ErxOption.Legacy && !Preferences.GetBool(PreferenceName.NewCropIsLegacy) && !provErxCur.IsIdentifyProofed)//If new prov is not yet identity proofed, send to OD HQ.
				|| !provErxCur.IsSentToHq//If prov has not been sent to OD HQ yet, always send to OD HQ so we can track our providers using eRx.
				|| dateLastAccessMonth<new DateTime(DateTimeOD.Today.Year,DateTimeOD.Today.Month,1))//If it has been over a month since sent to OD HQ, send.
			{
				//An OD customer, or a Distributor customer if the distributor has a custom web service for updates.
				//For distributors who implement this feature, you will be able to use FormErxAccess at your office to control individual provider access.
				//We compare the last access date by month above, because eRx charges are based on monthly usage.  Avoid extra charges for disabled providers.
				XmlWriterSettings xmlWSettings=new XmlWriterSettings();
				xmlWSettings.Indent=true;
				xmlWSettings.IndentChars=("    ");
				StringBuilder strBuild=new StringBuilder();
				using(XmlWriter xmlWriter=XmlWriter.Create(strBuild,xmlWSettings)) {
					xmlWriter.WriteStartElement("ErxAccessRequest");
						xmlWriter.WriteStartElement("RegistrationKey");
						xmlWriter.WriteString(Preferences.GetString(PreferenceName.RegistrationKey));
						xmlWriter.WriteEndElement();//End reg key
					List<ProviderErx> listUnsentProviders=ProviderErxs.GetAllUnsent();
					for(int i=0;i<listUnsentProviders.Count;i++) {
						xmlWriter.WriteStartElement("Prov");
							xmlWriter.WriteAttributeString("NPI",listUnsentProviders[i].NationalProviderID);
							xmlWriter.WriteAttributeString("IsEna",((int)listUnsentProviders[i].IsEnabled).ToString());
							xmlWriter.WriteAttributeString("ErxType",((int)listUnsentProviders[i].ErxType).ToString());
							xmlWriter.WriteAttributeString("UserId",listUnsentProviders[i].UserId);
						xmlWriter.WriteEndElement();//End Prov
					}
					xmlWriter.WriteEndElement();//End ErxAccessRequest
				}
#if DEBUG
				Imedisoft.localhost.Service1 updateService=new Imedisoft.localhost.Service1();
#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
					updateService.Url=Prefs.GetString(PrefName.UpdateServerAddress);
#endif
				if(Preferences.GetString(PreferenceName.UpdateWebProxyAddress)!="") {
					IWebProxy proxy=new WebProxy(Preferences.GetString(PreferenceName.UpdateWebProxyAddress));
					ICredentials cred=new NetworkCredential(Preferences.GetString(PreferenceName.UpdateWebProxyUserName),Preferences.GetString(PreferenceName.UpdateWebProxyPassword));
					proxy.Credentials=cred;
					updateService.Proxy=proxy;
				}
				try {
					string result=updateService.GetErxAccess(strBuild.ToString());//may throw error
					XmlDocument xmlDoc=new XmlDocument();
					xmlDoc.LoadXml(result);
					XmlNodeList listXmlNodes=xmlDoc.SelectNodes("//Prov");
					List<ProviderErx> listProviderErxs=ProviderErxs.GetDeepCopy();
					bool[] arrayIsSentToHq=new bool[listProviderErxs.Count];
					bool isCacheRefreshNeeded=false;
					for(int i=0;i<listXmlNodes.Count;i++) {//Loop through providers.
						XmlNode nodeProv=listXmlNodes[i];
						string provNpi="";
						string provErxUserId="";
						ErxStatus provEnabledStatus=ErxStatus.Disabled;
						bool isProvIdp=false;
						bool isCurrentErxType=true;
						for(int j=0;j<nodeProv.Attributes.Count;j++) {//Loop through the attributes for the current provider.
							XmlAttribute xmlAttribute=nodeProv.Attributes[j];
							if(xmlAttribute.Name=="NPI") {
								provNpi=Regex.Replace(xmlAttribute.Value,"[^0-9]*","");//NPI with all non-numeric characters removed.
								if(provNpi.Length!=10) {
									provNpi="";
									break;//Invalid NPI
								}
							}
							else if(xmlAttribute.Name=="IsEna") {
								provEnabledStatus=PIn.Enum<ErxStatus>(xmlAttribute.Value,false,ErxStatus.Undefined);
							}
							else if(xmlAttribute.Name=="IsIdp" && xmlAttribute.Value=="1") {
								isProvIdp=true;
							}
							else if(xmlAttribute.Name=="ErxType" && PIn.Enum<ErxOption>(PIn.Int(xmlAttribute.Value))!=erxOption) {
								isCurrentErxType=false;
							}
							else if(xmlAttribute.Name=="UserId") {
								provErxUserId=xmlAttribute.Value;
							}
						}
						if(!isCurrentErxType) {//We don't want to change records for DoseSpot if the user is using NewCrop and vice versa.
							continue;
						}
						if(provNpi=="") {
							continue;
						}
						ProviderErx oldProvErx=ProviderErxs.GetOneForNpiAndOption(provNpi,erxOption);
						if(oldProvErx==null) {
							continue;
						}
						arrayIsSentToHq[listProviderErxs.Select(x => x.ProviderErxNum).ToList().IndexOf(oldProvErx.ProviderErxNum)]=true;
						ProviderErx provErx=oldProvErx.Clone();
						provErx.IsEnabled=provEnabledStatus;
						provErx.IsIdentifyProofed=isProvIdp;
						provErx.IsSentToHq=true;
						provErx.UserId=provErxUserId;
						//Dont need to set the ErxType here because it's not something that can be changed by HQ.
						if(ProviderErxs.Update(provErx,oldProvErx)) {
							isCacheRefreshNeeded=true;
						}
					}
					//Any proverxs which are in the local customer database but not sent to HQ, flag as unsent.
					//Providererx records were being deleted from HQ due to a sync issue at HQ.
					for(int i=0;i<arrayIsSentToHq.Length;i++) {
						if(arrayIsSentToHq[i]) {
							continue;
						}
						ProviderErx provErx=listProviderErxs[i];
						ProviderErx oldProvErx=provErx.Clone();
						provErx.IsSentToHq=false;
						if(ProviderErxs.Update(provErx,oldProvErx)) {
							isCacheRefreshNeeded=true;
						}	
					}
					if(isCacheRefreshNeeded) {
						DataValid.SetInvalid(InvalidType.ProviderErxs);
					}
					if(erxOption==ErxOption.Legacy) {
						if(Preferences.Set(PreferenceName.NewCropDateLastAccessCheck,DateTimeOD.Today)) {
							DataValid.SetInvalid(InvalidType.Prefs);
						}
					}
					else {//DoseSpot
						if(Preferences.Set(PreferenceName.DoseSpotDateLastAccessCheck,DateTimeOD.Today)) {
							DataValid.SetInvalid(InvalidType.Prefs);
						}
					}
				}
				catch {
					//Failed to contact server and/or update provider IsEnabled statuses.  We will simply use what we already know in the local database.
				}
			}
		}
#endregion Methods - Private - General

#region Methods - Private - Tab EnterTx
		///<summary>Sets many fields for a new procedure, then displays it for editing before inserting it into the db.  No need to worry about ProcOld because it's an insert, not an update.  AddProcedure and AddQuick both call AddProcHelper, where most of the logic for setting the fields for a new procedure is located.</summary>
		private void AddProcedure(Procedure ProcCur,List<Fee> listFees) {
			if(!AddProcHelper(ProcCur,listFees)) { //Procedure was deleted.
				return;
			}
			//Get from DB to get updated timestamps for permission checks and to initialize nullable variables, like strings, before filling FormProcEdit.
			bool isAdditionalProc=ProcCur.IsAdditional;
			ProcCur=Procedures.GetOneProc(ProcCur.ProcNum,true);//This breaks the reference to the original Procedure object in the calling method.
			ProcCur.IsAdditional=isAdditionalProc;
			FormProcEdit formProcEdit=new FormProcEdit(ProcCur,_patCur.Copy(),_famCur,listPatToothInitials:_listToothInitials);
			formProcEdit.IsNew=true;
			formProcEdit.ShowDialog();
			if(formProcEdit.DialogResult==DialogResult.Cancel) {
				try {
					Procedures.Delete(ProcCur.ProcNum);//also deletes the claimprocs
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
				}
				return;//cancelled insert
			}
			if(_procStatusNew==ProcStat.C) {//User didn't cancel (delete) in FormProcEdit
				Encounters.InsertDefaultEncounter(ProcCur.PatNum,ProcCur.ProvNum,ProcCur.ProcDate);//Auto-insert default encounter
			}
			if(!Programs.UsingOrion //no need to synch with Orion
				&& new[] { ProcStat.C,ProcStat.EC,ProcStat.EO }.Contains(_procStatusNew)) //only run Recalls for completed, existing current, or existing other
			{
				Recalls.Synch(_patCur.PatNum);
			}
			logComplCreate(ProcCur);
		}

		///<summary>Called by AddProcedure and AddQuick.  Both methods contained versions of this code and a bug was introduced in version 15.3 because the order of the regions changed in the two methods and no longer matched.  This helper method prevents bugs caused by trying to keep duplicate code blocks synced.</summary>
		private bool AddProcHelper(Procedure ProcCur,List<Fee> listFees) {
			ProcCur.PatNum=_patCur.PatNum;
			//ProcCur.CodeNum=ProcedureCodes.GetProcCode(ProcCur.OldCode).CodeNum;//already set
			if(textDate.Text=="" || textDate.errorProvider1.GetError(textDate)!="") {
				ProcCur.DateTP=DateTimeOD.Today;
			}
			else {
				ProcCur.DateTP=PIn.Date(textDate.Text);
			}
			if(_procStatusNew!=ProcStat.EO) {
				ProcCur.ProcDate=ProcCur.DateTP;
			}
			ProcedureCode procCodeCur=ProcedureCodes.GetById(ProcCur.CodeNum);


#region ProvNum
			//This strategy for assigning procnum is consistent here, in the Chart Module, but it doesn't seem to be used anywhere else.
			//For example, in the Appt Module, for recall, etc, the proc is simply whatever appointment we are in.
			ProcCur.ProvNum=procCodeCur.DefaultProviderId??0;//use proc default prov if set
			if(ProcCur.ProvNum==0) {//no proc default prov set, check for appt prov, then use pri prov
				long provPri=_patCur.PriProv;
				long provSec=_patCur.SecProv;
				Appointment[] arrayAptToday=_arrayAppts.Where(x => x.AptDateTime.Date==DateTime.Today && x.AptStatus!=ApptStatus.Planned).ToArray();
				if(arrayAptToday.Length>0) {
					provPri=arrayAptToday[0].ProvNum;
					provSec=arrayAptToday[0].ProvHyg;
				}
				if(Providers.GetById(provPri).IsHidden) {
					//If the Patient's Primary Provider is hidden, use the patient's clinic's default provider, or practice default provider
					if(PrefC.HasClinicsEnabled){
						provPri=Providers.GetDefaultProvider(_patCur.ClinicNum).Id;
					}
					else {
						provPri=Providers.GetDefaultProvider().Id;
					}
				}
				if(procCodeCur.IsHygiene && provSec!=0 && !Providers.GetById(provSec).IsHidden) {//Do not assign Sec. Provider's to Procedures when hidden
					ProcCur.ProvNum=provSec;
				}
				else {
					ProcCur.ProvNum=provPri;
				}
			}
#endregion ProvNum


			if(_procStatusNew==ProcStat.C) {
				if(ProcCur.ProcDate.Date>DateTime.Today.Date && !Preferences.GetBool(PreferenceName.FutureTransDatesAllowed)) {
					MessageBox.Show("Completed procedures cannot be set for future dates.");
					return false;
				}
				Procedures.SetOrthoProcComplete(ProcCur,procCodeCur); //does nothing if not an ortho proc
			}
#region Note
			if(_procStatusNew==ProcStat.C || _procStatusNew==ProcStat.TP) {
				string procNoteDefault=ProcCodeNotes.GetNote(ProcCur.ProvNum,ProcCur.CodeNum,_procStatusNew);
				if(ProcCur.Note!="" && procNoteDefault!="") {
					ProcCur.Note+="\r\n"; //add a new line if there was already a ProcNote on the procedure.
				}
				ProcCur.Note+=procNoteDefault;
				if(!Preferences.GetBool(PreferenceName.ProcPromptForAutoNote)) {
					//Users do not want to be prompted for auto notes, so remove them all from the procedure note.
					ProcCur.Note=Regex.Replace(ProcCur.Note,@"\[\[.+?\]\]","");
				}
			}
			else {
				ProcCur.Note="";
			}
#endregion
			ProcCur.ClinicNum=_patCur.ClinicNum;
			if(_procStatusNew==ProcStat.R || _procStatusNew==ProcStat.EO || _procStatusNew==ProcStat.EC) {
				ProcCur.ProcFee=0;
			}
			else {
				ProcCur.MedicalCode=procCodeCur.MedicalCode;
				ProcCur.ProcFee=Procedures.GetProcFee(_patCur,_listPatPlans,_listInsSubs,_listInsPlans,ProcCur.CodeNum,ProcCur.ProvNum,ProcCur.ClinicNum,
					ProcCur.MedicalCode,listFees:listFees);
			}
			if(_procStatusNew==ProcStat.C 
				&& !Security.IsAuthorized(Permissions.ProcComplCreate,ProcCur.ProcDate,ProcCur.CodeNum,ProcCur.ProcFee)) 
			{			
				return false;
			}
			//surf
			//toothnum
			if(comboPriority.SelectedIndex==0) {
				ProcCur.Priority=0;
			}
			else {
				ProcCur.Priority=comboPriority.GetSelectedDefNum();
			}
			ProcCur.ProcStatus=_procStatusNew;
			if(listDx.SelectedIndex!=-1) {
				ProcCur.Dx=listDx.GetSelected<Definition>().Id;
			}
			if(comboPrognosis.SelectedIndex==0) {
				ProcCur.Prognosis=0;
			}
			else {
				ProcCur.Prognosis=comboPrognosis.GetSelectedDefNum();
			}
			ProcCur.BaseUnits=procCodeCur.BaseUnits;
			ProcCur.SiteNum=_patCur.SiteNum;
			ProcCur.RevCode=procCodeCur.RevenueCodeDefault;
			ProcCur.DiagnosticCode=Preferences.GetString(PreferenceName.ICD9DefaultForNewProcs);
			ProcCur.PlaceService=Preferences.GetString(PreferenceName.DefaultProcedurePlaceService, PlaceOfService.Office);//Default Proc Place of Service for the Practice is used. 
			if(Users.IsUserCpoe(Security.CurrentUser)) {
				//This procedure is considered CPOE because the provider is the one that has added it.
				ProcCur.IsCpoe=true;
			}
			//Find out if we are going to link the procedure to an ortho case.
			OrthoCaseProcLinkingData orthoCaseProcLinkingData=new OrthoCaseProcLinkingData(ProcCur.PatNum);
			ProcCur.ProcNum=Procedures.Insert(ProcCur,skipDiscountPlanAdjustment:orthoCaseProcLinkingData.CanProcLinkToOrthoCase(ProcCur));
			OrthoProcLink orthoProcLink=OrthoProcLinks.TryLinkProcForActiveOrthoCaseAndUpdate(orthoCaseProcLinkingData,ProcCur);
			if(_listChartedProcs!=null) {
				_listChartedProcs.Add(ProcCur);
			}
			if(_procStatusNew==ProcStat.TP) {
				AttachProcToTPs(ProcCur);
			}
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& procCodeCur.PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(_patCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			Procedures.ComputeEstimates(ProcCur,_patCur.PatNum,ref listClaimProcs,true,_listInsPlans,_listPatPlans,_listBenefits,
				null,null,true,
				_patCur.Age,_listInsSubs,
				null,false,false,_listSubstitutionLinks,false,
				listFees,orthoProcLink:orthoProcLink,orthoCase:orthoCaseProcLinkingData.ActiveOrthoCase,orthoSchedule:orthoCaseProcLinkingData.OrthoSchedule
				,listOrthoProcLinksForOrthoCase:orthoCaseProcLinkingData.ListProcLinksForCase);
			listClaimProcs=ClaimProcs.GetForProc(ClaimProcs.Refresh(_patCur.PatNum),ProcCur.ProcNum);
			long verifyCode;
			bool isMandibular=false;
			if(!string.IsNullOrEmpty(ProcCur.ToothRange)) {
				isMandibular=ProcCur.ToothRange.Split(',').Any(x => !Tooth.IsMaxillary(x));
			}
			if(AutoCodeItems.ShouldPromptForCodeChange(ProcCur,procCodeCur,_patCur,isMandibular,listClaimProcs,out verifyCode)) {
				FormAutoCodeLessIntrusive FormACLI=new FormAutoCodeLessIntrusive(_patCur,ProcCur,procCodeCur,verifyCode,_listPatPlans,_listInsSubs,_listInsPlans,
					_listBenefits,listClaimProcs);
				if(FormACLI.ShowDialog()!=DialogResult.OK
					&& Preferences.GetBool(PreferenceName.ProcEditRequireAutoCodes))
				{
					FormProcEdit formProcEdit=new FormProcEdit(ProcCur,_patCur,_famCur,listPatToothInitials:_listToothInitials);//ProcCur may be modified in this form due to passing by reference. Intentional.
					formProcEdit.ShowDialog();
					if(formProcEdit.DialogResult!=DialogResult.OK) {
						try {
							Procedures.Delete(ProcCur.ProcNum,true);//also deletes the claimprocs
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
						}
						return false;
					}					
				}
			}
			return true;
		}

		///<summary>Add a procedure for all selected quadrants.</summary>
		private void AddQuadProcs(Procedure procCur,List<Fee> listFees) {
			if(_toothChartRelay.SelectedTeeth.Count>0) {
				List<string> listSelectedQuads=Tooth.GetQuadsForTeeth(_toothChartRelay.SelectedTeeth);
				foreach(string quad in listSelectedQuads) {
					Procedure proc=procCur.Copy();
					proc.Surf=quad;
					AddQuick(proc,listFees);
				}
			}
			else {
				//Don't know what to set the surface to, so we have the user enter it.
				AddProcedure(procCur,listFees);
			}
		}

		///<summary>No user dialog is shown.  This only works for some kinds of procedures.  Set the codeNum first. AddProcedure and AddQuick both call AddProcHelper, where most of the logic for setting the fields for a new procedure is located. No validation is done before adding the procedure so check all permissions and such prior to calling this method.</summary>
		private void AddQuick(Procedure ProcCur,List<Fee> listFees) {
			Plugins.HookAddCode(this,"ContrChart.AddQuick_begin",ProcCur,listFees);
			if(!AddProcHelper(ProcCur,listFees)) { //Procedure was deleted.
				return;
			}
			FormProcEdit formProcEdit=null;
			if(Programs.UsingOrion) {//Orion requires a DPC to be set. Force the proc edit window open so they can set it.
				//Get from DB to get updated timestamps for permission checks and to initialize nullable variables, like strings, before filling FormProcEdit.
				ProcCur=Procedures.GetOneProc(ProcCur.ProcNum,true);//This breaks the reference to the original Procedure object in the calling method.
				formProcEdit=new FormProcEdit(ProcCur,_patCur.Copy(),_famCur,listPatToothInitials:_listToothInitials);
				formProcEdit.IsNew=true;
				formProcEdit.OrionProvNum=Providers.GetOrionProvNum(ProcCur.ProvNum);
				formProcEdit.ShowDialog();
				if(formProcEdit.DialogResult==DialogResult.Cancel) {
					try {
						Procedures.Delete(ProcCur.ProcNum);//also deletes the claimprocs
					}
					catch(Exception ex) {
						MessageBox.Show(ex.Message);
					}
					return;//cancelled insert
				}
			}
			if(_procStatusNew==ProcStat.C) {//either not using Orion, or user didn't cancel (delete) in FormProcEdit
				Encounters.InsertDefaultEncounter(ProcCur.PatNum,ProcCur.ProvNum,ProcCur.ProcDate);//Auto-insert default encounter
			}
			if(new[] { ProcStat.C,ProcStat.EC,ProcStat.EO }.Contains(_procStatusNew)) //only run Recalls for completed, existing current, or existing other
			{
				Recalls.Synch(_patCur.PatNum);
			}
			logComplCreate(ProcCur);
		}

		private void ClearButtons() {
			//unfortunately, these colors no longer show since the XP button style was introduced.
			butM.BackColor=Color.FromName("Control"); ;
			butOI.BackColor=Color.FromName("Control");
			butD.BackColor=Color.FromName("Control");
			butL.BackColor=Color.FromName("Control");
			butBF.BackColor=Color.FromName("Control");
			butV.BackColor=Color.FromName("Control");
			textSurf.Text="";
			listDx.SelectedIndex=-1;
			//listProcButtons.SelectedIndex=-1;
			listViewButtons.SelectedIndices.Clear();
			textProcCode.Text="Type Proc Code";
		}

		private void DeleteRows() {
			if(gridProg.SelectedIndices.Length==0) {
				MessageBox.Show("Please select an item first.");
				return;
			}
			if(MessageBox.Show("Delete Selected Item(s)?","",MessageBoxButtons.OKCancel)
				!=DialogResult.OK) {
				return;
			}
			int skippedSecurity=0;
			int skippedRxSecurity=0;
			int skippedC=0;
			int skippedAttached=0;
			int skippedLinkedToOrthoCase=0;
			List<DataRow> listSelectedRows=gridProg.SelectedIndices.Where(x => x>-1 && x<gridProg.Rows.Count)
				.Select(x => (DataRow)gridProg.Rows[x].Tag).ToList();
			OrthoProcLink orthoProcLink=null;
			Dictionary<long,OrthoProcLink> dictOrthoProcLinks=OrthoProcLinks.GetManyForProcs(listSelectedRows.Where(x => PIn.Long(x["ProcNum"].ToString())!=0)
				.ToList().Select(y => PIn.Long(y["ProcNum"].ToString())).ToList()).ToDictionary(z => z.ProcNum,z => z);
			foreach(DataRow row in listSelectedRows) {
				long procNum=PIn.Long(row["ProcNum"].ToString());
				if(procNum!=0) {
					dictOrthoProcLinks.TryGetValue(procNum,out orthoProcLink);
				}
				if(!CanDeleteRow(row,doCheckDb:true,ref skippedSecurity,ref skippedRxSecurity,ref skippedC,ref skippedAttached,ref skippedLinkedToOrthoCase,
					orthoProcLink)) 
				{
					continue;
				}
				if(row["ProcNum"].ToString()!="0") {
					try {
						Procedures.Delete(PIn.Long(row["ProcNum"].ToString()));//also deletes the claimprocs
						CanadianLabFeeHelper(Procedures.GetOneProc(PIn.Long(row["ProcNum"].ToString()),false).ProcNumLab);
						SecurityLogs.MakeLogEntry(Permissions.ProcDelete,_patCur.PatNum,row["ProcCode"].ToString()+" ("+row["procStatus"]+"), "
							+PIn.Double(row["procFee"].ToString()).ToString("c"));
					}
					catch(Exception ex) {
						MessageBox.Show(ex.Message);
						//continue;//continues to next in loop from here implicitly, no explicit call to continue necessary
					}
				}
				else if(row["RxNum"].ToString()!="0") {
					RxPat rxPat=RxPats.GetRx(PIn.Long(row["RxNum"].ToString()));
					SecurityLogs.MakeLogEntry(Permissions.RxEdit,_patCur.PatNum,"FROM("+rxPat.RxDate.ToShortDateString()+","+rxPat.Drug+","+rxPat.ProvNum+","
						+rxPat.Disp+","+rxPat.Refills+")"+"\r\nTO('deleted')",rxPat.Id,rxPat.DateTStamp);
					RxPats.Delete(PIn.Long(row["RxNum"].ToString()));
				}
			}
			Recalls.Synch(_patCur.PatNum);
			if(skippedC>0) {
				MessageBox.Show("Not allowed to delete completed procedures from here."+"\r"
					+skippedC.ToString()+" "+"item(s) skipped.");
			}
			if(skippedLinkedToOrthoCase>0) {
				MessageBox.Show("Not allowed to delete procedures that are linked to ortho cases. "+
					"Detach the procedure or delete the ortho case first."+"\r"+skippedLinkedToOrthoCase.ToString()+" "+"item(s) skipped.");
			}
			if(skippedSecurity>0) {
				MessageBox.Show("Not allowed to delete procedures due to security."+"\r"
					+skippedSecurity.ToString()+" "+"item(s) skipped.");
			}
			if(skippedRxSecurity>0) {
				MessageBox.Show("Not allowed to delete Rx due to security."+"\r"
					+skippedRxSecurity.ToString()+" "+"item(s) skipped.");
			}
			if(skippedAttached>0) {
				MessageBox.Show("Not allowed to delete TP procedures with payments attached."+"\r"
					+skippedAttached.ToString()+" "+"item(s) skipped. ");
			}
			ModuleSelected(_patCur.PatNum);
		}

		private void EnterTypedCode() {
			//orionProcNum=0;
			if(_procStatusNew==ProcStat.C) {
				if(!Preferences.GetBool(PreferenceName.AllowSettingProcsComplete)) {
					MessageBox.Show("Set the procedure complete by setting the appointment complete.  "
						+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
					return;
				}
				//We will call this method again with the real ProcFee once we know it.
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),ProcedureCodes.GetCodeNum(textProcCode.Text),0)) {
					return;
				}
			}
			if(CultureInfo.CurrentCulture.Name=="en-US" && Regex.IsMatch(textProcCode.Text,@"^\d{4}$")) {//if exactly 4 digits
				if(!ProcedureCodes.GetContainsKey(textProcCode.Text)) {//4 digit code is not found
					textProcCode.Text="D"+textProcCode.Text;
				}
				else { //or if it's a 4 digit code that's hidden, also add the D
					ProcedureCode procCode=ProcedureCodes.GetProcCode(textProcCode.Text);
					if(Definitions.GetHidden(DefinitionCategory.ProcCodeCats,procCode.ProcedureCategory)) {
						textProcCode.Text="D"+textProcCode.Text;
					}
				}
			}
			if(!ProcedureCodes.GetContainsKey(textProcCode.Text)) {
				MessageBox.Show("Invalid code.");
				//textProcCode.Text="";
				textProcCode.SelectionStart=textProcCode.Text.Length;
				return;
			}
			if(Definitions.GetHidden(DefinitionCategory.ProcCodeCats,ProcedureCodes.GetProcCode(textProcCode.Text).ProcedureCategory)) {//if the category is hidden
				MessageBox.Show("Code is in a hidden category and cannot be added from here.");
				textProcCode.SelectionStart=textProcCode.Text.Length;
				return;
			}
			//Do not return past this point---------------------------------------------------------------------------------
			if(_listSubstitutionLinks==null) {
				_listSubstitutionLinks=SubstitutionLinks.GetAllForPlans(_listInsPlans);
			}
			ProcedureCode procedureCode=ProcedureCodes.GetProcCode(textProcCode.Text);
			List<Fee> listFees=Fees.GetListFromObjects(new List<ProcedureCode>(){ procedureCode },null,//no proc, so medical code won't have changed
				null,//listProvNumsTreat: providers will instead be set from other places
				_patCur.PriProv,_patCur.SecProv,_patCur.FeeSched,_listInsPlans,new List<long>(){_patCur.ClinicNum},_arrayAppts.ToList(),_listSubstitutionLinks,_patCur.DiscountPlanNum);
			List<string> procCodes=new List<string>();
			//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
			if(textProcCode.Text!="D9986" && textProcCode.Text!="D9987") {
				Procedures.SetDateFirstVisit(DateTimeOD.Today,1,_patCur);
			}
			ProcedureTreatmentArea tArea;
			Procedure ProcCur;
			for(int n=0;n==0 || n<_toothChartRelay.SelectedTeeth.Count;n++) {//always loops at least once.
				ProcCur=new Procedure();//this will be an insert, so no need to set CurOld
				ProcCur.CodeNum=ProcedureCodes.GetCodeNum(textProcCode.Text);
				bool isValid=true;
				tArea=procedureCode.TreatmentArea;//ProcedureCodes.GetProcCode(ProcCur.CodeNum).TreatArea;
				if((tArea==ProcedureTreatmentArea.Arch
					|| tArea==ProcedureTreatmentArea.Mouth
					|| tArea==ProcedureTreatmentArea.Quad
					|| tArea==ProcedureTreatmentArea.Sextant
					|| tArea==ProcedureTreatmentArea.ToothRange)
					&& n>0) {//the only two left are tooth and surf
					continue;//only entered if n=0, so they don't get entered more than once.
				}
				else if(tArea==ProcedureTreatmentArea.Quad) {
					AddQuadProcs(ProcCur,listFees);
				}
				else if(tArea==ProcedureTreatmentArea.Surface) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						isValid=false;
					}
					else {
						ProcCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
					}
					if(textSurf.Text=="") {
						isValid=false;
					}
					else {
						ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,ProcCur.ToothNum);//it's ok if toothnum is invalid
					}
					if(isValid) {
						AddQuick(ProcCur,listFees);
					}
					else {
						AddProcedure(ProcCur,listFees);
					}
				}
				else if(tArea==ProcedureTreatmentArea.Tooth) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						AddProcedure(ProcCur,listFees);
					}
					else {
						ProcCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
						AddQuick(ProcCur,listFees);
					}
				}
				else if(tArea==ProcedureTreatmentArea.ToothRange) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						AddProcedure(ProcCur,listFees);
					}
					else {
						ProcCur.ToothRange="";
						for(int b=0;b<_toothChartRelay.SelectedTeeth.Count;b++) {
							if(b!=0) ProcCur.ToothRange+=",";
							ProcCur.ToothRange+=_toothChartRelay.SelectedTeeth[b];
						}
						AddQuick(ProcCur,listFees);
					}
				}
				else if(tArea==ProcedureTreatmentArea.Arch) {
					if(_toothChartRelay.SelectedTeeth.Count==0) {
						AutoCodeItem autoCodeItem=null;
						if(AutoCodeItems.Contains(ProcCur.CodeNum)) { 
							autoCodeItem=AutoCodeItems.GetByAutoCode(AutoCodeItems.GetById(ProcCur.CodeNum).AutoCodeId)
								.FirstOrDefault(x => x.ProcedureCodeId==ProcCur.CodeNum);
						}
						List<AutoCodeCondition> listAutoCodeCond=new List<AutoCodeCondition>();
						if(autoCodeItem!=null) {
							listAutoCodeCond=AutoCodeConditions.GetByAutoCodeItem(autoCodeItem.Id);
						}
						if(listAutoCodeCond.Count==1) {
							if(listAutoCodeCond[0].Type==AutoCodeConditionType.Maxillary) {
								ProcCur.Surf="U";
							}
							else if(listAutoCodeCond[0].Type==AutoCodeConditionType.Mandibular) {
								ProcCur.Surf="L";
							}
						}
						else {
							ProcCur.Surf=CodeMapping.GetArchSurfaceFromProcCode(ProcedureCodes.GetById(ProcCur.CodeNum));
						}
						AddProcedure(ProcCur,listFees);
						continue;
					}
					foreach(string arch in Tooth.GetArchesForTeeth(_toothChartRelay.SelectedTeeth)) {
						Procedure proc=ProcCur.Copy();
						proc.Surf=arch;
						AddQuick(proc,listFees);
					}
				}
				else if(tArea==ProcedureTreatmentArea.Sextant) {
					AddProcedure(ProcCur,listFees);
				}
				else {//mouth
					AddQuick(ProcCur,listFees);
				}
				procCodes.Add(ProcedureCodes.GetById(ProcCur.CodeNum).Code);
			}//n selected teeth
			//this was requiring too many irrelevant queries and going too slowly   //ModuleSelected(PatCur.PatNum);			
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			ClearButtons();
			FillProgNotes();
			textProcCode.Text="";
			textProcCode.Select();
			if(_procStatusNew==ProcStat.C) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodes,_patCur.PatNum);
			}
		}

		///<summary>If quickbutton, then pass in procButtonQuick and set procButton to null.</summary>
		private void ProcButtonClicked(ProcButton procButton,ProcButtonQuick procButtonQuick=null) {
			if(_procStatusNew==ProcStat.C) {
				if(!Preferences.GetBool(PreferenceName.AllowSettingProcsComplete)) {
					MessageBox.Show("Set the procedure complete by setting the appointment complete.  "
						+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
					return;
				}
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))) {
					return;
				}
			}
			bool isValid;
			ProcedureTreatmentArea tArea;
			int quadCount=0;//automates quadrant codes.
			long[] arrayCodeList;
			long[] arrayAutoCodeList;
			if(procButton==null) {//Quick Button
				arrayCodeList=new long[1];
				arrayCodeList[0]=ProcedureCodes.GetCodeNum(procButtonQuick.CodeValue);
				if(arrayCodeList[0]==0) {
					MessageBox.Show(this,"Procedure code does not exist in database"+" : "+procButtonQuick.CodeValue);
					return;
				}
				arrayAutoCodeList=new long[0];
			}
			else {//Proc Button
				arrayCodeList=ProcButtonItems.GetCodeNumListForButton(procButton.ProcButtonNum);
				arrayAutoCodeList=ProcButtonItems.GetAutoListForButton(procButton.ProcButtonNum);
				//if(codeList.
			}
			//It is very important that we stop users here before entering any procedures or doing any automation.
			foreach(long autoCodeNum in arrayAutoCodeList) {
				if(!AutoCodes.GetContainsKey(autoCodeNum)) {
					MessageBox.Show($"The procedure button '{procButton.Description}' contains an invalid AutoCode.\r\n" +
						$"Run {nameof(DatabaseMaintenances.ProcButtonItemsDeleteWithInvalidAutoCode)} in the Database Maintenance Tool and try again.");
					return;
				}
				AutoCode autoCode=AutoCodes.GetById(autoCodeNum);
				if(AutoCodeItems.GetByAutoCode(autoCode.Id).Count==0) {
					//AutoCode is not setup correctly.
					MessageBox.Show(this,"The following AutoCode has no associated Procedure Codes: "+"\r\n"+autoCode.Description+"\r\n"
						+"AutoCode must be setup correctly before it can be used with a Quick Proc Button.");
					return;
				}
			}
			//Do not return past this point---------------------------------------------------------------------------------
			List<ProcedureCode> listProcedureCodes=new List<ProcedureCode>();//just for the fee info
			for(int i=0;i<arrayCodeList.Length;i++) {
				listProcedureCodes.Add(ProcedureCodes.GetById(arrayCodeList[i]));//could be a harmless empty code
			}
			string toothNumString;
			for(int i=0;i<arrayAutoCodeList.Length;i++) {//this is just a quick loop for fees. The real one is down further
				for(int n=0;n==0 || n<_toothChartRelay.SelectedTeeth.Count;n++) {
					isValid=true;
					if(_toothChartRelay.SelectedTeeth.Count!=0) toothNumString=_toothChartRelay.SelectedTeeth[n]; else toothNumString="";
					string surf="";
					if(textSurf.Text=="5" && CultureInfo.CurrentCulture.Name.EndsWith("CA")) surf="V"; else surf=Tooth.SurfTidyForClaims(textSurf.Text,toothNumString);
					bool isAdditional=n>0;
					bool willBeMissing=Procedures.WillBeMissing(toothNumString,_patCur.PatNum);//db call, but this NEEDS to happen.
					long codeNum=AutoCodeItems.GetProcedureCode(arrayAutoCodeList[i],toothNumString,surf,isAdditional,_patCur.Age,willBeMissing);
					listProcedureCodes.Add(ProcedureCodes.GetById(codeNum));
				}
			}
			if(_listSubstitutionLinks==null) {
				_listSubstitutionLinks=SubstitutionLinks.GetAllForPlans(_listInsPlans);
			}
			List<Fee> listFees=Fees.GetListFromObjects(listProcedureCodes,null,//no proc, so medical code won't have changed
				null,//listProvNumsTreat: providers will instead be set from other places
				_patCur.PriProv,_patCur.SecProv,_patCur.FeeSched,_listInsPlans,new List<long>(){_patCur.ClinicNum},_arrayAppts.ToList(),_listSubstitutionLinks,_patCur.DiscountPlanNum);
			//If there are any codes in the list that are NOT 9986s and 9987s, then set the date first visit.
			if(arrayCodeList.Any(x => ProcedureCodes.GetStringProcCode(x) != "D9986" && ProcedureCodes.GetStringProcCode(x) != "D9987")) {
				Procedures.SetDateFirstVisit(DateTimeOD.Today,1,_patCur);
			}
			List<string> listProcCodes=new List<string>();
			//"Bug fix" for Dr. Lazar-------------
			bool isPeriapicalSix=false;
			if(arrayCodeList.Length==6) {//quick check before checking all codes. So that the program isn't slowed down too much.
				string tempVal="";
				foreach(long code in arrayCodeList) {
					tempVal+=ProcedureCodes.GetById(code).ShortDescription;
				}
				if(tempVal=="PAPA+PA+PA+PA+PA+") {
					isPeriapicalSix=true;
					_toothChartRelay.SelectedTeeth.Clear();//set tooth numbers later
				}
			}
			Procedure procCur=null;
			_listChartedProcs=new List<Procedure>();
			for(int i=0;i<arrayCodeList.Length;i++) {
				//needs to loop at least once, regardless of whether any teeth are selected.	
				for(int n=0;n==0 || n<_toothChartRelay.SelectedTeeth.Count;n++) {
					isValid=true;
					procCur=new Procedure();//insert, so no need to set CurOld
					procCur.CodeNum=ProcedureCodes.GetById(arrayCodeList[i]).Id;
					tArea=ProcedureCodes.GetById(procCur.CodeNum).TreatmentArea;
					//"Bug fix" for Dr. Lazar-------------
					if(isPeriapicalSix) {
						//PA code is already set to treatment area mouth by default.
						procCur.ToothNum=",8,14,19,24,30".Split(',')[i];//first code has tooth num "";
						if(i==0) {
							tArea=ProcedureTreatmentArea.Mouth;
						}
						else {
							tArea=ProcedureTreatmentArea.Tooth;
						}
					}
					if((tArea==ProcedureTreatmentArea.Arch
						|| tArea==ProcedureTreatmentArea.Mouth
						|| tArea==ProcedureTreatmentArea.Quad
						|| tArea==ProcedureTreatmentArea.Sextant
						|| tArea==ProcedureTreatmentArea.ToothRange)
						&& n>0) 
					{//the only two left are tooth and surf
						continue;//only entered if n=0, so they don't get entered more than once.
					}
					else if(tArea==ProcedureTreatmentArea.Quad) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							switch(quadCount%4) {
								case 0: procCur.Surf="UR"; break;
								case 1: procCur.Surf="UL"; break;
								case 2: procCur.Surf="LL"; break;
								case 3: procCur.Surf="LR"; break;
							}
							quadCount++;
							if(procButtonQuick!=null && !string.IsNullOrWhiteSpace(procButtonQuick.Surf)) {//from quick buttons only.
								procCur.Surf=Tooth.SurfTidyFromDisplayToDb(procButtonQuick.Surf,procCur.ToothNum);
							}
							AddQuick(procCur,listFees);
						}
						else {
							AddQuadProcs(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Surface) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							isValid=false;
						}
						else {
							procCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
						}
						if(textSurf.Text=="" && procButtonQuick==null) {
							isValid=false;// Pre-ODButtonPanel behavior
						}
						else if(procButtonQuick!=null && procButtonQuick.Surf=="") {
							isValid=false; // ODButtonPanel behavior
						}
						else {
							procCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,procCur.ToothNum);//it's ok if toothnum is not valid.
							if(procButtonQuick!=null && !string.IsNullOrWhiteSpace(procButtonQuick.Surf)) {//from quick buttons only.
								procCur.Surf=Tooth.SurfTidyFromDisplayToDb(procButtonQuick.Surf,procCur.ToothNum);
								if(string.IsNullOrWhiteSpace(procCur.Surf)) {
									//QuickButton setup with a surface that is invalid for the selected tooth.  User should manually select surfaces via FormProcEdit.
									isValid=false;
								}
								//ProcCur.Surf=pbq.Surf;
							}
						}
						if(isValid) {
							AddQuick(procCur,listFees);
						}
						else {
							AddProcedure(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Tooth) {
						if(isPeriapicalSix) {
							AddQuick(procCur,listFees);
						}
						else if(_toothChartRelay.SelectedTeeth.Count==0) {
							AddProcedure(procCur,listFees);
						}
						else {
							procCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
							AddQuick(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.ToothRange) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							AddProcedure(procCur,listFees);
						}
						else {
							procCur.ToothRange="";
							for(int b=0;b<_toothChartRelay.SelectedTeeth.Count;b++) {
								if(b!=0) procCur.ToothRange+=",";
								procCur.ToothRange+=_toothChartRelay.SelectedTeeth[b];
							}
							AddQuick(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Arch) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							procCur.Surf=CodeMapping.GetArchSurfaceFromProcCode(ProcedureCodes.GetById(procCur.CodeNum));
							AddProcedure(procCur,listFees);
							continue;
						}
						foreach(string arch in Tooth.GetArchesForTeeth(_toothChartRelay.SelectedTeeth)) {
							Procedure proc=procCur.Copy();
							proc.Surf=arch;
							AddQuick(proc,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Sextant) {
						AddProcedure(procCur,listFees);
					}
					else {//mouth
						AddQuick(procCur,listFees);
					}
					listProcCodes.Add(ProcedureCodes.GetById(procCur.CodeNum).Code);
				}//n selected teeth
			}//end Part 1 checking for ProcCodes, now will check for AutoCodes
			//long orionProvNum=0;
			for(int i=0;i<arrayAutoCodeList.Length;i++) {
				for(int n=0;n==0 || n<_toothChartRelay.SelectedTeeth.Count;n++) {
					isValid=true;
					if(_toothChartRelay.SelectedTeeth.Count!=0) {
						toothNumString=_toothChartRelay.SelectedTeeth[n];
					}
					else {
						toothNumString="";
					}
					procCur=new Procedure();//this will be an insert, so no need to set CurOld
					//Clean to db
					string surf="";
					//For Canadians, when the only surface charted is 5, we need to not remove the 5 so that the correct one surface auto code is found.
					//However, if multiple surfaces are chated with the 5 then we need to remove the 5 because the surface is redundant.  E.g. B5 -> B
					if(textSurf.Text=="5" && CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						//5 is the Canadian equivalent of V and V is how we save it to the database.
						//We have to do this little extra step right here because SurfTidyForClaims() ignores the 5 surface because it Converts db to claim value.
						surf="V";
					}
					else {
						surf=Tooth.SurfTidyForClaims(textSurf.Text,toothNumString);
					}
					procCur.IsAdditional=n>0;	//This is used for determining the correct autocode in a little bit.
					bool willBeMissing=Procedures.WillBeMissing(toothNumString,_patCur.PatNum);
					procCur.CodeNum=AutoCodeItems.GetProcedureCode(arrayAutoCodeList[i],toothNumString,surf,procCur.IsAdditional,_patCur.Age,willBeMissing);
					tArea=ProcedureCodes.GetById(procCur.CodeNum).TreatmentArea;
					if((tArea==ProcedureTreatmentArea.Arch
						|| tArea==ProcedureTreatmentArea.Mouth
						|| tArea==ProcedureTreatmentArea.Quad
						|| tArea==ProcedureTreatmentArea.Sextant
						|| tArea==ProcedureTreatmentArea.ToothRange)
						&& n>0)
					{//the only two left are tooth and surf
						continue;//only entered if n=0, so they don't get entered more than once.
					}
					else if(tArea==ProcedureTreatmentArea.Quad) {
						if(toothNumString=="") {
							switch(quadCount%4) {
								case 0: procCur.Surf="UR"; break;
								case 1: procCur.Surf="UL"; break;
								case 2: procCur.Surf="LL"; break;
								case 4: procCur.Surf="LR"; break;
							}
							quadCount++;
							AddQuick(procCur,listFees);
						}
						else {
							AddQuadProcs(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Surface) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							isValid=false;
						}
						else {
							procCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
						}
						if(textSurf.Text=="") {
							isValid=false;
						}
						else {
							procCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurf.Text,procCur.ToothNum);//it's ok if toothnum is invalid
						}
						
						if(isValid) {
							AddQuick(procCur,listFees);
						}
						else {
							AddProcedure(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Tooth) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							AddProcedure(procCur,listFees);
						}
						else {
							procCur.ToothNum=_toothChartRelay.SelectedTeeth[n];
							AddQuick(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.ToothRange) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							AddProcedure(procCur,listFees);
						}
						else {
							procCur.ToothRange="";
							for(int b=0;b<_toothChartRelay.SelectedTeeth.Count;b++) {
								if(b!=0) procCur.ToothRange+=",";
								procCur.ToothRange+=_toothChartRelay.SelectedTeeth[b];
							}
							AddQuick(procCur,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Arch) {
						if(_toothChartRelay.SelectedTeeth.Count==0) {
							procCur.Surf=CodeMapping.GetArchSurfaceFromProcCode(ProcedureCodes.GetById(procCur.CodeNum));
							AddProcedure(procCur,listFees);
							continue;
						}
						foreach(string arch in Tooth.GetArchesForTeeth(_toothChartRelay.SelectedTeeth)) {
							Procedure proc=procCur.Copy();
							proc.Surf=arch;
							AddQuick(proc,listFees);
						}
					}
					else if(tArea==ProcedureTreatmentArea.Sextant) {
						AddProcedure(procCur,listFees);
					}
					else {//mouth
						AddQuick(procCur,listFees);
					}
					listProcCodes.Add(ProcedureCodes.GetById(procCur.CodeNum).Code);
				}//n selected teeth
				//orionProvNum=ProcCur.ProvNum;
			}//for i
			//this was requiring too many irrelevant queries and going too slowly   //ModuleSelected(PatCur.PatNum);			
			_listToothInitials=ToothInitials.Refresh(_patCur.PatNum);
			if(procButton!=null && procButton.IsMultiVisit) {
				//There are many complicated paths which might cause some of the procedures to be deleted (such as user cancel).
				//Refresh the procedures from the database to ensure the ones that we group together actually exist.
				_listChartedProcs=Procedures.GetManyProc(_listChartedProcs.Select(x => x.ProcNum).Where(x => x!=0).ToList(),false)
					.FindAll(x => x.ProcStatus!=ProcStat.D);
				ProcMultiVisits.CreateGroup(_listChartedProcs);
			}
			_listChartedProcs=null;
			ClearButtons();
			FillProgNotes();
			if(_procStatusNew==ProcStat.C) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,listProcCodes,_patCur.PatNum);
				LoadData.ListProcs=Procedures.Refresh(_patCur.PatNum);
			}
		}

		private void UpdateSurf() {
			textSurf.Text="";
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				return;
			}
			if(butM.BackColor==Color.White) {
				textSurf.AppendText("M");
			}
			if(butOI.BackColor==Color.White) {
				if(ToothGraphic.IsAnterior(_toothChartRelay.SelectedTeeth[0])) {
					textSurf.AppendText("I");
				}
				else {	
					textSurf.AppendText("O");
				}
			}
			if(butD.BackColor==Color.White) {
				textSurf.AppendText("D");
			}
			if(butV.BackColor==Color.White) {
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					textSurf.AppendText("5");
				}
				else {
					textSurf.AppendText("V");
				}
			}
			if(butBF.BackColor==Color.White) {
				if(ToothGraphic.IsAnterior(_toothChartRelay.SelectedTeeth[0])) {
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						textSurf.AppendText("V");//vestibular
					}
					else {
						textSurf.AppendText("F");
					}
				}
				else {
					textSurf.AppendText("B");
				}
			}
			if(butL.BackColor==Color.White) {
				textSurf.AppendText("L");
			}
		}

		public void UserLogOffCommited() {
			_sheetLayoutController?.UserLogOffCommited();//Can be null if user never visted the chart module.
		}
#endregion Methods - Private Tab - EnterTx

#region Methods - Private - Tab Movements
		private void FillMovementsAndHidden() {
			if(_toothChartRelay.SelectedTeeth.Count==0) {
				textShiftM.Text="";
				textShiftO.Text="";
				textShiftB.Text="";
				textRotate.Text="";
				textTipM.Text="";
				textTipB.Text="";
			}
			else {
				textShiftM.Text=
					ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[0],ToothInitialType.ShiftM).ToString();
				textShiftO.Text=
					ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[0],ToothInitialType.ShiftO).ToString();
				textShiftB.Text=
					ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[0],ToothInitialType.ShiftB).ToString();
				textRotate.Text=
					ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[0],ToothInitialType.Rotate).ToString();
				textTipM.Text=
					ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[0],ToothInitialType.TipM).ToString();
				textTipB.Text=
					ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[0],ToothInitialType.TipB).ToString();
				//At this point, all 6 blanks have either a number or 0.
				//As we go through this loop, none of the values will change.
				//The only thing that will happen is that some of them will become blank.
				string move;
				for(int i=1;i<_toothChartRelay.SelectedTeeth.Count;i++) {
					move=ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftM).ToString();
					if(textShiftM.Text!=move) {
						textShiftM.Text="";
					}
					move=ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftO).ToString();
					if(textShiftO.Text!=move) {
						textShiftO.Text="";
					}
					move=ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[i],ToothInitialType.ShiftB).ToString();
					if(textShiftB.Text!=move) {
						textShiftB.Text="";
					}
					move=ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[i],ToothInitialType.Rotate).ToString();
					if(textRotate.Text!=move) {
						textRotate.Text="";
					}
					move=ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipM).ToString();
					if(textTipM.Text!=move) {
						textTipM.Text="";
					}
					move=ToothInitials.GetMovement(_listToothInitials,_toothChartRelay.SelectedTeeth[i],ToothInitialType.TipB).ToString();
					if(textTipB.Text!=move) {
						textTipB.Text="";
					}
				}
			}
#region Hidden
			listHidden.Items.Clear();
			_arrayListHiddenTeeth=ToothInitials.GetHiddenTeeth(_listToothInitials);
			for(int i=0;i<_arrayListHiddenTeeth.Count;i++) {
				listHidden.Items.Add(Tooth.ToInternat((string)_arrayListHiddenTeeth[i]));
			}
#endregion
		}
#endregion Methods - Private - Tab Movements

#region Methods - Private - Tab Planned Appts
		private void FillPlanned() {
			if(_patCur==null) {
				//clear patient data, might be left over if login sessions changed
				gridPlanned.BeginUpdate();
				gridPlanned.Rows.Clear();
				gridPlanned.EndUpdate();
				checkDone.Checked=false;
				butNew.Enabled=false;
				butPin.Enabled=false;
				butClear.Enabled=false;
				butUp.Enabled=false;
				butDown.Enabled=false;
				gridPlanned.Enabled=false;
				return;
			}
			else {
				butNew.Enabled=true;
				butPin.Enabled=true;
				butClear.Enabled=true;
				butUp.Enabled=true;
				butDown.Enabled=true;
				gridPlanned.Enabled=true;
			}
			if(_patCur.PlannedIsDone) {
				checkDone.Checked=true;
			}
			else {
				checkDone.Checked=false;
			}
			//Fill grid
			gridPlanned.BeginUpdate();
			gridPlanned.Columns.Clear();
			GridColumn col;
			col=new GridColumn("#",25,HorizontalAlignment.Center);
			gridPlanned.Columns.Add(col);
			col=new GridColumn("Min",35);
			gridPlanned.Columns.Add(col);
			col=new GridColumn("Procedures",175);
			gridPlanned.Columns.Add(col);
			col=new GridColumn("Note",175);
			gridPlanned.Columns.Add(col);
			if(Programs.UsingOrion) {
				col=new GridColumn("SchedBy",80);
			}
			else {
				col=new GridColumn("DateSched",80);
			}
			gridPlanned.Columns.Add(col);
			gridPlanned.Rows.Clear();
			GridRow row;
			_tablePlannedAll=LoadData.TablePlannedAppts;
			//This gets done in the business layer:
			/*
			bool iochanged=false;
			for(int i=0;i<table.Rows.Count;i++) {
				if(table.Rows[i]["ItemOrder"].ToString()!=i.ToString()) {
					PlannedAppt planned=PlannedAppts.CreateObject(PIn.PLong(table.Rows[i]["PlannedApptNum"].ToString()));
					planned.ItemOrder=i;
					PlannedAppts.InsertOrUpdate(planned);
					iochanged=true;
				}
			}
			if(iochanged) {
				DataSetMain=ChartModules.GetAll(PatCur.PatNum,checkAudit.Checked);
				table=_loadData.TablePlannedAppts;
			}*/
			_listPlannedAppt=new List<DataRow>();
			for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
				if(_tablePlannedAll.Rows[i]["AptStatus"].ToString()=="2" && !checkShowCompleted.Checked) {
					continue;
				}
				_listPlannedAppt.Add(_tablePlannedAll.Rows[i]);//List containing only rows we are showing, can be the same as _tablePlannedAll
				row=new GridRow();
				row.Cells.Add((gridPlanned.Rows.Count+1).ToString());
				row.Cells.Add(_tablePlannedAll.Rows[i]["minutes"].ToString());
				row.Cells.Add(_tablePlannedAll.Rows[i]["ProcDescript"].ToString());
				row.Cells.Add(_tablePlannedAll.Rows[i]["Note"].ToString());
				if(Programs.UsingOrion) {
					string text;
					List<Procedure> listProcs=Procedures.Refresh(_patCur.PatNum);
					DateTime newDateSched=new DateTime();
					for(int p=0;p<listProcs.Count;p++) {
						if(listProcs[p].PlannedAptNum==PIn.Long(_tablePlannedAll.Rows[i]["AptNum"].ToString())) {
							OrionProc orionProc=OrionProcs.GetOneByProcNum(listProcs[p].ProcNum);
							if(orionProc!=null && orionProc.DateScheduleBy.Year>1880) {
								if(newDateSched.Year<1880) {
									newDateSched=orionProc.DateScheduleBy;
								}
								else {
									if(orionProc.DateScheduleBy<newDateSched) {
										newDateSched=orionProc.DateScheduleBy;
									}
								}
							}
						}
					}
					if(newDateSched.Year>1880) {
						text=newDateSched.ToShortDateString();
					}
					else {
						text="None";
					}
					row.Cells.Add(text);
				}
				else {//Not Orion
					ApptStatus aptStatus=(ApptStatus)(PIn.Long(_tablePlannedAll.Rows[i]["AptStatus"].ToString()));
					if(aptStatus==ApptStatus.UnschedList) {
						row.Cells.Add("Unsched");
					}
					else if(aptStatus==ApptStatus.Broken) {
						row.Cells.Add("Broken");
					}
					else {//scheduled, complete and ASAP
						row.Cells.Add(_tablePlannedAll.Rows[i]["dateSched"].ToString());
					}
				}
				row.ForeColor=Color.FromArgb(PIn.Int(_tablePlannedAll.Rows[i]["colorText"].ToString()));
				row.BackColor=Color.FromArgb(PIn.Int(_tablePlannedAll.Rows[i]["colorBackG"].ToString()));
				row.Tag=PIn.Long(_tablePlannedAll.Rows[i]["AptNum"].ToString());
				gridPlanned.Rows.Add(row);
			}
			gridPlanned.EndUpdate();
			for(int i=0;i<_listPlannedAppt.Count;i++) {
				if(_listSelectedAptNums.Contains(PIn.Long(_listPlannedAppt[i]["AptNum"].ToString()))) {
					gridPlanned.SetSelected(i,true);
				}
			}
		}
#endregion Methods - Private - Tab Planned Appts

#region Methods - Private - Tab Show
		///<summary>Searches the given row at the given column for any matching search terms in searchInput. If a match is found, the search term is removed from searchInput.</summary>
		private void CheckForSearchMatch(string columnName,DataRow rowCur,ref List<string> searchInput,bool isClinicDesc=false,bool isClinicAbbr=false) {
			for(int i=searchInput.Count-1;i>=0;--i) {
				if(isClinicAbbr) {
					if(Clinics.GetAbbr(PIn.Long((rowCur["ClinicNum"].ToString().ToLower()))).Contains(searchInput[i])) {
						searchInput.RemoveAt(i);
					}
				}
				else if(isClinicDesc) {
					if(Clinics.GetDescription(PIn.Long((rowCur["ClinicNum"].ToString().ToLower()))).Contains(searchInput[i])) {
						searchInput.RemoveAt(i);
					}
				}
				else if((rowCur[columnName].ToString().ToLower()).Contains(searchInput[i])) {
					searchInput.RemoveAt(i);
				}
			}
		}

		///<summary>Creates a new thread and searchs the progress notes for the entered search text.</summary>
		private void SearchProgNotes() {
			List<string> listSearchWords=new List<string>();
			string currentSearchText=textSearch.Text.ToLower();
			foreach(string word in textSearch.Text.ToLower().Split(' ')) {
				if(word!="") {
					listSearchWords.Add(word);
				}
			}
			_dateTimeLastSearch=DateTime.Now;
			//Create copy of list results to loop through within the search function
			List<DataRow> listSearchResults=new List<DataRow>();
			if(_listSearchResults!=null) {
				foreach(DataRow curRow in _listSearchResults) {
					DataRow row=LoadData.TableProgNotes.NewRow();
					row.ItemArray=curRow.ItemArray;
					listSearchResults.Add(row);
				}
			}
			else {
				listSearchResults=null;
			}
			ODThread thread=new ODThread((o) => SearchProgNotes(o,listSearchWords,_dateTimeLastSearch,listSearchResults,currentSearchText));
			thread.Name="SearchProgNotes";
			thread.AddExceptionHandler((e) => this.Invoke(() => FriendlyException.Show("Error searching progress notes",e)));
			thread.Start();
		}
		
		///<summary>Searches the current view of the progress notes for any search terms passed in.</summary>
		private void SearchProgNotes(ODThread o,List<string> listSearchWords,DateTime dateTimeStartSearch,List<DataRow>listSearchResults, 
			string currentSearchText) 
		{
			string previousSearchText=_previousSearchText;
			List<DisplayField> listDisFields;
			if(_chartViewCurDisplay==null) {//No chart views, Use default values.
				listDisFields=DisplayFields.GetDefaultList(DisplayFieldCategory.None);
			}
			else {
				listDisFields=DisplayFields.GetForChartView(_chartViewCurDisplay.Id);
			}
			DataTable table=new DataTable();
			//If the current search is the last search with more added on, only search the currently selected rows. Helps with speed for long progress notes.
			if(currentSearchText.StartsWith(previousSearchText) && previousSearchText!="" && listSearchResults!=null) {
				table=LoadData.TableProgNotes.Clone();
				for(int i=0;i<listSearchResults.Count;i++) {
					table.Rows.Add(listSearchResults[i].ItemArray);
				}
			}
			else {
				//Otherwise search all of the progress notes.
				table=LoadData.TableProgNotes;
			}
			listSearchResults?.Clear();
			listSearchResults=new List<DataRow>();
			foreach(DataRow rowCur in table.Rows) {
				//We are going to remove words as they are found from this list. If the list is empty, then that means it matches all the search terms.
				List<string> listSearchInput=new List<string>(listSearchWords);
				if(checkNotes.Checked) {
					CheckForSearchMatch("note",rowCur,ref listSearchInput);
				}
				for(int f=0;f<listDisFields.Count;f++) {
					switch(listDisFields[f].InternalName) {
						case DisplayFields.InternalNames.ChartView.Date:
							CheckForSearchMatch("procDate",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Time:
							CheckForSearchMatch("procTime",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Th:
							CheckForSearchMatch("toothNum",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Surf:
							CheckForSearchMatch("surf",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Dx:
							CheckForSearchMatch("dx",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Description:
							CheckForSearchMatch("description",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Stat:
							CheckForSearchMatch("procStatus",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Prov:
							CheckForSearchMatch("prov",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Amount:
							CheckForSearchMatch("procFee",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.ProcCode:
							CheckForSearchMatch("ProcCode",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.User:
							CheckForSearchMatch("user",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Signed:
							CheckForSearchMatch("signature",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Priority:
							CheckForSearchMatch("priority",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.DateEntry:
							CheckForSearchMatch("dateEntryC",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Prognosis:
							CheckForSearchMatch("prognosis",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.DateTP:
							CheckForSearchMatch("dateTP",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.EndTime:
							CheckForSearchMatch("procTimeEnd",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Quadrant:
							CheckForSearchMatch("quadrant",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.ScheduleBy:
							CheckForSearchMatch("orionDateScheduleBy",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.StopClock:
							CheckForSearchMatch("orionDateStopClock",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.DPC:
							CheckForSearchMatch("orionDPC",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.EffectiveComm:
							CheckForSearchMatch("orionIsEffectiveComm",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.OnCall:
							CheckForSearchMatch("orionIsOnCall",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Stat2:
							CheckForSearchMatch("orionStatus2",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.DPCpost:
							CheckForSearchMatch("orionDPCpost",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Length:
							CheckForSearchMatch("length",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Abbr: //abbreviation for procedures
							CheckForSearchMatch("AbbrDesc",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Locked:
							CheckForSearchMatch("isLocked",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.HL7Sent:
							CheckForSearchMatch("hl7Sent",rowCur,ref listSearchInput);
							break;
						case DisplayFields.InternalNames.ChartView.Clinic:
							CheckForSearchMatch("ClinicNum",rowCur,ref listSearchInput,isClinicAbbr:true);
							break;
						case DisplayFields.InternalNames.ChartView.ClinicDesc:
							CheckForSearchMatch("ClinicNum",rowCur,ref listSearchInput,isClinicDesc:true);
							break;
						default:
							break;
					}
					if(listSearchInput.Count==0) {//All the passed in search terms match.
						listSearchResults.Add(rowCur);
						break;
					}
				}
			}
			Thread.Sleep(200);//This is so that we don't fill the grid after the user has only typed one search character.
			if(_dateTimeLastSearch==dateTimeStartSearch) {//If the user has not typed something while the search occurred
				this.Invoke(() => {
					_previousSearchText=currentSearchText;
					_listSearchResults=listSearchResults;
					FillProgNotes(isSearch: true);
				});
			}
		}
#endregion Methods - Private - Tab Show

#region Methods - Helpers - Tab Planned
		///<summary>Sets item orders appropriately. Does not reorder list, and does not repaint/refill grid.</summary>
		private void moveItemOrderHelper(DataRow plannedAppt,int newItemOrder) {
			int plannedApptItemOrder=PIn.Int(plannedAppt["ItemOrder"].ToString());
			if(plannedApptItemOrder>newItemOrder) {//moving item up, itterate down through list
				for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
					int itemOrderCur=PIn.Int(_tablePlannedAll.Rows[i]["ItemOrder"].ToString());
					if(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString()==plannedAppt["PlannedApptNum"].ToString()) {
						_tablePlannedAll.Rows[i]["ItemOrder"]=newItemOrder;//set item order of this PlannedAppt.
						continue;
					}
					if(itemOrderCur>=newItemOrder && itemOrderCur<plannedApptItemOrder) {//all items between newItemOrder and oldItemOrder
						_tablePlannedAll.Rows[i]["ItemOrder"]=itemOrderCur+1;
					}
				}
			}
			else {//moving item down, itterate up through list
				for(int i=_tablePlannedAll.Rows.Count-1;i>=0;i--) {
					int itemOrderCur=PIn.Int(_tablePlannedAll.Rows[i]["ItemOrder"].ToString());
					if(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString()==plannedAppt["PlannedApptNum"].ToString()) {
						_tablePlannedAll.Rows[i]["ItemOrder"]=newItemOrder;//set item order of this PlannedAppt.
						continue;
					}
					if(itemOrderCur<=newItemOrder && itemOrderCur>plannedApptItemOrder) {//all items between newItemOrder and oldItemOrder
						_tablePlannedAll.Rows[i]["ItemOrder"]=itemOrderCur-1;
					}
				}
			}
			//tablePlannedAll has correct itemOrder values, which we need to copy to _listPlannedAppt without changing the actual order of _listPlannedAppt.
			for(int i=0;i<_listPlannedAppt.Count;i++) {
				for(int j=0;j<_tablePlannedAll.Rows.Count;j++) {
					if(_listPlannedAppt[i]["PlannedApptNum"].ToString()!=_tablePlannedAll.Rows[j]["PlannedApptNum"].ToString()) {
						continue;
					}
					_listPlannedAppt[i]=_tablePlannedAll.Rows[j];//update order.
				}
			}
		}

		///<summary>Updates database based on the values in _tablePlannedAll.Rows.</summary>
		private void saveChangesToDBHelper() {
			//Get all PlannedAppts from db to check for changes
			List<PlannedAppt> listPlannedAllDB=PlannedAppts.Refresh(_patCur.PatNum);
			//Itterate through current PlannedAppts list in memory and compare to db list
			for(int i=0;i<_tablePlannedAll.Rows.Count;i++) {
				//find db version of PlannedAppt to update
				PlannedAppt oldPlannedAppt=null;
				PlannedAppt plannedAppt=null;
				for(int j=0;j<listPlannedAllDB.Count;j++) {
					if(PIn.Long(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString())!=listPlannedAllDB[j].PlannedApptNum) {
						continue;//not the correct PlannedAppt
					}
					//found the correct PlannedAppt
					oldPlannedAppt=PlannedAppts.GetOne(PIn.Long(_tablePlannedAll.Rows[i]["PlannedApptNum"].ToString()));
					plannedAppt=oldPlannedAppt.Copy();
					plannedAppt.ItemOrder=PIn.Int(_tablePlannedAll.Rows[i]["ItemOrder"].ToString());
					break;//found match
				}
				if(plannedAppt==null) {//should never happen, this would mean a planned appt in our local list doesn't exist in the db
					continue;
				}
				PlannedAppts.Update(plannedAppt,oldPlannedAppt);
			}
		}
#endregion Methods - Helpers - Tab Planned

#region Helper - Classes - Public
		///<summary>Class that holds a DataRow along with the index of the table it is a part of.</summary>
		public class DataRowWithIdx {
			public DataRow Row;
			public int Index;

			public DataRowWithIdx(DataRow row,int index) {
				Row=row;
				Index=index;
			}
		}
#endregion Helper - Classes - Public

#region Methods - Inactive

		//private void Tool_EHR_Click_old(bool onLoadShowOrders) {
		//	#if EHRTEST
		//		//so we can step through for debugging.
		//	/*
		//		EhrQuarterlyKey keyThisQ=EhrQuarterlyKeys.GetKeyThisQuarter();
		//		if(keyThisQ==null) {
		//			MessageBox.Show("No quarterly key entered for this quarter.");
		//			return;
		//		}
		//		if(!((FormEHR)FormOpenDental.FormEHR).QuarterlyKeyIsValid((DateTime.Today.Year-2000).ToString(),EhrQuarterlyKeys.MonthToQuarter(DateTime.Today.Month).ToString(),
		//			Prefs.GetString(PrefName.PracticeTitle),keyThisQ.KeyValue)) {
		//			MessageBox.Show("Invalid quarterly key.");
		//			return;
		//		}
		//	*/
		//		((FormEHR)FormOpenDental.FormEHR).PatNum=PatCur.PatNum;
		//		((FormEHR)FormOpenDental.FormEHR).OnShowLaunchOrders=onLoadShowOrders;
		//		((FormEHR)FormOpenDental.FormEHR).ShowDialog();
		//		if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.None) {
		//			//return;
		//		}
		//		if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.RxEdit) {
		//			FormRxEdit FormRXE=new FormRxEdit(PatCur,RxPats.GetRx(((FormEHR)FormOpenDental.FormEHR).LaunchRxNum));
		//			FormRXE.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);//recursive.  The only way out of the loop is EhrFormResult.None.
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.RxSelect) {
		//			FormRxSelect FormRS=new FormRxSelect(PatCur);
		//			FormRS.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.Medical) {
		//			FormMedical formM=new FormMedical(PatientNoteCur,PatCur);
		//			formM.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.PatientEdit) {
		//			FormPatientEdit formP=new FormPatientEdit(PatCur,FamCur);
		//			formP.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.Online) {
		//			FormEhrOnlineAccess formO=new FormEhrOnlineAccess();
		//			formO.PatCur=PatCur;
		//			formO.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.MedReconcile) {
		//			FormMedicationReconcile FormMR=new FormMedicationReconcile();
		//			FormMR.PatCur=PatCur;
		//			FormMR.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.Referrals) {
		//			FormReferralsPatient formRP=new FormReferralsPatient();
		//			formRP.PatNum=PatCur.PatNum;
		//			formRP.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(false);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.MedicationPatEdit) {
		//			FormMedPat formMP=new FormMedPat();
		//			formMP.MedicationPatCur=MedicationPats.GetOne(((FormEHR)FormOpenDental.FormEHR).LaunchMedicationPatNum);
		//			formMP.ShowDialog();
		//			ModuleSelected(PatCur.PatNum);
		//			Tool_EHR_Click(true);
		//		}
		//		else if(((FormEHR)FormOpenDental.FormEHR).ResultOnClosing==EhrFormResult.MedicationPatNew) {
		//			//This cannot happen unless a provider is logged in with a valid ehr key
		//			FormMedications FormM=new FormMedications();
		//			FormM.IsSelectionMode=true;
		//			FormM.ShowDialog();
		//			if(FormM.DialogResult==DialogResult.OK) {
		//				Medication med=Medications.GetMedicationFromDb(FormM.SelectedMedicationNum);
		//				if(med.RxCui==0 //if the med has no Cui, it won't trigger an alert
		//					|| RxAlertL.DisplayAlerts(PatCur.PatNum,med.RxCui,0))//user sees alert and wants to continue
		//				{
		//					MedicationPat medicationPat=new MedicationPat();
		//					medicationPat.PatNum=PatCur.PatNum;
		//					medicationPat.MedicationNum=FormM.SelectedMedicationNum;
		//					medicationPat.ProvNum=Security.CurUser.ProvNum;
		//					medicationPat.DateStart=DateTime.Today;
		//					FormMedPat FormMP=new FormMedPat();
		//					FormMP.MedicationPatCur=medicationPat;
		//					FormMP.IsNew=true;
		//					FormMP.IsNewMedOrder=true;
		//					FormMP.ShowDialog();
		//					if(FormMP.DialogResult==DialogResult.OK) {
		//						ModuleSelected(PatCur.PatNum);
		//					}
		//				}
		//			}
		//			Tool_EHR_Click(true);
		//		}
		//	//#else
		//	//TODO:
		//		//Type type=FormOpenDental.AssemblyEHR.GetType("OpenDental.ObjSomeEhrSuperClass");//namespace.class
		//		object[] args;
		//		EhrQuarterlyKey keyThisQ=EhrQuarterlyKeys.GetKeyThisQuarter();
		//		if(keyThisQ==null) {
		//			MessageBox.Show("No quarterly key entered for this quarter.");
		//			return;
		//		}
		//		args=new object[] { (DateTime.Today.Year-2000).ToString(),EhrQuarterlyKeys.MonthToQuarter(DateTime.Today.Month).ToString(),
		//			Prefs.GetString(PrefName.PracticeTitle),keyThisQ.KeyValue };
		//		FormEHR Ehr = new FormEHR();
		//		Ehr.PatNum=PatCur.PatNum;
		//		Ehr.PatNotCur=PatientNoteCur;
		//		Ehr.PatFamCur=FamCur;
		//		Ehr.ShowDialog();
		//		if(!(bool)type.InvokeMember("QuarterlyKeyIsValid",System.Reflection.BindingFlags.InvokeMethod,null,FormOpenDental.ObjSomeEhrSuperClass,args)) {
		//			MessageBox.Show("Invalid quarterly key.");
		//			return;
		//		}
		//		//args=new object[] {PatCur.PatNum};
		//		//type.InvokeMember("PatNum",System.Reflection.BindingFlags.SetField,null,FormOpenDental.ObjSomeEhrSuperClass,args);
		//		//type.InvokeMember("ShowDialog",System.Reflection.BindingFlags.InvokeMethod,null,FormOpenDental.ObjSomeEhrSuperClass,null);
		//		//if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.None) {
		//		//	return;
		//		//}
		//		//if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.RxEdit) {
		//		//	long launchRxNum=(long)type.InvokeMember("LaunchRxNum",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null);
		//		//	FormRxEdit FormRXE=new FormRxEdit(PatCur,RxPats.GetRx(launchRxNum));
		//		//	FormRXE.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.RxSelect) {
		//		//	FormRxSelect FormRS=new FormRxSelect(PatCur);
		//		//	FormRS.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.Medical) {
		//		//	FormMedical formM=new FormMedical(PatientNoteCur,PatCur);
		//		//	formM.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.PatientEdit) {
		//		//	FormPatientEdit formP=new FormPatientEdit(PatCur,FamCur);
		//		//	formP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.Online) {
		//		//	FormPatientPortal formPP=new FormPatientPortal();
		//		//	formPP.PatCur=PatCur;
		//		//	formPP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.MedReconcile) {
		//		//	FormMedicationReconcile FormMR=new FormMedicationReconcile();
		//		//	FormMR.PatCur=PatCur;
		//		//	FormMR.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.Referrals) {
		//		//	FormReferralsPatient formRP=new FormReferralsPatient();
		//		//	formRP.PatNum=PatCur.PatNum;
		//		//	formRP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(false);
		//		//}
		//		//else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.MedicationPatEdit) {
		//		//	long medicationPatNum=(long)type.InvokeMember("LaunchMedicationPatNum",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null);
		//		//	FormMedPat formMP=new FormMedPat();
		//		//	formMP.MedicationPatCur=MedicationPats.GetOne(medicationPatNum);
		//		//	formMP.ShowDialog();
		//		//	ModuleSelected(PatCur.PatNum);
		//		//	Tool_EHR_Click(true);
		//		//}
		//		/*No longer allowed to create medication orders from the MedicalOrder (CPOE) window.
		//		else if(((EhrFormResult)type.InvokeMember("ResultOnClosing",System.Reflection.BindingFlags.GetField,null,FormOpenDental.ObjSomeEhrSuperClass,null))==EhrFormResult.MedicationPatNew) {
		//			//This cannot happen unless a provider is logged in with a valid ehr key
		//			FormMedications FormM=new FormMedications();
		//			FormM.IsSelectionMode=true;
		//			FormM.ShowDialog();
		//			if(FormM.DialogResult==DialogResult.OK) {
		//				Medication med=Medications.GetMedicationFromDb(FormM.SelectedMedicationNum);
		//				if(med.RxCui==0 //if the med has no Cui, it won't trigger an alert
		//					|| RxAlertL.DisplayAlerts(PatCur.PatNum,med.RxCui,0))//user sees alert and wants to continue
		//				{
		//					MedicationPat medicationPat=new MedicationPat();
		//					medicationPat.PatNum=PatCur.PatNum;
		//					medicationPat.MedicationNum=FormM.SelectedMedicationNum;
		//					medicationPat.ProvNum=Security.CurUser.ProvNum;
		//					FormMedPat FormMP=new FormMedPat();
		//					FormMP.MedicationPatCur=medicationPat;
		//					FormMP.IsNew=true;
		//					FormMP.ShowDialog();
		//					if(FormMP.DialogResult==DialogResult.OK) {
		//						ModuleSelected(PatCur.PatNum);
		//					}
		//				}
		//			}
		//			Tool_EHR_Click(true);
		//		}*/
		//	#endif
		//}

		//#region Quick Buttons (Deprecated)
		//private void panelQuickButtons_Paint(object sender,PaintEventArgs e) {

		//}

		//private void buttonCDO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "DO";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void buttonCMOD_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MOD";
		//	ProcButtonClicked(0,"D2393");
		//}

		//private void buttonCO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "O";
		//	ProcButtonClicked(0,"D2391");
		//}

		//private void buttonCMO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MO";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void butCOL_Click(object sender,EventArgs e) {
		//	textSurf.Text = "OL";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void butCOB_Click(object sender,EventArgs e) {
		//	textSurf.Text = "OB";
		//	ProcButtonClicked(0,"D2392");
		//}

		//private void butDL_Click(object sender,EventArgs e) {
		//	textSurf.Text = "DL";
		//	ProcButtonClicked(0,"D2331");
		//}

		//private void butML_Click(object sender,EventArgs e) {
		//	textSurf.Text = "ML";
		//	ProcButtonClicked(0,"D2331");
		//}

		//private void buttonCSeal_Click(object sender,EventArgs e) {
		//	textSurf.Text = "";
		//	ProcButtonClicked(0,"D1351");
		//}

		//private void buttonADO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "DO";
		//	ProcButtonClicked(0,"D2150");
		//}

		//private void buttonAMOD_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MOD";
		//	ProcButtonClicked(0,"D2160");
		//}

		//private void buttonAO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "O";
		//	ProcButtonClicked(0,"D2140");
		//}

		//private void buttonAMO_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MO";
		//	ProcButtonClicked(0,"D2150");
		//}

		//private void butCMDL_Click(object sender,EventArgs e) {
		//	textSurf.Text = "MDL";
		//	ProcButtonClicked(0,"D2332");
		//}

		//private void buttonAOL_Click(object sender, EventArgs e){
		//	textSurf.Text = "OL";
		//	ProcButtonClicked(0, "D2150");
		//}

		//private void buttonAOB_Click(object sender, EventArgs e){
		//	textSurf.Text = "OB";
		//	ProcButtonClicked(0, "D2150");
		//}

		//private void buttonAMODL_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODL";
		//	ProcButtonClicked(0, "D2161");
		//}

		//private void buttonAMODB_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODB";
		//	ProcButtonClicked(0, "D2161");
		//}

		//private void buttonCMODL_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODL";
		//	ProcButtonClicked(0, "D2394");
		//}

		//private void buttonCMODB_Click(object sender, EventArgs e){
		//	textSurf.Text = "MODB";
		//	ProcButtonClicked(0, "D2394");
		//}
		//#endregion Quick Buttons
#endregion Methods - Inactive

#region VisiQuick integration code written by Thomas Jensen tje@thomsystems.com 
		/*
		private void XrayLinkBtn_Click(object sender, System.EventArgs e)	// TJE
		{
			if (!Patients.PatIsLoaded || Patients.Cur.PatNum<1)
				return;
			VQLink.VQStart(false,"",0,0);
		}

		private void SetPanelCol(Panel p, char c)	// TJE
		{
			if (c != '0')
				p.BackColor=SystemColors.ActiveCaption;
			else
				p.BackColor=SystemColors.ActiveBorder;
		}

		private void VQUpdatePatient()	// TJE
		{
			String	s;
			if (!Patients.PatIsLoaded || Patients.Cur.PatNum<1)	
				s="";
			else
				s=VQLink.SearchTStatus(Patients.Cur.PatNum);
			if (s.Length>=32) 
			{
				SetPanelCol(tooth11,s[0]);
				SetPanelCol(tooth12,s[1]);
				SetPanelCol(tooth13,s[2]);
				SetPanelCol(tooth14,s[3]);
				SetPanelCol(tooth15,s[4]);
				SetPanelCol(tooth16,s[5]);
				SetPanelCol(tooth17,s[6]);
				SetPanelCol(tooth18,s[7]);
				SetPanelCol(tooth21,s[8]);
				SetPanelCol(tooth22,s[9]);
				SetPanelCol(tooth23,s[10]);
				SetPanelCol(tooth24,s[11]);
				SetPanelCol(tooth25,s[12]);
				SetPanelCol(tooth26,s[13]);
				SetPanelCol(tooth27,s[14]);
				SetPanelCol(tooth28,s[15]);
				SetPanelCol(tooth31,s[16]);
				SetPanelCol(tooth32,s[17]);
				SetPanelCol(tooth33,s[18]);
				SetPanelCol(tooth34,s[19]);
				SetPanelCol(tooth35,s[20]);
				SetPanelCol(tooth36,s[21]);
				SetPanelCol(tooth37,s[22]);
				SetPanelCol(tooth38,s[23]);
				SetPanelCol(tooth41,s[24]);
				SetPanelCol(tooth42,s[25]);
				SetPanelCol(tooth43,s[26]);
				SetPanelCol(tooth44,s[27]);
				SetPanelCol(tooth45,s[28]);
				SetPanelCol(tooth46,s[29]);
				SetPanelCol(tooth47,s[30]);
				SetPanelCol(tooth48,s[31]);
			}
			if (s.Length>=32+6) 
			{
				SetPanelCol(toothpanos,s[32]);
				SetPanelCol(toothcephs,s[33]);
				if (s[34]!='0' | s[35]!='0' | s[36]!='0' | s[37]!='0') 
				{
					SetPanelCol(toothbw,'1');
					SetPanelCol(toothbwfloat,'1');
				}
				else
				{
					SetPanelCol(toothbw,'0');
					SetPanelCol(toothbwfloat,'0');
				}
			}
			if (s.Length>=32+6+9) 
			{
				if (s[39]!='0' | s[40]!='0' | s[41]!='0' | s[43]!='0') 
					SetPanelCol(toothcolors,'1');
				else
					SetPanelCol(toothcolors,'0');
				SetPanelCol(toothxrays,s[42]);
				SetPanelCol(toothpanos,s[44]);
				SetPanelCol(toothcephs,s[45]);
				SetPanelCol(toothdocs,s[46]);
			}
			if (s.Length>=32+6+9+1) 
			{
				SetPanelCol(toothfiles,s[47]);
			}
		}

		private void tooth18_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos(((Panel)sender).Name.Substring(5,2),VisiQuick.spf_tinymode+VisiQuick.spf_single,0);	
		}

		private void toothbwfloat_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_tinymode+VisiQuick.spf_2horizontal,VisiQuick.spi_bitewings);
		}

		private void toothbw_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.npi_xrayview,VisiQuick.spi_bitewings);
		}

		private void toothxrays_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.VQStart(false,"",0,VisiQuick.npi_xrayview);
		}

		private void toothcolors_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.VQStart(false,"",0,VisiQuick.npi_colorview);
		}

		private void toothpanos_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_panview);
		}

		private void toothcephs_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_cephview);
		}

		private void toothdocs_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_docview);
		}

		private void toothfiles_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_fileview);
		}
		*/
#endregion

	}//end class

#region Class - ODDataRowComparer
	/// <summary>Compares two given DataRows by their associated row types and PK column.</summary>
	public class ODDataRowComparer : IEqualityComparer<DataRow> {
		public bool Equals(DataRow x,DataRow y) {
			ProgNotesRowType rowTypeX=ChartModules.GetRowType(x,out long rowXPK);
			ProgNotesRowType rowTypeY=ChartModules.GetRowType(y,out long rowYPK);
			return (rowTypeX==rowTypeY && rowXPK==rowYPK);
		}

		public int GetHashCode(DataRow obj) {
			return 0;
		}
	}
#endregion Class - ODDataRowComparer
}
