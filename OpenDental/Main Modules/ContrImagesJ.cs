#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;
using OpenDental.Thinfinity;
#endregion using

namespace OpenDental{
	///<summary>The Images Module.  See long comments at bottom of this file.</summary>
	public partial class ContrImagesJ : UserControl{
		#region Fields - Private
		///<summary>Typically just one bitmap at idx 0.  Mount image might be scaled.  Windowing (brightness/contrast) has already been applied. Does not have applied crop, flip, rotation, mount translation, global translation, or zoom.  If it's a mount, there is one for each mount position, and some can be null.</summary>
		private Bitmap[] _arrayBitmapsShowing;
		///<summary>Typically just one document at idx 0.  For mounts, there is one for each mount position, which all start out null.</summary>
		private Document[] _arrayDocumentsShowing;
		///<summary>If any bitmap gets resized for a mount, this is how we keep track of the original size.  Image proportion is the same.  Otherwise, it will just contain the same size as bitmapShowing.  Only used for mounts.</summary>
		private Size[] _arraySizesOriginal;
		///<summary>Only used when a single doc or mount item is selected.  This is the raw image that is the basis for BrightContrast adjustments.  Mount image might be scaled but it won't have any color adj.  If scaled, it's the same ratio as original.  We use width to calc scale, since height could be off by a parial pixel.</summary>
		private Bitmap _bitmapRaw;
		private Cursor _cursorPan;
		private DateTime _dateTimeMouseDownPanel;
		private Family _familyCur=null;
		//private Form _formImaging;
		///<summary>The index of the currently selected item within a mount.</summary>
		private int _idxSelectedInMount=-1;
		///<summary>The index of the previously selected item within a mount.</summary>
		private int _idxSelectedInMountOld=-1;
		private bool _initializedOnStartup=false;
		private bool _isDraggingMount=false;
		///<summary>Used to flag when filling tree and also ImagesModuleTreeIsCollapsed=2. This lets us ignore the expand and collapse commands temporarily.</summary>
		private bool _isFillingTreeWithPref;
		private bool _isMouseDownPanel;
		private bool _isMouseDownTree;
		///<summary>A list of primary defNums of the categories that should be expanded when the image module is loaded.</summary>
		private List<long> _listExpandedCats=new List<long>();
		///<summary>If a mount is currently selected, this is the list of the mount items on it.</summary>
		private List<MountItem> _listMountItems=null;
		///<summary>Keeps track of the currently selected mount object (only when a mount is selected).</summary>
		private Mount _mountShowing=null;
		///<summary>The old tree Tag that was selected for dragging purposes.  This helps determine if user is dragging a node.</summary>
		private NodeObjTag _nodeObjTagDragging;
		///<summary>The old tree Tag that was selected.  This helps determine if user is user is clicking on a tree node that was already selected.</summary>
		private NodeObjTag _nodeObjTagOld;
		///<summary>One of these 4 states is active at any time.</summary>
		private EnumCropPanAdj _cropPanEditAdj;
		private Patient _patCur=null;
		///<summary>Set with each RefreshModuleData, and that's where it's set if it doesn't yet exist.  For now, we are not using _patCur.ImageFolder, because we haven't tested whether it properly updates the patient object.  We don't want to risk using an outdated patient folder path.  And we don't want to waste time refreshing PatCur after every ImageStore.GetPatientFolder().</summary>
		private string _patFolder="";
		///<summary>Prevents too many security logs for this patient.</summary>
		private long _patNumLastSecurityLog=0;
		private long _patNumPrev=0;
		///<summary>When dragging mount item, this is the starting point of the center of the mount item where raw image will draw, in mount coordinates.</summary>
		private Point _pointDragStart;
		///<summary>When dragging mount bitmap, this is the current point of the bitmap, in mount coordinates.</summary>
		private Point _pointDragNow;
		private Point _pointMouseDown=new Point(0,0);
		///<summary>This translation is added to the bitmaps showing, based on user drags.  It's in bitmap/mount coords, not screen coords.</summary>
		private Point _pointTranslation;
		///<summary>When mouse down, this is recorded as the _pointTranslation for delta purposes while dragging.</summary>
		private Point _pointTranslationOld;
		///<summary>In panel coords.</summary>
		private Rectangle _rectangleCrop;
		///<summary>Used to display Topaz signatures on Windows.</summary>
		private Control _sigBoxTopaz;
		//private Type _typeFormImaging;
		///<summary>Tracks the last user to load ContrImages</summary>
		private long _userNumPrev=-1;
		private WebBrowser _webBrowser=null;
		///<summary>The location of the file that <see cref="_webBrowser" /> has navigated to.</summary>
		private string _webBrowserFilePath=null;
		#endregion Fields - Private

		#region Constructor
		public ContrImagesJ(){
			InitializeComponent();
			_cursorPan=new Cursor(GetType(),"CursorPalm.cur");
			panelMain.Cursor=_cursorPan;
			panelMain.MouseWheel += panelMain_MouseWheel;
			try {
				_sigBoxTopaz=TopazWrapper.GetTopaz();
				panelNote.Controls.Add(_sigBoxTopaz);
				_sigBoxTopaz.Location=sigBox.Location;//new System.Drawing.Point(437,15);
				_sigBoxTopaz.Name="sigBoxTopaz";
				_sigBoxTopaz.Size=new System.Drawing.Size(362,79);
				_sigBoxTopaz.TabIndex=93;
				_sigBoxTopaz.Text="sigPlusNET1";
				_sigBoxTopaz.DoubleClick+=new System.EventHandler(this.sigBoxTopaz_DoubleClick);
				TopazWrapper.SetTopazState(_sigBoxTopaz,0);
			}
			catch { }
		}
		#endregion Constructor

		#region Enums
		///<summary>3 States.</summary>
		private enum EnumCropPanAdj{
			///<summary>Looks like arrow. Only for docs, not mounts.</summary>
			Crop,
			///<summary>Looks like a hand.</summary>
			Pan,
			///<summary>Cursor is 4 arrows.</summary>
			Adj
		}

		///<summary>Category,Document,Mount</summary>
		public enum EnumNodeType {
			///<summary>Uses Def.</summary>
			Category,
			///<summary>Uses DocNum, Document, and ImgType.</summary>
			Document,
			///<summary>Uses MountNum and Mount.</summary>
			Mount
		}

		private enum EnumLoadBitmapType{
			///<summary>Load into _arrayBitmapsShowing[idx] for each item in mount when SelectTreeNode.</summary>
			OnlyIdx,
			///<summary>Load into _arrayBitmapsShowing[idx] and _bitmapRaw when mount image is resized. When using single images rather than mounts, this is the only enum option used, including for SelectTreeNode and cropping.</summary>
			IdxAndRaw,
			///<summary>Load image into _bitmapRaw very time user selects new mount item, but not from disk if no bright/contr yet.</summary>
			OnlyRaw
		}

		///<summary>ToolBarButton enumeration instead of strings.  For all three toolBars combined.</summary>
		private enum TB{
			Imaging,
			Print,
			Delete,
			Info,
			Sign,
			ScanDoc,
			ScanMultiDoc,
			ScanXRay,
			ScanPhoto,
			Import,
			Export,
			Copy,
			Paste,
			Forms,
			Mounts,
			ZoomOne,
			Crop,
			Pan,
			Flip,
			RotateL,
			RotateR,
			Rotate180,
			Adj,
			Size,
			Remove,
			Add
		}

		#endregion Enums

		#region Properties
		///<summary>Only used when a single document is selected, not a mount.  Gets and sets the first element in _arrayBitmapsShowing.</summary>
		private Bitmap _bitmapShowing {
			get{
				if(_arrayBitmapsShowing==null){
					return null;
				}
				if(_arrayBitmapsShowing.Length==0){
					return null;
				}
				return _arrayBitmapsShowing[0];
			}
			set {
				if(_arrayBitmapsShowing==null || _arrayBitmapsShowing.Length!=1){
					_arrayBitmapsShowing=new Bitmap[1];
				}
				_arrayBitmapsShowing[0]=value;
			}
		}

		///<summary>Only used when a single document is selected, not a mount.  Gets and sets the first element in _arrayDocumentsShowing.</summary>
		private Document _documentShowing {
			get{
				if(_arrayDocumentsShowing==null){
					return null;
				}
				if(_arrayDocumentsShowing.Length==0){
					return null;
				}
				return _arrayDocumentsShowing[0];
			}
			set {
				if(_arrayDocumentsShowing==null || _arrayDocumentsShowing.Length!=1){
					_arrayDocumentsShowing=new Document[1];
				}
				_arrayDocumentsShowing[0]=value;
			}
		}
		#endregion Properties

		#region Methods - Public
		///<summary>Refreshes list from db, then fills the treeview.  Set keepSelection to true in order to keep the current selection active.</summary>
		public void FillTree(bool keepSelection) {
			NodeObjTag nodeObjTagSelection=null;
			if(keepSelection && treeMain.SelectedNode!=null) {
				nodeObjTagSelection=(NodeObjTag)treeMain.SelectedNode.Tag;
			}
			treeMain.SelectedNode=null;
			treeMain.Nodes.Clear();
			if(_patCur==null) {
				return;
			}
			NodeObjTag nodeObjTag;
			TreeNode treeNode;
			//Category Nodes--------------------------------------------------------------------
			List<Def> listDefsImageCats=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			for(int i=0;i<listDefsImageCats.Count;i++) {
				nodeObjTag=new NodeObjTag(listDefsImageCats[i]);
				treeNode=new TreeNode();
				treeNode.Text=nodeObjTag.Text;
				treeNode.Tag=nodeObjTag;
				if(listDefsImageCats[i].ItemValue.Contains("L")) { 
					//Patient Portal Folder. This image is the only alteration in this entire module for "L" defs.
					treeNode.SelectedImageIndex=7;
					treeNode.ImageIndex=7;
				}
				else {
					treeNode.SelectedImageIndex=1;
					treeNode.ImageIndex=1;
				}
				treeMain.Nodes.Add(treeNode);
			}
			DataSet dataSet=Documents.RefreshForPatient(new string[] { _patCur.PatNum.ToString() });
			DataRowCollection rows=dataSet.Tables["DocumentList"].Rows;
			for(int i=0;i<rows.Count;i++) {
				nodeObjTag=new NodeObjTag(rows[i]);
				treeNode=new TreeNode();
				treeNode.Text=nodeObjTag.Text;
				treeNode.Tag=nodeObjTag;
				if(nodeObjTag.NodeType==EnumNodeType.Document) { 
					treeNode.ImageIndex=2+(int)nodeObjTag.ImgType;
				}
				else {//mount
					treeNode.ImageIndex=6;
				}
				treeNode.SelectedImageIndex=treeNode.ImageIndex;
				treeMain.Nodes[nodeObjTag.IdxFolder].Nodes.Add(treeNode);
				if(treeNode.Tag.Equals(nodeObjTagSelection)) {
					SelectTreeNode((NodeObjTag)treeNode.Tag);
				}
			}
			if(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)==0) {//Expand the document tree each time the Images module is visited
				treeMain.ExpandAll();//Invalidates tree too.
			}
			else if(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)==1) {//Document tree collapses when patient changes
				TreeNode treeNodeSelected=treeMain.SelectedNode;//Save the selection so we can reselect after collapsing.
				treeMain.CollapseAll();//Invalidates tree and clears selection too.
				treeMain.SelectedNode=treeNodeSelected;//This will expand any category/folder nodes necessary to show the selection.
				if(_patNumPrev==_patCur.PatNum) {//Maintain previously expanded nodes when patient not changed.
					for(int i=0;i<_listExpandedCats.Count;i++) {
						for(int j=0;j<treeMain.Nodes.Count;j++) {//Enumerate the image categories.
							if(_listExpandedCats[i]==((NodeObjTag)treeMain.Nodes[j].Tag).Def.DefNum) {
								treeMain.Nodes[j].Expand();
								break;
							}
						}
					}
				}
				else {//Patient changed.
					_listExpandedCats.Clear();
				}
				_patNumPrev=_patCur.PatNum;
			}
			else {//Document tree folders persistent expand/collapse per user
				_isFillingTreeWithPref=true;//Initialize flag so that we don't run into duplication of the UserOdPref overrides rows.
				if(_userNumPrev==Security.CurUser.UserNum) {//User has not changed.  Maintain expanded nodes.
					TreeNode selectedNode=treeMain.SelectedNode;//Save the selection so we can reselect after collapsing.
					treeMain.CollapseAll();//Invalidates tree and clears selection too.
					treeMain.SelectedNode=selectedNode;//This will expand any category/folder nodes necessary to show the selection.
					for(int i=0;i<_listExpandedCats.Count;i++) {
						for(int j=0;j<treeMain.Nodes.Count;j++) {//Enumerate the image categories.
							NodeObjTag nodeIdTagCategory=(NodeObjTag)treeMain.Nodes[j].Tag;//Get current tree document node.
							if(nodeIdTagCategory.Def.DefNum==_listExpandedCats[i]){
								treeMain.Nodes[j].Expand();
								break;
							}
						}
					}
				}
				else {//User has changed.  Expand image categories based on user preference.
					_listExpandedCats.Clear();
					List<UserOdPref> _listUserOdPrefImageCats=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.Definition);//Update override list.
					foreach(Def def in listDefsImageCats) {
						//Should only be one value with associated Fkey.
						UserOdPref userOdPrefTemp=_listUserOdPrefImageCats.FirstOrDefault(x => x.Fkey==def.DefNum);
						if(userOdPrefTemp!=null) {//User has a preference for this image category.
							if(!userOdPrefTemp.ValueString.Contains("E")) {//The user's preference is to collapse this category.
								continue;
							}
							for(int j=0;j<treeMain.Nodes.Count;j++) {//Enumerate the image categories.
								NodeObjTag nodeObjTagCategory=(NodeObjTag)treeMain.Nodes[j].Tag;//Get current tree document node.
								if(nodeObjTagCategory.Def.DefNum==userOdPrefTemp.Fkey) {
									treeMain.Nodes[j].Expand();//Expand folder.
									break;
								}
							}
						}
						else {//User doesn't have a preference for this image category.
							if(!def.ItemValue.Contains("E")) {//The default preference is to collapse this category.
								continue;
							}
							for(int j=0;j<treeMain.Nodes.Count;j++) {//Enumerate the image categories.
								NodeObjTag nodeObjTagCategory=(NodeObjTag)treeMain.Nodes[j].Tag;//Get current tree document node.
								if(nodeObjTagCategory.Def.DefNum==def.DefNum) {
									treeMain.Nodes[j].Expand();
									break;
								}
							}
						}
					}
				}
				_userNumPrev=Security.CurUser.UserNum;//Update the Previous user num.
				_isFillingTreeWithPref=false;//Disable flag
			}
			//if(XVWeb.IsDisplayingImagesInProgram && !_isFillingXVWebFromThread) {//list was already added if this is from module refresh
			//	FillTreeXVWebItems(_patCur.PatNum);
			//}
		}

		///<summary>Also does LayoutToolBars. Doesn't really do much, but we like to have one for each module.</summary>
		public void InitializeOnStartup(){
			if(_initializedOnStartup) {
				return;
			}
			_initializedOnStartup=true;
			LayoutToolBars();
		}
		
		///<summary>Layout both toolbars.</summary>
		public void LayoutToolBars() {
			toolBarMain.Buttons.Clear();
			toolBarPaint.Buttons.Clear();
			ODToolBarButton button;
			//toolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Imaging"),-1,Lan.g(this,"Open Imaging Window"),TB.Imaging));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Print"),1,Lan.G(this,"Print"),TB.Print));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Delete"),2,Lan.G(this,"Delete"),TB.Delete));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Info"),3,Lan.G(this,"Item Info"),TB.Info));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Sign"),-1,Lan.G(this,"Sign this document"),TB.Sign));
			toolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.G(this,"Scan:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			toolBarMain.Buttons.Add(button);
			toolBarMain.Buttons.Add(new ODToolBarButton("",14,Lan.G(this,"Scan Document"),TB.ScanDoc));
			toolBarMain.Buttons.Add(new ODToolBarButton("",18,Lan.G(this,"Scan Multi-Page Document"),TB.ScanMultiDoc));
			toolBarMain.Buttons.Add(new ODToolBarButton("",16,Lan.G(this,"Scan Radiograph"),TB.ScanXRay));
			toolBarMain.Buttons.Add(new ODToolBarButton("",15,Lan.G(this,"Scan Photo"),TB.ScanPhoto));
			toolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Import"),5,Lan.G(this,"Import From File"),TB.Import));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Export"),19,Lan.G(this,"Export To File"),TB.Export));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Copy"),17,Lan.G(this,"Copy displayed image to clipboard"),TB.Copy));
			toolBarMain.Buttons.Add(new ODToolBarButton(Lan.G(this,"Paste"),6,Lan.G(this,"Paste From Clipboard"),TB.Paste));
			//Forms:
			button=new ODToolBarButton(Lan.G(this,"Templates"),-1,"",TB.Forms);
			button.Style=ODToolBarButtonStyle.DropDownButton;
			menuForms=new ContextMenu();
			string formDir=FileAtoZ.CombinePaths(ImageStore.GetPreferredAtoZpath(),"Forms");
			if(CloudStorage.IsCloudStorage) {
				//Running this asynchronously to not slowdown start up time.
				ODThread odThreadTemplate=new ODThread((o) => {
					OpenDentalCloud.Core.TaskStateListFolders state=CloudStorage.ListFolderContents(formDir);
					foreach(string fileName in state.ListFolderPathsDisplay) {
						if(InvokeRequired) {
							Invoke((Action)delegate () {
								menuForms.MenuItems.Add(Path.GetFileName(fileName),new EventHandler(menuForms_Click));
							});
						}
					}
				});
				//Swallow all exceptions and allow thread to exit gracefully.
				odThreadTemplate.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { }));
				odThreadTemplate.Start(true);
			}
			else {//Not cloud
				if(Directory.Exists(formDir)) {
					DirectoryInfo dirInfo=new DirectoryInfo(formDir);
					FileInfo[] fileInfos=dirInfo.GetFiles();
					for(int i=0;i<fileInfos.Length;i++) {
						if(Documents.IsAcceptableFileName(fileInfos[i].FullName)) {
							menuForms.MenuItems.Add(fileInfos[i].Name,menuForms_Click);
						}
					}
				}
			}
			button.DropDownMenu=menuForms;
			toolBarMain.Buttons.Add(button);
			button=new ODToolBarButton(Lan.G(this,"Mounts"),-1,"",TB.Mounts);
			button.Style=ODToolBarButtonStyle.DropDownButton;
			menuMounts=new ContextMenu();
			List<MountDef> listMountDefs=MountDefs.GetDeepCopy();
			for(int i=0;i<listMountDefs.Count;i++){
				menuMounts.MenuItems.Add(listMountDefs[i].Description,menuMounts_Click);
			}
			button.DropDownMenu=menuMounts;
			toolBarMain.Buttons.Add(button);
			//button=new ODToolBarButton(Lan.g(this,"Capture"),-1,"Capture Image From Device","Capture");
			//button.Style=ODToolBarButtonStyle.ToggleButton;
			//toolBarMain.Buttons.Add(button);
			//Program links:
			ProgramL.LoadToolbar(toolBarMain,ToolBarsAvail.ImagesModule);
			//ToolbarPaint-------------------------------------------------------------------------------------
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"Zoom 1"),-1,Lan.G(this,"Zoom to One Image"),TB.ZoomOne));
			toolBarPaint.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.G(this,"Crop"),7,"",TB.Crop);
			button.Style=ODToolBarButtonStyle.ToggleButton;
			toolBarPaint.Buttons.Add(button);
			if(_cropPanEditAdj==EnumCropPanAdj.Crop){
				toolBarPaint.Buttons[TB.Crop.ToString()].Pushed=true;
			}
			button=new ODToolBarButton(Lan.G(this,"Pan"),10,"",TB.Pan);
			button.Style=ODToolBarButtonStyle.ToggleButton;
			toolBarPaint.Buttons.Add(button);
			if(_cropPanEditAdj==EnumCropPanAdj.Pan){
				toolBarPaint.Buttons[TB.Pan.ToString()].Pushed=true;
			}
			button=new ODToolBarButton(Lan.G(this,"Adj"),20,Lan.G(this,"Adjust position"),TB.Adj);
			button.Style=ODToolBarButtonStyle.ToggleButton;
			toolBarPaint.Buttons.Add(button);
			if(_cropPanEditAdj==EnumCropPanAdj.Adj){
				toolBarPaint.Buttons[TB.Adj.ToString()].Pushed=true;
			}
			toolBarPaint.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"Size"),-1,Lan.G(this,"Set Size"),TB.Size));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"Remove"),-1,Lan.G(this,"Remove Image from Mount"),TB.Remove));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"Add"),-1,Lan.G(this,"Add Copy of Existing Patient Image"),TB.Add));
			toolBarPaint.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"FlipH"),11,Lan.G(this,"Flip Horizontally"),TB.Flip));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"-90"),12,Lan.G(this,"Rotate Left"),TB.RotateL));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"+90"),13,Lan.G(this,"Rotate Right"),TB.RotateR));
			toolBarPaint.Buttons.Add(new ODToolBarButton(Lan.G(this,"180"),-1,Lan.G(this,"Rotate 180"),TB.Rotate180));
			toolBarMain.Invalidate();
			toolBarPaint.Invalidate();
			Plugins.HookAddCode(this,"ContrDocs.LayoutToolBar_end",_patCur);
		}
		
		///<summary></summary>
		public void ModuleSelected(long patNum,long docNum=0){
			try {
				RefreshModuleData(patNum);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.G(this,"Error accessing images."),ex);
			}
			RefreshModuleScreen();
			if(docNum!=0) {
				SelectTreeNode(new NodeObjTag(EnumNodeType.Document,docNum));
			}
			Plugins.HookAddCode(this,"ContrImages.ModuleSelected_end",patNum,docNum);
		}
		
		///<summary></summary>
		public void ModuleUnselected(){
			_familyCur=null;
			foreach(Control c in this.Controls) {
				if(c.GetType()==typeof(WebBrowser)) {//_webBrowserDocument
					Controls.Remove(c);
					c.Dispose();
				}
			}
			_patNumLastSecurityLog=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			Plugins.HookAddCode(this,"ContrImages.ModuleUnselected_end");
		}

		///<summary>Fired when user clicks on tree and also for automated selection that's not by mouse, such as image import, image paste, etc.  Can pass in NULL.  localPathImported will be set only if using Cloud storage and an image was imported.  We want to use the local version instead of re-downloading what was just uploaded.  nodeObjTag does not need to be same object, but must match type and priKey.</summary>
		public void SelectTreeNode(NodeObjTag nodeObjTag,string localPathImportedCloud="") {
			//Select the node always, but perform additional tasks when necessary (i.e. load an image, or mount).
			//this is redundant when user is clicking, but harmless 
			treeMain.SelectedNode=GetTreeNodeByKey(nodeObjTag);
			treeMain.Invalidate();
			if(nodeObjTag is null){	
				ClearObjects();
				if(_webBrowser!=null) {
					_webBrowser.Dispose();//Clear any previously loaded Acrobat .pdf file.
					_webBrowser=null;
				}
				return;
			}
			//if(nodeObjTag.Equals(_nodeObjTagOld)){
				//New selection is same as old selection. 
				//We always refresh here.  To handle clicking vs doubleclicking, see those event handlers.
			//}
			ClearObjects();
			_pointTranslation=new Point();
			_nodeObjTagOld=nodeObjTag.Copy();
			panelMain.Visible=true;
			if(_webBrowser!=null) {
				_webBrowser.Dispose();//Clear any previously loaded Acrobat .pdf file.
				_webBrowser=null;
			}
			if(nodeObjTag.NodeType==EnumNodeType.Document){
				_documentShowing=Documents.GetByNum(nodeObjTag.DocNum,doReturnNullIfNotFound:true);
				if(_documentShowing==null) {
					MessageBox.Show("Document was previously deleted.");
					FillTree(false);
					return;
				}
				Action actionCloseDownloadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseDownloadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.G("ContrImages","Downloading..."));
				}
				_arrayBitmapsShowing=new Bitmap[1]; 
				_arraySizesOriginal=new Size[1];
				LoadBitmap(0,EnumLoadBitmapType.IdxAndRaw);
				//_bitmapRaw will always be null for PDFs
				//Diverges slightly from the normal use of this event, in that it is fired from SelectTreeNode() rather than ModuleSelected.  Appropriate
				//here because this is the only data in ContrImages that might affect the PatientDashboard, and there is no "LoadData" in this Module.
				PatientDashboardDataEvent.Fire(ODEventType.ModuleSelected
					,new PatientDashboardDataEventArgs() {
						Pat=_patCur,
						ListDocuments=_arrayDocumentsShowing.ToList(),
						BitmapImagesModule=_bitmapRaw,
					}
				);
				actionCloseDownloadProgress?.Invoke();
				bool isExportable=panelMain.Visible;
				if(_bitmapRaw==null) {
					if(ImageHelper.HasImageExtension(_documentShowing.FileName)) {
						string srcFileName = ODFileUtils.CombinePaths(_patFolder,_documentShowing.FileName);
						if(File.Exists(srcFileName)) {
							MessageBox.Show(Lan.G(this,"File found but cannot be opened, probably because it's too big:")+srcFileName);
						}
						else {
							MessageBox.Show(Lan.G(this,"File not found")+": " + srcFileName);
						}
					}
					else if(Path.GetExtension(_documentShowing.FileName).ToLower()==".pdf") {//Adobe acrobat file.
						LoadPdf(_patFolder,_documentShowing.FileName,localPathImportedCloud,ref isExportable,"Downloading Document...");
					}
				}
				SetWindowingSlider();
				//In Web mode the buttons do not appear when hovering over the PDF, so we need to enable the print toolbar button.
				bool doShowPrint=panelMain.Visible;
				EnableToolBarButtons(print:doShowPrint, delete:true, info:true, sign:true, export:isExportable, copy:panelMain.Visible, brightAndContrast:panelMain.Visible, zoom:panelMain.Visible, zoomOne:false, crop:panelMain.Visible, pan:panelMain.Visible, adj:false, size:false, remove:false, add:false, flip:panelMain.Visible, rotateL:panelMain.Visible, rotateR:panelMain.Visible, rotate180:panelMain.Visible);
				SetCropPanEditAdj(EnumCropPanAdj.Pan);
			}
			if(nodeObjTag.NodeType==EnumNodeType.Mount){
				_mountShowing=Mounts.GetByNum(nodeObjTag.MountNum);
				_listMountItems=MountItems.GetItemsForMount(_mountShowing.MountNum);
				_arrayDocumentsShowing=Documents.GetDocumentsForMountItems(_listMountItems);
				_idxSelectedInMount=-1;//No selection to start.
				_idxSelectedInMountOld=-1;
				Action actionCloseDownloadProgress=null;
				if(CloudStorage.IsCloudStorage) {
					actionCloseDownloadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.G("ContrImages","Downloading..."));
				}
				else{
					Cursor=Cursors.WaitCursor;
				}
				//_arrayBitmapsRaw=ImageStore.OpenImages(_arrayDocumentsShowing,_patFolder,localPathImportedCloud);
				_arrayBitmapsShowing=new Bitmap[_arrayDocumentsShowing.Length]; 
				_arraySizesOriginal=new Size[_arrayDocumentsShowing.Length];
				List<int> listMissingMountNums=new List<int>();//List to count missing images not found in the A-Z folder
				for(int i=0;i<_arrayDocumentsShowing.Length;i++){
					if(_arrayDocumentsShowing[i]==null){
						_arrayBitmapsShowing[i]=null;
						continue;
					}
					LoadBitmap(i,EnumLoadBitmapType.OnlyIdx);
					if(_arrayBitmapsShowing[i]==null) {
						listMissingMountNums.Add(i);
					}
				}
				if(listMissingMountNums.Count>0) {//Notify user of any files that were unable to load
					string errorMessage=Lan.G(this,"Files not found for mount:")+"\r\n";
					for(int m=0;m<listMissingMountNums.Count;m++) {
						errorMessage+=Lan.G(this,"Mount position ")+(listMissingMountNums[m]+1)+": "+_arrayDocumentsShowing[listMissingMountNums[m]].FileName;
						if(m!=listMissingMountNums.Count-1) {
							errorMessage+="\r\n";
						}
					}
					MsgBox.Show(errorMessage);
				}
				actionCloseDownloadProgress?.Invoke();	
				EnableToolBarButtonsMount();
				SetCropPanEditAdj(EnumCropPanAdj.Pan);
				Cursor=Cursors.Default;
			}
			SetPanelNoteVisibility();
			LayoutAll();
			SetZoomSlider();
			FillSignature();
			//InvalidateSettingsColor();
			panelMain.Invalidate();
		}

		#endregion Methods - Public

		#region Methods - Event Handlers
		private void ContrImages_Resize(object sender, EventArgs e){
			LayoutAll();
		}

		private void menuForms_Click(object sender,System.EventArgs e) {
			string formName=((MenuItem)sender).Text;
			Document doc;
			try {
				doc=ImageStore.ImportForm(formName,GetCurrentCategory(),_patCur);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			FillTree(false);
			SelectTreeNode(new NodeObjTag(EnumNodeType.Document,doc.DocNum));
			FormDocInfo FormD=new FormDocInfo(_patCur,doc);
			FormD.ShowDialog(this);//some of the fields might get changed, but not the filename
			if(FormD.DialogResult!=DialogResult.OK) {
				DeleteDocument(false,false,doc);
			}
			else {
				FillTree(true);//Refresh possible changes in the document due to FormD.
			}	
		}

		private void menuMounts_Click(object sender,System.EventArgs e) {
			int idx = ((MenuItem)sender).Index;
			List<MountDef> listMountDefs=MountDefs.GetDeepCopy();
			//MsgBox.Show(listMountDefs[idx].Description);
			Mount mount=Mounts.CreateMountFromDef(listMountDefs[idx],_patCur.PatNum,GetCurrentCategory());
			FillTree(false);
			SelectTreeNode(new NodeObjTag(EnumNodeType.Mount,mount.MountNum));
		}

		private void menuTree_Click(object sender,System.EventArgs e) {
			if(treeMain.SelectedNode==null) {
				return;//Probably the user has no patient selected
			}
			//Categories already blocked at the click
			switch(((MenuItem)sender).Index) {
				case 0://print
					ToolBarPrint_Click();
					break;
				case 1://delete
					ToolBarDelete_Click();
					break;
				case 2://info
					ToolBarInfo_Click();
					break;
			}
		}

		private void panelMain_MouseDown(object sender, MouseEventArgs e){
			_pointMouseDown=new Point(e.X,e.Y);
			_isMouseDownPanel=true;
			_dateTimeMouseDownPanel=DateTime.Now;
			_pointTranslationOld=_pointTranslation;
			if(IsMountShowing()){
				//if(_panCropMount==EnumPanCropMount.Pan/Edit){
					//handled on mouse up
				//}
				if(_cropPanEditAdj==EnumCropPanAdj.Adj){
					Point pointRaw=ControlPointToBitmapPoint(e.Location);
					_idxSelectedInMount=-1;
					for(int i=0;i<_listMountItems.Count;i++){
						Rectangle rect=new Rectangle(_listMountItems[i].Xpos,_listMountItems[i].Ypos,_listMountItems[i].Width,_listMountItems[i].Height);
						if(rect.Contains(pointRaw)){
							_idxSelectedInMount=i;
						}
					}
					if(_idxSelectedInMount!=_idxSelectedInMountOld){
						LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.OnlyRaw);
						_idxSelectedInMountOld=_idxSelectedInMount;
					}
					EnableToolBarButtonsMount();
					if(_idxSelectedInMount>-1){//individual image
						if(_arrayDocumentsShowing[_idxSelectedInMount]!=null){
							_isDraggingMount=true;
							//float scaleFactor=zoomSlider.Value/100f;
							//To handle rotation, crop, etc, always measure from center of mount position.  Crop is not involved until mouse up.
							_pointDragNow=new Point(_listMountItems[_idxSelectedInMount].Xpos+_listMountItems[_idxSelectedInMount].Width/2,
								_listMountItems[_idxSelectedInMount].Ypos+_listMountItems[_idxSelectedInMount].Height/2);
							_pointDragStart=_pointDragNow;
						}
					}
					panelMain.Invalidate();
				}
			}	
		}

		private void panelMain_MouseMove(object sender, MouseEventArgs e){
			if(!_isMouseDownPanel) {
				return;
			}
			if(treeMain.SelectedNode==null) {
				return;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType==EnumNodeType.Category) {
				return;
			}
			float scaleTrans=(float)100/zoomSlider.Value;//example, 200%, 100/200=.5, indicating .5 bitmap pixels for each screen pixel
			if(_cropPanEditAdj==EnumCropPanAdj.Crop) {
				Rectangle rectangle1=new Rectangle(Math.Min(e.Location.X,_pointMouseDown.X), Math.Min(e.Location.Y,_pointMouseDown.Y),
					Math.Abs(e.Location.X-_pointMouseDown.X),	Math.Abs(e.Location.Y-_pointMouseDown.Y));
				Rectangle rectangle2=new Rectangle(0,0,panelMain.Width-1,panelMain.Height-1);
				_rectangleCrop=Rectangle.Intersect(rectangle1,rectangle2);
				panelMain.Invalidate();
			}
			if(_cropPanEditAdj==EnumCropPanAdj.Pan){
				//scaleTrans=1;
				int xTrans=(int)(_pointTranslationOld.X+(e.Location.X-_pointMouseDown.X)*scaleTrans);
				int yTrans=(int)(_pointTranslationOld.Y+(e.Location.Y-_pointMouseDown.Y)*scaleTrans);
				_pointTranslation=new Point(xTrans,yTrans);
				panelMain.Invalidate();
			}
			if(_cropPanEditAdj==EnumCropPanAdj.Adj){
				if(_idxSelectedInMount==-1 || _arrayDocumentsShowing[_idxSelectedInMount]==null){
					return;
				}
				int xTrans=(int)(_pointDragStart.X+(ControlPointToBitmapPoint(e.Location).X-ControlPointToBitmapPoint(_pointMouseDown).X));
				int yTrans=(int)(_pointDragStart.Y+(ControlPointToBitmapPoint(e.Location).Y-ControlPointToBitmapPoint(_pointMouseDown).Y));
				_pointDragNow=new Point(xTrans,yTrans);
				//Rectangle rectClip=new Rectangle(0,0,200,200);
				//panelMain.Invalidate(rectClip);
				panelMain.Invalidate();
			}
		}

		private void panelMain_MouseUp(object sender, MouseEventArgs e){
			_isMouseDownPanel=false;
			if(treeMain.SelectedNode==null) {
				return;
			}
			NodeObjTag nodeObjTag=(NodeObjTag)treeMain.SelectedNode.Tag;
			if(nodeObjTag.NodeType==EnumNodeType.Category) {
				return;
			}
			bool isClick=false;
			if(Math.Abs(e.Location.X-_pointMouseDown.X) <3 
				&& Math.Abs(e.Location.Y-_pointMouseDown.Y) <3
				&& _dateTimeMouseDownPanel.AddMilliseconds(600) > DateTime.Now)//anything longer than a 600 ms mouse down becomes a drag
			{
				isClick=true;
			}
			if(nodeObjTag.NodeType==EnumNodeType.Mount && isClick	&& _cropPanEditAdj==EnumCropPanAdj.Pan){//Adj is handled on mouse down 
				_idxSelectedInMount=HitTestInMount(e.X,e.Y);
				if(_idxSelectedInMount!=_idxSelectedInMountOld){
					LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.OnlyRaw);
					_idxSelectedInMountOld=_idxSelectedInMount;
				}
				EnableToolBarButtonsMount();
				_isDraggingMount=false;
				panelMain.Invalidate();
				SetWindowingSlider();
				return;
			}
			if(nodeObjTag.NodeType==EnumNodeType.Mount && !isClick && _cropPanEditAdj==EnumCropPanAdj.Adj && _isDraggingMount){
				SetWindowingSlider();
				//Calc where the center of the dragged image is, in mount coords, assuming it was same size as mount rect.
				//This won't work perfectly if the mount has gaps between items, so we might need to add a supplemental check for that. Or not.
				//Point pointCenter=new Point(_pointDragNow.X+_listMountItems[_idxSelectedInMount].Width/2,_pointDragNow.Y+_listMountItems[_idxSelectedInMount].Height/2);
				int idxNewPos=HitTestInMount(_pointDragNow.X,_pointDragNow.Y,true);//pointCenter.X,pointCenter.Y);
				if(idxNewPos==-1){
					_isDraggingMount=false;
					panelMain.Invalidate();
					return;
				}
				Document docOld=_arrayDocumentsShowing[_idxSelectedInMount].Copy();
				if(idxNewPos!=_idxSelectedInMount && _arrayDocumentsShowing[idxNewPos]!=null){//don't allow dragging to an occupied spot
					_isDraggingMount=false;
					panelMain.Invalidate();
					return;
				}
				//calculate the distance of drag that happened, in MOUNT coords.
				int xDrag=ControlPointToBitmapPoint(e.Location).X-ControlPointToBitmapPoint(_pointMouseDown).X;
				int yDrag=ControlPointToBitmapPoint(e.Location).Y-ControlPointToBitmapPoint(_pointMouseDown).Y;
				if(idxNewPos!=_idxSelectedInMount){
					bool movedPosNoCrop=false;//moving to a new mount position, but no crop was specified, then don't add one
					_arrayDocumentsShowing[_idxSelectedInMount].MountItemNum=_listMountItems[idxNewPos].MountItemNum;
					if(_arrayDocumentsShowing[_idxSelectedInMount].CropW==0 || _arrayDocumentsShowing[_idxSelectedInMount].CropH==0){
						movedPosNoCrop=true;
					}
					Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],docOld);
					_arrayBitmapsShowing[idxNewPos]=_arrayBitmapsShowing[_idxSelectedInMount];
					_arrayBitmapsShowing[_idxSelectedInMount]=null;
					_arrayDocumentsShowing[idxNewPos]=_arrayDocumentsShowing[_idxSelectedInMount];
					_arrayDocumentsShowing[_idxSelectedInMount]=null;
					//reference point for drag needs to be relative to new mountitem instead of old mountitem, or crop xy gets set wrong
					xDrag+=_listMountItems[_idxSelectedInMount].Xpos+_listMountItems[_idxSelectedInMount].Width/2
						-(_listMountItems[idxNewPos].Xpos+_listMountItems[idxNewPos].Width/2);
					yDrag+=_listMountItems[_idxSelectedInMount].Ypos+_listMountItems[_idxSelectedInMount].Height/2
						-(_listMountItems[idxNewPos].Ypos+_listMountItems[idxNewPos].Height/2);
					_idxSelectedInMount=idxNewPos;
					if(movedPosNoCrop){//not adding crop, so nothing else to do
						_isDraggingMount=false;
						panelMain.Invalidate();
						return;
					}
					//It's now in new idx, but we still need to fix the crop below
				}
				//if no existing crop, we make one.
				if(_arrayDocumentsShowing[_idxSelectedInMount].CropW==0 || _arrayDocumentsShowing[_idxSelectedInMount].CropH==0){
					float scaleItem=ZoomSlider.CalcScaleFit(new Size(_listMountItems[_idxSelectedInMount].Width, _listMountItems[_idxSelectedInMount].Height), _arraySizesOriginal[_idxSelectedInMount], _arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated);
					if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated.In(0,180)){
						_arrayDocumentsShowing[_idxSelectedInMount].CropX=-((int)(_listMountItems[_idxSelectedInMount].Width/scaleItem)
							-_arraySizesOriginal[_idxSelectedInMount].Width)/2;//neg or 0
						_arrayDocumentsShowing[_idxSelectedInMount].CropY=-((int)(_listMountItems[_idxSelectedInMount].Height/scaleItem)
							-_arraySizesOriginal[_idxSelectedInMount].Height)/2;
						_arrayDocumentsShowing[_idxSelectedInMount].CropW=_arraySizesOriginal[_idxSelectedInMount].Width//in bitmap pixels
							+(int)(_listMountItems[_idxSelectedInMount].Width/scaleItem)-_arraySizesOriginal[_idxSelectedInMount].Width;//pos or 0
						_arrayDocumentsShowing[_idxSelectedInMount].CropH=_arraySizesOriginal[_idxSelectedInMount].Height
							+(int)(_listMountItems[_idxSelectedInMount].Height/scaleItem)-_arraySizesOriginal[_idxSelectedInMount].Height;
					}
					else{//90 or 270
						_arrayDocumentsShowing[_idxSelectedInMount].CropX=-((int)(_listMountItems[_idxSelectedInMount].Height/scaleItem)
							-_arraySizesOriginal[_idxSelectedInMount].Width)/2;
						_arrayDocumentsShowing[_idxSelectedInMount].CropY=-((int)(_listMountItems[_idxSelectedInMount].Width/scaleItem)
							-_arraySizesOriginal[_idxSelectedInMount].Height)/2;
						_arrayDocumentsShowing[_idxSelectedInMount].CropW=_arraySizesOriginal[_idxSelectedInMount].Width
							+(int)(_listMountItems[_idxSelectedInMount].Height/scaleItem)-_arraySizesOriginal[_idxSelectedInMount].Width;
						_arrayDocumentsShowing[_idxSelectedInMount].CropH=_arraySizesOriginal[_idxSelectedInMount].Height
							+(int)(_listMountItems[_idxSelectedInMount].Width/scaleItem)-_arraySizesOriginal[_idxSelectedInMount].Height;
					}

				}
				//now, we have a crop to work with for all items,
				if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated==0){
					float scaleItem=(float)_listMountItems[_idxSelectedInMount].Width/_arrayDocumentsShowing[_idxSelectedInMount].CropW;
					_arrayDocumentsShowing[_idxSelectedInMount].CropX-=(int)(xDrag/scaleItem);
					_arrayDocumentsShowing[_idxSelectedInMount].CropY-=(int)(yDrag/scaleItem);
				}
				if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated==180){
					float scaleItem=(float)_listMountItems[_idxSelectedInMount].Width/_arrayDocumentsShowing[_idxSelectedInMount].CropW;
					_arrayDocumentsShowing[_idxSelectedInMount].CropX+=(int)(xDrag/scaleItem);
					_arrayDocumentsShowing[_idxSelectedInMount].CropY+=(int)(yDrag/scaleItem);
				}
				if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated==90){
					float scaleItem=(float)_listMountItems[_idxSelectedInMount].Height/_arrayDocumentsShowing[_idxSelectedInMount].CropW;
					_arrayDocumentsShowing[_idxSelectedInMount].CropX-=(int)(yDrag/scaleItem);
					_arrayDocumentsShowing[_idxSelectedInMount].CropY+=(int)(xDrag/scaleItem);
				}
				if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated==270){
					float scaleItem=(float)_listMountItems[_idxSelectedInMount].Height/_arrayDocumentsShowing[_idxSelectedInMount].CropW;
					_arrayDocumentsShowing[_idxSelectedInMount].CropX+=(int)(yDrag/scaleItem);
					_arrayDocumentsShowing[_idxSelectedInMount].CropY-=(int)(xDrag/scaleItem);
				}
				//make sure the crop area falls within the image
				Rectangle rectangleBitmap=new Rectangle(0,0,_arraySizesOriginal[_idxSelectedInMount].Width,_arraySizesOriginal[_idxSelectedInMount].Height);
				Rectangle rectangleCrop=new Rectangle(_arrayDocumentsShowing[_idxSelectedInMount].CropX,_arrayDocumentsShowing[_idxSelectedInMount].CropY,
					_arrayDocumentsShowing[_idxSelectedInMount].CropW,_arrayDocumentsShowing[_idxSelectedInMount].CropH);
				if(!rectangleBitmap.IntersectsWith(rectangleCrop)){
					_arrayDocumentsShowing[_idxSelectedInMount].CropX=0;
					_arrayDocumentsShowing[_idxSelectedInMount].CropY=0;
					_arrayDocumentsShowing[_idxSelectedInMount].CropW=0;
					_arrayDocumentsShowing[_idxSelectedInMount].CropH=0;
				}
				Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],docOld);
				_isDraggingMount=false;
				panelMain.Invalidate();
				return;
			}
			if(nodeObjTag.NodeType==EnumNodeType.Mount && _cropPanEditAdj==EnumCropPanAdj.Adj){
				//catches rest of situations not addressed above
				_isDraggingMount=false;
				SetWindowingSlider();
				panelMain.Invalidate();
			}
			if(_cropPanEditAdj==EnumCropPanAdj.Crop){
				if(_rectangleCrop.Width<=0 || _rectangleCrop.Height<=0) {
					return;
				}
				//these two rectangles are in bitmap coords rather then screen
				Rectangle rectangleBitmap;
				if(_documentShowing.CropW>0 && _documentShowing.CropH>0){
					rectangleBitmap=new Rectangle(_documentShowing.CropX,_documentShowing.CropY,_documentShowing.CropW,_documentShowing.CropH);
				}
				else{
					rectangleBitmap=new Rectangle(0,0,_bitmapShowing.Width,_bitmapShowing.Height);
				}
				//need to untangle this rect after running it through the matrices
				Point pointCrop1=ControlPointToBitmapPoint(new Point(_rectangleCrop.X,_rectangleCrop.Y));
				Point pointCrop2=ControlPointToBitmapPoint(new Point(_rectangleCrop.X+_rectangleCrop.Width,_rectangleCrop.Y+_rectangleCrop.Height));
				Rectangle rectangleCrop=new Rectangle(Math.Min(pointCrop1.X,pointCrop2.X), Math.Min(pointCrop1.Y,pointCrop2.Y),
					Math.Abs(pointCrop1.X-pointCrop2.X),	Math.Abs(pointCrop1.Y-pointCrop2.Y));
				rectangleCrop=Rectangle.Intersect(rectangleBitmap,rectangleCrop);
				if(rectangleCrop==new Rectangle()){//crop is entirely outside image
					_rectangleCrop=new Rectangle();
					panelMain.Invalidate();
					return;
				}
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Crop to Rectangle?")) {
					_rectangleCrop=new Rectangle();
					panelMain.Invalidate();
					return;
				}
				//MessageBox.Show(rectangleCrop.ToString());	
				Document docOld=_documentShowing.Copy();
				_documentShowing.CropX=rectangleCrop.X;
				_documentShowing.CropY=rectangleCrop.Y;
				_documentShowing.CropW=rectangleCrop.Width;
				_documentShowing.CropH=rectangleCrop.Height;
				Documents.Update(_documentShowing,docOld);
				ImageStore.DeleteThumbnailImage(_documentShowing,_patFolder);
				LoadBitmap(0,EnumLoadBitmapType.IdxAndRaw);
				SetZoomSlider();
				_pointTranslation=new Point();
				_rectangleCrop=new Rectangle();
				panelMain.Invalidate();
			}
		}

		private void panelMain_Paint(object sender, PaintEventArgs e){
			//consider original image, crop, and zoom
			Graphics g=e.Graphics;//alias
			g.Clear(Color.White);
			if(_documentShowing==null && _mountShowing==null){
				return;
			}
			//Center screen:
			g.TranslateTransform(panelMain.Width/2,panelMain.Height/2);
			//because of order, scaling is center of panel instead of center of image.
			float scaleFactor=zoomSlider.Value/100f;
			g.ScaleTransform(scaleFactor,scaleFactor);
			//and the user translation must be in image coords rather than panel coords
			g.TranslateTransform(_pointTranslation.X,_pointTranslation.Y);
			try{
				if(IsDocumentShowing()){
					DrawDocument(g);
				}
				if(IsMountShowing()){
					DrawMount(g);
				}
			}
			catch{
				//one of the objects above might get damaged in the middle of a paint.
			}
		}

		private void panelMain_MouseWheel(object sender, MouseEventArgs e){
			if(treeMain.SelectedNode==null) {
				return;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType==EnumNodeType.Category) {
				return;
			}
			if(_cropPanEditAdj==EnumCropPanAdj.Pan){
				float deltaZoom=zoomSlider.Value*(float)e.Delta/SystemInformation.MouseWheelScrollDelta/8f;//For example, -15
				zoomSlider.SetByWheel(deltaZoom);
				panelMain.Invalidate();
			}
			if(_cropPanEditAdj==EnumCropPanAdj.Adj){
				//no
			}
		}

		private void printDocument_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Graphics g=e.Graphics;
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;
			g.CompositingQuality=CompositingQuality.HighQuality;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			Bitmap bitmapPrint=null;
			if(IsDocumentShowing()){
				bitmapPrint=(Bitmap)_bitmapShowing.Clone();
			}
			if(IsMountShowing()){
				if(_idxSelectedInMount>-1 && _arrayDocumentsShowing[_idxSelectedInMount]!=null) {
					bitmapPrint=ImageHelper.ApplyDocumentSettingsToImage2(_arrayDocumentsShowing[_idxSelectedInMount],_arrayBitmapsShowing[_idxSelectedInMount],
						ImageSettingFlags.CROP | ImageSettingFlags.FLIP | ImageSettingFlags.ROTATE);

				}
				else {//Entire mount needs different drawing strategy
					g.TranslateTransform(e.MarginBounds.X+e.MarginBounds.Width/2,e.MarginBounds.Y+e.MarginBounds.Height/2);//Center of page
					bool isWide=false;
					if((double)_mountShowing.Width/_mountShowing.Height > (double)e.MarginBounds.Width/e.MarginBounds.Height){
						isWide=true;
					}
					if(isWide){//print landscape
						g.RotateTransform(90);
						float scale=ZoomSlider.CalcScaleFit(new Size(e.MarginBounds.Height,e.MarginBounds.Width),new Size(_mountShowing.Width,_mountShowing.Height),0);
						g.ScaleTransform(scale,scale);
					}
					else{
						float scale=ZoomSlider.CalcScaleFit(e.MarginBounds.Size,new Size(_mountShowing.Width,_mountShowing.Height),0);
						g.ScaleTransform(scale,scale);
					}
					DrawMount(g);
					e.HasMorePages=false;
					return;
				}
			}
			double ratio=Math.Min((double)e.MarginBounds.Width/bitmapPrint.Width,(double)e.MarginBounds.Height/bitmapPrint.Height);
			g.DrawImage(bitmapPrint,e.MarginBounds.X,e.MarginBounds.Y,(int)(bitmapPrint.Width*ratio),(int)(bitmapPrint.Height*ratio));
			bitmapPrint?.Dispose();
			e.HasMorePages=false;
		}

		private void timerTreeClick_Tick(object sender, EventArgs e){
			//this timer starts when user clicks on a treenode that's already selected.
			//It gets cancelled if the click turns into a double click.
			//This might cause a problem if we add a context menu
			if(treeMain.SelectedNode==null){
				return;
			}
			NodeObjTag nodeObjTag=(NodeObjTag)treeMain.SelectedNode.Tag;
			timerTreeClick.Enabled=false;//only fire once
			SelectTreeNode(nodeObjTag);
		}

		private void toolBarMain_ButtonClick(object sender, ODToolBarButtonClickEventArgs e){
			if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,_patCur);
				return;
			}
			if(e.Button.Tag.GetType()!=typeof(TB)) {
				throw new Exception("Bad Tag");
			}
			switch((TB)e.Button.Tag) {
				case TB.Imaging:/* not currently visible
					if(_formImaging==null || _formImaging.IsDisposed){
						//bool isPresent=File.Exists("OpenDentalImaging.exe");
						Assembly assemblyImaging=Assembly.Load("OpenDentalImaging");
						_typeFormImaging=assemblyImaging.GetType("OpenDentalImaging.FormImaging");
						_formImaging=(Form)(Activator.CreateInstance(_typeFormImaging));
						_formImaging.Show();
					}
					_formImaging.Activate();
					_formImaging.BringToFront();
					MethodInfo methodInfoModuleSelected=_typeFormImaging.GetMethod("ModuleSelected");
					object[] parameters = new object[1];
					parameters[0] = _patCur.PatNum;
					methodInfoModuleSelected.Invoke(_formImaging,parameters);*/
					break;
				case TB.Print:
					//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
					//when it comes from a toolbar click.
					//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
					//ToolBarClick toolClick=ToolBarPrint_Click;
					this.BeginInvoke(ToolBarPrint_Click);//toolClick);
					break;
				case TB.Delete:
					ToolBarDelete_Click();
					break;
				case TB.Info:
					ToolBarInfo_Click();
					break;
				case TB.Sign:
					ToolBarSign_Click();
					break;
				case TB.ScanDoc:
					ToolBarScan_Click("doc");
					break;
				case TB.ScanMultiDoc:
					ToolBarScanMulti_Click();
					break;
				case TB.ScanXRay:
					ToolBarScan_Click("xray");
					break;
				case TB.ScanPhoto:
					ToolBarScan_Click("photo");
					break;
				case TB.Import://import is always enabled
					ToolBarImport_Click();
					break;
				case TB.Export:
					ToolBarExport_Click();
					break;
				case TB.Copy:
					ToolBarCopy_Click();
					break;
				case TB.Paste:
					ToolBarPaste_Click();
					break;
				case TB.Forms:
					MessageBox.Show("Use the dropdown list.  Add forms to the list by copying image files into your A-Z folder, Forms.  Restart the program to see newly added forms.");
					break;
				case TB.Mounts:
					MessageBox.Show("Use the dropdown list.  Manage Mounts from the Setup/Images menu.");
					break;
			}
		}

		private void toolBarPaint_ButtonClick(object sender, ODToolBarButtonClickEventArgs e){
			if(e.Button.Tag.GetType()!=typeof(TB)) {
				return;
			}
			switch((TB)e.Button.Tag) {
				case TB.ZoomOne:
					ToolBarZoomOne_Click();
					break;
				case TB.Crop:
					SetCropPanEditAdj(EnumCropPanAdj.Crop);
					break;
				case TB.Pan:
					SetCropPanEditAdj(EnumCropPanAdj.Pan);
					//if(IsMountShowing()){
						//_idxSelectedInMount=-1;
						//_idxSelectedInMountOld=-1;
						//EnableToolBarButtonsMount();
						//panelMain.Invalidate();
					//}
					break;
				case TB.Adj:
					SetCropPanEditAdj(EnumCropPanAdj.Adj);
					break;
				case TB.Size:
					ToolBarSize_Click();
					break;
				case TB.Remove:
					ToolBarRemove_Click();
					break;
				case TB.Add:
					ToolBarAdd_Click();
					break;
				case TB.Flip:
					ToolBarFlip_Click();
					break;
				case TB.RotateL:
					ToolBarRotateL_Click();
					break;
				case TB.RotateR:
					ToolBarRotateR_Click();
					break;
				case TB.Rotate180:
					ToolBarRotate180_Click();
					break;
			}
		}

		private void TreeMain_AfterCollapse(object sender,TreeViewEventArgs e) {
			NodeObjTag nodeObjTag=(NodeObjTag)e.Node.Tag;
			_listExpandedCats.RemoveAll(x => x==nodeObjTag.Def.DefNum);
			UpdateUserOdPrefForImageCat(nodeObjTag.Def.DefNum,false);
		}

		private void TreeMain_AfterExpand(object sender,TreeViewEventArgs e) {
			NodeObjTag nodeObjTag=(NodeObjTag)e.Node.Tag;
			if(!_listExpandedCats.Contains(nodeObjTag.Def.DefNum)) {
				_listExpandedCats.Add(nodeObjTag.Def.DefNum);
			}
			UpdateUserOdPrefForImageCat(nodeObjTag.Def.DefNum,true);
		}

		private void treeMain_DragDrop(object sender,DragEventArgs e) {
			TreeNode treeNodeOver=treeMain.GetNodeAt(treeMain.PointToClient(Cursor.Position));
			if(treeNodeOver==null) {
				return;
			}
			NodeObjTag nodeObjTagOver=(NodeObjTag)treeNodeOver.Tag;
			long nodeOverCatDefNum=0;
			if(nodeObjTagOver.NodeType==EnumNodeType.Category) {
				nodeOverCatDefNum=nodeObjTagOver.Def.DefNum;
			}
			else {
				nodeOverCatDefNum=Defs.GetDefsForCategory(DefCat.ImageCats,true)[treeNodeOver.Parent.Index].DefNum;
			}
			Document documentNew=new Document();
			NodeObjTag nodeObjTagNew=nodeObjTagOver.Copy();//In case we cancel
			string[] arrayDraggedFiles=(string[])e.Data.GetData(DataFormats.FileDrop);
			string errorMsg="";
			for(int i=0;i<arrayDraggedFiles.Length;i++) {
				if(Directory.Exists(arrayDraggedFiles[i])) {//Not allowed to drag drop a folder
					errorMsg+="\r\n"+Path.GetFileName(arrayDraggedFiles[i]);
					continue;
				}
				documentNew=ImageStore.Import(arrayDraggedFiles[i],nodeOverCatDefNum,_patCur);
				FormDocInfo formDocInfo=new FormDocInfo(_patCur,documentNew);
				formDocInfo.ShowDialog(this);
				if(formDocInfo.DialogResult==DialogResult.OK) {
					nodeObjTagNew=new NodeObjTag(EnumNodeType.Document,documentNew.DocNum);
					_documentShowing=documentNew.Copy();
				}
				else {
					DeleteDocument(false,false,documentNew);
				}
			}
			FillTree(true);
			SelectTreeNode(nodeObjTagNew);
			if(errorMsg!="") {
				MessageBox.Show("The following items are directories and were not copied into the images folder for this patient."+errorMsg);
			}
		}

		private void treeMain_DragEnter(object sender,DragEventArgs e) {
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect=DragDropEffects.Copy;//Fills the DragEventArgs for DragDrop
			}
		}

		private void treeMain_MouseClick(object sender, MouseEventArgs e){
			//not currently used
		}

		private void treeMain_MouseDoubleClick(object sender, MouseEventArgs e){
			timerTreeClick.Enabled=false;
			if(_webBrowser!=null) {
				//This prevents users from previewing the PDF in OD at the same time they have it open in an external PDF viewer.
				//There was a strange graphical bug that occurred when the PDF was previewed at the same time the PDF was open in the Adobe Acrobat Reader DC
				//if the Adobe "Enable Protected Mode" option was disabled.  The graphical bug caused many ODButtons and ODGrids to disappear even though their
				//Visible flags were set to true.  Somehow the WndProc() for the form which owned these controls was not calling the OnPaint() method.
				//Thus the bug affected many custom drawn controls.
				_webBrowser.Dispose();
				_webBrowser=null;
			}
			TreeNode treeNodeClicked=treeMain.GetNodeAt(e.Location);
			if(treeNodeClicked==null) {
				return;
			}
			NodeObjTag nodeObjTag=(NodeObjTag)treeNodeClicked.Tag;
			if(nodeObjTag.NodeType==EnumNodeType.Category) {
				return;
			}
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				MessageBox.Show("Images stored directly in database. Export file in order to open with external program.");
				return;//Documents must be stored in the A to Z Folder to open them outside of Open Dental.  Users can use the export button for now.
			}
			if(nodeObjTag.NodeType==EnumNodeType.Mount) {
				//Do nothing.  Must be consistent with how Docs are edited, so must use the Info button.
				//FormMountEdit fme=new FormMountEdit(_mountSelected);
				//fme.ShowDialog();//Edits the MountSelected object directly and updates and changes to the database as well.
				//FillDocList(true);//Refresh tree in case description for the mount changed.
				//return;
			}
			if(nodeObjTag.NodeType==EnumNodeType.Document) {
				if(_documentShowing==null){
					return;//This can happen if double click on category, then tree automatically shifts and e.Location is useless.
				}
				//Document _documentShowing=Documents.GetByNum(nodeObjTag.DocNum);
				string ext=ImageStore.GetExtension(_documentShowing);
				if(ext==".jpg" || ext==".jpeg" || ext==".gif") {
					return;
				}
				//We allow anything which ends with a different extention to be viewed in the windows fax viewer.
				//Specifically, multi-page faxes can be viewed more easily by one of our customers using the fax viewer.
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {

						try {
							string filePath=ImageStore.GetFilePath(_documentShowing,_patFolder);
							Process.Start(filePath);
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
						}
					
				}
				else {//Cloud
					//Download document into temp directory for displaying.
					FormProgress formProgress=new FormProgress();
					formProgress.DisplayText="Downloading Document...";
					formProgress.NumberFormat="F";
					formProgress.NumberMultiplication=1;
					formProgress.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					formProgress.TickMS=1000;
					OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(_patFolder.Replace("\\","/")
						,_documentShowing.FileName
						,new OpenDentalCloud.ProgressHandler(formProgress.OnProgress));
					formProgress.ShowDialog();
					if(formProgress.DialogResult==DialogResult.Cancel) {
						state.DoCancel=true;
					}
					else {
						string tempFile=PrefC.GetRandomTempFile(Path.GetExtension(_documentShowing.FileName));
						File.WriteAllBytes(tempFile,state.FileContent);

							Process.Start(tempFile);
						
					}
				}
			}
		}

		///<summary></summary>
		private void TreeMain_MouseDown(object sender,MouseEventArgs e) {
			_isMouseDownTree=true;
			//This fires on the mouse down for either button.  It also fires on the first click of a doubleclick.
			TreeNode treeNodeOver=treeMain.GetNodeAt(e.Location);
			if(treeNodeOver==null) {
				return;
			}
			NodeObjTag nodeObjTag=(NodeObjTag)treeNodeOver.Tag;
			_nodeObjTagDragging=nodeObjTag.Copy();//Saving in the event that the user drags the node.
			if(nodeObjTag.NodeType==EnumNodeType.Category) {
				SelectTreeNode(nodeObjTag);
				return;
			}
			if(e.Button==MouseButtons.Right) {
				//menuTree is actually shown separately because of its association as the context menu for the tree
				//The code here is to also sometimes refresh the image
				if(nodeObjTag.Equals(_nodeObjTagOld)) {//On node already selected
					//Do nothing because a large image will cause a pause before menu comes up
				}
				else if(nodeObjTag.NodeType==EnumNodeType.Document) {
					Document document=Documents.GetByNum(nodeObjTag.DocNum);
					if(Path.GetExtension(document.FileName).ToLower()==".pdf") {
						//Don't do entire SelectTreeNode because that refreshes the browser control asynch, which then grabs focus after refresh and the menu disappears.
						treeMain.SelectedNode=treeNodeOver;
						_documentShowing=Documents.GetByNum(nodeObjTag.DocNum);
						//todo: Is there anything else that needs to be done here?
					}
					else {
						SelectTreeNode(nodeObjTag);
					}
				}
				else {
					SelectTreeNode(nodeObjTag);
				}
				return;
			}
			//Left button from here down
			if(nodeObjTag.Equals(_nodeObjTagOld)) {
				//We refresh on the same node so that user can reset zoom/pan.
				//Can't always refresh immediately because it messes up double click.
				//So we pass off the refresh to a timer that will refresh in one sec.
				//But the timer will be cancelled if the user double clicks.
				//This keeps the UI very fast.
				if(nodeObjTag.NodeType==EnumNodeType.Document) {
					Document document=Documents.GetByNum(nodeObjTag.DocNum);
					if(Path.GetExtension(document.FileName).ToLower()==".pdf") {
						return;
					}
				}
				if(zoomSlider.Value>1000) {//User clicked many many times on the zoom +.  We need to quickly reset the zoom for them.
					timerTreeClick.Interval=1;
				}
				else {
					timerTreeClick.Interval=1000;
				}
				timerTreeClick.Enabled=true;
			}
			else {//new node
				SelectTreeNode(nodeObjTag);
			}
		}

		private void treeMain_MouseLeave(object sender,EventArgs e) {
			if(!_isMouseDownTree){
				return;
			}
			//Reset the curor and old Tag Object
			treeMain.Cursor=Cursors.Default;
			_nodeObjTagDragging=null;
		}

		/// <summary>This changes the cursor in the event that a document is being moved to a new Def category within the tree.</summary>
		private void treeMain_MouseMove(object sender,MouseEventArgs e) {
			if(!_isMouseDownTree){
				return;
			}
			if(_nodeObjTagDragging==null) {
				treeMain.Cursor=Cursors.Default;
				return;
			}
			TreeNode treeNodeMouseOver=treeMain.GetNodeAt(e.Location);
			//Unknown malfunction, return cursor to orginal state
			if(treeNodeMouseOver==null) {
				treeMain.Cursor=Cursors.Default;
				return;
			}
			//Over the original node, no change needed
			if(_nodeObjTagDragging.Equals((NodeObjTag)treeNodeMouseOver.Tag)) {
				treeMain.Cursor=Cursors.Default;
				return;
			}
			//Show drag cursor if holding the left mouse button
			if(e.Button==MouseButtons.Left) {
				treeMain.Cursor=Cursors.Hand;
			}
		}

		private void treeMain_MouseUp(object sender,MouseEventArgs e) {
			_isMouseDownTree=false;
			treeMain.Cursor=Cursors.Default;
			if(_nodeObjTagDragging==null) {//
				return;
			}
			//Dragging should only happen with the left mouse button
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			//Compare tree category information
			TreeNode treeNodeNewCat=treeMain.GetNodeAt(e.Location);
			TreeNode treeNodeOldCat=GetTreeNodeByKey(_nodeObjTagDragging);
			if(treeNodeNewCat==null || treeNodeOldCat==null) {
				return;
			}
			NodeObjTag nodeObjTagNewCat=(NodeObjTag)treeNodeNewCat.Tag;
			long nodeNewCatDefNum=0;
			long nodeOldCatDefNum=0;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			if(nodeObjTagNewCat.NodeType==EnumNodeType.Category) {
				nodeNewCatDefNum=listDefs[treeNodeNewCat.Index].DefNum;
			}
			else {
				nodeNewCatDefNum=listDefs[treeNodeNewCat.Parent.Index].DefNum;
			}
			nodeOldCatDefNum=listDefs[treeNodeOldCat.Parent.Index].DefNum;
			//If we try to move a category or if the node has not moved categories then return
			if(_nodeObjTagDragging.NodeType==EnumNodeType.Category || nodeOldCatDefNum==nodeNewCatDefNum) {
				return;
			}
			if(_nodeObjTagDragging.NodeType==EnumNodeType.Mount) {
				Mount mount=Mounts.GetByNum(_nodeObjTagDragging.MountNum);
				string mountOriginalCat=Defs.GetDef(DefCat.ImageCats,mount.DocCategory).ItemName;
				string mountNewCat=Defs.GetDef(DefCat.ImageCats,nodeNewCatDefNum).ItemName;
				mount.DocCategory=nodeNewCatDefNum;
				SecurityLogs.MakeLogEntry(Permissions.ImageEdit,mount.PatNum,Lan.G(this,"Mount moved from")+" "+mountOriginalCat+" "
					+Lan.G(this,"to")+" "+mountNewCat);
				Mounts.Update(mount);
			}
			else {
				Document document=Documents.GetByNum(_nodeObjTagDragging.DocNum);
				string docOldCat=Defs.GetDef(DefCat.ImageCats,document.DocCategory).ItemName;
				string docNewCat=Defs.GetDef(DefCat.ImageCats,nodeNewCatDefNum).ItemName;
				document.DocCategory=nodeNewCatDefNum;
				string logText=Lan.G(this,"Document moved")+": "+document.FileName;
				if(document.Description!="") {
					string docDescript=document.Description;
					if(docDescript.Length>50) {
						docDescript=docDescript.Substring(0,50);
					}
					logText+=" "+Lan.G(this,"with description")+" "+docDescript;
				}
				logText+=" "+Lan.G(this,"from category")+" "+docOldCat+" "+Lan.G(this,"to category")+" "+docNewCat;
				SecurityLogs.MakeLogEntry(Permissions.ImageEdit,document.PatNum,logText,document.DocNum,document.DateTStamp);
				Documents.Update(document);
			}
			FillTree(true);
			_nodeObjTagDragging=null;
		}

		private void windowingSlider_Scroll(object sender,EventArgs e) {
			if(IsDocumentShowing()) {
				_documentShowing.WindowingMin=windowingSlider.MinVal;
				_documentShowing.WindowingMax=windowingSlider.MaxVal;
				InvalidateSettingsColor();
			}
			if(IsMountItemShowing()) {
				_arrayDocumentsShowing[_idxSelectedInMount].WindowingMin=windowingSlider.MinVal;
				_arrayDocumentsShowing[_idxSelectedInMount].WindowingMax=windowingSlider.MaxVal;
				InvalidateSettingsColor();
			}
		}

		private void windowingSlider_ScrollComplete(object sender,EventArgs e) {
			if(IsDocumentShowing()) {
				Documents.Update(_documentShowing);
				ImageStore.DeleteThumbnailImage(_documentShowing,_patFolder);
				InvalidateSettingsColor();
			}
			if(IsMountItemShowing()) {
				Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount]);
				ImageStore.DeleteThumbnailImage(_arrayDocumentsShowing[_idxSelectedInMount],_patFolder);
				InvalidateSettingsColor();
			}
		}

		private void zoomSlider_FitPressed(object sender, EventArgs e){
			_pointTranslation=new Point(0,0);
			panelMain.Invalidate();
		}

		private void zoomSlider_Zoomed(object sender, EventArgs e){
			panelMain.Invalidate();
		}
		#endregion Methods - Event Handlers

		#region Methods - Private
		///<summary>Mostly to dispose of the old bitmaps all in one place.</summary>
		private void ClearObjects(){
			_bitmapRaw?.Dispose();
			_bitmapRaw=null;
			if(_arrayBitmapsShowing!=null && _arrayBitmapsShowing.Length>0){
				for(int i=0;i<_arrayBitmapsShowing.Length;i++){
					_arrayBitmapsShowing[i]?.Dispose();
					_arrayBitmapsShowing[i]=null;
				}
			}
			_arrayBitmapsShowing=null;
			_documentShowing=null;
			_mountShowing=null;
			_nodeObjTagOld=null;//so that next tree click will work
			panelMain.Invalidate();
		}

		///<summary>Converts a point in panelMain into a point in _bitmapShowing.  If mount, then it's coords within entire mount.</summary>
		private Point ControlPointToBitmapPoint(Point pointPanel) {
			Matrix matrix=new Matrix();
			matrix.Translate(-panelMain.Width/2,-panelMain.Height/2,MatrixOrder.Append);
			matrix.Scale(1f/zoomSlider.Value*100f,1f/zoomSlider.Value*100f,MatrixOrder.Append);//1 if no scale
			matrix.Translate(-_pointTranslation.X,-_pointTranslation.Y,MatrixOrder.Append);
			//We are now at center image.
			if(IsMountShowing()){
				//no rotation or flip
				matrix.Translate(_mountShowing.Width/2f,_mountShowing.Height/2f,MatrixOrder.Append);
			}
			if(IsDocumentShowing()){
				matrix.Rotate(-_documentShowing.DegreesRotated,MatrixOrder.Append);
				if(_documentShowing.IsFlipped){
					Matrix matrixFlip=new Matrix(-1,0,0,1,0,0);
					matrix.Multiply(matrixFlip,MatrixOrder.Append);
				}
				if(_documentShowing.CropW>0 && _documentShowing.CropH>0){
					matrix.Translate(_documentShowing.CropW/2f,_documentShowing.CropH/2f,MatrixOrder.Append);//back to UL of cropped area
					matrix.Translate(_documentShowing.CropX,_documentShowing.CropY,MatrixOrder.Append);//then back to 00 of image
					//We can't really clip here.  We could test below, just before return, or caller could test point.
				}
				else{
					matrix.Translate(_bitmapShowing.Width/2f,_bitmapShowing.Height/2f,MatrixOrder.Append);
				}
			}
			Point[] points={pointPanel };
			matrix.TransformPoints(points);
			return points[0];
		}

		///<summary>Deletes the specified document from the database and refreshes the tree view. Set securityCheck false when creating a new document that might get cancelled.  Document is passed in because it might not be in the tree if the image folder it belongs to is now hidden.</summary>
		private void DeleteDocument(bool isVerbose,bool doSecurityCheck,Document document) {
			if(doSecurityCheck) {
				if(!Security.IsAuthorized(Permissions.ImageDelete,document.DateCreated)) {
					return;
				}
			}
			EhrLab lab=EhrLabImages.GetFirstLabForDocNum(document.DocNum);
			if(lab!=null) {
				string dateSt=lab.ObservationDateTimeStart.PadRight(8,'0').Substring(0,8);//stored in DB as yyyyMMddhhmmss-zzzz
				DateTime dateT=PIn.Date(dateSt.Substring(4,2)+"/"+dateSt.Substring(6,2)+"/"+dateSt.Substring(0,4));
				MessageBox.Show(Lan.G(this,"This image is attached to a lab order for this patient on "+dateT.ToShortDateString()+". "+Lan.G(this,"Detach image from this lab order before deleting the image.")));
				return;
			}
			//EnableAllToolBarButtons(false);
			if(isVerbose) {
				if(document.ImgType.In(ImageType.Document,ImageType.File)){
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete document?")) {
						return;
					}
				}
				else{
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete image?")) {
						return;
					}
				}
			}
			Statements.DetachDocFromStatements(document.DocNum);
			//SelectTreeNode(null);//Release access to current image so it may be properly deleted.
			if(_webBrowser!=null) {
				_webBrowser.Dispose();//Clear any previously loaded Acrobat .pdf file.
				_webBrowser=null;
			}
			Document[] docs=new Document[1] { document };	
			try {
				ImageStore.DeleteDocuments(docs,_patFolder);
			}
			catch(Exception ex) {  //Image could not be deleted, in use.
				MessageBox.Show(this,ex.Message);
			}
			if(IsDocumentShowing()){
				FillTree(false);
				SelectTreeNode(null);
				panelMain.Invalidate();
			}
			if(IsMountItemShowing()){
				//need to review with more situations.  What if item isn't filled yet?
				_arrayDocumentsShowing[_idxSelectedInMount]=null;
				_arrayBitmapsShowing[_idxSelectedInMount]=null;
				panelMain.Invalidate();
			}
		}

		///<summary></summary>
		private void DrawDocument(Graphics g){
			//we're at the center of the image, and working in image coordinates
			g.RotateTransform(_documentShowing.DegreesRotated);
			if(_documentShowing.IsFlipped){
				Matrix matrix=new Matrix(-1,0,0,1,0,0);
				g.MultiplyTransform(matrix);
			}
			//Make our 0,0 reference point be the center of the portion of the image that will show:
			if(_documentShowing.CropW>0 && _documentShowing.CropH>0){
				g.TranslateTransform(-_documentShowing.CropW/2,-_documentShowing.CropH/2);//back to UL of cropped area
				g.TranslateTransform(-_documentShowing.CropX,-_documentShowing.CropY);//then back to 00 of image
				g.SetClip(new Rectangle(_documentShowing.CropX,_documentShowing.CropY,_documentShowing.CropW,_documentShowing.CropH));
			}
			else{
				g.TranslateTransform(-_bitmapShowing.Width/2,-_bitmapShowing.Height/2);//back to UL corner 00 of image
			}
			g.DrawImage(_bitmapShowing,0,0);
			g.ResetClip();
			if(_rectangleCrop.Width>0 && _rectangleCrop.Height>0) {//Drawn last so it's on top.
				g.ResetTransform();//panel coords
				g.DrawRectangle(Pens.Blue,_rectangleCrop);
			}
		}

		///<summary>Used for drawing on screen in real time and also for drawing to printer.</summary>
		private void DrawMount(Graphics g){
			//we're at center of the mount, and working in mount coordinates
			g.TranslateTransform(-_mountShowing.Width/2f,-_mountShowing.Height/2f);
			using(SolidBrush brushBack=new SolidBrush(_mountShowing.ColorBack)){
				g.FillRectangle(brushBack,0,0,_mountShowing.Width,_mountShowing.Height);
			}
			//&& 
			for(int i=0;i<_listMountItems.Count;i++){
				if(_isDraggingMount && i==_idxSelectedInMount){// && _dateTimeMouseDown.AddMilliseconds(600) < DateTime.Now){//_pointDragStart!=_pointDragNow){
					continue;//we'll paint this one after the loop
				}
				DrawMountOne(g,i);
			}
			if(_isDraggingMount && _idxSelectedInMount>-1){// && _dateTimeMouseDown.AddMilliseconds(600) < DateTime.Now){//_pointDragStart!=_pointDragNow){
				DrawMountOne(g,_idxSelectedInMount);
			}
			//SELECT * FROM document WHERE patnum=293 AND mountitemnum > 0
			//outlines:
			for(int i=0;i<_listMountItems.Count;i++){
				g.DrawRectangle(Pens.Silver,_listMountItems[i].Xpos,_listMountItems[i].Ypos,
					_listMountItems[i].Width,_listMountItems[i].Height);//silver is 50% black
			}
			//yellow outline of selected
			if(_idxSelectedInMount!=-1){
				g.DrawRectangle(Pens.Yellow,_listMountItems[_idxSelectedInMount].Xpos,_listMountItems[_idxSelectedInMount].Ypos,
					_listMountItems[_idxSelectedInMount].Width,_listMountItems[_idxSelectedInMount].Height);
			}
		}

		private void DrawMountOne(Graphics g,int i){
			GraphicsState graphicsStateMount=g.Save();
			g.TranslateTransform(_listMountItems[i].Xpos,_listMountItems[i].Ypos);//UL of mount position
			if(_isDraggingMount && i==_idxSelectedInMount){// && _dateTimeMouseDown.AddMilliseconds(600) < DateTime.Now){//_pointDragStart!=_pointDragNow){
				g.TranslateTransform(_pointDragNow.X-_pointDragStart.X,_pointDragNow.Y-_pointDragStart.Y);
				//combined with the centering below, this has the effect of dragging based on center
				//we show the entire image when dragging, so no clip
			}
			else{
				g.SetClip(new Rectangle(0,0,_listMountItems[i].Width,_listMountItems[i].Height));
			}
			g.TranslateTransform(_listMountItems[i].Width/2,_listMountItems[i].Height/2);//rotate and flip about the center of the mount box
			if(_arrayBitmapsShowing[i]!=null){
				if(_arrayDocumentsShowing[i].CropW > 0 && _arrayDocumentsShowing[i].CropH > 0){
					//We would scale here if bitmap was at original scale, but we've already scaled it when loading.
					//If we had scaled it, we would be in bitmap coords from here down instead of mount coords
					//We are also in the center of the cropped area because of the last two translations that happen just before drawImage
					g.RotateTransform(_arrayDocumentsShowing[i].DegreesRotated);
					if(_arrayDocumentsShowing[i].IsFlipped){
						Matrix matrix=new Matrix(-1,0,0,1,0,0);
						g.MultiplyTransform(matrix);
					}
					if(_arrayDocumentsShowing[i].DegreesRotated.In(0,180)){
						g.TranslateTransform(-_listMountItems[i].Width/2,-_listMountItems[i].Height/2);//back to UL corner of cropped area
						float scale=(float)_listMountItems[i].Width/_arrayDocumentsShowing[i].CropW;//example 100/200=.5 because image was already resampled smaller
						g.TranslateTransform(-_arrayDocumentsShowing[i].CropX*scale,-_arrayDocumentsShowing[i].CropY*scale);//then to the 00 of the image
					}
					else{//90,270
						g.TranslateTransform(-_listMountItems[i].Height/2,-_listMountItems[i].Width/2);
						float scale=(float)_listMountItems[i].Height/_arrayDocumentsShowing[i].CropW;
						g.TranslateTransform(-_arrayDocumentsShowing[i].CropX*scale,-_arrayDocumentsShowing[i].CropY*scale);
					}
					g.DrawImage(_arrayBitmapsShowing[i],0,0);
				}
				else{//no crop specified, so just fit it to the mount space
					//g.TranslateTransform(_listMountItems[i].Xpos+_listMountItems[i].Width/2,_listMountItems[i].Ypos+_listMountItems[i].Height/2);//center
					//float scaleItem=ZoomSlider.CalcScaleFit(new Size(_listMountItems[i].Width,_listMountItems[i].Height),
					//	_arrayBitmapsShowing[i].Size,_arrayDocumentsShowing[i].DegreesRotated);
					//g.ScaleTransform(scaleItem,scaleItem);//then, scale to fit
					g.RotateTransform(_arrayDocumentsShowing[i].DegreesRotated);
					if(_arrayDocumentsShowing[i].IsFlipped){
						Matrix matrix=new Matrix(-1,0,0,1,0,0);
						g.MultiplyTransform(matrix);
					}
					g.TranslateTransform(-_arrayBitmapsShowing[i].Width/2,-_arrayBitmapsShowing[i].Height/2);//from center of the image to UL
					g.DrawImage(_arrayBitmapsShowing[i],0,0);
				}
			}
			g.Restore(graphicsStateMount);
			/* //This is useful for debugging, but sort of annoying for regular use, especially for non-xray photos
			//if(_isDraggingMount){ //&& _pointDragStart!=_pointDragNow){
				graphicsStateMount=g.Save();
				g.TranslateTransform(_listMountItems[i].Xpos,_listMountItems[i].Ypos);//UL of mount position
				g.TranslateTransform(_listMountItems[i].Width/2,_listMountItems[i].Height/2);//center
				using(Font font=new Font(FontFamily.GenericSansSerif,40)){
					SizeF sizeString=g.MeasureString(_listMountItems[i].ItemOrder.ToString(),font);
					g.DrawString(_listMountItems[i].ItemOrder.ToString(),font,Brushes.OrangeRed,-sizeString.Width/2,-sizeString.Height/2);
				}
				g.Restore(graphicsStateMount);
			//}*/
		}

		///<summary>Enables or disables all the buttons in both toolbars to handle situation where no patient is selected.</summary>
		private void EnableToolBarsPatient(bool enable) {
			for(int i=0;i<toolBarMain.Buttons.Count;i++) {
				if(toolBarMain.Buttons[i].Tag.ToString()==TB.Imaging.ToString()){
					continue;
				}
				toolBarMain.Buttons[i].Enabled=enable;
			}
			//Capture button?
			toolBarMain.Invalidate();
			for(int i=0;i<toolBarPaint.Buttons.Count;i++) {
				toolBarPaint.Buttons[i].Enabled=enable;
			}
			toolBarPaint.Invalidate();
			windowingSlider.Enabled=enable;
			zoomSlider.Enabled=enable;
			windowingSlider.Invalidate();
		}

		///<summary>Not technically all.  There are some buttons that we never disable such as import, scan, etc.</summary>
		private void EnableAllToolBarButtons(bool enable) {
			EnableToolBarButtons(print:enable, delete:enable, info:enable, sign:enable, export:enable, copy:enable, brightAndContrast:enable, zoom:enable, zoomOne:enable, crop:enable, pan:enable, adj:enable, size:enable, remove:enable, add:enable, flip:enable, rotateL:enable, rotateR:enable, rotate180:enable);
		}

		///<summary>This is the same thing from many places, so it's centralized.</summary>
		private void EnableToolBarButtonsMount(){
			if(_idxSelectedInMount==-1){//entire mount
				EnableToolBarButtons(print:true, delete:true, info:true, sign:false, export:true, copy:true, brightAndContrast:false, zoom:true, zoomOne:false, crop:false, pan:true, adj:true, size:false, remove:false, add:false, flip:false, rotateL:false, rotateR:false, rotate180:false);
			}
			else{//individual image
				EnableToolBarButtons(print:true, delete:true, info:true, sign:false, export:true, copy:true, brightAndContrast:true, zoom:true, zoomOne:true, crop:false, pan:true, adj:true, size:true, remove:true, add:true, flip:true, rotateL:true, rotateR:true, rotate180:true);
			}
		}

		///<summary>Defined this way to force future programming to consider which tools are enabled and disabled for every possible tool in the menu.  To prevent bugs, you must always use named arguments.  Called when users clicks on Crop/Pan/Mount buttons, clicks Tree, or clicks around on a mount.</summary>
		private void EnableToolBarButtons(bool print, bool delete, bool info, bool sign, bool export, bool copy, bool brightAndContrast, bool zoom, bool zoomOne, bool crop, bool pan, bool adj, bool size, bool remove, bool add, bool flip, bool rotateL,bool rotateR, bool rotate180) {
			//Some buttons don't show here because they are always enabled as long as there is a patient,
			//including Scan, Import, Paste, Templates, Mounts
			toolBarMain.Buttons[TB.Print.ToString()].Enabled=print;
			toolBarMain.Buttons[TB.Delete.ToString()].Enabled=delete;
			toolBarMain.Buttons[TB.Info.ToString()].Enabled=info;
			toolBarMain.Buttons[TB.Sign.ToString()].Enabled=sign;
			toolBarMain.Buttons[TB.Export.ToString()].Enabled=export;
			toolBarMain.Buttons[TB.Copy.ToString()].Enabled=copy;
			toolBarMain.Invalidate();
			windowingSlider.Enabled=brightAndContrast;
			windowingSlider.Invalidate();
			zoomSlider.Enabled=zoom;
			toolBarPaint.Buttons[TB.ZoomOne.ToString()].Enabled=zoomOne;
			toolBarPaint.Buttons[TB.Crop.ToString()].Enabled=crop;
			toolBarPaint.Buttons[TB.Pan.ToString()].Enabled=pan;
			toolBarPaint.Buttons[TB.Adj.ToString()].Enabled=adj;
			toolBarPaint.Buttons[TB.Size.ToString()].Enabled=size;
			toolBarPaint.Buttons[TB.Remove.ToString()].Enabled=remove;
			toolBarPaint.Buttons[TB.Add.ToString()].Enabled=add;
			toolBarPaint.Buttons[TB.Flip.ToString()].Enabled=flip;
			toolBarPaint.Buttons[TB.RotateR.ToString()].Enabled=rotateR;
			toolBarPaint.Buttons[TB.RotateL.ToString()].Enabled=rotateL;
			toolBarPaint.Buttons[TB.Rotate180.ToString()].Enabled=rotate180;
			toolBarPaint.Invalidate();
			//toolBarMount buttons are always visible
			
		}

		///<summary>Fills the panelnote control with the current document signature when the panelnote is visible and when a valid document is currently selected.</summary>
		private void FillSignature() {
			if(!IsDocumentShowing()){
				return;
			}
			textNote.Text="";
			sigBox.ClearTablet();
			if(!panelNote.Visible) {
				return;
			}
			textNote.Text=_documentShowing.Note;
			labelInvalidSig.Visible=false;
			sigBox.Visible=true;
			sigBox.SetTabletState(0);//never accepts input here
			//Topaz box is not supported in Unix, since the required dll is Windows native.
			if(_documentShowing.SigIsTopaz) {
				if(_documentShowing.Signature!=null && _documentShowing.Signature!="") {
					//if(allowTopaz) {	
					sigBox.Visible=false;
					_sigBoxTopaz.Visible=true;
					TopazWrapper.ClearTopaz(_sigBoxTopaz);
					TopazWrapper.SetTopazCompressionMode(_sigBoxTopaz,0);
					TopazWrapper.SetTopazEncryptionMode(_sigBoxTopaz,0);
					TopazWrapper.SetTopazKeyString(_sigBoxTopaz,"0000000000000000");//Clear out the key string
					string keystring=GetHashString(_documentShowing);
					TopazWrapper.SetTopazAutoKeyData(_sigBoxTopaz,keystring);
					TopazWrapper.SetTopazEncryptionMode(_sigBoxTopaz,2);//high encryption
					TopazWrapper.SetTopazCompressionMode(_sigBoxTopaz,2);//high compression
					TopazWrapper.SetTopazSigString(_sigBoxTopaz,_documentShowing.Signature);
					_sigBoxTopaz.Refresh();
					//If sig is not showing, then setting the Key String to the hashed data. This is the way we used to handle signatures.
					if(TopazWrapper.GetTopazNumberOfTabletPoints(_sigBoxTopaz)==0) {
						TopazWrapper.SetTopazKeyString(_sigBoxTopaz,"0000000000000000");//Clear out the key string
						TopazWrapper.SetTopazKeyString(_sigBoxTopaz,keystring);
						TopazWrapper.SetTopazSigString(_sigBoxTopaz,_documentShowing.Signature);
					}
					//If sig is not showing, then try encryption mode 3 for signatures signed with old SigPlusNet.dll.
					if(TopazWrapper.GetTopazNumberOfTabletPoints(_sigBoxTopaz)==0) {
						TopazWrapper.SetTopazEncryptionMode(_sigBoxTopaz,3);//Unknown mode (told to use via TopazSystems)
						TopazWrapper.SetTopazSigString(_sigBoxTopaz,_documentShowing.Signature);
					}
					if(TopazWrapper.GetTopazNumberOfTabletPoints(_sigBoxTopaz)==0) {
						labelInvalidSig.Visible=true;
					}
					//}
				}
			}
			else {//not topaz
				if(_documentShowing.Signature!=null && _documentShowing.Signature!="") {
					sigBox.Visible=true;
					//if(allowTopaz) {	
					_sigBoxTopaz.Visible=false;
					//}
					sigBox.ClearTablet();
					//sigBox.SetSigCompressionMode(0);
					//sigBox.SetEncryptionMode(0);
					sigBox.SetKeyString(GetHashString(_documentShowing));
					//"0000000000000000");
					//sigBox.SetAutoKeyData(ProcCur.Note+ProcCur.UserNum.ToString());
					//sigBox.SetEncryptionMode(2);//high encryption
					//sigBox.SetSigCompressionMode(2);//high compression
					sigBox.SetSigString(_documentShowing.Signature);
					if(sigBox.NumberOfTabletPoints()==0) {
						labelInvalidSig.Visible=true;
					}
					sigBox.SetTabletState(0);//not accepting input.
				}
			}
		}

		private string GetHashString(Document doc) {
			return ImageStore.GetHashString(doc,_patFolder);
		}

		///<summary>Gets the DefNum category of the current selection. The current selection can be a folder itself, or a document within a folder. If nothing selected, then it returns the DefNum of first in the list.</summary>
		private long GetCurrentCategory() {
			if(treeMain.SelectedNode==null){
				return Defs.GetDefsForCategory(DefCat.ImageCats,true)[0].DefNum;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType==EnumNodeType.Document 
				|| ((NodeObjTag)treeMain.SelectedNode.Tag).NodeType==EnumNodeType.Mount) 
			{
				TreeNode treeNode=treeMain.SelectedNode;
				while(treeNode.Parent!=null) {
					treeNode=treeNode.Parent;
				}
				return ((NodeObjTag)treeNode.Tag).Def.DefNum;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType==EnumNodeType.Category) {
				return ((NodeObjTag)treeMain.SelectedNode.Tag).Def.DefNum;
			}
			throw new Exception();
		}
		
		///<summary>Recursive method that searches the current node for a child node with the type and prikey specified by the Tag.  If can't find, then returns null.</summary>
		private TreeNode GetTreeNodeByKey(NodeObjTag nodeObjTag,TreeNode treeNodeParent=null) {
			if(nodeObjTag==null){
				return null;
			}
			if(treeNodeParent==null){//first iteration
				foreach(TreeNode treeNode in treeMain.Nodes){
					TreeNode treeNodeResult=GetTreeNodeByKey(nodeObjTag,treeNode);//children of each main node
					if(treeNodeResult!=null){
						return treeNodeResult;
					}
				}
				return null;//couldn't find the node in any children. Final result.
			}			
			foreach(TreeNode treeNode in treeNodeParent.Nodes) {
				if(((NodeObjTag)treeNode.Tag).Equals(nodeObjTag)){//it's just looking at type and primary key
					return treeNode;
				}
				//now, any children of this node
				TreeNode treeNodeResult=GetTreeNodeByKey(nodeObjTag,treeNode);
				if(treeNodeResult!=null){
					return treeNodeResult;
				}
			}
			return null;//this node and its children are not a match.
		}

		///<summary>Pass in panel or mount coords. Returns index of item in mount, or -1 if no hit.</summary>
		private int HitTestInMount(int x,int y,bool isMountCoords=false){
			if(_listMountItems==null){
				return -1;
			}
			Point pointMount;
			if(isMountCoords){
				pointMount=new Point(x,y);
			}
			else{
				pointMount=ControlPointToBitmapPoint(new Point(x,y));
			}
			for(int i=0;i<_listMountItems.Count;i++){
				Rectangle rect=new Rectangle(_listMountItems[i].Xpos,_listMountItems[i].Ypos,_listMountItems[i].Width,_listMountItems[i].Height);
				if(rect.Contains(pointMount)){
					return i;
				}
			}
			return -1;
		}

		///<summary>Returns true if a valid document is showing.  This is very different from testing the property _documentShowing, which would return true for mounts.</summary>
		private bool IsDocumentShowing(){
			if(treeMain.SelectedNode==null){
				return false;
			}
			if(treeMain.SelectedNode.Tag==null){//Internal error
				return false;
			}
			if(treeMain.SelectedNode.Parent==null) {		//This is a folder node.
				return false;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType!=EnumNodeType.Document){
				return false;
			}
			if(_documentShowing==null){
				return false;
			}
			return true;
		}

		///<summary>Returns true if a valid mount is showing.</summary>
		private bool IsMountShowing(){
			if(treeMain.SelectedNode==null){
				return false;
			}
			if(treeMain.SelectedNode.Tag==null){//Internal error
				return false;
			}
			if(treeMain.SelectedNode.Parent==null) {		//This is a folder node.
				return false;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType!=EnumNodeType.Mount){
				return false;
			}
			if(_mountShowing==null){
				return false;
			}
			return true;
		}

		///<summary>Returns true if a valid mountitem is selected and there's a valid bitmap in that location.</summary>
		private bool IsMountItemShowing(){
			if(!IsMountShowing()){
				return false;
			}
			if(_idxSelectedInMount==-1){
				return false;
			}
			if(_arrayDocumentsShowing[_idxSelectedInMount]==null){
				return false;
			}
			return true;
		}

		///<summary>Invalidates the color image setting and recalculates.  This is not on a separate thread.  Instead, it's just designed to run no more than about every 300ms, which completely avoids any lockup.</summary>
		private void InvalidateSettingsColor() {
			if(IsDocumentShowing()){
				//if((imageSettingFlags & ImageSettingFlags.COLORFUNCTION) !=0 || (imageSettingFlags & ImageSettingFlags.CROP) !=0){
				//We don't do any flip, rotate, or crop here
				_bitmapShowing?.Dispose();
				_bitmapShowing=ImageHelper.ApplyDocumentSettingsToImage2(_documentShowing,_bitmapRaw, ImageSettingFlags.COLORFUNCTION);
				//}
				panelMain.Invalidate();
			}
			if(IsMountItemShowing()){
				_arrayBitmapsShowing[_idxSelectedInMount]?.Dispose();
				_arrayBitmapsShowing[_idxSelectedInMount]=ImageHelper.ApplyDocumentSettingsToImage2(_arrayDocumentsShowing[_idxSelectedInMount],_bitmapRaw, ImageSettingFlags.COLORFUNCTION);
				panelMain.Invalidate();
				//eventually: only invalidate the rectangle of single mount item
			}
		}

		private void labelInvalidSig_DoubleClick(object sender, EventArgs e){
			ToolBarSign_Click();
		}

		private void label15_DoubleClick(object sender, EventArgs e){
			ToolBarSign_Click();
		}

		private void label1_DoubleClick(object sender, EventArgs e){
			ToolBarSign_Click();
		}

		///<summary>Resizes all controls in the image module to fit inside the current window, including controls which have varying visibility.</summary>
		private void LayoutAll(){
			panelMain.Top=toolBarPaint.Bottom;
			panelNote.Width=panelMain.Width;
			panelNote.Height=(int)Math.Min(114,Height-panelMain.Top);
			int panelNoteHeight=panelNote.Visible?panelNote.Height:0;
			panelMain.Height=Height-panelNoteHeight-panelMain.Top;
			if(_webBrowser!=null && _webBrowser.Visible) {
				_webBrowser.Size=panelMain.Size;
				_webBrowser.Location=panelMain.Location;
			}
			panelNote.Location=new Point(panelMain.Left,Height-panelNoteHeight-1);
		}

		///<summary>Loads bitmap from disk, resizes, applies bright/contrast, and saves it to _arrayBitmapsShowing and/or _bitmapRaw.</summary>
		private void LoadBitmap(int idx,EnumLoadBitmapType loadBitmapType,string localPathImportedCloud=""){
			if(idx==-1 || _arrayBitmapsShowing==null || idx > _arrayBitmapsShowing.Length-1){
				return;
			}
			if(_arrayDocumentsShowing[idx]==null){
				return;
			}
			if(loadBitmapType==EnumLoadBitmapType.OnlyIdx || loadBitmapType==EnumLoadBitmapType.IdxAndRaw){
				_arrayBitmapsShowing[idx]?.Dispose();
				_arrayBitmapsShowing[idx]=null;
				_arraySizesOriginal[idx]=new Size();
			}
			if(loadBitmapType==EnumLoadBitmapType.IdxAndRaw || loadBitmapType==EnumLoadBitmapType.OnlyRaw){
				_bitmapRaw?.Dispose();
				_bitmapRaw=null;
			}
			if(loadBitmapType==EnumLoadBitmapType.OnlyRaw){
				if(_arrayDocumentsShowing[idx].WindowingMax==0 || (_arrayDocumentsShowing[idx].WindowingMax==255 && _arrayDocumentsShowing[idx].WindowingMin==0)){
					if(_arrayBitmapsShowing[idx]==null){
						return;//we should already have an image showing, so this shouldn't happen
					}
					_bitmapRaw=new Bitmap(_arrayBitmapsShowing[idx]);
					return;
				}
			}
			//from here down, we must be getting the image from disk
			Bitmap bitmapTemp=ImageStore.OpenImage(_arrayDocumentsShowing[idx],_patFolder,localPathImportedCloud);
			if(bitmapTemp==null){
				return;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType==EnumNodeType.Document){
				//always IdxAndRaw
				//single images simply load up the whole unscaled image. Mounts load the whole image, but maybe at a different scale to match mount scale.
				try{
					_arrayBitmapsShowing[idx]=new Bitmap(bitmapTemp);//can crash here on a large image (WxHx24 > 250M)
					ImageHelper.ApplyColorSettings(_arrayBitmapsShowing[idx],_arrayDocumentsShowing[idx].WindowingMin,_arrayDocumentsShowing[idx].WindowingMax);
					_bitmapRaw=new Bitmap(bitmapTemp);//or it can crash here
					bitmapTemp.Dispose();//frees up the lock on the file on disk
				}
				catch{
					//This happens with large images due to 32 bit limits.  This would be solved by move to 64 bit.
					//It could also be addressed by holding only one image in memory instead of two.  There are downsides.
					//It could also be addressed by downgrading the image or only showing the image at just enough resolution to match screen pixels.  Difficult.
					//It could also be addressed by compressing one bitmap into memory using a stream. e.g. _bitmapRaw. Seems good.
					//Yet another solution would be to maintain ref and lock to actual file on disk.
					//The final solution will be some combination of the above.
					//error message will show on return in SelectTreeNode because _bitmapRaw==null
				}
				finally{
					bitmapTemp.Dispose();//frees up the lock on the file on disk
				}
				return;
			}
			_arraySizesOriginal[idx]=bitmapTemp.Size;
			double scale;
			if(_arrayDocumentsShowing[idx].CropW==0){//no crop, so scaled to fit mount item
				scale=ZoomSlider.CalcScaleFit(new Size(_listMountItems[idx].Width,_listMountItems[idx].Height),bitmapTemp.Size,_arrayDocumentsShowing[idx].DegreesRotated);
			}
			else{
				if(_arrayDocumentsShowing[idx].DegreesRotated.In(0,180)){
					scale=(double)_listMountItems[idx].Width/_arrayDocumentsShowing[idx].CropW;
				}
				else{//90,270
					//Can't use cropH, because we always assume it's faulty.
					scale=(double)_listMountItems[idx].Height/_arrayDocumentsShowing[idx].CropW;
				}
			}
			if(loadBitmapType==EnumLoadBitmapType.OnlyIdx || loadBitmapType==EnumLoadBitmapType.IdxAndRaw){
				_arrayBitmapsShowing[idx]=new Bitmap(bitmapTemp,new Size((int)(bitmapTemp.Width*scale),(int)(bitmapTemp.Height*scale)));
				ImageHelper.ApplyColorSettings(_arrayBitmapsShowing[idx],_arrayDocumentsShowing[idx].WindowingMin,_arrayDocumentsShowing[idx].WindowingMax);
			}
			if(loadBitmapType==EnumLoadBitmapType.IdxAndRaw || loadBitmapType==EnumLoadBitmapType.OnlyRaw){
				_bitmapRaw=new Bitmap(bitmapTemp,new Size((int)(bitmapTemp.Width*scale),(int)(bitmapTemp.Height*scale)));
				//don't apply color
			}
			bitmapTemp.Dispose();//frees up the lock on the file on disk
			//_arrayBitmapsRaw[i].Clone(  //don't ever do it this way. Messes up dpi.
		}

		///<summary>Displays the PDF in a web browser. Downloads the PDF file from the cloud if necessary.</summary>
		private void LoadPdf(string atoZFolder,string atoZFileName,string localPath,ref bool isExportable,string downloadMessage) {
			try {
				_webBrowser=new WebBrowser();
				this.Controls.Add(_webBrowser);
				_webBrowser.Visible=true;
				_webBrowser.Size=panelMain.Size;
				_webBrowser.Location=panelMain.Location;
				string pdfFilePath="";
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
					pdfFilePath=ODFileUtils.CombinePaths(atoZFolder,atoZFileName);
				}
				else if(CloudStorage.IsCloudStorage) {
					if(localPath!="") {
						pdfFilePath=localPath;
					}
					else {
						//Download PDF into temp directory for displaying.
						pdfFilePath=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),_documentShowing.DocNum+(_patCur!=null ? _patCur.PatNum.ToString() : "")+".pdf");
						FileAtoZ.Download(FileAtoZ.CombinePaths(atoZFolder,atoZFileName),pdfFilePath,downloadMessage);
					}
				}
				else {
					pdfFilePath=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),_documentShowing.DocNum+(_patCur!=null ? _patCur.PatNum.ToString() : "")+".pdf");
					File.WriteAllBytes(pdfFilePath,Convert.FromBase64String(_documentShowing.RawBase64));
				}
				if(!File.Exists(pdfFilePath)) {
					MessageBox.Show(Lan.G(this,"File not found")+": " + atoZFileName);
				}
				else {
					Application.DoEvents();//Show the browser control before loading, in case loading a large PDF, so the user can see the preview has started without waiting.
					_webBrowser.Navigate(pdfFilePath);//The return status of this function doesn't seem to be helpful.
					_webBrowserFilePath=pdfFilePath;
					panelMain.Visible=false;
					isExportable=true;
					//We used to delete the pdf as it was no longer needed.  
					//The web browser can take time to load and requires the file to be present when it finishes loading, 
					//so it will get deleted later (either when switching preview images or closing Open Dental
				}
			}
			catch {
				//An exception can happen if they do not have Adobe Acrobat Reader version 8.0 or later installed.
			}
		}

		private void panelNote_DoubleClick(object sender, EventArgs e){
			ToolBarSign_Click();
		}

		///<summary></summary>
		private void RefreshModuleData(long patNum) {
			//SelectTreeNode(null);//Clear selection and image and reset visibilities.
			if(patNum==0) {
				_familyCur=null;
				_patCur=null;
				return;
			}
			_familyCur=Patients.GetFamily(patNum);
			_patCur=_familyCur.GetPatient(patNum);
			_patFolder=ImageStore.GetPatientFolder(_patCur,ImageStore.GetPreferredAtoZpath());//This is where the pat folder gets created if it does not yet exist.
			if(_patNumLastSecurityLog!=patNum) {
				SecurityLogs.MakeLogEntry(Permissions.ImagesModule,patNum,"");
				_patNumLastSecurityLog=patNum;
			}
			Action actionClosing=null;
			if(CloudStorage.IsCloudStorage) {
				actionClosing=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.G(this,"Loading..."));
			}
			ImageStore.AddMissingFilesToDatabase(_patCur);
			actionClosing?.Invoke();
		}

		private void RefreshModuleScreen() {
			if(this.Enabled && _patCur!=null) {
				//Enable tools which must always be accessible when a valid patient is selected.
				EnableToolBarsPatient(true);
				//Item specific tools disabled until item chosen.
				EnableAllToolBarButtons(false);
			}
			else {
				EnableToolBarsPatient(false);//Disable entire menu (besides select patient).
			}
			ClearObjects();
			toolBarMain.Invalidate();
			toolBarPaint.Invalidate();
			FillTree(false);
		}

		///<summary>Sets cursor, sets pushed, and sets toolBarMount visible/invisible. This is called when user clicks on one of these buttons or when they select a treeNode.  Can also be called when LayoutToolbars is called from FormOpenDental, and in that case, we don't want to change EnableTreeItemTools.  EnableTreeItemTools is intentionally not centralized here, but is instead based on clicking events.</summary>
		private void SetCropPanEditAdj(EnumCropPanAdj cropPanEditAdj){
			toolBarPaint.Buttons[TB.Crop.ToString()].Pushed=false;
			toolBarPaint.Buttons[TB.Pan.ToString()].Pushed=false;
			toolBarPaint.Buttons[TB.Adj.ToString()].Pushed=false;
			switch(cropPanEditAdj){
				case EnumCropPanAdj.Crop:
					panelMain.Cursor=Cursors.Arrow;
					toolBarPaint.Buttons[TB.Crop.ToString()].Pushed=true;
					break;
				case EnumCropPanAdj.Pan:
					panelMain.Cursor=_cursorPan;
					toolBarPaint.Buttons[TB.Pan.ToString()].Pushed=true;
					break;
				case EnumCropPanAdj.Adj:
					panelMain.Cursor=Cursors.SizeAll;
					toolBarPaint.Buttons[TB.Adj.ToString()].Pushed=true;
					break;
			}
			_cropPanEditAdj=cropPanEditAdj;
			LayoutAll();
			toolBarPaint.Invalidate();
		}

		///<summary>Sets the panelnote visibility based on the given document's signature data and the current operating system.</summary>
		private void SetPanelNoteVisibility() {
			if(!IsDocumentShowing()){
				panelNote.Visible=false;
				return;
			}
			if(!_documentShowing.Note.IsNullOrEmpty()){
				panelNote.Visible=true;
				return;
			}
			if(!_documentShowing.Signature.IsNullOrEmpty()){
				panelNote.Visible=true;
				return;
			}
			panelNote.Visible=false;
		}

		private void SetZoomSlider(){
			if(IsDocumentShowing()){
				if(_bitmapRaw==null){
					//pdf. It will be disabled anyway
					zoomSlider.SetValue(panelMain.Size,panelMain.Size,0);//100
					return;
				}
				Size sizeImage=_bitmapRaw.Size;
				if(_documentShowing.CropW>0){
					sizeImage=new Size(_documentShowing.CropW,_documentShowing.CropH);
				}
				zoomSlider.SetValue(panelMain.Size,sizeImage,_documentShowing.DegreesRotated);
			}
			if(IsMountShowing()){
				zoomSlider.SetValue(panelMain.Size,new Size(_mountShowing.Width,_mountShowing.Height),0);
			}
		}

		private void SetWindowingSlider() {
			if(IsDocumentShowing()){
				if(_documentShowing.WindowingMax==0) {//default
					windowingSlider.MinVal=0;
					windowingSlider.MaxVal=255;
				}
				else {
					windowingSlider.MinVal=_documentShowing.WindowingMin;
					windowingSlider.MaxVal=_documentShowing.WindowingMax;
				}
				return;
			}
			if(IsMountItemShowing()){
				if(_arrayDocumentsShowing[_idxSelectedInMount].WindowingMax==0) {//default
					windowingSlider.MinVal=0;
					windowingSlider.MaxVal=255;
				}
				else {
					windowingSlider.MinVal=_arrayDocumentsShowing[_idxSelectedInMount].WindowingMin;
					windowingSlider.MaxVal=_arrayDocumentsShowing[_idxSelectedInMount].WindowingMax;
				}
			}
			else{
				windowingSlider.MinVal=0;
				windowingSlider.MaxVal=255;
			}
		}

		private void sigBox_DoubleClick(object sender, EventArgs e){
			ToolBarSign_Click();
		}

		private void sigBoxTopaz_DoubleClick(object sender,EventArgs e) {
			ToolBarSign_Click();
		}

		private void textNote_DoubleClick(object sender, EventArgs e){
			ToolBarSign_Click();
		}

		private void ToolBarAdd_Click(){
			if(!IsMountShowing()){
				return;
			}
			if(_idxSelectedInMount==-1 || _arrayDocumentsShowing[_idxSelectedInMount]!=null){
				MessageBox.Show("Please select an empty mount item, first.");
				return;
			}
			FormImageSelect formImageSelect=new FormImageSelect();
			formImageSelect.Text="Select an image to copy into the mount";
			formImageSelect.PatNum=_patCur.PatNum;
			formImageSelect.OnlyShowImages=true;
			formImageSelect.ShowDialog();
			if(formImageSelect.DialogResult!=DialogResult.OK){
				return;
			}
			Document documentOrig=Documents.GetByNum(formImageSelect.SelectedDocNum,true);
			if(documentOrig==null){
				return;
			}
			Bitmap bitmapOrig=null;
			Action actionCloseDownloadProgress=null;
			if(CloudStorage.IsCloudStorage) {
				actionCloseDownloadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.G("ContrImages","Downloading..."));
			}
			bitmapOrig=ImageStore.OpenImage(documentOrig,_patFolder);//PDF files will always return null.
			actionCloseDownloadProgress?.Invoke();
			if(bitmapOrig==null) {
				MessageBox.Show("Error opening file.");
				return;
			}
			Document documentNew=null;
			try {
				documentNew=ImageStore.Import(bitmapOrig,GetCurrentCategory(),ImageType.Photo,_patCur);//Makes log entry
			}
			catch {
				MessageBox.Show("Error saving image.");
				bitmapOrig.Dispose();
				return;
			}
			Document documentOld=documentNew.Copy();
			documentNew.MountItemNum=_listMountItems[_idxSelectedInMount].MountItemNum;
			Documents.Update(documentNew,documentOld);
			_arrayDocumentsShowing[_idxSelectedInMount]=documentNew;
			_bitmapRaw=new Bitmap(bitmapOrig);
			_arrayBitmapsShowing[_idxSelectedInMount]=ImageHelper.ApplyDocumentSettingsToImage2(
				_arrayDocumentsShowing[_idxSelectedInMount],_bitmapRaw, ImageSettingFlags.COLORFUNCTION);
			bitmapOrig.Dispose();
			panelMain.Invalidate();
		}

		private void ToolBarCopy_Click(){
			//not enabled when pdf selected
			Bitmap bitmapCopy=null;
			string fileName="";
			Cursor=Cursors.WaitCursor;
			if(IsMountItemShowing()){
				fileName=_arrayDocumentsShowing[_idxSelectedInMount].FileName;
				//color function has already been applied to the bitmapShowing
				bitmapCopy=ImageHelper.ApplyDocumentSettingsToImage2(_arrayDocumentsShowing[_idxSelectedInMount],_arrayBitmapsShowing[_idxSelectedInMount],	
					ImageSettingFlags.FLIP | ImageSettingFlags.ROTATE | ImageSettingFlags.CROP);
			}
			else if(IsMountShowing()){
				fileName=_patCur.LName+_patCur.FName+".jpg";
				bitmapCopy=new Bitmap(_mountShowing.Width,_mountShowing.Height);
				Graphics g=Graphics.FromImage(bitmapCopy);
				g.TranslateTransform(bitmapCopy.Width/2,bitmapCopy.Height/2);//Center of image
				DrawMount(g);
				g.Dispose();
			}
			else if(IsDocumentShowing()){
				fileName=_documentShowing.FileName;
				bitmapCopy=ImageHelper.ApplyDocumentSettingsToImage2(_documentShowing,_bitmapShowing,	
					ImageSettingFlags.FLIP | ImageSettingFlags.ROTATE | ImageSettingFlags.CROP);
			}
			else{
				Cursor=Cursors.Default;
				MessageBox.Show("Please select an image before copying");
				return;
			}
			if(bitmapCopy==null) {
				Cursor=Cursors.Default;
				MessageBox.Show("Error.");
				return;
			}
			try {
				DataObject dataObject=new DataObject();
				dataObject.SetData(DataFormats.Bitmap,bitmapCopy);
				fileName=Path.GetTempPath()+fileName;
					//Path.GetTempFileName().Replace(".tmp",".jpg");
				bitmapCopy.Save(fileName);
				string[] stringArray=new string[1];
				stringArray[0]=fileName;
				dataObject.SetData(DataFormats.FileDrop,stringArray);
				Clipboard.SetDataObject(dataObject);
			}
			catch {
				MessageBox.Show("Could not copy contents to the clipboard.  Please try again.");
				return;
			}
			//Can't do this, or the clipboard object goes away.
			//bitmapCopy.Dispose();
			//bitmapCopy=null;
			long patNum=0;
			if(_patCur!=null) {
				patNum=_patCur.PatNum;
			}
			Cursor=Cursors.Default;
			if(IsMountItemShowing()){
				SecurityLogs.MakeLogEntry(Permissions.Copy,patNum,"Patient image "+_arrayDocumentsShowing[_idxSelectedInMount].FileName+" copied to clipboard");
			}
			else if(IsMountShowing()){
				SecurityLogs.MakeLogEntry(Permissions.Copy,patNum,"Patient mount "+_mountShowing.Description+" copied to clipboard");
			}
			else if(IsDocumentShowing()){
				SecurityLogs.MakeLogEntry(Permissions.Copy,patNum,"Patient image "+_documentShowing.FileName+" copied to clipboard");
			}
		}

		private void ToolBarDelete_Click(){
			if(treeMain.SelectedNode==null) {
				MessageBox.Show("No item is currently selected");
				return;
			}
			if(IsDocumentShowing()){
				DeleteDocument(true,true,_documentShowing);
				return;
			}
			if(IsMountItemShowing()){
				DeleteDocument(true,true,_arrayDocumentsShowing[_idxSelectedInMount]);
				return;
			}
			if(IsMountShowing()){
				bool isEmpty=true;
				for(int i=0;i<_arrayDocumentsShowing.Length;i++){
					if(_arrayDocumentsShowing[i]!=null){
						isEmpty=false;
					}
				}
				if(!isEmpty){
					MessageBox.Show("Not allowed to delete entire mount.  Delete images one at a time.");
					return;
				}
				Mounts.Delete(_mountShowing);
				FillTree(false);
				panelMain.Invalidate();
			}
		}

		private void ToolBarExport_Click(){
			if(IsDocumentShowing()){
				SaveFileDialog saveFileDialog=new SaveFileDialog();
				saveFileDialog.Title="Export a Document";
				saveFileDialog.FileName=_documentShowing.FileName;
				saveFileDialog.DefaultExt=Path.GetExtension(_documentShowing.FileName);
				if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
					return;
				}
				try {
					ImageStore.Export(saveFileDialog.FileName,_documentShowing,_patCur);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.G(this,"Unable to export file, May be in use")+": " + ex.Message);
					return;
				}
				MessageBox.Show("Done.");
				return;
			}
			if(IsMountItemShowing()){
				SaveFileDialog saveFileDialog=new SaveFileDialog();
				saveFileDialog.Title="Export Image";
				saveFileDialog.FileName=_arrayDocumentsShowing[_idxSelectedInMount].FileName;
				saveFileDialog.DefaultExt=Path.GetExtension(_arrayDocumentsShowing[_idxSelectedInMount].FileName);
				if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
					return;
				}
				try {
					ImageStore.Export(saveFileDialog.FileName,_arrayDocumentsShowing[_idxSelectedInMount],_patCur);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.G(this,"Unable to export file, May be in use")+": " + ex.Message);
					return;
				}
				MessageBox.Show("Done.");
				return;
			}
			if(IsMountShowing()){
				SaveFileDialog saveFileDialog=new SaveFileDialog();
				saveFileDialog.Title="Export Mount";
				saveFileDialog.FileName=".jpg";//_documentShowing.FileName;
				saveFileDialog.DefaultExt=".jpg";
				if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
					return;
				}
				Bitmap bitmapExport=new Bitmap(_mountShowing.Width,_mountShowing.Height);
				Graphics g=Graphics.FromImage(bitmapExport);
				g.TranslateTransform(bitmapExport.Width/2,bitmapExport.Height/2);//Center of image
				DrawMount(g);
				g.Dispose();
				try {
					bitmapExport.Save(saveFileDialog.FileName);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.G(this,"Unable to export file, May be in use")+": " + ex.Message);
					return;
				}
				bitmapExport.Dispose();
				MessageBox.Show("Done.");
				return;
			}
		}

		private void ToolBarExport_ClickWeb(){
			if(IsDocumentShowing()){
				string tempFilePath=ODFileUtils.CombinePaths(Path.GetTempPath(),_documentShowing.FileName);
				string docPath=FileAtoZ.CombinePaths(ImageStore.GetPatientFolder(_patCur,ImageStore.GetPreferredAtoZpath()),_documentShowing.FileName);
				FileAtoZ.Copy(docPath,tempFilePath,FileAtoZSourceDestination.AtoZToLocal,"Exporting file...");
				ThinfinityUtils.ExportForDownload(tempFilePath);
				MessageBox.Show("Done.");
				return;
			}
			if(IsMountItemShowing()){
				string tempFilePath=ODFileUtils.CombinePaths(Path.GetTempPath(),_arrayDocumentsShowing[_idxSelectedInMount].FileName);
				string docPath=FileAtoZ.CombinePaths(ImageStore.GetPatientFolder(_patCur,ImageStore.GetPreferredAtoZpath()),_arrayDocumentsShowing[_idxSelectedInMount].FileName);
				FileAtoZ.Copy(docPath,tempFilePath,FileAtoZSourceDestination.AtoZToLocal,"Exporting file...");
				ThinfinityUtils.ExportForDownload(tempFilePath);
				MessageBox.Show("Done.");
				return;
			}
			if(IsMountShowing()){
				string tempFilePath=ODFileUtils.CombinePaths(Path.GetTempPath(),"mount.jpg");
				Bitmap bitmapExport=new Bitmap(_mountShowing.Width,_mountShowing.Height);
				Graphics g=Graphics.FromImage(bitmapExport);
				g.TranslateTransform(bitmapExport.Width/2,bitmapExport.Height/2);//Center of image
				DrawMount(g);
				g.Dispose();
				bitmapExport.Save(tempFilePath);
				ThinfinityUtils.ExportForDownload(tempFilePath);
				bitmapExport.Dispose();
				MessageBox.Show("Done.");
				return;
			}
		}

		private void ToolBarFlip_Click(){
			if(IsDocumentShowing()) {
				Document docOld=_documentShowing.Copy();
				_documentShowing.IsFlipped=!_documentShowing.IsFlipped;
				//Document is always flipped and then rotated when drawn, but we want to flip it
				//horizontally no matter what orientation it's in right now
				if(_documentShowing.DegreesRotated==90){
					_documentShowing.DegreesRotated=270;
				}
				else if(_documentShowing.DegreesRotated==270){
					_documentShowing.DegreesRotated=90;
				}
				Documents.Update(_documentShowing,docOld);
				ImageStore.DeleteThumbnailImage(_documentShowing,_patFolder);
			}
			if(IsMountShowing() && _idxSelectedInMount>-1 && _arrayDocumentsShowing[_idxSelectedInMount]!=null){
				Document docOld=_arrayDocumentsShowing[_idxSelectedInMount].Copy();
				_arrayDocumentsShowing[_idxSelectedInMount].IsFlipped=!_arrayDocumentsShowing[_idxSelectedInMount].IsFlipped;
				if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated==90){
					_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated=270;
				}
				else if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated==270){
					_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated=90;
				}
				Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],docOld);
				ImageStore.DeleteThumbnailImage(_arrayDocumentsShowing[_idxSelectedInMount],_patFolder);
			}
			panelMain.Invalidate();
		}

		private void ToolBarImport_Click(){
			if(IsMountShowing()){
				ToolBarImportMount();
			}
			else{//including nothing selected
				ToolBarImportSingle();
			}
		}

		private void ToolBarImportMount(){
			//supports multiple file imports, so user doesn't actually need to select a mount item first.
			OpenFileDialog openFileDialog=new OpenFileDialog();
			openFileDialog.Multiselect=true;
			if(Prefs.GetContainsKey(nameof(PrefName.UseAlternateOpenFileDialogWindow)) && PrefC.GetBool(PrefName.UseAlternateOpenFileDialogWindow)){//Hidden pref, almost always false.
				//We don't know why this makes any difference but people have mentioned this will stop some hanging issues.
				//https://stackoverflow.com/questions/6718148/windows-forms-gui-hangs-when-calling-openfiledialog-showdialog
				openFileDialog.ShowHelp=true;
			}
			if(openFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string[] fileNames=openFileDialog.FileNames;
			if(fileNames.Length<1) {
				return;
			}
			List<MountItem> listAvail=GetAvailSlots(fileNames.Length);
			if(listAvail==null){
				return;
			}
			Document doc=null;
			Action actionCloseUploadProgress=null;
			if(CloudStorage.IsCloudStorage) {
				actionCloseUploadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.G("ContrImages","Uploading..."));
			}
			for(int i=0;i<fileNames.Length;i++) {
				try {
					//.FileName is full path
					doc=ImageStore.Import(fileNames[i],GetCurrentCategory(),_patCur);//Makes log
					Document docOld=doc.Copy();
					doc.MountItemNum=listAvail[i].MountItemNum;
					Documents.Update(doc,docOld);
				}
				catch(Exception ex) {
					actionCloseUploadProgress?.Invoke();
					MessageBox.Show(Lan.G(this,"Unable to copy file, May be in use: ")+ex.Message+": "+fileNames[i]);
					continue;
				}
				//this is all far too complicated:
				//_arrayDocumentsShowing[_idxSelectedInMount]=doc.Copy();
				//_arrayBitmapsRaw[_idxSelectedInMount]=ImageStore.OpenImage(_arrayDocumentsShowing[_idxSelectedInMount],_patFolder,openFileDialog.FileName);
				//_arrayBitmapsShowing[_idxSelectedInMount]=ImageHelper.ApplyDocumentSettingsToImage2(
				//	_arrayDocumentsShowing[_idxSelectedInMount],_arrayBitmapsRaw[_idxSelectedInMount], ImageSettingFlags.CROP | ImageSettingFlags.COLORFUNCTION);
			}
			actionCloseUploadProgress?.Invoke();
			SelectTreeNode(new NodeObjTag(EnumNodeType.Mount,_mountShowing.MountNum));
		}

		private List<MountItem> GetAvailSlots(int countNeed){
			//make a list of all the empty mount slots
			List<MountItem> listAvail=new List<MountItem>();
			int idxItem=_idxSelectedInMount;
			if(idxItem==-1){
				idxItem=0;
			}
			//idxItem could be in the middle of _listMountItems.  
			while(idxItem<_listMountItems.Count){
				if(_arrayDocumentsShowing[idxItem]!=null){
					//occupied
					idxItem++;
					continue;
				}
				listAvail.Add(_listMountItems[idxItem]);
				idxItem++;
			}
			if(_idxSelectedInMount>0){//Second loop to catch items lower than the first loop
				idxItem=0;
				while(idxItem<_idxSelectedInMount){
					if(_arrayDocumentsShowing[idxItem]!=null){
						idxItem++;
						continue;
					}
					listAvail.Add(_listMountItems[idxItem]);
					idxItem++;
				}
			}
			if(listAvail.Count==0){
				MsgBox.Show("No available slots in the mount.");
				return null;
			}
			if(listAvail.Count<countNeed){
				MsgBox.Show("Not enough slots in the mount for the number of files selected.");
				return null;
			}
			return listAvail;
		}

		private void ToolBarImportSingle() {
			if(Plugins.HookMethod(this,"ContrImages.ToolBarImport_Click_Start",_patCur)) {//Named differently for backwards compatibility
				FillTree(true);
				return;
			}
			OpenFileDialog openFileDialog=new OpenFileDialog();
			openFileDialog.Multiselect=true;
			if(Prefs.GetContainsKey(nameof(PrefName.UseAlternateOpenFileDialogWindow)) && PrefC.GetBool(PrefName.UseAlternateOpenFileDialogWindow)){//Hidden pref, almost always false.
				//We don't know why this makes any difference but people have mentioned this will stop some hanging issues.
				//https://stackoverflow.com/questions/6718148/windows-forms-gui-hangs-when-calling-openfiledialog-showdialog
				openFileDialog.ShowHelp=true;
			}
			if(openFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string[] fileNames=openFileDialog.FileNames;
			if(fileNames.Length<1) {
				return;
			}
			NodeObjTag nodeObjTag=null;
			Document doc=null;
			Action actionCloseUploadProgress=null;
			for(int i=0;i<fileNames.Length;i++) {
				if(CloudStorage.IsCloudStorage) {
					actionCloseUploadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage: Lan.G("ContrImages","Uploading..."));
				}
				try {
					doc=ImageStore.Import(fileNames[i],GetCurrentCategory(),_patCur);//Makes log
					actionCloseUploadProgress?.Invoke();
				}
				catch(Exception ex) {
					actionCloseUploadProgress?.Invoke();
					MessageBox.Show(Lan.G(this,"Unable to copy file, May be in use: ")+ex.Message+": "+openFileDialog.FileName);
					continue;
				}
				FillTree(false);
				SelectTreeNode(new NodeObjTag(EnumNodeType.Document,doc.DocNum),fileNames[i]);
				FormDocInfo FormD=new FormDocInfo(_patCur,doc);
				FormD.TopMost=true;
				FormD.ShowDialog(this);//some of the fields might get changed, but not the filename
				if(FormD.DialogResult!=DialogResult.OK) {
					DeleteDocument(false,false,doc);
				}
				else {
					if(doc.ImgType==ImageType.Photo) {
						PatientEvent.Fire(ODEventType.Patient,_patCur);//Possibly updated the patient picture.
					}
					nodeObjTag=new NodeObjTag(EnumNodeType.Document,doc.DocNum);
					_documentShowing=doc.Copy();
				}
			}
			//Reselect the last successfully added node when necessary.
			if(doc!=null && !new NodeObjTag(EnumNodeType.Document,doc.DocNum).Equals(nodeObjTag)) {
				TreeNode treeNode=GetTreeNodeByKey(new NodeObjTag(EnumNodeType.Document,doc.DocNum));
				if(treeNode!=null) {
					SelectTreeNode((NodeObjTag)treeNode.Tag,fileNames[fileNames.Length-1]);
				}
			}
			FillTree(true);
		}

		private void ToolBarInfo_Click(){
			if(IsMountItemShowing()){
				if(_arrayDocumentsShowing[_idxSelectedInMount]==null){
					return;//silent fail is fine
				}
				FormDocInfo formDocInfo=new FormDocInfo(_patCur,_arrayDocumentsShowing[_idxSelectedInMount]);
				formDocInfo.ShowDialog();
				LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.IdxAndRaw);
				panelMain.Invalidate();
				return;
			}
			if(IsMountShowing()) {
				FormMountEdit formMountEdit=new FormMountEdit(_mountShowing);
				formMountEdit.ShowDialog();
				if(formMountEdit.DialogResult!=DialogResult.OK) {
					return;
				}
				Cursor=Cursors.WaitCursor;//because it can take a few seconds to reload.
				FillTree(true);
				Cursor=Cursors.Default;
				return;
			}
			if(IsDocumentShowing()) {
				FormDocInfo formDocInfo=new FormDocInfo(_patCur,_documentShowing);
				formDocInfo.ShowDialog();
				if(formDocInfo.DialogResult!=DialogResult.OK) {
					return;
				}
				FillTree(true);
			}
		}

		private void ToolBarPaste_Click(){
			Bitmap bitmapPaste;
			string[] fileNames=null;
			try {
				bitmapPaste = ODClipboard.Image;
				if(bitmapPaste==null && Clipboard.ContainsFileDropList()) {
					IDataObject iDataObject=Clipboard.GetDataObject();
					if(iDataObject.GetDataPresent(DataFormats.FileDrop)) {
						fileNames=(string[])iDataObject.GetData(DataFormats.FileDrop);
						bitmapPaste=(Bitmap)Bitmap.FromFile(fileNames[0]);
					}
				}
			}
			catch {
				MessageBox.Show("Could not paste contents from the clipboard.  Please try again.");
				return;
			}
			if(bitmapPaste==null){
				MessageBox.Show(Lan.G(this,"No bitmap present on clipboard"));
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(IsMountShowing()){
				if(fileNames!=null){
					//fileDrop supports multiple files, and we don't care if they've selected a mount item.
					List<MountItem> listAvail=GetAvailSlots(fileNames.Length);
					if(listAvail==null){
						Cursor=Cursors.Default;
						return;
					}
					Document doc=null;
					Action actionCloseUploadProgress=null;
					if(CloudStorage.IsCloudStorage) {
						actionCloseUploadProgress=ODProgress.Show(ODEventType.ContrImages,startingMessage:Lan.G("ContrImages","Uploading..."));
					}
					for(int i=0;i<fileNames.Length;i++) {
						try {
							//fileName is full path
							doc=ImageStore.Import(fileNames[i],GetCurrentCategory(),_patCur);//Makes log
							Document docOld=doc.Copy();
							doc.MountItemNum=listAvail[i].MountItemNum;
							Documents.Update(doc,docOld);
						}
						catch(Exception ex) {
							actionCloseUploadProgress?.Invoke();
							Cursor=Cursors.Default;
							MessageBox.Show(Lan.G(this,"Unable to copy file, May be in use: ")+ex.Message+": "+fileNames[i]);
							continue;
						}
					}
					actionCloseUploadProgress?.Invoke();
					Cursor=Cursors.Default;
					SelectTreeNode(new NodeObjTag(EnumNodeType.Mount,_mountShowing.MountNum));
				}
				else{//one bitmap
					if(_idxSelectedInMount==-1 || _arrayDocumentsShowing[_idxSelectedInMount]!=null){
						Cursor=Cursors.Default;
						MessageBox.Show(Lan.G(this,"Please select an empty mount item, first."));
						return;
					}
					try {
						Document doc=ImageStore.Import(bitmapPaste,GetCurrentCategory(),ImageType.Photo,_patCur);//Makes log
						Document docOld=doc.Copy();
						doc.MountItemNum=_listMountItems[_idxSelectedInMount].MountItemNum;
						Documents.Update(doc,docOld);
					}
					catch(Exception ex) {
						Cursor=Cursors.Default;
						MessageBox.Show(Lan.G(this,"Unable to paste bitmap: ")+ex.Message);
					}
					Cursor=Cursors.Default;
					SelectTreeNode(new NodeObjTag(EnumNodeType.Mount,_mountShowing.MountNum));
				}//bitmap
				return;
			}//mount
			//all other situations are for document, but we cannot test for that. Needs to work for cat selected, nothing selected, existing doc selected, etc.
			Document document;
			try {
				document=ImageStore.Import(bitmapPaste,GetCurrentCategory(),ImageType.Photo,_patCur);//Makes log entry
			}
			catch {
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.G(this,"Error saving document."));
				return;
			}
			FillTree(false);
			SelectTreeNode(new NodeObjTag(EnumNodeType.Document,document.DocNum));
			Cursor=Cursors.Default;
			FormDocInfo formD=new FormDocInfo(_patCur,document);
			formD.ShowDialog(this);
			if(formD.DialogResult!=DialogResult.OK) {
				DeleteDocument(false,false,document);
			}
			else {
				_documentShowing=document.Copy();
				FillTree(true);
			}
			panelMain.Invalidate();
		}

		private void ToolBarPrint_Click(){
			if(!IsDocumentShowing() && !IsMountShowing()){
				MessageBox.Show("Cannot print. No document currently selected.");
				return;
			}
			if(IsDocumentShowing()){
				if(Path.GetExtension(_documentShowing.FileName).ToLower()==".pdf") {//Selected document is PDF, we handle differently than documents that aren't pdf.

						//This line will not work if _webBrowser.ReadyState==Uninitialized.
						_webBrowser.ShowPrintPreviewDialog();
					
					SecurityLogs.MakeLogEntry(Permissions.Printing,_patCur.PatNum,"Patient PDF "+_documentShowing.FileName+" "+_documentShowing.Description+" printed");
					return;
				}
			}
			PrinterL.TryPrintOrDebugClassicPreview(printDocument_PrintPage,Lan.G(this,"Image printed."));
			if(IsDocumentShowing()){
				SecurityLogs.MakeLogEntry(Permissions.Printing,_patCur.PatNum,"Patient image "+_documentShowing.FileName+" "+_documentShowing.Description+" printed");
			}
			if(IsMountShowing()){
				SecurityLogs.MakeLogEntry(Permissions.Printing,_patCur.PatNum,"Patient mount "+_mountShowing.Description+" printed");
			}
		}

		private void ToolBarRemove_Click(){
			if(!IsMountShowing()){
				return;
			}
			if(_idxSelectedInMount==-1 || _arrayDocumentsShowing[_idxSelectedInMount]==null){
				MessageBox.Show("Please select a mount item, first.");
				return;
			}
			//Database shows file, but missing in OpenDentImages
			if(_arrayDocumentsShowing[_idxSelectedInMount]!=null && _arrayBitmapsShowing[_idxSelectedInMount]==null) {
				MessageBox.Show(Lan.G(this,"File not found")+": " + _arrayDocumentsShowing[_idxSelectedInMount].FileName+"\r\n"
					+Lan.G(this,"Return the file to the A-Z folder or click Delete to remove this mount item."));
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Remove this image from the mount and place it in the documents folder as a standalone image?")){
				return;
			}
			Document documentOld=_arrayDocumentsShowing[_idxSelectedInMount].Copy();
			_arrayDocumentsShowing[_idxSelectedInMount].MountItemNum=0;
			_arrayDocumentsShowing[_idxSelectedInMount].DocCategory=_mountShowing.DocCategory;
			Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],documentOld);
			_arrayDocumentsShowing[_idxSelectedInMount]=null;
			_bitmapRaw?.Dispose();
			_bitmapRaw=null;
			_arrayBitmapsShowing[_idxSelectedInMount].Dispose();
			_arrayBitmapsShowing[_idxSelectedInMount]=null;
			FillTree(true);
			panelMain.Invalidate();
		}

		private void ToolBarRotateL_Click(){
			if(IsDocumentShowing()) {
				Document docOld=_documentShowing.Copy();
				_documentShowing.DegreesRotated-=90;
				while(_documentShowing.DegreesRotated<0) {
					_documentShowing.DegreesRotated+=360;
				}
				Documents.Update(_documentShowing,docOld);
				ImageStore.DeleteThumbnailImage(_documentShowing,_patFolder);
				SetZoomSlider();
			}
			if(IsMountShowing() && _idxSelectedInMount>-1 && _arrayDocumentsShowing[_idxSelectedInMount]!=null){
				Document docOld=_arrayDocumentsShowing[_idxSelectedInMount].Copy();
				if(_arrayDocumentsShowing[_idxSelectedInMount].CropW != 0){
					//find center point of crop area, in image coords
					double xCenter=_arrayDocumentsShowing[_idxSelectedInMount].CropX+_arrayDocumentsShowing[_idxSelectedInMount].CropW/2;
					double yCenter=_arrayDocumentsShowing[_idxSelectedInMount].CropY+_arrayDocumentsShowing[_idxSelectedInMount].CropH/2;
					//ratio changes, causing change to x,y,w,h
					double wNew=_arrayDocumentsShowing[_idxSelectedInMount].CropH;
					double hNew=_arrayDocumentsShowing[_idxSelectedInMount].CropW;
					double xNew=xCenter-wNew/2;
					double yNew=yCenter-hNew/2;
					_arrayDocumentsShowing[_idxSelectedInMount].CropX=(int)xNew;
					_arrayDocumentsShowing[_idxSelectedInMount].CropY=(int)yNew;
					_arrayDocumentsShowing[_idxSelectedInMount].CropW=(int)wNew;
					_arrayDocumentsShowing[_idxSelectedInMount].CropH=(int)hNew;
				}
				_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated-=90;
				while(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated<0) {
					_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated+=360;
				}
				Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],docOld);
				ImageStore.DeleteThumbnailImage(_arrayDocumentsShowing[_idxSelectedInMount],_patFolder);
				//probably not necessary with existing crop because scale hasn't changed.
				LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.IdxAndRaw);
			}
			panelMain.Invalidate();
		}

		private void ToolBarRotateR_Click(){
			if(IsDocumentShowing()) {
				Document docOld=_documentShowing.Copy();
				_documentShowing.DegreesRotated+=90;
				_documentShowing.DegreesRotated%=360;
				Documents.Update(_documentShowing,docOld);
				ImageStore.DeleteThumbnailImage(_documentShowing,_patFolder);
				SetZoomSlider();
			}
			if(IsMountShowing() && _idxSelectedInMount>-1 && _arrayDocumentsShowing[_idxSelectedInMount]!=null){
				Document docOld=_arrayDocumentsShowing[_idxSelectedInMount].Copy();
				if(_arrayDocumentsShowing[_idxSelectedInMount].CropW != 0){
					//find center point of crop area, in image coords
					double xCenter=_arrayDocumentsShowing[_idxSelectedInMount].CropX+_arrayDocumentsShowing[_idxSelectedInMount].CropW/2;
					double yCenter=_arrayDocumentsShowing[_idxSelectedInMount].CropY+_arrayDocumentsShowing[_idxSelectedInMount].CropH/2;
					//ratio changes, causing change to x,y,w,h
					double wNew=_arrayDocumentsShowing[_idxSelectedInMount].CropH;
					double hNew=_arrayDocumentsShowing[_idxSelectedInMount].CropW;
					double xNew=xCenter-wNew/2;
					double yNew=yCenter-hNew/2;
					_arrayDocumentsShowing[_idxSelectedInMount].CropX=(int)xNew;
					_arrayDocumentsShowing[_idxSelectedInMount].CropY=(int)yNew;
					_arrayDocumentsShowing[_idxSelectedInMount].CropW=(int)wNew;
					_arrayDocumentsShowing[_idxSelectedInMount].CropH=(int)hNew;
				}
				_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated+=90;
				_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated%=360;
				Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],docOld);
				ImageStore.DeleteThumbnailImage(_arrayDocumentsShowing[_idxSelectedInMount],_patFolder);
				LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.IdxAndRaw);
			}
			panelMain.Invalidate();
		}

		private void ToolBarRotate180_Click(){
			if(IsDocumentShowing()){
				Document docOld=_documentShowing.Copy();
				if(_documentShowing.DegreesRotated>=180){
					_documentShowing.DegreesRotated-=180;
				}
				else{
					_documentShowing.DegreesRotated+=180;
				}
				Documents.Update(_documentShowing,docOld);
				ImageStore.DeleteThumbnailImage(_documentShowing,_patFolder);
				SetZoomSlider();
			}
			if(IsMountShowing() && _idxSelectedInMount>-1 && _arrayDocumentsShowing[_idxSelectedInMount]!=null){
				Document docOld=_arrayDocumentsShowing[_idxSelectedInMount].Copy();
				if(_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated>=180){
					_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated-=180;
				}
				else{
					_arrayDocumentsShowing[_idxSelectedInMount].DegreesRotated+=180;
				}
				Documents.Update(_arrayDocumentsShowing[_idxSelectedInMount],docOld);
				ImageStore.DeleteThumbnailImage(_arrayDocumentsShowing[_idxSelectedInMount],_patFolder);
				//LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.IdxAndRaw);//no change to scale
			}
			panelMain.Invalidate();
		}

		///<summary>Valid values for scanType are "doc","xray",and "photo"</summary>
		private void ToolBarScan_Click(string scanType){
			Cursor=Cursors.WaitCursor;
			Bitmap bitmapScanned=null;
			IntPtr hdib=IntPtr.Zero;
			try {
				xImageDeviceManager.Obfuscator.ActivateEZTwain();
			}
			catch {
				Cursor=Cursors.Default;
				MessageBox.Show("EzTwain4.dll not found.  Please run the setup file in your images folder.");
				return;
			}
			if(IsDisposed) {
				return;//Possibly the form was being closed as the user clicked the scan button. 
			}
			if(ComputerPrefs.LocalComputer.ScanDocSelectSource) {
				if(!EZTwain.SelectImageSource(this.Handle)) {//dialog to select source
					Cursor=Cursors.Default;
					return;//User clicked cancel.
				}
			}
			EZTwain.SetHideUI(!ComputerPrefs.LocalComputer.ScanDocShowOptions);
			if(!EZTwain.OpenDefaultSource()) {//if it can't open the scanner successfully
				Cursor=Cursors.Default;
				MessageBox.Show("Default scanner could not be opened.  Check that the default scanner works from Windows Control Panel and from Windows Fax and Scan.");
				return;
			}
			EZTwain.SetResolution(ComputerPrefs.LocalComputer.ScanDocResolution);
			if(ComputerPrefs.LocalComputer.ScanDocGrayscale) {
				EZTwain.SetPixelType(1);//8-bit grayscale - only set if scanner dialog will not show
			}
			else {
				EZTwain.SetPixelType(2);//24-bit color
			}
			EZTwain.SetJpegQuality(ComputerPrefs.LocalComputer.ScanDocQuality);
			EZTwain.SetXferMech(EZTwain.XFERMECH_MEMORY);
			Cursor=Cursors.Default;
			hdib=EZTwain.Acquire(this.Handle);//This is where the options dialog would come up. The settings above will not populate this window.
			int errorCode=EZTwain.LastErrorCode();
			if(errorCode!=0) {
				string message="";
				if(errorCode==(int)EZTwainErrorCode.EZTEC_USER_CANCEL) {//19
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_JPEG_DLL) {//22
					message="Missing dll\r\n\r\nRequired file EZJpeg.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_0_PAGES) {//38
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_NO_PDF) {//43
					message="Missing dll\r\n\r\nRequired file EZPdf.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_DEVICE_PAPERJAM) {//76
					message="Paper jam\r\n\r\nPlease check the scanner document feeder and ensure there path is clear of any paper jams.";
				}
				else {
					message=errorCode+" "+((EZTwainErrorCode)errorCode).ToString();
				}
				MessageBox.Show(Lan.G(this,"Unable to scan. Please make sure you can scan using other software. Error: "+message));
				return;
			}
			if(hdib==(IntPtr)0) {//This is down here because there might also be an informative error code that we would like to use above.
				return;//User cancelled
			}
			double xdpi=EZTwain.DIB_XResolution(hdib);
			double ydpi=EZTwain.DIB_XResolution(hdib);
			IntPtr hbitmap=EZTwain.DIB_ToDibSection(hdib);
			try {
				bitmapScanned=Bitmap.FromHbitmap(hbitmap);//Sometimes throws 'A generic error occurred in GDI+.'
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.G(this,"Error importing eob")+": "+ex.Message,ex);
				return;
			}			
			bitmapScanned.SetResolution((float)xdpi,(float)ydpi);
			try {
				Clipboard.SetImage(bitmapScanned);//We do this because a customer requested it, and some customers probably use it.
			}
			catch {
				//Rarely, setting the clipboard image fails, in which case we should ignore the failure because most people do not use this feature.
			}
			ImageType imgType;
			if(scanType=="xray") {
				imgType=ImageType.Radiograph;
			}
			else if(scanType=="photo") {
				imgType=ImageType.Photo;
			}
			else {//Assume document
				imgType=ImageType.Document;
			}
			bool saved=true;
			Document doc = null;
			try {//Create corresponding image file.
				doc=ImageStore.Import(bitmapScanned,GetCurrentCategory(),imgType,_patCur);
			}
			catch(Exception ex) {
				saved=false;
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.G(this,"Unable to save document")+": "+ex.Message);
			}
			if(bitmapScanned!=null) {
				bitmapScanned.Dispose();
				bitmapScanned=null;
			}
			if(hdib!=IntPtr.Zero) {
				EZTwain.DIB_Free(hdib);
			}
			Cursor=Cursors.Default;
			if(saved) {
				FillTree(false);//Reload and keep new document selected.
				SelectTreeNode(new NodeObjTag(EnumNodeType.Document,doc.DocNum));
				FormDocInfo formDocInfo=new FormDocInfo(_patCur,_documentShowing);
				formDocInfo.ShowDialog(this);
				if(formDocInfo.DialogResult!=DialogResult.OK) {
					DeleteDocument(false,false,doc);
				}
				else {
					FillTree(true);//Update tree, in case the new document's icon or category were modified in formDocInfo.
				}
			}
		}

		private void ToolBarScanMulti_Click() {
			string tempFile=PrefC.GetRandomTempFile(".pdf");
			try {
				xImageDeviceManager.Obfuscator.ActivateEZTwain();
			}
			catch {
				Cursor=Cursors.Default;
				MessageBox.Show("EzTwain4.dll not found.  Please run the setup file in your images folder.");
				return;
			}
			if(IsDisposed) {
				return;//Possibly the form was being closed as the user clicked the scan button. 
			}
			if(ComputerPrefs.LocalComputer.ScanDocSelectSource) {
				if(!EZTwain.SelectImageSource(this.Handle)) {
					return;//User clicked cancel.
				}
			}
			//EZTwain.LogFile(7);//Writes at level 7 (very detailed) in the C:\eztwain.log text file. Useful for getting help from EZTwain support on their forum.
			EZTwain.SetHideUI(!ComputerPrefs.LocalComputer.ScanDocShowOptions);
			EZTwain.PDF_SetCompression((int)this.Handle,(int)ComputerPrefs.LocalComputer.ScanDocQuality);
			if(!EZTwain.OpenDefaultSource()) {//if it can't open the scanner successfully
				MessageBox.Show("Default scanner could not be opened.  Check that the default scanner works from Windows Control Panel and from Windows Fax and Scan.");
				Cursor=Cursors.Default;
				return;
			}
			bool duplexEnabled=EZTwain.EnableDuplex(ComputerPrefs.LocalComputer.ScanDocDuplex);//This line seems to cause problems.
			if(ComputerPrefs.LocalComputer.ScanDocGrayscale) {
				EZTwain.SetPixelType(1);//8-bit grayscale
			}
			else {
				EZTwain.SetPixelType(2);//24-bit color
			}
			EZTwain.SetResolution(ComputerPrefs.LocalComputer.ScanDocResolution);
			EZTwain.AcquireMultipageFile(this.Handle,tempFile);//This is where the options dialog will come up if enabled. This will ignore and override the settings above.
			int errorCode=EZTwain.LastErrorCode();
			if(errorCode!=0) {
				string message="";
				if(errorCode==(int)EZTwainErrorCode.EZTEC_USER_CANCEL) {//19
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_JPEG_DLL) {//22
					message="Missing dll\r\n\r\nRequired file EZJpeg.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_0_PAGES) {//38
					//message="\r\nScanning cancelled.";//do nothing
					return;
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_NO_PDF) {//43
					message="Missing dll\r\n\r\nRequired file EZPdf.dll is missing.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_DEVICE_PAPERJAM) {//76
					message="Paper jam\r\n\r\nPlease check the scanner document feeder and ensure there path is clear of any paper jams.";
				}
				else if(errorCode==(int)EZTwainErrorCode.EZTEC_DS_FAILURE) {//5
					message="Duplex failure\r\n\r\nDuplex mode without scanner options window failed. Try enabling the scanner options window or disabling duplex mode.";
				}
				else {
					message=errorCode+" "+((EZTwainErrorCode)errorCode).ToString();
				}
				MessageBox.Show(Lan.G(this,"Unable to scan. Please make sure you can scan using other software. Error: "+message));
				return;
			}
			NodeObjTag nodeObjTag=null;
			bool copied=true;
			Document doc=null;
			try {
				doc=ImageStore.Import(tempFile,GetCurrentCategory(),_patCur);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.G(this,"Unable to copy file, May be in use: ") + ex.Message + ": " + tempFile);
				copied = false;
			}
			if(copied) {
				FillTree(false);
				SelectTreeNode(new NodeObjTag(EnumNodeType.Document,doc.DocNum));
				FormDocInfo FormD=new FormDocInfo(_patCur,doc);
				FormD.ShowDialog(this);//some of the fields might get changed, but not the filename 
				//Customer complained this window was showing up behind OD.  We changed above line to add a parent form as an attempted fix.
				//If this doesn't solve it we can also try adding FormD.BringToFront to see if it does anything.
				if(FormD.DialogResult!=DialogResult.OK) {
					DeleteDocument(false,false,doc);
				}
				else {
					nodeObjTag=new NodeObjTag(EnumNodeType.Document,doc.DocNum);
					_documentShowing=doc.Copy();
				}
			}
			ImageStore.TryDeleteFile(tempFile
				,actInUseException:(msg) => MsgBox.Show(msg)//Informs user when a 'file is in use' exception occurs.
			);
			//Reselect the last successfully added node when necessary. js This code seems to be copied from import multi.  Simplify it.
			if(doc!=null && !new NodeObjTag(EnumNodeType.Document,doc.DocNum).Equals(nodeObjTag)) {
				SelectTreeNode(new NodeObjTag(EnumNodeType.Document,doc.DocNum));
			}
			FillTree(true);
		}

		private void ToolBarSign_Click(){
			if(treeMain.SelectedNode==null ||				//No selection
				treeMain.SelectedNode.Tag==null ||			//Internal error
				treeMain.SelectedNode.Parent==null) {		//This is a folder node.
				return;
			}
			if(((NodeObjTag)treeMain.SelectedNode.Tag).NodeType!=EnumNodeType.Document){
				//shouldn't happen
				return;
			}
			//Show the underlying panel note box while the signature is being filled.
			panelNote.Visible=true;
			LayoutAll();
			//Display the document signature form.
			FormDocSign formDocSign=new FormDocSign(_documentShowing,_patCur);//Updates our local document and saves changes to db also.
			formDocSign.Location=PointToScreen(new Point(treeMain.Left,this.ClientRectangle.Bottom-formDocSign.Height));
			formDocSign.Width=Math.Max(0,Math.Min(formDocSign.Width,panelMain.Right-treeMain.Left));
			formDocSign.ShowDialog();
			FillTree(true);
			//Adjust visibility of panel note based on changes made to the signature above.
			SetPanelNoteVisibility();
			//Resize controls in our window to adjust for a possible change in the visibility of the panel note control.
			LayoutAll();
			//Update the signature and note with the new data.
			FillSignature();
		}

		private void ToolBarSize_Click(){
			if(IsDocumentShowing()){
				//no time for this.  Later.
				//FormDocumentSize...
			}
			if(IsMountItemShowing()){
				if(_arrayDocumentsShowing[_idxSelectedInMount]==null){
					return;
				}
				FormDocumentSizeMount formDocumentSizeMount=new FormDocumentSizeMount();
				formDocumentSizeMount.DocumentCur=_arrayDocumentsShowing[_idxSelectedInMount];
				formDocumentSizeMount.SizeRaw=_arraySizesOriginal[_idxSelectedInMount];
				formDocumentSizeMount.SizeMount=new Size(_listMountItems[_idxSelectedInMount].Width,_listMountItems[_idxSelectedInMount].Height);
				formDocumentSizeMount.ShowDialog();
				//the form will change DocumentCur and save it
				if(formDocumentSizeMount.DialogResult==DialogResult.OK){
					LoadBitmap(_idxSelectedInMount,EnumLoadBitmapType.IdxAndRaw);
				}
				panelMain.Invalidate();
			}
			//ignore mount without selected item because the button shouldn't be enabled
		}

		private void ToolBarZoomOne_Click(){
			if(_idxSelectedInMount==-1){
				return;
			}
			_pointTranslation=new Point(
				_mountShowing.Width/2-_listMountItems[_idxSelectedInMount].Xpos-_listMountItems[_idxSelectedInMount].Width/2,
				_mountShowing.Height/2-_listMountItems[_idxSelectedInMount].Ypos-_listMountItems[_idxSelectedInMount].Height/2);
			float newZoom=ZoomSlider.CalcScaleFit(new Size(panelMain.Width,panelMain.Height),
				new Size(_listMountItems[_idxSelectedInMount].Width,_listMountItems[_idxSelectedInMount].Height),0);
			zoomSlider.SetValueAndMax(newZoom*95);				
			panelMain.Invalidate();
		}

		private void UpdateUserOdPrefForImageCat(long defNum,bool isExpand) {
			if(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)!=2) {//Document tree folders persistent expand/collapse per user.
				return;
			}
			//Calls to Expand() and Collapse() in code cause the TreeDocuments_AfterExpand() and TreeDocuments_AfterCollapse() events to fire.
			//This flag helps us ignore these two events when initializing the tree.
			if(_isFillingTreeWithPref) {
				return;
			}
			Def defImageCatCur=Defs.GetDefsForCategory(DefCat.ImageCats,true).FirstOrDefault(x => x.DefNum==defNum);
			if(defImageCatCur==null) {
				return;//Should never happen, but if it does, there was something wrong with the treeDocument list, and thus nothing should be changed.
			}
			string defaultValue=defImageCatCur.ItemValue;//Stores the default ItemValue of the definition from the catList.
			string curValue=defaultValue;//Stores the current edited ImageCats to compare to the default.
			if(isExpand && !curValue.Contains("E")) {//Since we are expanding we would like to see if the expand flag is present.
				curValue+="E";//If it is not, add expanded flag.
			}
			else if(!isExpand && curValue.Contains("E")) {//Since we are collapsing we want to see if the expand flag is present.
				curValue=curValue.Replace("E","");//If it is, remove expanded flag.
			}
			//Always delete to remove previous value (prevents duplicates).
			UserOdPrefs.DeleteForFkey(Security.CurUser.UserNum,UserOdFkeyType.Definition,defImageCatCur.DefNum);
			if(defaultValue!=curValue) {//Insert an override in the UserOdPref table, only if the chosen value is different than the default.
				UserOdPref userPrefCur=new UserOdPref();//Preference to be inserted to override.
				userPrefCur.UserNum=Security.CurUser.UserNum;
				userPrefCur.Fkey=defImageCatCur.DefNum;
				userPrefCur.FkeyType=UserOdFkeyType.Definition;
				userPrefCur.ValueString=curValue;
				UserOdPrefs.Insert(userPrefCur);
			}
		}
		#endregion Methods - Private

		#region Classes - Nested
		///<summary>(nested class) All info selected from Db is stored in objects attached to treeNode Tags, and not anywhere else.</summary>
		public class NodeObjTag {
			///<summary>Straight from the initial module query at GetTreeListTableForPatient. There are not very many columns, so we just go ahead and expose most of the columns as fields here.</summary>
			public DataRow DataRow=null;
			///<summary>If this is a category folder, this contains the Def.</summary>
			public Def Def=null;
			///<summary>Set with initial refresh from DataRow.DocNum</summary>
			public long DocNum=0;
			///<summary>Index of the folder that this doc or mount belongs in.</summary>
			public int IdxFolder;
			///<summary>Only for nodes of type 'Document'.</summary>
			public ImageType ImgType;
			///<summary>Set with initial refresh from DataRow.MountNum</summary>
			public long MountNum=0;
			public EnumNodeType NodeType;
			///<summary>The text to display in the tree. For non-category, this is set with initial refresh from DataRow.description.</summary>
			public string Text="";

			private NodeObjTag() {
			}
			
			public NodeObjTag(Def def){
				NodeType=EnumNodeType.Category;
				Def=def;
				Text=Def.ItemName;
			}

			public NodeObjTag(DataRow dataRow){
				DataRow=dataRow;
				DocNum=PIn.Long(dataRow["DocNum"].ToString());
				MountNum=PIn.Long(dataRow["MountNum"].ToString());
				//DocCategory might be used for dragging between folders.  Maybe add later.
				//DateCreated not used
				IdxFolder=PIn.Int(dataRow["docFolder"].ToString());//column in datatable is badly named
				Text=PIn.String(dataRow["Description"].ToString());
				if(DocNum!=0){
					NodeType=EnumNodeType.Document;
					ImgType=(ImageType)PIn.Int(dataRow["ImgType"].ToString());
				}
				else{//assume mount
					NodeType=EnumNodeType.Mount;
				}
			}

			///<summary>This is a dummy object just used for comparing Equals based on type and priKey.  Not suitable for use in tree.</summary>
			public NodeObjTag(EnumNodeType nodeType,long priKey){
				NodeType=nodeType;
				switch(NodeType){
					case EnumNodeType.Document:
						DocNum=priKey;
						break;
					case EnumNodeType.Mount:
						MountNum=priKey;
						break;
					case EnumNodeType.Category:
						Def=Defs.GetDef(DefCat.ImageCats,priKey);
						break;
				}
			}

			public override bool Equals(object obj){
				return Equals(obj as NodeObjTag);
			}

			///<summary>Returns true if they are the same type and primary key.</summary>
			public bool Equals(NodeObjTag nodeObjTag2){
				if(nodeObjTag2 is null){
					return false;
				}
				//This override does not solve the problem of wanting 2 nulls to return true.
				//If that becomes necessary, I'll add a new method in the calling class to test 2 NodeObjTags.
				if(NodeType!=nodeObjTag2.NodeType){
					return false;
				}
				switch(NodeType){
					case EnumNodeType.Document:
						if(DocNum==nodeObjTag2.DocNum){
							return true;
						}
						return false;
					case EnumNodeType.Mount:
						if(MountNum==nodeObjTag2.MountNum){
							return true;
						}
						return false;
					case EnumNodeType.Category:
						if(Def.DefNum==nodeObjTag2.Def.DefNum){
							return true;
						}
						return false;
				}
				return false;//should never happen
			}

			public override int GetHashCode(){
				return Tuple.Create(NodeType,DocNum,MountNum).GetHashCode();
			}

			//public static bool operator == (NodeObjTag nodeObjTag1, NodeObjTag nodeObjTag2){
			//No. An overloaded equality operator is discouraged for reference types that lack value semantics, even if Equals is overridden.
			//Especially true if mutable.  It can break some internal C# machinery.
			//https://stackoverflow.com/questions/104158/what-is-best-practice-for-comparing-two-instances-of-a-reference-type

			public override String ToString(){
				return NodeType.ToString()+" "+Text;
			} 

			public NodeObjTag Copy(){
				NodeObjTag nodeObjTag=new NodeObjTag();
				nodeObjTag.DataRow=DataRow;
				nodeObjTag.Def=Def;
				nodeObjTag.DocNum=DocNum;
				nodeObjTag.IdxFolder=IdxFolder;
				nodeObjTag.ImgType=ImgType;
				nodeObjTag.MountNum=MountNum;
				nodeObjTag.NodeType=NodeType;
				nodeObjTag.Text=Text;
				return nodeObjTag;
			}
		}
		#endregion Classes - Nested
	}
}


//2020-04-08-Todo:

//Need to document how XVWeb and Suni use the old Images module.

//This new Images module will never support the following features:
//ClaimPayment/EOB mode.  Old Images module will continue to be used for that.
//EhrAmendment mode.  Can use old images module if it's still required.
//4K and sensor capture are not included here, but instead will be in the new Imaging application
//Cannot scan into a mount position.

//Features that we might add soon:
//Drag mount item, check mouse position to allow longer drags within single position.
//When drag image to occupied mount position, swap
//context menu in main panel (see ContrImages.menuMountItem_Opening)
//Yellow outline pixel width could be better: fixed instead of variable
//ToolBarSize_Click for document
//More icons for toolbars
//panelMain.Invalidate would be a little faster if we intelligently invalidated smaller rectangles.

//New features for documentation:
//Zoom slider in the toolbar
//Mouse scroll to zoom
//Edit Image Info window has a button to remove existing crop, flip, and rotation
//Mount setup and usage
//Mount size window


