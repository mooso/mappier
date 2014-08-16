using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappier.Tests
{
	[TestClass]
	public class USATests
	{
		[TestMethod]
		public void BasicChecks()
		{
			var usa = USA.Create2013USA();
			Assert.AreEqual(56, usa.States.Count);
			var washington = usa.States.Single(s => s.Name == "Washington");
			var oregon = usa.States.Single(s => s.Name == "Oregon");
			Assert.AreEqual(washington.Geometry.EnvelopeInternal.MinY, oregon.Geometry.EnvelopeInternal.MaxY, 1.0);
		}
	}
}
