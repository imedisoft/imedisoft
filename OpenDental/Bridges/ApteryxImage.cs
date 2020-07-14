using System;
using System.Drawing;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
	public class ApteryxImage
	{
		public long Id { get; set; }

		public DateTime AcquisitionDate { get; set; }

		public long Width { get; set; }

		public long Height { get; set; }

		public long FileSize { get; set; }

		public string AdultTeeth { get; set; }

		public string DeciduousTeeth { get; set; }

		public string FormattedTeeth => 
			Tooth.FormatRangeForDisplay(
				AdultTeeth + (!string.IsNullOrEmpty(AdultTeeth) && !string.IsNullOrEmpty(DeciduousTeeth) ? "," : "") + DeciduousTeeth);
	}

	public class ApteryxThumbnail
	{
		public ApteryxImage Image;
		public Bitmap Thumbnail;
		public long PatNum;
	}
}
