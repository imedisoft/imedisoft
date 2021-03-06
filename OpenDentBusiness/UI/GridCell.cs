using System;
using System.Drawing;

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

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the value of the cell should be displayed in bold.
		///		</para>
		///		<para>
		///			If set to null, the value of the parent <see cref="GridRow"/> is used.
		///		</para>
		/// </summary>
		public bool? Bold { get; set; } = null;

		/// <summary>
		/// Gets or sets a value indicating whether the value of the cell should be displayed underlined.
		/// </summary>
		public bool Underline { get; set; } = false;

		/// <summary>
		///		<para>
		///			Gets or sets the foreground (text) color of the cell.
		///		</para>
		///		<para>
		///			If set to null, the value of the parent <see cref="GridRow"/> is used.
		///		</para>
		/// </summary>
		public Color? ForeColor { get; set; } = null;

		/// <summary>
		///		<para>
		///			Gets or sets the background color of the cell.
		///		</para>
		///		<para>
		///			If set to null, the value of the parent <see cref="GridRow"/> is used.
		///		</para>
		/// </summary>
		public Color? BackColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the text value of the cell.
		/// </summary>
		public string Text { get; set; } = "";

		/// <summary>
		///		<para>
		///			Occurs when the cell is clicked.
		///		</para>
		///		<para>
		///			Only applies if the <see cref="ODGrid.SelectionMode"/> of the parent 
		///			<see cref="ODGrid"/> control is set to <see cref="GridSelectionMode.One"/>.
		///		</para>
		/// </summary>
		/// <remarks>
		///		If this is not null, the cell will be drawn as a button.
		/// </remarks>
		public EventHandler Clicked { get; set; }

		public int ComboSelectedIndex { get; set; } = -1;

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the cell is a button.
		///		</para>
		/// </summary>
		public bool IsButton => Clicked != null;

		/// <summary>
		/// True immediately after the user clicks the cell, forces the grid to repaint with a flattened "pressed" button, or to restore the button color. 
		/// Used only in GridSelectionMode.One.
		/// </summary>
		public bool ButtonIsPressed { get; set; } = false;
	}
}
