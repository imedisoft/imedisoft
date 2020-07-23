using System;
using OpenDentBusiness;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace OpenDentBusiness.WebTypes.WebForms
{
	[Serializable]
	public class WebForms_SheetDef : TableBase
	{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey = true)]
		public long WebSheetDefID;
		///<summary>FK to customers.patient.PatNum.</summary>
		public long DentalOfficeID;
		///<summary>The description of this sheetdef.</summary>
		public string Description;
		///<summary>Enum:SheetTypeEnum  The type of sheet.  Only PatientForm, MedicalHistory, and Consent sheet types are supported in Web Forms.
		///!!!NOTE!!! The actual enum type associated to this field is incorrect by pointing to the SheetFieldType enum.
		///This cannot easily change due to backwards compatibility reasons.
		///E.g. Older versions are expecting the value in the XML payload to be a string representation of SheetFieldType values.
		///This means that there needs to be a backwards compatible fix put in place when this enum type gets corrected.</summary>
		public SheetFieldType SheetType;
		///<summary>The default fontSize for the sheet.  The actual font must still be saved with each sheetField.</summary>
		public float FontSize;
		///<summary>The default fontName for the sheet.  The actual font must still be saved with each sheetField.</summary>
		public string FontName;
		///<summary>Width of the sheet in pixels, 100 pixels per inch.</summary>
		public int Width;
		///<summary>Height of the sheet in pixels, 100 pixels per inch.</summary>
		public int Height;
		///<summary>Set to true to print landscape.</summary>
		public bool IsLandscape;
		///<summary>FK to sheetdef.SheetDefNum.  The SheetDef that was used to create this sheet.</summary>
		public long SheetDefNum;
		///<summary>If true then this Sheet has been designed for mobile and will be displayed as a mobile-friendly WebForm.</summary>
		public bool HasMobileLayout;
		///<summary>This db column does not contain a valid value.  There is currently no such thing as a clinic specific sheet def.
		///The ClinicNum on the webforms_sheet table will get set via the ClinicNum that is passed into Web Forms GWT app via the URL.
		///This field needs to be treated as if it were a real db column so that the GWT to and from data table row methods contain ClinicNum.</summary>
		public long ClinicNum;

		public List<WebForms_SheetFieldDef> SheetFieldDefs;
	}
}
