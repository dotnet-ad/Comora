namespace Comora.Diagnostics
{
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class FpsCounter : IFpsCounter
	{
		public FpsCounter(Camera camera, PixelFont font, int maximumSamples = 100)
		{
			this.camera = camera;
			MaximumSamples = maximumSamples;
			this.font = font;
		}

		#region Fields

		private readonly PixelFont font;

		readonly Camera camera;

		private readonly Queue<float> sampleBuffer = new Queue<float>();

		#endregion

		#region Properties

		public long TotalFrames { get; private set; }

		public float AverageFramesPerSecond { get; private set; }

		public float CurrentFramesPerSecond { get; private set; }

        public int MaximumSamples { get; }

        public bool IsVisible { get; set; } = true;

		#endregion

		#region Lifecycle

		public void Reset()
		{
			TotalFrames = 0;
			sampleBuffer.Clear();
		}

		public void Update(GameTime gameTime)
		{
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			CurrentFramesPerSecond = 1.0f / deltaTime;

			sampleBuffer.Enqueue(CurrentFramesPerSecond);

			if (sampleBuffer.Count > MaximumSamples)
			{
				sampleBuffer.Dequeue();
				AverageFramesPerSecond = sampleBuffer.Average(i => i);
			}
			else
			{
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
		}

		Color bgColor = new Color(Color.Black, 0.5f);

		public void Draw(SpriteBatch sb)
		{
            if (!IsVisible)
                return;
            
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

			var message = ((int)this.CurrentFramesPerSecond).ToString();
			var s = this.font.Measure(message, 15);

			this.font.Draw(sb, new Vector2(this.camera.Width - s.X - 20, 20),message , Color.White,15);  

            sb.End();
		}

		#endregion

	}
}

