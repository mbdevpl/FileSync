using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileSyncObjects;

namespace FileSyncObjectsTest {

	[TestClass]
	public class UserObjectsTest {

		[TestMethod]
		public void CredentialsTest() {
			var obj1 = new Credentials("login", "password");
			
			Assert.IsInstanceOfType(obj1, typeof(Credentials));
			Assert.Equals(obj1.Login, "login");
			Assert.Equals(obj1.Password, "password");

			var obj2 = new Credentials(obj1);
			
			Assert.IsInstanceOfType(obj2, typeof(Credentials));
			Assert.Equals(obj2.Login, "login");
			Assert.Equals(obj2.Password, "password");

			var obj3 = Credentials.NewInstance("login", "password");
			
			Assert.IsInstanceOfType(obj3, typeof(Credentials));
			Assert.Equals(obj3.Login, "login");
			Assert.Equals(obj3.Password, "password");

			Assert.Equals(obj1, obj2);
			Assert.Equals(obj2, obj3);
			Assert.Equals(obj1, obj3);
		}

		[TestMethod]
		public void UserIdentityTest() {

		}

		[TestMethod]
		public void UserContentsTest() {

		}

	}
}
