namespace Comora
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class CameraAnimationBuilder : ICameraAnimation, ICameraAnimationBuilder
	{
		public CameraAnimationBuilder()
		{
			this.animations = new List<ICameraAnimation[]>();
		}

		private int currentIndex;

		private readonly List<ICameraAnimation[]> animations;

		public double Duration => this.animations.Sum((parralel) => parralel.Max((a) =>  a.Duration));

		public double Time { get; private set; }

		public bool Update(Camera camera, double timeMs)
		{
			var currentAnimations = this.animations[this.currentIndex];

			var stepFinished = true;

			foreach (var anim in currentAnimations)
			{
				stepFinished &= anim.Update(camera, timeMs);
			}

			if (stepFinished)
			{
				this.currentIndex++;
			}
			
			return (currentIndex >= animations.Count);
		}

		public ICameraAnimationBuilder Then(params ICameraAnimation[] animation)
		{
			this.animations.Add(animation);
			return this;
		}
	}
}

