using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Vidiludo
{
	public class ScoreBoard
	{
        public Int64 FleetSize, GameScore, HighScore;

		public Point LeftElement = new Point(0, 0);
        public Point RightElement = new Point(0, 0);

		public ScoreBoard(int Right, int Top)
		{
            Layout(Right, Top);

            if (File.Exists("highscore.txt"))
            {
                StreamReader Stream = new StreamReader("highscore.txt");
                HighScore = Convert.ToInt32(Stream.ReadLine());
                Stream.Close();
            }

            NewGame();
		}

		public void NewGame()
		{
            GameScore = 0;
            NewSquad();
		}

        public void NewSquad()
        {
            FleetSize = 3;
        }

        public void Score(int Points)
        {
            GameScore += Points;
        }

        public void DepleteFleet()
        {
            if (0 >= --FleetSize) GameOver();
        }

        public void GameOver()
        {
            FleetSize = 0;

            if (HighScore < GameScore)
            {
                HighScore = GameScore;

                StreamWriter Stream = new StreamWriter("highscore.txt", false);
                Stream.WriteLine(HighScore.ToString());
                Stream.Close();
            }
        }

        public void Layout(int Right, int Top)
        {
            RightElement.X = Right - 100;
            RightElement.Y = Top;

            LeftElement.X = 0;
            LeftElement.Y = Top;
        }

        #region Draw Score Board

        private string[] ReserveSquad = {"ѫ", "ѫ ѫ"};

        public Font Font = new Font("Arial", 20.0f, FontStyle.Bold, GraphicsUnit.Pixel);

        public virtual void Draw(Graphics g)
        {
            if (0 < FleetSize)
            {
                g.DrawString("Score: " + GameScore.ToString(), Font, Brushes.GreenYellow, LeftElement.X, LeftElement.Y, new StringFormat());
                g.DrawString("Lives: " + ReserveSquad[FleetSize - 2], Font, Brushes.GreenYellow, RightElement.X, RightElement.Y, new StringFormat());
            }
            else
            {
                g.DrawString("Final Score: " + GameScore.ToString(), Font, Brushes.RoyalBlue, LeftElement.X, LeftElement.Y, new StringFormat());
                g.DrawString("High Score: " + HighScore.ToString(), Font, Brushes.RoyalBlue, RightElement.X, RightElement.Y, new StringFormat());
            }
        }

        #endregion
    }
}
