using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
	/// <summary>
	/// Insurance coverage categories.  They need to look like in the manual for the American calculations to work properly.
	/// </summary>
	[Table("")]
	public class CovCat : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// Description of this category.
		/// </summary>
		public string Description;

		/// <summary>
		/// Default percent for this category. -1 to skip this category and not apply a percentage.
		/// </summary>
		public int DefaultPercent;

		/// <summary>
		/// The order in which the categories are displayed. Includes hidden categories. 0-based.
		/// </summary>
		public byte CovOrder;

		/// <summary>
		/// If true, this category will be hidden.
		/// </summary>
		public bool IsHidden;

		/// <summary>
		/// Enum:EbenefitCategory The X12 benefit categories. 
		/// Each CovCat can link to one X12 category. 
		/// Default is 0 (unlinked).
		/// </summary>
		public EbenefitCategory EbenefitCat;

		public CovCat Copy()
		{
            return new CovCat
            {
                Id = Id,
                Description = Description,
                DefaultPercent = DefaultPercent,
                CovOrder = CovOrder,
                IsHidden = IsHidden,
                EbenefitCat = EbenefitCat
            };
		}
	}
}
