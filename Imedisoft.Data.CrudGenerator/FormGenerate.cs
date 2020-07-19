using Imedisoft.Data.Annotations;
using Imedisoft.Data.CrudGenerator.Generator;
using Imedisoft.Data.CrudGenerator.Properties;
using Imedisoft.Data.CrudGenerator.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace Imedisoft.Data.CrudGenerator
{
    public partial class FormGenerate : Form
    {
        private readonly Assembly assembly;

        public FormGenerate(Assembly assembly)
        {
            InitializeComponent();

            this.assembly = assembly;
        }

        private void FormGenerate_Load(object sender, EventArgs e)
        {
            namespaceTextBox.Text = Settings.Default.OutputNamespace;
            outputTextBox.Text = Settings.Default.OutputDirectory;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                outputTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void GenerateClass(Type type, string ns, string output)
        {
            try
            {
                var path = Path.Combine(output, string.Concat(type.Name, "Crud.cs"));

                var table = new Table(type);

                var code = EntityClassGenerator.Generate(table, ns);

                File.WriteAllText(path, code);
            }
            catch { }
        }

        private void GenerateClasses(string ns, string output)
        {
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

                GenerateClass(type, ns, output);
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            var ns = namespaceTextBox.Text.Trim();
            if (ns.Length == 0)
            {
                MessageBox.Show(this,
                    "You have to specify a namespace.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                namespaceTextBox.Focus();

                return;
            }

            var output = outputTextBox.Text.Trim();
            if (output.Length == 0)
            {
                MessageBox.Show(this,
                    "You have to specify the output directory.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                outputTextBox.Focus();

                return;
            }

            if (!Directory.Exists(output))
            {
                var result = MessageBox.Show(this,
                    "The selected output directory does not exist. The output directory will be created.", Text, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                try
                {
                    Directory.CreateDirectory(output);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this,
                        exception.Message, Text, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            GenerateClasses(ns, output);

            Settings.Default.OutputNamespace = ns;
            Settings.Default.OutputDirectory = output;
            Settings.Default.Save();

            MessageBox.Show(this,
                "All CRUD classes have been generated succesfully.", Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}
