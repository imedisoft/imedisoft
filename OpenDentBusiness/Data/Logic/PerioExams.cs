using Imedisoft.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class PerioExams
	{
		/// <summary>
		/// Bad pattern. This is public static because it would be hard to pass it into ContrPerio.  Only used by UI.
		/// </summary>
		public static List<PerioExam> Exams;

		/// <summary>
		/// Most recent date last. All exams loaded, even if not displayed.
		/// </summary>
		public static void Refresh(long patientId) 
			=> Exams = GetByPatient(patientId).ToList();

		public static PerioExam GetById(long perioExamId)
			=> SelectOne(perioExamId);

		public static IEnumerable<PerioExam> GetByPatient(long patientId)
			=> SelectMany("SELECT * FROM `perio_exams` WHERE `patient_id` = " + patientId + " ORDER BY `exam_data`");

		public static void Update(PerioExam perioExam) 
			=> ExecuteUpdate(perioExam);

		public static long Insert(PerioExam perioExam) 
			=> ExecuteInsert(perioExam);

		public static void Delete(PerioExam perioExam)
			=> ExecuteDelete(perioExam);
	}
}
