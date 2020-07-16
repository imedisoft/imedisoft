using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace OpenDentBusiness
{
    public class ODDataTable
	{
		public string Name;
		public List<ODDataRow> Rows;

		public ODDataTable()
		{
			Rows = new List<ODDataRow>();
			Name = "";
		}

		public ODDataTable(string xmlData)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlData);
			Rows = new List<ODDataRow>();
			Name = "";
			ODDataRow currentRow;
			XmlNodeList nodesRows = doc.DocumentElement.ChildNodes;
			for (int i = 0; i < nodesRows.Count; i++)
			{
				currentRow = new ODDataRow();
				if (Name == "")
				{
					Name = nodesRows[i].Name;
				}
				foreach (XmlNode nodeCell in nodesRows[i].ChildNodes)
				{
					currentRow.Add(nodeCell.Name.ToString(), nodeCell.InnerXml);
				}
				Rows.Add(currentRow);
			}
		}
	}
}
