using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Vidiludo
{
    // Delegate types used to hook up event notifiers

    public delegate void DisintegratedEventHandler(object sender, EventArgs e);

    public class Sprite
    {
        public event DisintegratedEventHandler DisintegratedSubscriber;

        private Random Aleatory = new Random((int) DateTime.Now.Ticks);

        private SoundEffect Sound = null;

        private Form Form;

        public Sprite(Form Form)
        {
            this.Form = Form;
        }

        public void AnimateSprite(Bitmap Animation, Point Vector, SoundEffect Sound)
        {
            this.Exploded = false;

            this.Animation = Animation;
            this.Vector = Vector;

            this.Sound = Sound;
        }

        public void AnimateExplosion(int Frames, SoundEffect Sound)
        {
            ExplosionIndex = Frames;
            Vector = Stopped;

            if (null != Sound) Sound.Play();
        }

        public virtual void Draw(Graphics g)
        {
            if (!Exploded)
            {
                if (0 == ExplosionIndex)
                    AnimateImage(g);
                else
                    AnimateExplosion(g);
            }
        }

        #region Sprite Geometry Control

        private Rectangle Perimeter = new Rectangle(0, 0, 0, 0);

        #region Sprite Vector

        public int Run
        {
            get
            {
                return Vector.X;
            }
            set
            {
                Vector.X = value;
            }
        }

        public int Rise
        {
            get
            {
                return Vector.Y;
            }
            set
            {
                Vector.Y = value;
            }
        }

        #endregion

        #region Sprite Size

        public int Width
        {
            get
            {
                return Perimeter.Width;
            }
            set
            {
                Perimeter.Width = value;
            }
        }

        public int Height
        {
            get
            {
                return Perimeter.Height;
            }
            set
            {
                Perimeter.Height = value;
            }
        }

        #endregion

        #region Sprite Location

        public int X
        {
            get
            {
                return Perimeter.X;
            }
            set
            {
                Perimeter.X = value;
            }
        }

        public int Y
        {
            get
            {
                return Perimeter.Y;
            }
            set
            {
                Perimeter.Y = value;
            }
        }

        public int Top
        {
            get
            {
                return Perimeter.Top;
            }
            set
            {
                Perimeter.Y = value;
            }
        }

        public int Bottom
        {
            get
            {
                return Perimeter.Bottom;
            }
            set
            {
                Perimeter.Y = value - Perimeter.Height;
            }
        }

        public int Left
        {
            get
            {
                return Perimeter.Left;
            }
            set
            {
                Perimeter.X = value;
            }
        }

        public int Right
        {
            get
            {
                return Perimeter.Right;
            }
            set
            {
                Perimeter.X = value - Perimeter.Width;
            }
        }

        public void Set(int X, int Y)
        {
            Perimeter.X = X;
            Perimeter.Y = Y;
        }

        public void Offset(int X, int Y) 
        {
            Perimeter.Offset(X, Y);
        }

        public Rectangle Location
        {
            set
            {
                Perimeter = value;
            }
            get
            {
                return Perimeter;
            }
        }

        #endregion

        #endregion

        #region Sprite Explosion Control

        private bool Exploded = false;
        private int ExplosionIndex = 0;

        public bool Exploding
        {
            get
            {
                return 0 < ExplosionIndex;
            }
        }

        public bool Disintegrated
        {
            get
            {
                return this.Exploded;
            }
        }

        private void AnimateExplosion(Graphics g)
        {
            for (int i = 0; i < 50; i++)
            {
                g.DrawRectangle(Pens.Chartreuse, Aleatory.Next(Perimeter.Width), Aleatory.Next(Perimeter.Height), 2, 2);
            }

            ExplosionIndex--;

            if (0 == ExplosionIndex)
            {
                Exploded = true;

                if (null != DisintegratedSubscriber) DisintegratedSubscriber(this, EventArgs.Empty);
            }
        }
        
        #endregion

        #region Sprite Render Control

        private Image[] Frame = null;
        private int Frames = 0, FrameIndex = 0;

        private Point Vector = new Point(0, 0);
        private static Point Stopped = new Point(0, 0);

        private Bitmap Animation
        {
            set
            {
                Perimeter.Size = value.Size;;

                Frames = value.GetFrameCount(FrameDimension.Time);

                Frame = new Image[Frames];

                for (int i = Frames - 1; i >= 0; i--)
                {
                    value.SelectActiveFrame(FrameDimension.Time, i);
                    Frame[i] = value;
                }
            }
        }

        private void AnimateImage(Graphics g)
        {
            Perimeter.Offset(Vector);

            g.DrawImage(Frame[FrameIndex], Perimeter, 0, 0, Perimeter.Width, Perimeter.Height, GraphicsUnit.Pixel);

            FrameIndex++;
            FrameIndex %= Frames;
        }

        #endregion
    }
}
