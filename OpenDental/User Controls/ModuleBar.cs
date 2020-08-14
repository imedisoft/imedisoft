using CodeBase;
using Imedisoft.Properties;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
	/// <summary>The button bar along the left side for the modules.</summary>
	public class ModuleBar : Control
	{
		#region Fields - Public
		///<summary>This is also used by the main toolbar for now</summary>
		public static bool IsAlternateIcons;
		public static Action ActionIconChange;
		#endregion Fields - Public

		#region Fields - Private
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private Brush _brushBack = SystemBrushes.Control;
		private SolidBrush _brushHot = new SolidBrush(Color.FromArgb(235, 235, 235));
		private SolidBrush _brushPressed = new SolidBrush(Color.FromArgb(210, 210, 210));
		private SolidBrush _brushSelected = new SolidBrush(Color.FromArgb(255, 255, 255));
		private int currentHot = -1;
		///<summary>At 96dpi</summary>
		private int _heightButton = 39 + 26;
		///<summary>Ignore Font.</summary>
		private Font _font = new Font("Arial", 8);
		///<summary></summary>
		private List<ModuleBarButton> _listButtons;
		private Pen _penOutline = new Pen(Color.FromArgb(28, 81, 128));
		private int _radiusCorner = 4;
		///<summary>Property backer</summary>
		private int _selectedIndex = -1;
		///<summary>Used when click event is cancelled.</summary>
		private int _selectedIndexPrevious;
		///<summary>Class level variable, to avoid allocating and disposing memory repeatedly every frame.</summary>
		private StringFormat _stringFormat;
		#endregion Fields - Private

		#region Constructor
		///<summary></summary>
		public ModuleBar()
		{
			InitializeComponent();
			DoubleBuffered = true;
			//Rectangle gradientRect=new Rectangle(myButton.Bounds.X,myButton.Bounds.Y+myButton.Bounds.Height-10,myButton.Bounds.Width,10);
			//_brushHot=new LinearGradientBrush(new PointF(0,0),new PointF(0,10),_outlookSelectedBrush.Color,_outlookPressedBrush.Color);
			_stringFormat = new StringFormat();
			_stringFormat.Alignment = StringAlignment.Center;
			_listButtons = new List<ModuleBarButton>();
			_listButtons.Add(new ModuleBarButton(EnumModuleType.Appointments, "Appts", GetImage(EnumModuleType.Appointments)));//0
			_listButtons.Add(new ModuleBarButton(EnumModuleType.Family, "Family", GetImage(EnumModuleType.Family)));           //1
			_listButtons.Add(new ModuleBarButton(EnumModuleType.Account, "Account", GetImage(EnumModuleType.Account)));        //2
			_listButtons.Add(new ModuleBarButton(EnumModuleType.TreatPlan, "Treat' Plan", GetImage(EnumModuleType.TreatPlan)));//3
			_listButtons.Add(new ModuleBarButton(EnumModuleType.Chart, "Chart", GetImage(EnumModuleType.Chart)));              //4
			_listButtons.Add(new ModuleBarButton(EnumModuleType.Images, "Imaging", GetImage(EnumModuleType.Images)));           //5
			_listButtons.Add(new ModuleBarButton(EnumModuleType.Manage, "Manage", GetImage(EnumModuleType.Manage)));           //6
			_selectedIndex = 0;
			ComputeButtonSizes();
		}
		#endregion Constructor

		#region Component Designer generated code
		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
		#endregion Component Designer generated code

		#region Events - Raise
		///<summary></summary>
		[Category("OD")]
		[Description("Occurs when a module button is clicked.")]
		public event ButtonClickedEventHandler ButtonClicked = null;
		///<summary></summary>
		protected void OnButtonClicked(ModuleBarButton myButton, bool myCancel)
		{
			if (ButtonClicked != null)
			{
				//previousSelected=SelectedIndex;
				ButtonClicked_EventArgs oArgs = new ButtonClicked_EventArgs(myButton, myCancel);
				ButtonClicked(this, oArgs);
				if (oArgs.Cancel)
				{
					_selectedIndex = _selectedIndexPrevious;
					Invalidate();
				}
			}
		}
		#endregion Events - Raise

		#region Properties
		///<summary></summary>
		[Browsable(false)]
		[DefaultValue(EnumModuleType.None)]
		public EnumModuleType SelectedModule
		{
			get
			{
				if (_selectedIndex == -1)
				{
					return EnumModuleType.None;
				}
				return _listButtons[_selectedIndex].ModuleType;
			}
			set
			{
				ModuleBarButton moduleBarButton = _listButtons.FirstOrDefault(x => x.ModuleType == value);
				if (moduleBarButton == null)
				{
					return;
				}
				_selectedIndex = _listButtons.IndexOf(moduleBarButton);
			}
		}

		///<summary>Only used in 3 places where it can't be avoided because of the business layer.</summary>
		[Browsable(false)]
		[DefaultValue(-1)]
		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				_selectedIndex = value;
				Invalidate();
			}
		}
		#endregion

		#region Methods - Public
		///<summary>Needed in just a few areas for backward compatibility.</summary>
		public int IndexOf(EnumModuleType moduleType)
		{
			if (moduleType == EnumModuleType.None)
			{
				return -1;
			}
			ModuleBarButton moduleBarButton = _listButtons.FirstOrDefault(x => x.ModuleType == moduleType);
			if (moduleBarButton == null)
			{
				return -1;
			}
			return _listButtons.IndexOf(moduleBarButton);
		}

		/// <summary>Fixes theme image and text translation for any existing buttons.</summary>
		public void RefreshButtons()
		{
			bool isMedical = Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum);
			for (int i = 0; i < _listButtons.Count; i++)
			{
				_listButtons[i].Image = GetImage(_listButtons[i].ModuleType, isMedical);
				switch (_listButtons[i].ModuleType)
				{
					case EnumModuleType.Appointments:
						_listButtons[i].Caption = "Appts";
						break;
					case EnumModuleType.Family:
						_listButtons[i].Caption = "Family";
						break;
					case EnumModuleType.Account:
						_listButtons[i].Caption = "Account";
						break;
					case EnumModuleType.TreatPlan:
						_listButtons[i].Caption = "Treat' Plan";
						break;
					case EnumModuleType.Chart:
						if (Prefs.GetBool(PrefName.EasyHideClinical))
						{
							_listButtons[i].Caption = "Procs";
						}
						_listButtons[i].Caption = "Chart";
						break;
					case EnumModuleType.Images:
						_listButtons[i].Caption = "Imaging";
						break;
					case EnumModuleType.Manage:
						_listButtons[i].Caption = "Manage";
						break;
				}
			}
			Invalidate();
		}

		public static void SetIcons(bool isAlternateIcons)
		{
			if (isAlternateIcons == IsAlternateIcons)
			{
				return;
			}
			IsAlternateIcons = isAlternateIcons;
			if (ActionIconChange != null)
			{
				ActionIconChange.Invoke();
			}
		}

		///<summary></summary>
		public void SetVisible(EnumModuleType moduleType, bool isVisible)
		{
			ModuleBarButton moduleBarButton = _listButtons.FirstOrDefault(x => x.ModuleType == moduleType);
			if (moduleBarButton == null)
			{
				return;
			}
			moduleBarButton.Visible = isVisible;
			Invalidate();
		}
		#endregion Methods - Public

		#region Methods - OnPaint
		/// <summary>Triggered every time the control decides to repaint itself.</summary>
		protected override void OnPaint(PaintEventArgs pe)
		{
			try
			{
				ComputeButtonSizes();
				bool isHot;
				bool isSelected;
				bool isPressed;
				pe.Graphics.DrawLine(Pens.Gray, Width - 1, 0, Width - 1, Height - 1);
				for (int i = 0; i < _listButtons.Count; i++)
				{
					Point mouseLoc = PointToClient(MousePosition);
					isHot = _listButtons[i].Bounds.Contains(mouseLoc);
					isPressed = (MouseButtons == MouseButtons.Left && isHot);
					isSelected = (i == _selectedIndex);
					DrawButton(_listButtons[i], isHot, isPressed, isSelected, pe.Graphics);
				}
			}
			catch
			{
				//We had one customer who was receiving overflow exceptions because the ClientRetangle provided by the system was invalid,
				//due to a graphics device hardware state change when loading the Dexis client application via our Dexis bridge.
				//If we receive an invalid ClientRectangle, then we will simply not draw the button for a frame or two until the system has initialized.
				//A couple of frames later the system should return to normal operation and we will be able to draw the button again.
			}
			// Calling the base class OnPaint
			base.OnPaint(pe);
		}

		/// <summary>Draws one button. isHot: Is the mouse currently hovering over this button. isPressed: Is the left mouse button currently down on this button. isSelected: Is this the currently selected button</summary>
		private void DrawButton(ModuleBarButton button, bool isHot, bool isPressed, bool isSelected, Graphics g)
		{
			if (!button.Visible)
			{
				g.FillRectangle(_brushBack, button.Bounds.X, button.Bounds.Y
					, button.Bounds.Width + 1, button.Bounds.Height + 1);
				return;
			}
			if (isPressed)
			{
				g.FillRectangle(_brushPressed, button.Bounds.X, button.Bounds.Y, button.Bounds.Width + 1, button.Bounds.Height + 1);
			}
			else if (isSelected)
			{
				g.FillRectangle(_brushSelected, button.Bounds.X, button.Bounds.Y, button.Bounds.Width + 1, button.Bounds.Height + 1);
				g.FillRectangle(button.BrushHot, button.Bounds.X, button.Bounds.Y + button.Bounds.Height - 10, button.Bounds.Width + 1, 10);
			}
			else if (isHot)
			{
				g.FillRectangle(_brushHot, button.Bounds.X, button.Bounds.Y, button.Bounds.Width + 1, button.Bounds.Height + 1);
				//g.FillRectangle(myButton.BrushHot,myButton.Bounds.X,myButton.Bounds.Y+myButton.Bounds.Height-10,myButton.Bounds.Width+1,10);
			}
			else
			{
				g.FillRectangle(_brushBack, button.Bounds.X, button.Bounds.Y, button.Bounds.Width + 1, button.Bounds.Height + 1);
			}
			//outline
			if (isPressed || isSelected || isHot)
			{
				//block out the corners so they won't show.  This can be improved later.
				g.FillPolygon(_brushBack, new Point[] {
				new Point(button.Bounds.X,button.Bounds.Y),
				new Point(button.Bounds.X+3,button.Bounds.Y),
				new Point(button.Bounds.X,button.Bounds.Y+3)});
				g.FillPolygon(_brushBack, new Point[] {//it's one pixel to the right because of the way rect drawn.
				new Point(button.Bounds.X+button.Bounds.Width-2,button.Bounds.Y),
				new Point(button.Bounds.X+button.Bounds.Width+1,button.Bounds.Y),
				new Point(button.Bounds.X+button.Bounds.Width+1,button.Bounds.Y+3)});
				g.FillPolygon(_brushBack, new Point[] {//it's one pixel down and right.
				new Point(button.Bounds.X+button.Bounds.Width+1,button.Bounds.Y+button.Bounds.Height-3),
				new Point(button.Bounds.X+button.Bounds.Width+1,button.Bounds.Y+button.Bounds.Height+1),
				new Point(button.Bounds.X+button.Bounds.Width-3,button.Bounds.Y+button.Bounds.Height+1)});
				g.FillPolygon(_brushBack, new Point[] {//it's one pixel down
				new Point(button.Bounds.X,button.Bounds.Y+button.Bounds.Height-3),
				new Point(button.Bounds.X+3,button.Bounds.Y+button.Bounds.Height+1),
				new Point(button.Bounds.X,button.Bounds.Y+button.Bounds.Height+1)});
				//then draw outline
				GraphicsHelper.DrawRoundedRectangle(g, _penOutline, button.Bounds, _radiusCorner);
			}
			//Image
			Rectangle imgRect = new Rectangle((Width - Dpi.Scale(this, 32)) / 2, button.Bounds.Y + 3, Dpi.Scale(this, 32), Dpi.Scale(this, 32));
			if (button.Image != null)
			{
				ODException.SwallowAnyException(() =>
				{
					g.DrawImage(button.Image, imgRect);
				});
			}
			//Text
			Rectangle textRect = new Rectangle(button.Bounds.X - 1, imgRect.Bottom + 3, button.Bounds.Width + 2, button.Bounds.Bottom - imgRect.Bottom + 3);
			_font?.Dispose();
			_font = new Font("Arial", Dpi.Scale(this, 8));
			g.DrawString(button.Caption, _font, Brushes.Black, textRect, _stringFormat);
		}
		#endregion Methods - OnPaint

		#region Methods - Override
		///<summary></summary>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(e);
			//Graphics g=this.CreateGraphics();
			if (currentHot != -1)
			{
				//redraw current button to give feedback on mouse down.
				Invalidate(); //just invalidate to force a repaint
			}
		}

		///<summary></summary>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
			if (currentHot != -1)
			{
				//undraw previous button
				Invalidate(); //just invalidate to force a repaint.
			}
			currentHot = -1;
		}

		///<summary></summary>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			int hotBut = GetButtonI(new Point(e.X, e.Y));
			if (hotBut != currentHot)
			{
				Invalidate(); //just invalidate to force a repaint.
				currentHot = hotBut;
			}
		}

		//<summary></summary>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			int selectedBut = GetButtonI(new Point(e.X, e.Y));
			if (selectedBut == -1)
			{
				return;
			}
			if (!_listButtons[selectedBut].Visible)
			{
				return;
			}
			//int oldSelected=SelectedIndex;
			_selectedIndexPrevious = _selectedIndex;
			_selectedIndex = selectedBut;
			Invalidate(); //just invalidate to force a repaint
			OnButtonClicked(_listButtons[_selectedIndex], false);
		}

		///<summary></summary>
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);
			//CalculateButtonInfo();
			Invalidate();
		}
		#endregion Methods - Override

		#region Methods - Private
		private void ComputeButtonSizes()
		{
			// Calculates button sizes and maybe more later
			//int barTop = 1;
			using (Graphics g = this.CreateGraphics())
			{
				int top = 0;
				int width = this.Width - 2;
				//int textHeight=0;
				for (int i = 0; i < _listButtons.Count; i++)
				{
					//--- Look if multiline text, if is add extra Height to button.
					//SizeF textSize = g.MeasureString(Buttons[i].Caption,textFont,width+2);
					//textHeight = (int)(Math.Ceiling(textSize.Height));
					//if(textHeight<26)
					//	textHeight=26;//default to height of 2 lines of text for uniformity.
					_listButtons[i].Bounds = new Rectangle(0, top, width, Dpi.Scale(this, _heightButton));//39+26);
					_listButtons[i].BrushHot?.Dispose();
					_listButtons[i].BrushHot = new LinearGradientBrush(new PointF(0, top + Dpi.Scale(this, _heightButton) - 10), new PointF(0, top + Dpi.Scale(this, _heightButton)),
						_brushSelected.Color, _brushPressed.Color);
					top += Dpi.Scale(this, _heightButton) + 1;//39+26+1;
				}//for
			}//using
		}

		private int GetButtonI(Point myPoint)
		{
			for (int i = 0; i < _listButtons.Count; i++)
			{
				//Item item = activeBar.Items[it];
				if (_listButtons[i].Bounds.Contains(myPoint))
				{
					return i;
				}
			}//for
			return -1;
		}

		private Image GetImage(EnumModuleType moduleType, bool isMedical = false)
		{
			return moduleType switch
			{
				EnumModuleType.Appointments => Resources.Icon32CalendarAlt,
				EnumModuleType.None => null,
				EnumModuleType.Family => Resources.Icon32Users,
				EnumModuleType.Account => Resources.Icon32MoneyCheckAlt,
				EnumModuleType.TreatPlan => Resources.Icon32ClipboardList,
				EnumModuleType.Chart => Resources.Icon32Tooth,
				EnumModuleType.Images => Resources.Icon32Images,
				EnumModuleType.Manage => Resources.Icon32Cogs,
				_ => null,
			};
		}


		#endregion Methods - Private



	}

	#region Enum
	/// <summary>There is no relationship between the underlying enum values and the idx of each module.  These numbers are not stored in the database and may be freely changed with new versions.  Idx numbers, by contrast, might be stored in db sometimes, although I have not yet found an instance.</summary>
	public enum EnumModuleType
	{
		None,
		Appointments,
		Family,
		Account,
		TreatPlan,
		Chart,
		Images,
		//EcwChart and/or TP?,
		Manage
	}
	#endregion Enum

	#region Class ModuleButton
	///<summary>Lightweight, just to keep track of a few fields.</summary>
	public class ModuleBarButton
	{
		///<summary></summary>
		public ModuleBarButton(EnumModuleType moduleType, string caption, Image image)
		{
			Caption = caption;
			ModuleType = moduleType;
			Image = image;
		}

		///<summary>Linear gradient brush depends on a start and stop Y points, so it must be different for every button unless we start having module buttons draw themselves.</summary>
		public LinearGradientBrush BrushHot;
		///<summary></summary>
		public Rectangle Bounds;
		///<summary></summary>
		public string Caption;
		///<summary></summary>
		public Image Image;
		///<summary></summary>
		public EnumModuleType ModuleType;
		///<summary></summary>
		public bool Visible = true;

	}
	#endregion Class ModuleButton

	#region EventArgs
	///<summary></summary>
	public class ButtonClicked_EventArgs
	{
		private ModuleBarButton outlookButton;
		private bool cancel;

		///<summary></summary>
		public ButtonClicked_EventArgs(ModuleBarButton myButton, bool myCancel)
		{
			outlookButton = myButton;
		}

		///<summary></summary>
		public ModuleBarButton OutlookButton
		{
			get
			{
				return outlookButton;
			}
		}

		///<summary>Set true to cancel the event.</summary>
		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}
	}

	///<summary></summary>
	public delegate void ButtonClickedEventHandler(object sender, ButtonClicked_EventArgs e);
	#endregion EventArgs
}
