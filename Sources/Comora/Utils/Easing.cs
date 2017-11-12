namespace Comora
{
	using System;
	using Microsoft.Xna.Framework;

	public static class Easing
	{
		public enum Mode
		{
			Linear,
			EaseIn,
			EaseOut,
			EaseBoth,
		}

		/// <summary>
		/// Calculate the current value for a time between zero and one, and a curve mode.
		/// </summary>
		/// <param name="mode">Curve mode.</param>
		/// <param name="time">Current time (1.0f represents the one way total duration).</param>
		public static double Calculate(Mode mode, double time)
		{
			time = Math.Max(0.0,Math.Min(time, 1.0));

			switch (mode)
			{
				case Mode.EaseIn: return time * time;
				case Mode.EaseOut: return -1 * time * (time - 2);
				case Mode.EaseBoth: 
					time /= 0.5;
					if (time < 1) return 0.5 * time * time;
					time--;
					return -0.5 * (time * (time - 2) - 1);
				default: return time;
			}
		}

		public static Vector4 Calculate(Mode mode, double time, Vector4 start, Vector4 end)
		{
			time = Easing.Calculate(mode, time);

			end -= start;
			end *= (float)time;
			end += start;

			return end;
		}

		public static Vector3 Calculate(Mode mode, double time, Vector3 start, Vector3 end)
		{
			time = Easing.Calculate(mode, time);

			end -= start;
			end *= (float)time;
			end += start;

			return end;
		}

		public static Vector2 Calculate(Mode mode, double time, Vector2 start, Vector2 end)
		{
			time = Easing.Calculate(mode, time);

			end -= start;
			end *= (float)time;
			end += start;

			return end;
		}

		public static float Calculate(Mode mode, double time, float start, float end)
		{
			time = Easing.Calculate(mode, time);

			end -= start;
			end *= (float)time;
			end += start;

			return end;
		}

		public static Color Calculate(Mode mode, double time, Color start, Color end)
		{
			var startVector = start.ToVector4();
			var endVector = end.ToVector4();

			return new Color(Easing.Calculate(mode, time, startVector, endVector));
		}
	}
}

