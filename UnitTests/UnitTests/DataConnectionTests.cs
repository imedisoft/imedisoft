using DataConnectionBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.DataConnection_Tests
{
    [TestClass]
	public class DataConnectionTests : TestBase
	{
		[ClassInitialize]
		public static void SetupClass(TestContext testContext)
		{
			//Add anything here that you want to run once before the tests in this class run.
		}

		[TestInitialize]
		public void SetupTest()
		{
			//Add anything here that you want to run before every test in this class.
		}

		[TestCleanup]
		public void TearDownTest()
		{
			//Add anything here that you want to run after every test in this class.
		}

		[ClassCleanup]
		public static void TearDownClass()
		{
			//Add anything here that you want to run after all the tests in this class have been run.
		}

		[TestMethod]
		public void DataConnectionTests_SOut_HasInjectionChars()
		{
			Assert.IsTrue(SOut.HasInjectionChars("'"));
			Assert.IsTrue(SOut.HasInjectionChars("\""));
			Assert.IsTrue(SOut.HasInjectionChars("\r"));
			Assert.IsTrue(SOut.HasInjectionChars("\n"));
			Assert.IsTrue(SOut.HasInjectionChars("\t"));
			Assert.IsTrue(SOut.HasInjectionChars("'\"\r\n\t"));
			Assert.IsTrue(SOut.HasInjectionChars("abc\t"));
			Assert.IsFalse(SOut.HasInjectionChars(""));
			Assert.IsFalse(SOut.HasInjectionChars("a"));
			Assert.IsFalse(SOut.HasInjectionChars("12345"));
			Assert.IsFalse(SOut.HasInjectionChars("abcde"));
			Assert.IsFalse(SOut.HasInjectionChars("^@bcd3$"));
		}
	}
}
