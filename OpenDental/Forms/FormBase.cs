using Imedisoft.Properties;
using OpenDental;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public class FormBase : ODForm
    {
        public FormBase()
        {
            Font = new Font("Tahoma", 8f);
            Icon = Resources.ProgramIconBlue;
        }

        protected void ShowError(string errorMessage)
            => CodeBase.ODMessageBox.Show(this, errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

        protected void ShowInfo(string errorMessage)
            => CodeBase.ODMessageBox.Show(this, errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

        protected DialogResult Prompt(string question, MessageBoxButtons buttons = MessageBoxButtons.YesNo) 
            => CodeBase.ODMessageBox.Show(this, question, Text, buttons, MessageBoxIcon.Question);
    }
}
