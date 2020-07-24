using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public class GridCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GridCell"/> class.
		/// </summary>
		public GridCell()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GridCell"/> class.
		/// </summary>
		/// <param name="text">The cell text.</param>
		public GridCell(string text)
		{
			Text = text;
		}

		#region Properties
		///<summary>If YN.Unknown, then the row state is used for bold.  Otherwise, this overrides the row.</summary>
		public YN Bold { get; set; } = YN.Unknown;

		///<summary>The event that should happen if this cell is clicked. Also causes cell to be styled as a "button". For use in GridSelectionMode.One. Unsure if these should be set with = or +=.</summary>
		public EventHandler ClickEvent { get; set; }

		///<summary>Default is Color.Empty.  If any color is set, it will override the row color.</summary>
		public Color ColorText { get; set; } = Color.Empty;

		///<summary>Default is Color.Empty.  If any color is set, it might blend with the background color; still researching behavior.</summary>
		public Color ColorBackG { get; set; } = Color.Empty;

		///<summary></summary>
		public int ComboSelectedIndex { get; set; } = -1;

		///<summary>True if the ClickEvent is not null.</summary>
		public bool IsButton
		{
			get { return ClickEvent != null; }
		}

		///<summary>True immediately after the user clicks the cell, forces the grid to repaint with a flattened "pressed" button, or to restore the button color. Used only in GridSelectionMode.One. </summary>
		public bool ButtonIsPressed { get; set; } = false;

		///<summary></summary>
		public string Text { get; set; } = "";

		///<summary>If YN.Unknown, then the row state is used for underline.  Otherwise, this overrides the row.</summary>
		public YN Underline { get; set; } = YN.Unknown;


		#endregion Properties
	}
}
