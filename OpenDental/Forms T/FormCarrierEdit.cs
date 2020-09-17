using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace Imedisoft.Forms
{
	public partial class FormCarrierEdit : FormBase
	{
		private readonly Carrier carrier;

		public FormCarrierEdit(Carrier carrier)
		{
			InitializeComponent();

			this.carrier = carrier;
		}

		private void FormCarrierEdit_Load(object sender, EventArgs e)
		{
			if (carrier.Id != 0)
			{
				textCarrierNum.Text = carrier.Id.ToString();
			}

			textCarrierName.Text = carrier.Name;
			textPhone.Text = carrier.Phone;
			textAddress.Text = carrier.AddressLine1;
			textAddress2.Text = carrier.AddressLine2;
			textCity.Text = carrier.City;
			textState.Text = carrier.State;
			textZip.Text = carrier.Zip;
			textElectID.Text = carrier.ElectronicId;
			List<Definition> listDefsCarrierGroupNames = Definitions.GetByCategory(DefinitionCategory.CarrierGroupNames);//Only Add non hidden definitions
																														 //new List<Def> { new Def() { DefNum=0,ItemName="Unspecified" } };
																														 //listDefsCarrierGroupNames.AddRange(Defs.GetDefsForCategory(DefinitionCategory.CarrierGroupNames,true));
			if (listDefsCarrierGroupNames.Count > 0)
			{//only show if at least one CarrierGroupName definition
				labelCarrierGroupName.Visible = true;
				comboCarrierGroupName.Visible = true;
				comboCarrierGroupName.Items.Add("Unspecified", new Definition());//defNum 0
				comboCarrierGroupName.Items.AddDefs(listDefsCarrierGroupNames);
				comboCarrierGroupName.SetSelectedDefNum(carrier.CarrierGroupName);
			}
			comboSendElectronically.Items.AddEnums<NoSendElectType>();
			comboSendElectronically.SetSelectedEnum(carrier.NoSendElect);
			checkIsHidden.Checked = carrier.IsHidden;
			checkRealTimeEligibility.Checked = carrier.TrustedEtransFlags.HasFlag(TrustedEtransTypes.RealTimeEligibility);
			radioBenefitSendsPat.Checked = (!carrier.IsCoinsuranceInverted);//Default behaviour.
			radioBenefitSendsIns.Checked = (carrier.IsCoinsuranceInverted);
			List<string> dependentPlans = Carriers.DependentPlans(carrier);
			textPlans.Text = dependentPlans.Count.ToString();
			comboPlans.Items.Clear();
			for (int i = 0; i < dependentPlans.Count; i++)
			{
				comboPlans.Items.Add(dependentPlans[i]);
			}
			if (dependentPlans.Count > 0)
			{
				comboPlans.SelectedIndex = 0;
			}
			//textTemplates.Text=Carriers.DependentTemplates().ToString();
			isCdaCheckBox.Checked = carrier.IsCDA;//Can be checked but not visible.
			if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				labelCitySt.Text = "City,Province,PostalCode";
				labelElectID.Text = "Carrier Identification Number";
				cdaNetGroupBox.Visible = isCdaCheckBox.Checked;
			}
			else
			{//everyone but Canada
				isCdaCheckBox.Visible = false;
				cdaNetGroupBox.Visible = false;
				int newHeight = (this.Height - cdaNetGroupBox.Height - isCdaCheckBox.Height);//Dynamically hide the CDAnet groupbox and Is CDAnet Carrier checkbox.
				this.Size = new Size(525, newHeight);
			}
			//Canadian stuff is filled in for everyone, because a Canadian user might sometimes have a computer set to American.
			//So a computer set to American would not be able to SEE the Canadian fields, but they at least would not be damaged.
			comboNetwork.Items.Add("none");
			comboNetwork.SelectedIndex = 0;
			List<CanadianNetwork> listCanadianNetworks = CanadianNetworks.GetAll();
			for (int i = 0; i < listCanadianNetworks.Count; i++)
			{
				comboNetwork.Items.Add(listCanadianNetworks[i].Abbr + " - " + listCanadianNetworks[i].Description);
				if (carrier.CanadianNetworkId == listCanadianNetworks[i].Id)
				{
					comboNetwork.SelectedIndex = i + 1;
				}
			}
			textVersion.Text = carrier.CDAnetVersion;
			textEncryptionMethod.Text = carrier.CanadianEncryptionMethod.ToString();
			check08.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.EligibilityTransaction_08) == CanSupTransTypes.EligibilityTransaction_08);
			check07.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.CobClaimTransaction_07) == CanSupTransTypes.CobClaimTransaction_07);
			check02.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.ClaimReversal_02) == CanSupTransTypes.ClaimReversal_02);
			check03.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.PredeterminationSinglePage_03) == CanSupTransTypes.PredeterminationSinglePage_03);
			check03m.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.PredeterminationMultiPage_03) == CanSupTransTypes.PredeterminationMultiPage_03);
			check04.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.RequestForOutstandingTrans_04) == CanSupTransTypes.RequestForOutstandingTrans_04);
			check05.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.RequestForSummaryReconciliation_05) == CanSupTransTypes.RequestForSummaryReconciliation_05);
			check06.Checked = ((carrier.CanadianSupportedTypes & CanSupTransTypes.RequestForPaymentReconciliation_06) == CanSupTransTypes.RequestForPaymentReconciliation_06);
			odColorPickerBack.AllowTransparentColor = true;
			if (carrier.Id == 0)
			{
				odColorPickerBack.BackgroundColor = Color.Black;//Black means no color
			}
			else
			{
				odColorPickerBack.BackgroundColor = carrier.ApptTextBackColor;
			}
		}

		private void textCarrierName_TextChanged(object sender, System.EventArgs e)
		{
			if (textCarrierName.Text.Length == 1)
			{
				textCarrierName.Text = textCarrierName.Text.ToUpper();
				textCarrierName.SelectionStart = 1;
			}
		}

		private void textAddress_TextChanged(object sender, System.EventArgs e)
		{
			if (textAddress.Text.Length == 1)
			{
				textAddress.Text = textAddress.Text.ToUpper();
				textAddress.SelectionStart = 1;
			}
		}

		private void textAddress2_TextChanged(object sender, System.EventArgs e)
		{
			if (textAddress2.Text.Length == 1)
			{
				textAddress2.Text = textAddress2.Text.ToUpper();
				textAddress2.SelectionStart = 1;
			}
		}

		private void textCity_TextChanged(object sender, System.EventArgs e)
		{
			if (textCity.Text.Length == 1)
			{
				textCity.Text = textCity.Text.ToUpper();
				textCity.SelectionStart = 1;
			}
		}

		private void textState_TextChanged(object sender, System.EventArgs e)
		{
			int cursor = textState.SelectionStart;
			//for all countries, capitalize the first letter
			if (textState.Text.Length == 1)
			{
				textState.Text = textState.Text.ToUpper();
				textState.SelectionStart = cursor;
				return;
			}
			//for US and Canada, capitalize second letter as well.
			if (CultureInfo.CurrentCulture.Name == "en-US"
				|| CultureInfo.CurrentCulture.Name == "en-CA")
			{
				if (textState.Text.Length == 2)
				{
					textState.Text = textState.Text.ToUpper();
					textState.SelectionStart = cursor;
				}
			}
		}

		private void checkIsCDAnet_Click(object sender, EventArgs e)
		{
			cdaNetGroupBox.Visible = isCdaCheckBox.Checked;
		}

		private void butDelete_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Delete Carrier?", "", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			try
			{
				Carriers.Delete(carrier);
			}
			catch (ApplicationException ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult = DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e)
		{
			if (textCarrierName.Text == "")
			{
				MessageBox.Show("Carrier Name cannot be blank.");
				return;
			}
			Carrier carrierOld = carrier.Copy();
			carrier.Name = textCarrierName.Text;
			carrier.Phone = textPhone.Text;
			carrier.AddressLine1 = textAddress.Text;
			carrier.AddressLine2 = textAddress2.Text;
			carrier.City = textCity.Text;
			carrier.State = textState.Text;
			carrier.Zip = textZip.Text;
			carrier.ElectronicId = textElectID.Text;
			//The SelectedItem will be null if hidden. Don't change if the def selected is still hidden.
			//DefNum will be 0 if "Unspecified" is selected. 
			if (comboCarrierGroupName.GetSelectedDefNum() != 0)
			{
				carrier.CarrierGroupName = comboCarrierGroupName.GetSelectedDefNum();
			}
			carrier.NoSendElect = comboSendElectronically.GetSelected<NoSendElectType>();
			carrier.IsHidden = checkIsHidden.Checked;
			carrier.TrustedEtransFlags = (checkRealTimeEligibility.Checked ? TrustedEtransTypes.RealTimeEligibility : TrustedEtransTypes.None);
			carrier.IsCDA = isCdaCheckBox.Checked;
			carrier.ApptTextBackColor = odColorPickerBack.BackgroundColor;
			carrier.IsCoinsuranceInverted = radioBenefitSendsIns.Checked;

			if (carrier.Id == 0)
			{
				try
				{
					Carriers.Insert(carrier);
				}
				catch (ApplicationException ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
			}
			else
			{
				try
				{
					Carriers.Update(carrier);
				}
				catch (ApplicationException ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
			}

			DialogResult = DialogResult.OK;
		}
	}
}
