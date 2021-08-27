using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Vidiludo
{
    public class WonkySprite
    {
        protected Image Frame0 = null;

        public Point Position = new Point();

        protected Rectangle ImageBounds = new Rectangle();
        protected Rectangle MovingBounds = new Rectangle();

        public WonkySprite()
        {
            Frame0 = null;
        }

        public WonkySprite(string ImageName)
        {
            Image Image = Properties.Resources.ResourceManager.GetObject(ImageName) as Image;

            Image.SelectActiveFrame(FrameDimension.Time, 0);
            Frame0 = Properties.Resources.ResourceManager.GetObject(ImageName) as Image;

            ImageBounds.Width = Frame0.Width;
            ImageBounds.Height = Frame0.Height;
        }

        public int Width
        {
            get
            {
                return ImageBounds.Width;
            }
        }

        public int Height
        {
            get
            {
                return ImageBounds.Height;
            }
        }

        public virtual Rectangle GetBounds()
        {
            return MovingBounds;
        }

        public void UpdateBounds()
        {
            MovingBounds = ImageBounds;

            MovingBounds.Offset(Position);
        }

        public virtual void Draw(Graphics g)
        {
            UpdateBounds();

            g.DrawImage(Frame0, MovingBounds, 0, 0, ImageBounds.Width, ImageBounds.Height, GraphicsUnit.Pixel);
        }
    }
}