using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Administrative Site (IIS)
    ///		</para>
    ///		<para>
    ///			Corresponds to HL7 table <b>0163</b>.
    ///		</para>
    /// </summary>
    public static class AdministrativeSite
	{
		public const string LeftThigh = "LT";
		public const string LeftArm = "LA";
		public const string LeftDeltoid = "LD";
		public const string LeftGluteousMedius = "LG";
		public const string LeftVastusLateralis = "RVL";
		public const string LeftLowerForearm = "LLFA";
		public const string RightArm = "RA";
		public const string RightThigh = "RT";
		public const string RightVastusLateralis = "RVL";
		public const string RightGluteousMedius = "RG";
		public const string RightDeltoid = "RD";
		public const string RightLowerForearm = "RLFA";

		public static IEnumerable<DataItem<string>> GetDataItems()
        {
			yield return new DataItem<string>(LeftThigh, "Left Thigh");
			yield return new DataItem<string>(LeftArm, "Left Arm");
			yield return new DataItem<string>(LeftDeltoid, "Left Deltoid");
			yield return new DataItem<string>(LeftGluteousMedius, "Left Gluteous Medius");
			yield return new DataItem<string>(LeftVastusLateralis, "Left Vastus Lateralis");
			yield return new DataItem<string>(LeftLowerForearm, "Left Lower Forearm");
			yield return new DataItem<string>(RightArm, "Right Arm");
			yield return new DataItem<string>(RightThigh, "Right Thigh");
			yield return new DataItem<string>(RightVastusLateralis, "Right Vastus Lateralis");
			yield return new DataItem<string>(RightGluteousMedius, "Right Gluteous Medius");
			yield return new DataItem<string>(RightDeltoid, "Right Deltoid");
			yield return new DataItem<string>(RightLowerForearm, "Right Lower Forearm");
		}
	}
}
