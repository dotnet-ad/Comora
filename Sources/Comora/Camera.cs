namespace Comora
{
    using System;
    using Diagnostics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Transform;

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
            this.device = device;
			this.Debug = new DebugLayer(this);
			this.getWindowSize = getWindowSize;
			var initialSize = getWindowSize();
			this.width = initialSize.X;
			this.Height = initialSize.Y;
            this.Transform = new Transform2D();
            this.UpdateOffset();
		}

        private void UpdateOffset()
        {
            this.offset = new Transform2D()
            {
                Parent = this.Transform,
                Position = new Vector2(-this.Width * 0.5f, -this.Height * 0.5f)
            };
        }

        #region Properties

        private Func<Vector2> getWindowSize;

        private GraphicsDevice device;

        private Transform2D offset;

        private float width, height;

        #endregion

        #region Transform

        public Transform2D Offset => this.offset;

        public Transform2D Transform { get; set; }

        public DebugLayer Debug { get; private set; }

        public AspectMode ResizeMode { get; set; }

        public float Width 
        {
            get => this.width;
            set
            {
                this.width = value;
                this.UpdateOffset();
            }
        }

        public float Height 
        {
            get => this.height;
            set
            {
                this.height = value;
                this.UpdateOffset();
            }
        }

        public float Zoom
        {
            get => 1 / this.Transform.Scale.X;
            set => this.Transform.Scale = new Vector2(1 / value,1 / value);
        }

        public float Rotation
        {
            get => this.Transform.Rotation;
            set => this.Transform.Rotation = value;
        }


        public Vector2 Position
        {
            get => this.Transform.AbsolutePosition;
            set => this.Transform.Position = value;
        }

        #endregion

        #region Lifecycle

        public Rectangle GetBounds()
        {
            var pos = this.Transform.AbsolutePosition;
            var w = Width;
            var h = Height;
            var x = pos.X - (w / 2);
            var y = pos.Y - (h / 2);
            return new Rectangle((int)x, (int)y, (int)w, (int)h);
        }

        public Rectangle GetBounds(Vector2 parralax)
        {
            throw new NotImplementedException();
        }

        public void LoadContent()
        {
            this.Debug.LoadContent(device);
        }

        public void ToScreen(ref Vector2 worldPosition, out Vector2 screenPosition)
        {
            this.Transform.ToLocalPosition(ref worldPosition, out screenPosition);
        }

        public void ToWorld(ref Vector2 screenPosition, out Vector2 worldPosition)
        {
            this.Transform.ToAbsolutePosition(ref screenPosition, out worldPosition);
        }

        #endregion
    }
}


