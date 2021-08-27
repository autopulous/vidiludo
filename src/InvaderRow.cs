using System;
using System.Drawing;

namespace Vidiludo
{
    public class InvaderRow
	{
		public Invader[] Invaders = new Invader[11];

		public Point LastPosition = new Point(0, 0);

		public const int BombSeparation = 50;

		public InvaderRow(string image1, string image2, int row)
		{
			for (int column = 0; column < Invaders.Length; column++)
			{
			   Invaders[column] = new Invader(image1, image2);
			   Invaders[column].Position.X = column * (Invaders[column].GetBounds().Width + 15);
			   Invaders[column].Position.Y = row * (Invaders[column].GetBounds().Height + 20);
			   Invaders[column].SetCounter(column * BombSeparation);
			}

			LastPosition = Invaders[Invaders.Length - 1].Position;
		}

		public void ResetBombCounters()
		{
			for (int i = 0; i < Invaders.Length; i++)
			{
				Invaders[i].ResetBombPosition();
				Invaders[i].SetCounter(i * BombSeparation);
			}
		}

		public Invader this [int index]   // indexer declaration
		{
			get 
			{
			  return Invaders[index];
			}
		}

		public void Draw(Graphics g)
		{
			for (int i = 0; i < Invaders.Length; i++)
			{
				Invaders[i].Draw(g);
			}
		}

		public bool GoingRight
		{
			set
			{
				for (int i = 0; i < Invaders.Length; i++)
				{
					Invaders[i].GoingRight = value;
				}
			}
		}

		public void Move()
		{
			for (int i = 0; i < Invaders.Length; i++)
			{
				Invaders[i].Move();
			}

            if (Invaders[0].GoingRight)
            {
                LastPosition = Invaders[Invaders.Length - 1].Position;
            }
            else
            {
                LastPosition = Invaders[0].Position;
            }
		}

        public void MoveDown()
        {
            for (int i = 0; i < Invaders.Length; i++)
            {
                Invaders[i].Position.Y += Invaders[i].GetBounds().Height / 4;
                Invaders[i].UpdateBounds();
            }
        }
        
        public void MoveInPlace()
		{
			for (int i = 0; i < Invaders.Length; i++)
			{
				Invaders[i].MoveInPlace();
			}
		}

		public Invader GetFirstInvader()
		{
			int count = 0;
			Invader Invader = Invaders[count];

			while (Invader.Exploding && Invaders.Length - 1 > count)
			{
			   count++;
			   Invader = Invaders[count];
			}

			return Invader;
		}

		public Invader GetLastInvader()
		{
			int count = Invaders.Length - 1;
			Invader Invader = Invaders[count];

			while (Invader.Exploding && 0 < count)
			{
				count--;
				Invader = Invaders[count];
			}

			return Invader;
		}

		public int NumberOfLiveInvaders()
		{
			int count = 0;

			for (int i = 0; i < Invaders.Length; i++)
			{
				if (!Invaders[i].Vaporized) count++;
			}

			return count;
		}

        public int CollisionTest(Rectangle aRect)
        {
            for (int i = 0; i < Invaders.Length; i++)
            {
                if (!Invaders[i].Exploding && Invaders[i].GetBounds().IntersectsWith(aRect)) return i;
            }

            return -1;
        }

        public bool AlienHasLanded(int bottom)
		{
			for (int i = 0; i < Invaders.Length; i++)
			{
                if (!Invaders[i].Exploding && Invaders[i].GetBounds().Bottom >= bottom) return true;
			}

			return false;
		}
	}
}
