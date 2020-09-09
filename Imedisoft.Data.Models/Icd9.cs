﻿using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    [Table("icd9")]
	public class Icd9
	{
		[PrimaryKey]
		public string Code;

		public string Description;

		[Column(AutoGenerated = true)]
		public DateTime LastModifiedDate;

		/// <summary>
		/// Returns a string representation of the ICD-9 code.
		/// </summary>
		public override string ToString() => Code + "  " + Description;
    }
}