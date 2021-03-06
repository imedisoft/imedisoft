﻿using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    [Table("allergies")]
	public class Allergy
	{
		[PrimaryKey]
		public long Id;

		//[ForeignKey(typeof(Patient), nameof(Patient.PatNum))]
		public long PatientId;

		[ForeignKey(typeof(AllergyDef), nameof(AllergyDef.Id))]
		public long AllergyDefId;

		/// <summary>
		/// A description of the adverse reaction.
		/// </summary>
		public string Reaction;

		/// <summary>
		/// The SNOMED-CT code that identifies the allergic reaction. Optional.
		/// </summary>
		[Nullable]
		public string ReactionSnomedCode;

		/// <summary>
		/// The historical date that the patient had the adverse reaction to this agent.
		/// </summary>
		public DateTime? AdverseReactionDate;

		/// <summary>
		/// The date and time on which the allergy was last modified.
		/// </summary>
		[Column(AutoGenerated = true)]
		public DateTime LastModifiedDate;

		/// <summary>
		/// A value indicating whether the allergy is active.
		/// </summary>
		public bool IsActive;
	}
}
