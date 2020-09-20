using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Imedisoft.Claims
{
	public abstract class ClaimProgramBridge : ClaimBridge
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClaimProgramBridge"/> class.
		/// </summary>
		/// <param name="name">The name of the bridge.</param>
		public ClaimProgramBridge(string name) : base(name)
		{
		}

		protected override bool OnSend(Clearinghouse clearingHouse, long batchNumber, List<ClaimSendQueueItem> queueItems, EnumClaimMedType medType)
		{
			try
			{
				Process.Start(clearingHouse.ClientProgram);
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;

				return false;
			}

			return true;
		}
	}
}
