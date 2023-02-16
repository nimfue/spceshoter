﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;

namespace testgame
{
	abstract class GameObject
	{
		protected Texture2D texture;
		protected Vector2 vector;

		public GameObject(Texture2D texture, float X, float Y)
		{
			this.texture = texture;
			this.vector.X = X;
			this.vector.Y = Y;
		}
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, vector, Color.White);
		}
		public float X { get { return vector.X; } }
		public float Y { get { return vector.Y; } }
		public float Width { get { return texture.Width; } }
		public float Height { get { return texture.Height; } }
	}
	abstract class MovingObject : GameObject
	{
		protected Vector2 speed;
		public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y)
		{
			speed.X = speedX;    
			speed.Y = speedY;
		}
	}

	abstract class PhysicalObject : MovingObject
	{
		protected bool isAlive = true;

		public PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY) :
			base(texture, X, Y, speedX, speedY)
		{ }

		public bool CheckCollision(PhysicalObject other)
		{
			Rectangle myRect = new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y),
				Convert.ToInt32(Width), Convert.ToInt32(Height));
			Rectangle otherRect = new Rectangle(Convert.ToInt32(other.X), Convert.ToInt32(other.Y),
			   Convert.ToInt32(other.Width), Convert.ToInt32(other.Height));
			return myRect.Intersects(otherRect);
		}
		public bool IsAlive
		{
			get { return isAlive; }
			set { isAlive = value; }
		}
	}
}

