using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenDental.UI
{
	public class GridColumn
	{
		///<summary>Property backer.</summary>
		private float _dynamicWeight = 1;

		///<summary>Set this to an event method and it will be used when the column header is clicked.</summary>
		public EventHandler CustomClickEvent;

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GridColumn"/> class.
		/// </summary>
		public GridColumn()
		{
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public GridColumn(string headerText, int columnWidth)
		{
			HeaderText = headerText;
			ColumnWidth = columnWidth;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width.</summary>
		public GridColumn(string heading, int colWidth, HorizontalAlignment textAlign)
		{
			HeaderText = heading;
			ColumnWidth = colWidth;
			TextAlign = textAlign;
		}

		///<summary>Deprecated. Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public GridColumn(string heading, int colWidth, HorizontalAlignment textAlign, bool isEditable)
		{
			HeaderText = heading;
			ColumnWidth = colWidth;
			TextAlign = textAlign;
			IsEditable = isEditable;
		}

		///<summary>Deprecated. Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public GridColumn(string heading, int colWidth, bool isEditable)
		{
			HeaderText = heading;
			ColumnWidth = colWidth;
			IsEditable = isEditable;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and sorting strategy.</summary>
		public GridColumn(string heading, int colWidth, GridSortingStrategy sortingStrategy)
		{
			HeaderText = heading;
			ColumnWidth = colWidth;
			SortingStrategy = sortingStrategy;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and sorting strategy.</summary>
		public GridColumn(string heading, int colWidth, HorizontalAlignment textAlign, GridSortingStrategy sortingStrategy)
		{
			HeaderText = heading;
			ColumnWidth = colWidth;
			TextAlign = textAlign;
			SortingStrategy = sortingStrategy;
		}
		#endregion Constructors

		#region Properties
		




		/// <summary>
		/// Gets or sets the width of the column.
		/// </summary>
		[DefaultValue(80)]
		public int ColumnWidth { get; set; } = 80;

		/// <summary>
		/// Gets or sets the text value of the column header.
		/// </summary>
		[DefaultValue("")]
		public string HeaderText { get; set; } = "";

		/// <summary>
		///		<para>
		///			Gets or sets a value indicating whether the cells in this column can be edited.
		///		</para>
		///		<para>
		///			Only applies when the <see cref="ODGrid.SelectionMode"/> of the parent 
		///			<see cref="ODGrid"/> is set to <see cref="GridSelectionMode.OneCell"/>.
		///		</para>
		/// </summary>
		[DefaultValue(false)]
		public bool IsEditable { get; set; } = false;






		///<summary>When combo boxes are used in column cells, this can be set to force a width of dropdown instead of using the column width.</summary>
		[DefaultValue(0)]
		public int DropDownWidth { get; set; } = 0;



		///<summary>List of images that can be picked from within each cell.</summary>
		[DefaultValue(null)]
		public ImageList ImageList { get; set; } = null;

		///<summary>Default false. Set a non-zero starting width for the column, then set this to true.  Column(s) will resize dynamically as long as not hScrollVisible.  If no columns are set to be dynamic, then the right column resizes automatically.</summary>
		[DefaultValue(false)]
		public bool IsWidthDynamic { get; set; } = false;

		///<summary>If IsWidthDynamic and if there are multiple dynamic columns, then this can also optionally be set.  By default, the weight of a dynamic column is 1.  Setting this to 2.5, for example, will make it have a dynamic width that is 2.5x bigger than a column with a weight of 1.  Must be 1 or greater.</summary>
		[DefaultValue(1f)]
		public float DynamicWeight
		{
			get
			{
				return _dynamicWeight;
			}
			set
			{
				if (value < 1)
				{
					_dynamicWeight = 1;

					return;
				}
				_dynamicWeight = value;
			}
		}



		/// <summary>
		/// Can be used when grid.SelectionMode=OneCell. 
		/// Setting this list of strings causes combo boxes to be used in column cells instead of textboxes.
		/// This is the list of strings to show in the combo boxes.
		/// </summary>
		public List<string> ListDisplayStrings { get; set; } = null;

		[DefaultValue(GridSortingStrategy.StringCompare)]
		public GridSortingStrategy SortingStrategy { get; set; } = GridSortingStrategy.StringCompare;

		/// <summary>
		/// Gets or sets the object that contains data about the column.
		/// </summary>
		public object Tag { get; set; } = null;

		/// <summary>
		/// Gets or sets the text alignment of the column.
		/// </summary>
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign { get; set; } = HorizontalAlignment.Left;




		///<summary>These fields can only be changed internally by ODGrid, never from outside ODGrid.  Includes Pos, Width, and Right of this column.</summary>
		public GridColState State { get; set; } = new GridColState();


		#endregion Properties

		public override string ToString()
		{
			return HeaderText + ":" + ColumnWidth.ToString();
		}

		public class GridColState
		{
			///<summary>The location of the left edge.</summary>
			public int XPos = 0;

			public int Width = 0;

			///<summary>The right edge.  Same as the left edge of the next cell.</summary>
			public int Right = 0;

			public override string ToString()
			{
				return "Pos " + XPos.ToString() + ", Width: " + Width.ToString();
			}
		}
	}


	public enum GridSortingStrategy
	{
		///<summary>0- Default</summary>
		StringCompare,
		DateParse,
		ToothNumberParse,
		AmountParse,
		TimeParse,
		VersionNumber,
	}
}
