using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappier
{
	public sealed class USA
	{
		private ImmutableList<State> _states;

		private USA(IEnumerable<State> states)
		{
			_states = states.ToImmutableList();
		}

		public ImmutableList<State> States { get { return _states; } }

		public static USA Create2013USA()
		{
			var tempDirPath = Path.GetTempFileName();
			File.Delete(tempDirPath);
			Directory.CreateDirectory(tempDirPath);
			var shapeFilePath = Copy2013USStateShapeFilesToDirectory(tempDirPath);
			try
			{
				var geometryFactory = new GeometryFactory();
				using (var shapeReader = Shapefile.CreateDataReader(shapeFilePath, geometryFactory))
				{
					return new USA(ReadStates(shapeReader));
				}
			}
			finally
			{
				Directory.Delete(tempDirPath, recursive: true);
			}
		}

		public Image DrawContinentalUSA(int width = 800)
		{
			var continentalBounds = GetContinentalBounds();
			var image = new Bitmap(width, (int)((continentalBounds.Height * width) / continentalBounds.Width));
			using (var graphics = Graphics.FromImage(image))
			{
				var pen = new Pen(Color.White) { Width = 1 };
				var transformation = GeometryDrawing.TranslateFromEnvelope(continentalBounds, image);
				foreach (var state in _states)
				{
					if (continentalBounds.Contains(state.Geometry.EnvelopeInternal))
					{
						GeometryDrawing.DrawGeometry(state.Geometry, pen, graphics, transformation);
					}
				}
			}
			return image;
		}

		private static string Copy2013USStateShapeFilesToDirectory(string targetDirectory)
		{
			var assembly = typeof(USA).Assembly;
			var wantedResources = assembly.GetManifestResourceNames().Where(n => n.Contains("2013_us_state"));
			string shapeFilePath = null;
			foreach (var resource in wantedResources)
			{
				var resourceFilePath = resource.Substring("Mappier.Resources.".Length);
				var targetFilePath = Path.Combine(targetDirectory, resourceFilePath);
				using (var reader = assembly.GetManifestResourceStream(resource))
				using (var writer = File.Create(targetFilePath))
				{
					WriteStreamToStream(reader, writer);
				}
				if (resourceFilePath.EndsWith(".shp"))
				{
					shapeFilePath = targetFilePath;
				}
			}
			return shapeFilePath;
		}

		private static IEnumerable<State> ReadStates(ShapefileDataReader shapeReader)
		{
			while (shapeReader.Read())
			{
				yield return State.FromShapefileReaderCurrent(shapeReader);
			}
		}

		private Envelope GetContinentalBounds()
		{
			var washington = _states.Single(s => s.Name == "Washington");
			var maine = _states.Single(s => s.Name == "Maine");
			var florida = _states.Single(s => s.Name == "Florida");
			var minnesota = _states.Single(s => s.Name == "Minnesota");
			return new Envelope(washington.Geometry.Coordinates.Select(c => c.X).Min(),
				maine.Geometry.Coordinates.Select(c => c.X).Max(),
				florida.Geometry.Coordinates.Select(c => c.Y).Min(),
				minnesota.Geometry.Coordinates.Select(c => c.Y).Max());
		}

		private static void WriteStreamToStream(Stream source, Stream target)
		{
			var buffer = new byte[10 * 1024];
			int bytesRead;
			while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
			{
				target.Write(buffer, 0, bytesRead);
			}
		}
	}
}
