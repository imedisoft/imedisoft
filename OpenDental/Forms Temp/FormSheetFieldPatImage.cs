using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormSheetFieldPatImage:FormSheetFieldBase {
		private List<Definition> _listImageCatDefs;

		public FormSheetFieldPatImage(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly):base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			
		}

		private void FormSheetFieldPatImage_Load(object sender,EventArgs e) {
			FillCombo();
		}

		private void FillCombo(){
			comboImageCategory.Items.Clear();
			_listImageCatDefs=Definitions.GetDefsForCategory(DefinitionCategory.ImageCats,true);
			for(int i=0;i<_listImageCatDefs.Count;i++) {
				comboImageCategory.Items.Add(_listImageCatDefs[i].Name);
				if(SheetFieldDefCur.FieldName==_listImageCatDefs[i].Id.ToString()) {
					comboImageCategory.SelectedIndex=i;
				}
			}
		}

        protected override void OnOk() {
            if(!ArePosAndSizeValid()) {
                return;
            }
			if(comboImageCategory.SelectedIndex<0) {
				MessageBox.Show("Please select an image category first.");
				return;
			}
			SheetFieldDefCur.FieldName=_listImageCatDefs[comboImageCategory.SelectedIndex].Id.ToString();
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}