using OpenDentBusiness;

namespace Imedisoft.Bridges.Impl
{
	public class ActeonImagingSuite : CommandLineBridge
	{
		protected string GetPatientId(Program program, Patient patient)
        {
			var identificationType = ProgramProperties.Get(program.Id, ProgramPropertyName.PatientIdentificationType);

			return identificationType switch
			{
				"patient_id"   => patient.PatNum.ToString(),
				"chart_number" => patient.ChartNumber,
				_              => patient.PatNum.ToString()
			};
        }

        protected override string GetCommandLineArguments(Program program, Patient patient)
        {
			var dobFormat = ProgramProperties.Get(program.Id, "birth_date_format");
			if (string.IsNullOrEmpty(dobFormat))
            {
				dobFormat = "yyyyMMdd";
            }

			return $"{GetPatientId(program, patient)} \"{Tidy(patient.LName)}\" \"{Tidy(patient.FName)}\" \"{Tidy(patient.Birthdate.ToString(dobFormat))}\"";
        }

		private static string Tidy(string value) => value.Replace("\"", "");
	}
}
