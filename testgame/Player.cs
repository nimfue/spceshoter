using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace testgame
{
	class Player : PhysicalObject
	{ 
		int point = 0;
		List<Bullet> bullets;
		Texture2D bulletTexture;
		double timeSinceLastBullet = 0;

		public Player(Texture2D texture, float X, float Y,float speedX, float speedY, Texture2D bulletTexture)
			: base(texture, X, Y, speedX, speedY)
		{
			this.bulletTexture = bulletTexture;
			bullets = new List<Bullet>();
		}

		public List<Bullet> Bullets { get { return bullets; } }

		public void Update(GameWindow window, GraphicsDeviceManager _graphics, GameTime gameTime)
		{
			
			if (vector.X > _graphics.PreferredBackBufferWidth - texture.Width)
			{
				vector.X = _graphics.PreferredBackBufferWidth - texture.Width;
			}
			if (vector.X < texture.Width - texture.Width)
			{
				vector.X = texture.Width - texture.Width;
			}
			if (vector.Y > _graphics.PreferredBackBufferHeight - texture.Height)
			{
				vector.Y = _graphics.PreferredBackBufferHeight - texture.Height;
			}
			if (vector.Y < texture.Height - texture.Height)
			{
				vector.Y = texture.Height - texture.Height;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				vector.Y -= speed.Y;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				vector.Y += speed.Y;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				vector.X -= speed.X;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				vector.X += speed.X;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 400)
				{
					Bullet tmp = new Bullet(bulletTexture, X + Width/2 - bulletTexture.Width/2, Y-10);
					bullets.Add(tmp);
					timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
				}
			}
			foreach (Bullet bullet in bullets.ToList())
			{
				bullet.Update();
				if (!bullet.IsAlive)
					bullets.Remove(bullet);
			}
			
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, vector, Color.White);
			foreach (Bullet bullet in bullets)
				bullet.Draw(spriteBatch);
		}
		public void Reset(float X, float Y, float speedX, float speedY)

		{
			vector.X = X;
			vector.Y = Y;
			speed.X = speedX;
			speed.Y = speedY;
			bullets.Clear();
			timeSinceLastBullet = 0;
			isAlive = true;

		}
	}

	

	class Bullet : PhysicalObject
	{
		public Bullet(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 7f)
		{ }
		public void Update()
		{
			vector.Y -= speed.Y;
			if (vector.Y < 0)
				isAlive = false;
		}
	}
}
