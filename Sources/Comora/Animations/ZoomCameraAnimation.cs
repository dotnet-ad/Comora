namespace Comora
{
	using System;

	public class ZoomCameraAnimation : CameraAnimation
	{
		public ZoomCameraAnimation(Easing.Mode mode, TimeSpan duration, float start, float end) : base(mode,duration)
		{
			this.Start = start;
			this.End = end;
		}

		public float Start { get; private set; }

		public float End { get; private set; }

		protected override void UpdateFromAmount(Camera camera, double amount)
		{
			camera.Scale = this.Start + (this.End - this.Start) * (float)amount;
		}
	}
}

