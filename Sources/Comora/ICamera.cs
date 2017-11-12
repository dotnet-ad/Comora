namespace Comora
{
	using Comora.Diagnostics;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
    using Transform;

    /// <summary>
    /// Represents a player camera.
    /// </summary>
    public interface ICamera
    {
        #region Properties

        /// <summary>
        /// Gets or sets the way the content fits the screen.
        /// </summary>
        /// <value>The resize mode.</value>
        AspectMode ResizeMode { get; set; }

        /// <summary>
        /// Gets or sets the width of the screen.
        /// </summary>
        /// <value>The width.</value>
	    float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the screen.
        /// </summary>
        /// <value>The height.</value>
		float Height { get; set; }

        /// <summary>
        /// Gets or sets the transform of the camera (position, scale and rotation).
        /// </summary>
        /// <value>The transform.</value>
        Transform2D Transform { get; set; }

        Transform2D ViewportOffset { get; }

        /// <summary>
        /// Gets or sets the zoom level.
        /// </summary>
        /// <value>The zoom.</value>
        float Zoom { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>The rotation.</value>
        float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets the layer that displays debugging information.
        /// </summary>
        /// <value>The debug.</value>
		DebugLayer Debug { get; }

        #endregion

        #region Methods

        Rectangle GetBounds();

        #region Conversions

        /// <summary>
        /// Converts absolute world to the screen relative coordinates.
        /// </summary>
        /// <returns>The screen coordinates.</returns>
        /// <param name="worldPosition">A world absolute position.</param>
        /// <param name="screenPosition">The local screen position.</param>
        void ToScreen(ref Vector2 worldPosition, out Vector2 screenPosition);

        /// <summary>
        /// Converts relative coordinates to absolute world coordinate.
        /// </summary>
        /// <returns>The world coordinates.</returns>
        /// <param name="screenPosition">The local screen position.</param>
        /// <param name="worldPosition">A world absolute position.</param>
        void ToWorld(ref Vector2 screenPosition, out Vector2 worldPosition);

        #endregion

        #region Lifecycle

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="device">Device.</param>
        void LoadContent();

        void Update(GameTime time);

        #endregion

        ICamera Clone();

        #endregion
    }
}    

