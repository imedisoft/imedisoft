using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    /// <summary>
    /// Lan is short for language.
    /// Used to translate text to another language.
    /// </summary>
    public class Lan
	{
		/// <summary>
		/// Converts a string to the current language.
		/// </summary>
		public static string G(string classType, string text) 
			=> Lans.ConvertString(classType, text);

		/// <summary>
		/// Converts a string to the current language.
		/// </summary>
		public static string G(object sender, string text) 
			=> Lans.ConvertString(sender?.GetType().Name ?? "All", text);

		/// <summary>
		/// Translates the text of all menu items to another language.
		/// </summary>
		public static void C(Control sender, params Menu[] menus) 
			=> C(sender?.GetType().Name ?? "All", menus);

		/// <summary>
		/// Translates the text of all menu items to another language.
		/// </summary>
		public static void C(string classType, params Menu[] menus)
		{
			foreach (Menu mainMenu in menus)
			{
				foreach (MenuItem menuItem in mainMenu.MenuItems)
				{
					TranslateMenuItems(classType, menuItem);
				}
			}
		}

		/// <summary>
		/// This is recursive
		/// </summary>
		private static void TranslateMenuItems(string classType, MenuItem menuItem)
		{
			foreach (MenuItem subMenuItem in menuItem.MenuItems)
			{
				TranslateMenuItems(classType, subMenuItem);
			}

			menuItem.Text = Lans.ConvertString(classType, menuItem.Text);
		}

		/// <summary>
		/// Translates the text of all context menu strip items to another language.
		/// </summary>
		public static void C(string classType, params ContextMenuStrip[] contextMenuStrips)
		{
			foreach (var contextMenuStrip in contextMenuStrips)
			{
				foreach (ToolStripMenuItem toolStripItem in contextMenuStrip.Items)
				{
					TranslateToolStripMenuItems(classType, toolStripItem);
				}
			}
		}

		/// <summary>
		/// This is recursive
		/// </summary>
		private static void TranslateToolStripMenuItems(string classType, ToolStripMenuItem toolStripMenuItem)
		{
			foreach (ToolStripMenuItem dropDownItem in toolStripMenuItem.DropDownItems)
			{
				TranslateToolStripMenuItems(classType, dropDownItem);
			}

			toolStripMenuItem.Text = Lans.ConvertString(classType, toolStripMenuItem.Text);
		}

		public static void C(string classType, params Control[] controls) 
			=> C(classType, controls, false);

		public static void C(Control sender, params Control[] controls) 
			=> C(sender, controls, false);
		
		public static void C(Control sender, Control[] controls, bool recursive) 
			=> C(sender?.GetType().Name ?? "All", controls, recursive);
		
		public static void C(string classType, Control[] controls, bool recursive)
		{
			foreach (var ctrl in controls)
			{
				if (ctrl.GetType() == typeof(UI.ODGrid))
				{
					TranslateGrid(ctrl);
				}
				else
				{
					ctrl.Text = Lans.ConvertString(classType, ctrl.Text);
					if (recursive)
					{
						Cchildren(classType, ctrl);
					}
				}
			}
		}

		/// <summary>
		/// This is recursive, but a little simpler than Fchildren.
		/// </summary>
		private static void Cchildren(string classType, Control parent)
		{
			foreach (Control ctrl in parent.Controls)
			{
				if (ctrl.HasChildren)
				{
					Cchildren(classType, ctrl);
				}

				ctrl.Text = Lans.ConvertString(classType, ctrl.Text);
			}
		}

		private static void TranslateGrid(Control ctrl)
		{
			if (ctrl.GetType() != typeof(UI.ODGrid))
			{
				return;
			}

			var grid = (ODGrid)ctrl;

			grid.Title = Lans.ConvertString(grid.TranslationName, grid.Title);
			foreach (GridColumn gridColumn in grid.ListGridColumns)
			{
				gridColumn.Heading = Lans.ConvertString(grid.TranslationName, gridColumn.Heading);
			}

			if (grid.ContextMenu != null)
			{
				C(grid.TranslationName, grid.ContextMenu);
			}

			if (grid.ContextMenuStrip != null)
			{
				C(grid.TranslationName, grid.ContextMenuStrip);
			}
		}

		public static void C(string classType, params TabControl[] tabControls)
		{
			foreach (var tabControl in tabControls)
			{
				foreach (TabPage tabPage in tabControl.TabPages)
				{
					tabPage.Text = Lans.ConvertString(classType, tabPage.Text);
				}
			}
		}

		/// <summary>
		/// Translates the following controls on the entire form: title Text, labels, buttons, groupboxes, checkboxes, radiobuttons, ODGrid.
		/// </summary>
		public static void F(Form sender) 
			=> F(sender, new Control[] { });

		/// <summary>
		/// Translates the following controls on the entire form: title Text, labels, buttons, groupboxes, checkboxes, radiobuttons, ODGrid.
		/// 
		/// Can include a list of controls to exclude. Also puts all the correct controls into the All category (OK,Cancel,Close,Delete,etc).
		/// </summary>
		public static void F(Form sender, params Control[] exclusions)
		{
			if (CultureInfo.CurrentCulture.Name == "en-US") return;

			// First translate the main title Text on the form:
			if (!exclusions.Contains(sender))
			{
				sender.Text = Lans.ConvertString(sender.GetType().Name, sender.Text);
			}

			// Then launch the recursive function for all child controls
			Fchildren(sender, sender, exclusions);
		}

		/// <summary>
		/// Translates all children of the given control except those in the exclusions list.
		/// </summary>
		private static void Fchildren(Form sender, Control parent, params Control[] exclusions)
		{
			foreach (Control ctrl in parent.Controls)
			{
				Type type = ctrl.GetType();

				// Any controls with children of their own.  First so that we always translate children.
				if (ctrl.HasChildren)
				{
					Fchildren(sender, ctrl, exclusions);
				}

				// Do not process if the control is excluded...
				if (exclusions != null && exclusions.Contains(ctrl))
				{
					continue;
				}

				// Test to see if the control supports the .Text property.
				// Every control will have a .Text property present but some controls will purposefully throw a NotSupportedException (e.g. WebBrowser).
				try
				{
					ctrl.Text = ctrl.Text;
				}
				catch (Exception)
				{
					continue; // We cannot translate this control so move on.
				}

				if (type == typeof(GroupBox))
				{
					TranslateControl(sender.GetType().Name, ctrl);
				}
				else if (ctrl.GetType() == typeof(UI.ODGrid))
				{
					TranslateGrid(ctrl);
				}
				else if (type == typeof(TabControl))
				{
					// Translate all tab pages on the tab control.
					C(sender.GetType().Name, (TabControl)ctrl);
				}
				else
				{
					// Generically try to translate all orther controls not specifically mentioned above.
					TranslateControl(sender.GetType().Name, ctrl);
				}
			}
		}

		private static void TranslateControl(string classType, Control ctrl)
		{
			if (ctrl.Text == "OK" || 
				ctrl.Text == "&OK" || 
				ctrl.Text == "Cancel" || 
				ctrl.Text == "&Cancel" || 
				ctrl.Text == "Close" || 
				ctrl.Text == "&Close" || 
				ctrl.Text == "Add" || 
				ctrl.Text == "&Add" || 
				ctrl.Text == "Delete" || 
				ctrl.Text == "&Delete" || 
				ctrl.Text == "Up" || 
				ctrl.Text == "&Up" || 
				ctrl.Text == "Down" || 
				ctrl.Text == "&Down" || 
				ctrl.Text == "Print" || 
				ctrl.Text == "&Print")
			{
				ctrl.Text = Lans.ConvertString("All", ctrl.Text);
			}
			else
			{
				ctrl.Text = Lans.ConvertString(classType, ctrl.Text);
			}
		}
	}
}
