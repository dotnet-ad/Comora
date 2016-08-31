namespace Comora
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Diagnostics;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	/// <summary>
	/// Represents the point of view.
	/// </summary>
	public class Camera : ICamera
	{
		public Camera(GraphicsDevice device) : this(device,() => new Vector2(device.Viewport.Width, device.Viewport.Height))
		{
		}

		public Camera(GraphicsDevice device, Func<Vector2> getWindowSize)
		{
			this.Debug = new DebugLayer(this);
			this.getWindowSize = getWindowSize;
			var initialSize = getWindowSize();
			this.Width = initialSize.X;
			this.Height = initialSize.Y;
			this.Scale = 1;
			this.animations = new List<ICameraAnimation>();
		}

		readonly Func<Vector2> getWindowSize;

		private float width, height;

		#region Transform

		public ResizeMode ResizeMode { get; set; }

		private Vector2 ViewportScale
		{
			get
			{
				var result = Vector2.One;

				var windowsSize = getWindowSize();

				switch (this.ResizeMode)
				{
					case ResizeMode.FillStretch:
						result.X = this.Width / windowsSize.X;
						result.Y = this.Height / windowsSize.Y;
						break;

					case ResizeMode.FillUniform:
						var scale =  Math.Min(this.Width / windowsSize.X , this.Height / windowsSize.Y);
						result.X = scale;
						result.Y = scale;
						break;
				}

				return result;
			}
		}

		public Vector2 AbsoluteScale
		{
			get
			{
				var scale = ViewportScale;
				scale.X = (this.Scale + this.ScaleOffset) / scale.X;
				scale.Y = (this.Scale + this.ScaleOffset) / scale.Y;
				return scale;
			}
		}

		public float AbsoluteAngle
		{
			get
			{
				return this.Angle + this.AngleOffset;
			}
		}

		public Vector2 AbsolutePosition
		{
			get
			{
				return this.Position + this.PositionOffset;
			}
		}

		public float Width
		{
			get { return this.ResizeMode == ResizeMode.Expand ? this.getWindowSize().X : width; }
			set { this.width = value; }
		}

		public float Height
		{
			get { return this.ResizeMode == ResizeMode.Expand ? this.getWindowSize().Y : height; }
			set { this.height = value; }
		}

		public DebugLayer Debug
		{
			get; private set;
		}

		public Vector2 Position { get; set; }

		public float Angle { get; set; }

		public float Scale { get; set; }

		public float AngleOffset { get; set; }

		public Vector2 PositionOffset { get; set; }

		public float ScaleOffset { get; set; }

		#endregion

		#region Collisions

		public Rectangle GetBounds()
		{
			return this.GetBounds(Vector2.One);
		}

		public Rectangle GetBounds(Vector2 parralax)
		{
			var tl = new Vector2(0, 0);
			var tr = new Vector2(this.Width, 0);
			var bl = new Vector2(0, this.Height);
			var br = new Vector2(this.Width, this.Height);

			tl = ToWorld(tl);
			tr = ToWorld(tr);
			bl = ToWorld(bl);
			br = ToWorld(br);

			var x = (int)Math.Floor(Math.Min(tl.X, Math.Min(tr.X, Math.Min(bl.X, br.X))));
			var w = (int)Math.Ceiling(Math.Max(tl.X, Math.Max(tr.X, Math.Max(bl.X, br.X)))) - x;
			var y = (int)Math.Floor(Math.Min(tl.Y, Math.Min(tr.Y, Math.Min(bl.Y, br.Y))));
			var h = (int)Math.Ceiling(Math.Max(tl.Y, Math.Max(tr.Y, Math.Max(bl.Y, br.Y)))) - y;

			return new Rectangle(x,y,w,h);
		}

		#endregion

		#region Transform matrix

		public Matrix CreateTransform()
		{
			return this.CreateTransform(Vector2.One);
		}

		public Matrix CreateTransform(Vector2 parralax)
		{
			var viewport = this.ViewportScale;
			var angle = this.AbsoluteAngle;
			var scale = this.AbsoluteScale;
			var position = this.AbsolutePosition;

			return Matrix.CreateTranslation(new Vector3(-1 * position.X * parralax.X, -1 * position.Y * parralax.Y, 0))
				 * Matrix.CreateRotationZ(angle)
		         * Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1))
				 * Matrix.CreateTranslation(new Vector3(this.Width * 0.5f * viewport.X, this.Height * 0.5f * viewport.Y, 0));
		}

		#endregion

		#region Conversions

		public Vector2 ToScreen(float x, float y)
		{
			return ToScreen(new Vector2(x, y));
		}

		public Vector2 ToScreen(Vector2 worldPosition)
		{
			return Vector2.Transform(worldPosition, CreateTransform());
		}

		public Vector2 ToWorld(float x, float y)
		{
			return ToWorld(new Vector2(x, y));
		}

		public Vector2 ToWorld(Vector2 screenPosition)
		{
			return Vector2.Transform(screenPosition, Matrix.Invert(CreateTransform()));
		}

		#endregion

		#region Animations

		private List<ICameraAnimation> animations;

		public bool IsAnimated { get { return this.animations.Any(); } }

		public ICameraAnimationBuilder StartAnimation()
		{
			var builder = new CameraAnimationBuilder();
			this.animations.Add(builder);
			return builder;
		}

		#endregion

		#region Lifecycle

		public void LoadContent(GraphicsDevice device)
		{
			this.Debug.LoadContent(device);
		}

		public void Update(GameTime time)
		{
			var delta = time.ElapsedGameTime.TotalMilliseconds;

			for (int i = 0; i < animations.Count;)
			{
				var animation = this.animations[i];
				if (animation.Update(this, delta))
				{
					animations.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		#endregion
	}
}


