namespace Comora.Diagnostics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

	public class DebugLayer
	{
		public DebugLayer(Camera camera)
		{
			this.font = new PixelFont();
			this.FpsCounter = new FpsCounter(camera,font);
			this.Grid = new Grid(camera,font);
		}

		readonly PixelFont font;

		public bool IsVisible { get; set; }

		public IGrid Grid { get; }

        public IFpsCounter FpsCounter { get; }

        public Texture2D Pixel { get; private set; }

        public Color BackgroundColor { get; set; } = new Color(Color.Black, 0.2f);

        private GraphicsDevice device;

		public void LoadContent(GraphicsDevice device)
        {
            this.device = device;
            Pixel = new Texture2D(device, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
			this.font.LoadContent(device);
            this.Grid.LoadContent(device);
		}

        public void Update(GameTime gameTime)
        {
            this.FpsCounter.Update(gameTime);
        }

		public void Draw(SpriteBatch sb, Vector2 parralax)
		{
			if (this.IsVisible)
			{
                sb.Begin();
                sb.Draw(this.Pixel, new Rectangle(0, 0, this.device.Viewport.Width, this.device.Viewport.Height), BackgroundColor);
                sb.End();

                this.FpsCounter.Draw(sb);

				this.Grid.Draw(sb,parralax);

				this.FpsCounter.Draw(sb);
			}
		}
	}
}

