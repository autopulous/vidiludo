using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vidiludo
{
	public class Form : System.Windows.Forms.Form
	{
        private IContainer components;

        private Timer Timer;
        private long TickCount = 0;

        private Audio Audio;

        private int Level = 0;
        private ScoreBoard Score = null;

		private Turret Turret = null;

        private Saucer Saucer = null;
        private int SaucerSpawnTickInterval = 400;

        private bool ShellFired = false;
        private Bullet Shell = new Bullet(20, 30);

        private const int InvaderRowQuantity = 5;
        private InvaderRow[] InvaderRows = new InvaderRow[InvaderRowQuantity];
        private InvaderRow Invaders = null;
        private int AttackingInvaderTickInterval = 6;

        private const int BunkerQuantity = 4;
        private Bunker[] Bunkers = new Bunker[BunkerQuantity];

		private string CurrentKeyDown = "";
		private string LastKeyDown = "";

		private MainMenu mainMenu;
		private MenuItem menuItemFile;
		private MenuItem menuItemRestart;
		private MenuItem menuItemExit;

		#region Windows Form Designer generated code

        [STAThread]

        private void InitializeComponent()
		{
            this.SuspendLayout();

            this.DoubleBuffered = true;
            this.ClientSize = new Size(1099, 627);
            this.AutoScaleBaseSize = new Size(5, 13);

            this.Name = "Display";
            this.Text = "Everything\'s out to get me!";

            this.HelpButton = true;
            this.KeyPreview = true;

            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form));
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));

            this.components = new Container();

            this.Timer = new Timer(this.components);
            this.Timer.Interval = 50;
            this.Timer.Tick += new EventHandler(this.Timer_Tick);

            this.menuItemRestart = new MenuItem();
            this.menuItemRestart.Index = 0;
            this.menuItemRestart.Text = "Restart";
            this.menuItemRestart.Click += new EventHandler(this.menuItemRestart_Click);

            this.menuItemExit = new MenuItem();
            this.menuItemExit.Index = 1;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new EventHandler(this.Menu_Exit);

            this.menuItemFile = new MenuItem();
            this.menuItemFile.Index = 0;
            this.menuItemFile.Text = "File";
            this.menuItemFile.MenuItems.AddRange(new MenuItem[] {this.menuItemRestart, this.menuItemExit});

            this.mainMenu = new MainMenu(this.components);
            this.mainMenu.MenuItems.AddRange(new MenuItem[] {this.menuItemFile});

            this.Menu = this.mainMenu;

            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = global::Vidiludo.Properties.Resources.background_1d;

            this.KeyDown += new KeyEventHandler(this.Form_KeyDown);
            this.KeyUp += new KeyEventHandler(this.Form_KeyUp);
            this.Paint += new PaintEventHandler(this.Form_Main_Paint);

            this.ResumeLayout(false);
        }

		#endregion

        static void Main()
		{
			Application.Run(new Form());
		}

        public Form()
        {
            InitializeComponent(); // Required for Windows Form Designer support

            // reduce flicker

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            Audio = new Audio();

            Score = new ScoreBoard(ClientRectangle.Right, 50);

            InitializeGameObjects(0);

            Timer.Start();
        }

        private void InitializeGameObjects(Int32 Level)
        {
            if (0 == Level)
            {
                Score.NewGame();
            }

            Turret = new Turret(this);

            InitializeBunkers();

            InitializeInvaderRows(Level);
        }

        private void InitializeBunkers()
        {
            for (int Bunker = 0; Bunker < BunkerQuantity; Bunker++)
            {
                Bunkers[Bunker] = new Bunker();
                Bunkers[Bunker].UpdateBounds();
                Bunkers[Bunker].Position.X = (Bunkers[Bunker].GetBounds().Width + 75) * Bunker + 25;
                Bunkers[Bunker].Position.Y = ClientRectangle.Bottom - (Bunkers[Bunker].GetBounds().Height + 75);
            }
        }

        void InitializeInvaderRows(int level)
        {
            InvaderRows[0] = new InvaderRow("invader_1a", "invader_1b", 2 + level);
            InvaderRows[1] = new InvaderRow("invader_2a", "invader_2b", 3 + level);
            InvaderRows[2] = new InvaderRow("invader_2a", "invader_2b", 4 + level);
            InvaderRows[3] = new InvaderRow("invader_3a", "invader_3b", 5 + level);
            InvaderRows[4] = new InvaderRow("invader_3a", "invader_3b", 6 + level);

            AttackingInvaderTickInterval = 6;
        }

        private void InitializeSaucer()
        {
            Saucer = new Saucer("saucer");
        }

        private void Form_Main_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			for (int i = 0; i < BunkerQuantity; i++)
			{
				Bunkers[i].Draw(g);
			}

			Turret.Draw(g);
			Score.Draw(g);

			if (ShellFired)
			{
			  Shell.Draw(g);
			}

			if (null != Saucer)
			{
			  Saucer.Draw(g);
			}

			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];
				Invaders.Draw(g);
			}
		}

        private Int64 FurthestLeft()
        {
            Int64 LeftmostX = 500000;

            for (int i = 0; i < InvaderRowQuantity; i++)
            {
                Invaders = InvaderRows[i];

                int FirstPosition = Invaders.GetFirstInvader().Position.X;

                if (LeftmostX > FirstPosition) LeftmostX = FirstPosition;
            }

            return LeftmostX;
        }

        private Int64 FurthestRight()
		{
            Int64 MaximumX = 0;

			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];

				int RightmostX = Invaders.GetLastInvader().Position.X;

				if (MaximumX < RightmostX) MaximumX = RightmostX;
			}

			return MaximumX;
		}

		private void MoveInvaders()
		{
			bool Dropping = false;

			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];
				Invaders.Move();
			}

            Audio.Play(Properties.Resources.Beat__3_, 1);

            if (ClientRectangle.Width - InvaderRows[4][0].Width < FurthestRight())
			{
				Invaders.GoingRight = false;
				DirectInvaders(false);
                Dropping = true;
            }

			if (InvaderRows[4][0].Width / 3 > FurthestLeft()) 
			{
				Invaders.GoingRight = true;
				DirectInvaders(true);
				Dropping = true;
			}

			if (Dropping)
			{
				for (int i = 0; i < InvaderRowQuantity; i++)
				{
					Invaders = InvaderRows[i];
					Invaders.MoveDown();
                    TestForLanding();
				}
			}
		}

		private int CountInvaders()
		{
		   int Count = 0;

		   for (int i = 0; i < InvaderRowQuantity; i++)
           {
              Invaders = InvaderRows[i];
			  Count += Invaders.NumberOfLiveInvaders();
		   }

			return Count;
		}

		private void MoveInvadersInPlace()
		{
			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];
				Invaders.MoveInPlace();
			}
		}

		private void DirectInvaders(bool GoRight)
		{
			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];
				Invaders.GoingRight = GoRight;
			}
		}

		public int ScoreValue(int Level)
		{
			switch (Level)
			{
				case 0:
					return 30;
				case 1:
                    return 20;
				case 2:
                    return 20;
				default:
                    return 10;
			}
		}

		void TestShellCollision()
		{
            if (!ShellFired) return;

            if (null != Saucer && Saucer.GetBounds().IntersectsWith(Shell.GetBounds()))
            {
                Saucer.Exploding = true;
                Score.Score(Saucer.ScoreValue);
                Audio.Play(Properties.Resources.Tone__19_, 10000);
            }

            for (int collisionIndex, i = InvaderRowQuantity - 1; 0 <= i; i--)
            {
                collisionIndex = InvaderRows[i].CollisionTest(Shell.GetBounds());

                if (0 <= collisionIndex)
                {
                    InvaderRows[i].Invaders[collisionIndex].Exploding = true;

                    Score.Score(ScoreValue(i));
                    Audio.Play(Properties.Resources.Rayshot__13_, 100);

                    ShellFired = false;
                    Shell.Reset();

                    break;
                }
            }
        }

		void TestForLanding()
		{
			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];

                if (Invaders.AlienHasLanded(Turret.Location.Bottom))
				{
                    Turret.Explode();
                    Audio.Play(Properties.Resources.Explosion__9_, 10000);
                    Score.GameOver();
				}
			}
		}

		void ResetAllBombCounters()
		{
			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];
				Invaders.ResetBombCounters();
			}
		}

		void TestBombCollision()
		{
            if (Turret.Exploding) return;

            if (Turret.Disintegrated)
			{
                Score.DepleteFleet();

                if (0 < Score.FleetSize)
				{
                    Turret.Stop();
                    ResetAllBombCounters();
				}
				else
				{
                    Score.GameOver();
                }

                return;
			}

			for (int i = 0; i < InvaderRowQuantity; i++)
			{
				Invaders = InvaderRows[i];

				for (int j = 0; j < Invaders.Invaders.Length; j++)
				{
			        for (int k = 0; k < BunkerQuantity; k++)
					{
						bool bulletHole = false;

						if (Bunkers[k].TestCollision(Invaders.Invaders[j].GetBombBounds(), true, out bulletHole))
						{
							Invaders.Invaders[j].ResetBombPosition();
							Invalidate(Bunkers[k].GetBounds());
						}

						if (Bunkers[k].TestCollision(Shell.GetBounds(), false, out bulletHole))
						{
							ShellFired = false;
							Invalidate(Bunkers[k].GetBounds());
							Shell.Reset();
						}
					}
				 
					if (Invaders.Invaders[j].IsBombColliding(Turret.Location))
                    {
                        if (1 < Score.FleetSize)
                        {
                            Audio.Play(Properties.Resources.Rayshot__6_, 100000);
                        }
                        else
                        {
                            Audio.Play(Properties.Resources.Explosion__1_, 100000);
                        }
                    }
				}
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
            TestBombCollision();
            
            HandleKeys();

			TickCount++;

            if (0 == Score.FleetSize)
			{
				if (0 == TickCount % 6) MoveInvadersInPlace();
				Invalidate();
				return;
			}

			if (0 == TickCount % SaucerSpawnTickInterval)
			{
				InitializeSaucer();
                Audio.Play(Properties.Resources.Rayshot__14_, 11500);
                Saucer = new Saucer("saucer"); ;
			}

			if (null != Saucer)
			{
				Saucer.Move();

				if (Saucer.GetBounds().Left > ClientRectangle.Right)
				{
				  Saucer = null;
				}
			}

            if (0 > Shell.Position.Y) ShellFired = false;

            TestShellCollision();

            if (0 == TickCount % AttackingInvaderTickInterval)
            {
                MoveInvaders();

                TestShellCollision();
            }

            switch (CountInvaders())
            {
                case 0: // level cleared
                    InitializeGameObjects(Level++);			
                    break;
                case 1: // last invader
                    AttackingInvaderTickInterval = 2;
                    break;
                case 3: // 2 or 3 invaders
                    AttackingInvaderTickInterval = 4;
                    break;
                case 5: // 4 or 5 invaders
                    AttackingInvaderTickInterval = 6;
                    break;
                case 10: // 6 thru 10 invaders
                    AttackingInvaderTickInterval = 8;
                    break;
                case 20: // 11 thru 20 invaders
                    AttackingInvaderTickInterval = 10;
                    break;
                default: // more than 20 invaders
                    break;
            }

            Invalidate();
		}

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            string result = e.KeyData.ToString();

            CurrentKeyDown = result;

            if (result == "Left" || result == "Right")
            {
                LastKeyDown = result;
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            string result = e.KeyData.ToString();

            if (result == "Left" || result == "Right")
            {
                LastKeyDown = "";
            }
        }

        private void Menu_Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemRestart_Click(object sender, EventArgs e)
		{
			this.InitializeGameObjects(0);
		}

        private void HandleKeys()
        {
            switch (CurrentKeyDown)
            {
                case "Escape":
                    Application.Exit();
                    break;

                case "Space":
                    if (null == Shell)
                    {
                        Shell = new Shell(Turret.Gunpoint, "shell");

                        Shell.Position = Turret.Gunpoint;
                        ShellFired = true;
                        Audio.Play(Properties.Resources.Gunshot__1_, 1000);
                    }

                    CurrentKeyDown = LastKeyDown;
                    break;

                case "Left":
                    if (!Turret.Exploding) Turret.Leftward();

                    Invalidate(Turret.Location);

                    if (!Timer.Enabled) Timer.Start();
                    break;

                case "Right":
                    if (!Turret.Exploding) Turret.Rightward();

                    Invalidate(Turret.Location);

                    if (!Timer.Enabled) Timer.Start();
                    break;

                default:
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && null != components) components.Dispose();

            base.Dispose(disposing);
        }
    }
}
