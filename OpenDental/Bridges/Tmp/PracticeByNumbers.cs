using CodeBase;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
    /// <summary>
    /// Link to Practice By Numbers Report Provider.
    /// </summary>
    public static class PracticeByNumbers
	{
		public static void ShowPage()
		{
			try
			{
				if (Programs.IsEnabled(ProgramName.PracticeByNumbers))
				{
					ODFileUtils.ProcessStart("http://www.opendental.com/manual/portalpracticebynumbers.html");
				}
				else
				{
					ODFileUtils.ProcessStart("http://www.opendental.com/resources/redirects/redirectpracticebynumbers.html");
				}
			}
			catch
			{
				MsgBox.Show("Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet then try again.");
			}
		}
	}
}
