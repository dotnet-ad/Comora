namespace Comora.Diagnostics
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public interface IGrid
	{
		void AddLines(double intervals, Color color, double width = 1);

		void RemoveLines();

		void LoadContent(GraphicsDevice device);

		void Draw(SpriteBatch spriteBatch);

		void Draw(SpriteBatch spriteBatch, Vector2 parralax);
	}
}

