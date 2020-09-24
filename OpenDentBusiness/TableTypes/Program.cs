using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    /// Each row is a bridge to an outside program, frequently an imaging program. 
    /// Most of the bridges are hard coded, and simply need to be enabled. 
    /// But user can also add their own custom bridge.
    /// </summary>
    [Table]
	public class Program : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The name of the program. This value is hardcoded and cannot be modified.
		/// </summary>
		[Column(ReadOnly = true)]
		public string Name;

		/// <summary>
		/// A description of the program.
		/// </summary>
		public string Description; // TODO: Description should be provided by the bridge, that way it can be translated...

		/// <summary>
		/// A value indicating whether the program is enabled.
		/// </summary>
		public bool Enabled;

		/// <summary>
		/// The path of the executable to run or file to open.
		/// </summary>
		public string Path;

		/// <summary>
		/// Some programs will accept command line arguments.
		/// </summary>
		public string CommandLine;

		/// <summary>
		/// Notes about this program link. Peculiarities, etc.
		/// </summary>
		public string Note;

		/// <summary>
		/// If no image, then will be an empty string. In this case, the bitmap will be null when loaded from the database.
		/// 
		/// Must be a 22 x 22 image, and thus needs (width) x (height) x (depth) = 22 x 22 x 4 = 1936 bytes.
		/// </summary>
		public string ButtonImage;

		public Program Copy()
		{
			return (Program)MemberwiseClone();
		}
	}

	/// <summary>
	/// This enum is stored in the database as strings rather than as numbers, so we can do the order alphabetically and we can change it whenever we want.
	/// </summary>
	public enum ProgramName
	{
		None,
		ActeonImagingSuite,
		Adstra,
		Apixia,
		Apteryx,
		AudaxCeph,
		///<summary>Avalara Inc.</summary>
		AvaTax,
		BencoPracticeManagement,
		BioPAK,
		///<summary>Newer version of MediaDent. Uses OLE COM interface.</summary>
		CADI,
		CallFire,
		Camsight,
		CaptureLink,
		CareCredit,
		Carestream,
		CentralDataStorage,
		Cerec,
		CleaRay,
		CliniView,
		ClioSoft,
		DBSWin,
		DemandForce,
		DentalEye,
		DentalIntel,
		DentalStudio,
		DentalTekSmartOfficePhone,
		DentForms,
		DentX,
		Dexis,
		DexisIntegrator,
		Digora,
		Dimaxis,
		Divvy,
		Dolphin,
		DrCeph,
		Dropbox,
		DXCPatientCreditScore,
		Dxis,
		EasyNotesPro,
		eClinicalWorks,
		///<summary>electronic Rx.</summary>
		eRx,
		EvaSoft,
		EwooEZDent,
		FHIR,
		FloridaProbe,
		Guru,
		HandyDentist,
		HdxWill,
		HouseCalls,
		IAP,
		iCat,
		iDixel,
		ImageFX,
		iRYS,
		Lightyear,
		MediaDent,
		Midway,
		MiPACS,
		Mountainside,
		NewTomNNT,
		Office,
		///<summary>Please use Programs.UsingOrion where possible.</summary>
		Orion,
		OrthoCAD,
		OrthoInsight3d,
		OrthoPlex,
		Oryx,
		Owandy,
		PandaPerio,
		PandaPerioAdvanced,
		PayConnect,
		PaySimple,
		Patterson,
		PerioPal,
		PDMP,
		Podium,
		PracticeByNumbers,
		PracticeWebReports,
		PreXionAquire,
		PreXionViewer,
		Progeny,
		///<summary>Paperless Technology.</summary>
		PT,
		///<summary>Paperless Technology.</summary>
		PTupdate,
		RapidCall,
		RayMage,
		Romexis,
		Scanora,
		Schick,
		SFTP,
		Sirona,
		SMARTDent,
		Sopro,
		TigerView,
		Transworld,
		Triana,
		Trojan,
		TrojanExpressCollect,
		Trophy,
		TrophyEnhanced,
		Tscan,
		UAppoint,
		Vipersoft,
		visOra,
		VistaDent,
		VixWin,
		VixWinBase36,
		VixWinBase41,
		VixWinNumbered,
		VixWinOld,
		Xcharge,
		XDR,
		XVWeb,
		ZImage
	}
}
