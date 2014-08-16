using GeoAPI.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappier
{
	public sealed class State
	{
		private readonly string _name;
		private readonly IGeometry _geometry;

		public State(string name, IGeometry geometry)
		{
			_name = name;
			_geometry = geometry;
		}

		public static State FromShapefileReaderCurrent(ShapefileDataReader reader)
		{
			return new State(reader.GetString(6), reader.Geometry);
		}

		public string Name { get { return _name; } }
		public IGeometry Geometry { get { return _geometry; } }
	}
}
