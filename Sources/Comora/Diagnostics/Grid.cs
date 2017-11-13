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
			this.units.Add(new Unit
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

        public bool IsVisible { get; set; } = true;

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

        public void Draw(SpriteBatch spriteBatch, Vector2 parralax)
		{
            if (!this.IsVisible)
                return;
            
			if (this.pixel == null)
				throw new InvalidOperationException("The grid 'LoadContent' must be invoked prior to any 'Draw'.");

            var px = this.camera.Transform.AbsolutePosition.X;
            var py = this.camera.Transform.AbsolutePosition.Y;

            var bounds = this.camera.GetBounds();

            var w = bounds.Width;
            var h = bounds.Height;

            spriteBatch.Begin(this.camera);

			foreach (var unit in this.units)
			{
				var intervalX = unit.Value;
                var intervalY = unit.Value;

                var columns = 2 + (int)(w / intervalX);
                var rows = 2 + (int)(h / intervalY);

                var lw = w * intervalY;
                var lh = h * intervalX;

                var startX = ((int)(px / intervalX) * intervalX) - (1 + columns / 2) * intervalX;
                var startY = ((int)(py / intervalY) * intervalY) - (1 + rows / 2) * intervalY;

                for (int i = 0; i <= columns; i++)
				{
                    var x = startX + i * intervalX;
                    spriteBatch.Draw(this.pixel, new Rectangle((int)x, (int)startY, (int)(unit.Width), (int)lh), unit.Color);
				}

                for (int i = 0; i <= rows; i++)
                {
                    var y = startY + i * intervalY;
                    spriteBatch.Draw(this.pixel, new Rectangle((int)startX, (int)y, (int)lw, (int)(unit.Width)), unit.Color);
				}
			}

			spriteBatch.End();

            var message = $"{(int)px},{(int)py}\nANGLE: {this.camera.Transform.Rotation}\nZOOM: {this.camera.Zoom}";

			spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

			var messageSize = this.font.Measure(message, 10);

			spriteBatch.Draw(this.pixel, new Rectangle(20, 20, messageSize.X + 20, messageSize.Y + 20), bgColor);

			this.font.Draw(spriteBatch, new Vector2(30, 30), message , Color.White, 10);

			spriteBatch.End();
		}

		#endregion

	}
}