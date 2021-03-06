/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;
using OpenDental.Thinfinity;
using Imedisoft.Forms;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental{
///<summary></summary>
	public class FormProcCodes:ODForm {
		private System.ComponentModel.Container components = null;
		///<summary>If IsSelectionMode=true and DialogResult=OK, then this will contain the selected CodeNum.</summary>
		public long SelectedCodeNum;
		//public string SelectedADA;
		///<summary>This is just set once for the whole session.  It doesn't get toggled back to false.  So it only get acted upon when the form closes, sending a signal to other computers telling them to refresh their proc codes.</summary>
		private bool _setInvalidProcCodes;
		///<summary>Once a synch is done, this will switch back to false.  This also triggers SaveLogs, which then clears _dictFeeLogs to prepare for more logs.</summary>
		private bool _needsSynch;
		///<summary>Set to true externally in order to let user select one procedure code.</summary>
		public bool IsSelectionMode;
		///<summary>The list of definitions that is currently showing in the category list.</summary>
		private Definition[] CatList;
		private UI.Button butOK;
		private UI.Button butCancel;
		private UI.Button butEditFeeSched;
		private UI.Button butTools;
		private GroupBox groupFeeScheds;
		private Label label2;
		private ListBox listCategories;
		private Label label3;
		private Label label1;
		private UI.Button butEditCategories;
		private CheckBox checkShowHidden;
		private Label label4;
		private UI.Button butAll;
		private TextBox textCode;
		private TextBox textAbbreviation;
		private TextBox textDescription;
		private UI.Button butShowHiddenDefault;
		private GroupBox groupBox1;
		private ODGrid gridMain;
		private Label label5;
		private UI.Button butNew;
		private UI.Button butExport;
		private UI.Button butImport;
		private UI.Button butProcTools;
		private GroupBox groupProcCodeSetup;
		private GroupBox groupBox2;
		private ComboBox comboProvider1;
		private ComboBoxClinicPicker comboClinic1;
		private ComboBox comboFeeSched1;
		private GroupBox groupBox3;
		private ComboBox comboProvider2;
		private ComboBoxClinicPicker comboClinic2;
		private ComboBox comboFeeSched2;
		private GroupBox groupBox4;
		private ComboBox comboProvider3;
		private ComboBoxClinicPicker comboClinic3;
		private ComboBox comboFeeSched3;
		private List<FeeSchedule> _listFeeScheds; //Note to reviewer: I'm doing these to avoid using calls like FeeSchedC.ListShort[idx] later.
		private List<Clinic> _listClinics;
		private List<Provider> _listProviders;
		private Label labelSched1;
		private Label labelClinic1;
		private Label labelProvider1;
		private Label labelSched2;
		private Label labelClinic2;
		private Label labelProvider2;
		private Label labelSched3;
		private Label labelClinic3;
		private Label labelProvider3;
		private GroupBox groupBox5;
		private Label label22;
		private Label label20;
		private Label label19;
		private Label label21;
		private UI.Button butPickProv1;
		private UI.Button butPickClinic1;
		private UI.Button butPickSched1;
		private UI.Button butPickProv2;
		private UI.Button butPickClinic2;
		private UI.Button butPickSched2;
		private UI.Button butPickProv3;
		private UI.Button butPickClinic3;
		private UI.Button butPickSched3;
		private Color _colorClinic;
		private Color _colorProv;
		private Color _colorProvClinic;
		private Color _colorDefault;
		private System.Windows.Forms.Button butColorClinicProv;
		private System.Windows.Forms.Button butColorProvider;
		private System.Windows.Forms.Button butColorClinic;
		private System.Windows.Forms.Button butColorDefault;
		//<summary>Local copy of a FeeCache class that contains all fees, stored in memory for easy access and editing.  Synced on form closing.</summary>
		//private FeeCache _feeCache;
		///<summary>List of all fees for all three selected fee schedules.  This includes all clinic and provider overrides, regardless of selected clinic and provider.  We could do three separate lists, but that doesn't save us much.  And it's common to use all three columns with the same feeschedule, which would make synching separate lists difficult. Gets synched to db every time selected feescheds change.  This keeps it snappy when entering a series of fees because there is no db write.</summary>
		private List<Fee> _listFees;
		///<summary>The orginal list of fees used for synch.</summary>
		private List<Fee> _listFeesDb;
		private ComboBox comboSort;
		private Label label6;
		private ProcCodeListSort _procCodeSort;
		/// <summary> List should contain two logs per fee because we are inserting two security logs everytime we update a fee.</summary>
		private Dictionary<long,List<SecurityLog>> _dictFeeLogs;
		private bool _canShowHidden;
		///<summary>Contains all of the procedure codes that were selected if IsSelectionMode is true.
		///If IsSelectionMode is true and this list is prefilled with procedure codes then the grid will preselect as many codes as possible.
		///It is not guaranteed that all procedure codes will be selected due to filters.
		///This list should only be read from externally after DialogResult.OK has been returned.</summary>
		public List<ProcedureCode> ListSelectedProcCodes=new List<ProcedureCode>();
		private CheckBox checkGroups1;
		private CheckBox checkGroups2;
		private CheckBox checkGroups3;
		private ComboBox comboFeeSchedGroup1;
		private ComboBox comboFeeSchedGroup2;
		private ComboBox comboFeeSchedGroup3;

		///<summary>Set to true when IsSelectionMode is true and the user should be able to select multiple procedure codes instead of just one.
		///ListSelectedProcCodes will contain all of the procedure codes that the user selected.</summary>
		public bool AllowMultipleSelections;

		///<summary>When canShowHidden is true to the "Hidden" checkbox and "default" button are visible.</summary>
		public FormProcCodes(bool canShowHidden=false) {
			InitializeComponent();// Required for Windows Form Designer support
			
			_canShowHidden=canShowHidden;
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcCodes));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butEditFeeSched = new OpenDental.UI.Button();
			this.butTools = new OpenDental.UI.Button();
			this.groupFeeScheds = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.butEditCategories = new OpenDental.UI.Button();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butAll = new OpenDental.UI.Button();
			this.textCode = new System.Windows.Forms.TextBox();
			this.textAbbreviation = new System.Windows.Forms.TextBox();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.butShowHiddenDefault = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.comboSort = new System.Windows.Forms.ComboBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label5 = new System.Windows.Forms.Label();
			this.butNew = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butProcTools = new OpenDental.UI.Button();
			this.groupProcCodeSetup = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboFeeSchedGroup1 = new System.Windows.Forms.ComboBox();
			this.checkGroups1 = new System.Windows.Forms.CheckBox();
			this.butPickProv1 = new OpenDental.UI.Button();
			this.butPickClinic1 = new OpenDental.UI.Button();
			this.butPickSched1 = new OpenDental.UI.Button();
			this.labelSched1 = new System.Windows.Forms.Label();
			this.labelClinic1 = new System.Windows.Forms.Label();
			this.labelProvider1 = new System.Windows.Forms.Label();
			this.comboProvider1 = new System.Windows.Forms.ComboBox();
			this.comboClinic1 = new OpenDental.UI.ComboBoxClinicPicker();
			this.comboFeeSched1 = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.comboFeeSchedGroup2 = new System.Windows.Forms.ComboBox();
			this.checkGroups2 = new System.Windows.Forms.CheckBox();
			this.butPickProv2 = new OpenDental.UI.Button();
			this.butPickClinic2 = new OpenDental.UI.Button();
			this.butPickSched2 = new OpenDental.UI.Button();
			this.labelSched2 = new System.Windows.Forms.Label();
			this.comboProvider2 = new System.Windows.Forms.ComboBox();
			this.labelClinic2 = new System.Windows.Forms.Label();
			this.comboClinic2 = new OpenDental.UI.ComboBoxClinicPicker();
			this.labelProvider2 = new System.Windows.Forms.Label();
			this.comboFeeSched2 = new System.Windows.Forms.ComboBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.comboFeeSchedGroup3 = new System.Windows.Forms.ComboBox();
			this.checkGroups3 = new System.Windows.Forms.CheckBox();
			this.butPickProv3 = new OpenDental.UI.Button();
			this.butPickClinic3 = new OpenDental.UI.Button();
			this.butPickSched3 = new OpenDental.UI.Button();
			this.labelSched3 = new System.Windows.Forms.Label();
			this.comboProvider3 = new System.Windows.Forms.ComboBox();
			this.labelClinic3 = new System.Windows.Forms.Label();
			this.labelProvider3 = new System.Windows.Forms.Label();
			this.comboClinic3 = new OpenDental.UI.ComboBoxClinicPicker();
			this.comboFeeSched3 = new System.Windows.Forms.ComboBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butColorClinicProv = new System.Windows.Forms.Button();
			this.butColorProvider = new System.Windows.Forms.Button();
			this.butColorClinic = new System.Windows.Forms.Button();
			this.butColorDefault = new System.Windows.Forms.Button();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.groupFeeScheds.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupProcCodeSetup.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(794, 743);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 20;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(889, 743);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 21;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butEditFeeSched
			// 
			this.butEditFeeSched.Location = new System.Drawing.Point(12, 16);
			this.butEditFeeSched.Name = "butEditFeeSched";
			this.butEditFeeSched.Size = new System.Drawing.Size(81, 26);
			this.butEditFeeSched.TabIndex = 18;
			this.butEditFeeSched.Text = "Fee Scheds";
			this.butEditFeeSched.Click += new System.EventHandler(this.butEditFeeSched_Click);
			// 
			// butTools
			// 
			this.butTools.Location = new System.Drawing.Point(109, 16);
			this.butTools.Name = "butTools";
			this.butTools.Size = new System.Drawing.Size(81, 26);
			this.butTools.TabIndex = 19;
			this.butTools.Text = "Fee Tools";
			this.butTools.Click += new System.EventHandler(this.butTools_Click);
			// 
			// groupFeeScheds
			// 
			this.groupFeeScheds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupFeeScheds.Controls.Add(this.butTools);
			this.groupFeeScheds.Controls.Add(this.butEditFeeSched);
			this.groupFeeScheds.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupFeeScheds.Location = new System.Drawing.Point(778, 663);
			this.groupFeeScheds.Name = "groupFeeScheds";
			this.groupFeeScheds.Size = new System.Drawing.Size(200, 51);
			this.groupFeeScheds.TabIndex = 14;
			this.groupFeeScheds.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 20);
			this.label2.TabIndex = 17;
			this.label2.Text = "By Descript";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listCategories
			// 
			this.listCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listCategories.FormattingEnabled = true;
			this.listCategories.Location = new System.Drawing.Point(10, 149);
			this.listCategories.Name = "listCategories";
			this.listCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listCategories.Size = new System.Drawing.Size(145, 420);
			this.listCategories.TabIndex = 15;
			this.listCategories.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listCategories_MouseUp);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 20);
			this.label3.TabIndex = 19;
			this.label3.Text = "By Code";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 123);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 16;
			this.label1.Text = "By Category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butEditCategories
			// 
			this.butEditCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditCategories.Location = new System.Drawing.Point(10, 585);
			this.butEditCategories.Name = "butEditCategories";
			this.butEditCategories.Size = new System.Drawing.Size(94, 26);
			this.butEditCategories.TabIndex = 23;
			this.butEditCategories.Text = "Edit Categories";
			this.butEditCategories.Click += new System.EventHandler(this.butEditCategories_Click);
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.Location = new System.Drawing.Point(10, 617);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(90, 17);
			this.checkShowHidden.TabIndex = 24;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.Click += new System.EventHandler(this.checkShowHidden_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 20);
			this.label4.TabIndex = 22;
			this.label4.Text = "By Abbrev";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAll
			// 
			this.butAll.Location = new System.Drawing.Point(93, 123);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(62, 25);
			this.butAll.TabIndex = 22;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// textCode
			// 
			this.textCode.Location = new System.Drawing.Point(82, 69);
			this.textCode.Name = "textCode";
			this.textCode.Size = new System.Drawing.Size(73, 20);
			this.textCode.TabIndex = 2;
			this.textCode.TextChanged += new System.EventHandler(this.textCode_TextChanged);
			// 
			// textAbbreviation
			// 
			this.textAbbreviation.Location = new System.Drawing.Point(82, 17);
			this.textAbbreviation.Name = "textAbbreviation";
			this.textAbbreviation.Size = new System.Drawing.Size(73, 20);
			this.textAbbreviation.TabIndex = 0;
			this.textAbbreviation.TextChanged += new System.EventHandler(this.textAbbreviation_TextChanged);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(82, 43);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(73, 20);
			this.textDescription.TabIndex = 1;
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// butShowHiddenDefault
			// 
			this.butShowHiddenDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butShowHiddenDefault.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butShowHiddenDefault.Location = new System.Drawing.Point(100, 614);
			this.butShowHiddenDefault.Name = "butShowHiddenDefault";
			this.butShowHiddenDefault.Size = new System.Drawing.Size(56, 20);
			this.butShowHiddenDefault.TabIndex = 25;
			this.butShowHiddenDefault.Text = "default";
			this.butShowHiddenDefault.Click += new System.EventHandler(this.butShowHiddenDefault_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.comboSort);
			this.groupBox1.Controls.Add(this.butShowHiddenDefault);
			this.groupBox1.Controls.Add(this.textDescription);
			this.groupBox1.Controls.Add(this.textAbbreviation);
			this.groupBox1.Controls.Add(this.textCode);
			this.groupBox1.Controls.Add(this.butAll);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.checkShowHidden);
			this.groupBox1.Controls.Add(this.butEditCategories);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.listCategories);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(2, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(165, 646);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 94);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(79, 20);
			this.label6.TabIndex = 34;
			this.label6.Text = "Sort Order";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSort
			// 
			this.comboSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSort.FormattingEnabled = true;
			this.comboSort.Location = new System.Drawing.Point(82, 95);
			this.comboSort.Name = "comboSort";
			this.comboSort.Size = new System.Drawing.Size(73, 21);
			this.comboSort.TabIndex = 33;
			this.comboSort.SelectionChangeCommitted += new System.EventHandler(this.comboSort_SelectionChangeCommitted);
			// 
			// gridMain
			// 
			this.gridMain.AddButtonEnabled = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.EditableEnterMovesDown = true;
			this.gridMain.Location = new System.Drawing.Point(170, 8);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(604, 761);
			this.gridMain.TabIndex = 19;
			this.gridMain.Title = "Procedures";
			this.gridMain.TranslationName = "TableProcedures";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellLeave);
			this.gridMain.CellEnter += new OpenDental.UI.ODGridClickEventHandler(this.GridMain_CellEnter);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(779, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(199, 17);
			this.label5.TabIndex = 21;
			this.label5.Text = "Compare Fee Schedules";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butNew
			// 
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNew.Image = global::Imedisoft.Properties.Resources.Add;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(85, 57);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(75, 26);
			this.butNew.TabIndex = 29;
			this.butNew.Text = "&New";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// butExport
			// 
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(85, 19);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(75, 26);
			this.butExport.TabIndex = 27;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butImport
			// 
			this.butImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butImport.Location = new System.Drawing.Point(6, 19);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(75, 26);
			this.butImport.TabIndex = 26;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butProcTools
			// 
			this.butProcTools.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProcTools.Location = new System.Drawing.Point(6, 57);
			this.butProcTools.Name = "butProcTools";
			this.butProcTools.Size = new System.Drawing.Size(75, 26);
			this.butProcTools.TabIndex = 28;
			this.butProcTools.Text = "Tools";
			this.butProcTools.Click += new System.EventHandler(this.butProcTools_Click);
			// 
			// groupProcCodeSetup
			// 
			this.groupProcCodeSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupProcCodeSetup.Controls.Add(this.butProcTools);
			this.groupProcCodeSetup.Controls.Add(this.butImport);
			this.groupProcCodeSetup.Controls.Add(this.butExport);
			this.groupProcCodeSetup.Controls.Add(this.butNew);
			this.groupProcCodeSetup.Location = new System.Drawing.Point(2, 678);
			this.groupProcCodeSetup.Name = "groupProcCodeSetup";
			this.groupProcCodeSetup.Size = new System.Drawing.Size(165, 91);
			this.groupProcCodeSetup.TabIndex = 26;
			this.groupProcCodeSetup.TabStop = false;
			this.groupProcCodeSetup.Text = "Procedure Codes";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.comboFeeSchedGroup1);
			this.groupBox2.Controls.Add(this.checkGroups1);
			this.groupBox2.Controls.Add(this.butPickProv1);
			this.groupBox2.Controls.Add(this.butPickClinic1);
			this.groupBox2.Controls.Add(this.butPickSched1);
			this.groupBox2.Controls.Add(this.labelSched1);
			this.groupBox2.Controls.Add(this.labelClinic1);
			this.groupBox2.Controls.Add(this.labelProvider1);
			this.groupBox2.Controls.Add(this.comboProvider1);
			this.groupBox2.Controls.Add(this.comboClinic1);
			this.groupBox2.Controls.Add(this.comboFeeSched1);
			this.groupBox2.Location = new System.Drawing.Point(780, 55);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 175);
			this.groupBox2.TabIndex = 27;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Fee 1";
			// 
			// comboFeeSchedGroup1
			// 
			this.comboFeeSchedGroup1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSchedGroup1.FormattingEnabled = true;
			this.comboFeeSchedGroup1.Location = new System.Drawing.Point(157, 84);
			this.comboFeeSchedGroup1.Name = "comboFeeSchedGroup1";
			this.comboFeeSchedGroup1.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSchedGroup1.TabIndex = 41;
			this.comboFeeSchedGroup1.Visible = false;
			this.comboFeeSchedGroup1.SelectionChangeCommitted += new System.EventHandler(this.comboFeeSchedGroup_SelectionChangeCommitted);
			// 
			// checkGroups1
			// 
			this.checkGroups1.Location = new System.Drawing.Point(14, 16);
			this.checkGroups1.Name = "checkGroups1";
			this.checkGroups1.Size = new System.Drawing.Size(176, 17);
			this.checkGroups1.TabIndex = 40;
			this.checkGroups1.Text = "Show Fee Schedule Groups";
			this.checkGroups1.UseVisualStyleBackColor = true;
			this.checkGroups1.CheckedChanged += new System.EventHandler(this.checkGroups1_CheckedChanged);
			// 
			// butPickProv1
			// 
			this.butPickProv1.Location = new System.Drawing.Point(167, 136);
			this.butPickProv1.Name = "butPickProv1";
			this.butPickProv1.Size = new System.Drawing.Size(23, 21);
			this.butPickProv1.TabIndex = 5;
			this.butPickProv1.Text = "...";
			this.butPickProv1.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickClinic1
			// 
			this.butPickClinic1.Location = new System.Drawing.Point(167, 96);
			this.butPickClinic1.Name = "butPickClinic1";
			this.butPickClinic1.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic1.TabIndex = 3;
			this.butPickClinic1.Text = "...";
			this.butPickClinic1.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butPickSched1
			// 
			this.butPickSched1.Location = new System.Drawing.Point(167, 54);
			this.butPickSched1.Name = "butPickSched1";
			this.butPickSched1.Size = new System.Drawing.Size(23, 21);
			this.butPickSched1.TabIndex = 1;
			this.butPickSched1.Text = "...";
			this.butPickSched1.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// labelSched1
			// 
			this.labelSched1.Location = new System.Drawing.Point(14, 36);
			this.labelSched1.Name = "labelSched1";
			this.labelSched1.Size = new System.Drawing.Size(174, 17);
			this.labelSched1.TabIndex = 32;
			this.labelSched1.Text = "Fee Schedule";
			this.labelSched1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelClinic1
			// 
			this.labelClinic1.Location = new System.Drawing.Point(14, 78);
			this.labelClinic1.Name = "labelClinic1";
			this.labelClinic1.Size = new System.Drawing.Size(174, 17);
			this.labelClinic1.TabIndex = 30;
			this.labelClinic1.Text = "Clinic";
			this.labelClinic1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelProvider1
			// 
			this.labelProvider1.Location = new System.Drawing.Point(14, 118);
			this.labelProvider1.Name = "labelProvider1";
			this.labelProvider1.Size = new System.Drawing.Size(174, 17);
			this.labelProvider1.TabIndex = 28;
			this.labelProvider1.Text = "Provider";
			this.labelProvider1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboProvider1
			// 
			this.comboProvider1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider1.FormattingEnabled = true;
			this.comboProvider1.Location = new System.Drawing.Point(14, 136);
			this.comboProvider1.Name = "comboProvider1";
			this.comboProvider1.Size = new System.Drawing.Size(151, 21);
			this.comboProvider1.TabIndex = 4;
			this.comboProvider1.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProv_SelectionChangeCommitted);
			// 
			// comboClinic1
			// 
			this.comboClinic1.ForceShowUnassigned = true;
			this.comboClinic1.IncludeUnassigned = true;
			this.comboClinic1.Location = new System.Drawing.Point(14, 96);
			this.comboClinic1.Name = "comboClinic1";
			this.comboClinic1.ShowLabel = false;
			this.comboClinic1.Size = new System.Drawing.Size(151, 21);
			this.comboClinic1.TabIndex = 2;
			this.comboClinic1.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProv_SelectionChangeCommitted);
			// 
			// comboFeeSched1
			// 
			this.comboFeeSched1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched1.FormattingEnabled = true;
			this.comboFeeSched1.Location = new System.Drawing.Point(14, 54);
			this.comboFeeSched1.Name = "comboFeeSched1";
			this.comboFeeSched1.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSched1.TabIndex = 0;
			this.comboFeeSched1.SelectionChangeCommitted += new System.EventHandler(this.comboFeeSched_SelectionChangeCommitted);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.comboFeeSchedGroup2);
			this.groupBox3.Controls.Add(this.checkGroups2);
			this.groupBox3.Controls.Add(this.butPickProv2);
			this.groupBox3.Controls.Add(this.butPickClinic2);
			this.groupBox3.Controls.Add(this.butPickSched2);
			this.groupBox3.Controls.Add(this.labelSched2);
			this.groupBox3.Controls.Add(this.comboProvider2);
			this.groupBox3.Controls.Add(this.labelClinic2);
			this.groupBox3.Controls.Add(this.comboClinic2);
			this.groupBox3.Controls.Add(this.labelProvider2);
			this.groupBox3.Controls.Add(this.comboFeeSched2);
			this.groupBox3.Location = new System.Drawing.Point(780, 233);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(200, 175);
			this.groupBox3.TabIndex = 28;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Fee 2";
			// 
			// comboFeeSchedGroup2
			// 
			this.comboFeeSchedGroup2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSchedGroup2.FormattingEnabled = true;
			this.comboFeeSchedGroup2.Location = new System.Drawing.Point(157, 81);
			this.comboFeeSchedGroup2.Name = "comboFeeSchedGroup2";
			this.comboFeeSchedGroup2.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSchedGroup2.TabIndex = 42;
			this.comboFeeSchedGroup2.Visible = false;
			this.comboFeeSchedGroup2.SelectionChangeCommitted += new System.EventHandler(this.comboFeeSchedGroup_SelectionChangeCommitted);
			// 
			// checkGroups2
			// 
			this.checkGroups2.Location = new System.Drawing.Point(14, 16);
			this.checkGroups2.Name = "checkGroups2";
			this.checkGroups2.Size = new System.Drawing.Size(173, 17);
			this.checkGroups2.TabIndex = 39;
			this.checkGroups2.Text = "Show Fee Schedule Groups";
			this.checkGroups2.UseVisualStyleBackColor = true;
			this.checkGroups2.CheckedChanged += new System.EventHandler(this.checkGroups2_CheckedChanged);
			// 
			// butPickProv2
			// 
			this.butPickProv2.Location = new System.Drawing.Point(167, 136);
			this.butPickProv2.Name = "butPickProv2";
			this.butPickProv2.Size = new System.Drawing.Size(23, 21);
			this.butPickProv2.TabIndex = 11;
			this.butPickProv2.Text = "...";
			this.butPickProv2.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickClinic2
			// 
			this.butPickClinic2.Location = new System.Drawing.Point(167, 96);
			this.butPickClinic2.Name = "butPickClinic2";
			this.butPickClinic2.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic2.TabIndex = 9;
			this.butPickClinic2.Text = "...";
			this.butPickClinic2.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butPickSched2
			// 
			this.butPickSched2.Location = new System.Drawing.Point(167, 54);
			this.butPickSched2.Name = "butPickSched2";
			this.butPickSched2.Size = new System.Drawing.Size(23, 21);
			this.butPickSched2.TabIndex = 7;
			this.butPickSched2.Text = "...";
			this.butPickSched2.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// labelSched2
			// 
			this.labelSched2.Location = new System.Drawing.Point(14, 36);
			this.labelSched2.Name = "labelSched2";
			this.labelSched2.Size = new System.Drawing.Size(174, 17);
			this.labelSched2.TabIndex = 35;
			this.labelSched2.Text = "Fee Schedule";
			this.labelSched2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboProvider2
			// 
			this.comboProvider2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider2.FormattingEnabled = true;
			this.comboProvider2.Location = new System.Drawing.Point(14, 136);
			this.comboProvider2.Name = "comboProvider2";
			this.comboProvider2.Size = new System.Drawing.Size(151, 21);
			this.comboProvider2.TabIndex = 10;
			this.comboProvider2.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProv_SelectionChangeCommitted);
			// 
			// labelClinic2
			// 
			this.labelClinic2.Location = new System.Drawing.Point(14, 78);
			this.labelClinic2.Name = "labelClinic2";
			this.labelClinic2.Size = new System.Drawing.Size(174, 17);
			this.labelClinic2.TabIndex = 34;
			this.labelClinic2.Text = "Clinic";
			this.labelClinic2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboClinic2
			// 
			this.comboClinic2.ForceShowUnassigned = true;
			this.comboClinic2.IncludeUnassigned = true;
			this.comboClinic2.Location = new System.Drawing.Point(14, 96);
			this.comboClinic2.Name = "comboClinic2";
			this.comboClinic2.ShowLabel = false;
			this.comboClinic2.Size = new System.Drawing.Size(151, 21);
			this.comboClinic2.TabIndex = 8;
			this.comboClinic2.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProv_SelectionChangeCommitted);
			// 
			// labelProvider2
			// 
			this.labelProvider2.Location = new System.Drawing.Point(14, 118);
			this.labelProvider2.Name = "labelProvider2";
			this.labelProvider2.Size = new System.Drawing.Size(174, 17);
			this.labelProvider2.TabIndex = 33;
			this.labelProvider2.Text = "Provider";
			this.labelProvider2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboFeeSched2
			// 
			this.comboFeeSched2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched2.FormattingEnabled = true;
			this.comboFeeSched2.Location = new System.Drawing.Point(14, 54);
			this.comboFeeSched2.Name = "comboFeeSched2";
			this.comboFeeSched2.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSched2.TabIndex = 6;
			this.comboFeeSched2.SelectionChangeCommitted += new System.EventHandler(this.comboFeeSched_SelectionChangeCommitted);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.comboFeeSchedGroup3);
			this.groupBox4.Controls.Add(this.checkGroups3);
			this.groupBox4.Controls.Add(this.butPickProv3);
			this.groupBox4.Controls.Add(this.butPickClinic3);
			this.groupBox4.Controls.Add(this.butPickSched3);
			this.groupBox4.Controls.Add(this.labelSched3);
			this.groupBox4.Controls.Add(this.comboProvider3);
			this.groupBox4.Controls.Add(this.labelClinic3);
			this.groupBox4.Controls.Add(this.labelProvider3);
			this.groupBox4.Controls.Add(this.comboClinic3);
			this.groupBox4.Controls.Add(this.comboFeeSched3);
			this.groupBox4.Location = new System.Drawing.Point(780, 411);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(200, 175);
			this.groupBox4.TabIndex = 29;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Fee 3";
			// 
			// comboFeeSchedGroup3
			// 
			this.comboFeeSchedGroup3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSchedGroup3.FormattingEnabled = true;
			this.comboFeeSchedGroup3.Location = new System.Drawing.Point(157, 78);
			this.comboFeeSchedGroup3.Name = "comboFeeSchedGroup3";
			this.comboFeeSchedGroup3.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSchedGroup3.TabIndex = 42;
			this.comboFeeSchedGroup3.Visible = false;
			this.comboFeeSchedGroup3.SelectionChangeCommitted += new System.EventHandler(this.comboFeeSchedGroup_SelectionChangeCommitted);
			// 
			// checkGroups3
			// 
			this.checkGroups3.Location = new System.Drawing.Point(14, 16);
			this.checkGroups3.Name = "checkGroups3";
			this.checkGroups3.Size = new System.Drawing.Size(176, 17);
			this.checkGroups3.TabIndex = 40;
			this.checkGroups3.Text = "Show Fee Schedule Groups";
			this.checkGroups3.UseVisualStyleBackColor = true;
			this.checkGroups3.CheckedChanged += new System.EventHandler(this.checkGroups3_CheckedChanged);
			// 
			// butPickProv3
			// 
			this.butPickProv3.Location = new System.Drawing.Point(167, 136);
			this.butPickProv3.Name = "butPickProv3";
			this.butPickProv3.Size = new System.Drawing.Size(23, 21);
			this.butPickProv3.TabIndex = 17;
			this.butPickProv3.Text = "...";
			this.butPickProv3.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickClinic3
			// 
			this.butPickClinic3.Location = new System.Drawing.Point(167, 96);
			this.butPickClinic3.Name = "butPickClinic3";
			this.butPickClinic3.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic3.TabIndex = 15;
			this.butPickClinic3.Text = "...";
			this.butPickClinic3.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butPickSched3
			// 
			this.butPickSched3.Location = new System.Drawing.Point(167, 54);
			this.butPickSched3.Name = "butPickSched3";
			this.butPickSched3.Size = new System.Drawing.Size(23, 21);
			this.butPickSched3.TabIndex = 13;
			this.butPickSched3.Text = "...";
			this.butPickSched3.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// labelSched3
			// 
			this.labelSched3.Location = new System.Drawing.Point(14, 36);
			this.labelSched3.Name = "labelSched3";
			this.labelSched3.Size = new System.Drawing.Size(174, 17);
			this.labelSched3.TabIndex = 38;
			this.labelSched3.Text = "Fee Schedule";
			this.labelSched3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboProvider3
			// 
			this.comboProvider3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider3.FormattingEnabled = true;
			this.comboProvider3.Location = new System.Drawing.Point(14, 136);
			this.comboProvider3.Name = "comboProvider3";
			this.comboProvider3.Size = new System.Drawing.Size(151, 21);
			this.comboProvider3.TabIndex = 16;
			this.comboProvider3.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProv_SelectionChangeCommitted);
			// 
			// labelClinic3
			// 
			this.labelClinic3.Location = new System.Drawing.Point(14, 78);
			this.labelClinic3.Name = "labelClinic3";
			this.labelClinic3.Size = new System.Drawing.Size(174, 17);
			this.labelClinic3.TabIndex = 37;
			this.labelClinic3.Text = "Clinic";
			this.labelClinic3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelProvider3
			// 
			this.labelProvider3.Location = new System.Drawing.Point(14, 118);
			this.labelProvider3.Name = "labelProvider3";
			this.labelProvider3.Size = new System.Drawing.Size(174, 17);
			this.labelProvider3.TabIndex = 36;
			this.labelProvider3.Text = "Provider";
			this.labelProvider3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboClinic3
			// 
			this.comboClinic3.ForceShowUnassigned = true;
			this.comboClinic3.IncludeUnassigned = true;
			this.comboClinic3.Location = new System.Drawing.Point(14, 96);
			this.comboClinic3.Name = "comboClinic3";
			this.comboClinic3.ShowLabel = false;
			this.comboClinic3.Size = new System.Drawing.Size(151, 21);
			this.comboClinic3.TabIndex = 14;
			this.comboClinic3.SelectionChangeCommitted += new System.EventHandler(this.comboClinicProv_SelectionChangeCommitted);
			// 
			// comboFeeSched3
			// 
			this.comboFeeSched3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched3.FormattingEnabled = true;
			this.comboFeeSched3.Location = new System.Drawing.Point(14, 54);
			this.comboFeeSched3.Name = "comboFeeSched3";
			this.comboFeeSched3.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSched3.TabIndex = 12;
			this.comboFeeSched3.SelectionChangeCommitted += new System.EventHandler(this.comboFeeSched_SelectionChangeCommitted);
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.butColorClinicProv);
			this.groupBox5.Controls.Add(this.butColorProvider);
			this.groupBox5.Controls.Add(this.butColorClinic);
			this.groupBox5.Controls.Add(this.butColorDefault);
			this.groupBox5.Controls.Add(this.label21);
			this.groupBox5.Controls.Add(this.label22);
			this.groupBox5.Controls.Add(this.label20);
			this.groupBox5.Controls.Add(this.label19);
			this.groupBox5.Location = new System.Drawing.Point(780, 589);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(200, 70);
			this.groupBox5.TabIndex = 30;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Fee Colors";
			// 
			// butColorClinicProv
			// 
			this.butColorClinicProv.Enabled = false;
			this.butColorClinicProv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorClinicProv.Location = new System.Drawing.Point(89, 44);
			this.butColorClinicProv.Name = "butColorClinicProv";
			this.butColorClinicProv.Size = new System.Drawing.Size(20, 20);
			this.butColorClinicProv.TabIndex = 163;
			// 
			// butColorProvider
			// 
			this.butColorProvider.Enabled = false;
			this.butColorProvider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorProvider.Location = new System.Drawing.Point(10, 44);
			this.butColorProvider.Name = "butColorProvider";
			this.butColorProvider.Size = new System.Drawing.Size(20, 20);
			this.butColorProvider.TabIndex = 162;
			// 
			// butColorClinic
			// 
			this.butColorClinic.Enabled = false;
			this.butColorClinic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorClinic.Location = new System.Drawing.Point(89, 18);
			this.butColorClinic.Name = "butColorClinic";
			this.butColorClinic.Size = new System.Drawing.Size(20, 20);
			this.butColorClinic.TabIndex = 161;
			// 
			// butColorDefault
			// 
			this.butColorDefault.Enabled = false;
			this.butColorDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorDefault.Location = new System.Drawing.Point(10, 17);
			this.butColorDefault.Name = "butColorDefault";
			this.butColorDefault.Size = new System.Drawing.Size(20, 20);
			this.butColorDefault.TabIndex = 160;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(110, 19);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(48, 17);
			this.label21.TabIndex = 48;
			this.label21.Text = "= Clinic";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(31, 47);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(55, 17);
			this.label22.TabIndex = 46;
			this.label22.Text = "= Provider";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(110, 47);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(88, 17);
			this.label20.TabIndex = 44;
			this.label20.Text = "= Provider+Clinic";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(31, 19);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(60, 17);
			this.label19.TabIndex = 43;
			this.label19.Text = "= Default";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormProcCodes
			// 
			this.ClientSize = new System.Drawing.Size(982, 782);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupProcCodeSetup);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupFeeScheds);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormProcCodes";
			this.ShowInTaskbar = false;
			this.Text = "Procedure Codes - Fee Schedules";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProcedures_Closing);
			this.Load += new System.EventHandler(this.FormProcCodes_Load);
			this.groupFeeScheds.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupProcCodeSetup.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		private void FormProcCodes_Load(object sender,System.EventArgs e) {
			_dictFeeLogs=new Dictionary<long,List<SecurityLog>>();
			if(!Security.IsAuthorized(Permissions.Setup,DateTime.MinValue,true)) {
				groupFeeScheds.Visible=false;
				butEditCategories.Visible=false;
			}
			if(!Security.IsAuthorized(Permissions.ProcCodeEdit,true)) {
				groupProcCodeSetup.Visible=false;
			}
			if(!IsSelectionMode) {
				butOK.Visible=false;
				butCancel.Text="Close";
			}
			else if(AllowMultipleSelections) {
				//Allow the user to select multiple rows by changing the grid selection mode.
				gridMain.SelectionMode=GridSelectionMode.MultiExtended;
			}
			if(_canShowHidden) {
				checkShowHidden.Checked=Preferences.GetBool(PreferenceName.ProcCodeListShowHidden);
			}
			else {//checkShowHidden will always be unchecked.
				checkShowHidden.Visible=false;
				butShowHiddenDefault.Visible=false;
			}
			_listFeeScheds=FeeScheds.GetDeepCopy(true);
			FillCats();
			for(int i=0;i<listCategories.Items.Count;i++) {
				listCategories.SetSelected(i,true);
			}
			_listClinics=Clinics.GetByUser(Security.CurrentUser);
			_listProviders=Providers.GetAll(true);
			_colorProv=Definitions.GetColor(DefinitionCategory.FeeColors,Definitions.GetByExactName(DefinitionCategory.FeeColors,"Provider"));
			_colorProvClinic=Definitions.GetColor(DefinitionCategory.FeeColors,Definitions.GetByExactName(DefinitionCategory.FeeColors,"Provider and Clinic"));
			_colorClinic=Definitions.GetColor(DefinitionCategory.FeeColors,Definitions.GetByExactName(DefinitionCategory.FeeColors,"Clinic"));
			_colorDefault=Definitions.GetColor(DefinitionCategory.FeeColors,Definitions.GetByExactName(DefinitionCategory.FeeColors,"Default"));
			butColorProvider.BackColor=_colorProv;
			butColorClinicProv.BackColor=_colorProvClinic;
			butColorClinic.BackColor=_colorClinic;
			butColorDefault.BackColor=_colorDefault;
			labelSched1.ForeColor=_colorDefault;
			labelSched2.ForeColor=_colorDefault;
			labelSched3.ForeColor=_colorDefault;
			labelClinic1.ForeColor=_colorClinic;
			labelClinic2.ForeColor=_colorClinic;
			labelClinic3.ForeColor=_colorClinic;
			labelProvider1.ForeColor=_colorProv;
			labelProvider2.ForeColor=_colorProv;
			labelProvider3.ForeColor=_colorProv;
			bool _isShowingGroups=Preferences.GetBool(PreferenceName.ShowFeeSchedGroups);
			checkGroups1.Visible=_isShowingGroups;
			checkGroups2.Visible=_isShowingGroups;
			checkGroups3.Visible=_isShowingGroups;
			comboFeeSchedGroup1.Location=new Point(14,96);
			comboFeeSchedGroup2.Location=new Point(14,96);
			comboFeeSchedGroup3.Location=new Point(14,96);
			FillComboBoxes();
			SynchAndFillListFees(false);
			for(int i=0;i<Enum.GetNames(typeof(ProcCodeListSort)).Length;i++) {
				comboSort.Items.Add(Enum.GetNames(typeof(ProcCodeListSort))[i]);
			}
			_procCodeSort=(ProcCodeListSort)PrefC.GetInt(PreferenceName.ProcCodeListSortOrder);
			comboSort.SelectedIndex=(int)_procCodeSort;
			FillGrid();
			//Preselect corresponding procedure codes once on load.  Do not do it within FillGrid().
			if(ListSelectedProcCodes.Count > 0) {
				for(int i=0;i<gridMain.Rows.Count;i++) {
					if(ListSelectedProcCodes.Any(x => x.Id==((ProcedureCode)gridMain.Rows[i].Tag).Id)) {
						gridMain.SetSelected(i,true);
					}
				}
			}
		}

		///<summary>Called on Load and anytime the feeschedule list might have changed.  Also called when different fee schedule selected to handle global fee schedules and the enabling/disabling of prov and clinic selectors.  This does not make any calls to db, so should be fast.</summary>
		private void FillComboBoxes() {
			//js This was getting called slightly too often.  It was getting called on all 6 combo_SelectionChangeCommitted and all 3 butPickFeeSched_Click.  Curiously, it was not getting called on the 3 butPickClinic_Click events or butPickProvider_Click events, suggesting that it didn't need to be called from the other places, either.  I removed the places where it didn't seem needed.
			//Save combo box selected indexes prior to changing stuff.
			long feeSchedNum1Selected=0;//Default to the first 
			long feeSchedNum2Selected=0;//Default to none
			long feeSchedNum3Selected=0;//Default to none
			if(_listFeeScheds.Count > 0) {
				if(comboFeeSched1.SelectedIndex > -1) {
					feeSchedNum1Selected=comboFeeSched1.GetSelected<FeeSchedule>().Id;
				}
				if(comboFeeSched2.SelectedIndex > 0) {
					feeSchedNum2Selected=comboFeeSched2.GetSelected<FeeSchedule>().Id;
				}
				if(comboFeeSched3.SelectedIndex > 0) {
					feeSchedNum3Selected=comboFeeSched3.GetSelected<FeeSchedule>().Id;
				}
			}
			//Always update _listFeeScheds to reflect any potential changes.
			_listFeeScheds=FeeScheds.GetDeepCopy(true);
			//Check if feschednums from above were set to hidden, if so set selected index to 0 for the combo
			if(feeSchedNum1Selected > 0 && !_listFeeScheds.Any(x => x.Id==feeSchedNum1Selected)) {
				comboFeeSched1.SelectedIndex=0;
			}
			if(feeSchedNum2Selected > 0 && !_listFeeScheds.Any(x => x.Id==feeSchedNum2Selected)) {
				comboFeeSched2.SelectedIndex=0;
			}
			if(feeSchedNum3Selected > 0 && !_listFeeScheds.Any(x => x.Id==feeSchedNum3Selected)) {
				comboFeeSched3.SelectedIndex=0;
			}
			int comboProv1Idx=comboProvider1.SelectedIndex;
			int comboProv2Idx=comboProvider2.SelectedIndex;
			int comboProv3Idx=comboProvider3.SelectedIndex;
			long feeSchedGroup1Num=comboFeeSchedGroup1.GetSelected<FeeScheduleGroup>()?.Id??0;
			long feeSchedGroup2Num=comboFeeSchedGroup2.GetSelected<FeeScheduleGroup>()?.Id??0;
			long feeSchedGroup3Num=comboFeeSchedGroup3.GetSelected<FeeScheduleGroup>()?.Id??0;
			comboFeeSched1.Items.Clear();
			comboFeeSched2.Items.Clear();
			comboFeeSched3.Items.Clear();
			comboProvider1.Items.Clear();
			comboProvider2.Items.Clear();
			comboProvider3.Items.Clear();
			comboFeeSchedGroup1.Items.Clear();
			comboFeeSchedGroup2.Items.Clear();
			comboFeeSchedGroup3.Items.Clear();
			//Fill fee sched combo boxes (FeeSched 1 doesn't get the "None" option)
			ODBoxItem<FeeSchedule> boxItemNone=new ODBoxItem<FeeSchedule>("None",new FeeSchedule());
			comboFeeSched2.Items.Add(boxItemNone);
			if(feeSchedNum2Selected==0) {
				comboFeeSched2.SelectedItem=boxItemNone;
			}
			comboFeeSched3.Items.Add(boxItemNone);
			if(feeSchedNum3Selected==0) {
				comboFeeSched3.SelectedItem=boxItemNone;
			}
			string str;
			for(int i=0;i<_listFeeScheds.Count;i++) {
				str=_listFeeScheds[i].Description;
				if(_listFeeScheds[i].Type!=FeeScheduleType.Normal) {
					str+=" ("+_listFeeScheds[i].Type.ToString()+")";
				}
				ODBoxItem<FeeSchedule> boxItem=new ODBoxItem<FeeSchedule>(str,_listFeeScheds[i]);
				comboFeeSched1.Items.Add(boxItem);
				if(feeSchedNum1Selected==_listFeeScheds[i].Id) {
					comboFeeSched1.SelectedItem=boxItem;
				}
				comboFeeSched2.Items.Add(boxItem);
				if(feeSchedNum2Selected==_listFeeScheds[i].Id) {
					comboFeeSched2.SelectedItem=boxItem;
				}
				comboFeeSched3.Items.Add(boxItem);
				if(feeSchedNum3Selected==_listFeeScheds[i].Id) {
					comboFeeSched3.SelectedItem=boxItem;
				}
			}
			comboFeeSched1.SelectedIndex=comboFeeSched1.SelectedIndex>-1 ? comboFeeSched1.SelectedIndex : 0;
			if(_listFeeScheds.Count==0) {//No fee schedules in the database so set the first item to none.
				comboFeeSched1.Items.Add(new ODBoxItem<FeeSchedule>("None",new FeeSchedule()));
			}
			//Fill clinic combo boxes
			//Add none even if clinics is turned off so that 0 is a valid index to select.
			string defaultClinicText=(PrefC.HasClinicsEnabled ? "Default" : "None");
			//The clinic pickers have different unassigned text when disabled
			comboClinic1.HqDescription=defaultClinicText;
			comboClinic2.HqDescription=defaultClinicText;
			comboClinic3.HqDescription=defaultClinicText;
			if(!PrefC.HasClinicsEnabled) {//No clinics
				//For UI reasons, leave the clinic combo boxes visible for users not using clinics and they will just say "none".
				comboClinic1.Enabled=false;
				comboClinic2.Enabled=false;
				comboClinic3.Enabled=false;
				//The clinic pickers need to remain visible even with clinics disabled
				comboClinic1.Visible=true;
				comboClinic2.Visible=true;
				comboClinic3.Visible=true;
				//The unassigned option needs to be forced shown, otherwise 0 will be present 
				comboClinic1.ForceShowUnassigned=true;
				comboClinic2.ForceShowUnassigned=true;
				comboClinic3.ForceShowUnassigned=true;
				butPickClinic1.Enabled=false;
				butPickClinic2.Enabled=false;
				butPickClinic3.Enabled=false;
			}
			else {
				List<long> listCombo1GroupNums=new List<long>();
				List<long> listCombo2GroupNums=new List<long>();
				List<long> listCombo3GroupNums=new List<long>();
				for(int i=0;i<_listClinics.Count;i++) {
					ODBoxItem<Clinic> boxItemClinic=new ODBoxItem<Clinic>(_listClinics[i].Abbr,_listClinics[i]);
					//When FeeSchedGroups are on don't include clinics that are in a group for that particular fee schedule.
					if(Preferences.GetBool(PreferenceName.ShowFeeSchedGroups)) {
						//At this point, we do not know if we are going to display fee sched groups
						AddFeeSchedGroupToComboBox(comboFeeSchedGroup1,feeSchedNum1Selected
							,feeSchedGroup1Num,listCombo1GroupNums,boxItemClinic);
						AddFeeSchedGroupToComboBox(comboFeeSchedGroup2,feeSchedNum2Selected
							,feeSchedGroup2Num,listCombo2GroupNums,boxItemClinic);
						AddFeeSchedGroupToComboBox(comboFeeSchedGroup3,feeSchedNum3Selected
							,feeSchedGroup3Num,listCombo3GroupNums,boxItemClinic);
					}
				}
			}
			//Fill provider combo boxes
			comboProvider1.Items.Add("None");
			comboProvider2.Items.Add("None");
			comboProvider3.Items.Add("None");
			for(int i=0;i<_listProviders.Count;i++) {
				comboProvider1.Items.Add(_listProviders[i].Abbr);
				comboProvider2.Items.Add(_listProviders[i].Abbr);
				comboProvider3.Items.Add(_listProviders[i].Abbr);
			}
			comboProvider1.SelectedIndex=comboProv1Idx > -1 ? comboProv1Idx:0;
			comboProvider2.SelectedIndex=comboProv2Idx > -1 ? comboProv2Idx:0;
			comboProvider3.SelectedIndex=comboProv3Idx > -1 ? comboProv3Idx:0;
			//If previously selected FeeSched was global, and the newly selected FeeSched is NOT global, select OD's selected Clinic in the combo box.
			if(_listFeeScheds.Count > 0 && comboFeeSched1.SelectedItem!=null && comboFeeSched1.GetSelected<FeeSchedule>().IsGlobal) {
				comboClinic1.Enabled=false;
				butPickClinic1.Enabled=false;
				comboClinic1.SelectedClinicNum=0;				
				comboProvider1.Enabled=false;
				butPickProv1.Enabled=false;
				comboProvider1.SelectedIndex=0;
				comboFeeSchedGroup1.Enabled=false;
			}
			else {//Newly selected FeeSched is NOT global
				if(PrefC.HasClinicsEnabled) {
					if(feeSchedNum1Selected==0 || comboClinic1.Enabled==false) {
						//Previously selected FeeSched WAS global or there was none selected previously, select OD's selected Clinic
						comboClinic1.SelectedClinicNum=Clinics.Active.Id;
					}
					comboClinic1.Enabled=true;
					butPickClinic1.Enabled=true;
					comboFeeSchedGroup1.Enabled=true;
				}
				comboProvider1.Enabled=true;
				butPickProv1.Enabled=true;
			}
			if(comboFeeSched2.SelectedIndex==0 || (comboFeeSched2.SelectedItem!=null && comboFeeSched2.GetSelected<FeeSchedule>().IsGlobal)) {
				comboClinic2.Enabled=false;
				butPickClinic2.Enabled=false;
				comboClinic2.SelectedClinicNum=0;
				comboProvider2.Enabled=false;
				butPickProv2.Enabled=false;
				comboProvider2.SelectedIndex=0;
				comboFeeSchedGroup2.Enabled=false;
			}
			else {//Newly selected FeeSched is NOT global
				if(PrefC.HasClinicsEnabled) {
					if(comboClinic2.Enabled==false) {
						//Previously selected FeeSched WAS global, select OD's selected Clinic
						comboClinic2.SelectedClinicNum=Clinics.Active.Id;
					}
					comboClinic2.Enabled=true;
					butPickClinic2.Enabled=true;
					comboFeeSchedGroup2.Enabled=true;
				}
				comboProvider2.Enabled=true;
				butPickProv2.Enabled=true;
			}
			if(comboFeeSched3.SelectedIndex==0 || (comboFeeSched3.SelectedItem!=null && comboFeeSched3.GetSelected<FeeSchedule>().IsGlobal)) {
				comboClinic3.Enabled=false;
				butPickClinic3.Enabled=false;
				comboClinic3.SelectedClinicNum=0;
				comboProvider3.Enabled=false;
				butPickProv3.Enabled=false;
				comboProvider3.SelectedIndex=0;
				comboFeeSchedGroup3.Enabled=false;
			}
			else {//Newly selected FeeSched is NOT global
				if(PrefC.HasClinicsEnabled) {
					if(comboClinic3.Enabled==false) {//Previously selected FeeSched WAS global
						//Select OD's selected Clinic
						comboClinic3.SelectedClinicNum=Clinics.Active.Id;
					}
					comboClinic3.Enabled=true;
					butPickClinic3.Enabled=true;
					comboFeeSchedGroup3.Enabled=true;
				}
				comboProvider3.Enabled=true;
				butPickProv3.Enabled=true;
			}
		}

		private void AddFeeSchedGroupToComboBox(ComboBox comboFeeSchedGroup
			,long feeSchedNumSelected,long feeSchedGroupNum,List<long> listComboGroupNums,ODBoxItem<Clinic> boxItemClinic)
		{
			//Returning null means we didn't find a FeeSchedGroup.
			FeeScheduleGroup feeSchedGroupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSchedNumSelected,boxItemClinic.Tag.Id);
			if(feeSchedGroupCur!=null) {
				//If there is a clinic in the group that the user does not have access to do not add the group to the combobox.
				//_listClinics already filters for clinic restrictions.
				if(!feeSchedGroupCur.ListClinicNumsAll.Exists(x => !_listClinics.Any(y => y.Id==x))) {
					if(!listComboGroupNums.Contains(feeSchedGroupCur.Id) && feeSchedGroupCur.ListClinicNumsAll.Count>0) {//Skip duplicate/empty groups.
						ODBoxItem<FeeScheduleGroup> boxItemFeeSchedGroup=new ODBoxItem<FeeScheduleGroup>(feeSchedGroupCur.Description,feeSchedGroupCur);
						comboFeeSchedGroup.Items.Add(boxItemFeeSchedGroup);
						listComboGroupNums.Add(feeSchedGroupCur.Id);
						//Set this fee sched group as the selection if it matches the previous selection before refreshing
						if(boxItemFeeSchedGroup.Tag.Id==feeSchedGroupNum) {
							comboFeeSchedGroup.SelectedItem=boxItemFeeSchedGroup;
						}
					}
				}
			}
		}

		///<summary>The only reason we use this is to keep interface snappy when entering a series of fees in the grid.  So the moment the user stops doing that and switches to something else is when we take the time to synch the changes to the db and start over fresh the next time.  We also do that whenever the selected feesched is changed, when closing form, when double clicking a code, importing, etc.  Use this method liberally.  Run it first with includeSynch=true, then run it again after doing other things, just to make sure we have fresh data for the new situation.</summary>
		private void SynchAndFillListFees(bool includeSynch){
			//first, synch the old list
			Cursor=Cursors.WaitCursor;
			if(includeSynch && _listFeesDb!=null){
				Fees.SynchList(_listFees,_listFeesDb);
			}
			//Then, fill a new list
			long feeSched1=0;
			if(comboFeeSched1.SelectedIndex>-1) {//0 idx is "none"
				feeSched1=_listFeeScheds[comboFeeSched1.SelectedIndex].Id;
			}
			long feeSched2=0;
			if(comboFeeSched2.SelectedIndex>0){//0 idx is "none"
				feeSched2=_listFeeScheds[comboFeeSched2.SelectedIndex-1].Id;
			}
			long feeSched3=0;
			if(comboFeeSched3.SelectedIndex>0){
				feeSched3=_listFeeScheds[comboFeeSched3.SelectedIndex-1].Id;
			}
			long provider1Num=0;
			long provider2Num=0;
			long provider3Num=0;
			if(comboProvider1.SelectedIndex>0) {
				provider1Num=_listProviders[comboProvider1.SelectedIndex-1].Id;
			}
			if(comboProvider2.SelectedIndex>0) {
				provider2Num=_listProviders[comboProvider2.SelectedIndex-1].Id;
			}
			if(comboProvider3.SelectedIndex>0) {
				provider3Num=_listProviders[comboProvider3.SelectedIndex-1].Id;
			}
			long clinic1Num=0;
			long clinic2Num=0;
			long clinic3Num=0;
			if(PrefC.HasClinicsEnabled) { //Clinics is on
				if(Preferences.GetBool(PreferenceName.ShowFeeSchedGroups)) {
					//First groupbox
					if(checkGroups1.Checked && comboFeeSchedGroup1.SelectedIndex>-1) {
						clinic1Num=comboFeeSchedGroup1.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
					}
					else {
						clinic1Num=comboClinic1.SelectedClinicNum;
					}
					//Second groupbox
					if(checkGroups2.Checked && comboFeeSchedGroup2.SelectedIndex>-1) {
						clinic2Num=comboFeeSchedGroup2.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
					}
					else {
							clinic2Num=comboClinic2.SelectedClinicNum;
					}
					//Third groupbox
					if(checkGroups3.Checked && comboFeeSchedGroup3.SelectedIndex>-1) {
						clinic3Num=comboFeeSchedGroup3.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
					}
					else {
							clinic3Num=comboClinic3.SelectedClinicNum;
					}
				}
				else {
					clinic1Num=comboClinic1.SelectedClinicNum;
					clinic2Num=comboClinic2.SelectedClinicNum;
					clinic3Num=comboClinic3.SelectedClinicNum;
				}
			}
			_listFees=Fees.GetListForScheds(feeSched1,clinic1Num,provider1Num,feeSched2,clinic2Num,provider2Num,feeSched3,clinic3Num,provider3Num);
			_listFeesDb=new List<Fee>();
			foreach(Fee fee in _listFees){
				_listFeesDb.Add(fee.Copy());
			}
			SaveLogs();//two entires for each fee.  There could be a lot.
			Cursor=Cursors.Default;
		}

		private void SaveLogs() {
			foreach(long feeNum in _dictFeeLogs.Keys) {
				foreach(SecurityLog secLog in _dictFeeLogs[feeNum]) {
					SecurityLogs.MakeLogEntry(secLog);
				}
			}
			_dictFeeLogs.Clear();
		}

		private void FillCats() {
			ArrayList selected=new ArrayList();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				selected.Add(CatList[listCategories.SelectedIndices[i]].Id);
			}
			if(checkShowHidden.Checked) {
				CatList=Definitions.GetDefsForCategory(DefinitionCategory.ProcCodeCats).ToArray();
			}
			else {
				CatList=Definitions.GetDefsForCategory(DefinitionCategory.ProcCodeCats,true).ToArray();
			}
			listCategories.Items.Clear();
			for(int i=0;i<CatList.Length;i++) {
				listCategories.Items.Add(CatList[i].Name);
				if(selected.Contains(CatList[i].Id)) {
					listCategories.SetSelected(i,true);
				}
			}
		}

		///<summary>FillGrid does not go to the db for fees.  That's done separately when new feescheds are selected.</summary>
		private void FillGrid() {
			if(_listFeeScheds.Count==0) {
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				gridMain.EndUpdate();
				MessageBox.Show("You must have at least one fee schedule created.");
				return;
			}
			int scroll=gridMain.ScrollValue;
			List<Definition> listCatDefs=new List<Definition>();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				listCatDefs.Add(CatList[listCategories.SelectedIndices[i]]);
			}
			FeeSchedule feeSched1=_listFeeScheds[comboFeeSched1.SelectedIndex]; //First feesched will always have something selected.
			FeeSchedule feeSched2=null;
			if(comboFeeSched2.SelectedIndex>0) {
				feeSched2=_listFeeScheds[comboFeeSched2.SelectedIndex-1];
			} 
			FeeSchedule feeSched3=null;
			if(comboFeeSched3.SelectedIndex>0){
				feeSched3=_listFeeScheds[comboFeeSched3.SelectedIndex-1];
			}
			//Provider nums will be 0 for "None" value.
			long provider1Num=0;
			long provider2Num=0;
			long provider3Num=0;
			if(comboProvider1.SelectedIndex>0) {
				provider1Num=_listProviders[comboProvider1.SelectedIndex-1].Id;
			}
			if(comboProvider2.SelectedIndex>0) {
				provider2Num=_listProviders[comboProvider2.SelectedIndex-1].Id;
			}
			if(comboProvider3.SelectedIndex>0) {
				provider3Num=_listProviders[comboProvider3.SelectedIndex-1].Id;
			}
			//Clinic nums will be 0 for "Default" or "Off" value.
			long clinic1Num=0;
			long clinic2Num=0;
			long clinic3Num=0;
			if(PrefC.HasClinicsEnabled) { //Clinics is on
				if(Preferences.GetBool(PreferenceName.ShowFeeSchedGroups)) {
					//First groupbox
					if(checkGroups1.Checked && comboFeeSchedGroup1.SelectedIndex>-1) {
						clinic1Num=comboFeeSchedGroup1.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
					}
					else {
						clinic1Num=comboClinic1.SelectedClinicNum;
					}
					//Second groupbox
					if(checkGroups2.Checked && comboFeeSchedGroup2.SelectedIndex>-1) {
						clinic2Num=comboFeeSchedGroup2.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
					}
					else {
						clinic2Num=comboClinic2.SelectedClinicNum;
					}
					//Third groupbox
					if(checkGroups3.Checked && comboFeeSchedGroup3.SelectedIndex>-1) {
						clinic3Num=comboFeeSchedGroup3.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
					}
					else {
						clinic3Num=comboClinic3.SelectedClinicNum;
					}
				}
				else {
					clinic1Num=comboClinic1.SelectedClinicNum;
					clinic2Num=comboClinic2.SelectedClinicNum;
					clinic3Num=comboClinic3.SelectedClinicNum;
				}
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			//The order of these columns are important for gridMain_CellDoubleClick(), gridMain_CellLeave(), and GridMain_CellEnter()
			GridColumn col=new GridColumn("Category",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("Description",206);
			gridMain.Columns.Add(col);
			col=new GridColumn("Abbr",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("Code",50);
			gridMain.Columns.Add(col);
			col=new GridColumn("Fee 1",50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new GridColumn("Fee 2",50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new GridColumn("Fee 3",50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			string searchAbbr=textAbbreviation.Text;
			string searchDesc=textDescription.Text;
			string searchCode=textCode.Text;
			List<ProcedureCode> listProcsForCats=new List<ProcedureCode>();
			//Loop through the list of categories which are ordered by def.ItemOrder.
			if(_procCodeSort==ProcCodeListSort.Category) {
				for(int i=0;i<listCatDefs.Count;i++) {
					//Get all procedure codes that are part of the selected category.  Then order the list of procedures by ProcCodes.
					//Append the list of ordered procedures to the master list of procedures for the selected categories.
					//Appending the procedure codes in this fashion keeps them ordered correctly via the definitions ItemOrder.
					listProcsForCats.AddRange(ProcedureCodes.GetWhereFromList(proc => proc.ProcedureCategory==listCatDefs[i].Id)
						.OrderBy(proc => proc.Code).ToList());
				}
			}
			else if(_procCodeSort==ProcCodeListSort.ProcCode) {
				for(int i = 0;i<listCatDefs.Count;i++) {
					listProcsForCats.AddRange(ProcedureCodes.GetWhereFromList(proc => proc.ProcedureCategory==listCatDefs[i].Id).ToList());
				}
				listProcsForCats=listProcsForCats.OrderBy(proc => proc.Code).ToList();
			}
			//Remove any procedures that do not meet our filters.
			listProcsForCats.RemoveAll(proc => !proc.ShortDescription.ToLower().Contains(searchAbbr.ToLower()));
			listProcsForCats.RemoveAll(proc => !proc.Description.ToLower().Contains(searchDesc.ToLower()));
			listProcsForCats.RemoveAll(proc => !proc.Code.ToLower().Contains(searchCode.ToLower()));
			if(IsSelectionMode) {
				listProcsForCats.RemoveAll(proc => proc.Code==ProcedureCodes.GroupProcCode);
			}
			string lastCategoryName="";
			foreach(ProcedureCode procCode in listProcsForCats) { 
				row=new GridRow();
				row.Tag=procCode;
				//Only show the category on the first procedure code in that category.
				string categoryName=Definitions.GetName(DefinitionCategory.ProcCodeCats,procCode.ProcedureCategory);
				if(lastCategoryName!=categoryName && _procCodeSort==ProcCodeListSort.Category) {
					row.Cells.Add(categoryName);
					lastCategoryName=categoryName;
				}
				else {//This proc code is in the same category or we are not sorting by category.
					row.Cells.Add("");
				}
				row.Cells.Add(procCode.Description);
				row.Cells.Add(procCode.ShortDescription);
				row.Cells.Add(procCode.Code);
				Fee fee1=Fees.GetFee(procCode.Id,feeSched1.Id,clinic1Num,provider1Num,_listFees);
				Fee fee2=null;
				if(feeSched2!=null) {
					fee2=Fees.GetFee(procCode.Id,feeSched2.Id,clinic2Num,provider2Num,_listFees);
				}
				Fee fee3=null;
				if(feeSched3!=null) {
					fee3=Fees.GetFee(procCode.Id,feeSched3.Id,clinic3Num,provider3Num,_listFees);
				}
				if(fee1==null || fee1.Amount==-1) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(fee1.Amount.ToString("n"));
					row.Cells[row.Cells.Count-1].ForeColor=GetColorForFee(fee1);
				}
				if(fee2==null || fee2.Amount==-1) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(fee2.Amount.ToString("n"));
					row.Cells[row.Cells.Count-1].ForeColor=GetColorForFee(fee2);
				}
				if(fee3==null || fee3.Amount==-1) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(fee3.Amount.ToString("n"));
					row.Cells[row.Cells.Count-1].ForeColor=GetColorForFee(fee3);
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=scroll;
		}

		private Color GetColorForFee(Fee fee) {
			if(fee.ClinicNum!=0 && fee.ProvNum!=0) {
				return _colorProvClinic;
			}
			if(fee.ClinicNum!=0) {
				return _colorClinic;
			}
			if(fee.ProvNum!=0) {
				return _colorProv;
			}
			return _colorDefault;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_needsSynch){
				SynchAndFillListFees(true);
			}
			if(IsSelectionMode) {
				SelectedCodeNum=((ProcedureCode)gridMain.Rows[e.Row].Tag).Id;
				ListSelectedProcCodes.Clear();
				ListSelectedProcCodes.Add(((ProcedureCode)gridMain.Rows[e.Row].Tag).Copy());
				DialogResult=DialogResult.OK;
				return;
			}
			//else not selecting a code
			if(!Security.IsAuthorized(Permissions.ProcCodeEdit)) {
				return;
			}
			if(e.Col>3) {
				//Do nothing. All columns > 3 are editable (You cannot double click).
				return;
			}
			//not on a fee: Edit code instead
			//changed=false;//We just updated the database and synced our cache, set changed to false.
			ProcedureCode procCode=(ProcedureCode)gridMain.Rows[e.Row].Tag;
			Definition defProcCat=Definitions.GetDefsForCategory(DefinitionCategory.ProcCodeCats).FirstOrDefault(x => x.Id==procCode.ProcedureCategory);
			FormProcCodeEdit formProcCodeEdit=new FormProcCodeEdit(procCode);
			formProcCodeEdit.IsNew=false;
			formProcCodeEdit.ShowHiddenCategories=(defProcCat!=null ? defProcCat.IsHidden : false);
			formProcCodeEdit.ShowDialog();
			_setInvalidProcCodes=true;
			//The user could have edited a fee within the Procedure Code Edit window or within one of it's children so we need to refresh our cache.
			//Yes, it could have even changed if the user Canceled out of the Proc Code Edit window (e.g. use FormProcCodeEditMore.cs)
			SynchAndFillListFees(false);
			FillGrid();
		}

		private void GridMain_CellEnter(object sender,ODGridClickEventArgs e) {
			Security.IsAuthorized(Permissions.FeeSchedEdit);//Show message if user does not have permission.
		}

		///<summary>Takes care of individual cell edits.  Calls FillGrid to refresh other columns using the same data.</summary>
		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			//This is where the real fee editing logic is.
			if(!Security.IsAuthorized(Permissions.FeeSchedEdit,true)) { //Don't do anything if they don't have permission.
				return;
			}
			//Logic only works for columns 4 to 6.
			long codeNum=((ProcedureCode)gridMain.Rows[e.Row].Tag).Id;
			FeeSchedule feeSched=null;
			long provNum=0;
			long clinicNum=0;
			Fee fee=null;
			bool isEditingGroup=false;
			if(e.Col==4) {
				feeSched=_listFeeScheds[comboFeeSched1.SelectedIndex];
				if(comboProvider1.SelectedIndex>0) {
					provNum=_listProviders[comboProvider1.SelectedIndex-1].Id;
				}
				if(PrefC.HasClinicsEnabled) {
					if(checkGroups1.Checked && comboFeeSchedGroup1.SelectedIndex>-1) {
						clinicNum=comboFeeSchedGroup1.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
						isEditingGroup=true;
					}
					else {
						clinicNum=comboClinic1.SelectedClinicNum;
					}
				}
				fee=Fees.GetFee(codeNum,feeSched.Id,clinicNum,provNum,_listFees);
			}
			else if(e.Col==5) {
				if(comboFeeSched2.SelectedIndex==0) {//It's on the "none" option
					gridMain.Rows[e.Row].Cells[e.Col].Text="";
					return;
				}
				feeSched=_listFeeScheds[comboFeeSched2.SelectedIndex-1];
				if(comboProvider2.SelectedIndex>0) {
					provNum=_listProviders[comboProvider2.SelectedIndex-1].Id;
				}
				if(PrefC.HasClinicsEnabled) {
					if(checkGroups2.Checked && comboFeeSchedGroup2.SelectedIndex>-1) {
						clinicNum=comboFeeSchedGroup2.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
						isEditingGroup=true;
					}
					else {
						clinicNum=comboClinic2.SelectedClinicNum;
					}
				}
				fee=Fees.GetFee(codeNum,feeSched.Id,clinicNum,provNum,_listFees);
			}
			else if(e.Col==6) {
				if(comboFeeSched3.SelectedIndex==0) {//It's on the "none" option
					gridMain.Rows[e.Row].Cells[e.Col].Text="";
					return;
				}
				feeSched=_listFeeScheds[comboFeeSched3.SelectedIndex-1];
				if(comboProvider3.SelectedIndex>0) {
					provNum=_listProviders[comboProvider3.SelectedIndex-1].Id;
				}
				if(PrefC.HasClinicsEnabled) {
					if(checkGroups3.Checked && comboFeeSchedGroup3.SelectedIndex>-1) {
						clinicNum=comboFeeSchedGroup3.GetSelected<FeeScheduleGroup>().ListClinicNumsAll.FirstOrDefault();
						isEditingGroup=true;
					}
					else {
						clinicNum=comboClinic3.SelectedClinicNum;
					}
				}
				fee=Fees.GetFee(codeNum,feeSched.Id,clinicNum,provNum,_listFees);
			}
			//Fees set in the 3 ifs above.  If too slow, we will just put the fee into a Tag during FillGrid() and run FillGrid less
			//Fee fee=_feeCache.GetFee(codeNum,feeSched.FeeSchedNum,clinicNum,provNum);
			DateTime datePrevious=DateTime.MinValue;
			string feeAmtOld="";
			if(fee!=null){
				datePrevious = fee.SecDateTEdit;
				feeAmtOld=fee.Amount.ToString("n");
			}
			string feeAmtNewStr=gridMain.Rows[e.Row].Cells[e.Col].Text;
			double feeAmtNew=0;
			//Attempt to parse the entered value for errors.
			if(feeAmtNewStr!="" && !Double.TryParse(gridMain.Rows[e.Row].Cells[e.Col].Text,out feeAmtNew)) {
				gridMain.SetSelected(new Point(e.Col,e.Row));
				gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtOld;
				MessageBox.Show("Please fix data entry error first.");
				return;
			}
			if(Fees.IsFeeAmtEqual(fee,feeAmtNewStr) || !FeeL.CanEditFee(feeSched,provNum,clinicNum)) {
				if(fee==null || fee.Amount==-1) {
					gridMain.Rows[e.Row].Cells[e.Col].Text="";
				}
				else {
					gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtOld;
				}
				gridMain.Invalidate();
				return;
			}
			//Can't use values from fee object as it could be null. Instead use values pulled from UI that are also used to set new fees below.
			if(Preferences.GetBool(PreferenceName.ShowFeeSchedGroups) && provNum==0 && !isEditingGroup) {//Ignore provider fees and don't block from editing a group.
				FeeScheduleGroup groupForClinic=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSched.Id,clinicNum);
				if(groupForClinic!=null) {
					MsgBox.Show("Fee Schedule: "+feeSched.Description+" for Clinic: "+(_listClinics.FirstOrDefault(x => x.Id==clinicNum)).Abbr+
						" is part of Fee Schedule Group: "+groupForClinic.Description+". The fees must be edited at the group level.");
					//Duplicating if check from above to prevent us from accidentally hitting users with infinite popups. We want the same end result for both of the checks
					//but we need to do the group check 2nd.
					if(fee==null || fee.Amount==-1) {
						gridMain.Rows[e.Row].Cells[e.Col].Text="";
					}
					else {
						gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtOld;
					}
					gridMain.Invalidate();
					return;
				}
			}
			if(feeAmtNewStr!="") {
				gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtNew.ToString("n"); //Fix the number formatting and display it.
			}
			if(feeSched.IsGlobal) { //Global fee schedules have only one fee so blindly insert/update the fee. There will be no more localized copy.
				if(fee==null) { //Fee doesn't exist, insert
					fee=new Fee();
					fee.FeeScheduleId=feeSched.Id;
					fee.CodeNum=codeNum;
					fee.Amount=feeAmtNew;
					fee.ClinicNum=0;
					fee.ProvNum=0;
					//Fees.Insert(fee);
					_listFees.Add(fee);
				}
				else { //Fee does exist, update or delete.
					if(feeAmtNewStr=="") { //They want to delete the fee
						//Fees.Delete(fee.FeeNum);
						_listFees.Remove(fee);
					}
					else { //They want to update the fee
						fee.Amount=feeAmtNew;
						//Fees.Update(fee);
					}
				}
			}
			else { //FeeSched isn't global.
				if(feeAmtNewStr=="") { //They want to delete the fee
					//NOTE: If they are deleting the HQ fee we insert a blank (-1) for the current settings.
					if((fee.ClinicNum==0 && fee.ProvNum==0)
						&& (clinicNum!=0 || provNum!=0))
					{ 
						//The best match found was the default fee which should never be deleted when editing a fee schedule for a clinic or provider.
						//In this specific scenario, we have to add a fee to the database for the selected clinic and/or provider with an amount of -1.
						fee=new Fee();
						fee.FeeScheduleId=feeSched.Id;
						fee.CodeNum=codeNum;
						fee.Amount=-1.0;
						fee.ClinicNum=clinicNum;
						fee.ProvNum=provNum;
						//Fees.Insert(fee);
						_listFees.Add(fee);
					}
					else {//They want to delete a fee for their current settings.
						//Fees.Delete(fee.FeeNum);
						_listFees.Remove(fee);
					}
				}
				//The fee did not previously exist, or the fee found isn't for the currently set settings.
				else if(fee==null || fee.ClinicNum!=clinicNum || fee.ProvNum!=provNum) {
					fee=new Fee();
					fee.FeeScheduleId=feeSched.Id;
					fee.CodeNum=codeNum;
					fee.Amount=feeAmtNew;
					fee.ClinicNum=clinicNum;
					fee.ProvNum=provNum;
					//Fees.Insert(fee);
					_listFees.Add(fee);
				}
				else { //Fee isn't null, is for our current clinic, is for the selected provider, and they don't want to delete it.  Just update.
					fee.Amount=feeAmtNew;
					//Fees.Update(fee);
				}
			}
			SecurityLog secLog=SecurityLogs.MakeLogEntryNoInsert(Permissions.ProcFeeEdit,0,"Procedure: "+ProcedureCodes.GetStringProcCode(fee.CodeNum)
				+", Fee: "+fee.Amount.ToString("c")
				+", Fee Schedule: "+FeeScheds.GetDescription(fee.FeeScheduleId)
				+". Manual edit in grid from Procedure Codes list.",fee.CodeNum, SecurityLogSource.None);
			_dictFeeLogs[fee.FeeNum]=new List<SecurityLog>();
			_dictFeeLogs[fee.FeeNum].Add(secLog);
			_dictFeeLogs[fee.FeeNum].Add(SecurityLogs.MakeLogEntryNoInsert(Permissions.LogFeeEdit,0,"Fee changed",fee.FeeNum,SecurityLogSource.None,
				objectDate:fee.SecDateTEdit));
			_needsSynch=true;
			FillGrid();
		}

		#region Search

		private void butAll_Click(object sender,EventArgs e) {
			for(int i=0;i<listCategories.Items.Count;i++) {
				listCategories.SetSelected(i,true);
			}
			FillGrid();
		}

		private void butEditCategories_Click(object sender,EventArgs e) {
			//won't even be visible if no permission
			ArrayList selected=new ArrayList();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				selected.Add(CatList[listCategories.SelectedIndices[i]].Id);
			}
			FormDefinitions FormD=new FormDefinitions(DefinitionCategory.ProcCodeCats);
			FormD.ShowDialog();
			DataValid.SetInvalid(InvalidType.Defs);
			FillCats();
			for(int i=0;i<CatList.Length;i++) {
				if(selected.Contains(CatList[i].Id)) {
					listCategories.SetSelected(i,true);
				}
			}
			//we need to move security log to within the definition window for more complete tracking
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
			FillGrid();
		}

		private void textAbbreviation_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textCode_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void listCategories_MouseUp(object sender,MouseEventArgs e) {
			FillGrid();
		}

		private void checkShowHidden_Click(object sender,EventArgs e) {
			FillCats();
			FillGrid();
		}

		private void butShowHiddenDefault_Click(object sender,EventArgs e) {
			Preferences.Set(PreferenceName.ProcCodeListShowHidden,checkShowHidden.Checked);
			string hiddenStatus="";
			if(checkShowHidden.Checked) {
				hiddenStatus="checked.";
			}
			else {
				hiddenStatus="unchecked.";
			}
			MessageBox.Show("Show Hidden will default to"+" "+hiddenStatus);
		}

		#endregion

		#region Procedure Codes

		private void butEditFeeSched_Click(object sender,System.EventArgs e) {
			if(_needsSynch){
				SynchAndFillListFees(true);
			}
			//We are launching in edit mode thus we must check the FeeSchedEdit permission type.
			if(!Security.IsAuthorized(Permissions.FeeSchedEdit)) {
				return;
			}
			FormFeeScheds FormF=new FormFeeScheds(false); //The Fee Scheds window can add or hide schedules.  It cannot delete schedules.
			FormF.ShowDialog();
			FillComboBoxes();
			//I don't think it would highlight a new sched, so refresh fees should not be needed.
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit,0,"Fee Schedules");
		}

		private void butTools_Click(object sender,System.EventArgs e) {
			if(_listFeeScheds.Count==0) {
				MessageBox.Show("At least one fee schedule is required before using Fee Tools.");
				return;
			}
			if(_needsSynch){
				SynchAndFillListFees(true);
			}
			long schedNum=_listFeeScheds[comboFeeSched1.SelectedIndex].Id;
			using(FormFeeSchedTools FormF=new FormFeeSchedTools(schedNum,_listFeeScheds,_listProviders,_listClinics)) {
				FormF.ShowDialog();
			}
			//Fees could have changed from within the FeeSchedTools window.  Refresh our local fees.
			if(Programs.IsEnabled(ProgramName.eClinicalWorks)) {
				FillComboBoxes();//To show possible added fee schedule.
			}
			SynchAndFillListFees(false);
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Fee Schedule Tools");
		}

		private void butExport_Click(object sender,EventArgs e) {
			if(ProcedureCodes.GetCount()==0) {
				MessageBox.Show("No procedurecodes are displayed for export.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,"Only the codes showing in this list will be exported.  Continue?")) {
				return;
			}
			List<ProcedureCode> listCodes=new List<ProcedureCode>();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				ProcedureCode procCode=(ProcedureCode)gridMain.Rows[i].Tag;
				if(procCode.Code=="") {
					continue;
				}
				procCode.DefaultProviderId=0;  //We do not want to export ProvNumDefault because the receiving DB will not have the same exact provNums.
				listCodes.Add(procCode);
			}
			string filename="ProcCodes.xml";
			string filePath=ODFileUtils.CombinePaths(Path.GetTempPath(),filename); 

				SaveFileDialog saveDlg=new SaveFileDialog();
				saveDlg.InitialDirectory=Preferences.GetString(PreferenceName.ExportPath);
				saveDlg.FileName=filename;
				if(saveDlg.ShowDialog()!=DialogResult.OK) {
					return;
				}
				filePath=saveDlg.FileName;
			
			XmlSerializer serializer=new XmlSerializer(typeof(List<ProcedureCode>));
			TextWriter writer=new StreamWriter(filePath);
			serializer.Serialize(writer,listCodes);
			writer.Close();

				MessageBox.Show("Exported");
			
		}

		private void butImport_Click(object sender,EventArgs e) {
			if(_needsSynch){
				SynchAndFillListFees(true);
			}
			OpenFileDialog openDlg=new OpenFileDialog();
			openDlg.InitialDirectory=Preferences.GetString(PreferenceName.ExportPath);
			if(openDlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			int rowsInserted=0;
			try {
				rowsInserted=ImportProcCodes(openDlg.FileName,null,"");
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				FillGrid();
				return;
			}
			MessageBox.Show("Procedure codes inserted"+": "+rowsInserted);
			DataValid.SetInvalid(InvalidType.Defs,InvalidType.ProcCodes);
			ProcedureCodes.RefreshCache();
			FillCats();
			SynchAndFillListFees(false);//just in case there is a new fee?
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Imported Procedure Codes");
		}

		///<summary>Can be called externally.  Surround with try catch.  Returns number of codes inserted. 
		///Supply path to file to import or a list of procedure codes, or an xml string.  Make sure to set the other two values blank or null.</summary>
		public static int ImportProcCodes(string path,List<ProcedureCode> listCodes,string xmlData) {
			//if(listCodes==null) {
			//	listCodes=new List<ProcedureCode>();
			//}
			////xmlData should already be tested ahead of time to make sure it's not blank.
			//XmlSerializer serializer=new XmlSerializer(typeof(List<ProcedureCode>));
			//if(path!="") {
			//	if(!File.Exists(path)) {
			//		throw new ApplicationException("File does not exist.");
			//	}
			//	try {
			//		using(TextReader reader=new StreamReader(path)) {
			//			listCodes=(List<ProcedureCode>)serializer.Deserialize(reader);
			//		}
			//	}
			//	catch {
			//		throw new ApplicationException("Invalid file format");
			//	}
			//}
			//else if(xmlData!="") {
			//	try {
			//		using(TextReader reader=new StringReader(xmlData)) {
			//			listCodes=(List<ProcedureCode>)serializer.Deserialize(reader);
			//		}
			//	}
			//	catch {
			//		throw new ApplicationException("xml format");
			//	}
			//	XmlDocument xmlDocNcodes=new XmlDocument();
			//	xmlDocNcodes.LoadXml(xmlData);
			//	//Currently this will only run for NoFeeProcCodes.txt
			//	//If we run this for another file we will need to double check the structure of the file and make changes to this if needed.
			//	foreach(XmlNode procNode in xmlDocNcodes.ChildNodes[1]){//1=ArrayOfProcedureCode
			//		string procCode="";
			//		string procCatDescript="";
			//		foreach(XmlNode procFieldNode in procNode.ChildNodes) {
			//			if(procFieldNode.Name=="ProcCode") {
			//				procCode=procFieldNode.InnerText;
			//			}
			//			if(procFieldNode.Name=="ProcCatDescript") {
			//				procCatDescript=procFieldNode.InnerText;
			//			}
			//		}
			//		listCodes.First(x => x.Code==procCode).ProcCatDescript=procCatDescript;
			//	}
			//}
			//int retVal=0;
			//for(int i=0;i<listCodes.Count;i++) {
			//	if(ProcedureCodes.GetContainsKey(listCodes[i].Code)) {
			//		continue;//don't import duplicates.
			//	}
			//	listCodes[i].ProcedureCategory=Definitions.GetByExactName(DefinitionCategory.ProcCodeCats,listCodes[i].ProcCatDescript);
			//	if(listCodes[i].ProcedureCategory==0) {//no category exists with that name
			//		Definition def=new Definition();
			//		def.Category=DefinitionCategory.ProcCodeCats;
			//		def.Name=listCodes[i].ProcCatDescript;
			//		def.SortOrder=Definitions.GetDefsForCategory(DefinitionCategory.ProcCodeCats).Count;
			//		Definitions.Insert(def);
			//		Cache.Refresh(InvalidType.Defs);
			//		listCodes[i].ProcedureCategory=def.Id;
			//	}
			//	listCodes[i].DefaultProviderId=0;//Always import procedure codes with no specific provider set.  The incoming prov might not exist.
			//	ProcedureCodes.Insert(listCodes[i]);				
			//	SecurityLogs.MakeLogEntry(Permissions.ProcCodeEdit,0,"Code"+listCodes[i].Code+" added from procedure code import.",listCodes[i].Id,
			//		DateTime.MinValue);
			//	retVal++;
			//}
			//return retVal;
			//don't forget to refresh procedurecodes

			return 0;
		}

		private void butProcTools_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin, DateTime.MinValue)) {
				return;
			}
			if(_needsSynch){
				SynchAndFillListFees(true);
			}
			FormProcTools FormP=new FormProcTools();
			FormP.ShowDialog();
			if(!FormP.Changed) {
				return;
			}
			FillCats();
			ProcedureCodes.RefreshCache();
			//the form above already fired off all necessary invalidation signals
			SynchAndFillListFees(false);
			FillGrid();
		}

		private void butNew_Click(object sender,System.EventArgs e) {
			//won't be visible if no permission
			if(_needsSynch){
				SynchAndFillListFees(true);
			}
			FormProcCodeNew formProcCodeNew=new FormProcCodeNew();
			formProcCodeNew.ShowDialog();
			if(!formProcCodeNew.Changed) {
				return;
			}
			_setInvalidProcCodes=true;
			ProcedureCodes.RefreshCache();
			SynchAndFillListFees(false);
			FillGrid();
		}
		#endregion

		#region Compare Fee Schedules
		///<summary>For all three combo feescheds</summary>
		private void comboFeeSched_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboBoxes();//for global fee scheds, to disable other combos
			SynchAndFillListFees(true);
			FillGrid();
		}

		///<summary>For all 6 clinic and provider combos</summary>
		private void comboClinicProv_SelectionChangeCommitted(object sender,EventArgs e) {
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void butPickFeeSched_Click(object sender,EventArgs e) {
			int selectedIndex=GetFeeSchedIndexFromPicker();
			//If the selectedIndex is -1, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==-1) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickSched1) { //First FeeSched combobox doesn't have "None" option.
				comboFeeSched1.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickSched2) {
				comboFeeSched2.SelectedIndex=selectedIndex+1;
			}
			else if(pickerButton==butPickSched3) {
				comboFeeSched3.SelectedIndex=selectedIndex+1;
			}
			FillComboBoxes();//to handle global fee scheds
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void butPickClinic_Click(object sender,EventArgs e){
			List<Clinic> listClinicsToShow=new List<Clinic>();
			List<FeeScheduleGroup> listGroupsToShow=new List<FeeScheduleGroup>();
			UI.Button pickerButton=(UI.Button)sender;
			bool isPickingFeeSchedGroup=false;
			//Build the list of clinics to show from the combobox tags.
			//We get the list of clinics from the comboboxes because that list has been filtered to prevent clinics already in FeeSchedGroups from showing.
			//TODO: overload to show generic picker for feeschedgroups.
			if(pickerButton==butPickClinic1) {
				if(checkGroups1.Checked){
					listGroupsToShow=(comboFeeSchedGroup1.Items.OfType<ODBoxItem<FeeScheduleGroup>>()).Select(x => x.Tag).ToList();
					isPickingFeeSchedGroup=true;
				}
				else {
					listClinicsToShow=comboClinic1.ListClinics;
				}
			}
			else if(pickerButton==butPickClinic2) {
				if(checkGroups2.Checked) {
					listGroupsToShow=(comboFeeSchedGroup2.Items.OfType<ODBoxItem<FeeScheduleGroup>>()).Select(x => x.Tag).ToList();
					isPickingFeeSchedGroup=true;
				}
				else {
					listClinicsToShow=comboClinic2.ListClinics;
				}
			}
			else if(pickerButton==butPickClinic3) {
				if(checkGroups3.Checked) {
					listGroupsToShow=(comboFeeSchedGroup3.Items.OfType<ODBoxItem<FeeScheduleGroup>>()).Select(x => x.Tag).ToList();
					isPickingFeeSchedGroup=true;
				}
				else {
					listClinicsToShow=comboClinic3.ListClinics;
				}
			}
			else {
				listClinicsToShow=_listClinics;
			}
			int selectedIndex=0;
			long selectedClinicNum=0;
			if(isPickingFeeSchedGroup) {
				selectedIndex=GetFeeSchedGroupIndexFromPicker(listGroupsToShow);
				if(pickerButton==butPickClinic1 && comboFeeSchedGroup1.Items.Count>0) {
					comboFeeSchedGroup1.SelectedIndex=selectedIndex;
				}
				else if(pickerButton==butPickClinic2 && comboFeeSchedGroup2.Items.Count>0) {
					comboFeeSchedGroup2.SelectedIndex=selectedIndex;
				}
				else if(pickerButton==butPickClinic3 && comboFeeSchedGroup3.Items.Count>0) {
					comboFeeSchedGroup3.SelectedIndex=selectedIndex;
				}
			}
			else {
				selectedClinicNum=GetClinicNumFromPicker(listClinicsToShow);//All clinic combo boxes have a none option, so always add 1.
				if(pickerButton==butPickClinic1) {
					comboClinic1.SelectedClinicNum=selectedClinicNum;
				}
				else if(pickerButton==butPickClinic2) {
					comboClinic2.SelectedClinicNum=selectedClinicNum;
				}
				else if(pickerButton==butPickClinic3) {
					comboClinic3.SelectedClinicNum=selectedClinicNum;
				}
			}
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void butPickProvider_Click(object sender,EventArgs e){
			int selectedIndex=GetProviderIndexFromPicker()+1;//All provider combo boxes have a none option, so always add 1.
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickProv1) {
				comboProvider1.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickProv2) {
				comboProvider2.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickProv3) {
				comboProvider3.SelectedIndex=selectedIndex;
			}
			SynchAndFillListFees(true);
			FillGrid();
		}

		///<summary>Launches the Provider picker and lets the user pick a specific provider.
		///Returns the index of the selected provider within the Provider Cache (short).  Returns -1 if the user cancels out of the window.</summary>
		private int GetProviderIndexFromPicker() {
			FormProviderPick FormP=new FormProviderPick();
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return -1;
			}
			return Providers.GetIndex(FormP.SelectedProviderId);
		}

		///<summary>Launches the Clinics window and lets the user pick a specific clinic.
		///Returns the index of the selected clinic within _arrayClinics.  Returns -1 if the user cancels out of the window.</summary>
		private long GetClinicNumFromPicker(List<Clinic> listClinics) {
			FormClinics FormC=new FormClinics(listClinics);
			FormC.IsSelectionMode=true;
			FormC.ShowDialog();
			return FormC.SelectedClinic?.Id ?? 0;
		}

		private int GetFeeSchedGroupIndexFromPicker(List<FeeScheduleGroup> listFeeSchedGroup) {
			List<GridColumn> listColumnHeaders=new List<GridColumn>() {
				new GridColumn("Description",100){ IsWidthDynamic=true }
			};
			List<GridRow> listRowValues=new List<GridRow>();
			listFeeSchedGroup.ForEach(x => {
				GridRow row=new GridRow(x.Description);
				row.Tag=x;
				listRowValues.Add(row);
			});
			string formTitle="Fee Schedule Group Picker";
			string gridTitle="Fee Schedule Groups";
			FormGridSelection form=new FormGridSelection(listColumnHeaders,listRowValues,formTitle,gridTitle);
			if(form.ShowDialog()==DialogResult.OK) {
				return listFeeSchedGroup.FindIndex((x => x.Id==((FeeScheduleGroup)form.ListSelectedTags[0]).Id));
			}
			//Nothing was selected. This matches what happens with GetClinicIndexFromPicker.
			return 0;
		}

		///<summary>Launches the Fee Schedules window and lets the user pick a specific schedule.
		///Returns the index of the selected schedule within _listFeeScheds.  Returns -1 if the user cancels out of the window.</summary>
		private int GetFeeSchedIndexFromPicker() {
			//No need to check security because we are launching the form in selection mode.
			FormFeeScheds FormFS=new FormFeeScheds(true);
			FormFS.ShowDialog();
			return _listFeeScheds.FindIndex(x => x.Id==FormFS.SelectedFeeSchedNum);//Returns index of the found element or -1.
		}

		#endregion

		private void comboSort_SelectionChangeCommitted(object sender,EventArgs e) {
			_procCodeSort=(ProcCodeListSort)comboSort.SelectedIndex;
			FillGrid();
		}

		private void checkGroups1_CheckedChanged(object sender,EventArgs e) {
			if(checkGroups1.Checked) {
				//Hide clinic combobox and wipe selection.
				comboClinic1.Visible=false;
				comboClinic1.SelectedClinicNum=0;
				comboFeeSchedGroup1.Visible=true;
				labelClinic1.Text="Fee Schedule Group";
			}
			else {
				comboClinic1.Visible=true;
				comboFeeSchedGroup1.Visible=false;
				labelClinic1.Text="Clinic";
			}
			//Making the picker window display FeeSchedGroups could be an enhamcement in the future.
			butPickClinic1.Enabled=!checkGroups1.Checked;
			FillComboBoxes();
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void checkGroups2_CheckedChanged(object sender,EventArgs e) {
			if(checkGroups2.Checked) {
				//Hide clinic combobox and wipe selection.
				comboClinic2.Visible=false;
				comboClinic2.SelectedClinicNum=0;
				comboFeeSchedGroup2.Visible=true;
				labelClinic2.Text="Fee Schedule Group";
			}
			else {
				comboClinic2.Visible=true;
				comboFeeSchedGroup2.Visible=false;
				labelClinic2.Text="Clinic";
			}
			//Making the picker window display FeeSchedGroups could be an enhamcement in the future.
			butPickClinic2.Enabled=!checkGroups2.Checked;
			FillComboBoxes();
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void checkGroups3_CheckedChanged(object sender,EventArgs e) {
			if(checkGroups3.Checked) {
				//Hide clinic combobox and wipe selection.
				comboClinic3.Visible=false;
				comboClinic3.SelectedClinicNum=0;
				comboFeeSchedGroup3.Visible=true;
				labelClinic3.Text="Fee Schedule Group";
			}
			else {
				comboClinic3.Visible=true;
				comboFeeSchedGroup3.Visible=false;
				labelClinic3.Text="Clinic";
			}
			//Making the picker window display FeeSchedGroups could be an enhamcement in the future.
			butPickClinic3.Enabled=!checkGroups3.Checked;
			FillComboBoxes();
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void comboFeeSchedGroup_SelectionChangeCommitted(object sender,EventArgs e) {
			SynchAndFillListFees(true);
			FillGrid();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MessageBox.Show("Please select a procedure code first.");
				return;
			}
			ListSelectedProcCodes=gridMain.SelectedTags<ProcedureCode>().Select(x => x.Copy()).ToList();
			SelectedCodeNum=ListSelectedProcCodes.First().Id;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProcedures_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			if(_needsSynch){
				Cursor=Cursors.WaitCursor;
				SynchAndFillListFees(true);
				Cursor=Cursors.Default;
			}
			if(_setInvalidProcCodes) {
				DataValid.SetInvalid(InvalidType.ProcCodes);
			}
		}
	}
}
