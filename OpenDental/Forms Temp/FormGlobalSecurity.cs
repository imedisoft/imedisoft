using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.DirectoryServices;
using System.Linq;
using CodeBase;
using Imedisoft.UI;
using Imedisoft.Data;
using Imedisoft.Data.Models;

namespace OpenDental
{
	public partial class FormGlobalSecurity : ODForm
	{
		///<summary>The ObjectGuid for the domain path entered.</summary>
		private string _domainObjectGuid;

		public FormGlobalSecurity()
		{
			InitializeComponent();

		}

		private void FormGlobalSecurity_Load(object sender, EventArgs e)
		{
			textLogOffAfterMinutes.Text = PrefC.GetInt(PreferenceName.SecurityLogOffAfterMinutes).ToString();
			checkAllowLogoffOverride.Checked = Preferences.GetBool(PreferenceName.SecurityLogOffAllowUserOverride);
			checkPasswordsMustBeStrong.Checked = Preferences.GetBool(PreferenceName.PasswordsMustBeStrong);
			checkPasswordsStrongIncludeSpecial.Checked = Preferences.GetBool(PreferenceName.PasswordsStrongIncludeSpecial);
			checkPasswordForceWeakToStrong.Checked = Preferences.GetBool(PreferenceName.PasswordsWeakChangeToStrong);
			checkTimecardSecurityEnabled.Checked = Preferences.GetBool(PreferenceName.TimecardSecurityEnabled);
			checkCannotEditPastPayPeriods.Checked = Preferences.GetBool(PreferenceName.TimecardUsersCantEditPastPayPeriods);
			checkCannotEditOwn.Checked = Preferences.GetBool(PreferenceName.TimecardUsersDontEditOwnCard);
			checkCannotEditOwn.Enabled = checkTimecardSecurityEnabled.Checked;
			checkCannotEditPastPayPeriods.Enabled = checkTimecardSecurityEnabled.Checked;
			checkDomainLoginEnabled.Checked = Preferences.GetBool(PreferenceName.DomainLoginEnabled);
			textDomainLoginPath.ReadOnly = !checkDomainLoginEnabled.Checked;
			textDomainLoginPath.Text = Preferences.GetString(PreferenceName.DomainLoginPath);
			checkLogOffWindows.Checked = Preferences.GetBool(PreferenceName.SecurityLogOffWithWindows);
			checkUserNameManualEntry.Checked = Preferences.GetBool(PreferenceName.UserNameManualEntry);
			checkMaintainPatient.Checked = Preferences.GetBool(PreferenceName.PatientMaintainedOnUserChange);
			if (!PrefC.HasClinicsEnabled)
			{
				//This pref only matters when clinics are turned on. When clinics are off it behaves the same as if the pref were on. 
				checkMaintainPatient.Visible = false;
			}
			if (PrefC.GetDate(PreferenceName.BackupReminderLastDateRun).ToShortDateString() == DateTime.MaxValue.AddMonths(-1).ToShortDateString())
			{
				checkDisableBackupReminder.Checked = true;
			}
			if (PrefC.GetInt(PreferenceName.SecurityLockDays) > 0)
			{
				textDaysLock.Text = PrefC.GetInt(PreferenceName.SecurityLockDays).ToString();
			}
			if (PrefC.GetDate(PreferenceName.SecurityLockDate).Year > 1880)
			{
				textDateLock.Text = PrefC.GetDate(PreferenceName.SecurityLockDate).ToShortDateString();
			}
			if (Preferences.GetBool(PreferenceName.CentralManagerSecurityLock))
			{
				butChange.Enabled = false;
				labelGlobalDateLockDisabled.Visible = true;
			}
			List<UserGroup> listGroupsNotAdmin = UserGroups.GetAll().FindAll(x => !GroupPermissions.HasPermission(x.Id, Permissions.SecurityAdmin, 0));
			for (int i = 0; i < listGroupsNotAdmin.Count; i++)
			{
				comboGroups.Items.Add(listGroupsNotAdmin[i].Description, listGroupsNotAdmin[i]);
				if (Preferences.GetLong(PreferenceName.DefaultUserGroup) == listGroupsNotAdmin[i].Id)
				{
					comboGroups.SelectedIndex = i;
				}
			}
		}

		private void checkTimecardSecurityEnabled_Click(object sender, EventArgs e)
		{
			if (!checkTimecardSecurityEnabled.Checked)
			{
				checkCannotEditOwn.Checked = false;
				checkCannotEditPastPayPeriods.Checked = false;
			}
			checkCannotEditOwn.Enabled = checkTimecardSecurityEnabled.Checked;//can't edit timecards at all
			checkCannotEditPastPayPeriods.Enabled = checkTimecardSecurityEnabled.Checked;//can only edit own timecard for current pay period
		}

		private void checkCanEditOwnCur_Click(object sender, EventArgs e)
		{
			if (checkCannotEditPastPayPeriods.Checked)
			{//one or other can be checked but not both, both can be unchecked
				checkCannotEditOwn.Checked = false;
			}
		}

		private void checkCannotEditOwn_Click(object sender, EventArgs e)
		{
			if (checkCannotEditOwn.Checked)
			{//one or other can be checked but not both, both can be unchecked
				checkCannotEditPastPayPeriods.Checked = false;
			}
		}

		private void checkDomainLoginEnabled_CheckedChanged(object sender, EventArgs e)
		{
			textDomainLoginPath.ReadOnly = !checkDomainLoginEnabled.Checked;
		}

		private void checkDomainLoginEnabled_MouseUp(object sender, MouseEventArgs e)
		{
			if (checkDomainLoginEnabled.Checked && string.IsNullOrWhiteSpace(textDomainLoginPath.Text))
			{
				if (MsgBox.Show(MsgBoxButtons.YesNo, "Would you like to use your current domain as the domain login path?"))
				{
					try
					{
						DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
						string defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();
						textDomainLoginPath.Text = "LDAP://" + defaultNamingContext;
						DirectoryEntry testEntry = new DirectoryEntry(textDomainLoginPath.Text);
						_domainObjectGuid = testEntry.Guid.ToString();
					}
					catch (Exception ex)
					{
						FriendlyException.Show("Unable to bind to the current domain.", ex);
					}
				}
			}
		}

		///<summary>Validation for the domain login path provided. 
		///Accepted formats are those listed here: https://msdn.microsoft.com/en-us/library/aa746384(v=vs.85).aspx, excluding plain "LDAP:"
		///Does not check if there are users on the domain object, only that the domain object exists and can be searched.</summary>
		private void textDomainLoginPath_Leave(object sender, EventArgs e)
		{
			if (checkDomainLoginEnabled.Checked)
			{
				if (string.IsNullOrWhiteSpace(textDomainLoginPath.Text))
				{
					MessageBox.Show("Warning. Domain Login is enabled, but no path has been entered. If you do not provide a domain path,"
						+ "you will not be able to assign domain logins to users.");
					_domainObjectGuid = "";
				}
				else
				{
					try
					{
						DirectoryEntry testEntry = new DirectoryEntry(textDomainLoginPath.Text);
						DirectorySearcher search = new DirectorySearcher(testEntry);
						SearchResultCollection testResults = search.FindAll(); //Just do a generic search to verify the object might have users on it
						_domainObjectGuid = testEntry.Guid.ToString();
					}
					catch (Exception ex)
					{
						FriendlyException.Show("An error occurred while attempting to access the provided Domain Login Path.", ex);
					}
				}
			}
		}

		private void checkPasswordsMustBeStrong_Click(object sender, EventArgs e)
		{
			if (!checkPasswordsMustBeStrong.Checked)
			{//unchecking the box
				if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Warning.  If this box is unchecked, the strong password flag on all users will be reset.  "
					+ "If strong passwords are again turned on later, then each user will have to edit their password in order to cause the strong password flag to be set again."))
				{
					checkPasswordsMustBeStrong.Checked = true;//recheck it.
					return;
				}
			}
		}

		private void checkDisableBackupReminder_Click(object sender, EventArgs e)
		{
			InputBox inputbox = new InputBox("Please enter password");
			inputbox.setTitle("Change Backup Reminder Settings");
			inputbox.ShowDialog();
			if (inputbox.DialogResult != DialogResult.OK)
			{
				checkDisableBackupReminder.Checked = !checkDisableBackupReminder.Checked;
				return;
			}
			if (inputbox.textResult.Text != "abracadabra")
			{
				checkDisableBackupReminder.Checked = !checkDisableBackupReminder.Checked;
				MessageBox.Show("Wrong password");
				return;
			}
		}

		private void butChange_Click(object sender, EventArgs e)
		{
			FormSecurityLock FormS = new FormSecurityLock();
			FormS.ShowDialog();//prefs are set invalid within that form if needed.
			if (PrefC.GetInt(PreferenceName.SecurityLockDays) > 0)
			{
				textDaysLock.Text = PrefC.GetInt(PreferenceName.SecurityLockDays).ToString();
			}
			else
			{
				textDaysLock.Text = "";
			}
			if (PrefC.GetDate(PreferenceName.SecurityLockDate).Year > 1880)
			{
				textDateLock.Text = PrefC.GetDate(PreferenceName.SecurityLockDate).ToShortDateString();
			}
			else
			{
				textDateLock.Text = "";
			}
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (textLogOffAfterMinutes.Text != "")
			{
				try
				{
					int logOffMinutes = Int32.Parse(textLogOffAfterMinutes.Text);
					if (logOffMinutes < 0)
					{//Automatic log off must be a positive numerical value.
						throw new Exception();
					}
				}
				catch
				{
					MessageBox.Show("Log off after minutes is invalid.");
					return;
				}
			}
			DataValid.SetInvalid(InvalidType.Security);
			bool invalidatePrefs = false;
			if ( //Prefs.UpdateBool(PrefName.PasswordsMustBeStrong,checkPasswordsMustBeStrong.Checked) //handled when box clicked.
				Preferences.Set(PreferenceName.TimecardSecurityEnabled, checkTimecardSecurityEnabled.Checked)
				| Preferences.Set(PreferenceName.TimecardUsersCantEditPastPayPeriods, checkCannotEditPastPayPeriods.Checked)
				| Preferences.Set(PreferenceName.TimecardUsersDontEditOwnCard, checkCannotEditOwn.Checked)
				| Preferences.Set(PreferenceName.SecurityLogOffWithWindows, checkLogOffWindows.Checked)
				| Preferences.Set(PreferenceName.UserNameManualEntry, checkUserNameManualEntry.Checked)
				| Preferences.Set(PreferenceName.PasswordsStrongIncludeSpecial, checkPasswordsStrongIncludeSpecial.Checked)
				| Preferences.Set(PreferenceName.PasswordsWeakChangeToStrong, checkPasswordForceWeakToStrong.Checked)
				| Preferences.Set(PreferenceName.SecurityLogOffAfterMinutes, PIn.Int(textLogOffAfterMinutes.Text))
				| Preferences.Set(PreferenceName.DomainLoginPath, PIn.String(textDomainLoginPath.Text))
				| Preferences.Set(PreferenceName.DomainLoginPath, textDomainLoginPath.Text)
				| Preferences.Set(PreferenceName.DomainLoginPath, textDomainLoginPath.Text)
				| Preferences.Set(PreferenceName.DomainLoginEnabled, checkDomainLoginEnabled.Checked)
				| (_domainObjectGuid != null && Preferences.Set(PreferenceName.DomainObjectGuid, _domainObjectGuid))
				| Preferences.Set(PreferenceName.DefaultUserGroup, comboGroups.SelectedIndex == -1 ? 0 : comboGroups.GetSelected<UserGroup>().Id)
				| Preferences.Set(PreferenceName.SecurityLogOffAllowUserOverride, checkAllowLogoffOverride.Checked)
				| Preferences.Set(PreferenceName.PatientMaintainedOnUserChange, checkMaintainPatient.Checked)
				)
			{
				invalidatePrefs = true;
			}
			//if PasswordsMustBeStrong was unchecked, then reset the strong password flags.
			if (Preferences.Set(PreferenceName.PasswordsMustBeStrong, checkPasswordsMustBeStrong.Checked) && !checkPasswordsMustBeStrong.Checked)
			{
				invalidatePrefs = true;
				Users.ResetStrongPasswordFlags();
			}
			if (checkDisableBackupReminder.Checked)
			{
				invalidatePrefs |= Preferences.Set(PreferenceName.BackupReminderLastDateRun, DateTime.MaxValue.AddMonths(-1)); //if MaxValue, gives error on startup.
			}
			else
			{
				invalidatePrefs |= Preferences.Set(PreferenceName.BackupReminderLastDateRun, DateTimeOD.Today);
			}
			if (invalidatePrefs)
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}