using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsCore {
	public class ProgramPropertyT {

		///<summary>Generates a single Unit Test ProgramProperty. This method refreshes the cache.</summary>
		public static ProgramProperty CreateProgramProperty(long programNum,string propertyDesc,long clinicNum,string propertyValue="",
			string computerName="")
		{
			ProgramProperty prop=new ProgramProperty();
			prop.ProgramId=programNum;
			prop.Name=propertyDesc;
			prop.Value=propertyValue;
			prop.MachineName=computerName;
			prop.ClinicId=clinicNum;
			ProgramProperties.Insert(prop);
			ProgramProperties.RefreshCache();
			return prop;
		}

		///<summary>Deletes everything from the Unit Test ProgramProperty table.</summary>
		public static void ClearProgamPropertyTable() {
			string command="DELETE FROM programproperty WHERE ProgramPropertyNum > 0";
			Database.ExecuteNonQuery(command);
			ProgramProperties.RefreshCache();
		}

		public static void UpdateProgramProperty(ProgramName progName,string propertyDesc,string propertyValue,long clinicNum=0) {
			Program prog=Programs.GetProgram(Programs.GetProgramNum(progName));
			ProgramProperty progProp=ProgramProperties.GetFirstOrDefault(x => x.ProgramId==prog.Id && x.Name==propertyDesc 
				&& x.ClinicId==clinicNum);
			progProp.Value=propertyValue;
			ProgramProperties.Update(progProp);
		}
	}
}
