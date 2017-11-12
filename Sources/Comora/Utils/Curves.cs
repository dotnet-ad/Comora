namespace Comora
{
	using Microsoft.Xna.Framework;

	public class Curves
	{
		#region Bezier

		public struct BezierNode
		{
			public BezierNode(Vector2 point, Vector2 dir)
			{
				this.Point = point;
				this.Direction = dir;
			}

			public Vector2 Point { get; set; }

			public Vector2 Direction { get; set; }
		}

		public static Vector2 Bezier(float time, Vector2 start0, Vector2 start1, Vector2 end0, Vector2 end1)
		{
			float u = 1 - time;
			float tt = time * time;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * time;

			var p = uuu * start0;
			p += 3 * uu * time * start1;
			p += 3 * u * tt * end0;
			p += ttt * end1;

			return p;
		}

		public static Vector2 Bezier(float time, BezierNode start, BezierNode end)
		{
			return Bezier(time, start.Point, start.Direction, end.Point, end.Direction);
		}

		#endregion
	}
}