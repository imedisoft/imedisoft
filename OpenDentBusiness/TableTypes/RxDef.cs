using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// Rx definitions.
    /// Can safely delete or alter, because they get copied to the rxPat table, not referenced.
    /// </summary>
    [Table]
	public class RxDef : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The name of the drug.
		/// </summary>
		public string Drug;

		/// <summary>
		///		<para>
		///			Directions intended for the pharmacist.
		///		</para>
		///		<para>
		///			SIGs provide physician’s or pharmacist’s instructions to the patient on how, 
		///			how much, when and how long the drug is to be taken.They usually specify 
		///			frequency, route of administration, and any special instructions important in 
		///			administering the medication.
		///		</para>
		/// </summary>
		public string Sig;

		/// <summary>
		/// Amount to dispense.
		/// </summary>
		public string Disp;

		/// <summary>
		/// Number of refills.
		/// </summary>
		public string Refills;

		/// <summary>
		/// Notes about this drug. Will not be copied to the rxpat.
		/// </summary>
		public string Notes;

		/// <summary>
		/// A value indicating whether this is a controlled substance. This will affect the way it prints.
		/// </summary>
		public bool IsControlled;

		/// <summary>
		/// RxNorm Code identifier. 
		/// Copied down into medicationpat.RxCui (medical order) when a prescription is written.
		/// </summary>
		public long RxCui;

		/// <summary>
		/// If true will require procedure be attached to this prescription when printed. 
		/// Usually true if <see cref="IsControlled"/> is true.
		/// </summary>
		public bool IsProcRequired;

		/// <summary>
		/// Directions intended for the patient.
		/// </summary>
		public string PatientInstruction;

		public RxDef Copy()
		{
			return (RxDef)MemberwiseClone();
		}
	}
}
