using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental{
///<summary>See usage notes at bottom of this file.</summary>
	public class ValidNum : TextBox{
		private System.ComponentModel.Container components = null;
		public ErrorProvider errorProvider1=new ErrorProvider();
		///<summary></summary>
		private int maxVal=255;
		///<summary></summary>
		private int minVal=0;

		///<summary></summary>
		public ValidNum(){
			InitializeComponent();
			errorProvider1.BlinkStyle=ErrorBlinkStyle.NeverBlink;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ValidNum
			// 
			this.Validated += new System.EventHandler(this.ValidNum_Validated);
			this.Validating += new System.ComponentModel.CancelEventHandler(this.ValidNum_Validating);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary>The minumum value that this number can be set to without generating an error.</summary>
		public int MinVal{
			get{
				return minVal;
			}
			set{
				minVal=value;
			}
		}

		///<summary>The maximum value that this number can be set to without generating an error.</summary>
		public int MaxVal{
			get{
				return maxVal;
			}
			set{
				maxVal=value;
			}
		}

		///<summary>Returns true if a valid number has been entered. This replaces the older construct: if(textAbcd.errorProvider1.GetError(textAbcd)!="")</summary>
		public bool IsValid {
			get {
				return string.IsNullOrEmpty(errorProvider1.GetError(this));
			}
		}

		private void ValidNum_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			string myMessage="";
			try{
				if(Text==""){
					Text="0";
				}
				if(System.Convert.ToInt32(this.Text)>MaxVal)
					throw new Exception("Number must be less than "+(MaxVal+1).ToString());
				if(System.Convert.ToInt32(this.Text)<minVal)
					throw new Exception("Number must be greater than or equal to "+(minVal).ToString());
				errorProvider1.SetError(this,"");
			}
			catch(Exception ex){
				if(ex.Message=="Input string was not in a correct format."){
					myMessage="Must be a number. No letters or symbols allowed";
				}
				else{
					myMessage=ex.Message;
				}
				this.errorProvider1.SetError(this,myMessage);
			}
		}

		private void ValidNum_Validated(object sender, System.EventArgs e) {			
			//FormValid=true;
		}


	}
}

//Jordan 2019-12-26 How to use:
//The style of these controls is obviously outdated, but they are still very much the recommended approach until we do a minor overhaul. 
//Decide which Valid... to use.
//ValidNum: For integer fields.  If set to 0, then zero will show.
//ValidNumber: for integer fields.  If set to 0, then box will be blank.
//ValidDouble: for numbers with decimals.  
//ValidDate, VAlidTime, ValidPhone: I did not review those when making these instructions.
//Paste the chosen control onto the Form.  Do not use the similar controls found in ODR.
//In designer, set MaxVal and MinVal
//Name it text...  Example: textLength.
//Set text for the object field, as usual. Example: textLength.Text=appt.Length.ToString();
//In ButOK_Click, at the top, include this validation:
/*
			if(textLength.errorProvider1.GetError(textLength)!="")
				|| (test additional textboxes as needed) 
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
//Then parse the textbox as usual. Example: appt.Length=PIn.Int(textLength.Text);
*/

