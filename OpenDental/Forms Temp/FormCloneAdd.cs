using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Imedisoft.Forms;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormCloneAdd:ODForm {
		///<summary>Shallow copy of the "master" patient that was passed into the constructor that the clone will be created from.</summary>
		private Patient _patientMaster;
		private Family _familyCur;
		private List<InsurancePlan> _listInsPlans;
		private List<InsSub> _listInsSubs;
		private List<Benefit> _listBenefits;
		private long _provNumSelected;
		private List<Provider> _listProviders;
		///<summary>A dictionary of specialties (key: DefNum) and the clinics that are set to that specialty (value: List&lt;Clinic&gt;)
		///If no specialties are present then this dictionary will have a single entry with a key of 0 with a list of all clinics available to the user.
		///Only filled on load if clinics are enabled.</summary>
		private Dictionary<long,List<Clinic>> _dictSpecialtyClinics;
		///<summary>Will be set to the PatNum of the new clone that was created if the user actually created one.</summary>
		public long PatNumClone;

		///<summary>Patient must be the original or master patient that will have a clone made from them.</summary>
		public FormCloneAdd(Patient patientMaster,Family familyCur=null,List<InsurancePlan> listInsPlans=null,List<InsSub> listInsSubs=null
			,List<Benefit> listBenefits=null) 
		{
			InitializeComponent();
			
			_patientMaster=patientMaster;
			_familyCur=familyCur;
			_listInsSubs=listInsSubs;
			_listInsPlans=listInsPlans;
			_listBenefits=listBenefits;
		}

		private void FormCloneAdd_Load(object sender,EventArgs e) {
			//Make sure that this patient is not already a clone of another patient.  We don't allow clones of clones.
			//PatientLink.PatNumFrom is the master and PatientLink.PatNumTo is the actual clone.  We care if this patient has ever been a "PatNumTo".
			if(PatientLinks.IsPatientAClone(_patientMaster.PatNum)) {
				MessageBox.Show("Cannot create a clone of a clone.  Please select the original patient in order to create another clone.");
				return;
			}
			textLName.Text=_patientMaster.LName;
			textFName.Text=_patientMaster.FName;
			textPreferred.Text=_patientMaster.Preferred;
			textMiddleI.Text=_patientMaster.MiddleI;
			textBirthDate.Text=(_patientMaster.Birthdate.Year < 1880) ? "" : _patientMaster.Birthdate.ToShortDateString();
			textAge.Text=PatientLogic.DateToAgeString(_patientMaster.Birthdate,_patientMaster.DateTimeDeceased);
			//We intentionally don't synch the patient's provider since the clone feature is so the clone can be assigned to a different provider for tracking production.
			_provNumSelected=Preferences.GetLong(PreferenceName.PracticeDefaultProv);
			_listProviders=Providers.GetAll(true);
			comboPriProv.Items.Clear();
			for(int i = 0;i<_listProviders.Count;i++) {
				comboPriProv.Items.Add(_listProviders[i].GetLongDesc());
				if(_listProviders[i].Id==_provNumSelected) {
					comboPriProv.SelectedIndex=i;
				}
			}
			if(_provNumSelected==0) {
				comboPriProv.SelectedIndex=0;
				_provNumSelected=_listProviders[0].Id;
			}
			if(comboPriProv.SelectedIndex==-1) {
				comboPriProv.Text=Providers.GetLongDesc(_provNumSelected);
			}
			labelSpecialty.Visible=true;
			comboSpecialty.Visible=true;
			if(PrefC.HasClinicsEnabled) {
				labelClinic.Visible=true;
				comboClinic.Visible=true;
				FillClinicComboBoxes();
			}
			else{//Without clinics enabled the specialty box is filled differently.
				FillComboSpecialtyNoClinics();
			}
		}

		///<summary>Fills both the Specialty and Clinic combo boxes according to the available clinics to the user and the unused specialties for the patient.
		///Only fills the combo box of clinics with clinics that are associated to specialties that have not been used for this patient yet.
		///E.g. Even if the user has access to Clinic X, if there is already a clone of this patient for Clinic X, it will no longer show.
		///Throws exceptions that should be shown to the user which should then be followed by closing the window.</summary>
		private void FillClinicComboBoxes() {
			_dictSpecialtyClinics=new Dictionary<long,List<Clinic>>();
			//Fill the list of clinics for this user.
			List<Clinic> listClinicsForUser=Clinics.GetByUser(Security.CurrentUser);
			//Make a deep copy of the list of clinics so that we can filter down to the clinics that have no specialty specified if all are hidden.
			List<Clinic> listClinicsNoSpecialty=listClinicsForUser.Select(x => x.Copy()).ToList();
			//Fill the list of defLinks used by clones of this patient.
			List<long> listClonePatNum=PatientLinks.GetPatNumsLinkedFrom(_patientMaster.PatNum,PatientLinkType.Clone);
			List<DefLink> listPatCurDefLinks=DefLinks.GetListByFKeys(listClonePatNum,DefLinkType.Patient);
			//Fill the list of clinics defLink
			List<DefLink> listClinicDefLinks=DefLinks.GetDefLinksByType(DefLinkType.Clinic);
			//Filter out any specialties that are currently in use by clones of this patient.
			if(listPatCurDefLinks.Count>0) {
				listClinicDefLinks.RemoveAll(x => x.DefinitionId.In(listPatCurDefLinks.Select(y => y.DefinitionId).ToList()));
			}
			//Get all non-hidden specialties
			List<Definition> listSpecialtyDefs=Definitions.GetDefsForCategory(DefinitionCategory.ClinicSpecialty,true);
			//If there are specialties present, we need to know which clinics have no specialty set so that the user can always make clones for that specialty.
			if(listSpecialtyDefs.Count > 0) {
				listClinicsNoSpecialty.RemoveAll(x => x.Id.In(listClinicDefLinks.Select(y => y.FKey).ToList()));
			}
			//Remove all clinics that do not have any specialties from the original list of clinics for the user.
			listClinicsForUser.RemoveAll(x => !x.Id.In(listClinicDefLinks.Select(y => y.FKey).ToList()));
			//Filter out any specialties that are not associated to any available clinics for this user.
			listSpecialtyDefs.RemoveAll(x => !x.Id.In(listClinicDefLinks.Select(y => y.DefinitionId).ToList()));
			//Lump all of the left over specialties into a dictionary and slap the associated clinics to them.
			comboSpecialty.Items.Clear();
			//Create a dummy specialty of 0 if there are any clinics that do not have a specialty.
			if(listClinicsNoSpecialty!=null && listClinicsNoSpecialty.Count > 0) {
				comboSpecialty.Items.Add(new ODBoxItem<Definition>("Unspecified",new Definition() { Id=0 }));
				_dictSpecialtyClinics[0]=listClinicsNoSpecialty;
			}
			foreach(Definition specialty in listSpecialtyDefs) {
				comboSpecialty.Items.Add(new ODBoxItem<Definition>(specialty.Name,specialty));
				//Get a list of all deflinks for the def
				List<DefLink> listLinkForDef=listClinicDefLinks.FindAll(x => x.DefinitionId==specialty.Id).ToList();
				_dictSpecialtyClinics[specialty.Id]=listClinicsForUser.FindAll(x => x.Id.In(listLinkForDef.Select(y => y.FKey).ToList()));
			}
			//If there are no specialties to show, we need to let the user know that they need to associate at least one clinic to a specialty.
			if(_dictSpecialtyClinics.Count < 1) {
				MessageBox.Show("This patient already has a clone for every Clinic Specialty available.\r\n"
					+"In the main menu, click Setup, Definitions, Clinic Specialties category to add new specialties.\r\n"
					+"In the main menu, click Lists, Clinics, and double click a clinic to set a Specialty.");
				DialogResult=DialogResult.Abort;
				return;
			}
			comboSpecialty.SelectedIndex=0;
			FillComboClinic();
		}

		private void FillComboClinic() {
			if(!PrefC.HasClinicsEnabled) {
				return;
			}
			comboClinic.Items.Clear();
			if(comboSpecialty.SelectedItem==null || comboSpecialty.SelectedItem.GetType()!=typeof(ODBoxItem<Definition>)) {
				return;//Somehow the specialty box changed to an invalid item.  Nothing else to do.
			}
			//Only allow the Unassigned clinic for the Unspecified specialty.
			if(comboSpecialty.SelectedIndex==0 
				&& ((ODBoxItem<Definition>)comboSpecialty.SelectedItem).Tag!=null
				&& ((ODBoxItem<Definition>)comboSpecialty.SelectedItem).Tag.Id==0)
			{
				comboClinic.Items.Add("Unassigned",new Clinic());
			}
			foreach(Clinic clinic in _dictSpecialtyClinics[((ODBoxItem<Definition>)comboSpecialty.SelectedItem).Tag.Id]) {
				comboClinic.Items.Add(clinic.Abbr,clinic);
			}
		}

		///<summary>Used in the case when clinics are disabled. Requires special logic that doesn't use clinics.</summary>
		private void FillComboSpecialtyNoClinics() {
			//Get all non-hidden specialties
			List<Definition> listSpecialtyDefs=Definitions.GetDefsForCategory(DefinitionCategory.ClinicSpecialty,true);
			//Fill the list of defLinks used by clones of this patient.
			List<long> listClonePatNums=PatientLinks.GetPatNumsLinkedFrom(_patientMaster.PatNum,PatientLinkType.Clone);
			List<DefLink> listPatCurDefLinks=DefLinks.GetListByFKeys(listClonePatNums,DefLinkType.Patient);
			//Filter out any specialties that are currently in use by clones of this patient.
			if(listPatCurDefLinks.Count>0) {
				listSpecialtyDefs.RemoveAll(x => x.Id.In(listPatCurDefLinks.Select(y => y.DefinitionId).ToList()));
			}
			comboSpecialty.Items.Clear();
			//Create a dummy specialty of 0.  Always allow the user to make Unspecified clones.
			comboSpecialty.Items.Add(new ODBoxItem<Definition>("Unspecified",new Definition() { Id=0 }));
			foreach(Definition specialty in listSpecialtyDefs) {
				comboSpecialty.Items.Add(new ODBoxItem<Definition>(specialty.Name,specialty));
			}
			comboSpecialty.SelectedIndex=0;
		}

		private void butPickPrimary_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick(_listProviders);
			if(comboPriProv.SelectedIndex > -1) {//Initial FormP selection if selected prov is not hidden.
				FormPP.SelectedProviderId=_provNumSelected;
			}
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboPriProv.SelectedIndex=_listProviders.FindIndex(x => x.Id==FormPP.SelectedProviderId);
			_provNumSelected=FormPP.SelectedProviderId;
		}

		private void comboPriProv_SelectionChangeCommitted(object sender,EventArgs e) {
			_provNumSelected=_listProviders[comboPriProv.SelectedIndex].Id;
		}

		///<summary>The clinic combo box needs to get refilled every time the specialty changes.</summary>
		private void comboSpecialty_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboClinic();
		}

		///<summary>Validates the form and will show a message to the user if something is invalid and then will return false, otherwise true.</summary>
		private bool IsValid() {
			if(_provNumSelected < 1) {
				MessageBox.Show("Invalid Primary Provider selected.");
				return false;
			}
			if(PrefC.HasClinicsEnabled) {
				#region Clinic Specific Validation
				if(comboSpecialty.SelectedItem!=null && comboSpecialty.SelectedItem.GetType()!=typeof(ODBoxItem<Definition>)) {
					MessageBox.Show("Invalid Specialty selected.");
					return false;
				}
				if(comboSpecialty.SelectedIndex < 0) {
					MessageBox.Show("A Specialty is required in order to create a clone.");
					return false;
				}
				if(comboClinic.SelectedIndex < 0) {
					MessageBox.Show("A Clinic is required in order to create a clone.");
					return false;
				}
				#endregion
			}
			return true;
		}

		private void butClone_Click(object sender,EventArgs e) {
			if(!IsValid()) {
				return;//A message should have already shown to the user.
			}
			long clinicNum=0;
			long defNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=comboClinic.GetSelected<Clinic>().Id;
			}
			defNum=((ODBoxItem<Definition>)comboSpecialty.SelectedItem).Tag.Id;
			Patient clone=Patients.CreateCloneAndSynch(_patientMaster,_familyCur,_listInsPlans,_listInsSubs,_listBenefits,_provNumSelected,clinicNum);
			if(clone!=null) {
				PatNumClone=clone.PatNum;
				if(defNum!=0) {
					DefLinks.Insert(new DefLink() {
						DefinitionId=defNum,
						FKey=PatNumClone,
						LinkType=DefLinkType.Patient,
					});
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}