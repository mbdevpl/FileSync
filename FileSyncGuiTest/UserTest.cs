using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileSyncGui;
using FileSyncObjects;

namespace FileSyncGuiTest {
	/// <summary>
	/// Summary description for UserTest
	/// </summary>
	[TestClass]
	public class UserTest {

		private TestContext testContextInstance;
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		private FileSyncConnection guiConn;
		public UserTest() {
			guiConn = new FileSyncConnection();
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void CreateAccountTest() {
			Credentials c = new Credentials("testUserLogin", 
            				Security.ComputeHash("testUserPass"));
            UserContents uc = new UserContents(c, "Test User", "test.email@domain.com");
			guiConn.AddUser(uc);
			Assert.AreEqual(guiConn.GetUser(c), uc);
		}
	}
}
