using CodeBase;
using Imedisoft.Forms;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.UI
{
	/// <summary>
	/// A quick entry form for various purposes. You can put several different types of controls on this form.
	/// </summary>
	public partial class InputBox : FormBase
	{



		private List<InputBoxParam> _listInputParams;
		private List<Control> _listInputControls;
		private bool _hasTimeout = false;
		private Func<string, bool> _onOkClick;

		#region Properties - Public
		public int MaxInputTextLength
		{
			get
			{
				if (textResult == null)
				{
					return -1;
				}
				return textResult.MaxLength;
			}
			set
			{
				if (textResult != null)
				{
					textResult.MaxLength = value;
				}
			}
		}

		public bool IsDeleteClicked { get; set; }

		public bool ShowDelete
		{
			get
			{
				return deleteButton.Visible;
			}
			set
			{
				deleteButton.Visible = value;
			}
		}

		public TextBox textResult
		{
			get
			{
				return (TextBox)_listInputControls.FirstOrDefault(x => x is TextBox);
			}
		}

		public ComboBoxPlus comboSelection
		{
			get
			{
				return (ComboBoxPlus)_listInputControls.FirstOrDefault(x => x is ComboBoxPlus);
			}
		}

		public DateTime DateEntered
		{
			get
			{
				return PIn.Date(((ValidDate)_listInputControls.FirstOrDefault(x => x is ValidDate)).Text);
			}
		}

		public TimeSpan TimeEntered
		{
			get
			{
				return PIn.Date(((ValidTime)_listInputControls.FirstOrDefault(x => x is ValidTime)).Text).TimeOfDay;
			}
		}

		public int SelectedIndex
		{
			get
			{
				if (SelectedIndices.Count < 1)
				{
					return -1;
				}
				return SelectedIndices[0];
			}
		}

		public CheckBox checkBoxResult
		{
			get
			{
				return (CheckBox)_listInputControls.FirstOrDefault(x => x is CheckBox);
			}
		}

		public List<int> SelectedIndices
		{
			get
			{
				Control comboBox = _listInputControls.FirstOrDefault(x => x is ComboBoxPlus || x is ListBox);
				if (comboBox == null)
				{
					return new List<int>();
				}
				if (comboBox is ComboBoxPlus)
				{
					return ((ComboBoxPlus)comboBox).SelectedIndices;
				}
				if (comboBox is ListBox)
				{
					return ((ListBox)comboBox).SelectedIndices.Cast<int>().ToList();
				}
				return new List<int>();
			}
		}

		///<summary>Returns the text that was entered in the textboxes. Will be in the order they were passed into the constructor.</summary>
		public List<string> ListTextEntered
		{
			get
			{
				//Includes ValidDate and ValidTime since they inherit from TextBox.
				return _listInputControls.Where(x => x is TextBox).Select(x => ((TextBox)x).Text).ToList();
			}
		}
		#endregion Properties - Public

		#region Properties - Protected Override
		protected override bool HasHelpKey
		{
			get
			{
				return false;
			}
		}
		#endregion Properties - Protected Override

		#region Constructors
		///<summary>Creates a textbox with a label containing the given prompt.</summary>
		public InputBox(string prompt)
			: this(prompt, false)
		{

		}

		///<summary>Creates a textbox with a label containing the given prompt, with a default string in the text field.</summary>
		public InputBox(string prompt, string defaultText)
			: this(prompt, false, defaultText, Size.Empty)
		{

		}

		public InputBox(string prompt, string defaultText, bool hasTimeout, Point position)
			: this(new List<InputBoxParam> { new InputBoxParam(InputBoxType.CheckBox, prompt, defaultText, hasTimeout, position) })
		{

		}

		///<summary>Creates a textbox with a label containing the given prompt.</summary>
		public InputBox(string prompt, bool isMultiLine)
			: this(new List<InputBoxParam> { new InputBoxParam(isMultiLine ? InputBoxType.TextBoxMultiLine : InputBoxType.TextBox, prompt) })
		{

		}

		///<summary>Creates a textbox with a label containing the given prompt, with a default string in the text field.</summary>
		public InputBox(string prompt, bool isMultiLine, string defaultText, Size paramSize)
			: this(new List<InputBoxParam> { new InputBoxParam(isMultiLine ? InputBoxType.TextBoxMultiLine : InputBoxType.TextBox, prompt, defaultText, paramSize) })
		{

		}

		///<summary>This constructor allows a list of strings to be sent in and fill a comboBox for users to select from.</summary>
		public InputBox(string prompt, List<string> listSelections)
			: this(prompt, listSelections, false)
		{

		}

		///<summary>This constructor allows a list of strings to be sent in and fill a comboBox for users to select from.</summary>
		public InputBox(string prompt, List<string> listSelections, int selectedIndex)
			: this(new List<InputBoxParam> {
					new InputBoxParam(InputBoxType.ComboSelect,prompt,listSelections,listSelectedIndices:new List<int> { selectedIndex }) })
		{
		}

		///<summary>This constructor allows a list of strings to be sent in and fill a listbox or comboBox for users to select from.</summary>
		public InputBox(string prompt, List<string> listSelections, bool isMultiSelect)
			: this(new List<InputBoxParam> {
					new InputBoxParam(isMultiSelect ? (listSelections.Count>=10 ? InputBoxType.ComboMultiSelect : InputBoxType.ListBoxMulti) : InputBoxType.ComboSelect,prompt,listSelections) })
		{

		}

		public InputBox(Func<string, bool> onOkClick, params InputBoxParam[] arrayInputBoxParams)
			: this(arrayInputBoxParams.ToList(), onOkClick)
		{

		}

		///<summary>Use this constructor to create multiple input controls.</summary>
		public InputBox(List<InputBoxParam> listInputBoxParams, Func<string, bool> onOkClick = null)
		{
			InitializeComponent();
			_listInputParams = listInputBoxParams;

			AddInputControls();
			_onOkClick = onOkClick;
		}
		#endregion Constructors



		///<summary>Adds the requested controls to the form.</summary>
		private void AddInputControls()
		{
			_listInputControls = new List<Control>();
			List<Label> listLabels = new List<Label>();
			int curLocationY = 2;
			int controlWidth = 385;
			int minWidth = 250;
			int posX = 32;
			int itemOrder = 1;
			foreach (InputBoxParam inputParam in _listInputParams)
			{
				if (inputParam == null)
				{
					continue;
				}
				if (!string.IsNullOrEmpty(inputParam.LabelText))
				{
					Label label = new Label();
					label.AutoSize = false;
					label.Size = new Size(inputParam.ParamSize == Size.Empty ? controlWidth : inputParam.ParamSize.Width, 36);
					label.Text = inputParam.LabelText;
					label.Name = "labelPrompt" + itemOrder;
					label.TextAlign = ContentAlignment.BottomLeft;
					label.Location = new Point(posX, curLocationY);
					label.Tag = inputParam;
					listLabels.Add(label);
					curLocationY += 38;
				}
				Control inputControl;
				switch (inputParam.ParamType)
				{
					case InputBoxType.TextBox:
						TextBox textBox = new TextBox();
						textBox.Name = "textBox" + itemOrder;
						textBox.Location = new Point(posX, curLocationY);
						textBox.Size = new Size(controlWidth, 20);
						textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
						textBox.Text = inputParam.Text;
						if (!String.IsNullOrEmpty(textBox.Text))
						{
							textBox.SelectionStart = 0;
							textBox.SelectionLength = textBox.Text.Length;
						}
						inputControl = textBox;
						curLocationY += 22;
						break;
					case InputBoxType.TextBoxMultiLine:
						TextBox textBoxMulti = new TextBox();
						textBoxMulti.Name = "textBox" + itemOrder;
						textBoxMulti.Location = new Point(posX, curLocationY);
						textBoxMulti.Size = new Size(controlWidth, 100);
						textBoxMulti.Multiline = true;
						textBoxMulti.Text = inputParam.Text;
						this.AcceptButton = null;
						textBoxMulti.ScrollBars = ScrollBars.Vertical;
						inputControl = textBoxMulti;
						curLocationY += 102;
						break;
					case InputBoxType.CheckBox:
						CheckBox checkBox = new CheckBox();
						checkBox.Name = "checkBox" + itemOrder;
						checkBox.Location = new Point(posX + inputParam.Position.X, curLocationY + inputParam.Position.Y);
						checkBox.Size = inputParam.ParamSize == Size.Empty ? new Size(controlWidth, 20) : inputParam.ParamSize;
						checkBox.Text = inputParam.Text;
						checkBox.FlatStyle = FlatStyle.System;
						inputControl = checkBox;
						if (inputParam.HasTimeout)
						{
							_hasTimeout = true;
						}
						curLocationY += checkBox.Size.Height + 2;
						break;
					case InputBoxType.ComboSelect:
						OpenDental.UI.ComboBoxPlus comboBoxPlus = new OpenDental.UI.ComboBoxPlus();
						comboBoxPlus.Name = "comboBox" + itemOrder;
						comboBoxPlus.Location = new Point(posX, curLocationY);
						comboBoxPlus.Size = inputParam.ParamSize == Size.Empty ? new Size(controlWidth, 21) : inputParam.ParamSize;
						comboBoxPlus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
						comboBoxPlus.Items.AddList<string>(inputParam.ListSelections, x => x);
						if (inputParam.ListSelectedIndices.Count > 0 && inputParam.ListSelectedIndices[0].Between(0, comboBoxPlus.Items.Count - 1))
						{
							comboBoxPlus.SetSelected(inputParam.ListSelectedIndices[0]);//If there is a valid initial selection, select it.
						}
						inputControl = comboBoxPlus;
						curLocationY += 23;
						break;
					case InputBoxType.ComboMultiSelect:
						OpenDental.UI.ComboBoxPlus comboBoxPlus2 = new OpenDental.UI.ComboBoxPlus();
						comboBoxPlus2.SelectionModeMulti = true;
						comboBoxPlus2.Name = "comboBox" + itemOrder;
						comboBoxPlus2.Location = new Point(posX, curLocationY);
						comboBoxPlus2.Size = new Size(controlWidth, 21);
						comboBoxPlus2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
						comboBoxPlus2.BackColor = SystemColors.Window;
						foreach (string selection in inputParam.ListSelections)
						{
							comboBoxPlus2.Items.Add(selection);
						}
						foreach (int selection in inputParam.ListSelectedIndices)
						{
							if (selection.Between(0, comboBoxPlus2.Items.Count - 1))
							{
								comboBoxPlus2.SetSelected(selection);//If there is a valid initial selection, select it.
							}
						}
						inputControl = comboBoxPlus2;
						curLocationY += 23;
						break;
					case InputBoxType.ValidDate:
						ValidDate validDate = new ValidDate();
						validDate.Name = "validDate" + itemOrder;
						validDate.Location = new Point(posX, curLocationY);
						validDate.Size = new Size(100, 20);
						validDate.Text = inputParam.Text;
						inputControl = validDate;
						Label label = new Label();
						label.Size = new Size(label.Width, validDate.Height);
						label.Text = $"({CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern})";
						label.Name = "labelDateFormat" + itemOrder;
						label.TextAlign = ContentAlignment.MiddleLeft;
						label.Location = new Point(validDate.Location.X + validDate.Width + 12, curLocationY);
						label.Tag = inputParam;
						listLabels.Add(label);
						curLocationY += 22;
						break;
					case InputBoxType.ValidTime:
						ValidTime validTime = new ValidTime();
						validTime.Name = "validTime" + itemOrder;
						validTime.Location = new Point(posX, curLocationY);
						validTime.Size = new Size(120, 20);
						inputControl = validTime;
						curLocationY += 22;
						break;
					case InputBoxType.ValidDouble:
						ValidDouble validDouble = new ValidDouble();
						validDouble.Name = "validDouble" + itemOrder;
						validDouble.Location = new Point(posX, curLocationY);
						validDouble.Size = new Size(120, 20);
						inputControl = validDouble;
						curLocationY += 22;
						break;
					case InputBoxType.ValidPhone:
						ValidPhone validPhone = new ValidPhone();
						validPhone.Name = "validPhone" + itemOrder;
						validPhone.Location = new Point(posX, curLocationY);
						validPhone.Size = new Size(140, 20);
						validPhone.Text = inputParam.Text;
						if (!String.IsNullOrEmpty(validPhone.Text))
						{
							validPhone.SelectionStart = 0;
							validPhone.SelectionLength = validPhone.Text.Length;
						}
						inputControl = validPhone;
						curLocationY += 22;
						break;
					case InputBoxType.ListBoxMulti:
						ListBox listBox = new ListBox();
						listBox.Name = "listBox" + itemOrder;
						listBox.Location = new Point(posX, curLocationY);
						listBox.BackColor = SystemColors.Window;
						listBox.SelectionMode = SelectionMode.MultiSimple;
						foreach (string selection in inputParam.ListSelections)
						{
							listBox.Items.Add(selection);
						}
						listBox.Size = new Size(controlWidth, listBox.PreferredHeight);
						inputControl = listBox;
						curLocationY += (listBox.PreferredHeight) + 2;
						break;
					default:
						throw new NotImplementedException("InputBoxType: " + inputParam.ParamType + " not implemented.");
				}
				inputControl.TabIndex = itemOrder;
				inputControl.Tag = inputParam;
				_listInputControls.Add(inputControl);
				minWidth = Math.Max(minWidth, inputControl.Width + 80);
			}
			//Now that we know the minWidth, we can center any controls that need to be centered.
			foreach (Control inputControl in _listInputControls.Union(listLabels))
			{
				InputBoxParam inputParam = (InputBoxParam)inputControl.Tag;
				if (inputParam.HorizontalAlign != HorizontalAlignment.Left)
				{
					inputControl.Location = new Point((minWidth - inputControl.Width) / 2, inputControl.Location.Y);
				}
			}
			Controls.AddRange(listLabels.ToArray());
			Controls.AddRange(_listInputControls.ToArray());
			Height = curLocationY + 90;
			Width = minWidth;
		}

		private void InputBox_Shown(object sender, EventArgs e)
		{
			if (_timer != null && _hasTimeout)
			{
				_timer.Enabled = true;
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			_timer.Enabled = false;
			_timer.Dispose();
			DialogResult = DialogResult.Abort;
		}

		public void setTitle(string title)
		{
			this.Text = title;
		}

		private void butDelete_Click(object sender, EventArgs e)
		{
			IsDeleteClicked = true;
			DialogResult = DialogResult.OK;
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (_listInputControls.Where(x => x is ValidDate)
				.Any(x => ((ValidDate)x).errorProvider1.GetError(x) != "")
				|| _listInputControls.Where(x => x is ValidTime)
				.Any(x => !((ValidTime)x).IsValid))
			{
				ShowError("Please fix data entry errors first.");
				return;
			}

			if (_listInputControls.OfType<ComboBoxPlus>().Any(x => !x.SelectionModeMulti && x.SelectedIndex == -1))
			{//single selection
				ShowError("Please make a selection.");
				return;
			}

			if (_listInputControls.OfType<ComboBoxPlus>().Any(x => x.SelectionModeMulti && x.SelectedIndices.Count == 0))
			{//multi selection
				ShowError("Please make at least one selection.");
				return;
			}

			if (_listInputControls.OfType<CheckBox>().ToList().Count > 1 && _listInputControls.OfType<CheckBox>().Where(x => x.Checked).Count() == 0)
			{
				ShowError("Please make a selection.");
				return;
			}

			if (_listInputControls.OfType<CheckBox>().ToList().Count > 1 && _listInputControls.OfType<CheckBox>().Where(x => x.Checked).Count() > 1)
			{
				ShowError("Can only make one selection.");
				return;
			}

			if (_onOkClick != null && !_onOkClick(textResult.Text))
			{
				//It is up to the implementor for _onOkClick to handle any messages to portray to the user.
				//We will simply block the user from closing the window.
				return;
			}
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}

	public class InputBoxParam
	{
		public InputBoxType ParamType;
		public string LabelText;
		public List<string> ListSelections = new List<string>();
		public string Text = "";
		public List<int> ListSelectedIndices = new List<int>();
		public bool HasTimeout;
		public Point Position;
		public Size ParamSize;
		public HorizontalAlignment HorizontalAlign = HorizontalAlignment.Left;

		public InputBoxParam()
		{

		}

		public InputBoxParam(InputBoxType paramType, string labelText, string text, Size paramSize)
		{
			LabelText = labelText;
			ParamType = paramType;
			Text = text;
			ParamSize = paramSize;
		}

		public InputBoxParam(InputBoxType paramType, string labelText, string text, bool hasTimeout, Point position)
		{
			LabelText = labelText;
			ParamType = paramType;
			Text = text;
			HasTimeout = hasTimeout;
			Position = position;
		}

		///<summary>If InputBoxType = ComboSelect or ComboMultiSelect, listComboSelections will be the items in the combobox.  If InputBoxType = TextBox or TextBoxMultiLine, textBoxText will be the default text in that text box. listSelectedIndices actually only works for single select.</summary>
		public InputBoxParam(InputBoxType paramType, string labelText, List<string> listComboSelections = null, string text = null,
			List<int> listSelectedIndices = null)
		{
			LabelText = labelText;
			ParamType = paramType;
			if (listComboSelections != null)
			{
				ListSelections = listComboSelections;
			}
			if (!string.IsNullOrEmpty(text))
			{
				Text = text;
			}
			if (listSelectedIndices != null)
			{
				ListSelectedIndices = listSelectedIndices;
			}
		}
	}

	public enum InputBoxType
	{
		TextBox,
		TextBoxMultiLine,
		ComboSelect,
		ComboMultiSelect,
		ValidDate,
		ValidTime,
		ValidDouble,
		ValidPhone,
		CheckBox,
		ListBoxMulti,
	}
}
