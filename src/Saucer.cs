using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Vidiludo 
{
	public class Saucer : WonkySprite
	{
		private Image Frame1 = null;
		private Image Frame2 = null;

        public int ScoreValue;

        public bool Exploding;

        public int ExplosionAnimation;

		public bool Vaporized;

		private int MovementIncrement;
		private int Frame;

        private Random Randomizer;

        public Font Font = new Font("Compact", 20.0f, FontStyle.Bold, GraphicsUnit.Pixel);

        public Saucer(string ImageName) : base(ImageName)
		{
            Image Image = Properties.Resources.ResourceManager.GetObject(ImageName) as Image;

            Image.SelectActiveFrame(FrameDimension.Time, 1);
            Frame1 = Image;

            Image.SelectActiveFrame(FrameDimension.Time, 2);
            Frame2 = Image;

            Randomizer = new Random((int) DateTime.Now.Ticks);

            Position.Y = 10;

            Reset();
		}

		public void Reset()
		{
			Vaporized = false;
			Exploding = false;
			ExplosionAnimation = 0;

            ScoreValue = Randomizer.Next(1, 4) * 50;

            if (0 == Randomizer.Next() % 2)
            {
                MovementIncrement = 10;
                Position.X = 0;
            }
            else
            {
                MovementIncrement = -10;
                Position.X = 700;
            }

            UpdateBounds();
		}

		public override void Draw(Graphics g)
		{
            if (Vaporized) return;

            if (Exploding)
            {
                DrawExplosion(g);
                return;
            }
            
            if (0 == Frame % 3)
                g.DrawImage(Frame0, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);
            else if (1 == Frame % 3)
                g.DrawImage(Frame1, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);
            else
                g.DrawImage(Frame2, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);

            UpdateBounds();
        }

		public void DrawExplosion(Graphics g)
		{
			ExplosionAnimation++;

			if (ExplosionAnimation < 15)
			{
                int xval = Randomizer.Next(MovingBounds.Width);
                int yval = Randomizer.Next(MovingBounds.Height);

                for (int i = 0; i < 50; i++)
				{
					xval += Position.X;
					yval += Position.Y;

					g.DrawLine(Pens.White, xval, yval, xval, yval+1);
				}
			}
			else
			{
				Vaporized = true;

                g.DrawString(ScoreValue.ToString(), Font, Brushes.RoyalBlue, MovingBounds, new StringFormat());
			}
		}

		public void Move()
		{
			if (Exploding) return;

            Frame++;
            Position.X += MovementIncrement;
		}
	}
}
