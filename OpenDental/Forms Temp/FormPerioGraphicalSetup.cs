using Imedisoft.Data;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormPerioGraphicalSetup : ODForm
	{
		public FormPerioGraphicalSetup()
		{
			InitializeComponent();

		}

		private void FormPerioGraphicalSetup_Load(object sender, EventArgs e)
		{
			this.butColorCal.BackColor = PrefC.GetColor(PreferenceName.PerioColorCAL);
			this.butColorFurc.BackColor = PrefC.GetColor(PreferenceName.PerioColorFurcations);
			this.butColorFurcRed.BackColor = PrefC.GetColor(PreferenceName.PerioColorFurcationsRed);
			this.butColorGM.BackColor = PrefC.GetColor(PreferenceName.PerioColorGM);
			this.butColorMGJ.BackColor = PrefC.GetColor(PreferenceName.PerioColorMGJ);
			this.butColorProbing.BackColor = PrefC.GetColor(PreferenceName.PerioColorProbing);
			this.butColorProbingRed.BackColor = PrefC.GetColor(PreferenceName.PerioColorProbingRed);
		}

		private void butColorCal_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorCal.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorCal.BackColor = this.colorPicker.Color;
			}
		}

		private void butColorFurc_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorFurc.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorFurc.BackColor = this.colorPicker.Color;
			}
		}

		private void butColorFurcRed_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorFurcRed.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorFurcRed.BackColor = this.colorPicker.Color;
			}
		}

		private void butColorGM_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorGM.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorGM.BackColor = this.colorPicker.Color;
			}
		}

		private void butColorMGJ_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorMGJ.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorMGJ.BackColor = this.colorPicker.Color;
			}
		}

		private void butColorProbing_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorProbing.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorProbing.BackColor = this.colorPicker.Color;
			}
		}

		private void butColorProbingRed_Click(object sender, EventArgs e)
		{
			this.colorPicker.Color = this.butColorProbingRed.BackColor;
			if (this.colorPicker.ShowDialog() == DialogResult.OK)
			{
				this.butColorProbingRed.BackColor = this.colorPicker.Color;
			}
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			Preferences.Set(PreferenceName.PerioColorCAL, this.butColorCal.BackColor.ToArgb());
			Preferences.Set(PreferenceName.PerioColorFurcations, this.butColorFurc.BackColor.ToArgb());
			Preferences.Set(PreferenceName.PerioColorFurcationsRed, this.butColorFurcRed.BackColor.ToArgb());
			Preferences.Set(PreferenceName.PerioColorGM, this.butColorGM.BackColor.ToArgb());
			Preferences.Set(PreferenceName.PerioColorMGJ, this.butColorMGJ.BackColor.ToArgb());
			Preferences.Set(PreferenceName.PerioColorProbing, this.butColorProbing.BackColor.ToArgb());
			Preferences.Set(PreferenceName.PerioColorProbingRed, this.butColorProbingRed.BackColor.ToArgb());
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
