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

		public static Image DrawGeometry(IGeometry geometry, Pen pen = null, Brush background = null, int width = 800)
		{
			pen = pen ?? new Pen(Color.Black);
			background = background ?? new SolidBrush(Color.White);
			var envelope = geometry.EnvelopeInternal;
			var image = new Bitmap(width, (int)((envelope.Height * width) / envelope.Width));
			using (var graphics = Graphics.FromImage(image))
			{
				var transformation = TranslateFromEnvelope(envelope, image);
				graphics.FillRectangle(background, 0, 0, image.Width, image.Height);
				DrawGeometry(geometry, pen, graphics, transformation);
			}
			return image;
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
