using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenDentBusiness.AutoComm
{
	/// <summary>
	/// This class contains fields that are useful in appointment-type AutoComms.
	/// </summary>
	public class ApptLite : AutoCommObj
	{
		public ApptStatus AptStatus { get; set; }

		public DateTime AptDateTime { get; set; }

		public DateTime DateTimeAskedToArrive { get; set; }

		/// <summary>
		/// Indicates the name of the Office.
		/// </summary>
		public string OfficeName { get; set; }

		/// <summary>
		/// Indicates the phone number of the Office.
		/// </summary>
		public string OfficePhone { get; set; }

		/// <summary>
		/// Indicates the email of the Office.
		/// </summary>
		public string OfficeEmail { get; set; }

		/// <summary>
		/// From Appointment table, in minutes.
		/// </summary>
		public int Length { get; set; }

		public ApptLite()
		{
		}

		public ApptLite(Appointment appt, PatComm patComm, bool isForThankYou = false)
		{
			PrimaryKey = appt.AptNum;
			//For most AutoCommApptAbs, this will be AptDateTime, but for ApptThankYous, SecDateTEntry is used.
			DateTimeEvent = isForThankYou ? appt.SecDateTEntry : appt.AptDateTime;
			AptDateTime = appt.AptDateTime;
			DateTimeAskedToArrive = appt.DateTimeAskedToArrive;
			if (DateTimeAskedToArrive.Year < 1880)
			{
				DateTimeAskedToArrive = DateTimeEvent;
			}
			AptStatus = appt.AptStatus;
			ClinicNum = appt.ClinicNum;
			PatNum = appt.PatNum;
			ProvNum = appt.ProvNum;
			Clinic clinic = (appt.ClinicNum == 0) ? Clinics.GetPracticeAsClinicZero() : Clinics.GetById(appt.ClinicNum);
			OfficeName = Clinics.GetOfficeName(clinic);
			OfficePhone = Clinics.GetOfficePhone(clinic);
			OfficeEmail = EmailAddresses.GetByClinic(clinic?.Id ?? 0).SmtpUsername;
			Length = appt.Length;
			SetPatientContact(patComm);
		}
	}
}
