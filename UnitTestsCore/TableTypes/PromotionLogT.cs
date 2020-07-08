using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsCore {
	public class PromotionLogT {
		public static PromotionLog CreatePromotionLog(long patNum,DateTime dateTimeSent,long emailHostingFK=0,long emailMessageNum=0,
			long promotionNum=0,PromotionLogStatus status=PromotionLogStatus.Unknown) {
			PromotionLog log=new PromotionLog();
			log.DateTimeSent=dateTimeSent;
			log.EmailHostingFK=emailHostingFK;
			log.EmailMessageNum=emailMessageNum;
			log.PatNum=patNum;
			log.PromotionNum=promotionNum;
			log.PromotionStatus=status;
			PromotionLogs.Insert(log);
			return log;
		}
	}
}
