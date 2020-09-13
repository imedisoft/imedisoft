using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
	/// User to specify user level permissions used for CDS interventions. 
	/// Unlike normal permissions and security, each permission has its own column and each employee has their own row.
	/// </summary>
    [Table("cds_permissions")]
	public class CdsPermission
	{
		[PrimaryKey(AutoIncrement = false)]
		public long UserId;

		/// <summary>
		/// A value indicating whether the user is allowed to edit EHR triggers.
		/// </summary>
		[Column("setup_cds")]
		public bool SetupCDS;

		/// <summary>
		/// True if user should see EHR triggers that are enabled. If false, no CDS interventions will show.
		/// </summary>
		[Column("show_cds")]
		public bool ShowCDS;

		/// <summary>
		/// True if user can see Infobutton.
		/// </summary>
		public bool ShowInfoButton;

		/// <summary>
		/// True if user can edit to bibliographic information.
		/// </summary>
		public bool EditBibliography;

		/// <summary>
		/// True to enable Problem based CDS interventions for this user.
		/// </summary>
		[Column("problem_cds")]
		public bool ProblemCDS;

		/// <summary>
		/// True to enable Medication based CDS interventions for this user.
		/// </summary>
		[Column("medication_cds")]
		public bool MedicationCDS;

		/// <summary>
		/// True to enable Allergy based CDS interventions for this user.
		/// </summary>
		[Column("allergy_cds")]
		public bool AllergyCDS;

		/// <summary>
		/// True to enable Demographic based CDS interventions for this user.
		/// </summary>
		[Column("demographic_cds")]
		public bool DemographicCDS;

		/// <summary>
		/// True to enable Lab Test based CDS interventions for this user.
		/// </summary>
		[Column("lab_test_cds")]
		public bool LabTestCDS;

		/// <summary>
		/// True to enable Vital Sign based CDS interventions for this user.
		/// </summary>
		[Column("vital_cds")]
		public bool VitalCDS;
	}
}
