using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormShutdown : ODForm
	{
		///<summary>Set to true if part of the update process.  Makes it behave more discretely to avoid worrying people.</summary>
		public bool IsUpdate;

		public FormShutdown()
		{
			InitializeComponent();
		}

		private void FormShutdown_Load(object sender, EventArgs e)
		{
			List<string> runningComps = Computers.GetRunningComputers().Select(x => x.CompName).ToList();
			for (int i = 0; i < runningComps.Count; i++)
			{
				listMain.Items.Add(runningComps[i]);
			}

			if (IsUpdate)
			{
				butShutdown.Text = Lan.G(this, "Continue");
			}
		}

		private void butShutdown_Click(object sender, EventArgs e)
		{
			if (IsUpdate)
			{
				DialogResult = DialogResult.OK;
				return;
			}

			if (!MsgBox.Show(MsgBoxButtons.OKCancel, "Shutdown this program on all workstations except this one?  Users will be given a 15 second warning to save data."))
			{
				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
