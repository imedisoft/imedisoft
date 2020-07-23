using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;

namespace UnitTests.Authentication_Tests
{
    [TestClass]
	public class AuthenticationTests : TestBase
	{
		[TestMethod]
		public void Authentication_CheckPasswordHashAndVerifyWithSHA512()
		{
			var passwordHash = Password.Hash("awesomePassword", PasswordAlgorithm.SHA512);

			Assert.IsTrue(!string.IsNullOrEmpty(passwordHash));
			Assert.IsTrue(Password.Verify("awesomePassword", passwordHash));
		}
	}
}
