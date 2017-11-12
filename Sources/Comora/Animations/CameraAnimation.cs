namespace Comora
{
	using System;

	public abstract class CameraAnimation : ICameraAnimation
	{
		public CameraAnimation(Easing.Mode mode, TimeSpan duration) 
		{
			this.Time = 0;
			this.Easing = mode;
			this.Duration = (long)duration.TotalMilliseconds;
		}

		public Easing.Mode Easing
		{
			get; 
			private set;
		}

		public double Duration
		{
			get; private set;
		}

		public double Time
		{
			get; private set;
		}

		protected virtual double GetAmount(double timeMS)
		{
			var amount = Math.Max(0, Math.Min(1, (1.0 * timeMS) / this.Duration));
			return Comora.Easing.Calculate(this.Easing, amount);
		}

		public bool Update(Camera camera, double timeMs)
		{
			this.Time += timeMs;

			var amount = this.GetAmount(this.Time);

			this.UpdateFromAmount(camera, amount);

			return (amount >= 1.0);
		}

		protected abstract void UpdateFromAmount(Camera camera, double amount);
	}
}

