using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MapAreas{
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

		///<summary>Pass in a MapAreaContainerNum to limit the list to a single room.  Otherwise all cubicles from every map will be returned.</summary>
		public static List<MapArea> Refresh(long mapAreaContainerNum=0) {
			
			string command="SELECT * FROM maparea";
			if(mapAreaContainerNum>0) {
				command+=$" WHERE MapAreaContainerNum={POut.Long(mapAreaContainerNum)}";
			}
			return Crud.MapAreaCrud.SelectMany(command);
		}
		/*		
		///<summary>Gets one MapArea from the db.</summary>
		public static MapArea GetOne(long mapAreaNum){
			
			return Crud.MapAreaCrud.SelectOne(mapAreaNum);
		}
		*/
		///<summary></summary>
		public static long Insert(MapArea mapArea){
			
			return Crud.MapAreaCrud.Insert(mapArea);
		}

		///<summary></summary>
		public static void Update(MapArea mapArea){
			
			Crud.MapAreaCrud.Update(mapArea);
		}

		///<summary></summary>
		public static void Delete(long mapAreaNum) {
			
			string command= "DELETE FROM maparea WHERE MapAreaNum = "+POut.Long(mapAreaNum);
			Db.NonQ(command);
		}



	}
}