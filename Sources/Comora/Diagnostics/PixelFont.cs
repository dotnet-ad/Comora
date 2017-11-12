namespace Comora.Diagnostics
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class PixelFont
	{
		private readonly Dictionary<char, short[]> Pixels = new Dictionary<char, short[]>()
		{
			{ '0', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0 }},
			{ '1', new short[] { 0, 1, 0,    /**/ 1, 1, 0,    /**/ 0, 1, 0,    /**/ 0, 1, 0,    /**/ 1, 1, 1    }},
			{ '2', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 0, 0, 1, 0, /**/ 0, 1, 0, 0, /**/ 1, 1, 1, 1 }},
			{ '3', new short[] { 1, 1, 1, 0, /**/ 0, 0, 0, 1, /**/ 0, 1, 1, 0, /**/ 0, 0, 0, 1, /**/ 1, 1, 1, 0 }},
			{ '4', new short[] { 0, 0, 1, 1, /**/ 0, 1, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 1, /**/ 0, 0, 0, 1 }},
			{ '5', new short[] { 1, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 1, 1, 1, /**/ 0, 0, 0, 1, /**/ 1, 1, 1, 0 }},
			{ '6', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 0, /**/ 1, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0 }},
			{ '7', new short[] { 1, 1, 1, 1, /**/ 0, 0, 0, 1, /**/ 0, 0, 1, 0, /**/ 0, 0, 1, 0, /**/ 0, 0, 1, 0 }},
			{ '8', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0 }},
			{ '9', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 1, /**/ 0, 0, 0, 1, /**/ 0, 1, 1, 0 }},
			{ '-', new short[] { 0, 0, 0, /**/ 0, 0, 0, /**/ 1, 1, 1, /**/ 0, 0, 0, /**/ 0, 0, 0 }},
			{ '+', new short[] { 0, 0, 0, /**/ 0, 1, 0, /**/ 1, 1, 1, /**/ 0, 1, 0, /**/ 0, 0, 0 }},
			{ ':', new short[] { 0, 0, 0, /**/ 0, 1, 0, /**/ 0, 0, 0, /**/ 0, 1, 0, /**/ 0, 0, 0 }},
			{ '(', new short[] { 0, 1, /**/ 1, 0, /**/ 1, 0, /**/ 1, 0, /**/ 0, 1 }},
			{ ')', new short[] { 1, 0, /**/ 0, 1, /**/ 0, 1, /**/ 0, 1, /**/ 1, 0 }},
			{ '.', new short[] { 0, 0, /**/ 0, 0, /**/ 0, 0, /**/ 0, 0, /**/ 1, 0 }},
			{ ',', new short[] { 0, 0, /**/ 0, 0, /**/ 0, 0, /**/ 0, 1, /**/ 1, 0 }},
			{ 'A', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1 }},
			{ 'B', new short[] { 1, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 0 }},
			{ 'C', new short[] { 0, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 0, 0, 0, /**/ 1, 0, 0, 0, /**/ 0, 1, 1, 1 }},
			{ 'D', new short[] { 1, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 0 }},
			{ 'E', new short[] { 1, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 1, 1, 1 }},
			{ 'F', new short[] { 1, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 0, 0, 0 }},
			{ 'G', new short[] { 1, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 1, 0, 1, 1, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 1 }},
			{ 'H', new short[] { 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1 }},
			{ 'I', new short[] { 1, /**/ 1, /**/ 1, /**/ 1, /**/ 1 }},
			{ 'J', new short[] { 0, 0, 0, 1, /**/ 0, 0, 0, 1, /**/ 0, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0 }},
			{ 'K', new short[] { 1, 0, 0, 1, /**/ 1, 0, 1, 0, /**/ 1, 1, 0, 0, /**/ 1, 0, 1, 0, /**/ 1, 0, 0, 1 }},
			{ 'L', new short[] { 1, 0, 0, /**/ 1, 0, 0, /**/ 1, 0, 0, /**/ 1, 0, 0, /**/ 1, 1, 1 }},
			{ 'M', new short[] { 1, 0, 0, 0, 1, /**/ 1, 1, 0, 1, 1, /**/ 1, 0, 1, 0, 1, /**/ 1, 0, 0, 0, 1, /**/ 1, 0, 0, 0, 1 }},
			{ 'N', new short[] { 1, 0, 0, 1, /**/ 1, 1, 0, 1, /**/ 1, 0, 1, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1 }},
			{ 'O', new short[] { 0, 1, 1, 1, 0, /**/ 1, 0, 0, 0, 1, /**/ 1, 0, 0, 0, 1, /**/ 1, 0, 0, 0, 1, /**/ 0, 1, 1, 1, 0 }},
			{ 'P', new short[] { 1, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 0, /**/ 1, 0, 0, 0, /**/ 1, 0, 0, 0 }},
			{ 'Q', new short[] { 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 1, 0, /**/ 0, 1, 0, 1 }},
			{ 'R', new short[] { 1, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 0, /**/ 1, 0, 1, 0, /**/ 1, 0, 0, 1 }},
			{ 'S', new short[] { 0, 1, 1, 1, /**/ 1, 0, 0, 0, /**/ 0, 1, 1, 0, /**/ 0, 0, 0, 1, /**/ 1, 1, 1, 0 }},
			{ 'T', new short[] { 1, 1, 1, 1, 1, /**/ 0, 0, 1, 0, 0, /**/ 0, 0, 1, 0, 0, /**/ 0, 0, 1, 0, 0, /**/ 0, 0, 1, 0, 0 }},
			{ 'U', new short[] { 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0 }},
			{ 'V', new short[] { 1, 0, 0, 0, 1, /**/ 1, 0, 0, 0, 1, /**/ 0, 1, 0, 1, 0, /**/ 0, 1, 0, 1, 0, /**/ 0, 0, 1, 0, 0 }},
			{ 'W', new short[] { 1, 0, 1, 0, 1, /**/ 1, 0, 1, 0, 1, /**/ 1, 0, 1, 0, 1, /**/ 0, 1, 0, 1, 0, /**/ 0, 1, 0, 1, 0 }},
			{ 'X', new short[] { 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 0, 1, 1, 0, /**/ 1, 0, 0, 1, /**/ 1, 0, 0, 1 }},
			{ 'Y', new short[] { 1, 0, 0, 1, /**/ 1, 0, 0, 1, /**/ 1, 1, 1, 1, /**/ 0, 0, 0, 1, /**/ 1, 1, 1, 0 }},
			{ 'Z', new short[] { 1, 1, 1, 1, /**/ 0, 0, 0, 1, /**/ 0, 1, 1, 0, /**/ 1, 0, 0, 0, /**/ 1, 1, 1, 1 }},
		};

		private const int LetterSpace = 1;

		private const int LineSpace = 3;

		private const int Space = 4;

		private Dictionary<char, Texture2D> Textures;

		public void LoadContent(GraphicsDevice device)
		{
			if (this.Textures == null)
			{
				this.Textures = new Dictionary<char, Texture2D>();
				foreach (var c in Pixels)
				{
					Textures[c.Key] = this.LoadCharacter(device, c.Key, c.Value);
				}
			}
		}

		private Texture2D LoadCharacter(GraphicsDevice device, char c, short[] pixels)
		{
			const int height = 5;
			var width = pixels.Length / height;
			var result = new Texture2D(device, width, height);
		 	var data = new Color[width * height];
			for (int pixel = 0; pixel < data.Length; pixel++)
			{
				data[pixel] = pixels[pixel] > 0 ? Color.White : Color.Transparent;
			}
			result.SetData(data);
			return result;
		}

		public Point Measure(string message, int fontSize = 10)
		{
			var scale = fontSize / 5.0f;
			var x = 0;
			var y = fontSize;
			var w = 0;

			foreach (var c in message)
			{
				if (c == ' ')
				{
					x += LetterSpace + Space;
				}
				else if (c == '\n')
				{
					y += LineSpace + fontSize;
					x = 0;
				}
				else
				{
					var texture = this.Textures[c];
					x += (int)((((x > 0) ? LetterSpace : 0) + texture.Width) * scale);
				}

				w = Math.Max(w, x);
			}

			return new Point((int)w, (int)y);
		}


		public void Draw(SpriteBatch spriteBatch, Vector2 position, string message, Color color, int fontSize = 10)
		{
			var scale = fontSize / 5.0f;
			var x = position.X;
			var y = position.Y;

			foreach (var c in message)
			{
				if (c == ' ')
				{
					x += LetterSpace + Space;
				}
				else if (c == '\n')
				{
					y += LineSpace + fontSize;
					x = position.X;
				}
				else
				{
					var texture = this.Textures[c];
					spriteBatch.Draw(texture, new Vector2(x, y), color: color, scale: Vector2.One * scale);
					x += (int)((((x > 0) ? LetterSpace : 0) + texture.Width) * scale);
				}
			}
		}
	}
}

