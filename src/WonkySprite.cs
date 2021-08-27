using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Tuplemetry;

namespace Vidiludo
{
    public class WonkySprite
    {
        protected Image Frame0 = null;

        public Coordinate Position = new Coordinate();

        protected Polygon ImageBounds = new Polygon();
        protected Polygon MovingBounds = new Polygon();

        /// <summary>
        /// Saucer state
        /// </summary>

        public int Width { get; private set; }
        public int Height { get; private set; }

        public WonkySprite()
        {
            Frame0 = null;
        }

        public WonkySprite(string ImageName)
        {
            Image Image = Properties.Resources.ResourceManager.GetObject(ImageName) as Image;

            Image.SelectActiveFrame(FrameDimension.Time, 0);
            Frame0 = Properties.Resources.ResourceManager.GetObject(ImageName) as Image;

            Width = Frame0.Width;
            Height = Frame0.Height;
        }

        public virtual Polygon GetBounds()
        {
            return MovingBounds;
        }

        public void UpdateBounds()
        {
            MovingBounds = ImageBounds;

            MovingBounds.Offset(Position.X, Position.Y);
        }

        public virtual void Draw(Graphics g)
        {
            UpdateBounds();

            g.DrawImage(Frame0, MovingBounds, 0, 0, Width, Height, GraphicsUnit.Pixel);
        }
    }
}