using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.UI
{
	public class GridRow
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GridRow"/> class.
		/// </summary>
		public GridRow()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GridRow"/> class.
		/// </summary>
		/// <param name="values">The cell values.</param>
		public GridRow(params object[] values)
		{
			foreach (var value in values)
            {
				Cells.Add(value?.ToString() ?? "");
            }
		}

		/// <summary>
		/// Gets the row cells.
		/// </summary>
		public GridCellCollection Cells { get; } = new GridCellCollection();

		/// <summary>
		/// Gets or sets the background color of the row.
		/// </summary>
		[DefaultValue(typeof(Color), "White")]
		public Color BackColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets a value indicating whether the row cells should be displayed in bold.
		/// </summary>
		public bool Bold { get; set; } = false;

		/// <summary>
		/// Gets or sets the foreground (text) color of the row.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		public Color ForeColor { get; set; } = Color.Black;

		/// <summary>
		/// Gets or sets the lower border color of the row.
		/// </summary>
		public Color? LowerBorderColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the object that contains data about the row.
		/// </summary>
		public object Tag { get; set; } = null;







		/// <summary>
		/// This is a very special field.  
		/// Since most of the tables in OD require the ability to attach long notes to each row, this field makes it possible.  
		/// Any note set here will be drawn as a sort of subrow below this row. 
		/// The note can span multiple columns, as defined in grid.NoteSpanStart and grid.NoteSpanStop.
		/// </summary>
		public string Note { get; set; } = "";


		///<summary>If this is a dropdown row, set this reference to a parent row that drops this row down.  If not, null.</summary>
		public GridRow DropDownParent { get; set; } = null;

		/// <summary>
		/// If this is a DropDown parent row, you can set this to true in order for it to show initially as dropped down.
		/// </summary>
		public bool DropDownInitiallyDown { get; set; } = false;

		/// <summary>
		/// These fields can only be changed internally by ODGrid, never from outside ODGrid. 
		/// Includes size and position of this row, visiblity, and dropdown state.
		/// </summary>
		public GridRowState State { get; set; } = new GridRowState();

		/// <summary>
		/// Returns a string representation of the row.
		/// </summary>
		public override string ToString() 
			=> $"YPos: {State.YPos}, HeightMain: {State.HeightMain}, Visible: {State.Visible}";




		public class GridRowState
		{
			///<summary>The height of the main part of the row without the note section.</summary>
			public int HeightMain = 0;
			
			///<summary>The height of the note section of this row.</summary>
			public int HeightNote = 0;
			
			///<summary>The vertical location at which to start drawing this row.  Coordinates are from the top of the first row, as it would be without any scrolling.  To paint, add vertical scrolling, origin, etc.</summary>
			public int YPos = 0;
			
			///<summary>ODGridDropDownState: 0: None (not a dropdown parent), 1:Up (not dropped), 2: Down.</summary>
			public ODGridDropDownState DropDownState = ODGridDropDownState.None;
			
			///<summary>All rows start out visible.  They can be set not visible if they have a dropdown parent that is up.</summary>
			public bool Visible = true;


			///<summary>HeightMain + HeightNote</summary>
			public int HeightTotal => HeightMain + HeightNote;

			public override string ToString()
			{
				return "YPos " + YPos.ToString() + ", HeightTotal: " + HeightTotal.ToString();
			}
		}
	}

	/// <summary>
	/// Determines the state of a dropdown row.
	/// </summary>
	public enum ODGridDropDownState
	{
		///<summary>0 - not a drop down parent.</summary>  
		None,

		///<summary>1 - not dropped down.</summary>  
		Up,

		///<summary>2 - dropped down.</summary>
		Down,
	}
}
