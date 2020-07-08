using OpenDentBusiness;
using System.ComponentModel;
using System.Web.Services;

namespace OpenDentalServer
{
    [WebService(Namespace = "http://www.open-dent.com/OpenDentalServer")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	public class ServiceMain : WebService
	{
		/// <summary>
		/// Pass in a serialized dto. 
		/// It returns a dto which must be deserialized by the client.
		/// </summary>
		[WebMethod]
		public string ProcessRequest(string dtoString)
		{
			return DtoProcessor.ProcessDto(dtoString, Server.MapPath("."));
		}
	}
}
