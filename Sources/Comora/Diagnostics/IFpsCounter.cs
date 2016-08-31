namespace Comora.Diagnostics
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public interface IFpsCounter
	{
		float CurrentFramesPerSecond { get; }

		void Update(GameTime gameTime);

		void Draw(SpriteBatch sb);
	}
}

