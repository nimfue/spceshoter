using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace testgame
{

	class LaserbeamWarning : PhysicalObject
	{
		int beamTimer = 0;
		public LaserbeamWarning(Texture2D texture, float X, float Y) :
			base(texture, X, Y, 0, 0)
		{ }
		public void Update(GameWindow window)
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
			beamTimer++;
		}
	}
	class Laserbeam : Enemy
	{
		int timeSinceWarning = 0;
		int beamTimer = 0;
		public Laserbeam(Texture2D texture, float X, float Y) :
			base(texture, X, Y, 0, 12f)
		{ }
		public override void Update(GameWindow window)
		{
			if (timeSinceWarning > 180)
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
				beamTimer++;
			}
			timeSinceWarning++;
		}
	}
}
