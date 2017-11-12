namespace Comora.Diagnostics
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public interface IFpsCounter
	{
        bool IsVisible { get; set; }

		float CurrentFramesPerSecond { get; }

		void Update(GameTime gameTime);

		void Draw(SpriteBatch sb);
	}
}

