using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges
{
    public static class ECW
	{
		/// <summary>
		/// AptNum is always passed in by eCW. It is used in the logic for setting procedures complete within apt edit window.
		/// </summary>
		public static long AptNum;
		public static string EcwConfigPath;
		public static long UserId;
		public static string JSessionId;
		public static string JSessionIdSSO;
		public static string LBSessionId;

		public static void SendHL7(long aptNum, long provNum, Patient pat, string pdfDataBase64, string pdfDescription, bool justPDF, List<Procedure> listProcs)
		{
			OpenDentBusiness.HL7.EcwDFT dft = new OpenDentBusiness.HL7.EcwDFT();
			dft.InitializeEcw(aptNum, provNum, pat, pdfDataBase64, pdfDescription, justPDF, listProcs);
			
			HL7Msg msg = new HL7Msg();
			if (justPDF)
			{
				msg.AptNum = 0; // Prevents the appt complete button from changing to the "Revise" button prematurely.
			}
			else
			{
				msg.AptNum = aptNum;
			}

			msg.HL7Status = HL7MessageStatus.OutPending; // It will be marked outSent by the HL7 service.
			msg.MsgText = dft.GenerateMessage();
			msg.PatNum = pat.PatNum;

			HL7ProcAttach hl7ProcAttach = new HL7ProcAttach();
			hl7ProcAttach.HL7MsgNum = HL7Msgs.Insert(msg);

			if (listProcs != null)
			{
				foreach (Procedure proc in listProcs)
				{
					hl7ProcAttach.ProcNum = proc.ProcNum;
					HL7ProcAttaches.Insert(hl7ProcAttach);
				}
			}
		}
	}
}
