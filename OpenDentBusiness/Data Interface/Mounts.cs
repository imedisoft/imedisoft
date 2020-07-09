using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using OpenDentBusiness;

namespace OpenDentBusiness {
	public class Mounts {
		public static long Insert(Mount mount){
			
			return Crud.MountCrud.Insert(mount);
		}

		public static void Update(Mount mount){
			
			Crud.MountCrud.Update(mount);
		}


		///<summary>Should already have checked that no images are attached.</summary>
		public static void Delete(Mount mount){
			
			string command="DELETE FROM mount WHERE MountNum='"+POut.Long(mount.MountNum)+"'";
			Db.NonQ(command);
			command="DELETE FROM mountitem WHERE MountItemNum='"+POut.Long(mount.MountNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Returns a single mount object corresponding to the given mount number key.</summary>
		public static Mount GetByNum(long mountNum) {
			
			Mount mount= Crud.MountCrud.SelectOne(mountNum);
			if(mount==null){
				return new Mount();
			}
			return mount;
		}

		///<summary>MountItems are included.  Everything has been inserted into the database.</summary>
		public static Mount CreateMountFromDef(MountDef mountDef, long patNum, long docCategory){
			//No need to check RemotingRole; no call to db.
			Mount mount=new Mount();
			mount.PatNum=patNum;
			mount.DocCategory=docCategory;
			mount.DateCreated=DateTime.Now;
			mount.Description=mountDef.Description;
			mount.Note="";
			mount.Width=mountDef.Width;
			mount.Height=mountDef.Height;
			mount.ColorBack=mountDef.ColorBack;
			mount.MountNum=Insert(mount);
			mount.ListMountItems=new List<MountItem>();
			List<MountItemDef> listMountItemDefs=MountItemDefs.GetForMountDef(mountDef.MountDefNum);
			for(int i=0;i<listMountItemDefs.Count;i++){
				MountItem mountItem=new MountItem();
				mountItem.MountNum=mount.MountNum;
				mountItem.Xpos=listMountItemDefs[i].Xpos;
				mountItem.Ypos=listMountItemDefs[i].Ypos;
				mountItem.ItemOrder=listMountItemDefs[i].ItemOrder;
				mountItem.Width=listMountItemDefs[i].Width;
				mountItem.Height=listMountItemDefs[i].Height;
				mountItem.MountItemNum=MountItems.Insert(mountItem);
				mount.ListMountItems.Add(mountItem);
			}
			return mount;
		}

	}
}