using OpenDentBusiness;
using System;
using System.Collections.Generic;

namespace UnitTestsCore {
	public class AdjustmentT {
		public static Adjustment MakeAdjustment(long patNum,double adjAmt,DateTime adjDate=default(DateTime),DateTime procDate=default(DateTime)
			,long procNum=0,long provNum=0,long adjType=0,long clinicNum=0,bool doInsert=true) 
		{
			Adjustment adjustment=new Adjustment();
			if(adjDate==default(DateTime)) {
				adjDate=DateTime.Today;
			}
			if(procDate==default(DateTime)) {
				procDate=DateTime.Today;
			}
			adjustment.PatNum=patNum;
			adjustment.AdjAmt=adjAmt;
			adjustment.ProcNum=procNum;
			adjustment.ProvNum=provNum;
			adjustment.AdjDate=adjDate;
			adjustment.ProcDate=procDate;
			adjustment.AdjType=adjType;
			adjustment.ClinicNum=clinicNum;
			if(doInsert) {
				Adjustments.Insert(adjustment);
			}
			return adjustment;
		}

		public static void InsertMany(List<Adjustment> listAdjustments) {
			// TODO: OpenDentBusiness.Crud.AdjustmentCrud.InsertMany(listAdjustments);
		}
	}
}
