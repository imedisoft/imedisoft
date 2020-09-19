using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoCodeItemEdit : FormBase
	{
		/// <summary>
		/// Gets the ID of the selected procedure code.
		/// </summary>
        public long ProcedureCodeId { get; private set; }

		/// <summary>
		/// Gets the selected conditions.
		/// </summary>
		public List<AutoCodeConditionType> Conditions { get; } = new List<AutoCodeConditionType>();

        public FormAutoCodeItemEdit(long procedureCodeId = 0, IEnumerable<AutoCodeConditionType> autoCodeConditionsTypes = null)
		{
			InitializeComponent();

			ProcedureCodeId = procedureCodeId;

			if (autoCodeConditionsTypes != null)
			{
				Conditions.AddRange(autoCodeConditionsTypes);
			}
		}

		private void FormAutoCodeItemEdit_Load(object sender, EventArgs e)
		{
			codeTextBox.Text = ProcedureCodes.GetStringProcCode(ProcedureCodeId);
			
			FillList();
		}

		private void FillList()
		{
			conditionsListBox.Items.Clear();

			foreach (AutoCodeConditionType autoCodeConditionType in Enum.GetValues(typeof(AutoCodeConditionType)))
			{
				var dataItem = 
					new DataItem<AutoCodeConditionType>(autoCodeConditionType, 
						AutoCodeConditions.GetDescription(autoCodeConditionType));

				var index = conditionsListBox.Items.Add(dataItem);

				if (Conditions.Contains(autoCodeConditionType))
                {
					conditionsListBox.SetItemChecked(index, true);
				}
			}
		}

		private void CodeButton_Click(object sender, EventArgs e)
		{
            using var formProcCodes = new FormProcCodes
            {
                IsSelectionMode = true
            };

			if (formProcCodes.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			ProcedureCodeId = formProcCodes.SelectedCodeNum;

			codeTextBox.Text = ProcedureCodes.GetStringProcCode(ProcedureCodeId);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (ProcedureCodeId == 0)
			{
				ShowError(Translation.Common.PleaseSelectCode);

				return;
			}

			var conditionTypes = conditionsListBox.CheckedItems.OfType<DataItem<AutoCodeConditionType>>().Select(x => x.Value).ToList();
			if (conditionTypes.Count == 0)
            {
				ShowError(Translation.Common.PleaseSelectAtLeastOneCondition);

				return;
            }

			Conditions.Clear();
			Conditions.AddRange(conditionTypes);

			DialogResult = DialogResult.OK;
		}
	}
}
