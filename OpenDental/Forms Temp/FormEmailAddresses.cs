using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental
{
	public partial class FormEmailAddresses : ODForm
	{
		public bool IsSelectionMode;
		public long EmailAddressNum;
		///<summary>If true, a signal for invalid Email cache will be sent out upon closing.</summary>
		public bool IsChanged;
		private List<EmailAddress> _listEmailAddresses;

		public FormEmailAddresses()
		{
			InitializeComponent();

		}

		private void FormEmailAddresses_Load(object sender, EventArgs e)
		{
			checkEmailDisclaimer.Checked = Preferences.GetBool(PreferenceName.EmailDisclaimerIsOn);
			if (IsSelectionMode)
			{
				labelInboxCheckInterval.Visible = false;
				textInboxCheckInterval.Visible = false;
				labelInboxCheckUnits.Visible = false;
				groupEmailPrefs.Visible = false;
				butAdd.Visible = false;
				checkEmailDisclaimer.Visible = false;
			}
			else
			{
				textInboxCheckInterval.Text = PrefC.GetInt(PreferenceName.EmailInboxCheckInterval).ToString();//Calls PIn() internally.
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (IsSelectionMode)
			{
				EmailAddressNum = _listEmailAddresses[gridMain.GetSelectedIndex()].Id;
				DialogResult = DialogResult.OK;
			}
			else
			{
				FormEmailAddressEdit FormEAE = new FormEmailAddressEdit(_listEmailAddresses[e.Row], true);
				FormEAE.ShowDialog();
				if (FormEAE.DialogResult == DialogResult.OK)
				{
					IsChanged = true;
					FillGrid();
				}
			}
		}

		private void FillGrid()
		{
			EmailAddresses.RefreshCache();
			_listEmailAddresses = EmailAddresses.GetDeepCopy();
			//Add user specific email addresses to the list
			List<User> listUsers = new List<User>();
			if (Security.IsAuthorized(Permissions.SecurityAdmin, true) && !IsSelectionMode)
			{
				listUsers.AddRange(Users.GetUsers());//If authorized, get all non-hidden users.
			}
			else
			{
				listUsers.Add(Security.CurrentUser);//Otherwise, just this user.
			}
			foreach (User user in listUsers)
			{
				EmailAddress userAddress = EmailAddresses.GetForUser(user.Id);
				if (userAddress != null)
				{
					_listEmailAddresses.Insert(0, userAddress);
				}
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col;
			col = new GridColumn("User Name", 240);
			gridMain.Columns.Add(col);
			col = new GridColumn("Sender Address", 270);
			gridMain.Columns.Add(col);
			col = new GridColumn("User", 135);
			gridMain.Columns.Add(col);
			col = new GridColumn("Default", 50, HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col = new GridColumn("Notify", 50, HorizontalAlignment.Center) { IsWidthDynamic = true };
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			foreach (EmailAddress emailAddress in _listEmailAddresses)
			{
				row = new GridRow();
				row.Cells.Add(emailAddress.SmtpUsername);
				row.Cells.Add(emailAddress.SenderAddress);
				row.Cells.Add(Users.GetUserName(emailAddress.UserId??0));
				row.Cells.Add((emailAddress.Id == Preferences.GetLong(PreferenceName.EmailDefaultAddressNum)) ? "X" : "");
				row.Cells.Add((emailAddress.Id == Preferences.GetLong(PreferenceName.EmailNotifyAddressNum)) ? "X" : "");
				row.Tag = emailAddress;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butSetDefault_Click(object sender, EventArgs e)
		{
			if (gridMain.GetSelectedIndex() == -1)
			{
				MessageBox.Show("Please select a row first.");
				return;
			}
			if (gridMain.SelectedTag<EmailAddress>().UserId > 0)
			{
				MessageBox.Show("User email address cannot be set as the default.");
				return;
			}
			if (Preferences.Set(PreferenceName.EmailDefaultAddressNum, _listEmailAddresses[gridMain.GetSelectedIndex()].Id))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void butWebMailNotify_Click(object sender, EventArgs e)
		{
			if (gridMain.GetSelectedIndex() == -1)
			{
				MessageBox.Show("Please select a row first.");
				return;
			}
			if (gridMain.SelectedTag<EmailAddress>().UserId > 0)
			{
				MessageBox.Show("User email address cannot be set as WebMail Notify.");
				return;
			}
			if (Preferences.Set(PreferenceName.EmailNotifyAddressNum, _listEmailAddresses[gridMain.GetSelectedIndex()].Id))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void butAdd_Click(object sender, EventArgs e)
		{
			FormEmailAddressEdit FormEAE = new FormEmailAddressEdit(0, true);
			FormEAE.ShowDialog();
			if (FormEAE.DialogResult == DialogResult.OK)
			{
				FillGrid();
				IsChanged = true;
			}
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				if (gridMain.GetSelectedIndex() == -1)
				{
					MessageBox.Show("Please select an email address.");
					return;
				}
				EmailAddressNum = _listEmailAddresses[gridMain.GetSelectedIndex()].Id;
			}
			else
			{//The following fields are only visible when not in selection mode.
				int inboxCheckIntervalMinuteCount = 0;
				try
				{
					inboxCheckIntervalMinuteCount = int.Parse(textInboxCheckInterval.Text);
					if (inboxCheckIntervalMinuteCount < 1 || inboxCheckIntervalMinuteCount > 60)
					{
						throw new ApplicationException("Invalid value.");//User never sees this message.
					}
				}
				catch
				{
					MessageBox.Show("Inbox check interval must be between 1 and 60 inclusive.");
					return;
				}
				if (Preferences.Set(PreferenceName.EmailInboxCheckInterval, inboxCheckIntervalMinuteCount)
					| Preferences.Set(PreferenceName.EmailDisclaimerIsOn, checkEmailDisclaimer.Checked))
				{
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			DialogResult = DialogResult.OK;
		}

		private void FormEmailAddresses_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (IsChanged)
			{
				DataValid.SetInvalid(InvalidType.Email);
			}
		}
	}
}
