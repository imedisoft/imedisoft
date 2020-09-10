using CodeBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebServiceSerializer;

namespace OpenDentBusiness
{
    /// <summary>
    /// This class provides helper methods when creating payloads to send to HQ hosted web services (e.g. WebServiceMainHQ.asmx and SheetsSynch.asmx)
    /// </summary>
    public class PayloadHelper
	{
		/// <summary>Returns an XML payload that includes common information required by most HQ hosted services (e.g. reg key, program version, etc).</summary>
		/// <param name="registrationKey">An Open Dental distributed registration key that HQ has on file.  Do not include hyphens.</param>
		/// <param name="practiceTitle">Any string is acceptable.</param>
		/// <param name="practicePhone">Any string is acceptable.</param>
		/// <param name="programVersion">Typically major.minor.build.revision.  E.g. 12.4.58.0</param>
		/// <param name="payloadContentxAsXml">Use CreateXmlWriterSettings(true) to create your payload xml. Outer-most xml element MUST be labeled 'Payload'.</param>
		/// <param name="serviceCode">Used on case by case basis to validate that customer is registered for the given service.</param>
		/// <returns>An XML string that can be passed into an HQ hosted web method.</returns>
		public static string CreatePayload(string payloadContentxAsXml, eServiceCode serviceCode, string registrationKey = null, string practiceTitle = null, string practicePhone = null, string programVersion = null)
		{
			var stringBuilder = new StringBuilder();

			using (var xmlWriter = XmlWriter.Create(stringBuilder, WebSerializer.CreateXmlWriterSettings(false)))
			{
				xmlWriter.WriteStartElement("Request");
				xmlWriter.WriteStartElement("Credentials");
				xmlWriter.WriteStartElement("RegistrationKey");
				xmlWriter.WriteString(registrationKey ?? Preferences.GetString(PreferenceName.RegistrationKey));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("PracticeTitle");
				xmlWriter.WriteString(practiceTitle ?? Preferences.GetString(PreferenceName.PracticeTitle));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("PracticePhone");
				xmlWriter.WriteString(practicePhone ?? Preferences.GetString(PreferenceName.PracticePhone));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("ProgramVersion");
				xmlWriter.WriteString(programVersion ?? Preferences.GetString(PreferenceName.ProgramVersion));
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("ServiceCode");
				xmlWriter.WriteString(serviceCode.ToString());
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteRaw(payloadContentxAsXml);
				xmlWriter.WriteEndElement();
			}

			return stringBuilder.ToString();
		}

		public static string CreatePayloadWebHostSynch(string registrationKey, params PayloadItem[] payloadItems) 
			=> CreatePayload(CreatePayloadContent(payloadItems.ToList()), eServiceCode.WebHostSynch, registrationKey, "", "", "");

		/// <summary>Returns an XML payload that includes common information required by most HQ hosted services (e.g. reg key, program version, etc).</summary>
		/// <param name="registrationKey">An Open Dental distributed registration key that HQ has on file.  Do not include hyphens.</param>
		/// <param name="practiceTitle">Any string is acceptable.</param>
		/// <param name="practicePhone">Any string is acceptable.</param>
		/// <param name="programVersion">Typically major.minor.build.revision.  E.g. 12.4.58.0</param>
		/// <param name="payloadItems">All items that need to be included with the payload.</param>
		/// <param name="serviceCode">Used on case by case basis to validate that customer is registered for the given service.</param>
		/// <returns>An XML string that can be passed into an HQ hosted web method.</returns>
		public static string CreatePayload(List<PayloadItem> payloadItems, eServiceCode serviceCode, string registrationKey = null, string practiceTitle = null, string practicePhone = null, string programVersion = null) 
			=>  CreatePayload(CreatePayloadContent(payloadItems), serviceCode, registrationKey, practiceTitle, practicePhone, programVersion);

		/// <summary>
		/// Creates an XML string for the payload of the provided content.
		/// Currently only useful if you have one thing to include in the payload.
		/// The root element will be Payload, followed by a tagName element, finished with the entire serialized version of content.
		/// </summary>
		public static string CreatePayloadContent(object content, string tagName) 
			=> CreatePayloadContent(new List<PayloadItem>() { new PayloadItem(content, tagName) });

		/// <summary>
		/// Creates an XML string for the payload of the provided content.
		/// The list passed in is a tuple where the first item is the content to be serialized and the second item is the tag name for the content.
		/// </summary>
		public static string CreatePayloadContent(IEnumerable<PayloadItem> payloadItems)
		{
			var stringBuilder = new StringBuilder();

			using (var xmlWriter = XmlWriter.Create(stringBuilder, WebSerializer.CreateXmlWriterSettings(true)))
			{
				xmlWriter.WriteStartElement("Payload");

				foreach (var payloadItem in payloadItems)
				{
					var xmlSerializer = new XmlSerializer(payloadItem.Content.GetType());

					xmlWriter.WriteStartElement(payloadItem.TagName);
					xmlSerializer.Serialize(xmlWriter, payloadItem.Content);
					xmlWriter.WriteEndElement();
				}

				xmlWriter.WriteEndElement();
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Returns XML string with a Response element that contains the given content with nodeName. Called from OpenDentalWebApps.
		/// </summary>
		public static string CreateSuccessResponse<T>(T content, string nodeName)
		{
			var stringBuilder = new StringBuilder();

			using (var xmlWriter = XmlWriter.Create(stringBuilder, WebSerializer.CreateXmlWriterSettings(false)))
			{
				var xmlSerializer = new XmlSerializer(typeof(T));

				xmlWriter.WriteStartElement("Response");
				xmlWriter.WriteStartElement(nodeName);
				xmlSerializer.Serialize(xmlWriter, content);
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Creates an XML string with a Response element for each of the provided content. 
		/// The list passed in is a tuple where the first item is the content to be serialized and the second item is the tag name for the content.
		/// </summary>
		public static string CreateSuccessResponse(List<PayloadItem> listPayloadItems)
		{
			StringBuilder strbuild = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(strbuild, WebSerializer.CreateXmlWriterSettings(true)))
			{
				writer.WriteStartElement("Response");
				foreach (PayloadItem payLoadItem in listPayloadItems)
				{
					XmlSerializer xmlListConfirmationRequestSerializer = new XmlSerializer(payLoadItem.Content.GetType());
					writer.WriteStartElement(payLoadItem.TagName);
					xmlListConfirmationRequestSerializer.Serialize(writer, payLoadItem.Content);
					writer.WriteEndElement();
				}
				writer.WriteEndElement(); //Response	
			}
			return strbuild.ToString();
		}

		/// <summary>
		/// Throws an exception if there is an XML node title 'Error'.
		/// </summary>
		public static void CheckForError(string xmlResult)
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlResult);

			var xmlNode = xmlDocument.SelectSingleNode("//Error");
			if (xmlNode != null)
			{
				throw new Exception(xmlNode.InnerText);
			}
		}
	}

	public class PayloadItem : Tuple<object, string>
	{
		public object Content { get { return Item1; } }

		public string TagName { get { return Item2; } }

		public PayloadItem(object content, string tagName) : base(content, tagName)
		{
		}
	}
}
