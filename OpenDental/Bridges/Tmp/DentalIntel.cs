using CodeBase;
using OpenDentBusiness;

namespace OpenDental.Bridges
{
    /// <summary>
    /// Link to Dental Intel Premium Report Provider.
    /// </summary>
    public static class DentalIntel
	{
		public static void ShowPage()
		{
			try
			{
				if (Programs.IsEnabled(ProgramName.DentalIntel))
				{
					ODFileUtils.ProcessStart("http://www.opendental.com/manual/portaldentalintel.html");
				}
				else
				{
					ODFileUtils.ProcessStart("http://www.opendental.com/resources/redirects/redirectdentalintel.html");
				}
			}
			catch
			{
				MsgBox.Show("Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet then try again.");
			}
		}
	}
}