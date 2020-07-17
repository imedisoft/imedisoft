using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsCore {
	public class OrthoProcLinkT {

		///<summary>Deletes everything from the orthoproclink table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearTable() {
			Database.ExecuteNonQuery("DELETE FROM orthoproclink WHERE OrthoProcLinkNum > 0");
		}

	}
}
