using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using CodeBase;
using Imedisoft.Data;
using OpenDentBusiness.FileIO;
using OpenDentBusiness.IO;
using OpenDentBusiness.WebTypes.Shared.XWeb;

namespace OpenDentBusiness {
	///<summary></summary>
	public class RecurringCharges{

		///<summary>Gets one RecurringCharge from the db.</summary>
		public static RecurringCharge GetOne(long recurringChargeNum) {
			
			return Crud.RecurringChargeCrud.SelectOne(recurringChargeNum);
		}

		///<summary>Gets a list of all RecurringCharges matching the passed in parameters. To get all RecurringCharges, pass in no parameters.
		///</summary>
		public static List<RecurringCharge> GetMany(params SQLWhere[] whereClause) {
			List<SQLWhere> listWheres=new List<SQLWhere>();
			foreach(SQLWhere where in whereClause) {
				listWheres.Add(where);
			}
			return GetMany(listWheres);
		}

		///<summary>Gets a list of all RecurringCharges matching the passed in parameters.</summary>
		public static List<RecurringCharge> GetMany(List<SQLWhere> listWheres) {
			
			string command="SELECT * FROM recurringcharge ";
			if(listWheres!=null && listWheres.Count > 0) {
				command+="WHERE "+string.Join(" AND ",listWheres);
			}
			return Crud.RecurringChargeCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(RecurringCharge recurringCharge) {
			
			return Crud.RecurringChargeCrud.Insert(recurringCharge);
		}

		///<summary></summary>
		public static void Update(RecurringCharge recurringCharge) {
			
			Crud.RecurringChargeCrud.Update(recurringCharge);
		}

		///<summary></summary>
		public static void Delete(long recurringChargeNum) {
			
			Crud.RecurringChargeCrud.Delete(recurringChargeNum);
		}

		///<summary></summary>
		public static void DeleteMany(List<RecurringCharge> listRecurringCharges) {
			if(listRecurringCharges.Count==0) {
				return;
			}
			
			string command=@"DELETE FROM recurringcharge
				WHERE RecurringChargeNum IN("+string.Join(",",listRecurringCharges.Select(x => POut.Long(x.RecurringChargeNum)))+")";
			Database.ExecuteNonQuery(command);
		}

		///<summary>This method returns true ONLY if RecurringChargesAllowedWhenNoPatBal AND CanChargeWhenNoBal are true.
		///In all other instances, it returns false. This is used as a control switch for functionality around the 
		///RecurringChargesAllowedWhenNoPatBal Preference.</summary>
		public static bool CanChargeWhenNoBal(bool isCreditCardCanChargeWhenNoBal) {
			return (Prefs.GetBool(PrefName.RecurringChargesAllowedWhenNoPatBal) && isCreditCardCanChargeWhenNoBal);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		///<summary></summary>
		public static List<RecurringCharge> Refresh(long patNum){
			
			string command="SELECT * FROM recurringcharge WHERE PatNum = "+POut.Long(patNum);
			return Crud.RecurringChargeCrud.SelectMany(command);
		}
		
		
		#endregion
		#region Modification Methods
			#region Insert
			#endregion
			#region Update
			#endregion
			#region Delete
			#endregion
		#endregion
		#region Misc Methods
		

		
		#endregion
		*/



	}

	///<summary>A data object that holds information for one recurring charge.</summary>
	public class RecurringChargeData {
		public RecurringCharge RecurringCharge;
		public DateTime DateStart;
		public int BillingCycleDay;
		public DateTime LatestPayment;
		public long ProvNum;
		public long PayPlanPatNum;
		public long PayPlanNum;
		public long Guarantor;
		public string PatName="";
		public string CCNumberMasked="";
		public DateTime CCExpiration;
		public string Address="";
		public string AddressPat="";
		public string Zip="";
		public string ZipPat="";
		public string Procedures="";
		public string XChargeToken;
		public string PayConnectToken="";
		public DateTime PayConnectTokenExp;
		public string PaySimpleToken="";
		public CreditCardSource CCSource=CreditCardSource.None;
		public DateTime RecurringChargeDate;
		public bool CanChargeWhenNoBal;
		public long PaymentType;
	}

	///<summary>A class that can be used to process recurring charges.</summary>
	public class RecurringChargerator {
		///<summary>A DateTime that can be used to give all charges processed a uniform time.</summary>
		protected DateTime _nowDateTime;
		///<summary>The program being used to process payments.</summary>
		protected Program _progCur;
		///<summary>This action gets called after each card is done being processed.</summary>
		public Action SingleCardFinished;
		///<summary>True if the Chargerator is currently running cards.</summary>
		public bool IsCharging;
		///<summary>For translating.</summary>
		private const string _lanThis="FormCreditRecurringCharges";
		///<summary>If true, the remaining charges should not be processed. The current card that is being processed will be finished before shutting
		///down.</summary>
		private bool _doShutdown;
		///<summary>A dictionary for balances of recurring charges that is not heavily modified by potentially failed credit card charges. 
		///This does not incorperate payPlanDue.</summary>
		private Dictionary<long,decimal> _dictFamBalNoPPlan=new Dictionary<long, decimal>();
		///<summary>True if running recurring charges manually from the recurring charge list. False if running automated recurring charges through 
		///OpenDentalService. </summary>
		private bool _isManual;
		///<summary>When shutting down, this is the maximum amount of time that we will wait for a single card to finish processing.</summary>
		private TimeSpan _shutdownWaitTimeout=TimeSpan.FromSeconds(10);
		
		///<summary>The current batch of recurring charges that are ready to be processed.</summary>
		public List<RecurringChargeData> ListRecurringChargeData {
			get;
			private set;
		}

		///<summary>The number of successful transactions.</summary>
		public int Success { get; private set;}

		///<summary>The number of failed transactions.</summary>
		public int Failed { get; private set;}

		///<summary>The number of cards updated by XCharge's Decline Minimizer.</summary>
		public int Updated { get; private set;}
		
		public RecurringChargerator(bool isManual) {
			_nowDateTime=MiscData.GetNowDateTime();
			_isManual=isManual;
		}

		// TODO: Show message boxes for log messages...

		///<summary>Fills the ListRecurringChargeData with recurring charges from the db. Gets recurring charges for all clinics for which the user has
		///permission to access.</summary>
		public List<RecurringChargeData> FillCharges(List<Clinic> listUserClinics) {
			DeleteNotYetCharged();
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled && Security.CurrentUser.ClinicIsRestricted) {
				listClinicNums=listUserClinics.Select(x => x.Id).ToList();
			}
			//if no clinics are selected but clinics are enabled and the user is restricted, the results will be empty so no need to run the report
			//if clinics are enabled and the user is not restricted and selects no clinics, there will not be a clinic filter in the query, so all clinics
			if(PrefC.HasClinicsEnabled && Security.CurrentUser.ClinicIsRestricted && listClinicNums.Count==0) {
				ListRecurringChargeData=new List<RecurringChargeData>();
			}
			else {
				ListRecurringChargeData=CreditCards.GetRecurringChargeList(listClinicNums,_nowDateTime);
			}
			Logger.LogVerbose("ListRecurringChargeData.Count: " + ListRecurringChargeData.Count);
			Dictionary<long,decimal> dictFamBals=new Dictionary<long,decimal>();//Keeps track of the family balance for each patient
			//Calculate the repeat charge amount and the amount to be charged for each credit card
			for(int i=ListRecurringChargeData.Count-1;i>-1;i--) {//loop through backwards since we may remove items if the charge amount is <=0
				RecurringChargeData chargeCur=ListRecurringChargeData[i];
				decimal famBalTotal=(decimal)chargeCur.RecurringCharge.FamBal;
				decimal rptChargeAmt;
				//will be 0 if this is not a payplan row, if negative don't subtract from the FamBalTotal
				decimal payPlanDue=Math.Max((decimal)chargeCur.RecurringCharge.PayPlanDue,0);
				long patNum=chargeCur.RecurringCharge.PatNum;
				//This is a very ineffecient way to get the total of the recurring charges for a card.  Example:  For the customers db, to generate a list of
				//161 cards with recurring charges due, TotalRecurringCharges is called ~2500 times.  We could modify this to get the ProcFee sum for each
				//card in the list with a single query and return the DataTable.  But since this is for HQ only, we will leave it for now.
				
				//non-HQ calculates repeating charges by the ChargeAmt on the credit card which is the sum of repeat charge and payplan payment amount
				rptChargeAmt=(decimal)chargeCur.RecurringCharge.ChargeAmt;
				
				//the Total Bal column should display the famBalTotal plus payPlanDue on the attached payplan if there is one with a positive amount due
				//if the payplan has a negative amount due, it is set to 0 above and does not subtract from famBalTotal
				//if the account balance is negative, the Total Bal column should still display the entire amount due on the payplan (if >0)
				//if the account balance is negative and there is no payplan, the Total Bal column will be the negative account balance
				if(payPlanDue>0) {//if there is a payplan attached to this repeatcharge and a positive amount due
					//negative family balance does not subtract from payplan amount due and negative payplan amount due does not subtract from family balance due
					famBalTotal=Math.Max(famBalTotal,0);
					if(PrefC.GetInt(PrefName.PayPlansVersion)==1) {//in PP v2, the PP amt due is included in the pat balance
						famBalTotal+=payPlanDue;
					}
				}
				long guarNum=chargeCur.Guarantor;
				//if guarantor is already in the dict and this is a payplan charge row, add the payPlanDue to fambal so the patient is charged
				if(dictFamBals.ContainsKey(guarNum) && payPlanDue>0
					&& PrefC.GetInt(PrefName.PayPlansVersion)==1) //in PP v2, the PP amt due is included in the pat balance
				{
					dictFamBals[guarNum]=Math.Max(dictFamBals[guarNum],0)+payPlanDue;//this way the payplan charge will be charged even if the fam bal is < 0
				}
				if(!dictFamBals.ContainsKey(guarNum)) {
					dictFamBals.Add(guarNum,famBalTotal);
				}
				//05/22/2017 Nathan and Chris discussed making it so that a credit card attached to a pay plan would charge no more than the Due Now on the
				//pay plan, but this messed up our internal accounting department because they had cards attached to payment plans that also were paying
				//down the account balance. In the future if our customers desire it, we can come up with a way so that a credit card attached to a pay plan
				//only charges the amount on the pay plan.
				decimal chargeAmt=Math.Max(dictFamBals[guarNum],payPlanDue);
				//If we CAN overcharge this patient, and rptChargeAmt WILL cause an overcharge, use rptChargeAmt. 
				if(RecurringCharges.CanChargeWhenNoBal(chargeCur.CanChargeWhenNoBal) && dictFamBals[guarNum]<=rptChargeAmt) {
					//If the user has RecurringChargesAllowedWhenNoPatBal turned on and this card has CanChargeWhenNoBal set to true
					//Ensure the charge amount is not modified to be less than the repeat charge amount so that it can credit the remainder
					//to the Patient's balance by the end.
					chargeAmt=rptChargeAmt;
				}
				else {
					//Otherwise make sure the charge amount is not more than the repeat charge amount
					chargeAmt=Math.Min(chargeAmt,rptChargeAmt);
				}
				if(chargeAmt<=0) {

					Logger.LogVerbose("Removing from ListRecurringChargeData. PatNum: " + chargeCur.RecurringCharge.PatNum + "  FamBal: " + famBalTotal
						+ "  PayPlanDue: " + payPlanDue + "  RepeatChargeAmt: " + rptChargeAmt);

					ListRecurringChargeData.RemoveAt(i);
					continue;
				}
				chargeCur.RecurringCharge.ChargeAmt=(double)chargeAmt;
				chargeCur.RecurringCharge.RepeatAmt=(double)rptChargeAmt;
				_dictFamBalNoPPlan[guarNum]=(decimal)chargeCur.RecurringCharge.FamBal;
				dictFamBals[guarNum]-=chargeAmt;//Decrease so the sum of repeating charges on all cards is not greater than the family balance
			}
			return ListRecurringChargeData;
		}

		///<summary>Processes charges for the enabled program. The object is used while a payment is processed.
		///When the program is signalled to shutdown, it will wait until an in-progress payment finishes before shutting down.</summary>
		public void SendCharges(List<RecurringChargeData> listRecurringChargeData,bool forceDuplicates) {
			if(!PaymentsWithinLockDate(listRecurringChargeData)) {
				return;
			}
			Prefs.Set(PrefName.RecurringChargesBeginDateTime,MiscData.GetNowDateTime());
			try {
				IsCharging=true;
				InsertRecurringCharges(listRecurringChargeData);
				ClearStats();
				StringBuilder strBuilderResultFileXCharge=new StringBuilder();
				StringBuilder strBuilderResultFilePayConnect=new StringBuilder();
				StringBuilder strBuilderResultFilePaySimple=new StringBuilder();
				List<long> listClinicNumsBadCredentialsXCharge=new List<long>();
				foreach(RecurringChargeData chargeData in listRecurringChargeData) {
					bool useXCharge=(!string.IsNullOrEmpty(chargeData.XChargeToken) && Programs.IsEnabled(ProgramName.Xcharge));
					bool usePayConnect=(!string.IsNullOrEmpty(chargeData.PayConnectToken) && Programs.IsEnabled(ProgramName.PayConnect));
					bool usePaySimple=(!string.IsNullOrEmpty(chargeData.PaySimpleToken) && Programs.IsEnabled(ProgramName.PaySimple));
					if(useXCharge) {
						_progCur=Programs.GetCur(ProgramName.Xcharge);
						bool doForceDuplicatesThisCharge=forceDuplicates || (!_isManual && PIn.Bool(ProgramProperties.GetPropValForClinicOrDefault(
							_progCur.Id,ProgramProperties.PropertyDescs.XCharge.XChargeForceRecurringCharge,chargeData.RecurringCharge.ClinicNum)));
						SendXCharge(chargeData,doForceDuplicatesThisCharge,strBuilderResultFileXCharge,listClinicNumsBadCredentialsXCharge);
					}
					else if(usePayConnect) {
						_progCur=Programs.GetCur(ProgramName.PayConnect);
						bool doForceDuplicatesThisCharge=forceDuplicates || (!_isManual && PIn.Bool(ProgramProperties.GetPropValForClinicOrDefault(
							_progCur.Id,PayConnect.ProgramProperties.PayConnectForceRecurringCharge,chargeData.RecurringCharge.ClinicNum)));
						SendPayConnect(chargeData,doForceDuplicatesThisCharge,strBuilderResultFilePayConnect);
					}
					else if(usePaySimple) {
						_progCur=Programs.GetCur(ProgramName.PaySimple);
						SendPaySimple(chargeData,strBuilderResultFilePaySimple);
					}
					RecurringCharges.Update(chargeData.RecurringCharge);
					if(_doShutdown) {
						DeleteNotYetCharged();
						break;
					}
					SingleCardFinished?.Invoke();
				}
				WriteResultsToFiles(strBuilderResultFileXCharge,strBuilderResultFilePayConnect,strBuilderResultFilePaySimple);
			}
			finally {
				IsCharging=false;
				Prefs.Set(PrefName.RecurringChargesBeginDateTime,"");
			}
		}

		///<summary>Writes the results to appropriate files.</summary>
		private void WriteResultsToFiles(StringBuilder strBuilderResultFileXCharge,StringBuilder strBuilderResultFilePayConnect,
			StringBuilder strBuilderResultFilePaySimple) 
		{
			if(strBuilderResultFileXCharge.Length > 0) {
				try {
					string xPath=Programs.GetProgramPath(Programs.GetCur(ProgramName.Xcharge));
					File.WriteAllText(ODFileUtils.CombinePaths(Path.GetDirectoryName(xPath),"RecurringChargeResult.txt"),strBuilderResultFileXCharge.ToString());
				}
				catch {
					//Do nothing cause this is just for internal use.
				}
			}
			if(strBuilderResultFilePayConnect.Length > 0) {
				string payConnectResultDir=FileAtoZ.CombinePaths(FileAtoZ.GetPreferredAtoZpath(),"PayConnect");
				string payConnectResultFile=FileAtoZ.CombinePaths(payConnectResultDir,"RecurringChargeResult.txt");
				try {
					if(!Directory.Exists(payConnectResultDir)) {
						Directory.CreateDirectory(payConnectResultDir);
					}
					FileAtoZ.WriteAllText(payConnectResultFile,strBuilderResultFilePayConnect.ToString());
				}
				catch {
					//Do nothing cause this is just for internal use.
				}
			}
			if(strBuilderResultFilePaySimple.Length > 0) {
				string paySimpleResultDir=FileAtoZ.CombinePaths(FileAtoZ.GetPreferredAtoZpath(),"PaySimple");
				string paySimpleResultFile=FileAtoZ.CombinePaths(paySimpleResultDir,"RecurringChargeResult.txt");
				try {
					if(!Directory.Exists(paySimpleResultDir)) {
						Directory.CreateDirectory(paySimpleResultDir);
					}
					FileAtoZ.WriteAllText(paySimpleResultFile,strBuilderResultFilePaySimple.ToString());
				}
				catch {
					//Do nothing cause this is just for internal use.
				}
			}
		}

		///<summary>Charges the credit card passed in using XCharge.</summary>
		public void SendXCharge(RecurringChargeData chargeData,bool forceDuplicates,StringBuilder strBuilderResultFile,
			List<long> listClinicNumsBadCredentials) 
		{
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			int tokenCount=CreditCards.GetXChargeTokenCount(chargeData.XChargeToken,false);
			if(chargeData.XChargeToken!="" && tokenCount!=1) {
				string msg=(tokenCount>1)?"A duplicate token was found":"A token no longer exists";
				MarkFailed(chargeData,msg+", the card cannot be charged.");
				return;
			}
			Patient patCur=Patients.GetPat(chargeData.RecurringCharge.PatNum);
			if(patCur==null) {
				MarkFailed(chargeData,"Unable to find patient"+" "+chargeData.RecurringCharge.PatNum);
				return;
			}
			bool wasChargeAttempted;
			StringBuilder strBuilderResultText;
			double amount;
			long xWebResponseNum=0;
			StringBuilder receipt;
			CreditCardSource ccSource;
			if(_isManual) {
				wasChargeAttempted=ProcessCardXChargeClientProgram(chargeData,forceDuplicates,strBuilderResultFile,listClinicNumsBadCredentials,
					out strBuilderResultText,out amount,out receipt);
				ccSource=CreditCardSource.XServer;
			}
			else {
				wasChargeAttempted=ProcessCardXWebDTG(chargeData,forceDuplicates,strBuilderResultFile,out strBuilderResultText,
					out amount,out receipt,out xWebResponseNum);
				ccSource=CreditCardSource.XWeb;
			}
			if(wasChargeAttempted) {
				CreatePayment(patCur,chargeData,strBuilderResultText.ToString(),amount,receipt.ToString(),ccSource,xWebResponseNum:xWebResponseNum);
			}
		}

		///<summary>Charges the card using the XCharge client executable. Returns true if the charge was successfully attempted.</summary>
		private bool ProcessCardXChargeClientProgram(RecurringChargeData chargeData,bool forceDuplicates,StringBuilder strBuilderResultFile,
			List<long> listClinicNumsBadCredentials,out StringBuilder strBuilderResultText,out double amount,out StringBuilder receipt) 
		{
			strBuilderResultText=new StringBuilder();
			amount=0;
			receipt=new StringBuilder();
			long clinicNumCur=0;
			if(PrefC.HasClinicsEnabled) {
				//this is patient.ClinicNum or if it's a payplan row it's the ClinicNum from one of the payplancharges on the payplan
				clinicNumCur=chargeData.RecurringCharge.ClinicNum;//If clinics were enabled but no longer are, use credentials for headquarters.
			}
			if(listClinicNumsBadCredentials.Contains(clinicNumCur)) {//username or password is blank, don't try to process
				MarkFailed(chargeData,"The X-Charge Username or Password for the clinic has not been set.",LogLevel.Info);
				return false;
			}
			string username=ProgramProperties.GetPropVal(_progCur.Id,"Username",clinicNumCur);
			string password=ProgramProperties.GetPropVal(_progCur.Id,"Password",clinicNumCur);
			if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {//clinicNumCur is not in listClinicNumsBadCredentials yet
				string clinicAbbr="Headquarters";
				if(clinicNumCur>0) {
					clinicAbbr=Clinics.GetAbbr(clinicNumCur);
				}
				MarkFailed(chargeData,"The X-Charge Username or Password for the following clinic has not been set"+":\r\n"+clinicAbbr+"\r\n"
					+"All charges for that clinic will be skipped.");
				listClinicNumsBadCredentials.Add(clinicNumCur);
				return false;
			}
			password=MiscUtils.Decrypt(password);
			string resultfile=Storage.GetTempFileName(".txt");
			try {
				File.Delete(resultfile);//delete the old result file.
			}
			catch {
				//Probably did not have permissions to delete the file.  Don't do anything, because a message will show telling them that the cards left in the grid failed.
				//They will then go try and run the cards in the Account module and will then get a detailed message telling them what is wrong.
				MarkFailed(chargeData,"Unable to delete result file.",LogLevel.Info);
				return false;
			}
			string xPath=Programs.GetProgramPath(_progCur);
			ProcessStartInfo info=new ProcessStartInfo(xPath);
			info.Arguments="";
			double amt=chargeData.RecurringCharge.ChargeAmt;
			DateTime exp=chargeData.CCExpiration;
			string address=chargeData.Address;
			string addressPat=chargeData.AddressPat;
			string zip=chargeData.Zip;
			string zipPat=chargeData.ZipPat;
			long creditCardNum=chargeData.RecurringCharge.CreditCardNum;
			info.Arguments+="/AMOUNT:"+amt.ToString("F2")+" /LOCKAMOUNT ";
			info.Arguments+="/TRANSACTIONTYPE:PURCHASE /LOCKTRANTYPE ";
			if(chargeData.XChargeToken!="") {
				info.Arguments+="/XCACCOUNTID:"+chargeData.XChargeToken+" ";
				info.Arguments+="/RECURRING ";
				info.Arguments+="/GETXCACCOUNTIDSTATUS ";
			}
			else {
				info.Arguments+="/ACCOUNT:"+chargeData.CCNumberMasked+" ";
			}
			if(exp.Year>1880) {
				info.Arguments+="/EXP:"+exp.ToString("MMyy")+" ";
			}
			if(address!="") {
				info.Arguments+="\"/ADDRESS:"+address+"\" ";
			}
			else if(addressPat!="") {
				info.Arguments+="\"/ADDRESS:"+addressPat+"\" ";
			}
			//If ODHQ, do not add the zip code if the customer has an active foreign registration key
			bool hasForeignKey=false;
			if(zip!="" && !hasForeignKey) {
				info.Arguments+="\"/ZIP:"+zip+"\" ";
			}
			else if(zipPat!="" && !hasForeignKey) {
				info.Arguments+="\"/ZIP:"+zipPat+"\" ";
			}
			info.Arguments+="/RECEIPT:Pat"+chargeData.RecurringCharge.PatNum+" ";//aka invoice#
			info.Arguments+="\"/CLERK:"+Security.CurrentUser.UserName+" R\" /LOCKCLERK ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+username+" ";
			info.Arguments+="/PASSWORD:"+password+" ";
			info.Arguments+="/HIDEMAINWINDOW ";
			info.Arguments+="/AUTOPROCESS ";
			info.Arguments+="/SMALLWINDOW ";
			info.Arguments+="/AUTOCLOSE ";
			info.Arguments+="/NORESULTDIALOG ";
			if(forceDuplicates) {
				info.Arguments+="/ALLOWDUPLICATES ";
			}
			info.Arguments+="/RECEIPTINRESULT ";
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			while(!process.HasExited) {
				Thread.Sleep(10);
			}
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			bool updateCard=false;
			string newAccount="";
			DateTime newExpiration=new DateTime();
			bool isSuccess=false;
			strBuilderResultFile.AppendLine("PatNum: "+chargeData.RecurringCharge.PatNum+" Name: "+chargeData.PatName);
			try {
				using(TextReader reader = new StreamReader(resultfile)) {
					string line=reader.ReadLine();
					while(line!=null) {
						if(!line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
							strBuilderResultText.AppendLine(line);
						}
						if(line.StartsWith("RESULT=")) {
							if(line=="RESULT=SUCCESS") {
								isSuccess=true;
							}
							else {
								isSuccess=false;
							}
						}
						else if(line=="XCACCOUNTIDUPDATED=T") {//Decline minimizer updated the account information since the last time this card was charged
							updateCard=true;
							Updated++;
						}
						else if(line.StartsWith("ACCOUNT=")) {
							newAccount=line.Substring("ACCOUNT=".Length);
						}
						else if(line.StartsWith("EXPIRATION=")) {
							string expStr=line.Substring("EXPIRATION=".Length);//Expiration should be MMYY
							newExpiration=new DateTime(PIn.Int("20"+expStr.Substring(2)),PIn.Int(expStr.Substring(0,2)),1);//First day of the month
						}
						else if(line.StartsWith("APPROVEDAMOUNT=")) {
							amount=PIn.Double(line.Substring("APPROVEDAMOUNT=".Length));
						}
						else if(line.StartsWith("RECEIPT=")) {
							receipt.Append(line.Substring("RECEIPT=".Length));
							receipt.Replace("\\n","\n");//The receipts from X-Charge escape newline characters.
							receipt.Replace("\r","");//remove any existing \r's before replacing \n's with \r\n's
							receipt.Replace("\n","\r\n");
						}
						line=reader.ReadLine();
					}
					strBuilderResultFile.AppendLine(strBuilderResultText.ToString());
					strBuilderResultFile.AppendLine();
					if(isSuccess) {
						chargeData.RecurringCharge.ChargeStatus=RecurringChargeStatus.ChargeSuccessful;
						Success++;
					}
					else {
						MarkFailed(chargeData,"Result from XCharge:"+" "+strBuilderResultText.ToString(),LogLevel.Info);
					}
				}
			}
			catch(Exception ex) {
				MarkFailed(chargeData,"XCharge error:"+" "+ex.Message,LogLevel.Info);
				return false;
			}
			//If the decline minimizer updated the card, returned a value in the ACCOUNT field, and returned a valid exp date.  Update our record.
			if(updateCard && newAccount!="" && newExpiration.Year>1880) {
				CreditCard creditCardCur=CreditCards.GetOne(creditCardNum);
				//Update the payment note with the changes.
				if(creditCardCur.CCNumberMasked != newAccount) {
					strBuilderResultText.AppendLine("Account number changed from"+" "+creditCardCur.CCNumberMasked+" "
						+"to"+" "+newAccount);
				}
				if(creditCardCur.CCExpiration != newExpiration) {
					strBuilderResultText.AppendLine("Expiration changed from"+" "+creditCardCur.CCExpiration.ToString("MMyy")+" "
						+"to"+" "+newExpiration.ToString("MMyy"));
				}
				creditCardCur.CCNumberMasked=newAccount;
				creditCardCur.CCExpiration=newExpiration;
				CreditCards.Update(creditCardCur);
			}
			return true;
		}

		///<summary>Charges the card using the XWeb Direct to Gateway API. Returns true if the charge was successfully attempted.</summary>
		private bool ProcessCardXWebDTG(RecurringChargeData chargeData,bool forceDuplicates,StringBuilder strBuilderResultFile,
			out StringBuilder strBuilderResultText,out double amount,out StringBuilder receipt,out long xWebResponseNum) 
		{
			receipt=new StringBuilder();//Automated payments won't have receipts
			strBuilderResultText=new StringBuilder();
			try {
				XWebResponse response=XWebs.MakePaymentWithAlias(chargeData.RecurringCharge.PatNum,"Made from automated recurring charge",
					chargeData.RecurringCharge.ChargeAmt,chargeData.RecurringCharge.CreditCardNum,false,ChargeSource.RecurringCharges,forceDuplicates);				
				amount=response.Amount;
				if(response.XWebResponseCode==XWebResponseCodes.Approval) {
					chargeData.RecurringCharge.ChargeStatus=RecurringChargeStatus.ChargeSuccessful;
					Success++;
				}
				else {
					MarkFailed(chargeData,"Response from XWeb:"+" "+response.XWebResponseCode.ToString(),LogLevel.Info);
					response.PayNote+="\r\n"+"Response from XWeb:"+" "+response.XWebResponseCode.ToString();
				}
				strBuilderResultText.Append(response.GetFormattedNote(true));
				xWebResponseNum=response.XWebResponseNum;
				return true;
			}
			catch(Exception ex) {
				MarkFailed(chargeData,"Unable to charge card.\r\nError Message: "+ex.Message,LogLevel.Error);
				amount=0;
				xWebResponseNum=0;
				//An error code of XWebDTGFailed means that communication with XWeb was successful so the charge was technically attempted, return true.
				if(ex is ODException && ((ODException)ex).ErrorCodeAsEnum!=ODException.ErrorCodes.XWebDTGFailed) {
					return false;
				}
				strBuilderResultText.Append(ex.Message);
				return true;//The error came from XWeb.
			}
		}
		
		///<summary>Charges the credit cards passed in using PayConnect.</summary>
		public void SendPayConnect(RecurringChargeData chargeData,bool forceDuplicates,StringBuilder strBuilderResultFile) {
			Dictionary<long,string> dictClinicNumDesc=new Dictionary<long,string>();
			if(PrefC.HasClinicsEnabled) {
				dictClinicNumDesc=Clinics.GetClinicsNoCache().ToDictionary(x => x.Id,x => x.Description);
			}
			dictClinicNumDesc[0]=Prefs.GetString(PrefName.PracticeTitle);
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			bool isPayConnectToken=true;
			string tokenOrCCMasked=chargeData.PayConnectToken;
			int tokenCount=CreditCards.GetPayConnectTokenCount(tokenOrCCMasked);
			if(tokenOrCCMasked!="" && tokenCount!=1) {
				string msg=(tokenCount>1)?"A duplicate token was found":"A token no longer exists";
				MarkFailed(chargeData,msg+", the card cannot be charged.");
				return;
			}
			long patNum=chargeData.RecurringCharge.PatNum;
			Patient patCur=Patients.GetPat(patNum);
			if(patCur==null) {
				MarkFailed(chargeData,"Unable to find patient"+" "+chargeData.RecurringCharge.PatNum);
				return;
			}
			DateTime exp=chargeData.PayConnectTokenExp;
			if(tokenOrCCMasked=="") {
				isPayConnectToken=false;
				tokenOrCCMasked=chargeData.CCNumberMasked;
				exp=chargeData.CCExpiration;
			}
			decimal amt=(decimal)chargeData.RecurringCharge.ChargeAmt;
			string zip=chargeData.Zip;
			long clinicNumCur=chargeData.RecurringCharge.ClinicNum;
			double amount=0;
			//request a PayConnect token, if a token was already saved PayConnect will return the same token,
			//otherwise replace CCNumberMasked with the returned token if the sale successful
			PayConnectService.creditCardRequest payConnectRequest=PayConnect.BuildSaleRequest(
				amt,tokenOrCCMasked,exp.Year,exp.Month,
				patCur.GetNameFLnoPref(),"",zip,null,
				PayConnectService.transType.SALE,"",true,
				isForced:forceDuplicates);
			//clinicNumCur could be 0, and the practice level or 'Headquarters' PayConnect credentials would be used for this charge
			PayConnectService.transResponse payConnectResponse=PayConnect.ProcessCreditCard(payConnectRequest,clinicNumCur,
				x => MarkFailed(chargeData,x));
			StringBuilder strBuilderResultText=new StringBuilder();//this payment's result text, used in payment note and then appended to file string builder
			strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+patCur.GetNameFLnoPref());
			if(payConnectResponse==null || payConnectResponse.Status==null) {
				MarkFailed(chargeData,"Transaction Failed, unknown error",LogLevel.Info);
				if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
					strBuilderResultText.AppendLine("CLINIC="+dictClinicNumDesc[clinicNumCur]);
				}
				strBuilderResultText.AppendLine("Transaction Failed, unknown error");
				strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
			}
			else if(payConnectResponse.Status.code!=0) {//error in transaction
				MarkFailed(chargeData,"Transaction Failed, error status"+" "+payConnectResponse.Status.description,LogLevel.Info);
				if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
					strBuilderResultText.AppendLine("CLINIC="+dictClinicNumDesc[clinicNumCur]);
				}
				strBuilderResultText.AppendLine("Transaction Type"+": "+PayConnectService.transType.SALE.ToString());
				strBuilderResultText.AppendLine("Status"+": "+payConnectResponse.Status.description);
				strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
			}
			else {//approved sale, update CC, add result to file string builder		
				chargeData.RecurringCharge.ChargeStatus=RecurringChargeStatus.ChargeSuccessful;
				Success++;
				CreditCard ccCur=CreditCards.GetOne(chargeData.RecurringCharge.CreditCardNum);
				UpdateCreditCardPayConnect(ccCur,payConnectResponse);
				//add to strbuilder that will be written to txt file and to the payment note
				if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
					strBuilderResultText.AppendLine("CLINIC="+dictClinicNumDesc[clinicNumCur]);
				}
				strBuilderResultText.AppendLine("RESULT="+payConnectResponse.Status.description);
				strBuilderResultText.AppendLine("TRANS TYPE="+PayConnectService.transType.SALE.ToString());
				strBuilderResultText.AppendLine("AUTH CODE="+payConnectResponse.AuthCode);
				strBuilderResultText.AppendLine("ENTRY=MANUAL");
				strBuilderResultText.AppendLine("CLERK="+Security.CurrentUser.UserName);
				strBuilderResultText.AppendLine("TRANSACTION NUMBER="+payConnectResponse.RefNumber);
				if(ccCur!=null) {
					strBuilderResultText.AppendLine("ACCOUNT="+ccCur.CCNumberMasked);//XXXXXXXXXXXX1234, all but last four numbers replaced with X's
				}
				if(payConnectResponse.PaymentToken!=null && payConnectResponse.PaymentToken.Expiration!=null) {
					strBuilderResultText.AppendLine("EXPIRATION="+payConnectResponse.PaymentToken.Expiration.month.ToString().PadLeft(2,'0')
						+(payConnectResponse.PaymentToken.Expiration.year%100));
				}
				if(isPayConnectToken) {
					strBuilderResultText.AppendLine("CARD TYPE=PayConnect Token");
				}
				else {
					strBuilderResultText.AppendLine("CARD TYPE="+CreditCardUtils.GetCardType(tokenOrCCMasked));
				}
				strBuilderResultText.AppendLine("AMOUNT="+payConnectRequest.Amount.ToString("F2"));
				amount=(double)payConnectRequest.Amount;
			}
			string receipt=PayConnect.BuildReceiptString(payConnectRequest,payConnectResponse,null,clinicNumCur);
			CreatePayment(patCur,chargeData,strBuilderResultText.ToString(),amount,receipt,CreditCardSource.PayConnect);
			strBuilderResultFile.AppendLine(strBuilderResultText.ToString());
		}

		///<summary>Charges the credit cards passed in using PaySimple.</summary>
		public void SendPaySimple(RecurringChargeData chargeData,StringBuilder strBuilderResultFile) {
			Dictionary<long,string> dictClinicNumDesc=new Dictionary<long,string>();
			if(PrefC.HasClinicsEnabled) {
				dictClinicNumDesc=Clinics.GetClinicsNoCache().ToDictionary(x => x.Id,x => x.Description);
			}
			dictClinicNumDesc[0]=Prefs.GetString(PrefName.PracticeTitle);
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			string paySimpleAccountId=chargeData.PaySimpleToken;
			int tokenCount=CreditCards.GetPaySimpleTokenCount(paySimpleAccountId,chargeData.CCSource==CreditCardSource.PaySimpleACH);
			if(string.IsNullOrWhiteSpace(paySimpleAccountId) || tokenCount!=1) {
				string msg=(tokenCount>1)?"A duplicate token was found":"A token was not found";
				MarkFailed(chargeData,msg+", the card cannot be charged.");
				return;
			}
			long patNum=chargeData.RecurringCharge.PatNum;
			Patient patCur=Patients.GetPat(patNum);
			if(patCur==null) {
				MarkFailed(chargeData,"Unable to find patient"+" "+chargeData.RecurringCharge.PatNum);
				return;
			}
			DateTime exp=chargeData.CCExpiration;//We don't have a PaySimpleTokenExpiration, so use the CC's stored one.
			decimal amt=(decimal)chargeData.RecurringCharge.ChargeAmt;
			string zip=chargeData.Zip;
			long clinicNumCur=chargeData.RecurringCharge.ClinicNum;
			double resultAmt=0;
			StringBuilder strBuilderResultText=new StringBuilder();//this payment's result text, used in payment note and then appended to file string builder
			strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+patCur.GetNameFLnoPref());
			string receipt="";
			try {
				PaySimple.ApiResponse response=PaySimple.MakePaymentByToken(patCur,new CreditCard() {
					CreditCardNum=chargeData.RecurringCharge.CreditCardNum,
					PaySimpleToken=paySimpleAccountId,
					PatNum=patCur.PatNum,
					CCSource=chargeData.CCSource,
				},amt,clinicNumCur);
				if(response==null) {
					//If this happens, the API method returned successfully and somehow we didn't create a response.
					//The intent of the PaySimple API integration is that we always get a response or throw exceptions.
					throw new ODException("Unknown error making payment.  Please contact support.");
				}
				//approved sale, update CC, add result to file string builder		
				chargeData.RecurringCharge.ChargeStatus=RecurringChargeStatus.ChargeSuccessful;
				Success++;
				CreditCard ccCur=CreditCards.GetOne(chargeData.RecurringCharge.CreditCardNum);
				if(ccCur!=null && ccCur.PaySimpleToken!=response.PaySimpleToken) {
					ccCur.PaySimpleToken=response.PaySimpleToken;
					CreditCards.Update(ccCur);
				}
				//add to strbuilder that will be written to txt file and to the payment note
				string clinicDesc="";
				if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
					clinicDesc=dictClinicNumDesc[clinicNumCur];
				}
				int ccExpYear=-1;
				int ccExpMonth=-1;
				string expStr="";
				if(ccCur.CCSource==CreditCardSource.PaySimple) {
					ccExpYear=ccCur.CCExpiration.Year;
					ccExpMonth=ccCur.CCExpiration.Month;
					expStr=ccExpMonth.ToString().PadLeft(2,'0')+(ccExpYear%100);
				}
				strBuilderResultText.AppendLine(response.ToNoteString(clinicDesc,"Manual",Security.CurrentUser.UserName,expStr,"PaySimple Token"));
				resultAmt=(double)response.Amount;
				response.BuildReceiptString(ccCur.CCNumberMasked,ccExpMonth,ccExpYear,"",clinicNumCur,isACH: ccCur.CCSource==CreditCardSource.PaySimpleACH);
				receipt=response.TransactionReceipt;
			}
			catch(Exception ex) {
				MarkFailed(chargeData,"Error processing card:"+" "+ex.Message,LogLevel.Info);
				string clinicDesc="";
				if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
					clinicDesc=dictClinicNumDesc[clinicNumCur];
				}
				AddErrorToStrb(strBuilderResultText,ex.Message,clinicDesc);
			}
			finally {
				CreatePayment(patCur,chargeData,strBuilderResultText.ToString(),resultAmt,receipt,chargeData.CCSource);
				strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
			}
		}

		///<summary>For PaySimple only. Adds the error message to the StringBuilder.</summary>
		private void AddErrorToStrb(StringBuilder strb,string errorMsg,string clinicDesc) {
			strb.AppendLine("Transaction Type"+": "+PaySimple.TransType.SALE.ToString());
			if(!string.IsNullOrWhiteSpace(clinicDesc)) {
				strb.AppendLine("CLINIC="+clinicDesc);
			}
			strb.AppendLine("Error"+": "+errorMsg);
		}

		///<summary>Updates the credit card's masked number and expiration.</summary>
		private void UpdateCreditCardPayConnect(CreditCard ccCur,PayConnectService.transResponse payConnectResponse) {
			if(ccCur==null || payConnectResponse==null || payConnectResponse.PaymentToken==null || payConnectResponse.PaymentToken.Expiration==null) {
				return;
			}
			PayConnectService.expiration payConnectExp =payConnectResponse.PaymentToken.Expiration;
			//if stored CC token or token expiration are different than those returned by PayConnect, update the stored CC
			if(ccCur.PayConnectToken!=payConnectResponse.PaymentToken.TokenId
				|| ccCur.PayConnectTokenExp.Year!=payConnectExp.year
				|| ccCur.PayConnectTokenExp.Month!=payConnectExp.month)
			{
				ccCur.PayConnectToken=payConnectResponse.PaymentToken.TokenId;
				ccCur.PayConnectTokenExp=new DateTime(payConnectExp.year,payConnectExp.month,
					DateTime.DaysInMonth(payConnectExp.year,payConnectExp.month));
				ccCur.CCNumberMasked=ccCur.PayConnectToken.Substring(ccCur.PayConnectToken.Length-4).PadLeft(ccCur.PayConnectToken.Length,'X');
				ccCur.CCExpiration=ccCur.PayConnectTokenExp;
				CreditCards.Update(ccCur);
			}
		}

		private void MarkFailed(RecurringChargeData chargeData,string errorMsg,LogLevel logLevel=LogLevel.Error) {
			if(chargeData.RecurringCharge.ChargeStatus==RecurringChargeStatus.NotYetCharged) {
				chargeData.RecurringCharge.ChargeStatus=RecurringChargeStatus.ChargeFailed;
				Failed++;
			}
			chargeData.RecurringCharge.ErrorMsg=chargeData.RecurringCharge.ErrorMsg?.AppendLine(errorMsg)??errorMsg;
			Logger.Write(logLevel,errorMsg + (errorMsg[errorMsg.Length-1]=='\n'?"":"\r\n")+"  "+"Patient:"+" "+chargeData.PatName);
		}

		///<summary>Sets the fields that are keeping count of the number of successes and failures.</summary>
		private void ClearStats() {
			Success=0;
			Failed=0;
			Updated=0;
		}

		///<summary>For each RecurringChargeData, inserts a RecurringCharge.</summary>
		private void InsertRecurringCharges(List<RecurringChargeData> listChargeData) {
			foreach(RecurringChargeData chargeCur in listChargeData) {
				chargeCur.RecurringCharge.DateTimeCharge=_nowDateTime;
				RecurringCharges.Insert(chargeCur.RecurringCharge);//Using individual insert because we need the primary key set
			}
		}

		///<summary>Tests the recurring charges with newly calculated pay dates.  If there's a date violation, a warning shows and false is returned.</summary>
		public bool PaymentsWithinLockDate(List<RecurringChargeData> listChargeData) {
			List<string> warnings=new List<string>();
			foreach(RecurringChargeData chargeCur in listChargeData) {
				//Calculate what the new pay date will be.
				DateTime newPayDate=GetPayDate(chargeCur);
				//Test if the user can create a payment with the new pay date.
				bool isBeforeLockDate;
				if(Security.CurrentUser.Id > 0) {
					isBeforeLockDate=(!Security.IsAuthorized(Permissions.PaymentCreate,newPayDate,true));
				}
				else {
					isBeforeLockDate=Security.IsGlobalDateLock(Permissions.PaymentCreate,newPayDate,true);
				}
				if(isBeforeLockDate) {
					if(warnings.Count==0) {
						warnings.Add("Lock date limitation is preventing the recurring charges from running"+":");
					}
					warnings.Add(newPayDate.ToShortDateString()+" - "+chargeCur.RecurringCharge.PatNum+": "+chargeCur.PatName+" - "
						+chargeCur.RecurringCharge.FamBal.ToString("c")+" - "+chargeCur.RecurringCharge.ChargeAmt.ToString("c"));
				}
			}
			if(warnings.Count>0) {
				//Show the warning message.  This allows the user the ability to unhighlight rows or go change the date limitation.
				Logger.LogError(string.Join("\r\n",warnings));
				return false;
			}
			return true;
		}

		///<summary>Inserts a payment and paysplit, called after processing a payment through either X-Charge or PayConnect. selectedIndex is the current 
		///selected index of the gridMain row this payment is for.</summary>
		protected void CreatePayment(Patient patCur,RecurringChargeData recCharge,string note,double amount,string receipt,CreditCardSource ccSource,
			long xWebResponseNum=0)
		{
			Payment paymentCur=new Payment();
			paymentCur.DateEntry=_nowDateTime.Date;
			paymentCur.PayDate=GetPayDate(recCharge);
			paymentCur.RecurringChargeDate=recCharge.RecurringChargeDate;
			paymentCur.PatNum=patCur.PatNum;
			//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
			//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
			paymentCur.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				paymentCur.ClinicNum=recCharge.RecurringCharge.ClinicNum;
			}
			//ClinicNum can be 0 for 'Headquarters' or clinics not enabled, PayType will be account module pref if set OR the 0 clinic or headquarters 
			//PayType if using PayConnect
			string ppPayTypeDesc="PaymentType";
			if(ccSource==CreditCardSource.PaySimple) {
				ppPayTypeDesc=PaySimple.PropertyDescs.PaySimplePayTypeCC;
			}
			else if(ccSource==CreditCardSource.PaySimpleACH) {
				ppPayTypeDesc=PaySimple.PropertyDescs.PaySimplePayTypeACH;
			}
			if(ccSource!=CreditCardSource.PaySimpleACH) {
				paymentCur.PayType=Prefs.GetLong(PrefName.RecurringChargesPayTypeCC);
			}
			if(paymentCur.PayType==0) {//Pref default not set or this is ACH
				paymentCur.PayType=PIn.Int(ProgramProperties.GetPropVal(_progCur.Id,ppPayTypeDesc,paymentCur.ClinicNum));
			}
			if(recCharge.PaymentType!=0) {
				paymentCur.PayType=recCharge.PaymentType;//Set the payment type to the recurring charges payment type override.
			}
			paymentCur.PayAmt=amount;
			double payPlanDue=recCharge.RecurringCharge.PayPlanDue;
			paymentCur.PayNote=note;
			paymentCur.IsRecurringCC=true;
			paymentCur.PaymentSource=ccSource;
			paymentCur.Receipt=receipt;
			Payments.Insert(paymentCur);
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,paymentCur.PatNum,patCur.GetNameLF()+", "
				+paymentCur.PayAmt.ToString("c")+", "+"created from the Recurring Charges List");
			recCharge.RecurringCharge.PayNum=paymentCur.PayNum;
			if(xWebResponseNum>0) {
				XWebResponse xWebResponse=XWebResponses.GetOne(xWebResponseNum);
				if(xWebResponse!=null) {
					xWebResponse.PaymentNum=paymentCur.PayNum;
					XWebResponses.Update(xWebResponse);
				}
			}
			long provNumPayPlan=recCharge.ProvNum;//for payment plans only
			//Regular payments need to apply to the provider that the family owes the most money to.
			//Also get provNum for provider owed the most if the card is for a payplan and for other repeating charges and they will be charged for both
			//the payplan and regular repeating charges
			long provNumRegPmts=0;
			if(provNumPayPlan==0 || paymentCur.PayAmt-payPlanDue>0) {//provNum==0 for cards not attached to a payplan.
				DataTable dt=Patients.GetPaymentStartingBalances(patCur.Guarantor,paymentCur.PayNum);
				double highestAmt=0;
				for(int j = 0;j<dt.Rows.Count;j++) {
					double afterIns=PIn.Double(dt.Rows[j]["AfterIns"].ToString());
					if(highestAmt>=afterIns) {
						continue;
					}
					highestAmt=afterIns;
					if(Prefs.GetBool(PrefName.RecurringChargesUsePriProv)) {
						provNumRegPmts=patCur.PriProv;
					}
					else {
						provNumRegPmts=PIn.Long(dt.Rows[j]["ProvNum"].ToString());
					}
				}
			}
			long splitPatNum=paymentCur.PatNum;
			long patNumPayPlan=recCharge.PayPlanPatNum;//for payment plans only
			if(patNumPayPlan!=0) {//Add the payplan's patnum to the paysplit. 
				splitPatNum=patNumPayPlan;
			}
			PaySplit split=new PaySplit();
			split.PatNum=splitPatNum;
			split.ClinicNum=paymentCur.ClinicNum;
			split.PayNum=paymentCur.PayNum;
			split.DatePay=paymentCur.PayDate;
			split.PayPlanNum=recCharge.PayPlanNum;
			if(split.PayPlanNum==0 || payPlanDue<=0) {//this row is not for a payplan or there is no payplandue
				split.PayPlanNum=0;//if the payplan does not have any amount due, don't attach split to payplan
				split.SplitAmt=paymentCur.PayAmt;
				paymentCur.PayAmt-=split.SplitAmt;
				split.ProvNum=provNumRegPmts;
				split.ClinicNum=patCur.ClinicNum;
				_dictFamBalNoPPlan[patCur.Guarantor]-=(decimal)split.SplitAmt;
				if(_dictFamBalNoPPlan[patCur.Guarantor]<0 && RecurringCharges.CanChargeWhenNoBal(recCharge.CanChargeWhenNoBal)) {
					split.UnearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
					split.ProvNum=0;//unearned paysplits are not associated to a provider unless the user explicitly does so.
				}
			}
			else {//row includes a payplan amount due, could also include a regular repeating pay amount as part of the total charge amount
				split.SplitAmt=Math.Min(payPlanDue,paymentCur.PayAmt);//ensures a split is not more than the actual payment amount
				paymentCur.PayAmt-=split.SplitAmt;//subtract the payplan pay amount from the total payment amount and create another split not attached to payplan
				split.ProvNum=provNumPayPlan;
			}
			PaySplits.Insert(split);
			//if the above split was for a payment plan and there is still some PayAmt left, insert another split not attached to the payplan
			if(paymentCur.PayAmt>0) {
				split=new PaySplit();
				split.PatNum=paymentCur.PatNum;
				split.ClinicNum=patCur.ClinicNum;
				split.PayNum=paymentCur.PayNum;
				split.DatePay=paymentCur.PayDate;
				split.ProvNum=provNumRegPmts;
				split.SplitAmt=paymentCur.PayAmt;
				split.PayPlanNum=0;
				//If family balance is 0 or less, and the preference RecurringChargesAllowedWhenNoPatBal is on, with CanChargeWhenNoBal set to true for the card
				//then the created split will be made an Unearned Prepayment type and, since this instance is garaunteed to have paid everything off by this point
				//set the ProvNum to 0 for the unallocated paysplit this creates.
				_dictFamBalNoPPlan[patCur.Guarantor]-=(decimal)split.SplitAmt;
				if(_dictFamBalNoPPlan[patCur.Guarantor]<0 && RecurringCharges.CanChargeWhenNoBal(recCharge.CanChargeWhenNoBal)) {
					split.UnearnedType=Prefs.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
					split.ProvNum=0;//unearned paysplits are not associated to a provider unless the user explicitly does so.
				}
				PaySplits.Insert(split);
			}
			//consider moving the aging calls up in the Send methods and building a list of actions to feed into RunParallel to thread them.
			if(Prefs.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				Ledgers.ComputeAging(patCur.Guarantor,PrefC.GetDate(PrefName.DateLastAging));
			}
			else {
				Ledgers.ComputeAging(patCur.Guarantor,_nowDateTime.Date);
				if(PrefC.GetDate(PrefName.DateLastAging)!=_nowDateTime.Date) {
					Prefs.Set(PrefName.DateLastAging,POut.Date(_nowDateTime.Date,false));
					//Since this is always called from UI, the above line works fine to keep the prefs cache current.
				}
			}
			//Only label it a recurring charge in the event the balance is less than zero and the Pref RecurringChargesAllowedWhenNoPatBal is on
			if(_dictFamBalNoPPlan[patCur.Guarantor]<0 && RecurringCharges.CanChargeWhenNoBal(recCharge.CanChargeWhenNoBal)) {
				Payment oldPayment=paymentCur.Clone();
				paymentCur.PayNote+="Recurring Charge";
				Payments.Update(paymentCur,oldPayment);
			}
		}

		///<summary>Returns a valid DateTime for the payment's PayDate.  Contains logic if payment should be for the previous or the current month.</summary>
		private DateTime GetPayDate(RecurringChargeData recCharge) {
			if(Prefs.GetBool(PrefName.RecurringChargesUseTransDate)) {
				return _nowDateTime;
			}
			return recCharge.RecurringChargeDate;
		}
		
		///<summary>Stops the recurring charges that are being processed. The current card will be allowed to finish processing. This method can
		///block until the current card is finished.</summary>
		public void StopCharges(bool doWaitForCardToFinish=false) {
			_doShutdown=true;
			if(!doWaitForCardToFinish) {
				return;
			}
			DateTime dtBegin=DateTime.Now;
			while(IsCharging && (DateTime.Now - dtBegin) < _shutdownWaitTimeout) {
				//This method needs to block until the current card is finished being processed.
				Thread.Sleep(1);
			}
		}

		///<summary>Deletes any recurring charges that have not been processed yet. This should be called when it is clear that the cards in this
		///list are not going to be processed.</summary>
		public void DeleteNotYetCharged()
		{
			if (ListRecurringChargeData == null)
			{
				return;
			}
			List<RecurringCharge> listToDelete = ListRecurringChargeData.Select(x => x.RecurringCharge)
				.Where(x => x.ChargeStatus == RecurringChargeStatus.NotYetCharged && x.RecurringChargeNum > 0).ToList();
			Logger.LogVerbose("Deleting " + listToDelete.Count + " pending charges.");
			RecurringCharges.DeleteMany(listToDelete);
		}
	}
}
