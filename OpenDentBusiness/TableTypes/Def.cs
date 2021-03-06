using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    public enum DefCatMiscColors
	{
		//0-8 can be added as needed.

		/// <summary>
		/// This color is used for the fields in the family module for the in case of emergency contacts.
		/// </summary>
		FamilyModuleICE = 9,
	}

	/// <summary>
	/// Represents the options for a definition category.
	/// </summary>
	public class DefinitionCategoryOptions
	{
		public string Category;

		public bool CanEditName;
		public bool EnableValue;
		public bool EnableColor;

		///<summary>This is the text that will show up in the Guidelines section of the Definitions window.</summary>
		public string HelpText;
		public bool CanDelete;
		public bool CanHide;

		///<summary>This is the text that will show up in the second column of the Definitions window and above the second text box in the edit window.
		///It is typically left blank unless each definition item has something special that it can do (image categories, etc).</summary>
		public string ValueText;
		public bool IsValueDefNum;
		public bool DoShowItemOrderInValue;

		///<summary>Indicates that the Color for these definitions can be left empty when editing.</summary>
		public bool DoShowNoColor;

		public DefinitionCategoryOptions(string definitionCategory, bool canDelete = false, bool canEditName = true, bool canHide = true, bool enableColor = false, bool enableValue = false, bool isValidDefNum = false, bool showNoColor = false)
		{
			Category = definitionCategory;
			CanDelete = canDelete;
			CanEditName = canEditName;
			CanHide = canHide;
			EnableColor = enableColor;
			EnableValue = enableValue;
			IsValueDefNum = isValidDefNum;
			DoShowNoColor = showNoColor;
		}

		public string Description => DefinitionCategory.GetDescription(Category);

		/// <summary>
		/// Returns a string representation of the category options.
		/// </summary>
		public override string ToString()
			=> DefinitionCategory.GetDescription(Category);

	}
}
