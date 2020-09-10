using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OpenDentBusiness
{
	public class OIDInternals
	{
		public static string OpenDentalOID = "2.16.840.1.113883.3.4337";
		private static long _customerPatNum = 0;

		/// <summary>
		/// The PatNum at Open Dental HQ associated to this database's registration key.
		/// Makes a web call to WebServiceCustomerUpdates in order to get the PatNum from HQ.
		/// Throws exceptions to show to the user if anything goes wrong in communicating with the web service. 
		/// Exceptions are already translated.
		/// </summary>
		public static long CustomerPatNum
		{
			get
			{
				if (_customerPatNum == 0)
				{
					//prepare the xml document to send--------------------------------------------------------------------------------------
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					settings.IndentChars = "    ";

					StringBuilder strbuild = new StringBuilder();
					using (XmlWriter writer = XmlWriter.Create(strbuild, settings))
					{
						writer.WriteStartElement("CustomerIdRequest");
						writer.WriteStartElement("RegistrationKey");
						writer.WriteString(Preferences.GetString(PreferenceName.RegistrationKey));
						writer.WriteEndElement();
						writer.WriteStartElement("RegKeyDisabledOverride");
						writer.WriteString("true");
						writer.WriteEndElement();
						writer.WriteEndElement();
					}

#if DEBUG
					OpenDentBusiness.localhost.Service1 OIDService = new OpenDentBusiness.localhost.Service1();
#else
					OpenDentBusiness.customerUpdates.Service1 OIDService=new OpenDentBusiness.customerUpdates.Service1();
					OIDService.Url=Prefs.GetString(PrefName.UpdateServerAddress);
#endif
					//Send the message and get the result---------------------------------------------------------------------------------------
					string result = "";
					try
					{
						result = OIDService.RequestCustomerID(strbuild.ToString());
					}
					catch (Exception ex)
					{
						throw new Exception("Error obtaining CustomerID:" + " " + ex.Message);
					}

					XmlDocument doc = new XmlDocument();
					doc.LoadXml(result);

					//Process errors------------------------------------------------------------------------------------------------------------
					XmlNode node = doc.SelectSingleNode("//Error");
					if (node != null)
					{
						throw new Exception("Error:" + " " + node.InnerText);
					}

					//Process a valid return value----------------------------------------------------------------------------------------------
					node = doc.SelectSingleNode("//CustomerIdResponse");
					if (node == null)
					{
						throw new ODException(
							"There was an error requesting your OID or processing the result of the request. Please try again.");
					}

					if (node.InnerText == "")
					{
						throw new ODException(
							"Invalid registration key. Your OIDs will have to be set manually.");
					}

					//CustomerIdResponse has been returned and is not blank
					_customerPatNum = PIn.Long(node.InnerText);
				}

				return _customerPatNum;
			}
		}

		/// <summary>
		/// Returns the currently defined OID for a given IndentifierType. 
		/// If not defined, IDroot will be empty string.
		/// </summary>
		public static OIDInternal GetForType(IdentifierType IDType)
		{
			InsertMissingValues();

			return Crud.OIDInternalCrud.SelectOne(
				"SELECT * FROM oidinternal WHERE IDType='" + IDType.ToString() + "'");
		}

		/// <summary>
		/// There should always be one entry in the DB per IdentifierType enumeration.
		/// </summary>
		public static void InsertMissingValues()
		{
			List<OIDInternal> listOIDInternals = Crud.OIDInternalCrud.SelectMany("SELECT * FROM oidinternal");

			List<IdentifierType> listIDTypes = new List<IdentifierType>();
			for (int i = 0; i < listOIDInternals.Count; i++)
			{
				listIDTypes.Add(listOIDInternals[i].IDType);
			}

			for (int i = 0; i < Enum.GetValues(typeof(IdentifierType)).Length; i++)
			{
				if (listIDTypes.Contains((IdentifierType)i))
				{
					continue; // DB contains a row for this enum value.
				}

				// Insert missing row with blank OID.
				Database.ExecuteNonQuery(
					"INSERT INTO oidinternal (IDType, IDRoot) VALUES('" + ((IdentifierType)i).ToString() + "','')");
			}
		}

		public static List<OIDInternal> GetAll()
		{
			InsertMissingValues();

			return Crud.OIDInternalCrud.SelectMany("SELECT * FROM oidinternal");
		}

		public static void Update(OIDInternal oIDInternal)
		{
			Crud.OIDInternalCrud.Update(oIDInternal);
		}
	}
}
