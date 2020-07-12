using System.Collections.Generic;

namespace SparksToothChart
{
    /// <summary>
    /// A series of vertices that are all connected into one continuous simple line.
    /// </summary>
    public class LineSimple
	{
		public List<Vertex3f> Vertices;

		public LineSimple()
		{
			Vertices = new List<Vertex3f>();
		}

		/// <summary>
		/// Specify a line as a series of points.  It's implied that they are grouped by threes.
		/// </summary>
		public LineSimple(params float[] coords)
		{
			Vertices = new List<Vertex3f>();

			for (int i = 0; i < coords.Length; i += 3)
			{
				Vertices.Add(
					new Vertex3f(
						coords[i],
						coords[i + 1], 
						coords[i + 2]));
			}
		}
	}
}
