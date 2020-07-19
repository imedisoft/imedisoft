using DataConnectionBase;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental
{
    public class UserOdPrefL
	{
		/// <summary>
		/// This should be called when the user is changed (excluding temporay logins such as job review logins).
		/// In the future, we could also call this if we detect the office theme has changed via signal preference cache refresh or if
		/// another person using the same login information changes the theme for a group of users.
		/// </summary>
		public static void SetThemeForUserIfNeeded()
		{
			// TODO: Rewrite me...

			bool isAlternateIcons = false;

			try
			{
				isAlternateIcons = PrefC.GetBool(PrefName.ColorTheme);
			}
			catch
			{
			}

			if (Security.CurUser == null)
			{
				ModuleBar.SetIcons(isAlternateIcons);

				return;
			}

			UserOdPref userOdPref = UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.Id, UserOdFkeyType.UserTheme).FirstOrDefault();

			if (!PrefC.GetBool(PrefName.ThemeSetByUser) || userOdPref == null)
			{
				ModuleBar.SetIcons(isAlternateIcons);
			}
			else if (userOdPref != null)
			{
				ModuleBar.SetIcons(SIn.Bool(userOdPref.Fkey.ToString()));
			}
		}
	}
}
