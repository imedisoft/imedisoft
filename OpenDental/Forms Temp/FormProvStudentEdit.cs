using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;

namespace OpenDental
{
	public partial class FormProvStudentEdit : ODForm
	{
		private long _autoUserName;
		private bool _isGeneratingAbbr = true;
		private User _existingUser;
		///<summary>Set this when selecting a pre-existing Student.</summary>
		public Provider ProvStudent;
		private List<SchoolClass> _listSchoolClasses;

		public FormProvStudentEdit()
		{
			InitializeComponent();
			
		}

		private void FormProvStudentEdit_Load(object sender, EventArgs e)
		{
			SetFilterControlsAndAction(() =>
			{
				if (_isGeneratingAbbr)
				{
					GenerateAbbr();
				}
			},
				(int)TimeSpan.FromSeconds(0.5).TotalMilliseconds,
				textFirstName, textLastName);
			_existingUser = new User();
			//Load the Combo Box
			_listSchoolClasses = SchoolClasses.GetAll();
			for (int i = 0; i < _listSchoolClasses.Count; i++)
			{
				comboClass.Items.Add(SchoolClasses.GetDescription(_listSchoolClasses[i]));
			}
			comboClass.SelectedIndex = 0;
			//Create a provider object if none has been provided
			if (ProvStudent == null)
			{
				ProvStudent = new Provider();
			}
			//From the add button - Select as much pre-given info as possible
			if (ProvStudent.Id == 0)
			{
				labelPassDescription.Visible = false;
				_autoUserName = Providers.GetNextAvailableProvNum();
				textUserName.Text = POut.Long(_autoUserName);//User-names are suggested to be the ProvNum of the provider.  This can be changed at will.
				for (int i = 0; i < _listSchoolClasses.Count - 1; i++)
				{
					if (_listSchoolClasses[i].Id != ProvStudent.SchoolClassId)
					{
						continue;
					}
					comboClass.SelectedIndex = i;
					break;
				}
				textFirstName.Text = ProvStudent.FirstName;
				textLastName.Text = ProvStudent.LastName;
			}
			//Double-Clicking an existing student
			else
			{
				_isGeneratingAbbr = false;
				for (int i = 0; i < _listSchoolClasses.Count - 1; i++)
				{
					if (_listSchoolClasses[i].Id != ProvStudent.SchoolClassId)
					{
						continue;
					}
					comboClass.SelectedIndex = i;
					break;
				}
				textAbbr.Text = ProvStudent.Abbr;
				textFirstName.Text = ProvStudent.FirstName;
				textLastName.Text = ProvStudent.LastName;
				List<User> userList = Providers.GetAttachedUsers(ProvStudent.Id);
				if (userList.Count > 0)
				{
					textUserName.Text = userList[0].UserName;//Should always happen if they are a student.
					_existingUser = userList[0];
				}
				textProvNum.Text = POut.Long(ProvStudent.Id);
			}
		}

		private void textAbbr_KeyUp(object sender, KeyEventArgs e)
		{
			_isGeneratingAbbr = false;
		}

		private void GenerateAbbr()
		{
			string abbr = "";
			if (textLastName.TextLength > 4)
			{
				abbr = textLastName.Text.Substring(0, 4);
			}
			else
			{
				abbr = textLastName.Text;
			}
			if (textFirstName.TextLength > 1)
			{
				abbr += textFirstName.Text.Substring(0, 1);
			}
			else
			{
				abbr += textFirstName.Text;
			}
			textAbbr.Text = abbr;
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (textFirstName.Text == "")
			{
				MessageBox.Show("Please fill in a first name.");
				return;
			}
			if (textLastName.Text == "")
			{
				MessageBox.Show("Please fill in a last name.");
				return;
			}
			if (textAbbr.Text == "")
			{
				MessageBox.Show("Please fill in an abbreviation.");
				return;
			}
			if (textUserName.Text == "")
			{
				MessageBox.Show("Please fill in a user name.");
				return;
			}
			ProvStudent.FirstName = textFirstName.Text;
			ProvStudent.LastName = textLastName.Text;
			ProvStudent.Abbr = textAbbr.Text;
			ProvStudent.SchoolClassId = _listSchoolClasses[comboClass.SelectedIndex].Id;
			User newUser = new User();
			bool isAutoUserName = true;
			if (ProvStudent.Id > 0 || _autoUserName.ToString() != textUserName.Text)
			{
				isAutoUserName = false;
			}
			if (isAutoUserName && !Preferences.GetBool(PreferenceName.RandomPrimaryKeys))
			{//Is a new student using the default user name given
				long provNum = Providers.GetNextAvailableProvNum();
				if (_autoUserName != provNum)
				{
					MessageBox.Show("The default user name was already taken.  The next available user name was used.");
					_autoUserName = provNum;
				}
				provNum = Providers.Insert(ProvStudent);
				if (provNum != _autoUserName)
				{
					MessageBox.Show("The default user name is unavailable.  Please set a user name manually.");
					Providers.Delete(ProvStudent);
					return;
				}
				newUser.UserName = _autoUserName.ToString();
				newUser.PasswordHash = Password.Hash(textPassword.Text);
				newUser.ProviderId = provNum;
				Users.Insert(newUser, new List<long> { Preferences.GetLong(PreferenceName.SecurityGroupForStudents) });
			}
			else
			{//Has changed the user name from the default or is editing a pre-existing student
				try
				{
					if (ProvStudent.Id == 0)
					{
						long provNum = Providers.Insert(ProvStudent);
						newUser.UserName = textUserName.Text;
						newUser.PasswordHash = Password.Hash(textPassword.Text);
						newUser.ProviderId = provNum;
						Users.Insert(newUser, new List<long> { Preferences.GetLong(PreferenceName.SecurityGroupForStudents) });//Performs validation
					}
					else
					{
						Providers.Update(ProvStudent);
						_existingUser.UserName = textUserName.Text;
						_existingUser.PasswordHash = Password.Hash(textPassword.Text);
						Users.Update(_existingUser);//Performs validation
					}
				}
				catch (Exception ex)
				{
					if (ProvStudent.Id == 0)
					{
						Providers.Delete(ProvStudent);
					}
					MessageBox.Show(ex.Message);
					return;
				}
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
