using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public class FormBase : Form
    {
        public FormBase()
        {
            Font = new Font("Tahoma", 8f);
            Padding = new Padding(10);
        }

        protected void ShowError(string errorMessage)
            => MessageBox.Show(this,
                errorMessage, "CEMT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

        protected void ShowInfo(string errorMessage)
            => MessageBox.Show(this,
                errorMessage, "CEMT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

        protected DialogResult Confirm(string question, MessageBoxButtons buttons = MessageBoxButtons.YesNo)
            => MessageBox.Show(this, question, "CEMT",
                buttons, MessageBoxIcon.Question);
    }
}
