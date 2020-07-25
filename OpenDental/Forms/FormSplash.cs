using Imedisoft.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public class FormSplash : Form
	{
		public FormSplash()
        {
            BackgroundImage = Resources.Splash;
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(500, 300);
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
