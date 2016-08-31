namespace Comora
{
	using System;

	public class WaitCameraAnimation : CameraAnimation
	{
		public WaitCameraAnimation(TimeSpan duration) : base(Comora.Easing.Mode.Linear,duration)
		{
		}

		protected override void UpdateFromAmount(Camera camera, double amount) { }
	}
}

