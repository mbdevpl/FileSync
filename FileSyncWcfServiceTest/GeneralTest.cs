using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileSyncWcfService;

namespace FileSyncWcfServiceTest {

	[TestClass]
	public class GeneralTest {

		[TestMethod]
		public void EntityFrameworkContextCreationTest() {
			filesyncEntitiesNew context = new filesyncEntitiesNew();
			Assert.IsInstanceOfType(context, typeof(filesyncEntitiesNew));
		}

	}

}
