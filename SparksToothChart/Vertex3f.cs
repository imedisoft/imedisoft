namespace SparksToothChart
{
    public class Vertex3f
	{
		public float X, Y, Z;

		/// <summary>
		/// Initializes a new instance of the <see cref="Vertex3f"/> class.
		/// </summary>
		public Vertex3f()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vertex3f"/> class.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vertex3f(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Returns the vertex as a array of floats.
		/// </summary>
		public float[] GetFloatArray() => new float[3] { X, Y, Z };

		/// <summary>
		/// Returns a string representation of the vertex.
		/// </summary>
		public override string ToString() => X.ToString() + "," + Y.ToString() + "," + Z.ToString();
	}
}
