using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness
{
	/// <summary>
	/// Allows the user to specify DEA number override and other overrides for the provider at the specified clinic. 
	/// This is different from the ProviderClinicLink table. That table records which providers are restricted to which clinics.
	/// </summary>
	[Table("provider_clinics")]
	public class ProviderClinic : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Provider), nameof(Provider.Id))]
		public long ProviderId;

		[ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long ClinicId;

		/// <summary>
		/// The DEA number for this provider and clinic.
		/// </summary>
		public string DeaNumber;

		/// <summary>
		/// License number corresponding to the StateWhereLicensed. Can include punctuation.
		/// </summary>
		public string StateLicense;

		/// <summary>
		/// Provider medical State ID.
		/// </summary>
		public string StateRxId;

		/// <summary>
		/// The state abbreviation where the state license number in the StateLicense field is legally registered.
		/// </summary>
		public string StateWhereLicensed;

		public ProviderClinic Copy()
		{
			return (ProviderClinic)MemberwiseClone();
		}
	}
}

