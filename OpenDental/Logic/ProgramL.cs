using CodeBase;
using Imedisoft.Bridges;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public class ProgramL
	{
		public static void ExecuteNew(long programId, Patient patient) // TODO: Rename this...
        {
			var program = Programs.SelectOne(programId);
			if (program == null)
            {
				ODMessageBox.Show("Error, program entry not found in database.");
				return;
            }

			// Get the bridge...
			var bridge = BridgeManager.GetBridge(program.Name);
			if (bridge == null)
            {
				// TODO: Error...
				return;
            }

			// Send the patient details to the remote program...
			bridge.Send(program, patient);
        }


		///<summary>Typically used when user clicks a button to a Program link.  This method attempts to identify and execute the program based on the given programNum.</summary>
		public static void Execute(long programNum, Patient pat)
		{
			Program prog = Programs.GetFirstOrDefault(x => x.Id == programNum);
			if (prog == null)
			{//no match was found
				MessageBox.Show("Error, program entry not found in database.");
				return;
			}
			if (pat != null && Prefs.GetBool(PrefName.ShowFeaturePatientClone))
			{
				pat = Patients.GetOriginalPatientForClone(pat);
			}

			//if(prog.PluginDllName!="") {
			//	if(pat==null) {
			//		Plugins.LaunchToolbarButton(programNum,0);
			//	}
			//	else{
			//		Plugins.LaunchToolbarButton(programNum,pat.PatNum);
			//	}
			//	return;
			//}

			if (prog.Name == ProgramName.ActeonImagingSuite.ToString())
			{
				//ActeonImagingSuite.SendData(prog, pat);
				return;
			}
			if (prog.Name == ProgramName.Adstra.ToString())
			{
				Adstra.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Apixia.ToString())
			{
				Apixia.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Apteryx.ToString())
			{
				Apteryx.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.AudaxCeph.ToString())
			{
				AudaxCeph.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.BencoPracticeManagement.ToString())
			{
				Benco.SendData(prog);
				return;
			}
			else if (prog.Name == ProgramName.BioPAK.ToString())
			{
				BioPAK.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.CADI.ToString())
			{
				CADI.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Camsight.ToString())
			{
				Camsight.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.CaptureLink.ToString())
			{
				CaptureLink.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Carestream.ToString())
			{
				Carestream.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Cerec.ToString())
			{
				Cerec.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.CleaRay.ToString())
			{
				CleaRay.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.CliniView.ToString())
			{
				Cliniview.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.ClioSoft.ToString())
			{
				ClioSoft.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DBSWin.ToString())
			{
				DBSWin.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DemandForce.ToString())
			{
				DemandForce.SendData(prog, pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if (prog.Name == ProgramName.DentalEye.ToString())
			{
				DentalEye.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DentalStudio.ToString())
			{
				DentalStudio.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DentX.ToString())
			{
				DentX.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DrCeph.ToString())
			{
				DrCeph.SendData(prog, pat);
				return;
			}
#endif
			else if (prog.Name == ProgramName.DentalTekSmartOfficePhone.ToString())
			{
				DentalTek.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DentForms.ToString())
			{
				DentForms.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Dexis.ToString())
			{
				Dexis.SendData(prog, pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if (prog.Name == ProgramName.DexisIntegrator.ToString())
			{
				DexisIntegrator.SendData(prog, pat);
				return;
			}
#endif
			else if (prog.Name == ProgramName.Digora.ToString())
			{
				Digora.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Dimaxis.ToString())
			{
				Planmeca.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Office.ToString())
			{
				Office.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Dolphin.ToString())
			{
				Dolphin.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.DXCPatientCreditScore.ToString())
			{
				DentalXChange.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Dxis.ToString())
			{
				Dxis.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.EvaSoft.ToString())
			{
				EvaSoft.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.EwooEZDent.ToString())
			{
				Ewoo.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.FloridaProbe.ToString())
			{
				FloridaProbe.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Guru.ToString())
			{
				Guru.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.HandyDentist.ToString())
			{
				HandyDentist.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.HouseCalls.ToString())
			{
				FormHouseCalls FormHC = new FormHouseCalls();
				FormHC.ProgramCur = prog;
				FormHC.ShowDialog();
				return;
			}
			else if (prog.Name == ProgramName.iCat.ToString())
			{
				ICat.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.HdxWill.ToString())
			{
				HdxWill.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.iDixel.ToString())
			{
				iDixel.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.ImageFX.ToString())
			{
				ImageFX.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.iRYS.ToString())
			{
				Irys.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Lightyear.ToString())
			{
				Lightyear.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.NewTomNNT.ToString())
			{
				NewTomNNT.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.MediaDent.ToString())
			{
				MediaDent.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Midway.ToString())
			{
				Midway.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.MiPACS.ToString())
			{
				MiPACS.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.OrthoCAD.ToString())
			{
				OrthoCad.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Oryx.ToString())
			{
				Oryx.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.OrthoInsight3d.ToString())
			{
				OrthoInsight3d.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Owandy.ToString())
			{
				Owandy.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.PandaPerio.ToString())
			{
				PandaPerio.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.PandaPerioAdvanced.ToString())
			{
				PandaPerioAdvanced.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Patterson.ToString())
			{
				Patterson.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.PDMP.ToString())
			{
				string response = "";
				if (PDMP.TrySendData(prog, pat, out response))
				{
					FormWebBrowser formWebBrowser = new FormWebBrowser(response);
					formWebBrowser.Show();
				}
				else
				{
					MessageBox.Show(response, "PDMP");
				}
				return;
			}
			else if (prog.Name == ProgramName.PerioPal.ToString())
			{
				PerioPal.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.PracticeByNumbers.ToString())
			{
				PracticeByNumbers.ShowPage();
				return;
			}
			else if (prog.Name == ProgramName.PreXionAquire.ToString())
			{
				PreXion.SendDataAquire(prog, pat);
			}
			else if (prog.Name == ProgramName.PreXionViewer.ToString())
			{
				PreXion.SendDataViewer(prog, pat);
			}
			else if (prog.Name == ProgramName.Progeny.ToString())
			{
				Progeny.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.PT.ToString())
			{
				PaperlessTechnology.SendData(prog, pat, false);
				return;
			}
			else if (prog.Name == ProgramName.PTupdate.ToString())
			{
				PaperlessTechnology.SendData(prog, pat, true);
				return;
			}
			else if (prog.Name == ProgramName.RayMage.ToString())
			{
				RayMage.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Romexis.ToString())
			{
				Romexis.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Scanora.ToString())
			{
				Scanora.SendData(prog, pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if (prog.Name == ProgramName.Schick.ToString())
			{
				Schick.SendData(prog, pat);
				return;
			}
#endif
			else if (prog.Name == ProgramName.Sirona.ToString())
			{
				// TODO: Sirona.SendData(prog,pat);
				return;
			}
			else if (prog.Name == ProgramName.SMARTDent.ToString())
			{
				SmartDent.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Sopro.ToString())
			{
				Sopro.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.TigerView.ToString())
			{
				TigerView.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Triana.ToString())
			{
				Triana.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.TrojanExpressCollect.ToString())
			{
				using (FormTrojanCollect FormT = new FormTrojanCollect(pat))
				{
					FormT.ShowDialog();
				}
				return;
			}
			else if (prog.Name == ProgramName.Trophy.ToString())
			{
				Trophy.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.TrophyEnhanced.ToString())
			{
				TrophyEnhanced.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.Tscan.ToString())
			{
				Tscan.SendData(prog, pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if (prog.Name == ProgramName.Vipersoft.ToString())
			{
				Vipersoft.SendData(prog, pat);
				return;
			}
#endif
			else if (prog.Name == ProgramName.visOra.ToString())
			{
				Visora.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.VistaDent.ToString())
			{
				VistaDent.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.VixWin.ToString())
			{
				VixWin.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.VixWinBase36.ToString())
			{
				VixWinBase36.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.VixWinBase41.ToString())
			{
				VixWinBase41.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.VixWinNumbered.ToString())
			{
				VixWinNumbered.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.VixWinOld.ToString())
			{
				VixWinOld.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.XDR.ToString())
			{
				XDR.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.XVWeb.ToString())
			{
				XVWeb.SendData(prog, pat);
				return;
			}
			else if (prog.Name == ProgramName.ZImage.ToString())
			{
				ZImage.SendData(prog, pat);
				return;
			}
		}

		///<summary>Helper method that replaces the message with all of the Message Replacements available for ProgramLinks.</summary>
		private static string ReplaceHelper(string message, Patient pat)
		{
			string retVal = message;
			retVal = Patients.ReplacePatient(retVal, pat);
			retVal = Patients.ReplaceGuarantor(retVal, pat);
			retVal = Referrals.ReplaceRefProvider(retVal, pat);
			return retVal;
		}

		public static void LoadToolbar(ODToolBar toolBar, ToolBarsAvail toolBarsAvail)
		{
			List<ToolButItem> toolButItems = ToolButItems.GetForToolBar(toolBarsAvail);
			foreach (ToolButItem toolButItemCur in toolButItems)
			{
				Program programCur = Programs.GetProgram(toolButItemCur.ProgramNum);
				if (PrefC.HasClinicsEnabled)
				{
					//User should not have PL hidden if Clinics are not Enabled, otherwise this could create a situation where users may turn clinics off but 
					//have hidden the PL button for HQ and then be unable to turn the button back on without re-enabling Clinics.
					ProgramProperty programProp = ProgramProperties.GetPropForProgByDesc(programCur.Id
						, ProgramProperties.PropertyDescs.ClinicHideButton, Clinics.ClinicId);
					if (programProp != null)
					{
						continue;//If there exists a programProp for a clinic which should have its buttons hidden, carry on and do not display the button.
					}
				}
				if (ProgramProperties.IsAdvertisingDisabled(programCur))
				{
					continue;
				}
				string key = programCur.Id.ToString() + programCur.Name.ToString();
				if (toolBar.ImageList.Images.ContainsKey(key))
				{
					//Dispose the existing image only if it already exists, because the image might have changed.
					toolBar.ImageList.Images[toolBar.ImageList.Images.IndexOfKey(key)].Dispose();
					toolBar.ImageList.Images.RemoveByKey(key);
				}
				if (programCur.ButtonImage != "")
				{
					Image image = PIn.Bitmap(programCur.ButtonImage);
					toolBar.ImageList.Images.Add(key, image);
				}
				else if (programCur.Name == ProgramName.Midway.ToString())
				{
					Image image = global::Imedisoft.Properties.Resources.Midway_Icon_22x22;
					toolBar.ImageList.Images.Add(key, image);
				}
				if (toolBarsAvail != ToolBarsAvail.MainToolbar)
				{
					toolBar.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				}
				ODToolBarButton button = new ODToolBarButton(toolButItemCur.ButtonText, -1, "", programCur);
				AddDropDown(button, programCur);
				toolBar.Buttons.Add(button);
			}




			for (int i = 0; i < toolBar.Buttons.Count; i++)
			{
				if (toolBar.Buttons[i].Tag is Program program)
                {
					string key = program.Id.ToString() + program.Name.ToString();

					if (toolBar.ImageList.Images.ContainsKey(key))
					{
						toolBar.Buttons[i].ImageIndex = toolBar.ImageList.Images.IndexOfKey(key);
					}
				}
			}
		}

		///<summary>Adds a drop down menu if this program requires it.</summary>
		private static void AddDropDown(ODToolBarButton toolBarButton, Program programCur)
		{
			if (programCur.Name == ProgramName.Oryx.ToString())
			{
				ContextMenu contextMenuOryx = new ContextMenu();
				MenuItem menuItemUserSettings = new MenuItem();
				menuItemUserSettings.Index = 0;
				menuItemUserSettings.Text = "User Settings";
				menuItemUserSettings.Click += Oryx.menuItemUserSettingsClick;
				contextMenuOryx.MenuItems.AddRange(new MenuItem[] {
						menuItemUserSettings,
				});
				toolBarButton.Style = ODToolBarButtonStyle.DropDownButton;
				toolBarButton.DropDownMenu = contextMenuOryx;
			}
		}
	}
}
