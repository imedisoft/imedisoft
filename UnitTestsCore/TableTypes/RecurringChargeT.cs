using CodeBase;
using Imedisoft.Data;
using OpenDentBusiness;
using System;

namespace UnitTestsCore {
	public class RecurringChargeT {

		public static RecurringCharge CreateRecurringCharge(long patNum,RecurringChargeStatus status,double chargeAmt,long creditCardNum,
			DateTime? dateCharge = null) 
		{
			RecurringCharge charge=new RecurringCharge {
				ChargeAmt=chargeAmt,
				ChargeStatus=status,
				CreditCardNum=creditCardNum,
				PatNum=patNum,
				DateTimeCharge=dateCharge?.Date ?? DateTimeOD.Today,
			};
			RecurringCharges.Insert(charge);
			return charge;
		}

		///<summary>Deletes everything from the recurringcharge table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearRecurringChargeTable() {
			string command="DELETE FROM recurringcharge WHERE RecurringChargeNum > 0";
			Database.ExecuteNonQuery(command);
		}
	}
}
