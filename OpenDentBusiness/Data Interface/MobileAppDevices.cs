using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MobileAppDevices{

		#region Get Methods

		///<summary>Gets one MobileAppDevice from the database.</summary>
		public static MobileAppDevice GetOne(long mobileAppDeviceNum) {
			
			return Crud.MobileAppDeviceCrud.SelectOne(mobileAppDeviceNum);
		}

		///<summary>Gets all MobileAppDevices from the database. If patNum is provided then filters by patNum. PatNum of 0 get unoccupied devices.</summary>
		public static List<MobileAppDevice> GetAll(long patNum=-1) {
			
			string command="SELECT * FROM mobileappdevice";
			if(patNum>-1) {
				command+=$" WHERE PatNum={POut.Long(patNum)}";
			}
			return Crud.MobileAppDeviceCrud.SelectMany(command);
		}

		public static List<MobileAppDevice> GetForUser(User user) {
			
			string command=$"SELECT * FROM mobileappdevice ";
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listClinicsForUser=Clinics.GetByUser(user);
				if(listClinicsForUser.Count==0) {
					return new List<MobileAppDevice>();
				}
				command+=$"WHERE ClinicNum in ({ string.Join(",",listClinicsForUser.Select(x => x.Id))})";
			}
			return Crud.MobileAppDeviceCrud.SelectMany(command);
		}

		#endregion Get Methods

		#region Modification Methods
		#region Update

		public static void Update(MobileAppDevice device) {
			
			Crud.MobileAppDeviceCrud.Update(device);			
			Signalods.SetInvalid(InvalidType.EClipboard);
		}

		///<summary>Keeps MobileAppDevice table current so we know which patient is on which device and for how long.</summary>
		public static void SetPatNum(long mobileAppDeviceNum,long patNum) {
			
			string command="UPDATE mobileappdevice SET PatNum="+POut.Long(patNum)+",LastCheckInActivity="+POut.DateT(DateTime.Now)
				+" WHERE MobileAppDeviceNum="+POut.Long(mobileAppDeviceNum);
			Database.ExecuteNonQuery(command);
			Signalods.SetInvalid(InvalidType.EClipboard);
		}

		///<summary>Syncs the two lists in the database.</summary>
		public static void Sync(List<MobileAppDevice> listDevicesNew,List<MobileAppDevice> listDevicesDb) {
			
			if(Crud.MobileAppDeviceCrud.Sync(listDevicesNew,listDevicesDb)) {
				Signalods.SetInvalid(InvalidType.EClipboard);
			}
		}

		#endregion Update

		#region Delete

		public static void Delete(long mobileAppDeviceNum) {
			
			WebTypes.PushNotificationUtils.CI_IsAllowedChanged(mobileAppDeviceNum,false); //deleting so always false
			Crud.MobileAppDeviceCrud.Delete(mobileAppDeviceNum);
			Signalods.SetInvalid(InvalidType.EClipboard);
		}

		#endregion Delete
		#endregion Modification Methods

		///<summary>For any device whose clinic num is not in the list passed in, sets IsAllowed to false.</summary>
		public static void UpdateIsAllowed(List<long> listClinicNums) {
			
			string command="UPDATE mobileappdevice SET IsAllowed=0";
			if(!listClinicNums.IsNullOrEmpty()) {
				command+=" WHERE ClinicNum NOT IN("+string.Join(",",listClinicNums)+")";
			}
			Database.ExecuteNonQuery(command);
		}

		///<summary>Returns true if this PatNum is currently linked to a MobileAppDevice row.</summary>
		public static bool PatientIsAlreadyUsingDevice(long patNum) {
			
			string command=$"SELECT COUNT(*) FROM mobileappdevice WHERE PatNum={POut.Long(patNum)}";
			return PIn.Long(Database.ExecuteString(command))>0;
		}

		///<summary>Returns true if this clinicNum is subscribed for eClipboard.</summary>
		public static bool IsClinicSignedUpForEClipboard(long clinicNum) {
			//No remoting role check needed.
			return Preferences.GetString(PreferenceName.EClipboardClinicsSignedUp)
				.Split(',')
				.Where(x => x!="")
				.Select(x => PIn.Long(x))
				.Any(x => x==clinicNum);
		}

		///<summary>Returns true if this clinicNum is subscribed for MobileWeb.</summary>
		public static bool IsClinicSignedUpForMobileWeb(long clinicNum) {
			//No remoting role check needed.
			return GetClinicSignedUpForMobileWeb().Any(x => x==clinicNum);
		}

		///<summary>Returns list of ClinicNum(s) which are currently subscribed for MobileWeb. 
		///Will include ClinicNum 0 if not using clinics and practice is enabled.</summary>
		public static List<long> GetClinicSignedUpForMobileWeb() {
			//No remoting role check needed.
			return Preferences.GetString(PreferenceName.MobileWebClinicsSignedUp)
				.Split(',')
				.Where(x => x!="")
				.Select(x => PIn.Long(x)).ToList();
		}
	}
}