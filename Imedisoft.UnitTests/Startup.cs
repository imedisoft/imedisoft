using Imedisoft.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imedisoft.UnitTests
{
    public class Startup
    {
        [AssemblyInitialize]
        public static void Init()
        {
            new DataConnection().SetDb("localhost", "root", "softlex", "opendental");
        }
    }
}
