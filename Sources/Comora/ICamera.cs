namespace Comora
{
	using Comora.Diagnostics;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public interface ICamera
	{
		ResizeMode ResizeMode { get; set; }

	    float Width { get; set; }

		float Height { get; set; }

		Vector2 Position { get; set; }

		float Angle { get; set; }

		float Scale { get; set; }

		float AngleOffset { get; set; }

		Vector2 PositionOffset { get; set; }

		float ScaleOffset { get; set; }

		Vector2 AbsoluteScale { get; }

		float AbsoluteAngle { get; }

		Vector2 AbsolutePosition { get; }

		DebugLayer Debug { get; }

		Rectangle GetBounds();

		Rectangle GetBounds(Vector2 parralax);

		Matrix CreateTransform();

		Matrix CreateTransform(Vector2 parralax);

		Vector2 ToScreen(float x, float y);

		Vector2 ToScreen(Vector2 worldPosition);

		Vector2 ToWorld(float x, float y);

		Vector2 ToWorld(Vector2 screenPosition);

		bool IsAnimated { get; }

		ICameraAnimationBuilder StartAnimation();

		void LoadContent(GraphicsDevice device);

		void Update(GameTime time);
	}
}    

