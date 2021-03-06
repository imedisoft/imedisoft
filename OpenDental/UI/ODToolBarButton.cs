using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental.UI
{
	/// <summary>
	/// Almost the same as the included ToolBarButton, but with a few extra properties.
	/// </summary>
	public class ODToolBarButton : System.ComponentModel.Component
	{
        public bool IsRed;

        /// <summary>
		/// A one or two character notification string which will show just above the dropdown arrow when dropDownMenu is not null.
		/// If null or empty, the dropdown arrow background will draw in the typical color and no text will show.
		/// Otherwise the dropdown rectangle will use the notification color background.
		/// </summary>
        public string NotificationText;

		/// <summary>
		/// DateTime of the last time this button was clicked. Used  to stop double clicking from firing 2 events.
		/// This must be public so that ODToolBar can access this.
		/// Must be a property or we will get a "marshall-by-reference" warning.
		/// </summary>
		public DateTime DateTimeLastClicked { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ODToolBarButton"/> class.
		/// </summary>
		public ODToolBarButton() : this("", -1, "", null)
		{
		}

		/// <summary>
		/// Creates a new ODToolBarButton with the given text.
		/// buttonTag will be a string for module specific buttons and will be a Program object for program link buttons.
		/// </summary>
		public ODToolBarButton(string text, int imageIndex, string tooltipText, object tag)
		{
			ImageIndex = imageIndex;
			Text = text;
			ToolTipText = tooltipText;
			Tag = tag;
		}

		/// <summary>
		/// Creates a new button of the specified style.
		/// </summary>
		public ODToolBarButton(ODToolBarButtonStyle style) : this("", -1, "", null)
		{
			Style = style;
		}

        /// <summary>
		/// The current page for this button.
		/// </summary>
        public int PageValue { get; set; }

        /// <summary>
		/// The max page for this button.
		/// </summary>
        public int PageMax { get; set; }

        /// <summary>
		/// The bounds of this button.
		/// </summary>
        public Rectangle Bounds { get; set; }

		public ODToolBarButtonStyle Style { get; set; } = ODToolBarButtonStyle.PushButton;

		public ToolBarButtonState State { get; set; } = ToolBarButtonState.Normal;

		public string Text { get; set; }

        public string ToolTipText { get; set; }

		public int ImageIndex { get; set; } = -1;

        public bool Enabled { get; set; } = true;

        public Menu DropDownMenu { get; set; }

        /// <summary>
		/// Holds extra information about the button, so we can tell which button was clicked.
		/// Tag will be set to a string for module specific buttons and will be a Program object for program link buttons.
		/// </summary>
        public object Tag { get; set; }

        /// <summary>
		/// Only used if style is ToggleButton.
		/// </summary>
        public bool Pushed { get; set; }
    }

	/// <summary>
	/// Identifies the state of a <see cref="ODToolBarButton"/>.
	/// </summary>
	public enum ToolBarButtonState
	{
		Normal,

		/// <summary>
		/// Mouse is hovering over the button and the mouse button is not pressed.
		/// </summary>
		Hover,

		/// <summary>
		/// Mouse was pressed over this button and is still down, even if it has moved off this button or off the toolbar.
		/// </summary>
		Pressed,

		/// <summary>
		/// In a dropdown button, only the dropdown portion is pressed. For hover, the entire button acts as one, but for pressing, the dropdown can be pressed separately.
		/// </summary>
		DropPressed
	}

	/// <summary>
	/// Identifies the style of a <see cref="ODToolBarButton"/>.
	/// </summary>
	public enum ODToolBarButtonStyle
	{
		/// <summary>
		/// A button with a dropdown list on the right.
		/// </summary>
		DropDownButton,

		/// <summary>
		/// A standard button
		/// </summary>
		PushButton,

		Separator,

		/// <summary>
		/// Toggles between pushed and not pushed when clicked on.
		/// </summary>
		ToggleButton,

		/// <summary>
		/// Not clickable. Just text where a button would normally be. Can also include an image.
		/// </summary>
		Label,

		/// <summary>
		/// Editable textbox that throws page nav events. Includes a label after the textbox to show total pages.
		/// </summary>
		PageNav,
	}
}
