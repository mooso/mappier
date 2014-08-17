using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappier.Tests
{
	[TestClass]
	public class GeometryDrawingTests
	{
		[TestMethod]
		public void TestTransform()
		{
			var envelope = new Envelope(1, 11, 2, 22);
			var transform = GeometryDrawing.TranslateFromEnvelope(envelope, new PointF(), new PointF(100, 100));
			AssertTransformed(transform, new PointF(1, 22), new PointF(0, 0));
			AssertTransformed(transform, new PointF(11, 2), new PointF(100, 100));
		}

		private static void AssertTransformed(Matrix transform, PointF original, PointF expectedTransform)
		{
			var array = new[] { original };
			transform.TransformPoints(array);
			Assert.IsTrue(Math.Abs(array[0].X - expectedTransform.X) < 0.001 && Math.Abs(array[0].Y - expectedTransform.Y) < 0.001,
				String.Format("Expected: {0}, Actual: {1}.", expectedTransform, array[0]));
		}
	}
}
