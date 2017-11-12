namespace Comora
{
	using System;
	using Microsoft.Xna.Framework;

	public class MoveCameraAnimation : CameraAnimation
	{
		public MoveCameraAnimation(Easing.Mode mode, TimeSpan duration, Vector2 start, Vector2 end) : base(mode,duration)
		{
			this.Start = start;
			this.End = end;
		}

		public Vector2 Start { get; private set; }

		public Vector2 End { get; private set; }

		protected override void UpdateFromAmount(Camera camera, double amount)
		{
			camera.Position = this.Start + (this.End - this.Start) * (float)amount;
		}
	}
}

