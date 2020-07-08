using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental.UI{
	public partial class PanelImagesModule : Control{
		public PanelImagesModule(){
			InitializeComponent();
			DoubleBuffered=true;
		}

		protected override void OnPaint(PaintEventArgs pe){
			base.OnPaint(pe);
		}
	}
}
