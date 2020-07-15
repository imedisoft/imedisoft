using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentalGraph
{
    public partial class BaseGraphOptionsCtrl : UserControl
	{
		public event EventHandler InputsChanged;

		public BaseGraphOptionsCtrl()
		{
		}

		protected void OnBaseInputsChanged(object sender, EventArgs e)
		{
			if ((sender is RadioButton radioButton) && !radioButton.Checked)
			{ 
				// Another event is coming shorts that will be for the newly checked radio button. Wait for that one to avoid double processing.
				return;
			}

            InputsChanged?.Invoke(this, new EventArgs());
        }

		public virtual int GetPanelHeight() => 63;
		

		/// <summary>
		/// If you override this and your override can return true, make sure you check to see if Clinics are enabled before showing the grouping options.
		/// </summary>
		public virtual bool HasGroupOptions => OpenDentBusiness.PrefC.HasClinicsEnabled;
	}
}
