using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace testgame
{
	abstract class Enemy : PhysicalObject
	{
		public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY) :
			base(texture, X, Y, speedX, speedY)
		{ }
		public abstract void Update(GameWindow window);
	}
	class Mine : Enemy
	{
		public Mine(Texture2D texture, float X, float Y) :
			base(texture, X, Y, 4.5f, 0.6f)
		{ }
		public override void Update(GameWindow window)
		{
			vector += speed;
			if (vector.X > window.ClientBounds.Width - Width ||
				vector.X< 0)
			{
				speed.X *= -1;
			}
			if (vector.Y > window.ClientBounds.Height)
			{
				isAlive = false;
			}
		}
	}

	class Laserbeam : Enemy
	{
		int beamTimer = 0;
		public Laserbeam(Texture2D texture, float X, float Y, Texture2D laserbeamWarning) :
			base(texture, X, Y, 0, 6f)
		{}
		public override void Update(GameWindow window)
		{
			vector += speed;
			if (vector.Y > window.ClientBounds.Height - Height)
			{
				speed.Y = 0;
			}
			if (beamTimer == 180)
			{
				isAlive = false;
			}
			beamTimer ++;
		}
	}
}