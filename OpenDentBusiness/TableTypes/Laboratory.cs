namespace OpenDentBusiness
{
    /// <summary>
    /// A dental laboratory. Will be attached to lab cases.
    /// </summary>
    public class Laboratory : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long LaboratoryNum;

		public string Description;

		///<summary>FK to sheetdef.SheetDefNum.  Lab slips can be set for individual laboratories.  If zero, then the default internal lab slip will be used instead of a custom lab slip.</summary>
		public long Slip;

		public string Phone;
		public string Notes;
		public string Address;
		public string City;
		public string State;
		public string Zip;
		public string Email;
		public string WirelessPhone;
		public bool IsHidden;
	}
}
