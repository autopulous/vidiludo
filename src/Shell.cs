using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Tiplemetry;

namespace Vidiludo
{
    class Shell : WonkySprite
    {
        private Image Frame1 = null;
        private Image Frame2 = null;

        public Shell(Coordinate LaunchPoint, string ImageName) : base(ImageName)
		{
            Position.X = LaunchPoint.X;
            Position.Y = LaunchPoint.Y;

            Image Image = Properties.Resources.ResourceManager.GetObject(ImageName) as Image;

            Image.SelectActiveFrame(FrameDimension.Time, 1);
            Frame1 = Image;

            Image.SelectActiveFrame(FrameDimension.Time, 2);
            Frame2 = Image;

            UpdateBounds();
		}
    }
}
