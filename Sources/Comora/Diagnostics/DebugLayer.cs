using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Comora.Diagnostics
{
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

		public void LoadContent(GraphicsDevice device)
		{
			this.font.LoadContent(device);
			this.Grid.LoadContent(device);
		}

		public void Draw(GameTime gameTime, SpriteBatch sb, Vector2 parralax)
		{
			this.FpsCounter.Update(gameTime);

			if (this.IsVisible)
			{
				this.Grid.Draw(sb,parralax);

				sb.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);
				this.FpsCounter.Draw(sb);
				sb.End();
			}
		}
	}
}

