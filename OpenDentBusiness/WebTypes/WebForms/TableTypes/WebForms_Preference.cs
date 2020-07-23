using OpenDentBusiness;
using System.Drawing;
using System.Xml.Serialization;
using System;
using System.Runtime.Serialization;

namespace OpenDentBusiness.WebTypes.WebForms
{
	[Serializable]
	public class WebForms_Preference : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long DentalOfficeID;

		public Color ColorBorder;

		public string CultureName;

		public bool DisableSignatures;

		public WebForms_Preference()
		{
		}

		public WebForms_Preference Copy()
		{
			return (WebForms_Preference)MemberwiseClone();
		}
	}
}