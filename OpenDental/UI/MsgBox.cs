using System.Windows.Forms;

namespace OpenDental
{
    public class MsgBox
	{
		/// <summary>
		/// Use this when you don't want automatic language translation.
		/// Any substrings that need translation should be wrapped with Lan.g().
		/// </summary>
		public static void Show(string text) => CodeBase.ODMessageBox.Show(text);

		/// <summary>
		/// Automates the language translation of the entire string. Do NOT use if the text could be variable in any way.
		/// Returns true if result is OK or Yes.
		/// </summary>
		public static bool Show(object sender, MsgBoxButtons buttons, string question)
		{
			return Show(buttons, question);
		}

		public static bool Show(MsgBoxButtons buttons, string question, string titleBarText = "")
		{
			switch (buttons)
			{
				case MsgBoxButtons.OKCancel:
					return MessageBox.Show(question, titleBarText, MessageBoxButtons.OKCancel) == DialogResult.OK;
				case MsgBoxButtons.YesNo:
					return MessageBox.Show(question, titleBarText, MessageBoxButtons.YesNo) == DialogResult.Yes;
				default:
					return false;
			}
		}
	}

	public enum MsgBoxButtons
	{
		OKCancel,
		YesNo
	}
}
