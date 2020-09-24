namespace Imedisoft.Forms
{
	partial class FormEhrPatientVaccineEdit
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.manufacturerLabel = new System.Windows.Forms.Label();
            this.amountLabel = new System.Windows.Forms.Label();
            this.amountTextBox = new System.Windows.Forms.TextBox();
            this.vaccineComboBox = new System.Windows.Forms.ComboBox();
            this.vaccineLabel = new System.Windows.Forms.Label();
            this.unitsComboBox = new System.Windows.Forms.ComboBox();
            this.manufacturerTextBox = new System.Windows.Forms.TextBox();
            this.dateStartLabel = new System.Windows.Forms.Label();
            this.dateStartTextBox = new System.Windows.Forms.TextBox();
            this.lotNumberLabel = new System.Windows.Forms.Label();
            this.lotNumberTextBox = new System.Windows.Forms.TextBox();
            this.dateStopTextBox = new System.Windows.Forms.TextBox();
            this.dateStopLabel = new System.Windows.Forms.Label();
            this.documentLabel = new System.Windows.Forms.Label();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.filledCityTextBox = new System.Windows.Forms.TextBox();
            this.filledCityLabel = new System.Windows.Forms.Label();
            this.filledStateTextBox = new System.Windows.Forms.TextBox();
            this.filledStateLabel = new System.Windows.Forms.Label();
            this.completionStatusComboBox = new System.Windows.Forms.ComboBox();
            this.completionStatusLabel = new System.Windows.Forms.Label();
            this.informationSourceLabel = new System.Windows.Forms.Label();
            this.informationSourceComboBox = new System.Windows.Forms.ComboBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.orderedByComboBox = new System.Windows.Forms.ComboBox();
            this.orderedByLabel = new System.Windows.Forms.Label();
            this.administeredByComboBox = new System.Windows.Forms.ComboBox();
            this.administeredByLabel = new System.Windows.Forms.Label();
            this.expirationDateLabel = new System.Windows.Forms.Label();
            this.refusalReasonLabel = new System.Windows.Forms.Label();
            this.refusalReasonComboBox = new System.Windows.Forms.ComboBox();
            this.actionLabel = new System.Windows.Forms.Label();
            this.actionComboBox = new System.Windows.Forms.ComboBox();
            this.administrationRouteLabel = new System.Windows.Forms.Label();
            this.administrationRouteComboBox = new System.Windows.Forms.ComboBox();
            this.administrationSiteComboBox = new System.Windows.Forms.ComboBox();
            this.administrationSiteLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.observationsGrid = new OpenDental.UI.ODGrid();
            this.ungroupButton = new OpenDental.UI.Button();
            this.groupButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.administeredByClearButton = new OpenDental.UI.Button();
            this.orderedByClearButton = new OpenDental.UI.Button();
            this.expirationDateTextBox = new OpenDental.ValidDate();
            this.administeredByPickButton = new OpenDental.UI.Button();
            this.orderedByPickButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // manufacturerLabel
            // 
            this.manufacturerLabel.AutoSize = true;
            this.manufacturerLabel.Location = new System.Drawing.Point(62, 42);
            this.manufacturerLabel.Name = "manufacturerLabel";
            this.manufacturerLabel.Size = new System.Drawing.Size(72, 13);
            this.manufacturerLabel.TabIndex = 2;
            this.manufacturerLabel.Text = "Manufacturer";
            // 
            // amountLabel
            // 
            this.amountLabel.AutoSize = true;
            this.amountLabel.Location = new System.Drawing.Point(90, 120);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(44, 13);
            this.amountLabel.TabIndex = 8;
            this.amountLabel.Text = "Amount";
            // 
            // amountTextBox
            // 
            this.amountTextBox.Location = new System.Drawing.Point(140, 117);
            this.amountTextBox.Name = "amountTextBox";
            this.amountTextBox.Size = new System.Drawing.Size(60, 20);
            this.amountTextBox.TabIndex = 9;
            this.amountTextBox.Text = "0";
            // 
            // vaccineComboBox
            // 
            this.vaccineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vaccineComboBox.FormattingEnabled = true;
            this.vaccineComboBox.Location = new System.Drawing.Point(140, 12);
            this.vaccineComboBox.Name = "vaccineComboBox";
            this.vaccineComboBox.Size = new System.Drawing.Size(200, 21);
            this.vaccineComboBox.TabIndex = 1;
            this.vaccineComboBox.SelectedIndexChanged += new System.EventHandler(this.VaccineComboBox_SelectedIndexChanged);
            // 
            // vaccineLabel
            // 
            this.vaccineLabel.AutoSize = true;
            this.vaccineLabel.Location = new System.Drawing.Point(91, 15);
            this.vaccineLabel.Name = "vaccineLabel";
            this.vaccineLabel.Size = new System.Drawing.Size(43, 13);
            this.vaccineLabel.TabIndex = 0;
            this.vaccineLabel.Text = "Vaccine";
            // 
            // unitsComboBox
            // 
            this.unitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitsComboBox.FormattingEnabled = true;
            this.unitsComboBox.Location = new System.Drawing.Point(206, 117);
            this.unitsComboBox.Name = "unitsComboBox";
            this.unitsComboBox.Size = new System.Drawing.Size(100, 21);
            this.unitsComboBox.TabIndex = 10;
            // 
            // manufacturerTextBox
            // 
            this.manufacturerTextBox.Location = new System.Drawing.Point(140, 39);
            this.manufacturerTextBox.Name = "manufacturerTextBox";
            this.manufacturerTextBox.ReadOnly = true;
            this.manufacturerTextBox.Size = new System.Drawing.Size(200, 20);
            this.manufacturerTextBox.TabIndex = 3;
            this.manufacturerTextBox.TabStop = false;
            // 
            // dateStartLabel
            // 
            this.dateStartLabel.AutoSize = true;
            this.dateStartLabel.Location = new System.Drawing.Point(52, 68);
            this.dateStartLabel.Name = "dateStartLabel";
            this.dateStartLabel.Size = new System.Drawing.Size(82, 13);
            this.dateStartLabel.TabIndex = 4;
            this.dateStartLabel.Text = "Date Time Start";
            // 
            // dateStartTextBox
            // 
            this.dateStartTextBox.Location = new System.Drawing.Point(140, 65);
            this.dateStartTextBox.Name = "dateStartTextBox";
            this.dateStartTextBox.Size = new System.Drawing.Size(120, 20);
            this.dateStartTextBox.TabIndex = 5;
            // 
            // lotNumberLabel
            // 
            this.lotNumberLabel.AutoSize = true;
            this.lotNumberLabel.Location = new System.Drawing.Point(72, 146);
            this.lotNumberLabel.Name = "lotNumberLabel";
            this.lotNumberLabel.Size = new System.Drawing.Size(62, 13);
            this.lotNumberLabel.TabIndex = 11;
            this.lotNumberLabel.Text = "Lot Number";
            // 
            // lotNumberTextBox
            // 
            this.lotNumberTextBox.Location = new System.Drawing.Point(140, 143);
            this.lotNumberTextBox.Name = "lotNumberTextBox";
            this.lotNumberTextBox.Size = new System.Drawing.Size(120, 20);
            this.lotNumberTextBox.TabIndex = 12;
            // 
            // dateStopTextBox
            // 
            this.dateStopTextBox.Location = new System.Drawing.Point(140, 91);
            this.dateStopTextBox.Name = "dateStopTextBox";
            this.dateStopTextBox.Size = new System.Drawing.Size(120, 20);
            this.dateStopTextBox.TabIndex = 7;
            // 
            // dateStopLabel
            // 
            this.dateStopLabel.AutoSize = true;
            this.dateStopLabel.Location = new System.Drawing.Point(54, 94);
            this.dateStopLabel.Name = "dateStopLabel";
            this.dateStopLabel.Size = new System.Drawing.Size(80, 13);
            this.dateStopLabel.TabIndex = 6;
            this.dateStopLabel.Text = "Date Time Stop";
            // 
            // documentLabel
            // 
            this.documentLabel.Location = new System.Drawing.Point(140, 246);
            this.documentLabel.Name = "documentLabel";
            this.documentLabel.Size = new System.Drawing.Size(336, 60);
            this.documentLabel.TabIndex = 19;
            this.documentLabel.Text = "Document reason not given below.  Reason can include a specific allergy, adverse " +
    "effect, intollerance, patient declines, specific disease, etc.";
            // 
            // noteTextBox
            // 
            this.noteTextBox.Location = new System.Drawing.Point(140, 309);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.Size = new System.Drawing.Size(336, 75);
            this.noteTextBox.TabIndex = 21;
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(104, 312);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(30, 13);
            this.noteLabel.TabIndex = 20;
            this.noteLabel.Text = "Note";
            // 
            // filledCityTextBox
            // 
            this.filledCityTextBox.Location = new System.Drawing.Point(580, 12);
            this.filledCityTextBox.Name = "filledCityTextBox";
            this.filledCityTextBox.Size = new System.Drawing.Size(200, 20);
            this.filledCityTextBox.TabIndex = 23;
            // 
            // filledCityLabel
            // 
            this.filledCityLabel.AutoSize = true;
            this.filledCityLabel.Location = new System.Drawing.Point(486, 15);
            this.filledCityLabel.Name = "filledCityLabel";
            this.filledCityLabel.Size = new System.Drawing.Size(88, 13);
            this.filledCityLabel.TabIndex = 22;
            this.filledCityLabel.Text = "City Where Filled";
            // 
            // filledStateTextBox
            // 
            this.filledStateTextBox.Location = new System.Drawing.Point(580, 38);
            this.filledStateTextBox.Name = "filledStateTextBox";
            this.filledStateTextBox.Size = new System.Drawing.Size(200, 20);
            this.filledStateTextBox.TabIndex = 25;
            // 
            // filledStateLabel
            // 
            this.filledStateLabel.AutoSize = true;
            this.filledStateLabel.Location = new System.Drawing.Point(479, 41);
            this.filledStateLabel.Name = "filledStateLabel";
            this.filledStateLabel.Size = new System.Drawing.Size(95, 13);
            this.filledStateLabel.TabIndex = 24;
            this.filledStateLabel.Text = "State Where Filled";
            // 
            // completionStatusComboBox
            // 
            this.completionStatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.completionStatusComboBox.FormattingEnabled = true;
            this.completionStatusComboBox.IntegralHeight = false;
            this.completionStatusComboBox.Location = new System.Drawing.Point(140, 195);
            this.completionStatusComboBox.Name = "completionStatusComboBox";
            this.completionStatusComboBox.Size = new System.Drawing.Size(200, 21);
            this.completionStatusComboBox.TabIndex = 16;
            // 
            // completionStatusLabel
            // 
            this.completionStatusLabel.AutoSize = true;
            this.completionStatusLabel.Location = new System.Drawing.Point(40, 198);
            this.completionStatusLabel.Name = "completionStatusLabel";
            this.completionStatusLabel.Size = new System.Drawing.Size(94, 13);
            this.completionStatusLabel.TabIndex = 15;
            this.completionStatusLabel.Text = "Completion Status";
            // 
            // informationSourceLabel
            // 
            this.informationSourceLabel.AutoSize = true;
            this.informationSourceLabel.Location = new System.Drawing.Point(475, 201);
            this.informationSourceLabel.Name = "informationSourceLabel";
            this.informationSourceLabel.Size = new System.Drawing.Size(99, 13);
            this.informationSourceLabel.TabIndex = 40;
            this.informationSourceLabel.Text = "Information Source";
            // 
            // informationSourceComboBox
            // 
            this.informationSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.informationSourceComboBox.FormattingEnabled = true;
            this.informationSourceComboBox.IntegralHeight = false;
            this.informationSourceComboBox.Location = new System.Drawing.Point(580, 198);
            this.informationSourceComboBox.Name = "informationSourceComboBox";
            this.informationSourceComboBox.Size = new System.Drawing.Size(300, 21);
            this.informationSourceComboBox.TabIndex = 41;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(545, 67);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(29, 13);
            this.userLabel.TabIndex = 26;
            this.userLabel.Text = "User";
            // 
            // orderedByComboBox
            // 
            this.orderedByComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderedByComboBox.Location = new System.Drawing.Point(580, 90);
            this.orderedByComboBox.MaxDropDownItems = 30;
            this.orderedByComboBox.Name = "orderedByComboBox";
            this.orderedByComboBox.Size = new System.Drawing.Size(240, 21);
            this.orderedByComboBox.TabIndex = 29;
            // 
            // orderedByLabel
            // 
            this.orderedByLabel.AutoSize = true;
            this.orderedByLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderedByLabel.Location = new System.Drawing.Point(515, 93);
            this.orderedByLabel.Name = "orderedByLabel";
            this.orderedByLabel.Size = new System.Drawing.Size(59, 13);
            this.orderedByLabel.TabIndex = 28;
            this.orderedByLabel.Text = "Ordered by";
            // 
            // administeredByComboBox
            // 
            this.administeredByComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.administeredByComboBox.Location = new System.Drawing.Point(580, 117);
            this.administeredByComboBox.MaxDropDownItems = 30;
            this.administeredByComboBox.Name = "administeredByComboBox";
            this.administeredByComboBox.Size = new System.Drawing.Size(240, 21);
            this.administeredByComboBox.TabIndex = 33;
            // 
            // administeredByLabel
            // 
            this.administeredByLabel.AutoSize = true;
            this.administeredByLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.administeredByLabel.Location = new System.Drawing.Point(493, 120);
            this.administeredByLabel.Name = "administeredByLabel";
            this.administeredByLabel.Size = new System.Drawing.Size(81, 13);
            this.administeredByLabel.TabIndex = 32;
            this.administeredByLabel.Text = "Administered by";
            // 
            // expirationDateLabel
            // 
            this.expirationDateLabel.AutoSize = true;
            this.expirationDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expirationDateLabel.Location = new System.Drawing.Point(55, 172);
            this.expirationDateLabel.Name = "expirationDateLabel";
            this.expirationDateLabel.Size = new System.Drawing.Size(79, 13);
            this.expirationDateLabel.TabIndex = 13;
            this.expirationDateLabel.Text = "Expiration Date";
            // 
            // refusalReasonLabel
            // 
            this.refusalReasonLabel.AutoSize = true;
            this.refusalReasonLabel.Location = new System.Drawing.Point(52, 225);
            this.refusalReasonLabel.Name = "refusalReasonLabel";
            this.refusalReasonLabel.Size = new System.Drawing.Size(82, 13);
            this.refusalReasonLabel.TabIndex = 17;
            this.refusalReasonLabel.Text = "Refusal Reason";
            // 
            // refusalReasonComboBox
            // 
            this.refusalReasonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.refusalReasonComboBox.FormattingEnabled = true;
            this.refusalReasonComboBox.IntegralHeight = false;
            this.refusalReasonComboBox.Location = new System.Drawing.Point(140, 222);
            this.refusalReasonComboBox.Name = "refusalReasonComboBox";
            this.refusalReasonComboBox.Size = new System.Drawing.Size(200, 21);
            this.refusalReasonComboBox.TabIndex = 18;
            // 
            // actionLabel
            // 
            this.actionLabel.AutoSize = true;
            this.actionLabel.Location = new System.Drawing.Point(537, 228);
            this.actionLabel.Name = "actionLabel";
            this.actionLabel.Size = new System.Drawing.Size(37, 13);
            this.actionLabel.TabIndex = 42;
            this.actionLabel.Text = "Action";
            // 
            // actionComboBox
            // 
            this.actionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actionComboBox.FormattingEnabled = true;
            this.actionComboBox.IntegralHeight = false;
            this.actionComboBox.Location = new System.Drawing.Point(580, 225);
            this.actionComboBox.Name = "actionComboBox";
            this.actionComboBox.Size = new System.Drawing.Size(150, 21);
            this.actionComboBox.TabIndex = 43;
            // 
            // administrationRouteLabel
            // 
            this.administrationRouteLabel.AutoSize = true;
            this.administrationRouteLabel.Location = new System.Drawing.Point(467, 147);
            this.administrationRouteLabel.Name = "administrationRouteLabel";
            this.administrationRouteLabel.Size = new System.Drawing.Size(107, 13);
            this.administrationRouteLabel.TabIndex = 36;
            this.administrationRouteLabel.Text = "Administration Route";
            // 
            // administrationRouteComboBox
            // 
            this.administrationRouteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.administrationRouteComboBox.FormattingEnabled = true;
            this.administrationRouteComboBox.Location = new System.Drawing.Point(580, 144);
            this.administrationRouteComboBox.Name = "administrationRouteComboBox";
            this.administrationRouteComboBox.Size = new System.Drawing.Size(200, 21);
            this.administrationRouteComboBox.TabIndex = 37;
            // 
            // administrationSiteComboBox
            // 
            this.administrationSiteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.administrationSiteComboBox.FormattingEnabled = true;
            this.administrationSiteComboBox.Location = new System.Drawing.Point(580, 171);
            this.administrationSiteComboBox.Name = "administrationSiteComboBox";
            this.administrationSiteComboBox.Size = new System.Drawing.Size(200, 21);
            this.administrationSiteComboBox.TabIndex = 39;
            // 
            // administrationSiteLabel
            // 
            this.administrationSiteLabel.AutoSize = true;
            this.administrationSiteLabel.Location = new System.Drawing.Point(478, 174);
            this.administrationSiteLabel.Name = "administrationSiteLabel";
            this.administrationSiteLabel.Size = new System.Drawing.Size(96, 13);
            this.administrationSiteLabel.TabIndex = 38;
            this.administrationSiteLabel.Text = "Administration Site";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(580, 64);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.ReadOnly = true;
            this.userTextBox.Size = new System.Drawing.Size(200, 20);
            this.userTextBox.TabIndex = 27;
            // 
            // observationsGrid
            // 
            this.observationsGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.observationsGrid.Location = new System.Drawing.Point(580, 269);
            this.observationsGrid.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.observationsGrid.Name = "observationsGrid";
            this.observationsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.observationsGrid.Size = new System.Drawing.Size(300, 120);
            this.observationsGrid.TabIndex = 44;
            this.observationsGrid.Title = "Observations";
            this.observationsGrid.TranslationName = "TableObservations";
            this.observationsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ObservationsGrid_CellDoubleClick);
            this.observationsGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.ObservationsGrid_CellClick);
            // 
            // ungroupButton
            // 
            this.ungroupButton.Location = new System.Drawing.Point(820, 395);
            this.ungroupButton.Name = "ungroupButton";
            this.ungroupButton.Size = new System.Drawing.Size(60, 25);
            this.ungroupButton.TabIndex = 49;
            this.ungroupButton.Text = "Ungroup";
            this.ungroupButton.Click += new System.EventHandler(this.UngroupButton_Click);
            // 
            // groupButton
            // 
            this.groupButton.Location = new System.Drawing.Point(754, 395);
            this.groupButton.Name = "groupButton";
            this.groupButton.Size = new System.Drawing.Size(60, 25);
            this.groupButton.TabIndex = 48;
            this.groupButton.Text = "Group";
            this.groupButton.Click += new System.EventHandler(this.GroupButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(580, 395);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 45;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(766, 484);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 50;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(852, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 51;
            this.cancelButton.Text = "&Cancel";
            // 
            // administeredByClearButton
            // 
            this.administeredByClearButton.Location = new System.Drawing.Point(857, 118);
            this.administeredByClearButton.Name = "administeredByClearButton";
            this.administeredByClearButton.Size = new System.Drawing.Size(50, 20);
            this.administeredByClearButton.TabIndex = 35;
            this.administeredByClearButton.Text = "None";
            this.administeredByClearButton.Click += new System.EventHandler(this.AdministeredByClearButton_Click);
            // 
            // orderedByClearButton
            // 
            this.orderedByClearButton.Location = new System.Drawing.Point(857, 90);
            this.orderedByClearButton.Name = "orderedByClearButton";
            this.orderedByClearButton.Size = new System.Drawing.Size(50, 20);
            this.orderedByClearButton.TabIndex = 31;
            this.orderedByClearButton.Text = "None";
            this.orderedByClearButton.Click += new System.EventHandler(this.OrderedByClearButton_Click);
            // 
            // expirationDateTextBox
            // 
            this.expirationDateTextBox.Location = new System.Drawing.Point(140, 169);
            this.expirationDateTextBox.Name = "expirationDateTextBox";
            this.expirationDateTextBox.Size = new System.Drawing.Size(80, 20);
            this.expirationDateTextBox.TabIndex = 14;
            // 
            // administeredByPickButton
            // 
            this.administeredByPickButton.Location = new System.Drawing.Point(826, 117);
            this.administeredByPickButton.Name = "administeredByPickButton";
            this.administeredByPickButton.Size = new System.Drawing.Size(25, 20);
            this.administeredByPickButton.TabIndex = 34;
            this.administeredByPickButton.Text = "...";
            this.administeredByPickButton.Click += new System.EventHandler(this.AdministeredByPickButton_Click);
            // 
            // orderedByPickButton
            // 
            this.orderedByPickButton.Location = new System.Drawing.Point(826, 90);
            this.orderedByPickButton.Name = "orderedByPickButton";
            this.orderedByPickButton.Size = new System.Drawing.Size(25, 20);
            this.orderedByPickButton.TabIndex = 30;
            this.orderedByPickButton.Text = "...";
            this.orderedByPickButton.Click += new System.EventHandler(this.OrderedByPickButton_Click);
            // 
            // editButton
            // 
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(616, 395);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 46;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(652, 395);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 47;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormEhrPatientVaccineEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(944, 521);
            this.Controls.Add(this.ungroupButton);
            this.Controls.Add(this.groupButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.observationsGrid);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.administeredByClearButton);
            this.Controls.Add(this.orderedByClearButton);
            this.Controls.Add(this.administrationSiteComboBox);
            this.Controls.Add(this.administrationSiteLabel);
            this.Controls.Add(this.administrationRouteComboBox);
            this.Controls.Add(this.administrationRouteLabel);
            this.Controls.Add(this.actionLabel);
            this.Controls.Add(this.actionComboBox);
            this.Controls.Add(this.refusalReasonLabel);
            this.Controls.Add(this.refusalReasonComboBox);
            this.Controls.Add(this.expirationDateLabel);
            this.Controls.Add(this.expirationDateTextBox);
            this.Controls.Add(this.administeredByComboBox);
            this.Controls.Add(this.administeredByPickButton);
            this.Controls.Add(this.administeredByLabel);
            this.Controls.Add(this.orderedByComboBox);
            this.Controls.Add(this.orderedByPickButton);
            this.Controls.Add(this.orderedByLabel);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.informationSourceLabel);
            this.Controls.Add(this.informationSourceComboBox);
            this.Controls.Add(this.completionStatusLabel);
            this.Controls.Add(this.completionStatusComboBox);
            this.Controls.Add(this.filledStateTextBox);
            this.Controls.Add(this.filledStateLabel);
            this.Controls.Add(this.filledCityTextBox);
            this.Controls.Add(this.filledCityLabel);
            this.Controls.Add(this.noteLabel);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.documentLabel);
            this.Controls.Add(this.dateStopTextBox);
            this.Controls.Add(this.dateStartTextBox);
            this.Controls.Add(this.dateStopLabel);
            this.Controls.Add(this.dateStartLabel);
            this.Controls.Add(this.unitsComboBox);
            this.Controls.Add(this.vaccineComboBox);
            this.Controls.Add(this.vaccineLabel);
            this.Controls.Add(this.lotNumberTextBox);
            this.Controls.Add(this.amountTextBox);
            this.Controls.Add(this.manufacturerTextBox);
            this.Controls.Add(this.lotNumberLabel);
            this.Controls.Add(this.amountLabel);
            this.Controls.Add(this.manufacturerLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrPatientVaccineEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patient Vaccine";
            this.Load += new System.EventHandler(this.FormEhrPatientVaccineEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label manufacturerLabel;
		private System.Windows.Forms.Label amountLabel;
		private System.Windows.Forms.TextBox amountTextBox;
		private System.Windows.Forms.ComboBox vaccineComboBox;
		private System.Windows.Forms.Label vaccineLabel;
		private System.Windows.Forms.ComboBox unitsComboBox;
		private System.Windows.Forms.TextBox manufacturerTextBox;
		private System.Windows.Forms.Label dateStartLabel;
		private System.Windows.Forms.TextBox dateStartTextBox;
		private System.Windows.Forms.Label lotNumberLabel;
		private System.Windows.Forms.TextBox lotNumberTextBox;
		private System.Windows.Forms.TextBox dateStopTextBox;
		private System.Windows.Forms.Label dateStopLabel;
		private System.Windows.Forms.Label documentLabel;
		private System.Windows.Forms.TextBox noteTextBox;
		private System.Windows.Forms.Label noteLabel;
		private System.Windows.Forms.TextBox filledCityTextBox;
		private System.Windows.Forms.Label filledCityLabel;
		private System.Windows.Forms.TextBox filledStateTextBox;
		private System.Windows.Forms.Label filledStateLabel;
		private System.Windows.Forms.ComboBox completionStatusComboBox;
		private System.Windows.Forms.Label completionStatusLabel;
		private System.Windows.Forms.Label informationSourceLabel;
		private System.Windows.Forms.ComboBox informationSourceComboBox;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.ComboBox orderedByComboBox;
		private OpenDental.UI.Button orderedByPickButton;
		private System.Windows.Forms.Label orderedByLabel;
		private System.Windows.Forms.ComboBox administeredByComboBox;
		private OpenDental.UI.Button administeredByPickButton;
		private System.Windows.Forms.Label administeredByLabel;
		private OpenDental.ValidDate expirationDateTextBox;
		private System.Windows.Forms.Label expirationDateLabel;
		private System.Windows.Forms.Label refusalReasonLabel;
		private System.Windows.Forms.ComboBox refusalReasonComboBox;
		private System.Windows.Forms.Label actionLabel;
		private System.Windows.Forms.ComboBox actionComboBox;
		private System.Windows.Forms.Label administrationRouteLabel;
		private System.Windows.Forms.ComboBox administrationRouteComboBox;
		private System.Windows.Forms.ComboBox administrationSiteComboBox;
		private System.Windows.Forms.Label administrationSiteLabel;
		private OpenDental.UI.Button orderedByClearButton;
		private OpenDental.UI.Button administeredByClearButton;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TextBox userTextBox;
		private OpenDental.UI.ODGrid observationsGrid;
		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button groupButton;
		private OpenDental.UI.Button ungroupButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button deleteButton;
    }
}
