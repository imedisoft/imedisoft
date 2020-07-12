using OpenDentBusiness;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace ODR
{
    /// <summary>
	/// Holds information about a parameter used in the report.
	/// </summary>
    /// <remarks>
	/// A parameter is a string that can be used in the WHERE clause of a query that will be replaced by user-provided data before the query is sent.
	/// For instance, "?date1" might be replaced with "(ProcDate = '2004-02-17' OR ProcDate = '2004-02-18')".
	/// The output value can be multiple items connected with OR's as in the example, or it can be a single value.
	/// The Snippet represents one of the multiple values.  In this example, it would be "ProcDate = '?'".
	/// The ? in the Snippet will be replaced by the values provided by the user.
	/// </remarks>
    public class Parameter
	{
        /// <summary>
		/// This is the name as it will show in the query, but without the preceding question mark.
		/// </summary>
        public string Name { get; set; }
        
		/// <summary>
		/// The value, in text form, as it will be substituted into the main query and sent to the database.
		/// </summary>
        public string OutputValue { get; set; }
        
		/// <summary>
		/// The type of value that the parameter can accept.
		/// </summary>
        public ParamValueType ValueType { get; set; }
        
		/// <summary>
		/// The values of the parameter, typically just one. Each value can be a string, date, number, currency, Boolean, etc.
		/// If the length of the ArrayList is 0, then the value is blank and will not be used in the query.
		/// CurrentValues can be set ahead of time in the report, so in this usage, they might be thought of as default values.
		/// </summary>
        public ArrayList CurrentValues { get; set; }

        /// <summary>
		/// The text that prompts the user what to enter for this parameter.
		/// </summary>
        public string Prompt { get; set; }
        
		/// <summary>
		/// The snippet of SQL that will be repeated once for each value supplied by the user, connected with OR's, and surrounded by parentheses.
		/// </summary>
        public string Snippet { get; set; }
        
		/// <summary>
		/// If the ValueType is Enum, then this specifies which type of enum. It is the string name of the type.
		/// </summary>
        public EnumType EnumerationType { get; set; }
        
		/// <summary>
		/// If ValueType is QueryData, then this contains the query to use to get the parameter list.
		/// </summary>
        public string QueryText { get; set; }

        public Parameter()
		{
			Name = "";
			OutputValue = "1";
			ValueType = ParamValueType.String;
			CurrentValues = new ArrayList();
			Prompt = "";
			Snippet = "";
			EnumerationType = EnumType.ApptStatus; // arbitrary
			QueryText = "";
		}

		/// <summary>
		/// CurrentValues must be set first.
		/// Then, this applies the values to the parameter to create the outputValue.
		/// The currentValues is usually just a single value.
		/// The only time there will be multiple values is for an Enum or QueryText.
		/// For example, if a user selects multiple items from a listbox for this parameter, then each item is connected by an OR.
		/// The entire OutputValue is surrounded by parentheses.
		/// </summary>
		public void FillOutputValue()
		{
			OutputValue = "(";
			if (CurrentValues.Count == 0)
			{ // if there are no values
				OutputValue += "1"; // display a 1 (true) to effectively exclude this snippet
			}

			for (int i = 0; i < CurrentValues.Count; i++)
			{
				if (i > 0)
				{
					OutputValue += " OR ";
				}
				if (ValueType == ParamValueType.Boolean)
				{
					if ((bool)CurrentValues[i])
					{
						OutputValue += Snippet; //snippet will show. There is no ? substitution
					}
					else
					{
						OutputValue += "1"; //instead of the snippet, a 1 will show
					}
				}
				else if (ValueType == ParamValueType.Date)
				{
					OutputValue += Regex.Replace(Snippet, @"\?", POut.Date((DateTime)CurrentValues[i], false));
				}
				else if (ValueType == ParamValueType.Enum)
				{
					OutputValue += Regex.Replace(Snippet, @"\?", POut.Long((int)CurrentValues[i]));
				}
				else if (ValueType == ParamValueType.Integer)
				{
					OutputValue += Regex.Replace(Snippet, @"\?", POut.Long((int)CurrentValues[i]));
				}
				else if (ValueType == ParamValueType.String)
				{
					OutputValue += Regex.Replace(Snippet, @"\?", POut.String((string)CurrentValues[i]));
				}
				else if (ValueType == ParamValueType.Number)
				{
					OutputValue += Regex.Replace(Snippet, @"\?", POut.Double((double)CurrentValues[i]));
				}
				else if (ValueType == ParamValueType.QueryData)
				{
					OutputValue += Regex.Replace(Snippet, @"\?", POut.String((string)CurrentValues[i]));
				}
			}
			OutputValue += ")";
		}
	}

	/// <summary>
	/// Specifies the type of value that the parameter will accept.
	/// Also used in the ContrMultInput control to determine what kind of input to display.
	/// </summary>
	public enum ParamValueType
	{
		/// <summary>
		/// Parameter takes a date/time value.
		/// </summary>
		Date,

		/// <summary>
		/// Parameter takes a string value.
		/// </summary>
		String,

		/// <summary>
		/// Parameter takes a boolean value.
		/// If false, then the snippet will not even be included.
		/// Because of the way this is implemented, the snippet can specify a true or false value, and the user can select whether to include the snippet.
		/// So the parameter can specify whether to include a false value among many other possibilities.
		/// There should not be a ? in a boolean snippet.
		/// </summary>
		Boolean,

		/// <summary>
		/// Parameter takes an integer value.
		/// </summary>
		Integer,

		/// <summary>
		/// Parameter takes a number(double) value which can include a decimal.
		/// </summary>
		Number,

		/// <summary>
		/// Parameter takes an enumeration value(s).
		/// User must select from a list.
		/// </summary>
		Enum,

		/// <summary>
		/// A list will be presented to the user based on the results of this query.
		/// Column one of the query results should contain the values, and column two should contain the display text.
		/// One typical use is when choosing providers: "SELECT ProvNum,Abbr FROM provider WHERE IsHidden=0 ORDER BY ItemOrder"
		/// </summary>
		QueryData
	}
}
