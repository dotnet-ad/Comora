namespace Comora
{
	using Comora.Diagnostics;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public static class SpriteBatchExtensions
	{
		public static void Begin(this SpriteBatch spriteBatch, ICamera camera, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null)
		{
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, camera.Offset.InvertAbsolute);
                       
			//spriteBatch.Begin(sortMode,blendState,samplerState,depthStencilState,rasterizerState,effect, camera.CreateTransform());
		}

		public static void Begin(this SpriteBatch spriteBatch, ICamera camera, Vector2 parralax, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null)
        {
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, camera.Offset.InvertAbsolute);
                
			//spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, camera.CreateTransform(parralax));
		}

		public static void Draw(this SpriteBatch spriteBatch, IGrid grid)
		{
			grid.Draw(spriteBatch);
		}

		public static void Draw(this SpriteBatch spriteBatch, GameTime time, DebugLayer grid)
		{
			grid.Draw(spriteBatch, Vector2.One);
		}

		public static void Draw(this SpriteBatch spriteBatch, GameTime time, DebugLayer grid, Vector2 parralax)
		{
			grid.Draw(spriteBatch,parralax);
		}

	}
}

