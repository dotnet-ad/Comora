namespace Comora.Diagnostics
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class Grid : IGrid
	{
		public class Unit
		{
			public double Value { get; set; }

			public double Width { get; set; }

			public Color Color { get; set; }
		}

		public Grid(ICamera camera, PixelFont font)
		{
			this.camera = camera;
			this.font = font;
			this.units = new List<Unit>();
		}

		#region Fields

		private PixelFont font;

		private Texture2D pixel;

		private readonly ICamera camera;

		private List<Unit> units;

		#endregion

		#region Units

		public void AddLines(double intervals, Color color, double width = 1)
		{
			this.units.Add(new Unit()
			{
				Value = intervals,
				Width = width,
				Color = color
			});
		}

		public void RemoveLines()
		{
			this.units.Clear();
		}

		#endregion

		#region Lifecycle

		public void LoadContent(GraphicsDevice device)
		{
			pixel = new Texture2D(device, 1, 1);
			pixel.SetData(new Color[] { Color.White });
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			this.Draw(spriteBatch, Vector2.One);
		}

		Color bgColor = new Color(Color.Black, 0.5f);
		Color bgLightColor = new Color(Color.Black, 0.2f);

		public void Draw(SpriteBatch spriteBatch, Vector2 parralax)
		{
			if (this.pixel == null)
				throw new InvalidOperationException("The grid 'LoadContent' must be invoked prior to any 'Draw'.");

			var scale = this.camera.AbsoluteScale;
			var angle = this.camera.AbsoluteAngle;
			var offset = this.camera.ToScreen(0, 0);

			var w = this.camera.Width * scale.X;
			var h = this.camera.Height * scale.Y;

			var translation = Matrix.CreateTranslation(new Vector3(offset.X, offset.Y, 0) * -0.5f);
			var transform = translation * Matrix.CreateRotationZ(angle) * Matrix.Invert(translation);

			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);

			spriteBatch.Draw(this.pixel, new Rectangle(0, 0, (int)w, (int)h), bgLightColor);

			foreach (var unit in this.units)
			{
				var intervalX = unit.Value * scale.X;
				var intervalY = unit.Value * scale.Y;

				var startX = offset.X % intervalX;
				var startY = offset.Y % intervalY;

				for (double x = startX; x < w; x += intervalX)
				{
					spriteBatch.Draw(this.pixel, new Rectangle((int)(x - unit.Width / 2), 0, (int)(unit.Width), (int)h), unit.Color);
				}

				for (double y = startY; y < h; y += intervalY)
				{
					spriteBatch.Draw(this.pixel, new Rectangle(0, (int)(y - unit.Width / 2), (int)w, (int)(unit.Width)), unit.Color);
				}
			}

			spriteBatch.End();

			var center = this.camera.ToWorld(w / 2, h / 2);
			var message = $"{(int)center.X},{(int)center.Y}\nANGLE: {this.camera.Angle}\nZOOM: {this.camera.Scale}";

			spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

			var messageSize = this.font.Measure(message, 10);

			spriteBatch.Draw(this.pixel, new Rectangle(20, 20, messageSize.X + 20, messageSize.Y + 20), bgColor);

			this.font.Draw(spriteBatch, new Vector2(30, 30), message , Color.White, 10);

			spriteBatch.End();
		}

		#endregion

	}
}

