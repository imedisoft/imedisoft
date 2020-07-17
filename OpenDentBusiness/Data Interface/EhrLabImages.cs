using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EhrLabImages{
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


		///<summary></summary>
		public static List<EhrLabImage> Refresh(long ehrLabNum) {
			
			string command="SELECT * FROM ehrlabimage WHERE EhrLabNum = "+POut.Long(ehrLabNum)+" AND DocNum > 0";
			return Crud.EhrLabImageCrud.SelectMany(command);
		}

		///<summary>Returns true if a row containing the given EhrLabNum and DocNum==-1 is found.  Otherwise returns false.</summary>
		public static bool IsWaitingForImages(long ehrLabNum) {
			
			string command="SELECT * FROM ehrlabimage WHERE EhrLabNum = "+POut.Long(ehrLabNum)+" AND DocNum = -1";
			return Crud.EhrLabImageCrud.SelectOne(command)!=null;
		}

		///<summary>EhrLab first EhrLab which this docNum is attached to. Or returns null if docNum is not attached to any EhrLabs.</summary>
		public static EhrLab GetFirstLabForDocNum(long docNum) {
			
			//Get first EhrLabImage that has this docNum attached.
			string command="SELECT * FROM ehrlabimage WHERE DocNum = "+POut.Long(docNum)+" LIMIT 1";
			EhrLabImage labImage=Crud.EhrLabImageCrud.SelectOne(command);
			if(labImage==null) { //Not found so return
				return null;
			}
			//Get the EhrLab which this labImage is attached to
			command="SELECT * FROM ehrlab WHERE EhrLabNum = "+POut.Long(labImage.EhrLabNum);
			return Crud.EhrLabCrud.SelectOne(labImage.EhrLabNum);
		}

		///<summary>Create an entry per each docNum in the list. If setting isWaiting flag to true then create a row containing the given EhrLabNum and DocNum==-1.  Otherwise omit such a row.</summary>
		public static void InsertAllForLabNum(long ehrLabNum,bool isWaiting,List<long> docNums) {
			
			//Delete existing rows for this EhrLabNum.
			DeleteForLab(ehrLabNum);
			//Create the waiting flag if necessary
			if(isWaiting) {
				EhrLabImage labImage=new EhrLabImage();
				labImage.EhrLabNum=ehrLabNum;
				labImage.DocNum=-1;
				Insert(labImage);
			}
			//Create the rest of the links
			for(int i=0;i<docNums.Count;i++) {
				EhrLabImage labImage=new EhrLabImage();
				labImage.EhrLabNum=ehrLabNum;
				labImage.DocNum=docNums[i];
				Insert(labImage);
			}
		}

		///<summary>Delete all rows for a given EhrLabNum.</summary>
		public static void DeleteForLab(long ehrLabNum) {
			
			//Delete existing rows for this EhrLabNum.
			string cmd="DELETE FROM ehrlabimage WHERE EhrLabNum = "+POut.Long(ehrLabNum);
			Database.ExecuteNonQuery(cmd);			
		}

		///<summary>From local list. Returns true if EhrLabImage is found. Otherwise returns false.</summary>
		public static bool GetDocNumExistsInList(long ehrLabNum,long docNum,List<EhrLabImage> listImages) {
			//No need to check RemotingRole; no call to db.
			return GetFromList(ehrLabNum,docNum,listImages)!=null;
		}

		///<summary>From local list. Returns EhrLabImage if found. Returns null if not found.</summary>
		public static EhrLabImage GetFromList(long ehrLabNum,long docNum,List<EhrLabImage> listImages) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<listImages.Count;i++) {
				if(listImages[i].DocNum==docNum && listImages[i].EhrLabNum==ehrLabNum) {
					return listImages[i];
				}
			}
			return null;
		}

		///<summary></summary>
		public static long Insert(EhrLabImage ehrLabImage) {
			
			return Crud.EhrLabImageCrud.Insert(ehrLabImage);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		 * ///<summary>Gets one EhrLabImage from the db.</summary>
		public static EhrLabImage GetOne(long ehrLabImageNum){
			
			return Crud.EhrLabImageCrud.SelectOne(ehrLabImageNum);
		}



		///<summary></summary>
		public static void Update(EhrLabImage ehrLabImage){
			
			Crud.EhrLabImageCrud.Update(ehrLabImage);
		}


		*/



	}
}