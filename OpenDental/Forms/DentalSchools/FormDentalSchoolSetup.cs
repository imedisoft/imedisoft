using Imedisoft.Data;
using Imedisoft.Data.Cache;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
	public partial class FormDentalSchoolSetup : FormBase
	{
		public FormDentalSchoolSetup()
		{
			InitializeComponent();
		}

		private void FormDentalSchoolSetup_Load(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}

			var studentGroup = UserGroups.GetGroup(Preferences.GetLong(PreferenceName.SecurityGroupForStudents));
			var instructorGroup = UserGroups.GetGroup(Preferences.GetLong(PreferenceName.SecurityGroupForInstructors));

			studentsTextBox.Text = studentGroup?.Description;
			instructorsTextBox.Text = instructorGroup?.Description;

			studentsButton.Enabled = instructorsTextBox.Enabled
				= Security.IsAuthorized(Permissions.SecurityAdmin);
		}

		private void StudentButton_Click(object sender, EventArgs e)
		{
            using var formUserGroupPicker = new FormUserGroupPicker
            {
                IsAdminMode = false
            };

            if (formUserGroupPicker.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			var result = Prompt(Translation.DentalSchools.ConfirmMoveStudentsToNewGroup, 
				MessageBoxButtons.YesNoCancel);

			if (result == DialogResult.Cancel) return;

			if (result == DialogResult.Yes)
			{
				try
				{
					Userods.UpdateUserGroupsForDentalSchools(formUserGroupPicker.UserGroup, false);
				}
				catch
				{
					ShowError(Translation.DentalSchools.CannotMoveStudentsDueToSecurityAdminPermission);

					return;
				}
			}

			Preferences.Set(PreferenceName.SecurityGroupForStudents, formUserGroupPicker.UserGroup.Id);

			studentsTextBox.Text = formUserGroupPicker.UserGroup.Description;

			CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
		}

		private void InstructorsButton_Click(object sender, EventArgs e)
		{
            using var formUserGroupPicker = new FormUserGroupPicker
            {
                IsAdminMode = false
            };

            if (formUserGroupPicker.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			var result = Prompt(Translation.DentalSchools.ConfirmMoveInstructorsToNewGroup, 
				MessageBoxButtons.YesNoCancel);

			if (result == DialogResult.Cancel) return;
			
			if (result == DialogResult.Yes)
			{
				try
				{
					Userods.UpdateUserGroupsForDentalSchools(formUserGroupPicker.UserGroup, true);
				}
				catch
				{
					ShowError(Translation.DentalSchools.CannotMoveInstructorsDueToSecurityAdminPermission);

					return;
				}
			}

			Preferences.Set(PreferenceName.SecurityGroupForInstructors, formUserGroupPicker.UserGroup.Id);

			instructorsTextBox.Text = formUserGroupPicker.UserGroup.Description;

			CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
		}

		private void GradingScalesButton_Click(object sender, EventArgs e)
		{
			using var formGradingScales = new FormGradingScales();

			formGradingScales.ShowDialog(this);
		}

		private void EvaluationsButton_Click(object sender, EventArgs e)
		{
			using var formEvaluationDefs = new FormEvaluationDefs();

			formEvaluationDefs.ShowDialog(this);
		}
    }
}
