using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Vidiludo 
{
    public class Invader : WonkySprite
	{
		private Image OtherImage = null;

		private const int BombInterval = 200;

		private Bomb Bomb = new Bomb(0, 0);

		private bool ActiveBomb = false;

		public bool Exploding = false;

		public int CountExplosion = 15;

		public bool Vaporized = false;

		private Random Aleatory = null;

		public bool GoingRight = true;

		private const int Speed = 10;

		private long Counter = 0;

		public Invader(string i1, string i2) : base(i1)
		{
            Object O = Properties.Resources.ResourceManager.GetObject(i2);

            if (O is Image)
            {
                OtherImage = O as Image;
            }

            Aleatory = new Random((int) DateTime.Now.Ticks);

			Position.X = 20;
			Position.Y = 10;

			UpdateBounds();
		}

		public override void Draw(Graphics g)
		{
			UpdateBounds();

			if (Exploding)
			{
				DrawExplosion(g);
				return;
			}

            if (0 == Counter % 2)
            {
                g.DrawImage(Frame0, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(OtherImage, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);
            }

			if (ActiveBomb)
			{
			    Bomb.Draw(g);

				if (null != Form.ActiveForm)
				{
					if (Bomb.Position.Y > Form.ActiveForm.ClientRectangle.Height)
					{
						ActiveBomb = false;
					}
				}
			}

			if (!ActiveBomb && 0 == Counter % BombInterval)
			{
				ActiveBomb = true;

				Bomb.Position.X = MovingBounds.X + (MovingBounds.Width / 2);
				Bomb.Position.Y = MovingBounds.Y + 5;
			}
		}

		public void ResetBombPosition()
		{
			Bomb.Position.X = MovingBounds.X + (MovingBounds.Width / 2);
			Bomb.ResetBomb(MovingBounds.Y + 5);
		}

		public void SetCounter(long lCount)
		{
			Counter = lCount;
		}

		public void DrawExplosion(Graphics g)
		{
			if (Vaporized) return;

			if (0 < CountExplosion)
			{
                CountExplosion--;

                int xval = Aleatory.Next(MovingBounds.Width);
                int yval = Aleatory.Next(MovingBounds.Height);

                for (int i = 0; i < 50; i++)
				{
					xval += Position.X;
					yval += Position.Y;

					g.DrawLine(Pens.White, xval, yval, xval, yval + 1);
				}
			}
			else
			{
				Vaporized = true;
			}
		}

		public void Move()
		{
			if (Exploding) return;

            Position.X += GoingRight ? Speed : -Speed;

            Counter++;
		}

		public void MoveInPlace()
		{
			Counter++;
		}

		public Rectangle GetBombBounds()
		{
		  return Bomb.GetBounds();
		}

		public bool IsBombColliding(Rectangle r)
		{
            return ActiveBomb && Bomb.GetBounds().IntersectsWith(r);
		}
	}
}
