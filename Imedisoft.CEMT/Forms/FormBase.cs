using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public class FormBase : Form
    {
        public FormBase()
        {
            Font = new Font("Segoe UI", 9f);
        }

        protected void ShowError(string errorMessage)
        {
            MessageBox.Show(this,
                errorMessage, "CEMT",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
