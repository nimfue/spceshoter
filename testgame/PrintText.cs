using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace testgame
{
	internal class PrintText
	{
		SpriteFont font;

		public PrintText(SpriteFont font)
		{
			this.font = font;
		}

		public void Print(string text, SpriteBatch spriteBatch, int X, int Y)
		{
			spriteBatch.DrawString(font, text, new Vector2(X, Y), Color.White);
		}
	}
}
