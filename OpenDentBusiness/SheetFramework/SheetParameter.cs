using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness
{
    public class SheetParameter
	{
		public bool IsRequired;

		/// <Summary>
		/// Usually, a columnName.
		/// </Summary>
		public string ParamName;

		/// <Summary>
		/// This is the value which must be set in order to obtain data from the database. It is usually an int primary key.
		/// If running a batch, this may be an array of int.
		/// </Summary>
		public object ParamValue;

		public SheetParameter Copy() 
			=> (SheetParameter)MemberwiseClone();

		/// <summary>
		/// Initializes a new instance of the <see cref="SheetParameter"/> class.
		/// </summary>
		public SheetParameter()
		{
			IsRequired = false;
			ParamName = "";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SheetParameter"/> class.
		/// </summary>
		public SheetParameter(bool isRequired, string paramName)
		{
			IsRequired = isRequired;
			ParamName = paramName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SheetParameter"/> class.
		/// </summary>
		public SheetParameter(bool isRequired, string paramName, string paramValue)
		{
			IsRequired = isRequired;
			ParamName = paramName;
			ParamValue = paramValue;
		}

		/// <Summary>
		/// Every sheet has at least one required parameter, usually the primary key of an important table.
		/// </Summary>
		public static IEnumerable<SheetParameter> GetForType(SheetTypeEnum sheetType)
		{
			switch (sheetType)
			{
				case SheetTypeEnum.LabelPatient:
					yield return new SheetParameter(true, "PatNum");
					break;

				case SheetTypeEnum.LabelCarrier:
					yield return new SheetParameter(true, "CarrierNum");
					break;

				case SheetTypeEnum.LabelReferral:
					yield return new SheetParameter(true, "ReferralNum");
					break;

				case SheetTypeEnum.ReferralSlip:
					yield return new SheetParameter(true, "PatNum");
					yield return new SheetParameter(true, "ReferralNum");
					break;

				case SheetTypeEnum.LabelAppointment:
					yield return new SheetParameter(true, "AptNum");
					break;

				case SheetTypeEnum.RxInstruction:
				case SheetTypeEnum.Rx:
					yield return new SheetParameter(true, "RxNum");
					break;

				case SheetTypeEnum.Consent:
					yield return new SheetParameter(true, "PatNum");
					break;

				case SheetTypeEnum.PatientLetter:
					yield return new SheetParameter(true, "PatNum");
					break;

				case SheetTypeEnum.ReferralLetter:
					yield return new SheetParameter(true, "PatNum");
					yield return new SheetParameter(true, "ReferralNum");
					yield return new SheetParameter(false, "CompletedProcs");
					yield return new SheetParameter(false, "toothChartImg");
					break;

				case SheetTypeEnum.PatientForm:
					yield return new SheetParameter(true, "PatNum");
					break;

				case SheetTypeEnum.RoutingSlip:
					yield return new SheetParameter(true, "AptNum");
					break;

				case SheetTypeEnum.MedicalHistory:
					yield return new SheetParameter(true, "PatNum");
					break;

				case SheetTypeEnum.LabSlip:
					yield return new SheetParameter(true, "PatNum");
					yield return new SheetParameter(true, "LabCaseNum");
					break;

				case SheetTypeEnum.ExamSheet:
					yield return new SheetParameter(true, "PatNum");
					break;

				case SheetTypeEnum.DepositSlip:
					yield return new SheetParameter(true, "DepositNum");
					break;

				case SheetTypeEnum.Screening:
					yield return new SheetParameter(true, "ScreenGroupNum");
					yield return new SheetParameter(false, "PatNum");
					yield return new SheetParameter(false, "ProvNum");
					break;

				case SheetTypeEnum.PaymentPlan:
					yield return new SheetParameter(false, "keyData");
					break;

				case SheetTypeEnum.RxMulti:
					yield return new SheetParameter(true, "ListRxNums");
					yield return new SheetParameter(true, "ListRxSheet");
					break;

				case SheetTypeEnum.ERA:
					yield return new SheetParameter(true, "ERA");
					yield return new SheetParameter(false, "IsSingleClaimPaid");
					break;

				case SheetTypeEnum.ERAGridHeader:
					yield return new SheetParameter(true, "EraClaimPaid");
					yield return new SheetParameter(true, "ClaimIndexNum");
					break;

				case SheetTypeEnum.TreatmentPlan:
				case SheetTypeEnum.Statement:
				case SheetTypeEnum.MedLabResults:
				default:
					break;
			}
		}

		public static void SetParameter(Sheet sheet, string paramName, object paramValue)
		{
			var sheetParameter = GetParamByName(sheet.Parameters, paramName);

			if (sheetParameter == null)
			{
				throw new ApplicationException("Parameter not found: " + paramName);
			}

			sheetParameter.ParamValue = paramValue;
		}

		public static SheetParameter GetParamByName(List<SheetParameter> sheetParameters, string paramName)
		{
			foreach (var sheetParameter in sheetParameters)
			{
				if (sheetParameter.ParamName == paramName)
				{
					return sheetParameter;
				}
			}

			return null;
		}
	}
}
