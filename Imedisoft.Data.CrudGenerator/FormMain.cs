using Imedisoft.Data.Annotations;
using Imedisoft.Data.CrudGenerator.Generator;
using Imedisoft.Data.CrudGenerator.Schema;
using Imedisoft.Data.Models;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Imedisoft.Data.CrudGenerator
{
    public partial class FormMain : Form
	{
		private Assembly assembly;

		public FormMain() => InitializeComponent();

		private void DiscoverTableTypes(Assembly assembly)
        {
			assemblyTextBox.Text = Path.GetFileName(assembly.Location);

			typesListBox.BeginUpdate();
			typesListBox.Items.Clear();

			foreach (var type in assembly.GetTypes())
            {
				if (!type.IsPublic || type.IsAbstract || type.IsInterface)
                {
					continue;
                }

				var attribute = type.GetCustomAttribute<TableAttribute>();
				if (attribute == null)
                {
					continue;
                }

				typesListBox.Items.Add(type);
			}

			if (typesListBox.Items.Count > 0)
            {
				typesListBox.SelectedIndex = 0;
            }

			typesListBox.EndUpdate();
        }

		private void FormMain_Load(object sender, EventArgs e)
		{
			snippetTextBox.SetTabStopWidth(4);

			Type typeTableBase = typeof(Account);

			assembly = Assembly.GetAssembly(typeTableBase);

			DiscoverTableTypes(assembly);
		}

		private void TypesComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			if (e.Index != -1)
			{
				if (typesListBox.Items[e.Index] is Type type)
				{
					var color = e.State.HasFlag(DrawItemState.Selected) ?
						SystemColors.HighlightText :
						SystemColors.ControlText;

					var image = iconsImageList.Images[0];
					var imageY = e.Bounds.Top + (e.Bounds.Height / 2) - (image.Height / 2);

					e.Graphics.DrawImage(image, new Point(e.Bounds.X + 3, imageY));

					TextRenderer.DrawText(e.Graphics,
						type.Name, typesListBox.Font,
						Rectangle.FromLTRB(
							e.Bounds.Left + 20,
							e.Bounds.Top,
							e.Bounds.Right - 5,
							e.Bounds.Bottom - 1),
						color, TextFormatFlags.VerticalCenter);
				}
			}
		}

		private void TypesComboBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var index = typesListBox.IndexFromPoint(e.Location);
			if (index == -1)
			{
				return;
			}

			typesListBox.SelectedIndex = index;

			if (typesListBox.SelectedItem is Type type)
			{
				try
				{
					var table = new Table(type);

					snippetTextBox.Text = EntityClassGenerator.Generate(table, namespaceTextBox.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show(this,
						ex.Message, Text,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show(this,
					"Please select a type for which the generate the snippet.", Text,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			using (var formGenerate = new FormGenerate(assembly))
            {
				formGenerate.ShowDialog(this);
            }
		}
    }
}
