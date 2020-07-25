using System;

namespace OpenDental
{
    public class UserOdPrefL
	{
		/// <summary>
		/// This should be called when the user is changed (excluding temporay logins such as job review logins).
		/// In the future, we could also call this if we detect the office theme has changed via signal preference cache refresh or if
		/// another person using the same login information changes the theme for a group of users.
		/// </summary>
		[Obsolete] public static void SetThemeForUserIfNeeded()
		{
			ModuleBar.SetIcons(true);

			return;
		}
	}
}
