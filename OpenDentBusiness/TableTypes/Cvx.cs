using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness
{
    /// <summary>
    /// Vaccines administered. Other tables generally use the CvxCode as their foreign key.
    /// </summary>
    [Table]
	public class Cvx : TableBase
	{
		[PrimaryKey]
		public long CvxNum;

		/// <summary>
		/// Cvx code. Not allowed to edit this column once saved in the database.
		/// </summary>
		public string CvxCode;

		/// <summary>
		/// Short Description provided by Cvx documentation.
		/// </summary>
		public string Description;

		/// <summary>
		/// Not currently in use.  Might not need this column. 
		/// If we use this in the future, then convert from string to bool. 1 if the code is an active code, 0 if the code is inactive.
		/// </summary>
		public string IsActive;
	}
}