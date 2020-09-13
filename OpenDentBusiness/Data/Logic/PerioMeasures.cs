using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class PerioMeasures
	{
		/// <summary>
		/// Bad pattern. 
		/// List of all perio measures for the current patient. 
		/// Dim 1 is exams. 
		/// Dim 2 is Sequences. 
		/// Dim 3 is Measurements, always 33 per sequence(0 is not used).  
		/// This public static variable is only used by the UI.  It's here because it would be complicated to put it in ContrPerio.
		/// </summary>
		public static PerioMeasure[,,] List;

		private static Dictionary<long, PerioMeasure[,]> Measures;

		public static IEnumerable<PerioMeasure> GetByPatient(long patientId)
			=> SelectMany(
				"SELECT pm.* " +
				"FROM `perio_measures` pm, `perio_exams` pe " +
				"WHERE pm.`perio_exam_id` = pe.`id` " +
				"AND pe.patient_id = " + patientId + " " +
				"ORDER BY pe.`exam_date`");

		public static IEnumerable<PerioMeasure> GetByPerioExam(long perioExamId)
			=> SelectMany(
				"SELECT * FROM `perio_measures` WHERE `perio_exam_id` = " + perioExamId);

		public static void Update(PerioMeasure perioMeasure) 
			=> ExecuteUpdate(perioMeasure);

		public static long Insert(PerioMeasure perioMeasure) 
			=> ExecuteInsert(perioMeasure);

		public static void Delete(PerioMeasure perioMeasure) 
			=> ExecuteDelete(perioMeasure);

		/// <summary>
		/// For the current exam, clears existing skipped teeth and resets them to the specified skipped teeth. The ArrayList valid values are 1-32 int.
		/// </summary>
		public static void SetSkipped(long perioExamId, List<int> skippedTeeth)
		{
			Database.ExecuteNonQuery(
				"DELETE FROM `perio_measures` " +
				"WHERE `perio_exam_id` = " + perioExamId + " " +
				"AND `sequence_type` = " + (int)PerioSequenceType.SkipTooth);

			foreach (var tooth in skippedTeeth)
			{
                Insert(new PerioMeasure
				{
					PerioExamId = perioExamId,
					SequenceType = PerioSequenceType.SkipTooth,
					Tooth = tooth,
					ToothValue = 1,
					MB = -1,
					B = -1,
					DB = -1,
					ML = -1,
					L = -1,
					DL = -1
				});
			}
		}

		/// <summary>
		/// Used in FormPerio.Add_Click. For the specified exam, gets a list of all skipped teeth. The ArrayList valid values are 1-32 int.
		/// </summary>
		public static List<int> GetSkipped(long perioExamId)
		{
			return Database.SelectMany(
				"SELECT `tooth` FROM `perio_measures` " +
				"WHERE `sequence_type` = " + (int)PerioSequenceType.SkipTooth + " " +
				"AND `perio_exam_id` = " + perioExamId + " AND `tooth_value` = '1'", 
					Database.ToScalar<int>).ToList();
		}

		/// <summary>
		/// Gets all measurements for the current patient, then organizes them by exam and sequence.
		/// </summary>
		public static void Refresh(long patientId, List<PerioExam> perioExams)
		{
			var perioSequenceTypes = Enum.GetNames(typeof(PerioSequenceType)).Length;

			Measures = new Dictionary<long, PerioMeasure[,]>();

			var perioMeasures = GetByPatient(patientId);
			foreach (var perioMeasure in perioMeasures)
            {
				if (!Measures.TryGetValue(perioMeasure.PerioExamId, out var examData))
                {
					examData = new PerioMeasure[perioSequenceTypes, 33];

					Measures[perioMeasure.PerioExamId] = examData;
                }

				examData[(int)perioMeasure.SequenceType, perioMeasure.Tooth] = perioMeasure;
            }
		}

		/// <summary>
		/// A -1 will be changed to a 0. Measures over 100 are changed to 100-measure. i.e. 100-104=-4 for hyperplastic GM.
		/// </summary>
		public static int AdjustGMVal(int measure)
		{
			if (measure == -1)
			{//-1 means no measurement, null.  In the places where this method is used, we have designed it to expect a 0 in those cases.
				return 0;
			}
			else if (measure > 100)
			{
				return 100 - measure;
			}
			return measure;//no adjustments needed.
		}
	}
}
