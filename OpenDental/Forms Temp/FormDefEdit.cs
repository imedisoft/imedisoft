/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public class FormDefEdit : ODForm {
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelValue;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.TextBox textValue;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butSelect;
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.Label labelColor;
		private System.Windows.Forms.CheckBox checkHidden;
		private Definition DefCur;
		//private Def 
		private OpenDental.UI.Button butDelete;
		private CheckBox checkExcludeSend;
		private CheckBox checkExcludeConfirm;
		private GroupBox groupEConfirm;
		private List<long> _listExcludeSendNums;
		private List<long> _listExcludeConfirmNums;
		///<summary>A list of DefNums that represent all of the Confirmation Statuses that should skip sending eReminders.</summary>
		private List<long> _listExcludeRemindNums;
		private List<long> _listExcludeThanksNums;
		private List<long> _listExcludeArrivalSendNums;
		private List<long> _listExcludeArrivalResponseNums;
		private UI.Button butClearValue;
		private DefCatOptions _defCatOptions;
		private string _selectedValueString;
		public bool IsDeleted = false;
		private GroupBox groupBoxEReminders;
		private CheckBox checkExcludeRemind;
		private CheckBox checkNoColor;
		private GroupBox groupBoxEThanks;
		private CheckBox checkExcludeThanks;
		private GroupBox groupBoxArrivals;
		private CheckBox checkExcludeArrivalSend;
		private CheckBox checkExcludeArrivalResponse;

		///<summary>The list of definitions that is showing in FormDefinitions.  This list will typically be out of synch with the cache.  Gets set in the constructor.</summary>
		private List<Definition> _defsList;
		
		///<summary>defCur should be the currently selected def from FormDefinitions.  defList is going to be the in-memory list of definitions currently displaying to the user.  defList typically is out of synch with the cache which is why we need to pass it in.</summary>
		public FormDefEdit(Definition defCur,List<Definition> defsList,DefCatOptions defCatOptions){
			InitializeComponent();// Required for Windows Form Designer support
			
			DefCur=defCur;
			_defCatOptions=defCatOptions;
			_defsList=defsList;
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefEdit));
			this.labelName = new System.Windows.Forms.Label();
			this.labelValue = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.textValue = new System.Windows.Forms.TextBox();
			this.butColor = new System.Windows.Forms.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.labelColor = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.butDelete = new OpenDental.UI.Button();
			this.checkExcludeSend = new System.Windows.Forms.CheckBox();
			this.checkExcludeConfirm = new System.Windows.Forms.CheckBox();
			this.groupEConfirm = new System.Windows.Forms.GroupBox();
			this.butSelect = new OpenDental.UI.Button();
			this.butClearValue = new OpenDental.UI.Button();
			this.groupBoxEReminders = new System.Windows.Forms.GroupBox();
			this.checkExcludeRemind = new System.Windows.Forms.CheckBox();
			this.checkNoColor = new System.Windows.Forms.CheckBox();
			this.groupBoxEThanks = new System.Windows.Forms.GroupBox();
			this.checkExcludeThanks = new System.Windows.Forms.CheckBox();
			this.groupBoxArrivals = new System.Windows.Forms.GroupBox();
			this.checkExcludeArrivalSend = new System.Windows.Forms.CheckBox();
			this.checkExcludeArrivalResponse = new System.Windows.Forms.CheckBox();
			this.groupEConfirm.SuspendLayout();
			this.groupBoxEReminders.SuspendLayout();
			this.groupBoxEThanks.SuspendLayout();
			this.groupBoxArrivals.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(12, 45);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(178, 17);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelValue
			// 
			this.labelValue.Location = new System.Drawing.Point(190, 34);
			this.labelValue.Name = "labelValue";
			this.labelValue.Size = new System.Drawing.Size(178, 28);
			this.labelValue.TabIndex = 1;
			this.labelValue.Text = "Value";
			this.labelValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(12, 64);
			this.textName.Multiline = true;
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(178, 64);
			this.textName.TabIndex = 0;
			// 
			// textValue
			// 
			this.textValue.Location = new System.Drawing.Point(190, 64);
			this.textValue.MaxLength = 256;
			this.textValue.Multiline = true;
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(178, 64);
			this.textValue.TabIndex = 1;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(371, 64);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 2;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// labelColor
			// 
			this.labelColor.Location = new System.Drawing.Point(371, 46);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(74, 16);
			this.labelColor.TabIndex = 5;
			this.labelColor.Text = "Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(255, 250);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(336, 250);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(12, 12);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(157, 18);
			this.checkHidden.TabIndex = 3;
			this.checkHidden.Text = "Hidden";
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::Imedisoft.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 250);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79, 25);
			this.butDelete.TabIndex = 6;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// checkExcludeSend
			// 
			this.checkExcludeSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeSend.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeSend.Name = "checkExcludeSend";
			this.checkExcludeSend.Size = new System.Drawing.Size(165, 18);
			this.checkExcludeSend.TabIndex = 7;
			this.checkExcludeSend.Text = "Exclude when sending";
			// 
			// checkExcludeConfirm
			// 
			this.checkExcludeConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeConfirm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeConfirm.Location = new System.Drawing.Point(6, 40);
			this.checkExcludeConfirm.Name = "checkExcludeConfirm";
			this.checkExcludeConfirm.Size = new System.Drawing.Size(165, 18);
			this.checkExcludeConfirm.TabIndex = 8;
			this.checkExcludeConfirm.Text = "Exclude when confirming";
			// 
			// groupEConfirm
			// 
			this.groupEConfirm.Controls.Add(this.checkExcludeSend);
			this.groupEConfirm.Controls.Add(this.checkExcludeConfirm);
			this.groupEConfirm.Location = new System.Drawing.Point(12, 130);
			this.groupEConfirm.Name = "groupEConfirm";
			this.groupEConfirm.Size = new System.Drawing.Size(177, 64);
			this.groupEConfirm.TabIndex = 9;
			this.groupEConfirm.TabStop = false;
			this.groupEConfirm.Text = "eConfirmations";
			// 
			// butSelect
			// 
			this.butSelect.Location = new System.Drawing.Point(371, 106);
			this.butSelect.Name = "butSelect";
			this.butSelect.Size = new System.Drawing.Size(21, 22);
			this.butSelect.TabIndex = 200;
			this.butSelect.Text = "...";
			this.butSelect.Click += new System.EventHandler(this.butSelect_Click);
			// 
			// butClearValue
			// 
			
			this.butClearValue.Image = global::Imedisoft.Properties.Resources.deleteX18;
			this.butClearValue.Location = new System.Drawing.Point(395, 106);
			this.butClearValue.Name = "butClearValue";
			this.butClearValue.Size = new System.Drawing.Size(21, 22);
			this.butClearValue.TabIndex = 201;
			this.butClearValue.Click += new System.EventHandler(this.butClearValue_Click);
			// 
			// groupBoxEReminders
			// 
			this.groupBoxEReminders.Controls.Add(this.checkExcludeRemind);
			this.groupBoxEReminders.Location = new System.Drawing.Point(12, 197);
			this.groupBoxEReminders.Name = "groupBoxEReminders";
			this.groupBoxEReminders.Size = new System.Drawing.Size(177, 46);
			this.groupBoxEReminders.TabIndex = 202;
			this.groupBoxEReminders.TabStop = false;
			this.groupBoxEReminders.Text = "eReminders";
			// 
			// checkExcludeRemind
			// 
			this.checkExcludeRemind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeRemind.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeRemind.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeRemind.Name = "checkExcludeRemind";
			this.checkExcludeRemind.Size = new System.Drawing.Size(165, 18);
			this.checkExcludeRemind.TabIndex = 7;
			this.checkExcludeRemind.Text = "Exclude when sending";
			// 
			// checkNoColor
			// 
			this.checkNoColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNoColor.Location = new System.Drawing.Point(260, 16);
			this.checkNoColor.Name = "checkNoColor";
			this.checkNoColor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkNoColor.Size = new System.Drawing.Size(141, 18);
			this.checkNoColor.TabIndex = 203;
			this.checkNoColor.Text = "No Color";
			this.checkNoColor.Visible = false;
			this.checkNoColor.CheckedChanged += new System.EventHandler(this.checkNoColor_CheckedChanged);
			// 
			// groupBoxEThanks
			// 
			this.groupBoxEThanks.Controls.Add(this.checkExcludeThanks);
			this.groupBoxEThanks.Location = new System.Drawing.Point(191, 197);
			this.groupBoxEThanks.Name = "groupBoxEThanks";
			this.groupBoxEThanks.Size = new System.Drawing.Size(177, 46);
			this.groupBoxEThanks.TabIndex = 203;
			this.groupBoxEThanks.TabStop = false;
			this.groupBoxEThanks.Text = "Automated Thank-You";
			// 
			// checkExcludeThanks
			// 
			this.checkExcludeThanks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeThanks.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeThanks.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeThanks.Name = "checkExcludeThanks";
			this.checkExcludeThanks.Size = new System.Drawing.Size(165, 18);
			this.checkExcludeThanks.TabIndex = 7;
			this.checkExcludeThanks.Text = "Exclude when sending";
			// 
			// groupBoxArrivals
			// 
			this.groupBoxArrivals.Controls.Add(this.checkExcludeArrivalSend);
			this.groupBoxArrivals.Controls.Add(this.checkExcludeArrivalResponse);
			this.groupBoxArrivals.Location = new System.Drawing.Point(191, 130);
			this.groupBoxArrivals.Name = "groupBoxArrivals";
			this.groupBoxArrivals.Size = new System.Drawing.Size(177, 64);
			this.groupBoxArrivals.TabIndex = 204;
			this.groupBoxArrivals.TabStop = false;
			this.groupBoxArrivals.Text = "Arrivals";
			// 
			// checkExcludeArrivalSend
			// 
			this.checkExcludeArrivalSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeArrivalSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeArrivalSend.Location = new System.Drawing.Point(6, 19);
			this.checkExcludeArrivalSend.Name = "checkExcludeArrivalSend";
			this.checkExcludeArrivalSend.Size = new System.Drawing.Size(165, 18);
			this.checkExcludeArrivalSend.TabIndex = 7;
			this.checkExcludeArrivalSend.Text = "Exclude when sending";
			// 
			// checkExcludeArrivalResponse
			// 
			this.checkExcludeArrivalResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeArrivalResponse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeArrivalResponse.Location = new System.Drawing.Point(6, 40);
			this.checkExcludeArrivalResponse.Name = "checkExcludeArrivalResponse";
			this.checkExcludeArrivalResponse.Size = new System.Drawing.Size(165, 18);
			this.checkExcludeArrivalResponse.TabIndex = 8;
			this.checkExcludeArrivalResponse.Text = "Exclude when responding";
			// 
			// FormDefEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(423, 283);
			this.Controls.Add(this.groupBoxArrivals);
			this.Controls.Add(this.groupBoxEThanks);
			this.Controls.Add(this.checkNoColor);
			this.Controls.Add(this.groupBoxEReminders);
			this.Controls.Add(this.butClearValue);
			this.Controls.Add(this.groupEConfirm);
			this.Controls.Add(this.butSelect);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.textValue);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.labelValue);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.labelColor);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(439, 322);
			this.Name = "FormDefEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Definition";
			this.Load += new System.EventHandler(this.FormDefEdit_Load);
			this.groupEConfirm.ResumeLayout(false);
			this.groupBoxEReminders.ResumeLayout(false);
			this.groupBoxEThanks.ResumeLayout(false);
			this.groupBoxArrivals.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDefEdit_Load(object sender, System.EventArgs e) {
			if(DefCur.Category==DefinitionCategory.ApptConfirmed) {
				_listExcludeSendNums=Prefs.GetString(PrefName.ApptConfirmExcludeESend).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeConfirmNums=Prefs.GetString(PrefName.ApptConfirmExcludeEConfirm).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeRemindNums=Prefs.GetString(PrefName.ApptConfirmExcludeERemind).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeThanksNums=Prefs.GetString(PrefName.ApptConfirmExcludeEThankYou).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeArrivalSendNums=Prefs.GetString(PrefName.ApptConfirmExcludeArrivalSend).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				_listExcludeArrivalResponseNums=Prefs.GetString(PrefName.ApptConfirmExcludeArrivalResponse).Split(',').ToList().Select(x => PIn.Long(x)).ToList();
				//0 will get automatically added to the list when this is the first of its kind.  We never want 0 inserted.
				_listExcludeSendNums.Remove(0);
				_listExcludeConfirmNums.Remove(0);
				_listExcludeRemindNums.Remove(0);
				_listExcludeThanksNums.Remove(0);
				_listExcludeArrivalSendNums.Remove(0);
				_listExcludeArrivalSendNums.Remove(0);
				checkExcludeSend.Checked=_listExcludeSendNums.Contains(DefCur.Id);
				checkExcludeConfirm.Checked=_listExcludeConfirmNums.Contains(DefCur.Id);
				checkExcludeRemind.Checked=_listExcludeRemindNums.Contains(DefCur.Id);
				checkExcludeThanks.Checked=_listExcludeThanksNums.Contains(DefCur.Id);
				checkExcludeArrivalSend.Checked=_listExcludeArrivalSendNums.Contains(DefCur.Id);
				checkExcludeArrivalResponse.Checked=_listExcludeArrivalResponseNums.Contains(DefCur.Id);
			}
			else {
				groupEConfirm.Visible=false;
				groupBoxEReminders.Visible=false;
				groupBoxEThanks.Visible=false;
				groupBoxArrivals.Visible=false;
			}
			if(DefCur.Id.In(Prefs.GetLong(PrefName.AppointmentTimeArrivedTrigger),Prefs.GetLong(PrefName.AppointmentTimeDismissedTrigger),
				Prefs.GetLong(PrefName.AppointmentTimeSeatedTrigger))) 
			{
				//We never want to send confirmation or reminders to an appointment when it is in a triggered confirm status.
				checkExcludeConfirm.Enabled=false;
				checkExcludeRemind.Enabled=false;
				checkExcludeSend.Enabled=false;
				checkExcludeThanks.Enabled=false;
				checkExcludeArrivalSend.Enabled=false;
				checkExcludeArrivalResponse.Enabled=false;
				checkExcludeConfirm.Checked=true;
				checkExcludeRemind.Checked=true;
				checkExcludeSend.Checked=true;
				checkExcludeThanks.Checked=true;
				checkExcludeArrivalSend.Checked=true;
				checkExcludeArrivalResponse.Checked=true;
			}
			string itemName=DefCur.Name;
			_selectedValueString=DefCur.Value;
			if(!_defCatOptions.CanEditName) {
				//Allow foreign users to translate definitions that they do not have access to translate.
				//Use FormDefinitions instead of 'this' because the users will have already translated the item names in that form and no need to duplicate.
				itemName=DefCur.Name;
				textName.ReadOnly=true;
				if(!DefCur.IsHidden || Definitions.IsDefDeprecated(DefCur)) {
					checkHidden.Enabled=false;//prevent hiding defs that are hard-coded into OD. Prevent unhiding defs that are deprecated.
				}
			}
			labelValue.Text=_defCatOptions.ValueText;
			if(DefCur.Category==DefinitionCategory.AdjTypes && !IsNew){
				labelValue.Text="Not allowed to change type after an adjustment is created.";
				textValue.Visible=false;
			}
			if(DefCur.Category==DefinitionCategory.BillingTypes) {
				labelValue.Text="E=Email bill, C=Collection, CE=Collection Excluded";
			}
			if(DefCur.Category==DefinitionCategory.PaySplitUnearnedType) {
				labelValue.Text="X=Do Not Show in Account or on Reports";
			}
			if(!_defCatOptions.EnableValue){
				labelValue.Visible=false;
				textValue.Visible=false;
			}
			if(!_defCatOptions.EnableColor){
				labelColor.Visible=false;
				butColor.Visible=false;
			}
			if(!_defCatOptions.CanHide){
				checkHidden.Visible=false;
			}
			if(!_defCatOptions.CanDelete){
				butDelete.Visible=false;
			}
			if(_defCatOptions.IsValueDefNum) {
				textValue.ReadOnly=true;
				textValue.BackColor=SystemColors.Control;
				labelValue.Text="Use the select button to choose a definition from the list.";
				long defNumCur=PIn.Long(DefCur.Value??"");
				if(defNumCur>0) {
					textValue.Text=_defsList.FirstOrDefault(x => defNumCur==x.Id)?.Name??"";
				}
				butSelect.Visible=true;
				butClearValue.Visible=true;
			}
			else if(_defCatOptions.DoShowItemOrderInValue) {
				labelValue.Text="Internal Priority";
				textValue.Text=DefCur.SortOrder.ToString();
				textValue.ReadOnly=true;
				butSelect.Visible=false;
				butClearValue.Visible=false;
			}
			else {
				textValue.Text=DefCur.Value;
				butSelect.Visible=false;
				butClearValue.Visible=false;
			}
			textName.Text=itemName;
			butColor.BackColor=DefCur.Color;
			checkHidden.Checked=DefCur.IsHidden;
			if(_defCatOptions.DoShowNoColor) {
				checkNoColor.Visible=true;
				//If there is no color in the database currently, make the UI match this.
				checkNoColor.Checked=(DefCur.Color.ToArgb()==Color.Empty.ToArgb());
			}
		}

		private void butColor_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
			checkNoColor.Checked=(colorDialog1.Color.ToArgb()==Color.Empty.ToArgb());
			//textColor.Text=colorDialog1.Color.Name;
		}

		private void butSelect_Click(object sender,EventArgs e) {
			long defNumParent=PIn.Long(DefCur.Value);//ItemValue could be blank, in which case defNumCur will be 0
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCur.Category,_defsList.ToList().FindAll(x => x.Id==defNumParent),DefCur.Id);
			FormDP.IsMultiSelectionMode=false;
			FormDP.HasShowHiddenOption=false;
			FormDP.ShowDialog();
			if(FormDP.DialogResult!=DialogResult.OK) {
				return;
			}
			Definition selectedDef=FormDP.ListSelectedDefs.DefaultIfEmpty(new Definition() { Name="" }).First();
			_selectedValueString=selectedDef.Id==0?"":selectedDef.Id.ToString();//list should have exactly one def in it, but this is safe
			textValue.Text=selectedDef.Name;
		}

		private void butClearValue_Click(object sender,EventArgs e) {
			_selectedValueString="";
			textValue.Clear();
		}

		private void checkNoColor_CheckedChanged(object sender,EventArgs e) {
			if(checkNoColor.Checked) {//Reset to empty color to tell the user the color is disabled.
				butColor.BackColor=Color.Empty;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			//This is VERY new.  Only allowed and visible for three categories so far: supply cats, claim payment types, and claim custom tracking.
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(DefCur.Category==DefinitionCategory.ClaimCustomTracking && _defsList.Count(x => x.Category==DefinitionCategory.ClaimCustomTracking)==1
				|| DefCur.Category==DefinitionCategory.InsurancePaymentType && _defsList.Count(x => x.Category==DefinitionCategory.InsurancePaymentType)==1
				|| DefCur.Category==DefinitionCategory.SupplyCats && _defsList.Count(x => x.Category==DefinitionCategory.SupplyCats)==1) 
			{
				MessageBox.Show("Cannot delete the last definition from this category.");
				return;
			}
			bool isAutoNoteRefresh=false;
			if(DefCur.Category==DefinitionCategory.AutoNoteCats && AutoNotes.GetExists(x => x.Category==DefCur.Id)) {
				if(!MsgBox.Show(MsgBoxButtons.YesNo,"Deleting this Auto Note Category will uncategorize some auto notes.  Delete anyway?")) {
					return;
				}
				isAutoNoteRefresh=true;
			}
			try{
				Definitions.Delete(DefCur);
				IsDeleted=true;
				if(isAutoNoteRefresh) {//deleting an auto note category currently in use will uncategorize those auto notes, refresh cache
					DataValid.SetInvalid(InvalidType.AutoNotes);
				}
				DialogResult=DialogResult.OK;
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(checkHidden.Checked && !IsNew) {
				if(!DefL.CanHideDef(DefCur,_defCatOptions)) {
					return;//CanHideDef() shows error message if def cannot be hidden, then we kick out here.
				}
			}
			if(textName.Text==""){
				MessageBox.Show("Name required.");
				return;
			}
			switch(DefCur.Category){
				case DefinitionCategory.AccountQuickCharge:
				case DefinitionCategory.ApptProcsQuickAdd:
					string[] procCodes=textValue.Text.Split(',');
					List<string> listProcCodes=new List<string>();
					for(int i=0;i<procCodes.Length;i++) {
						ProcedureCode procCode=ProcedureCodes.GetProcCode(procCodes[i]);
						if(procCode.CodeNum==0) {
							//Now check to see if the trimmed version of the code does not exist either.
							procCode=ProcedureCodes.GetProcCode(procCodes[i].Trim());
							if(procCode.CodeNum==0) {
								MessageBox.Show("Invalid procedure code entered"+": "+procCodes[i]);
								return;
							}
						}
						listProcCodes.Add(procCode.ProcCode);
					}
					textValue.Text=String.Join(",",listProcCodes);
					break;
				case DefinitionCategory.AdjTypes:
					if(textValue.Text!="+" && textValue.Text!="-" && textValue.Text!="dp"){
						MessageBox.Show("Valid values are +, -, or dp.");
						return;
					}
					break;
				case DefinitionCategory.BillingTypes:
					if(!textValue.Text.ToLower().In("","e","c","ce")) 
					{
						MessageBox.Show("Valid values are blank, E, C, or CE.");
						return;
					}
					break;
				case DefinitionCategory.ClaimCustomTracking:
					int value=0;
					if(!Int32.TryParse(textValue.Text,out value) || value<0) {
						MessageBox.Show("Days Suppressed must be a valid non-negative number.");
						return;
					}
					break;
				case DefinitionCategory.CommLogTypes:
					List<string> listCommItemTypes=Commlogs.GetCommItemTypes().Select(x => x.GetDescription(useShortVersionIfAvailable:true)).ToList();
					if(textValue.Text!="" && !listCommItemTypes.Any(x => x==textValue.Text)) {
						MessageBox.Show("Valid values are:"+" "+string.Join(", ",listCommItemTypes));
						return;
					}
					break;
				case DefinitionCategory.DiscountTypes:
					int discVal;
					if(textValue.Text=="") break;
					try {
						discVal=System.Convert.ToInt32(textValue.Text);
					}
					catch {
						MessageBox.Show("Not a valid number");
						return;
					}
					if(discVal < 0 || discVal > 100) {
						MessageBox.Show("Valid values are between 0 and 100");
						return;
					}
					textValue.Text=discVal.ToString();
					break;
				/*case DefinitionCategory.FeeSchedNames:
					if(textValue.Text=="C" || textValue.Text=="c") {
						textValue.Text="C";
					}
					else if(textValue.Text=="A" || textValue.Text=="a") {
						textValue.Text="A";
					}
					else textValue.Text="";
					break;*/
				case DefinitionCategory.ImageCats:
					textValue.Text=textValue.Text.ToUpper().Replace(",","");
					if(!Regex.IsMatch(textValue.Text,@"^[XPS]*$")){
						textValue.Text="";
					}
					break;
				case DefinitionCategory.InsurancePaymentType:
					if(textValue.Text!="" && textValue.Text!="N") {
						MessageBox.Show("Valid values are blank or N.");
						return;
					}
					break;
				case DefinitionCategory.PaySplitUnearnedType:
					if(!textValue.Text.ToLower().In("","x")) {
						MessageBox.Show("Valid values are blank or 'X'");
						return;
					}
					List<Definition> listDefsForUnearnedType=_defsList.FindAll(x => x.Category==DefinitionCategory.PaySplitUnearnedType);
					if(listDefsForUnearnedType.FindAll(x => string.IsNullOrEmpty(x.Value)).Count==1 && DefCur.Value=="" && textValue.Text!="" && !IsNew) {
						MessageBox.Show("Must have at least one definition that shows in Account.");
						return;
					}
					else if(listDefsForUnearnedType.FindAll(x => !string.IsNullOrEmpty(x.Value)).Count==1 && DefCur.Value!="" && textValue.Text=="") {
						MessageBox.Show("Must have at least one definition that does not show in Account.");
						return;
					}
					break;
				case DefinitionCategory.RecallUnschedStatus:
					if(textValue.Text.Length > 7){
						MessageBox.Show("Maximum length is 7.");
						return;
					}
					break;
				case DefinitionCategory.TxPriorities:
					if(textValue.Text.Length > 7){
						MessageBox.Show("Maximum length of abbreviation is 7.");
						return;
					}
					break;
				default:
					break;
			}//end switch DefCur.Category
			DefCur.Name=textName.Text;
			DefCur.Value=_selectedValueString;
			if(_defCatOptions.EnableValue && !_defCatOptions.IsValueDefNum) {
				DefCur.Value=textValue.Text;
			}
			if(_defCatOptions.EnableColor) {
				//If checkNoColor is checked, insert empty into the database. Otherwise, use the color they picked.
				DefCur.Color=(checkNoColor.Checked ? Color.Empty : butColor.BackColor);
			}
			DefCur.IsHidden=checkHidden.Checked;
			if(IsNew){
				Definitions.Insert(DefCur);
			}
			else{
				Definitions.Update(DefCur);
			}
			//Must be after the upsert so that we have access to the DefNum for new Defs.
			if(DefCur.Category==DefinitionCategory.ApptConfirmed) {
				//==================== EXCLUDE SEND ====================
				UpdateConfirmExcludes(checkExcludeSend,_listExcludeSendNums,DefCur,PrefName.ApptConfirmExcludeESend);
				//==================== EXCLUDE CONFIRM ====================
				UpdateConfirmExcludes(checkExcludeConfirm,_listExcludeConfirmNums,DefCur,PrefName.ApptConfirmExcludeEConfirm);
				//==================== EXCLUDE REMIND ====================				
				UpdateConfirmExcludes(checkExcludeRemind,_listExcludeRemindNums,DefCur,PrefName.ApptConfirmExcludeERemind);
				//==================== EXCLUDE THANKYOU ====================
				UpdateConfirmExcludes(checkExcludeThanks,_listExcludeThanksNums,DefCur,PrefName.ApptConfirmExcludeEThankYou);
				//==================== EXCLUDE ARRIVAL SEND ====================
				UpdateConfirmExcludes(checkExcludeArrivalSend,_listExcludeArrivalSendNums,DefCur,PrefName.ApptConfirmExcludeArrivalSend);
				//==================== EXCLUDE ARRIVAL RESPONSE ====================
				UpdateConfirmExcludes(checkExcludeArrivalResponse,_listExcludeArrivalResponseNums,DefCur,PrefName.ApptConfirmExcludeArrivalResponse);
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private static void UpdateConfirmExcludes(CheckBox check,List<long> listExcludeNums,Definition def, string prefName) {
			if(check.Checked) {
				listExcludeNums.Add(def.Id);
			}
			else {
				listExcludeNums.RemoveAll(x => x==def.Id);
			}
			string toString=string.Join(",",listExcludeNums.Distinct().OrderBy(x => x));
			Prefs.Set(prefName,toString);
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
