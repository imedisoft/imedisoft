using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace WebServiceSerializer
{
    /// <summary>
    /// Used to serialize primitives for WebServiceCustUpdates I/O.
    /// </summary>
    public static class WebSerializer
	{
		/// <summary>
		/// If the delimiter character is found in a given cell, then the cell's value will be 
		/// updated to include the place holder value in lieu of the delimiter. This ensures that 
		/// the delimiter is reserved for only delimiting cells. The place holder will be replaced
		/// by the delimiter value on the other end once the cells have been properly delimited.
		/// </summary>
		private const string _cellDelimiterPlaceHolder = "zzzzzzzzzz";

		/// <summary>
		/// This value is reserved strictly for delimiting cells in a serialized data row.
		/// </summary>
		private const string _cellDelimiter = "~";

		/// <summary>
		/// Format necessary for C#/Java date/time.
		/// </summary>
		private const string DotNetDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

		/// <summary>
		/// Returns our most commonly used XmlWriterSettings.
		/// </summary>
		public static XmlWriterSettings CreateXmlWriterSettings(bool omitXmlDeclaration)
		{
            return new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = omitXmlDeclaration
            };
		}

		/// <summary>
		/// Escapes common characters used in XML from the passed in String.
		/// </summary>
		private static string EscapeForXml(string str)
		{
			if (string.IsNullOrEmpty(str)) return "";

			var stringBuilder = new StringBuilder();
			foreach (var c in str)
			{
				switch (c)
				{
					case '<':
						stringBuilder.Append("&lt;");
						break;

					case '>':
						stringBuilder.Append("&gt;");
						break;

					case '\"':
						stringBuilder.Append("&quot;;");
						break;

					case '\'':
						stringBuilder.Append("&#039;");
						break;

					case '&':
						stringBuilder.Append("&amp;");
						break;

					default:
						stringBuilder.Append(c);
						break;
				}
			}

			return stringBuilder.ToString();
		}

		private static string ReplaceEscapes(string myString)
		{
			if (string.IsNullOrEmpty(myString))
			{
				return "";
			}
			StringBuilder processedXml = new StringBuilder();
			for (int i = 0; i < myString.Length; i++)
			{
				//if at any point this char is not a match then ONLY append the start char, then continue
				//every continue should be accompanied by processedXml.Append(startChar)
				//search for consecutive [[ to open the special char indicator
				string startChar = myString.Substring(i, 1);
				if (startChar != "[")
				{
					processedXml.Append(startChar);
					continue;
				}
				string nextChar = myString.Substring(i + 1, 1);
				if (nextChar != "[")
				{
					processedXml.Append(startChar);
					continue;
				}
				//search for the consecutive ]] to close the special char indicator
				string remaining = myString.Substring(i, myString.Length - i);
				int endsAt = remaining.IndexOf("]]");
				if (endsAt < 0)
				{ //make sure the special char is closed before the end of this xml tag
					processedXml.Append(startChar);
					continue;
				}
				//we have a good special char to translate it, append it, and set the new index location
				//get the guts of the special char
				string specialChar = remaining.Substring(2, remaining.IndexOf("]]") - 2);
                //convert to asci
                if (!int.TryParse(specialChar, out int asciiAsInt))
                { //not a valid ascii value
                    processedXml.Append(startChar);
                    continue;
                }
                //append the ascii char as a string
                processedXml.Append(char.ConvertFromUtf32(asciiAsInt));
				//set the new index location, we have skipped a good chunk... [[123]]
				i += (endsAt + 1);
			}
			return processedXml.ToString();
		}

		/// <summary>
		/// Works in conjunction with DeserializePrimitive. Typically used to pass single primitives back and forth between web services.
		/// </summary>
		public static string SerializePrimitive<T>(T obj) 
			=> SerializeForCSharp(typeof(T).ToString(), obj);

		/// <summary>
		/// Goes through all the possible types of objects and returns the object serialized for Java. objectType must be fully qualified. 
		/// Ex: System.Int32.  For DataTables, set objectType to "DataTable".  Returns an empty node if the object is null. 
		/// Throws exceptions.
		/// </summary>
		public static string SerializeForCSharp(string objectType, Object obj)
		{
			if (obj == null)
			{
				return "<" + objectType + "/>";//Return an empty node?
			}
			if (obj.GetType().IsEnum)
			{ //Serialize value as int.
				return "<" + objectType + ">" + POut.PInt((int)obj) + "</" + objectType + ">";
			}
			//Primitives--------------------------------------------------------------------
			if (objectType == "System.Int32" || objectType == "int")
			{
				return "<int>" + POut.PInt((int)obj) + "</int>";
			}
			if (objectType == "System.Int64" || objectType == "long")
			{
				return "<long>" + Convert.ToInt64(((long)obj)).ToString() + "</long>";
			}
			if (objectType == "System.Boolean" || objectType == "bool")
			{
				return "<bool>" + POut.PBool((bool)obj) + "</boolean>";
			}
			if (objectType == "System.String" || objectType == "string")
			{
				return "<string>" + EscapeForXml((string)obj) + "</string>";
			}
			if (objectType == "System.Char" || objectType == "char")
			{
				return "<char>" + Convert.ToChar((char)obj).ToString() + "</char>";
			}
			if (objectType == "System.Single" || objectType == "Single")
			{
				return "<float>" + POut.PFloat((float)obj) + "</float>";
			}
			if (objectType == "System.Byte" || objectType == "byte")
			{
				return "<byte>" + POut.PByte((byte)obj) + "</byte>";
			}
			if (objectType == "System.Double" || objectType == "double")
			{
				return "<double>" + POut.PDouble((double)obj) + "</double>";
			}
			if (objectType.StartsWith("List"))
			{//Lists.
				return SerializeList(objectType, obj);
			}
			//DateTime----------------------------------------------------------------------
			if (objectType == "DateTime")
			{
				return "<DateTime>" + ((DateTime)obj).ToString(DotNetDateTimeFormat) + "</DateTime>";
			}
			//DataTable---------------------------------------------------------------------
			if (objectType == "DataTable")
			{
				return SerializeDataTable((DataTable)obj);
			}
			//DataSet-----------------------------------------------------------------------
			if (objectType == "DataSet")
			{
				return SerializeDataSet((DataSet)obj);
			}
			throw new NotSupportedException("SerializeForCSharp, unsupported class type: " + objectType);
		}

		/// <summary>
		/// Returns the primitive or general object deserialized. Throws exception.
		/// </summary>
		public static object Deserialize(string typeName, string xml)
		{
			//Handle enums special.
			Type type = null;
			try { type = Type.GetType(typeName); }
			catch { /* We couldn't get the typeIn from the typeName so it won't be an enum. Swallow this error. */}
			if (type != null)
			{ //Our input type is an enum so deserialize it according to what type of enum and if it was serialized as a string or an int.
				if (type.IsEnum)
				{
                    using XmlReader reader = XmlReader.Create(new StringReader(xml));

                    if (reader.Read())
                    { //Value was serialized by int.
                        return Enum.ToObject(type, PIn.PInt(reader.ReadString()));
                    }
                }
				else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
				{
                    return DeserializeList(xml);
				}
			}

			try
			{
                using XmlReader reader = XmlReader.Create(new StringReader(xml));

                while (reader.Read())
                {
                    switch (reader.Name.ToLower())
                    {
                        case "int":
                        case "int32":
                            return PIn.PInt(reader.ReadString());
                        case "long":
                        case "int64":
                            return PIn.PLong(reader.ReadString());
                        case "bool":
                        case "boolean":
                            return PIn.PBool(reader.ReadString());
                        case "string":
                            return PIn.PString(reader.ReadString());
                        case "char":
                            return Convert.ToChar(reader.ReadString());
                        case "float":
                            return PIn.PFloat(reader.ReadString());
                        case "byte":
                            return PIn.PByte(reader.ReadString());
                        case "double":
                            return PIn.PDouble(reader.ReadString());
                        case "datetime": //Format matters here. Java put it in this format in Serializing.getSerializedObject().
                            return DateTime.ParseExact(reader.ReadString(), DotNetDateTimeFormat, null);
                        case "datatable":
                            return DeserializeDataTable(reader.ReadOuterXml());
                        case "dataset":
                            return DeserializeDataSet(reader.ReadOuterXml());
                    }
                }
            }
			catch
			{
				//Deserializing known type failed.
				string context = "Deserialize, error deserializing primitive or general type: " + typeName + "\r\n" + xml;

				throw;
			}

			throw new Exception("Deserialize, unsupported primitive or general type: " + typeName);
		}

		/// <summary>
		/// Searches through resultXml for the given tagName and returns a deserialized object.
		/// Specifically looks for an Error node and throws an exception with the InnerText of said node if found.
		/// </summary>
		public static T DeserializeTag<T>(string resultXml, string tagName)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(resultXml);
			//Validate output.
			XmlNode node = doc.SelectSingleNode("//Error");
			if (node != null)
			{
				throw new Exception(node.InnerText);
			}
			node = doc.SelectSingleNode("//" + tagName);
			if (node == null)
			{
				throw new Exception("tagName node not found: " + tagName);
			}
			T retVal;
			using (XmlReader reader = XmlReader.Create(new StringReader(node.InnerXml)))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				retVal = (T)serializer.Deserialize(reader);
			}
			if (retVal == null)
			{
				throw new Exception("tagName node invalid: " + tagName);
			}
			return retVal;
		}

		/// <summary>
		/// Returns the inner text for the node with the given nodeName.
		/// </summary>
		public static string DeserializeNode(string xml, string nodeName, bool doThrowIfNotFound = true)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			XmlNode node = doc.SelectSingleNode("//" + nodeName);
			if (node == null)
			{
				if (!doThrowIfNotFound)
				{
					return "";
				}
				throw new ODException("Node not found: " + nodeName);
			}
			return node.InnerText;
		}

		/// <summary>
		/// Parse the xml and look for a node called 'Error'.
		/// If found then throw the node's InnerText.
		/// </summary>
		private static void ParseErrorAndThrow(string xml)
		{
            using var xmlReader = XmlReader.Create(new StringReader(xml));

            xmlReader.MoveToContent();
            while (xmlReader.Read())
            {
                // Only detect start elements.
                if (!xmlReader.IsStartElement())
                {
                    continue;
                }

                // save field name and move to the value
                string fieldName = xmlReader.Name;
                xmlReader.Read();
                switch (fieldName)
                {
                    case "Error":
                        throw new Exception(ReplaceEscapes(xmlReader.ReadContentAsString()));
                }
            }
        }

		/// <summary>
		/// Works in conjunction with SerializePrimitive.
		/// Typically used to pass single primitives back and forth between web services.
		/// </summary>
		public static T DeserializePrimitiveOrThrow<T>(string xml)
		{
			ParseErrorAndThrow(xml);

			return (T)Deserialize(typeof(T).ToString(), xml);
		}

		private static DataTable DeserializeDataTable(string xml)
		{
			DataTable dataTable = new DataTable();
			using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
			{
				if (!reader.ReadToFollowing("Name"))
				{
					throw new Exception("Name tag not found");
				}
				dataTable.TableName = ReplaceEscapes(reader.ReadString());
				while (reader.Read())
				{
					if (!reader.IsStartElement())
					{
						continue;
					}
					if (reader.Name == "")
					{
						continue;
					}
					if (reader.Name == "Cols")
					{
						continue;
					}
					if (reader.Name == "Col")
					{ //new column header
						dataTable.Columns.Add(ReplaceEscapes(reader.ReadString()));
						continue;
					}
					if (reader.Name == "Cells")
					{ //starting rows
						continue;
					}
					if (reader.Name == "y")
					{ //new row						
						DataRow row = dataTable.NewRow();
						string pipedRow = reader.ReadString();
						string[] cells = pipedRow.Split(new string[1] { _cellDelimiter }, StringSplitOptions.None);
						if (cells.Length == dataTable.Columns.Count)
						{
							for (int i = 0; i < cells.Length; i++)
							{
								cells[i] = ReplaceEscapes(cells[i].Replace(_cellDelimiterPlaceHolder, _cellDelimiter));
							}
							row.ItemArray = cells;
							dataTable.Rows.Add(row);
						}
						continue;
					}
				}
			}
			return dataTable;
		}

		///<summary>Helper function that will serialize a data table by looping through the rows and columns.</summary>
		private static string SerializeDataTable(DataTable table)
		{
			StringBuilder result = new StringBuilder();
			result.Append("<DataTable>");
			//Table name.
			result.Append("<Name>").Append(table.TableName).Append("</Name>");
			//Column names.
			result.Append("<Cols>");
			for (int i = 0; i < table.Columns.Count; i++)
			{
				result.Append("<Col>").Append(table.Columns[i].ColumnName).Append("</Col>");
			}
			result.Append("</Cols>");
			//Set each cell by looping through each column row by row.
			result.Append("<Cells>");
			for (int i = 0; i < table.Rows.Count; i++)
			{//Row loop.
				result.Append("<y>");
				for (int j = 0; j < table.Columns.Count; j++)
				{//Column loop.
					string cellValue = table.Rows[i][j].ToString();
					if (table.Columns[j].DataType.Name == "DateTime")
					{ //DateTime requires special formatting so it can be deserialized by java in DataTable.getCellDateFromFormatString().
                        if (!DateTime.TryParse(table.Rows[i][j].ToString(), out DateTime dt))
                        { //Shouldn't get here but just in case, give it a default value.
                            dt = new DateTime(1, 1, 1);
                        }
                        cellValue = dt.ToString(DotNetDateTimeFormat);
					}
					//Add the formatted string.
					result.Append(EscapeForXml(cellValue).Replace(_cellDelimiter, _cellDelimiterPlaceHolder));
					if (j < table.Columns.Count - 1)
					{
						result.Append(_cellDelimiter);
					}
				}
				result.Append("</y>");
			}
			result.Append("</Cells>");
			result.Append("</DataTable>");
			return result.ToString();
		}

		private static DataSet DeserializeDataSet(string xml)
		{
			var dataSet = new DataSet();

            using (var xmlReader = XmlReader.Create(new StringReader(xml)))
			{
				xmlReader.MoveToContent();//Moves to root node, <List>.
				xmlReader.Read();//Moves to first objects node, this will be the type of the object list.

                string typeName = xmlReader.Name;
                if (typeName == "DataSet")
				{
					// Can happen if passed an empty data set.
					return dataSet;
				}

				do
				{
					dataSet.Tables.Add(DeserializeDataTable(xmlReader.ReadOuterXml()));
				} while (xmlReader.Name == typeName);
			}

			return dataSet;
		}

		/// <summary>
		/// Helper function that will serialize a data set.
		/// </summary>
		private static string SerializeDataSet(DataSet dataSet)
		{
			StringBuilder strb = new StringBuilder();
			strb.Append("<DataSet>");
			strb.Append("<DataTables>");
			for (int i = 0; i < dataSet.Tables.Count; i++)
			{
				strb.Append(SerializeDataTable(dataSet.Tables[i]));
			}
			strb.Append("</DataTables>");
			strb.Append("</DataSet>");
			return strb.ToString();
		}

		///<summary>Pass in the type of list and the list object and this method will serialize it.  The object within the list must be fully qualified.</summary>
		public static string SerializeList<T>(List<T> list)
		{
			return SerializeList("List[[60]]" + typeof(T).Name + "[[62]]", list);
		}

		///<summary>Pass in the type of list and the list object and this method will serialize it.  The object within the list must be fully qualified.</summary>
		public static string SerializeList(string objectType, object obj)
		{
            //Strip out what kind of objects this list contains.
            Match m = Regex.Match(objectType, @"^List\[\[60\]\]([a-zA-Z0-9._%+-]*)\[\[62\]\]$");
			if (!m.Success)
			{
				throw new Exception("SerializeList, unknown object list: " + objectType);
			}

            string listType = m.Result("$1");
            //Cast to a list of objects and loop through all the objects and call each objects corresponding serialize method.
            StringBuilder result = new StringBuilder();
			result.Append("<List>");

            if (obj is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    result.Append(SerializeForCSharp(listType, item));
                }
            }

            result.Append("</List>");
			return result.ToString();
		}

		///<summary>Pass in the type of list and the list object and this method will serialize it.  The object within the list must be fully qualified.</summary>
		public static List<object> DeserializeList(string xml)
		{
			List<object> listObject = new List<object>();

            //Find out what type of list this is.
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
			{
				reader.MoveToContent();//Moves to root node, <List>.
				reader.Read();//Moves to first objects node, this will be the type of the object list.
                string typeName = reader.Name;
                if (typeName == "List")
				{//Can happen if passed an empty list.
					return listObject;
				}

				do
				{
					listObject.Add(Deserialize(typeName, reader.ReadOuterXml()));
				} while (reader.Name == typeName);
			}

			return listObject;
		}

		private static string GetNodeNameFromType(Type t) 
			=> t.Name + "Xml";
		
		public static T ReadXml<T>(string xml) where T : new()
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);

			XmlNode node = doc.SelectSingleNode("//" + GetNodeNameFromType(typeof(T)));
			if (node == null)
			{
				throw new ApplicationException(GetNodeNameFromType(typeof(T)) + " node not present.");
			}

			T ret = default; 
			
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(node.InnerXml)))
			{
				ret = (T)serializer.Deserialize(reader);
			}
			if (ret == null)
			{
				ret = new T();
			}

			return ret;
		}

		public static string WriteXml<T>(T input)
		{
			var xmlSerializer = new XmlSerializer(typeof(T)); 
			var stringBuilder = new StringBuilder();

			using (var xmlWriter = XmlWriter.Create(stringBuilder, CreateXmlWriterSettings(true)))
			{
				xmlWriter.WriteStartElement(GetNodeNameFromType(typeof(T)));
				xmlSerializer.Serialize(xmlWriter, input);
				xmlWriter.WriteEndElement();
			}

			return stringBuilder.ToString();
		}
	}

	/// <summary>
	/// Converts various datatypes into strings formatted correctly for MySQL. 
	/// 
	/// "P" was originally short for Parameter because this class was written specifically to replace parameters in the mysql queries. 
	/// Using strings instead of parameters is much easier to debug. 
	/// This will later be rewritten as a System.IConvertible interface on custom mysql types. 
	/// I would rather not ever depend on the mysql connector for this so that this program remains very db independent.
	/// Marked internal so it doesn't get mistaken or misused in place of OpenDentBusiness.POut.
	/// </summary>
	internal class POut
	{
		public static string PBool(bool value) 
			=> value ? "1" : "0";

		public static string PByte(byte value) 
			=> value.ToString();

		public static string PDouble(double value)
		{
			try
			{
				return value.ToString("f", new NumberFormatInfo());
			}
			catch
			{
				return "0";
			}
		}

		public static string PInt(int value) 
			=> value.ToString();

		public static string PFloat(float value) 
			=> value.ToString();
	}

	/// <summary>
	/// Converts strings coming in from the database into the appropriate type.
	/// 
	/// "P" was originally short for Parameter because this class was written specifically to replace parameters in the mysql queries.
	/// Using strings instead of parameters is much easier to debug. 
	/// This will later be rewritten as a System.IConvertible interface on custom mysql types.
	/// I would rather not ever depend on the mysql connector for this so that this program remains very db independent.
	/// Marked internal so it doesn't get mistaken or misused in place of OpenDentBusiness.PIn.
	/// </summary>
	internal class PIn
	{
		public static bool PBool(string value) => value == "1";

		public static byte PByte(string value)
		{
			if (byte.TryParse(value, out var result))
            {
				return result;
            }

			return default;
		}

		public static double PDouble(string value)
		{
			if (double.TryParse(value, out var result))
			{
				return result;
			}

			return default;
		}

		public static int PInt(string value)
		{
			if (int.TryParse(value, out var result))
			{
				return result;
			}

			return default;
		}

		public static long PLong(string value)
		{
			if (long.TryParse(value, out var result))
			{
				return result;
			}

			return default;
		}

		public static float PFloat(string value)
		{
			if (float.TryParse(value, out var result))
			{
				return result;
			}

			return default;
		}

		public static string PString(string value) => value;
	}
}
