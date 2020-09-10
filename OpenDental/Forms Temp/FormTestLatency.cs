using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenDental
{
	public partial class FormTestLatency : ODForm
	{
		public FormTestLatency()
		{
			InitializeComponent();

		}

		private void FormTestLatency_Load(object sender, EventArgs e)
		{

		}

		private void butLatency_Click(object sender, EventArgs e)
		{
			Stopwatch watch = new Stopwatch();
			Cursor = Cursors.WaitCursor;
			watch.Start();
			MiscData.GetMySqlVersion();//a nice short query and small dataset.
			watch.Stop();
			textLatency.Text = watch.ElapsedMilliseconds.ToString();
			Cursor = Cursors.Default;
		}

		private void butSpeed_Click(object sender, EventArgs e)
		{
			Stopwatch watch = new Stopwatch();
			Cursor = Cursors.WaitCursor;
			watch.Start();
			MiscData.GetMySqlVersion();//a nice short query and small dataset.
			watch.Stop();
			long latency = watch.ElapsedMilliseconds;
			watch.Restart();
			Preferences.RefreshCache();
			watch.Stop();
			long speed = watch.ElapsedMilliseconds - latency;
			textSpeed.Text = speed.ToString();
			Cursor = Cursors.Default;
		}

		private void butClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
