using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappier
{
	public static class GeometryDrawing
	{
		public static void DrawGeometry(IGeometry geometry, Pen pen, Graphics graphics, Matrix transformation)
		{
			PointF[] points = geometry.Coordinates.Select(c => new PointF((float)c.X, (float)c.Y)).ToArray();
			transformation.TransformPoints(points);
			graphics.DrawPolygon(pen, points);
		}

		public static Matrix TranslateFromEnvelope(Envelope envelope, Image image)
		{
			return TranslateFromEnvelope(envelope, new PointF(), new PointF(image.Width, image.Height));
		}

		public static Matrix TranslateFromEnvelope(Envelope envelope, PointF targetTopLeft, PointF targetBottomRight)
		{
			var ret = new Matrix();
			ret.Scale((targetBottomRight.X - targetTopLeft.X) / (float)envelope.Width,
				(targetBottomRight.Y - targetTopLeft.Y) / -(float)envelope.Height);
			ret.Translate(targetTopLeft.X - (float)envelope.MinX,
				targetTopLeft.Y - (float)envelope.MaxY);
			return ret;
		}
	}
}
