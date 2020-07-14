using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralLogOn : FormBase
	{
		private List<Userod> users;

		public FormCentralLogOn() => InitializeComponent();

		private void FormCentralLogOn_Load(object sender, EventArgs e)
		{
			usernameTextBox.Select();

			Userods.RefreshCache();

			users = Userods.GetUsers(true);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var username = usernameTextBox.Text.Trim();
			var user = users.Where(u => u.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
			
			if (user == null)
            {
				ShowError("Invalid username or password.");

				return;
            }

			try
			{
				Userods.CheckUserAndPassword(user.UserName, passwordTextBox.Text, false);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			Security.CurUser = user.Copy();
			Security.PasswordTyped = passwordTextBox.Text;

			DialogResult = DialogResult.OK;
		}
	}
}
