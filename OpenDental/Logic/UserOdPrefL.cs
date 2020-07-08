using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental{
	public class UserOdPrefL{
		///<summary>This should be called when the user is changed (excluding temporay logins such as job review logins).
		///In the future, we could also call this if we detect the office theme has changed via signal preference cache refresh or if
		///another person using the same login information changes the theme for a group of users.</summary>
		public static void SetThemeForUserIfNeeded() {
			bool isAlternateIcons=false;
			//EnumTheme themeDefault=EnumTheme.Standard;
			try {
				isAlternateIcons=PrefC.GetBool(PrefName.ColorTheme);
				//themeDefault=(EnumTheme)PrefC.GetInt(PrefName.ColorTheme);
			}
			catch {
				//try/catch in case you are trying to convert from an older version of OD and need to update the DB.
			}
			if(Security.CurUser==null) {//no current user, set to the default practice theme.
				ModuleBar.SetIcons(isAlternateIcons);
				//ODColorTheme.SetTheme(themeDefault);
				return;
			}
			UserOdPref userOdPref=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.UserTheme).FirstOrDefault();
			//user theme not allowed or hasn't been set
			if(!PrefC.GetBool(PrefName.ThemeSetByUser) || userOdPref==null) {
				ModuleBar.SetIcons(isAlternateIcons);
				//ODColorTheme.SetTheme(themeDefault);
			}
			else if(userOdPref!=null) {//user theme allowed but needs to update for user pref
				ModuleBar.SetIcons(PIn.Bool(userOdPref.Fkey.ToString()));
				//ODColorTheme.SetTheme((EnumTheme)userOdPref.Fkey);
			}
		}
	}
}
