using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;
using CodeBase;

namespace UnitTests{
	public partial class FormComboTests : Form{
		public FormComboTests(){
			InitializeComponent();
			cClinic.IsTestModeNoDb=true;
			cClinicMulti.IsTestModeNoDb=true;
			cClinicL.IsTestModeNoDb=true;
			cClinicMultiL.IsTestModeNoDb=true;
			//new
			cClinic2.IsTestModeNoDb=true;
			cClinicMulti2.IsTestModeNoDb=true;
			cClinicL2.IsTestModeNoDb=true;
			cClinicMultiL2.IsTestModeNoDb=true;
			//in panel
			comboBoxClinicPicker1.IsTestModeNoDb=true;
			comboBoxClinicPicker2.IsTestModeNoDb=true;
			comboBoxClinicPicker3.IsTestModeNoDb=true;
			comboBoxClinicPicker4.IsTestModeNoDb=true;
		}

		private void FormComboTests_Load(object sender, EventArgs e){
			this.Location=new Point(Location.X,Location.Y-200);
			for(int i=0;i<15;i++){
				comboBoxMS.Items.Add("Item"+i.ToString());
				comboBoxMulti.Items.Add("Item"+i.ToString());
				//can't add clinics from outside
				if(i%2==0) {
					cPlus.Items.Add("Different"+i.ToString());
					cPlusMulti.Items.Add("Different"+i.ToString());
					cPlus2.Items.Add("Different"+i.ToString());
					cPlusMulti2.Items.Add("Different"+i.ToString());
				}
				else {
					cPlus.Items.Add("Item"+i.ToString());
					cPlusMulti.Items.Add("Item"+i.ToString());
					cPlus2.Items.Add("Item"+i.ToString());
					cPlusMulti2.Items.Add("Item"+i.ToString());
				}
			}
			List<string> listStrings=new List<string>(); 
			Random random=new Random();
			for(int i=0;i<=310;i++) {
				listStrings.Add(RandomString(random));
			}
			listStrings.Sort();
			for(int i=0;i<listStrings.Count;i++) {
				if(i<=30) {
					comboBoxPlus1.Items.Add(listStrings[i]);
					comboBoxPlus2.Items.Add(listStrings[i]);
				}
				comboBoxPlus3.Items.Add(listStrings[i]);
				comboBoxPlus4.Items.Add(listStrings[i]);
			}
			cPlus.SetSelected(2);
			cPlusMulti.SetSelected(2);
			cPlusMulti2.SetSelected(2);
			cPlus2.SetSelected(2);
			comboBoxMS.SelectedIndex=2;
			comboBoxMulti.SetSelected(2,true);
			cClinic.SelectedClinicNum=2;
			cClinicMulti.SelectedClinicNum=2;
			cClinic2.SelectedClinicNum=2;
			cClinicMulti2.SelectedClinicNum=2;
			//listboxes------------------------------------------
			for(int i=0;i<6;i++){
				listBoxMS.Items.Add("Item"+i.ToString());
				listBoxMSMulti.Items.Add("Item"+i.ToString());
			}
			listBoxMS.SelectedIndex=2;
			listBoxMSMulti.SelectedIndex=2;
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn("Items",100);
			gridMain.ListGridColumns.Add(col);
			 
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<20;i++){
				row=new GridRow();
				row.Cells.Add("Item"+i.ToString());		  
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void ButTest_Click(object sender, EventArgs e){
			
		}

		// Generate a random string with a given size  
		public string RandomString(Random random) {
			StringBuilder builder=new StringBuilder();
			string chars="ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string result=new string(
        Enumerable.Repeat(chars,9).Select(s => s[random.Next(s.Length)]).ToArray());
			return result.ToLower();
		}

		private void CPlus2_SelectionChangeCommitted(object sender,EventArgs e) {
			MsgBox.Show(sender,MsgBoxButtons.OKCancel,"This is a test");
		}

		private void CClinic2_SelectionChangeCommitted(object sender,EventArgs e) {
			MsgBox.Show(sender,MsgBoxButtons.OKCancel,"This is a test");
		}

		private void FormComboTests_DpiChanged(object sender, DpiChangedEventArgs e)
		{
			//Size=new Size(LogicalToDeviceUnits(Width),LogicalToDeviceUnits(Height));
		}

		private void FormComboTests_DpiChangedAfterParent(object sender, EventArgs e)
		{

		}

		private void FormComboTests_DpiChangedBeforeParent(object sender, EventArgs e)
		{

		}
	}
}
