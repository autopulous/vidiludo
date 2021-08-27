using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Vidiludo
{
	public class Bullet : WonkySprite
	{
		const int kBulletInterval = 20;
		public int BulletInterval = kBulletInterval;
		
		public Bullet(int x, int y)
		{
			ImageBounds.Width = 5;
			ImageBounds.Height = 15;

			Position.X = x;
			Position.Y = y;
		}

		public void Reset()
		{
			if (null != Form.ActiveForm)
			{
				Position.Y = Form.ActiveForm.ClientRectangle.Bottom;
				MovingBounds.Y = Position.Y;
			}

			BulletInterval = kBulletInterval;
		}

		public override void Draw(Graphics g)
		{
			UpdateBounds();
			g.FillRectangle(Brushes.Chartreuse, MovingBounds);
			Position.Y -= BulletInterval;
		}
	}
}
