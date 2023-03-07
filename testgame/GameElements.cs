using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace testgame
{
	internal class GameElements
	{
		static Menu menu;
		static Texture2D menuSprite;
		static Vector2 menuPos;
		static Player player;
		static PrintText text;
		static List<Enemy> normal_enemies;
		static List<Enemy> boss_enemies;
		static Texture2D mineTexture;
		static Texture2D redMineTexture;
		static Texture2D laserbeamTexture;
		static Texture2D laserbeamWarningTexture;
		static Background background;
		static Texture2D ship_texture;
		static int maxNumEnemies;

		public enum State { Menu, Run, HighScore, Quit};
		public static State currentState;
		public static void Initialize()
		{

		}
		public static void LoadContent(ContentManager content, GameWindow window)
		{
			StreamReader sr = new StreamReader("gameconfig.gcfg");
			try
			{
				string str = sr.ReadLine();
				string[] s = str.Split(": ");
				maxNumEnemies = int.Parse(s[1]);
			}
			catch 
			{
				sr.Close();
				StreamWriter sw = new StreamWriter("gameconfig.gcfg");
				sw.WriteLine("maxNumEnemies: "+20);
				sw.Close();
				maxNumEnemies = 20;
				
			}

			menu = new Menu((int)State.Menu);

			menu.AddItem(content.Load<Texture2D>("images/menu/menuStart"), (int)State.Run);
			menu.AddItem(content.Load<Texture2D>("images/menu/menuScore"), (int)State.HighScore);
			menu.AddItem(content.Load<Texture2D>("images/menu/menuExit"), (int)State.Quit);


			menuPos.X = 200;//window.ClientBounds.Width / 2 - menuSprite.Width / 2;
			menuPos.Y = 200;//window.ClientBounds.Height / 2 - menuSprite.Height / 2;

			ship_texture = content.Load<Texture2D>("images/player/spaceship_default");
			Texture2D bullet_texture = content.Load<Texture2D>("images/bullets/bullet");
			player = new Player(ship_texture,
				window.ClientBounds.Width / 2 - ship_texture.Width / 2,
				window.ClientBounds.Height - ship_texture.Width,
				2.5f, 4.5f, bullet_texture);

			normal_enemies = new List<Enemy>();
			boss_enemies = new List<Enemy>();
			mineTexture = content.Load<Texture2D>("images/enemies/Mine");
			redMineTexture = content.Load<Texture2D>("images/enemies/redmine");
			laserbeamTexture = content.Load<Texture2D>("images/enemies/laserbeam");
			laserbeamWarningTexture = content.Load<Texture2D>("images/enemies/laserbeamWarning");

			SpriteFont font = content.Load<SpriteFont>("myFont");
			text = new PrintText(font);

			background = new Background(content.Load<Texture2D>("images/game backround"), window);
		}
		public static void Reset(GameWindow window, ContentManager content)
		{
			player.Reset(window.ClientBounds.Width / 2 - ship_texture.Width / 2,
						window.ClientBounds.Height - ship_texture.Height,
						2.5f, 4.5f);
			normal_enemies.Clear();
			boss_enemies.Clear();

		}
		public static State MenuUpdate(GameTime gameTime, GameWindow window)
		{
			background.Update(window);
			return (State)menu.Update(gameTime);
		}
		public static void MenuDraw(SpriteBatch spriteBatch)
		{
			background.Draw(spriteBatch);
			menu.Draw(spriteBatch);
		}
		public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime, GraphicsDeviceManager graphics)
		{
			player.Update(window, graphics, gameTime);
			background.Update(window);
			foreach (Enemy enemy in normal_enemies.ToList())
			{
				if (enemy.IsAlive)
				{
					foreach (Bullet b in player.Bullets)
					{
						if (b.CheckCollision(enemy))
						{
							enemy.IsAlive = false;
							b.IsAlive = false;
						}
					}

					if (enemy.CheckCollision(player))
						player.IsAlive = false;
					enemy.Update(window);
				}
				else
				{
					normal_enemies.Remove(enemy);
				}
			}
			Random random = new Random();
			foreach (Enemy enemy in boss_enemies.ToList())
			{
				if (enemy.IsAlive)
				{
					foreach (Bullet b in player.Bullets)
					{
						if (b.CheckCollision(enemy))
						{
							enemy.IsAlive = false;
							b.IsAlive = false;
							int rndX = random.Next(20, window.ClientBounds.Width - laserbeamTexture.Width + 20);
							int Y = 0 - laserbeamTexture.Height;
							PhysicalObject laserbeamWarning = new LaserbeamWarning(laserbeamWarningTexture, rndX, 0);
							Enemy laserbeam = new Laserbeam(laserbeamTexture, rndX, Y);
							normal_enemies.Add(laserbeam);
						}
					}
					if (enemy.CheckCollision(player))
						player.IsAlive = false;

					enemy.Update(window);
				}
				else
				{
					boss_enemies.Remove(enemy);
				}
			}
			
			int newEnemy = random.Next(1, 180);
			if (normal_enemies.Count < maxNumEnemies)
			{
				if (newEnemy < 10)
				{
					int redMine = random.Next(1, 15);
					if (redMine == 2)
					{
						int rndX = random.Next(0, window.ClientBounds.Width - mineTexture.Width);
						int rndY = -random.Next(0, window.ClientBounds.Height);
						Enemy tmpRedEnemy = new Mine(redMineTexture, rndX, rndY);
						boss_enemies.Add(tmpRedEnemy);
					}
					else
					{
						int rndX = random.Next(0, window.ClientBounds.Width - mineTexture.Width);
						int rndY = -random.Next(0, window.ClientBounds.Height);
						Enemy tmpEnemy = new Mine(mineTexture, rndX, rndY);
						normal_enemies.Add(tmpEnemy);
					}

				}
			}
			
			if (!player.IsAlive)
			{
				Reset(window, content);
				return State.Menu;
			}
			return State.Run;
		}
		public static void RunDraw(SpriteBatch spriteBatch, GameWindow window)
		{
			background.Draw(spriteBatch);
			string s = "x: " + player.X + "\nY: " + player.Y;
			text.Print(s, spriteBatch, 0, 0);
			player.Draw(spriteBatch);
			text.Print("#mines: " + normal_enemies.Count, spriteBatch, window.ClientBounds.Width - 190, 0);
			text.Print("#redMines: " + boss_enemies.Count, spriteBatch, window.ClientBounds.Width - 100, 0);
			foreach (Enemy enemy in normal_enemies)
			{
				enemy.Draw(spriteBatch);
			}
			foreach (Enemy enemy in boss_enemies)
			{
				enemy.Draw(spriteBatch);
			}
		}
		public static State HighScoreUpdate(GameWindow window)
		{
			background.Update(window);
			KeyboardState keyboardState = Keyboard.GetState();
			if (keyboardState.IsKeyDown(Keys.Escape))
				return State.Menu;
			return State.HighScore;
		}
		public static void HighScoreDraw(SpriteBatch spriteBatch)
		{
			background.Draw(spriteBatch);
			text.Print("Highscore:", spriteBatch, 0, 0);
		}
	}
}
