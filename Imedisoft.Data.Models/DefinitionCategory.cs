using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Imedisoft.Data.Models
{
    public static class DefinitionCategory
    {
		public const string AccountColors = "01";
		public const string AdjTypes = "02";
		public const string ApptConfirmed = "03";
		public const string ApptProcsQuickAdd = "04";
		public const string BillingTypes = "05";
		public const string PaymentTypes = "10";
		public const string ProcCodeCats = "11";
		public const string ProgNoteColors = "12";
		public const string RecallUnschedStatus = "13";
		public const string DiscountTypes = "15";
		public const string Diagnosis = "16";
		public const string AppointmentColors = "17";

		/// <summary>
		///		<list type = "table" >
		///			<item><code>X</code> Show in Chart Module</item>
		///			<item><code>F</code> Show in Patient Forms</item>
		///			<item><code>L</code> Show in Patient Portal</item>
		///			<item><code>P</code> Show in Patient Pictures</item>
		///			<item><code>S</code> Statements</item>
		///			<item><code>T</code> Graphical Tooth Chartsitem>
		///			<item><code>R</code> Treatment Plans</item>
		///			<item><code>E</code> Expanded</item>
		///			<item><code>A</code> Payment Plans</item>
		///			<item><code>C</code> Claim Attachments</item>
		///			<item><code>B</code> Lab Cases</item>
		///		</list>
		/// </summary>
		public const string ImageCats = "18";

		public const string TxPriorities = "20";
		public const string MiscColors = "21";
		public const string ChartGraphicColors = "22";
		public const string ContactCategories = "23";
		public const string LetterMergeCats = "24";
		public const string BlockoutTypes = "25";

		/// <summary>
		/// Categories of procedure buttons in the Chart module.
		/// </summary>
		public const string ProcButtonCats = "26";

		/// <summary>
		/// Types of commlog entries.
		/// </summary>
		public const string CommLogTypes = "27";

		/// <summary>
		/// Categories of supplies
		/// </summary>
		public const string SupplyCats = "28";

		///<summary>29- Types of unearned income used in accrual accounting.</summary>
		public const string PaySplitUnearnedType = "29";

		///<summary>30- Prognosis types.</summary>
		public const string Prognosis = "30";

		///<summary>31- Custom Tracking, statuses such as 'review', 'hold', 'riskmanage', etc.</summary>
		public const string ClaimCustomTracking = "31";

		///<summary>32- PayType for claims such as 'Check', 'EFT', etc.</summary>
		public const string InsurancePaymentType = "32";

		///<summary>33- Categories of priorities for tasks.</summary>
		public const string TaskPriorities = "33";

		///<summary>34- Categories for fee override colors.</summary>
		public const string FeeColors = "34";

		///<summary>35- Provider specialties.  General, Hygienist, Pediatric, Primary Care Physician, etc.</summary>
		public const string ProviderSpecialties = "35";

		///<summary>36- Reason why a claim proc was rejected. This must be set on each individual claim proc.</summary>
		public const string ClaimPaymentTracking = "36";

		///<summary>37- Procedure quick charge list for patient accounts.</summary>
		public const string AccountQuickCharge = "37";

		///<summary>38- Insurance verification status such as 'Verified', 'Unverified', 'Pending Verification'.</summary>
		public const string InsuranceVerificationStatus = "38";

		///<summary>39- Regions that clinics can be assigned to.</summary>
		public const string Regions = "39";

		///<summary>40- ClaimPayment Payment Groups.</summary>
		public const string ClaimPaymentGroups = "40";

		///<summary>41 - Auto Note Categories.  Used to categorize autonotes into custom categories.</summary>
		public const string AutoNoteCats = "41";

		///<summary>42 - Web Sched New Patient Appointment Types.  Displays in Web Sched.  Selected type shows in appointment note.</summary>
		public const string WebSchedNewPatApptTypes = "42";

		///<summary>43 - Custom Claim Status Error Code.</summary>
		public const string ClaimErrorCode = "43";

		///<summary>44 - Specialties that clinics perform.  Useful for separating patient clones across clinics.</summary>
		public const string ClinicSpecialty = "44";

		///<summary>46 - Carrier Group Name.</summary>
		public const string CarrierGroupNames = "46";

		///<summary>47 - PayPlanCategory</summary>
		public const string PayPlanCategories = "47";

		///<summary>48 - Associates an insurance payment to an account number.  Currently only used with "Auto Deposits".</summary>
		public const string AutoDeposit = "48";

		///<summary>49 - Code Group used for insurance filing.</summary>
		public const string InsuranceFilingCodeGroup = "49";

		/// <summary>
		/// Time card adjustment types.
		/// Currently for PTO, but in future could be used for other types as well if we implement the Usage def field.
		/// </summary>
		public const string TimeCardAdjTypes = "50";

		public static string GetDescription(string code)
		{
			switch (code)
            {
				case AccountColors: return Translation.Enums.DefinitionCategoryAccountColors;
				case AdjTypes: return "Adjustment Types";
				case ApptConfirmed:	return "Appt Confirmed";
				case ApptProcsQuickAdd: return "Appt Procs Quick Add";
				case BillingTypes: return "Billing Types";
				case PaymentTypes: return "Payment Types";
				case ProcCodeCats: return "Procedure Code Categories";
				case ProgNoteColors: return "Progress Notes Colors";
				case RecallUnschedStatus: return "Recall/Unscheduled Status";
				case DiscountTypes: return "Discount Types";
				case Diagnosis: return "Diagnosis Types";
				case AppointmentColors: return "Appointment Colors";
				case ImageCats: return "Image Categories";
				case TxPriorities: return "Treatment Plan Priorities";
				case MiscColors: return "Misc. Colors";
				case ChartGraphicColors: return "Chart Graphic Colors";
				case ContactCategories: return "Contact Categories";
				case LetterMergeCats: return "Letter Merge Categories";
				case BlockoutTypes: return "Blockout Types";
				case ProcButtonCats: return "Proc Button Categories";
				case CommLogTypes: return "Commlog Types";
				case SupplyCats: return "Supply Categories";
				case PaySplitUnearnedType: return "PaySplit Unearned Types";
				case Prognosis: return "Prognosis";
				case ClaimCustomTracking: return "Claim Custom Tracking";
				case InsurancePaymentType: return "Insurance Payment Types";
				case TaskPriorities: return "Task Priorities";
				case FeeColors: return "Fee Colors";
				case ProviderSpecialties: return "Provider Specialties";
				case ClaimPaymentTracking: return "Claim Payment Tracking";
				case AccountQuickCharge: return "Account Procs Quick Add";
				case InsuranceVerificationStatus: return "Insurance Verification Status";
				case Regions: return "Regions";
				case ClaimPaymentGroups: return "Claim Payment Groups";
				case AutoNoteCats: return "Auto Note Categories";
				case WebSchedNewPatApptTypes: return "Web Sched New Pat Appt Types";
				case ClaimErrorCode: return "Claim Error Code";
				case ClinicSpecialty: return "Clinic Specialties";
				case CarrierGroupNames: return "Carrier Group Names";
				case PayPlanCategories: return "Payment Plan Categories";
				case AutoDeposit: return "Auto Deposit Account";
				case InsuranceFilingCodeGroup: return "Insurance Filing Code Group";
				case TimeCardAdjTypes: return "Time Card Adj Types";
			}

			return "";
		}

		/// <summary>
		/// Enumerates all the definition categories.
		/// </summary>
		public static IEnumerable<string> All 
			=> typeof(DefinitionCategory)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(field => field.GetValue(null))
				.Cast<string>();
	}
}
