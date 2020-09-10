using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using DataConnectionBase;
using CodeBase;
using Imedisoft.Data;

namespace OpenDentBusiness
{
	public partial class AlertItems
	{
		/// <summary>
		/// Inserts a generic alert where description will show in the menu item and itemValue will be shown within a MsgBoxCopyPaste.
		/// Set itemValue to more specific reason for the alert.  E.g. exception text details as to help the techs give better support.
		/// </summary>
		public static void CreateGenericAlert(string description, string details)
		{
            Insert(new AlertItem
			{
				Type = AlertType.Generic,
				Actions = ActionType.MarkAsRead | ActionType.Delete | ActionType.ShowItemValue,
				Description = description,
				Severity = SeverityType.Low,
				Details = details
			});
		}

		/// <summary>
		/// Checks whether the heartbeat of the OpenDental service occurred within the last 6 
		/// minutes. If not, an alert indicating the service is down will be generated.
		/// </summary>
		public static void CheckODServiceHeartbeat()
		{
			if (!IsODServiceRunning())
			{
				// If the service is not running, create an alert (only if there isn't already a alert).
				var alerts = RefreshForType(AlertType.OpenDentalServiceDown);

				if (!alerts.Any())
				{
                    Insert(new AlertItem
					{
						Actions = ActionType.MarkAsRead,
						ClinicId = null,
						Description = "No instance of Open Dental Service is running.",
						Type = AlertType.OpenDentalServiceDown,
						Severity = SeverityType.Medium
					});
				}
			}
		}

		/// <summary>
		/// Checks whether the OpenDental Service is running by checking the 
		/// <b>OpenDentalServiceHeartbeat</b> preference. If the preference was updated less then 
		/// 6 minutes ago the service is most likely running.
		/// </summary>
		/// <returns>True if the service is running; otherwise, false.</returns>
		public static bool IsODServiceRunning()
		{
			var dateTime = Preferences.GetDateTimeOrNull(PreferenceName.OpenDentalServiceHeartbeat);

			if (!dateTime.HasValue || dateTime.Value.AddMinutes(6) < DateTime.UtcNow)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// This method grabs all unread webmails, and creates/modifies/deletes alerts for the providers and linked users the webmails are addressed to.
		/// </summary>
		public static void CreateAlertsForNewWebmail()
		{
			var unreadMessageCounts = EmailMessages.GetProvUnreadWebMailCount();

			Logger.LogVerbose(
				"Collected Webmails for the following providers (ProvNum: # Webmails): " + 
				string.Join(", ", unreadMessageCounts.Select(x => x.Key + ":" + x.Value)));

			//This list contains every single WebMailRecieved alert and is synced with listAlerts later.
			var oldAlerts = RefreshForType(AlertType.WebMailRecieved).ToList();

			Logger.LogVerbose(
				"Fetched current alerts for users: " + 
				string.Join(", ", oldAlerts.Where(x => x.UserId.HasValue).Select(x => x.UserId.Value)));

			var changedAlertItemIds = new List<long>();

			// Go through each provider value, and create/update alerts for each patnum under that provider.
			// There will only be a value if they have atleast 1 unread webmail.
			foreach (KeyValuePair<long, long> kvp in unreadMessageCounts)
			{
				var users = Providers.GetAttachedUsers(kvp.Key);

				// Go through each usernum and create/update their alert item.
				foreach (long userId in users.Select(x => x.Id))
				{
					var alertItem = oldAlerts.FirstOrDefault(x => x.UserId == userId);

					// If an alert doesn't exist for the user, we'll create it.
					if (alertItem == null)
					{
                        Insert(new AlertItem
						{
							Type = AlertType.WebMailRecieved,
							FormToOpen = FormType.FormEmailInbox,
							Actions = ActionType.MarkAsRead | ActionType.OpenForm,
							Severity = SeverityType.Normal,
							ClinicId = null,
							UserId = userId,
							Description = kvp.Value.ToString()
						});

						Logger.LogVerbose("Created webmail alert for user " + userId);
					}
					else
					{
						var newDescription = kvp.Value.ToString();
						if (alertItem.Description != newDescription)
                        {
							alertItem.Description = newDescription;

							Update(alertItem);

							changedAlertItemIds.Add(alertItem.Id);
						}

						Logger.LogVerbose("Modified webmail alert for user " + userId);
					}
				}
			}

			AlertReads.DeleteForAlertItems(changedAlertItemIds);
		}

		/// <summary>
		/// Returns a list of lists which contains unique alert items. 
		/// Each inner list is a group of alerts that are duplicates of each other.
		/// </summary>
		public static List<List<AlertItem>> GetUniqueAlerts(long userNumCur, long clinicNumCur)
		{
			List<AlertSub> listUserAlertSubsAll = AlertSubs.GetAllForUser(userNumCur);
			bool isAllClinics = listUserAlertSubsAll.Any(x => x.ClinicId == -1);
			List<long> listAlertCatNums = new List<long>();
			if (isAllClinics)
			{//User subscribed to all clinics.
				listAlertCatNums = listUserAlertSubsAll.Select(x => x.AlertCategoryId).Distinct().ToList();
			}
			else
			{
				//List of AlertSubs for current clinic and user combo.
				List<AlertSub> listUserAlertSubs = listUserAlertSubsAll.FindAll(x => x.ClinicId == clinicNumCur);
				listAlertCatNums = listUserAlertSubs.Select(y => y.AlertCategoryId).ToList();
			}
			//AlertTypes current user is subscribed to.
			List<AlertType> listUserAlertLinks = AlertCategoryLinks.GetWhere(x => listAlertCatNums.Contains(x.AlertCategoryId))
				.Select(x => x.AlertType).ToList();
			List<long> listAllAlertCatNums = listUserAlertSubsAll.Select(y => y.AlertCategoryId).ToList();
			//AlertTypes current user is subscribed to for AlertItems which are not clinic specific.
			List<AlertType> listAllUserAlertLinks = AlertCategoryLinks.GetWhere(x => listAllAlertCatNums.Contains(x.AlertCategoryId))
				.Select(x => x.AlertType).ToList();
			//Each inner list is a group of alerts that are duplicates of each other.
			List<List<AlertItem>> listUniqueAlerts = new List<List<AlertItem>>();
			RefreshForClinicAndTypes(clinicNumCur, listUserAlertLinks)//Get alert items for the current clinic
				.Union(RefreshForClinicAndTypes(-1, listAllUserAlertLinks))//Get alert items that are for all clinics
				.DistinctBy(x => x.Id)
				.ForEach(x =>
				{
					foreach (List<AlertItem> listDuplicates in listUniqueAlerts)
					{
						if (AreDuplicates(listDuplicates.First(), x))
						{
							listDuplicates.Add(x);
							return;
						}
					}
					listUniqueAlerts.Add(new List<AlertItem> { x });
				}
			);
			return listUniqueAlerts;
		}

		/// <summary>
		/// Returns true if the two alerts match all fields other than AlertItemNum.
		/// </summary>
		public static bool AreDuplicates(AlertItem alert1, AlertItem alert2)
		{
			if (alert1 == null || alert2 == null)
			{
				return false;
			}

			return alert1.Actions == alert2.Actions
				&& alert1.ClinicId == alert2.ClinicId
				&& alert1.Description == alert2.Description
				&& alert1.ObjectId == alert2.ObjectId
				&& alert1.FormToOpen == alert2.FormToOpen
				&& alert1.Details == alert2.Details
				&& alert1.Severity == alert2.Severity
				&& alert1.Type == alert2.Type
				&& alert1.UserId == alert2.UserId;
		}

		/// <summary>
		/// Returns a list of AlertItems for the given clinicNum. 
		/// Doesn't include alerts that are assigned to other users.
		/// </summary>
		public static IEnumerable<AlertItem> RefreshForClinicAndTypes(long clinicId, List<AlertType> alertTypes = null)
		{
			if (alertTypes == null || alertTypes.Count == 0)
			{
				return new List<AlertItem>();
			}

			long providerId = 0;
			if (Security.CurrentUser != null && Userods.IsUserCpoe(Security.CurrentUser))
			{
				providerId = Security.CurrentUser.ProviderId.Value;
			}

			long userId = 0;
			if (Security.CurrentUser != null)
			{
				userId = Security.CurrentUser.Id;
			}

			return SelectMany(
				"SELECT * FROM `alert_items` " +
				"WHERE `type` IN (" + string.Join(",", alertTypes.Cast<int>()) + ") " +
				"AND (`user_id` IS NULL OR `user_id` = " + userId + ") " +
				"AND (CASE `type` WHEN " + (int)AlertType.RadiologyProcedures + " THEN `object_id` = " + providerId + " " +
				"ELSE `clinic_id` = " + clinicId + " OR `clinic_id` IS NULL END)");
		}

		/// <summary>
		/// Returns a list of AlertItems for the given alertType.
		/// </summary>
		public static IEnumerable<AlertItem> RefreshForType(AlertType alertType)
		{
			return SelectMany("SELECT * FROM `alert_items` WHERE `type` = " + (int)alertType);
		}

		/// <summary>
		/// If null listFKeys is provided then all rows of the given alertType will be deleted. 
		/// Otherwise only rows which match listFKeys entries.
		/// </summary>
		public static void DeleteFor(AlertType alertType, List<long> objectIds = null)
		{
			var alerts = RefreshForType(alertType);
			if (objectIds != null)
			{
				alerts = alerts.Where(x => x.ObjectId.HasValue && objectIds.Contains(x.ObjectId.Value));
			}

			foreach (var alert in alerts)
			{
				Delete(alert.Id);
			}
		}

		/// <summary>
		/// Also deletes any AlertRead objects for these AlertItems.
		/// </summary>
		public static void Delete(List<long> alertItemIds)
		{
			if (alertItemIds == null || alertItemIds.Count == 0)
			{
				return;
			}

			Database.ExecuteNonQuery(
				"DELETE FROM `alert_items` " +
				"WHERE `id` IN (" + string.Join(", ", alertItemIds) + ")");
		}
	}
}
