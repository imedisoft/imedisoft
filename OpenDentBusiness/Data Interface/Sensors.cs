using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness{
	class Sensors{ 
		///<summary></summary>
		public static List<Sensor> GetList() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Sensor>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM sensor";
				//+" ORDER BY DateReconcile";
			//return Crud.SensorCrud.SelectMany(command);
			return null;
		}
	}
}
