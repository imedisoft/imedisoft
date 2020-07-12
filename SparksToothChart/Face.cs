using System.Collections.Generic;

namespace SparksToothChart
{
    /// <summary>
    /// A face is a single polygon, usually a rectangle. Will soon be only triangles.
    /// </summary>
    public class Face
	{
		/// <summary>
		/// A list of indices to the VertexNormal list contained in the ToothGraphic object. 
		/// 0 indexed, unlike the raw files which are 1 indexed. 
		/// </summary>
		public List<int> IndexList { get; } = new List<int>();

		public Face()
		{
		}

		public override string ToString()
		{
			string result = "";

			for (int i = 0; i < IndexList.Count; i++)
			{
				if (i > 0)
				{
					result += ",";
				}
				result += IndexList[i].ToString();
			}

			return result;
		}

		public Face Copy()
		{
            return new Face
            {
                IndexList = new List<int>(this.IndexList)
            };
		}
	}
}
