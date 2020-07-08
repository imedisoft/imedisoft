using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class RegistrationKeyT {
		public static RegistrationKey CreateRegKey(long patNum,bool isOnlyForTesting=false) {
			RegistrationKey regKey=new RegistrationKey {
				PatNum=patNum,
				DateStarted=DateTime_.Today,
				IsOnlyForTesting=isOnlyForTesting,
			};
			RegistrationKeys.Insert(regKey);
			return regKey;
		}
	}
}
