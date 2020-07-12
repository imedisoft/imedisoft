namespace SparksToothChart
{
    /// <summary>
    /// Contains one vertex (xyz), one normal, and possibly one texture coordinate.
    /// </summary>
    public class VertexNormal
	{
		public Vertex3f Vertex;
		public Vertex3f Normal;

		/// <summary>
		/// 2D, So the third value is always zero.  Values are between 0 and 1.  Can be null.
		/// </summary>
		public Vertex3f Texture;

		public override string ToString()
		{
			string result = "v:" + Vertex.ToString() + " n:" + Normal.ToString();
			if (Texture != null)
			{
				result += " t:" + Texture.ToString();
			}
			return result;
		}
	}
}
