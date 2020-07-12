using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web;

namespace OpenDentBusiness.Mobile
{
    public class PrefmC
	{
		// Cannot have a static variable here because we want something unique for each patient.
		public Dictionary<string, Prefm> Dict = new Dictionary<string, Prefm>();
	}
}
