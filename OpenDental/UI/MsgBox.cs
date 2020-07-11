using System;
using System.Windows.Forms;

namespace OpenDental
{
	public class MsgBox
	{
		/// <summary>
		/// Use this when you don't want automatic language translation.
		/// Any substrings that need translation should be wrapped with Lan.g().
		/// </summary>
		public static void Show(string text)
		{
			MessageBox.Show(text);
		}

		/// <summary>
		/// Automates the language translation of the entire string. Do NOT use if the text could be variable in any way.
		/// Returns true if result is OK or Yes.
		/// </summary>
		public static bool Show(object sender, MsgBoxButtons buttons, string question)
		{
			return Show(buttons, question, "");
		}

		/// <summary>
		/// No language translation.  Returns true if result is OK or Yes.
		/// </summary>
		public static bool Show(MsgBoxButtons buttons, string question)
		{
			return Show(buttons, question, "");
		}

		public static bool Show(MsgBoxButtons buttons, string question, string titleBarText)
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

		[Obsolete("Instead of this, change the middle parameter from 'true' to 'MsgBoxButtons.OKCancel'.")]
		public static bool Show(object sender, bool okCancel, string question)
		{
			return Show(sender, MsgBoxButtons.OKCancel, question);
		}
	}

	public enum MsgBoxButtons
	{
		OKCancel,
		YesNo
	}
}
