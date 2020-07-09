using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrAptObses{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		///<summary></summary>
		public static List<EhrAptObs> Refresh(long aptNum) {
			
			string command="SELECT * FROM ehraptobs WHERE AptNum = "+POut.Long(aptNum);
			return Crud.EhrAptObsCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(EhrAptObs ehrAptObs) {
			
			return Crud.EhrAptObsCrud.Insert(ehrAptObs);
		}

		///<summary></summary>
		public static void Update(EhrAptObs ehrAptObs) {
			
			Crud.EhrAptObsCrud.Update(ehrAptObs);
		}

		///<summary></summary>
		public static void Delete(long ehrAptObsNum) {
			
			string command= "DELETE FROM ehraptobs WHERE EhrAptObsNum = "+POut.Long(ehrAptObsNum);
			Db.NonQ(command);
		}
		
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one EhrAptObs from the db.</summary>
		public static EhrAptObs GetOne(long ehrAptObsNum){
			
			return Crud.EhrAptObsCrud.SelectOne(ehrAptObsNum);
		}

		
		*/



	}
}