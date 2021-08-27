using System;
using System.IO;
using System.Drawing;
using Tuplemetry;

namespace Vidiludo
{
	public class Turret : Sprite
	{
        private static Bitmap Image = Properties.Resources.turret;

        private static Stream[] MovementSounds = {Properties.Resources.Beat__1_, Properties.Resources.Beat__2_, Properties.Resources.Beat__3_};
        private static Stream[] ExplosionSounds = {Properties.Resources.Explosion__3_};

        private static SoundEffect MovementSoundEffect = new SoundEffect(MovementSounds, 100, false);
        private static SoundEffect ExplosionSoundEffect = new SoundEffect(ExplosionSounds, 100000, false);

        private static Point LeftVector = new Point(-5, 0);
        private static Point StopVector = new Point(0, 0);
        private static Point RightVector = new Point(+5, 0);

        public Turret(Form Form) : base(Form)
        {
            X = Form.ClientRectangle.Left + ((Form.ClientRectangle.Width - Width) / 2);
            Y = Form.ClientRectangle.Bottom - 50;
        }
     
        public void Leftward()
        {
            AnimateSprite(Image, LeftVector, MovementSoundEffect); 
        }

        public void Stop()
        {
            AnimateSprite(Image, StopVector, null);
        }

        public void Rightward()
        {
            AnimateSprite(Image, RightVector, MovementSoundEffect);
        }

        public void Explode()
        {
            AnimateExplosion(15, ExplosionSoundEffect);
        }

        public Coordinate Gunpoint
		{
            get
            {
                return new Coordinate(Left + Width / 2, Top - 10);
            }
		}
	}
}
