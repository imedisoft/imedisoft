using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.Services.Models
{
    public class FeatureRequestDto
	{
		public long Id;
		public bool IsMine;
		public string Comments;
		public string VersionCompleted;
		public int Difficulty = 5;
		public double Weight;
		public FeatureRequestStatus Status;
		public string Description;
		public string Submitter;
		public int Priority = 5;
		public int Pledge;
		public bool IsCritical;
		public int TotalCrit;
		public int TotalComments;

		/// <summary>
		/// The bounty set for the feature, this in most cases is the sum of all pledges.
		/// </summary>
		public int Bounty;

		public static string GetStatusString(FeatureRequestStatus featureRequestStatus)
		{
			return featureRequestStatus switch
			{
				FeatureRequestStatus.New => "New",
				FeatureRequestStatus.NeedsClarification => "Needs Clarification",
				FeatureRequestStatus.Redundant => "Redundant",
				FeatureRequestStatus.TooBroad => "Too Broad",
				FeatureRequestStatus.NotARequest => "Not A Request",
				FeatureRequestStatus.AlreadyDone => "Already Done",
				FeatureRequestStatus.Obsolete => "Obsolete",
				FeatureRequestStatus.Approved => "Approved",
				FeatureRequestStatus.InProgress => "In Progress",
				FeatureRequestStatus.Complete => "Complete",

				_ => $"[TBT:{featureRequestStatus}]"
			};
		}

		public static string FormatAmount(int amount) 
			=> ((decimal)amount / 100).ToString("0.00");
	}
}
