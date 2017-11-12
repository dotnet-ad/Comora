namespace Comora
{
	using System;

	public class RelayCameraAnimation : CameraAnimation
	{
		public RelayCameraAnimation(Easing.Mode mode, TimeSpan duration, Action<Camera,double> update) : base(mode, duration)
		{
			this.update = update;
		}

		readonly Action<Camera, double> update;

		protected override void UpdateFromAmount(Camera camera, double amount)
		{
			this.update(camera, amount);
		}
	}
}

