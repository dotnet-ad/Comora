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

        private Texture2D pixel;

		private readonly Camera camera;

        private readonly Queue<float> sampleBuffer = new Queue<float>();

        private static readonly Color bgColor = new Color(Color.Black, 0.5f);

		#endregion

		#region Properties

		public long TotalFrames { get; private set; }

		public float AverageFramesPerSecond { get; private set; }

		public float CurrentFramesPerSecond { get; private set; }

        public int MaximumSamples { get; }

        public bool IsVisible { get; set; } = true;

		#endregion

        #region Lifecycle

        public void LoadContent(GraphicsDevice device)
        {
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

		public void Reset()
		{
			TotalFrames = 0;
			sampleBuffer.Clear();
		}

		public void Update(GameTime gameTime)
        {
            if (!IsVisible)
                return;
            
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

		public void Draw(SpriteBatch sb)
		{
            if (!IsVisible)
                return;
            
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

			var message = ((int)this.CurrentFramesPerSecond).ToString();
			var s = this.font.Measure(message, 15);

            var width = s.X + 20;
            var bounds = new Rectangle((int)sb.GraphicsDevice.Viewport.Width - 20 - width, 20, width,  20 + s.Y);

            sb.Draw(this.pixel, bounds, bgColor);
            this.font.Draw(sb, new Vector2(bounds.X + 10, bounds.Y + 10),message , Color.White,15);  

            sb.End();
		}

		#endregion

	}
}

