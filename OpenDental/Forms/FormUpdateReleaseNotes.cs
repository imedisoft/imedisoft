using System;

namespace Imedisoft.Forms
{
    public partial class FormUpdateReleaseNotes : FormBase
	{
		public FormUpdateReleaseNotes(string url)
		{
			InitializeComponent();

			webBrowser.Url = new Uri(url);
		}
	}
}
