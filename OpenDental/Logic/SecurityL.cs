using CodeBase;
using Imedisoft.Forms;
using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public class SecurityL
	{
		/// <summary>
		/// Called to change the password for Security.CurUser.
		/// Returns true if password was changed successfully.
		/// Set isForcedLogOff to force the program to log the user off if they cancel out of the Change Password window.
		/// </summary>
		public static bool ChangePassword(bool forcedLogOff)
		{
			if (Security.CurrentUser.UserNumCEMT != 0)
			{
				ODMessageBox.Show("Use the CEMT tool to change your password.", "Imedisoft",
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				return false;
			}

			using (var formUserPassword = new FormUserPassword(Security.CurrentUser.UserName, false))
			{
				formUserPassword.ShowDialog();

				if (formUserPassword.DialogResult == DialogResult.Cancel)
				{
					if (forcedLogOff)
					{
						var formOpenDental = Application.OpenForms.OfType<FormOpenDental>().ToList()[0];

						formOpenDental.LogOffNow(true);
					}
					return false;
				}

				bool isPasswordStrong = formUserPassword.PasswordIsStrong;

				try
				{
					Userods.UpdatePassword(Security.CurrentUser, formUserPassword.PasswordHash, isPasswordStrong);
				}
				catch (Exception exception)
				{
                    ODMessageBox.Show(exception.Message, "Imedisoft", 
						MessageBoxButtons.OK, MessageBoxIcon.Error);

					return false;
				}

				Security.CurrentUser.PasswordIsStrong = formUserPassword.PasswordIsStrong;
				Security.CurrentUser.PasswordHash = formUserPassword.PasswordHash;

				// TODO: Save the user?
			}

			return true;
		}
	}
}
