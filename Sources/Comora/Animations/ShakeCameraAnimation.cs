namespace Comora
{
	using System;

	public class ShakeCameraAnimation : CameraAnimation
	{
		public ShakeCameraAnimation(TimeSpan duration, float intensity) : base(Comora.Easing.Mode.Linear,duration)
		{
			this.Intensity = intensity;
			this.random = new Random();
		}

		readonly Random random;

		public float Intensity { get; private set; }

		protected override void UpdateFromAmount(Camera camera, double amount)
		{
			camera.PositionOffset = new Microsoft.Xna.Framework.Vector2((float)random.NextDouble(), (float)random.NextDouble()) * (float)(1.0 - amount) * this.Intensity;
			camera.AngleOffset =  (float)(random.NextDouble() * (1.0 - amount) * this.Intensity / 1500);
		}
	}
}

