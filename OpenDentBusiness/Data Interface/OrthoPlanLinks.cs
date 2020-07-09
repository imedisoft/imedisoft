using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class OrthoPlanLinks{
		#region Get Methods
		///<summary>Gets one OrthoPlanLink from the db.</summary>
		public static OrthoPlanLink GetOne(long orthoPlanLinkNum) {
			
			return Crud.OrthoPlanLinkCrud.SelectOne(orthoPlanLinkNum);
		}

		///<summary>Gets one OrthoPlanLink by OrthoCaseNum and OrthoPlanLinkType. Each OrthoCase should have no more than one of each
		///OrthoPlanLinkType associated to it.</summary>
		public static OrthoPlanLink GetOneForOrthoCaseByType(long orthoCaseNum,OrthoPlanLinkType linkType) {
			
			string command=$@"SELECT * FROM orthoplanlink WHERE orthoplanlink.OrthoCaseNum={POut.Long(orthoCaseNum)}
				AND orthoplanlink.LinkType={POut.Int((int)linkType)}";
			return Crud.OrthoPlanLinkCrud.SelectOne(command);
		}

		///<summary>Gets a list of all OrthoPlanLinks by OrthoPlanLinkType for a list of OrthoCaseNums.</summary>
		public static List<OrthoPlanLink> GetAllForOrthoCasesByType(List<long> listOrthoCaseNums,OrthoPlanLinkType linkType) {
			if(listOrthoCaseNums.Count<=0) {
				return new List<OrthoPlanLink>();
			}
			
			string command=$@"SELECT * FROM orthoplanlink WHERE orthoplanlink.LinkType={POut.Int((int)linkType)}
				AND orthoplanlink.OrthoCaseNum IN({string.Join(",",listOrthoCaseNums)})";
			return Crud.OrthoPlanLinkCrud.SelectMany(command);
		}
		#endregion Get Methods

		#region Modification Methods
		#region Insert
		///<summary>Inserts a OrthoPlanLink into the database. Returns the OrthoPlanLinkNum</summary>
		public static long Insert(OrthoPlanLink orthoPlanLink) {
			
			return Crud.OrthoPlanLinkCrud.Insert(orthoPlanLink);
		}
		#endregion Insert

		#region Update
		///<summary>Update only data that is different in newOrthoPlanLink.</summary>
		public static void Update(OrthoPlanLink newOrthoPlanLink,OrthoPlanLink oldOrthoPlanLink) {
			
			Crud.OrthoPlanLinkCrud.Update(newOrthoPlanLink,oldOrthoPlanLink);
		}
		#endregion Update

		#region Delete
		/////<summary>Delete an OrthoPlanLink from the database.</summary>
		//public static void Delete(long orthoPlanLinkNum) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		Meth.GetVoid(MethodBase.GetCurrentMethod(),orthoPlanLinkNum);
		//		return;
		//	}
		//	Crud.OrthoPlanLinkCrud.Delete(orthoPlanLinkNum);
		//}
		#endregion Delete
		#endregion Modification Methods

		#region Misc Methods
		#endregion Misc Methods
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		///<summary></summary>
		public static List<OrthoPlanLink> Refresh(long patNum){
			
			string command="SELECT * FROM orthoplanlink WHERE PatNum = "+POut.Long(patNum);
			return Crud.OrthoPlanLinkCrud.SelectMany(command);
		}
		///<summary></summary>
		public static void Update(OrthoPlanLink orthoPlanLink){
			
			Crud.OrthoPlanLinkCrud.Update(orthoPlanLink);
		}
		*/
	}
}