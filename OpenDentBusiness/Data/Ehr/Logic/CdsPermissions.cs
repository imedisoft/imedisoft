using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class CdsPermissions
	{
		public static CdsPermission GetByUser(long userId) 
			=> SelectOne(userId) ?? new CdsPermission { UserId = userId };

		public static IEnumerable<CdsPermission> GetAll() 
			=> SelectMany("SELECT * FROM `cds_permissions`");

		private static void ExecuteInsertOrUpdate(CdsPermission cdsPermission)
			=> Database.ExecuteNonQuery(
				"INSERT INTO `cds_permissions` " +
				"(`user_id`, `setup_cds`, `show_cds`, `show_info_button`, `edit_bibliography`, `problem_cds`, `medication_cds`, `allergy_cds`, `demographic_cds`, `lab_test_cds`, `vital_cds`) " +
				"VALUES (" +
					"@user_id, @setup_cds, @show_cds, @show_info_button, @edit_bibliography, @problem_cds, @medication_cds, @allergy_cds, @demographic_cds, @lab_test_cds, @vital_cds" +
				")" +
				"ON DUPLICATE KEY UPDATE " +
					"`setup_cds` = @setup_cds, " +
					"`show_cds` = @show_cds, " +
					"`show_info_button` = @show_info_button, " +
					"`edit_bibliography` = @edit_bibliography, " +
					"`problem_cds` = @problem_cds, " +
					"`medication_cds` = @medication_cds, " +
					"`allergy_cds` = @allergy_cds, " +
					"`demographic_cds` = @demographic_cds, " +
					"`lab_test_cds` = @lab_test_cds, " +
					"`vital_cds` = @vital_cds",
					new MySqlParameter("user_id", cdsPermission.UserId),
					new MySqlParameter("setup_cds", cdsPermission.SetupCDS ? 1 : 0),
					new MySqlParameter("show_cds", cdsPermission.ShowCDS ? 1 : 0),
					new MySqlParameter("show_info_button", cdsPermission.ShowInfoButton ? 1 : 0),
					new MySqlParameter("edit_bibliography", cdsPermission.EditBibliography ? 1 : 0),
					new MySqlParameter("problem_cds", cdsPermission.ProblemCDS ? 1 : 0),
					new MySqlParameter("medication_cds", cdsPermission.MedicationCDS ? 1 : 0),
					new MySqlParameter("allergy_cds", cdsPermission.AllergyCDS ? 1 : 0),
					new MySqlParameter("demographic_cds", cdsPermission.DemographicCDS ? 1 : 0),
					new MySqlParameter("lab_test_cds", cdsPermission.LabTestCDS ? 1 : 0),
					new MySqlParameter("vital_cds", cdsPermission.VitalCDS ? 1 : 0));

		public static void Save(CdsPermission cdsPermission)
        {
			ExecuteInsertOrUpdate(cdsPermission);

			var logMessage = 
				"CDSPermChanged,U:" + cdsPermission.UserId + ","+
				(cdsPermission.SetupCDS ? "T" : "F") +
				(cdsPermission.ShowCDS ? "T" : "F") +
				(cdsPermission.ShowInfoButton ? "T" : "F") +
				(cdsPermission.EditBibliography ? "T" : "F") +
				(cdsPermission.ProblemCDS ? "T" : "F") +
				(cdsPermission.MedicationCDS ? "T" : "F") +
				(cdsPermission.AllergyCDS ? "T" : "F") +
				(cdsPermission.DemographicCDS ? "T" : "F") +
				(cdsPermission.LabTestCDS ? "T" : "F") +
				(cdsPermission.VitalCDS ? "T" : "F");

			SecurityLogs.Write(Permissions.SecurityAdmin, logMessage);
		}
	}
}
