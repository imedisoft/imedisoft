using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.Services.Models
{
	public enum FeatureRequestStatus
	{
		New,
		NeedsClarification,
		Redundant,
		TooBroad,
		NotARequest,
		AlreadyDone,
		Obsolete,
		Approved,
		InProgress,
		Complete
	}
}
