using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using System;
using System.Linq;

namespace testgame
{
	public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
		public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 700;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();

			GameElements.currentState = GameElements.State.Menu;
			GameElements.Initialize();

			base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			GameElements.LoadContent(Content, Window);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			switch (GameElements.currentState)
			{
				case GameElements.State.Run:
					GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime, _graphics);
					break;
				case GameElements.State.HighScore:
					GameElements.currentState = GameElements.HighScoreUpdate(Window);
					break;
				case GameElements.State.Quit:
					this.Exit();
					break;
				default:
					GameElements.currentState = GameElements.MenuUpdate(gameTime, Window);
					break;

			}
			base.Update(gameTime);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			GraphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin();

			switch (GameElements.currentState)
			{
				case GameElements.State.Run:
					GameElements.RunDraw(_spriteBatch, Window);
					break;
				case GameElements.State.HighScore:
					GameElements.HighScoreDraw(_spriteBatch);
					break;
				case GameElements.State.Quit:
					this.Exit();
					break;
				default:
					GameElements.MenuDraw(_spriteBatch);
					break;
			}

			_spriteBatch.End();
			base.Draw(gameTime);
        }
    }
}
