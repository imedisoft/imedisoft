using CodeBase;
using DataConnectionBase;
using Imedisoft.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class Lans
	{
		public static string GetShortDateTimeFormat()
		{
			if (CultureInfo.CurrentCulture.Name == "en-US")
			{
				return "MM/dd/yyyy";
			}
			else
			{
				return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			}
		}

		public static string GetShortTimeFormat(CultureInfo ci)
		{
			string hFormat = "";
			ci.DateTimeFormat.AMDesignator = ci.DateTimeFormat.AMDesignator.ToLower();
			ci.DateTimeFormat.PMDesignator = ci.DateTimeFormat.PMDesignator.ToLower();
			string shortTimePattern = ci.DateTimeFormat.ShortTimePattern;
			if (shortTimePattern.IndexOf("hh") != -1)
			{//if hour is 01-12
				hFormat += "hh";
			}
			else if (shortTimePattern.IndexOf("h") != -1)
			{//or if hour is 1-12
				hFormat += "h";
			}
			else if (shortTimePattern.IndexOf("HH") != -1)
			{//or if hour is 00-23
				hFormat += "HH";
			}
			else
			{//hour is 0-23
				hFormat += "H";
			}
			if (shortTimePattern.IndexOf("t") != -1)
			{//if there is an am/pm designator
				hFormat += "tt";
			}
			else
			{//if no am/pm designator, then use :00
				hFormat += ":00";//time separator will actually change according to region
			}
			return hFormat;
		}
	}
}
