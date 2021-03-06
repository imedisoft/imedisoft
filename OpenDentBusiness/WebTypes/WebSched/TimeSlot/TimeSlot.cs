﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.WebTypes.WebSched.TimeSlot
{
	[Serializable]
	public class TimeSlot
	{
		public DateTime DateTimeStart;
		public DateTime DateTimeStop;

		/// <summary>FK to operatory.OperatoryNum</summary>
		public long OperatoryNum;

		/// <summary>FK to provider.ProvNum</summary>
		public long ProvNum;

		/// <summary>FK to definition.DefNum.  This will be a definition of type WebSchedNewPatApptTypes.</summary>
		public long DefNumApptType;

		public TimeSlot()
		{
		}

		public TimeSlot(DateTime dateTimeStart, DateTime dateTimeStop, long operatoryNum = 0, long provNum = 0, long defNumApptType = 0)
		{
			DateTimeStart = dateTimeStart;
			DateTimeStop = dateTimeStop;
			OperatoryNum = operatoryNum;
			ProvNum = provNum;
			DefNumApptType = defNumApptType;
		}
	}
}
