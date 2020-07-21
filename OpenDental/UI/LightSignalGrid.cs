using CodeBase;
using OpenDentBusiness;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.UI
{
    public delegate void ODLightSignalGridClickEventHandler(object sender, ODLightSignalGridClickEventArgs e);

	public partial class LightSignalGrid : Control
	{
		//<summary>collection of SignalButtonStates</summary>
		private SignalButtonState[] sigButStates;

		///<summary>number of pixels that the control has been scrolled by.</summary>
		private int _scrollVal;

		private int _buttonH;
		private bool mouseIsDown;
		private int hotButton;
		private static Font _fontBlue;

		[Category("Action"), Description("Occurs when a button is clicked.")]
		public event ODLightSignalGridClickEventHandler ButtonClick = null;

		public LightSignalGrid()
		{
			InitializeComponent();
			_buttonH = 25;
			hotButton = -1;
			_fontBlue = new Font(Font.FontFamily, 7f, Font.Style);
			MouseWheel += LightSignalGrid_MouseWheel;
		}

		private void LightSignalGrid_MouseWheel(object sender, MouseEventArgs e)
		{
			int scrollValOld = _scrollVal;
			_scrollVal += (e.Delta > 0 ? -1 : 1) * (_buttonH - 3);//_butonH-3 so that as users scroll down, the movement is smooth
																  //MaxScroll = (total calculated height) - (visible height); Can be zero or negative.
			RectifyScrollValue();
			if (scrollValOld != _scrollVal)
			{
				Invalidate();
			}
		}

		private void RectifyScrollValue()
		{
			ODException.SwallowAnyException(() =>
			{
				if (sigButStates == null)
				{
					return;
				}
				int maxScroll = sigButStates.Length * _buttonH - this.Height;
				if (_scrollVal > maxScroll)
				{
					_scrollVal = maxScroll;
				}
				if (_scrollVal < 0)
				{
					_scrollVal = 0;
				}
			});
		}

		protected override Size DefaultSize => new Size(50, 300);

		#region Painting

		protected override void OnPaintBackground(PaintEventArgs pea)
		{
			//don't paint background.  This reduces flickering when using double buffering.
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Height == 0 || sigButStates == null || !Visible)
			{
				base.OnPaint(e);
				return;
			}
			RectifyScrollValue();
			using (Bitmap doubleBuffer = new Bitmap(Width, Height, e.Graphics))
			using (Graphics g = Graphics.FromImage(doubleBuffer))
			{
				//button backgrounds
				Color mixedC;
				Color baseC;
				int R;
				int G;
				int B;
				for (int i = 0; i < sigButStates.Length; i++)
				{
					if ((i + 1) * _buttonH - _scrollVal < 0)
					{
						//button too high to see
						continue;
					}
					if (i * _buttonH - _scrollVal > this.Height)
					{
						//button too low to see
						continue;
					}
					baseC = sigButStates[i].CurrentColor;
					switch (sigButStates[i].State)
					{
						case ToolBarButtonState.Normal://Control is 224,223,227
						case ToolBarButtonState.Pressed:
							using (SolidBrush sb = new SolidBrush(baseC))
							{
								g.FillRectangle(sb, 0, i * _buttonH - _scrollVal, Width, _buttonH);
							}
							break;
						case ToolBarButtonState.Hover://this is darker
							R = baseC.R - 40;
							G = baseC.G - 40;
							B = baseC.B - 40;
							mixedC = Color.FromArgb(R < 0 ? 0 : R, G < 0 ? 0 : G, B < 0 ? 0 : B);
							using (SolidBrush sb = new SolidBrush(mixedC))
							{
								g.FillRectangle(new SolidBrush(mixedC), 0, i * _buttonH - _scrollVal, Width, _buttonH);
							}
							break;
					}
				}
				//grid
				for (int i = 0; i < sigButStates.Length; i++)
				{
					g.DrawLine(Pens.DarkGray, 0, i * _buttonH - _scrollVal, Width, i * _buttonH - _scrollVal);
				}
				//button text
				RectangleF rect;
				StringFormat format = new StringFormat();
				format.Alignment = StringAlignment.Center;
				format.LineAlignment = StringAlignment.Center;
				for (int i = 0; i < sigButStates.Length; i++)
				{
					rect = new RectangleF(-2, i * _buttonH - _scrollVal, Width + 4, _buttonH);
					g.DrawString(sigButStates[i].Text, Font, Brushes.Black, rect, format);
				}
				//outline control
				g.DrawRectangle(Pens.DarkGray, 0, 0, Width - 1, Height - 1);

				e.Graphics.DrawImageUnscaled(doubleBuffer, 0, 0);
			}
			base.OnPaint(e);
		}

		#endregion Painting

		///<summary>This will clear the buttons, reset buttons to the specified list, reset the buttonstates, layout the rows, and invalidate for repaint.</summary>
		public void SetButtons(SigButDef[] butDefs)
		{
			if (butDefs.Length == 0)
			{
				sigButStates = new SignalButtonState[0];
				Invalidate();
				return;
			}
			List<SignalButtonState> listSigButStates = null;
			if (sigButStates != null)
			{
				listSigButStates = sigButStates.Select(x => x.Copy()).ToList();
			}
			//since defs are ordered by buttonIndex, the last def will contain the max number of buttons
			sigButStates = new SignalButtonState[butDefs[butDefs.Length - 1].ButtonIndex + 1];
			for (int i = 0; i < sigButStates.Length; i++)
			{
				sigButStates[i] = new SignalButtonState();
				sigButStates[i].ButDef = SigButDefs.GetByIndex(i, butDefs);//might be null
				if (sigButStates[i].ButDef != null)
				{
					sigButStates[i].Text = sigButStates[i].ButDef.ButtonText;
				}
				sigButStates[i].CurrentColor = Color.White;
				if (listSigButStates != null && sigButStates[i].ButDef != null
					&& listSigButStates.Any(x => x.ButDef != null && x.ButDef.SigButDefNum == sigButStates[i].ButDef.SigButDefNum))
				{
					sigButStates[i].ActiveSignal = listSigButStates
						.FirstOrDefault(x => x.ButDef != null && x.ButDef.SigButDefNum == sigButStates[i].ButDef.SigButDefNum).ActiveSignal;
				}
			}
			Invalidate();
		}

		/// <summary>
		/// Sets the specified buttonIndex to a color and attaches the signal responsible.
		/// This is also used for the manual ack to increase responsiveness. buttonIndex is 0-based.
		/// </summary>
		public void SetButtonActive(int buttonIndex, Color color, SigMessage activeSigMessage)
		{
			if (!IsValidSigButState(buttonIndex))
			{
				return; // no button to light up.
			}

			sigButStates[buttonIndex].CurrentColor = color;
			if (activeSigMessage == null)
			{
				sigButStates[buttonIndex].ActiveSignal = null;
			}
			else
			{
				sigButStates[buttonIndex].ActiveSignal = activeSigMessage.Copy();
			}

			Invalidate();
		}

		/// <summary>
		/// An ack coming from the database.
		/// If it applies to any lights currently showing, then those lights will be turned off.
		/// Returns the 0-based index of the light acked, or -1.
		/// </summary>
		public int ProcessAck(long sigMessageNum)
		{
			for (int i = 0; i < sigButStates.Length; i++)
			{
				if (sigButStates[i].ActiveSignal == null)
				{
					continue;
				}

				if (sigButStates[i].ActiveSignal.SigMessageNum == sigMessageNum)
				{
					sigButStates[i].CurrentColor = Color.White;
					sigButStates[i].ActiveSignal = null;
					Invalidate();
					return i;
				}
			}

			return -1;
		}

		private bool IsValidSigButState(int index)
		{
			if (index < 0 || sigButStates == null)
			{
				return false;
			}
			//Either 0 or a positive index was passed in.  Make sure that index is less than the length of our array.
			return (index < sigButStates.Length);
		}

		/// <summary>
		/// This should only happen when mouse enters. Only causes a repaint if needed.
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (mouseIsDown)
			{
				//Regardless of whether a button is hot, nothing changes until the mouse is released.
				//A hot(pressed) button remains so, and no buttons are hot when hover, so do nothing.
				return;
			}
			//Mouse is not down
			int button = HitTest(e.X, e.Y);//this is the button the mouse is over at the moment.
										   //first handle the old hotbutton
			if (hotButton != -1)
			{//if there is a previous hotbutton
				if (hotButton != button && IsValidSigButState(hotButton))
				{//if we have moved to hover over a new button, or to hover over nothing
					sigButStates[hotButton].State = ToolBarButtonState.Normal;
					Invalidate();
				}
			}
			//then, the new button
			if (button != -1)
			{
				if (hotButton != button && IsValidSigButState(button))
				{//if we have moved to hover over a new button
					sigButStates[button].State = ToolBarButtonState.Hover;
					Invalidate();
				}
			}
			hotButton = button;//this might be -1 if hovering over nothing.
							   //if there was no previous hotbutton, and there is not current hotbutton, then do nothing.
		}

		///<summary>Returns the 0-based button index that contains these coordinates, or -1 if no hit.</summary>
		private int HitTest(int x, int y)
		{
			if (sigButStates == null)
			{
				//This was causing a UE for HQ when hovering over the light signals when Open Dental first starts up during an update.
				return -1;
			}
			int retVal = (y + _scrollVal) / _buttonH;//integer division
			if (retVal > sigButStates.Length - 1)
			{//button not visible
				return -1;
			}
			return (retVal < 0 ? -1 : retVal);//The only valid negative number is -1 regardless of the math above.
		}

		/// <summary>
		/// Resets button appearance. 
		/// This will also deactivate the button if it has been pressed but not released. 
		/// A pressed button will still be hot, however, so that if the mouse enters again, it will behave properly. 
		/// Repaints only if necessary.
		/// </summary>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
			if (mouseIsDown)
			{//mouse is down
			 //If a button is hot, it will remain so, even if leave.  As long as mouse is down, so do nothing.
			 //Also, if a button is not hot, nothing will happen when leave, so do nothing.
				return;
			}

			//Mouse is not down
			if (IsValidSigButState(hotButton))
			{//if there was a previous hotButton
				sigButStates[hotButton].State = ToolBarButtonState.Normal;
				Invalidate();
				hotButton = -1;
			}
		}

		/// <summary>
		/// Change the button to a pressed state.
		/// </summary>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
			{
				return;
			}
			mouseIsDown = true;
			int button = HitTest(e.X, e.Y);
			if (!IsValidSigButState(button))
			{//if there is no current hover button
				return;//don't set a hotButton
			}
			hotButton = button;
			sigButStates[button].State = ToolBarButtonState.Pressed;
			Invalidate();
		}

		/// <summary>
		/// Change button to hover state and repaint if needed.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
			{
				return;
			}
			mouseIsDown = false;
			int button = HitTest(e.X, e.Y);
			if (!IsValidSigButState(hotButton))
			{//if there was not a previous hotButton
				return;
			}
			//There was a previous hotButton
			sigButStates[hotButton].State = ToolBarButtonState.Normal;
			//But can't set it null yet, because still need it for testing
			Invalidate();
			//CLICK: 
			if (hotButton == button && IsValidSigButState(button))
			{//if mouse was released over the same button as it was depressed
				OnButtonClicked(button, sigButStates[button].ButDef, sigButStates[button].ActiveSignal);
				return;//the button will not revert back to hover
			}//end of click section
			else
			{//there was a hot button, but it did not turn into a click
				hotButton = -1;
			}
			if (IsValidSigButState(button))
			{//no click, and now there is a hover button, not the same as original button.
			 //this section could easily be deleted, since all the user has to do is move the mouse slightly.
				sigButStates[button].State = ToolBarButtonState.Hover;
				hotButton = button;//set the current hover button to be the new hotbutton
				Invalidate();
			}
		}

		protected void OnButtonClicked(int myButton, SigButDef myDef, SigMessage mySignal)
		{
            ButtonClick?.Invoke(this, new ODLightSignalGridClickEventArgs(myButton, myDef, mySignal));
        }
	}

	public class SignalButtonState
	{
		/// <summary>
		/// This is also present in the def, but this makes it easier to access.
		/// </summary>
		public string Text;

		/// <summary>
		/// The def assigned to this index.
		/// </summary>
		public SigButDef ButDef;

		public Color CurrentColor;

		public ToolBarButtonState State;

		/// <summary>
		/// If this button is lit up, then this will contain the signal that caused it.
		/// That way, when user clicks on the button to ack, the sigmessage object in the db can be ack'd properly.
		/// </summary>
		public SigMessage ActiveSignal;

		public SignalButtonState Copy()
		{
			SignalButtonState sigButState = (SignalButtonState)MemberwiseClone();
			sigButState.ActiveSignal = ActiveSignal?.Copy();
			sigButState.ButDef = ButDef?.Copy();
			return sigButState;
		}
	}

	public class ODLightSignalGridClickEventArgs
	{
        public ODLightSignalGridClickEventArgs(int myButton, SigButDef myDef, SigMessage mySignal)
		{
			ButtonIndex = myButton;
			ButtonDef = myDef;
			ActiveSignal = mySignal;
		}

        /// <summary>
		/// Remember that this is the 0-based index, but the database uses 1-based.
		/// </summary>
        public int ButtonIndex { get; }

        public SigButDef ButtonDef { get; }

        public SigMessage ActiveSignal { get; }
    }
}
