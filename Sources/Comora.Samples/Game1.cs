using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Comora.Samples
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D pixel;
		Camera camera;
		List<Tuple<Rectangle,Color>> rectangles;
		List<Tuple<Rectangle, Color>> rectangles2;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			this.Window.AllowUserResizing = true;
			graphics.PreferredBackBufferWidth = 1000;
			graphics.PreferredBackBufferHeight = 500;
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			this.camera = new Camera(this.graphics.GraphicsDevice);
			this.camera.Width = 400;
			this.camera.Height = 400;
			this.camera.Debug.Grid.AddLines(50, Color.White, 2);
			this.camera.Debug.Grid.AddLines(200, Color.Cyan, 4);

			var random = new Random();
			this.rectangles = new List<Tuple<Rectangle, Color>>();
			for (int i = 0; i < 500; i++)
			{
				var rect = new Rectangle(random.Next(-1000, 1000), random.Next(-1000, 1000), random.Next(50, 100), random.Next(50, 100));
				var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 120);
				this.rectangles.Add(new Tuple<Rectangle, Color>(rect, color));
			}
			this.rectangles2 = new List<Tuple<Rectangle, Color>>();
			for (int i = 0; i < 500; i++)
			{
				var rect = new Rectangle(random.Next(-1000, 1000), random.Next(-1000, 1000), random.Next(50, 100), random.Next(50, 100));
				var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
				this.rectangles2.Add(new Tuple<Rectangle, Color>(rect, color));
			}

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData(new Color[] { Color.White });

			this.camera.LoadContent(GraphicsDevice);
		}

		KeyboardState previousState;

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			var state = Keyboard.GetState();

			if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.S) && !this.camera.IsAnimated)
			{
				this.camera.Shake(TimeSpan.FromSeconds(0.4), 80);
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.S) && !this.camera.IsAnimated)
			{
				this.camera.Shake(TimeSpan.FromSeconds(0.2), 40);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.Z) && !this.camera.IsAnimated)
			{
				this.camera.Zoom(TimeSpan.FromSeconds(1), 3, 1);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Z) && !this.camera.IsAnimated)
			{
				this.camera.Zoom(TimeSpan.FromSeconds(1),1,3);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.M) && !this.camera.IsAnimated)
			{
				this.camera.Move(TimeSpan.FromSeconds(1), new Vector2(300, 300), new Vector2(0, 0));
			}
			if (Keyboard.GetState().IsKeyDown(Keys.M) && !this.camera.IsAnimated)
			{
				this.camera.Move(TimeSpan.FromSeconds(1), new Vector2(0,0), new Vector2(300, 300));
			}
			if (Keyboard.GetState().IsKeyDown(Keys.C) && !this.camera.IsAnimated)
			{
				this.camera.Move(TimeSpan.FromSeconds(1), new Vector2(300, 300), new Vector2(0, 0), Easing.Mode.EaseIn).ThenZoom(TimeSpan.FromSeconds(1),1,5);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.R) && !this.camera.IsAnimated)
			{
				this.camera.Rotate(TimeSpan.FromSeconds(1), 0);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.R) && !this.camera.IsAnimated)
			{
				this.camera.Rotate(TimeSpan.FromSeconds(1), 1);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				this.camera.Position = this.camera.ToWorld(new Vector2(-1, -1) * Mouse.GetState().Position.ToVector2()) - this.camera.Position;
			}

			if (!this.previousState.IsKeyDown(Keys.Enter) && state.IsKeyDown(Keys.Enter))
			{
				this.camera.ResizeMode = (ResizeMode)((((int)this.camera.ResizeMode + 1) % 3));
			}

			if (!this.previousState.IsKeyDown(Keys.G) && state.IsKeyDown(Keys.G))
			{
				this.camera.Debug.IsVisible = !this.camera.Debug.IsVisible;
			}

			this.previousState = state;

			this.camera.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(this.camera, new Vector2(0.85f,0.85f));

			foreach (var rect in this.rectangles)
			{
				spriteBatch.Draw(pixel, rect.Item1, rect.Item2);
			}

			spriteBatch.End();

			spriteBatch.Begin(this.camera);

			foreach (var rect in this.rectangles2)
			{
				spriteBatch.Draw(pixel, rect.Item1, rect.Item2);
			}
			spriteBatch.End();

			this.spriteBatch.Draw(gameTime, this.camera.Debug);

			base.Draw(gameTime);
		}
	}
}

