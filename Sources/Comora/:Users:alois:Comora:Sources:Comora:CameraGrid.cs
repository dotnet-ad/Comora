namespace Comora
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class CameraGrid : IGrid
	{
		public class Unit
		{
			public double Value { get; set; }

			public double Width { get; set; }

			public Color Color { get; set; }
		}

		public CameraGrid(ICamera camera)
		{
			this.camera = camera;
			this.units = new List<Unit>();
			this.IsVisible = false;
		}

		#region Fields

		private Texture2D pixel;

		private readonly ICamera camera;

		private List<Unit> units;

		#endregion

		#region Properties

		public bool IsVisible
		{
			get;
			set;
		}

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

		public void Draw(SpriteBatch spriteBatch, Vector2 parralax)
		{
			if (this.IsVisible)
			{
				if (this.pixel == null)
					throw new InvalidOperationException("The grid 'LoadContent' must be invoked prior to any 'Draw'.");

				var scale = this.camera.AbsoluteScale;
				var angle = this.camera.AbsoluteAngle;
				var offset = this.camera.ToScreen(0,0);

				var translation = Matrix.CreateTranslation(new Vector3(offset.X,offset.Y,0) * -0.5f);
				var transform = translation * Matrix.CreateRotationZ(angle) * Matrix.Invert(translation);

				spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);

				foreach (var unit in this.units)
				{
					var intervalX = unit.Value * scale.X;
					var intervalY = unit.Value * scale.Y;

					var startX =  offset.X % intervalX;
					var startY = offset.Y % intervalY;
					var endX = this.camera.Width * scale.X;
					var endY = this.camera.Height * scale.Y;

					for (double x = startX; x < endX; x += intervalX)
					{
						spriteBatch.Draw(this.pixel, new Rectangle((int)(x - unit.Width / 2), 0, (int)(unit.Width), (int)endY), unit.Color);
					}

					for (double y = startY; y < endY; y += intervalY)
					{
						spriteBatch.Draw(this.pixel, new Rectangle(0, (int)(y - unit.Width / 2), (int) endX, (int)(unit.Width)), unit.Color);
					}
				}

				spriteBatch.End();
			}
		}

		#endregion

	}
}

