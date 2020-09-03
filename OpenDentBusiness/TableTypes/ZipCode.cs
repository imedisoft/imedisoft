using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
	/// <summary>
	/// Zipcodes are also known as postal codes. 
	/// Zipcodes are always copied to patient records rather than linked. 
	/// So items in this list can be freely altered or deleted without harming patient data.
	/// </summary>
	[Serializable]
	public class ZipCode : TableBase
	{
		[PrimaryKey]
		public long ZipCodeNum;

		public string ZipCodeDigits;
		public string City;
		public string State;

		/// <summary>
		/// If true, then it will show in the dropdown list in the patient edit window.
		/// </summary>
		public bool IsFrequent;

		public ZipCode Copy() => (ZipCode)MemberwiseClone();

		/// <summary>
		/// Returns a string representation of the zip code.
		/// </summary>
		/// <returns></returns>
        public override string ToString()
        {
			if (IsFrequent) return City + " " + State + " (freq)";
			else
			{
				return City + " " + State;
			}
		}
	}
}
