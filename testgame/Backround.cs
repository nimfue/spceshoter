using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace testgame
{
	class Background
	{
		BackgroundSprite[,] background;
		int nrBackgroundsX, nrBackgroundsY;

		public Background(Texture2D texture, GameWindow window)
		{
			double tmpX = (double)window.ClientBounds.Width / texture.Width;
			nrBackgroundsX = (int)Math.Ceiling(tmpX);

			double tmpY = (double)window.ClientBounds.Height / texture.Height;
			nrBackgroundsY = (int)Math.Ceiling(tmpY) + 1;

			background = new BackgroundSprite[nrBackgroundsX, nrBackgroundsY];
			for(int col = 0; col < nrBackgroundsX; col++)
				for(int row = 0; row < nrBackgroundsY; row++)
				{
					int posX = col * texture.Width;
					int posY = row * texture.Height - texture.Height;
					background[col, row] = new BackgroundSprite(texture, posX, posY);
				}
		}
		public void Update(GameWindow window)
		{
			for (int col = 0; col < nrBackgroundsX; col++)
				for (int row = 0; row < nrBackgroundsY; row++)
					background[col, row].Update(window, nrBackgroundsY);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			for (int col = 0; col < nrBackgroundsX; col++)
				for (int row = 0; row < nrBackgroundsY; row++)
					background[col, row].Draw(spriteBatch);
		}
	}

	class BackgroundSprite : GameObject
	{
		public BackgroundSprite(Texture2D texture, float X, float Y):
			base(texture, X, Y)
		{ }
		public void Update(GameWindow window, int nrBackgroundsY)
		{
			vector.Y += 2f;
			if(vector.Y > window.ClientBounds.Height)
			{
				vector.Y = vector.Y - nrBackgroundsY * texture.Height;
			}
		}
	}
}
