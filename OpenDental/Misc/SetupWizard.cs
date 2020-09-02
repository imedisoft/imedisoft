using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.User_Controls.SetupWizard;
using OpenDentBusiness;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace OpenDental
{
    /// <summary>
    /// All the Setup Wizard methods that should show up in FormSetupWizard need to have a SetupCatAttr() attribute.
    /// This will cause the method to show up in FormSetupWizard.
    /// </summary>
    public partial class SetupWizard
	{
		public abstract class SetupWizClass
		{
			/// <summary>
			/// The name of the setup item that will appear in the list of setup items. This should be one or a few words. 
			/// It needs to make sense in the following sentences (where XX is the Name):
			/// "Let's set up your XX..."
			/// "Congratulations! You have finished setting up your XX!"
			/// "You can always go back through this setup wizard if you need to make any modifications to your XX."
			/// </summary>
			public abstract string Name { get; }

			/// <summary>
			/// The description and explanation for this setup item.
			/// </summary>
			public abstract string Description { get; }

			/// <summary>
			/// A category for this setup item.
			/// </summary>
			public abstract ODSetupCategory Category { get; }

			/// <summary>
			/// The status of this setup item at any given moment. Should return one of the three setup statuses conditionally.
			/// </summary>
			public abstract ODSetupStatus Status { get; }

			/// <summary>
			/// Most of the time, you should return a control that is stored in an instance of the setup class.
			/// </summary>
			public abstract SetupWizControl SetupControl { get; }
		}

		public class SetupIntro : SetupWizClass
		{
			private readonly SetupWizControl control;
			private readonly string name;

			public SetupIntro(string name, string descript)
			{
				this.name = name;

				control = new UserControlSetupWizIntro(name, descript);
			}

			public override ODSetupCategory Category
				=> throw new NotSupportedException();

			public override ODSetupStatus Status
				=> throw new NotSupportedException();

			public override string Name
				=> name;

			public override string Description
				=> throw new NotSupportedException();

			public override SetupWizControl SetupControl
				=> control;
		}

		public class SetupComplete : SetupWizClass
		{
			private readonly SetupWizControl control;
			private readonly string name;

			public SetupComplete(string name)
			{
				this.name = name;

				control = new UserControlSetupWizComplete(name);
			}

			public override ODSetupCategory Category
				=> throw new NotSupportedException();

			public override ODSetupStatus Status
				=> throw new NotSupportedException();

			public override string Name
				=> name;

			public override string Description
				=> throw new NotSupportedException();

			public override SetupWizControl SetupControl
				=> control;
		}

		public class FeatureSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizFeatures();

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override string Description
				=> "Turn features that your office uses on/off. Settings will affect all computers using the same database.";

			public override ODSetupStatus Status
				=> ODSetupStatus.Optional;

			public override string Name
				=> "Basic Features";

			public override SetupWizControl SetupControl
				=> control;
		}

		public class ClinicSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizClinic();

			public override string Name
				=> "Clinics";

			public override string Description =>
				"You have indicated that you will be using the Clinics feature. " +
				"Clinics can be used when you have multiple locations. Once clinics are set up, you can assign clinics throughout Open Dental. " +
				"If you follow basic guidelines, default clinic assignments for patient information should be accurate, thus reducing data entry.";

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override ODSetupStatus Status
			{
				get
				{
					var clinics = Clinics.GetAll(false);
					if (clinics.Count == 0)
					{
						return ODSetupStatus.NotStarted;
					}

					foreach (var clinic in clinics)
					{
						if (string.IsNullOrEmpty(clinic.Abbr) ||
							string.IsNullOrEmpty(clinic.Description) ||
							string.IsNullOrEmpty(clinic.Phone) ||
							string.IsNullOrEmpty(clinic.AddressLine1))
						{
							return ODSetupStatus.NeedsAttention;
						}
					}

					return ODSetupStatus.Complete;
				}
			}

			public override SetupWizControl SetupControl
				=> control;
		}

		public class DefinitionSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizDefinitions();

			public override string Name
				=> "Definitions";

			public override string Description =>
				"Definitions are an easy way to customize your software experience. Setup the colors, categories, and other customizable areas " +
				"within the program from this window.\r\nWe've selected some of the definitions you may be interested in customizing for this Setup Wizard. " +
				"You may view the entire list of definitions by going to Setup -> Definitions from the main tool bar.";

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override ODSetupStatus Status
				=> ODSetupStatus.Optional;

			public override SetupWizControl SetupControl
				=> control;
		}

		public class ProvSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizProvider();

			public override string Name
				=> "Providers";

			public override string Description =>
				"Providers will show up in almost every part of OpenDental. " +
				"It is important that all provider information is up-to-date so that " +
				"claims, reports, procedures, fee schedules, and estimates will function correctly.";


			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override ODSetupStatus Status
			{
				get
				{
					var providers = Providers.GetDeepCopy(true);
					if (providers.Count == 0)
					{
						return ODSetupStatus.NotStarted;
					}

					foreach (var provider in providers)
					{
						bool isDentist = IsPrimary(provider);
						bool isHyg = provider.IsSecondary;

						if (((isDentist || isHyg) && string.IsNullOrEmpty(provider.Abbr)) ||
							((isDentist || isHyg) && string.IsNullOrEmpty(provider.LName)) ||
							((isDentist || isHyg) && string.IsNullOrEmpty(provider.FName)) ||
							(isDentist && string.IsNullOrEmpty(provider.Suffix)) ||
							(isDentist && string.IsNullOrEmpty(provider.SSN)) ||
							(isDentist && string.IsNullOrEmpty(provider.NationalProvID)))
						{
							return ODSetupStatus.NeedsAttention;
						}
					}

					return ODSetupStatus.Complete;
				}
			}

			public override SetupWizControl SetupControl
				=> control;

			public static bool IsPrimary(Provider provider)
			{
				if (provider.IsSecondary) return false;

				if (Definitions.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "hygienist" ||
					Definitions.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "assistant" ||
					Definitions.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "labtech" ||
					Definitions.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "other" ||
					Definitions.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "notes" ||
					Definitions.GetName(DefinitionCategory.ProviderSpecialties, provider.Specialty).ToLower() == "none")
				{
					return false;
				}

				if (provider.IsNotPerson) return false;

				return true;
			}
		}

		public class OperatorySetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizOperatory();

			public override string Name
				=> "Operatories";

			public override string Description =>
				"Operatories define locations in which appointments take place, and are used to organize appointments shown on the schedule. " +
				"Normally, every chair in your office will have an unique operatory. ";

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override ODSetupStatus Status
			{
				get
				{
					var operatories = Operatories.GetDeepCopy(true);
					if (operatories.Count == 0)
					{
						return ODSetupStatus.NotStarted;
					}

					foreach (var operatory in operatories)
					{
						if (string.IsNullOrEmpty(operatory.OpName) || string.IsNullOrEmpty(operatory.Abbrev))
						{
							return ODSetupStatus.NeedsAttention;
						}
					}

					return ODSetupStatus.Complete;
				}
			}

			public override SetupWizControl SetupControl
				=> control;
		}

		public class PracticeSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizPractice();

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override string Description =>
				"Practice information includes general contact information, billing and pay-to addresses, and default providers. " +
				"This information will appear on most reports, claims, statements, and letters.";

			public override ODSetupStatus Status
				=> ODSetupStatus.Complete;

			public override string Name
				=> "Practice Info";

			public override SetupWizControl SetupControl
				=> control;
		}

		public class EmployeeSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizEmployee();

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override string Description =>
				"The Employee list is used to set up User profiles in Security and to set up Schedules. " +
				"This list also determines who can use the Time Clock.";

			public override ODSetupStatus Status
			{
				get
				{
					var employees = Employees.GetDeepCopy(true);
					if (employees.Count == 0)
					{
						return ODSetupStatus.NotStarted;
					}

					foreach (var employee in employees)
					{
						if (string.IsNullOrEmpty(employee.FName) || string.IsNullOrEmpty(employee.LName))
						{
							return ODSetupStatus.NeedsAttention;
						}
					}

					return ODSetupStatus.Complete;
				}
			}

			public override string Name
				=> "Employees";

			public override SetupWizControl SetupControl
				=> control;
		}

		public class FeeSchedSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizFeeSched();

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override string Description
				=> "Fee Schedules determine the fees billed for each procedure.";

			/// <summary>
			/// Returns Complete if all fee schedules have at least one fee entered.
			/// </summary>
			public override ODSetupStatus Status
			{
				get
				{
					var feeSchedules = FeeScheds.GetDeepCopy(true);
					if (feeSchedules.Count == 0)
					{
						return ODSetupStatus.NotStarted;
					}

					foreach (var feeSchedule in feeSchedules)
					{
						if (Fees.GetCountByFeeSchedNum(feeSchedule.FeeSchedNum) <= 0)
						{
							return ODSetupStatus.NeedsAttention;
						}
					}

					return ODSetupStatus.Complete;
				}
			}

			public override string Name
				=> "Fee Schedules";

			public override SetupWizControl SetupControl
				=> control;
		}

		public class PrinterSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizPrinter();

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override string Description =>
				"Set up print and scan options for the current workstation. " +
				"You can leave all settings to the default, or you can control where specific items are are printed.";

			public override ODSetupStatus Status
				=> ODSetupStatus.Optional;

			public override string Name
				=> "Printer/Scanner";

			public override SetupWizControl SetupControl
				=> control;
		}

		public class ScheduleSetup : SetupWizClass
		{
			private readonly SetupWizControl control = new UserControlSetupWizSchedule();

			public override ODSetupCategory Category
				=> ODSetupCategory.Basic;

			public override string Description =>
				"Schedule setup lets you enter all Provider and Employee schedules. " +
				"You can define any kind of rotating or alternating schedule you want. " +
				"Enter individual work hours, holidays, lunch hours, and staff meetings. " +
				"Once schedules are entered, open/closed hours will be indicated in the Appointment module.";

			public override ODSetupStatus Status
			{
				get
				{
					var schedules = Schedules.GetTwoYearPeriod(DateTime.Today.AddYears(-1));
					if (schedules.Count == 0)
					{
						return ODSetupStatus.NotStarted;
					}

					var providers = Providers.GetWhere(x => !x.IsNotPerson, true).ToList();
					foreach (var provider in providers)
					{
						if (!schedules.Select(x => x.ProvNum).Contains(provider.ProvNum))
						{
							return ODSetupStatus.NeedsAttention;
						}
					}

					return ODSetupStatus.Complete;
				}
			}

			public override string Name
				=> "Schedule";

			public override SetupWizControl SetupControl
				=> control;
		}

		public static Color GetColor(ODSetupStatus status)
		{
			switch (status)
			{
				case ODSetupStatus.NotStarted:
				case ODSetupStatus.NeedsAttention:
					return Color.FromArgb(255, 204, 204);

				case ODSetupStatus.Complete:
				case ODSetupStatus.Optional:
					return Color.FromArgb(204, 255, 204);

				default:
					return Color.White;
			}
		}
	}

	public enum ODSetupCategory
	{
		[Description("Misc Setup")]
		Misc,

		None,

		[Description("Pre-Setup")]
		PreSetup,

		[Description("Basic Setup")]
		Basic,

		[Description("Advanced Setup")]
		Advanced,
	}

	public enum ODSetupStatus
	{
		/// <summary>
		/// User hasn't started this setup item.
		/// </summary>
		[Description("Needs Input")]
		NotStarted,

		/// <summary>
		/// User has left this setup item in an incomplete state.
		/// </summary>
		[Description("Needs Input")]
		NeedsAttention,

		/// <summary>
		/// Setup item has been considered and required elements have been filled in.
		/// </summary>
		[Description("OK")]
		Complete,

		/// <summary>
		/// Setup item is not required.
		/// </summary>
		[Description("Optional")]
		Optional
	}
}
