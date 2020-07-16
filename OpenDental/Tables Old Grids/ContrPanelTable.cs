/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/

namespace OpenDental
{

    ///<summary></summary>
    public class ContrPanelTable : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public ContrPanelTable()
		{
			InitializeComponent();
		}

		///<summary></summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent()
		{
			// 
			// ContrPanelTable
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Name = "ContrPanelTable";

		}
		#endregion
	}
}
