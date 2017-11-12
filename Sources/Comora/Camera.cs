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
		public Camera(GraphicsDevice device)
		{
            this.device = device;
			this.Debug = new DebugLayer(this);
            this.Transform = new Transform2D();
            this.viewport = new Vector2(this.device.Viewport.Width, this.device.Viewport.Height);
            this.viewportOffset = new Transform2D
            {
                Parent = this.Transform,
            };
            this.UpdateOffset();
		}

        #region Properties

        private GraphicsDevice device;

        private Transform2D viewportOffset;

        private float? width, height;

        private AspectMode aspectMode;

        private Vector2 viewport;

        #endregion

        #region Properties

        public Transform2D ViewportOffset => this.viewportOffset;

        public Transform2D Transform { get; set; }

        public DebugLayer Debug { get; private set; }

        public AspectMode ResizeMode
        {
            get => this.aspectMode;
            set
            {
                if(this.aspectMode != value)
                {
                    this.aspectMode = value;

                    if(value == AspectMode.Expand)
                    {
                        this.viewportOffset.Scale = Vector2.One;
                    }

                    this.UpdateOffset();
                }
            }
        }

        public float Width 
        {
            get
            {
                if(this.ResizeMode == AspectMode.Expand)
                    return this.viewport.X;
                
                return this.width ?? this.viewport.X;
            }
            set => this.width = value;
        }

        public float Height 
        {
            get
            {
                if (this.ResizeMode == AspectMode.Expand)
                    return this.viewport.Y;

                return this.height ?? this.viewport.Y;
            }
            set => this.height = value;
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

        #region Methods

        public Rectangle GetBounds()
        {
            var pos = this.Transform.AbsolutePosition;
            var w = Width * this.Transform.AbsoluteScale.X;
            var h = Height * this.Transform.AbsoluteScale.Y;
            var s = Math.Max(w, h);
            var x = pos.X - (s / 2);
            var y = pos.Y - (s / 2);
            return new Rectangle((int)x, (int)y, (int)w, (int)h);
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

        public void Update(GameTime time)
        {
            if(this.viewport.X != this.device.Viewport.Width || this.viewport.X != this.device.Viewport.Height)
            {
                this.viewport = new Vector2(this.device.Viewport.Width, this.device.Viewport.Height);
                this.UpdateOffset();
            }

            this.Debug.Update(time);
        }

        private void UpdateOffset()
        {
            var w = this.Width;
            var h = this.Height;

            switch (this.ResizeMode)
            {
                case AspectMode.FillStretch:
                    this.viewportOffset.Scale = new Vector2(w / viewport.X, h / viewport.Y);
                    w = this.viewport.X * this.viewportOffset.Scale.X;
                    h = viewport.Y * this.viewportOffset.Scale.Y;
                    break;
                case AspectMode.FillUniform:
                    var size = Math.Min(w / viewport.X, h / viewport.Y);
                    this.viewportOffset.Scale = new Vector2(size, size);
                    w = viewport.X * this.viewportOffset.Scale.X;
                    h = viewport.Y * this.viewportOffset.Scale.Y;
                    break;
            }

            this.viewportOffset.Position = new Vector2(-w * 0.5f, -h * 0.5f);
        }

        public ICamera Clone()
        {
            var result = new Camera(this.device);
            result.width = this.width;
            result.height = this.height;
            result.aspectMode = this.aspectMode;
            result.Transform.Position = this.Transform.Position;
            result.Transform.Rotation = this.Transform.Rotation;
            result.Transform.Scale = this.Transform.Scale;
            result.UpdateOffset();
            return result;
        }

        #endregion
    }
}


