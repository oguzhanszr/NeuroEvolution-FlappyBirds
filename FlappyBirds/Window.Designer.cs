using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBirds
{
    partial class Window
    {
        private Timer Game { get; set; }

        private Label CounterLabel { get; set; }
        private int CounterLabelData { get; set; }

        private Timer GeneratePipes { get; set; }

        private Timer deneme { get; set; }

        private int population;
        private void InitializeComponents()
        {
            this.Paint += Window_Paint;
            this.Game.Interval = 10;
            this.Game.Tick += Game_Tick;

            this.GeneratePipes.Interval = 1300;
            this.GeneratePipes.Tick += GeneratePipes_Tick;

            this.CounterLabel = new Label();
            this.CounterLabel.Top = 10;
            this.CounterLabel.Left = this.Width - 90;
            this.CounterLabel.Font = new Font(FontFamily.GenericSerif, 23);
            this.CounterLabel.ForeColor = Color.Black;
            this.CounterLabelData = 0;
            this.CounterLabel.AutoSize = true;
            this.CounterLabel.Text = CounterLabelData.ToString();
            this.Controls.Add(CounterLabel);

            // this.deneme = new Timer();
            // this.deneme.Interval = 1000 * 60;
            // this.deneme.Tick += deneme_Tick;
            // this.deneme.Start();

            this.comparer = new CompareBird();

            population = 100;

            birds = new List<Bird>();
            for (int i = 0; i < population; i++)
                birds.Add(new Bird(50, 300, 35, 35));
            tempBirds = new List<Bird>(birds);

            pipes = new List<Pipe>();
            //initpipe
            // pipes.Add(new Pipe(new Rectangle(400, 0, 70, 300), new Rectangle(400, 0, 70, 300), 120));
            Pipe initPipe = Pipe.RandomPipe(70, 120, 550, 750);
                initPipe.Up_X = 500;
                initPipe.Down_X = 500;
                initPipe.Update();
                pipes.Add(initPipe);
            this.KeyDown += On_KeyDown;
        }

        private bool start = true;
        private void deneme_Tick(object sender, EventArgs e)
        {
            start = true;
        }

        private void GeneratePipes_Tick(object sender, EventArgs e)
        {
            pipes.Add(Pipe.RandomPipe(70, 120, 550, 750));
        }

        private void On_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (birds.Count == 1) birds[0].Up();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Game.Start();
                GeneratePipes.Start();
            }
            else if (e.KeyCode == Keys.R)
            {
                GameReset();
            }
        }

        private void GameReset()
        {
            Game.Stop();
            GeneratePipes.Stop();
            pipes.Clear();
            //initpipe
            // pipes.Add(new Pipe(new Rectangle(400, 0, 70, 300), new Rectangle(400, 0, 70, 300), 120));
            Pipe initPipe = Pipe.RandomPipe(70, 120, 550, 750);
                initPipe.Up_X = 500;
                initPipe.Down_X = 500;
                initPipe.Update();
                pipes.Add(initPipe);
            // birds.Add(new Bird(50, 300, 35, 35));
            CounterLabelData = 0;
            CounterLabel.Text = "0";
            birds.Clear();
            for (int i = 0; i < population; i++)
                birds.Add(new Bird(50, 300, 35, 35));
        }

        private CompareBird comparer;

        private void DetectOffScreenPipes()
        {
            foreach (var pipe in pipes)
            {
                if (pipe.Down_X + pipe.Down.Width < 0)
                {
                    pipe.OffScreen = true;
                    CounterLabelData++;
                    CounterLabel.Text = CounterLabelData.ToString();
                    return;
                }
            }
        }

        private void UpdatePipes()
        {
            pipes.Remove(pipes.Find(x => x.OffScreen == true));
        }

        private void DetectCollusion()
        {
            foreach (var pipe in pipes)
            {
                foreach (var bird in birds)
                {
                    if (bird.Body.IntersectsWith(pipe.Down))
                    {
                        // bird.X -= 3;
                        // bird.Y -= 3;
                        // bird.Update();
                        bird.isDead = true;
                    }
                    else if (bird.Body.IntersectsWith(pipe.Up))
                    {
                        // bird.X -= 3;
                        // bird.Y += 3;
                        // bird.Update();
                        bird.isDead = true;
                    }
                }
            }
        }



        private void UpdateBirds()
        {
            birds.Remove(birds.Find(x => x.isDead == true));
        }

        private void Gravity()
        {
            foreach (var bird in birds)
            {
                bird.Down();
            }
        }

        private void MovePipes()
        {
            foreach (var pipe in pipes)
            {
                pipe.Left();
            }
        }

        private void DetectOffScreenBirds()
        {
            foreach (var bird in birds)
            {
                if (bird.Body.Y <= 0)
                {
                    bird.isDead = true;
                }
                if (bird.Body.Y >= this.Height)
                {
                    bird.isDead = true;
                }
            }
        }

        private Pipe FindClosestPipe()
        {
            int birdX = 50;
            Pipe closestPipe = null;
            int closest = int.MaxValue;

            for(int i = 0; i < pipes.Count; i++)
            {
                int d = (pipes[i].Down_X + pipes[i].Down.Width) - birdX;
                if(d < closest && d > 0)
                {
                    closestPipe = pipes[i];
                    closest = d;
                }
            }
            closestPipe.Color = Brushes.Red;
            return closestPipe;
        }

        private void CalculateFitness()
        {
            int sum = 0;
            foreach (var bird in birds)
            {
                sum += bird.score;
            }

            foreach (var bird in birds)
            {
                bird.fitness = bird.score / sum;
            }
        }
        private Pipe closestPipe;
        private void Game_Tick(object sender, EventArgs e)
        {
            if (birds.Count == 0)
            {
                //  Game.Stop();
                //  GeneratePipes.Stop();

                CalculateFitness();
                pipes.Clear();
                //init pipe
                // pipes.Add(new Pipe(new Rectangle(400, 0, 70, 300), new Rectangle(400, 0, 70, 300), 120));
                Pipe initPipe = Pipe.RandomPipe(70, 120, 550, 750);
                initPipe.Up_X = 500;
                initPipe.Down_X = 500;
                initPipe.Update();
                pipes.Add(initPipe);
                // // birds.Add(new Bird(50, 300, 35, 35));
                CounterLabelData = 0;

                tempBirds.Sort(comparer.Compare);

                for (int i = 0; i < population; i++)
                {
                    if (i <= population / 2 - 1)
                        tempBirds[i].Mutate();

                    this.birds.Add(new Bird(tempBirds[i]));
                }
                // CounterLabel.Text = this.birds.Count.ToString();
                tempBirds = new List<Bird>(birds);

                // int a = tempBirds.Count;
                // Game.Start();
                // GeneratePipes.Start();
            }

            if (start == true)
                Invalidate();

            Gravity();

            closestPipe = FindClosestPipe();


            foreach (var bird in birds)
            {
                bird.Think(closestPipe.Up.Y + closestPipe.Up.Height, closestPipe.Down.Y, closestPipe.Up_X - (85));
                bird.score++;
                // bird.Mutate();
            }



            DetectOffScreenPipes();
            UpdatePipes();

            DetectOffScreenBirds();
            UpdateBirds();

            DetectCollusion();
            UpdateBirds();

            MovePipes();
        }

        private List<Pipe> pipes;

        private List<Bird> tempBirds;

        private List<Bird> ChosenBirds;
        private List<Bird> birds;
        private void Window_Paint(object sender, PaintEventArgs e)
        {
            foreach (var bird in birds)
            {
                bird.Draw(e.Graphics);

            }

            CounterLabel.Text = birds.Count.ToString();
            foreach (var pipe in pipes)
            {
                pipe.Draw(e.Graphics);
            }
        }
    }
}