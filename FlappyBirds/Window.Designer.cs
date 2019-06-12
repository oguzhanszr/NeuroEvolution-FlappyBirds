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

        private Label HighScoreLabel { get; set; }
        private int HighScoreLabelData { get; set; }

        private Label GenerationLabel { get; set; }
        private int GenerationLabelData { get; set; }

        private Label PopulationLabel { get; set; }

        private int PopulationLabelData { get; set; }

        private Timer GeneratePipes { get; set; }

        private int population;
        private void InitializeComponents()
        {
            this.Paint += Window_Paint;
            this.Game.Interval = 10;
            this.Game.Tick += Game_Tick;

            this.GeneratePipes.Interval = 1300;
            this.GeneratePipes.Tick += GeneratePipes_Tick;

            this.population = 100;

            InitializeLabels();

            this.Controls.Add(CounterLabel);
            this.Controls.Add(GenerationLabel);
            this.Controls.Add(HighScoreLabel);
            this.Controls.Add(PopulationLabel);


            birds = new List<Bird>();
            for (int i = 0; i < population; i++)
                birds.Add(new Bird(50, 300, 35, 35));

            tempBirds = new List<Bird>();

            pipes = new List<Pipe>();

            Pipe initPipe = Pipe.RandomPipe(70, 120, 550, 750);
            initPipe.Up_X = 500;
            initPipe.Down_X = 500;
            initPipe.Update();
            pipes.Add(initPipe);
            this.KeyDown += On_KeyDown;
        }

        private void InitializeLabels()
        {
            this.CounterLabel = new Label();
            this.CounterLabel.Top = 5;
            this.CounterLabel.Left = this.Width - 90;
            this.CounterLabel.Font = new Font(FontFamily.GenericSerif, 21);
            this.CounterLabel.ForeColor = Color.Black;
            this.CounterLabel.SizeChanged += labelResize;
            this.CounterLabelData = 0;
            this.CounterLabel.BackColor = Color.Transparent;
            this.CounterLabel.AutoSize = true;
            this.CounterLabel.Text = CounterLabelData.ToString();

            this.HighScoreLabel = new Label();
            this.HighScoreLabel.Top = 35;
            this.HighScoreLabel.Left = this.Width - 135;
            this.HighScoreLabel.Font = new Font(FontFamily.GenericSerif, 21);
            this.HighScoreLabel.ForeColor = Color.Black;
            this.HighScoreLabel.SizeChanged += labelResize2;
            this.HighScoreLabelData = 0;
            this.HighScoreLabel.BackColor = Color.Transparent;
            this.HighScoreLabel.AutoSize = true;
            this.HighScoreLabel.Text = "High:" + HighScoreLabelData.ToString();

            this.GenerationLabel = new Label();
            this.GenerationLabel.Top = 70;
            this.GenerationLabel.Left = this.Width - 135;
            this.GenerationLabel.Font = new Font(FontFamily.GenericSerif, 21);
            this.GenerationLabel.ForeColor = Color.Black;
            this.GenerationLabel.SizeChanged += labelResize3;
            this.GenerationLabelData = 0;
            this.GenerationLabel.BackColor = Color.Transparent;
            this.GenerationLabel.AutoSize = true;
            this.GenerationLabel.Text = "Gen:" + GenerationLabelData.ToString();

            this.PopulationLabel = new Label();
            this.PopulationLabel.Top = 105;
            this.PopulationLabel.Left = this.Width - 135;
            this.PopulationLabel.Font = new Font(FontFamily.GenericSerif, 21);
            this.PopulationLabel.ForeColor = Color.Black;
            this.PopulationLabelData = 0;
            this.PopulationLabel.BackColor = Color.Transparent;
            this.PopulationLabel.AutoSize = true;
            this.PopulationLabel.Text = "Live:" + PopulationLabelData.ToString();
        }
        private void labelResize(object sender, EventArgs e)
        {
            CounterLabel.Left -= 3;
        }

        private void labelResize2(object sender, EventArgs e)
        {
            HighScoreLabel.Left -= 3;
        }

        private void labelResize3(object sender, EventArgs e)
        {
            GenerationLabel.Left -= 3;
        }

        private void GeneratePipes_Tick(object sender, EventArgs e)
        {
            pipes.Add(Pipe.RandomPipe(70, 120, 550, 750));
        }

        private void On_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (birds.Count >= 1) birds[0].Up();
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
            tempBirds.Clear();
            Game.Stop();
            GeneratePipes.Stop();
            pipes.Clear();

            Pipe initPipe = Pipe.RandomPipe(70, 120, 550, 750);
            initPipe.Up_X = 500;
            initPipe.Down_X = 500;
            initPipe.Update();
            pipes.Add(initPipe);

            CounterLabelData = 0;
            CounterLabel.Text = "0";
            birds.Clear();
            for (int i = 0; i < population; i++)
                birds.Add(new Bird(50, 300, 35, 35));

        }

        private void DetectOffScreenPipes()
        {
            var pipes = new List<Pipe>(this.pipes);
            foreach (var pipe in pipes)
            {
                if (pipe.Down_X + pipe.Down.Width < 0)
                {
                    pipe.OffScreen = true;
                    this.pipes.Remove(pipe);
                    CounterLabelData++;
                    CounterLabel.Text = CounterLabelData.ToString();
                }
            }
        }

        private void DetectCollusion()
        {
            var birds = new List<Bird>(this.birds);
            foreach (var bird in birds)
            {
                foreach (var pipe in pipes)
                {
                    if (bird.Body.IntersectsWith(pipe.Down) || bird.Body.IntersectsWith(pipe.Up))
                    {
                        bird.isDead = true;
                        tempBirds.Add(bird);
                        this.birds.Remove(bird);
                    }
                }
                if (bird.Body.Y <= 0 || bird.Body.Y + bird.Height >= this.Height - 30)
                {
                    tempBirds.Add(bird);
                    this.birds.Remove(bird);
                }
            }
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

        private Pipe FindClosestPipe()
        {
            int birdX = 50;
            Pipe closestPipe = null;
            int closest = int.MaxValue;

            for (int i = 0; i < pipes.Count; i++)
            {
                int d = (pipes[i].Down_X + pipes[i].Down.Width) - birdX;
                if (d < closest && d > 0)
                {
                    closestPipe = pipes[i];
                    closest = d;
                }
            }
            return closestPipe;
        }

        private void NextGeneration()
        {
            GenerationLabelData++;
            GenerationLabel.Text = "Gen:" + GenerationLabelData.ToString();
            this.birds = new List<Bird>();
            pipes.Clear();
            Pipe initPipe = Pipe.RandomPipe(70, 120, 550, 750);
            initPipe.Up_X = 500;
            initPipe.Down_X = 500;
            initPipe.Update();
            pipes.Add(initPipe);

            CalculateFitness();

            for (int i = 0; i < population; i++)
            {
                birds.Add(PickOne());
            }

        }

        private Bird PickOne()
        {
            Random rnd = new Random();
            int index = 0;
            double r = rnd.NextDouble();

            while (r > 0)
            {
                if (index == population)
                    break;
                r -= tempBirds[index].fitness;
                index++;
            }

            index--;

            Bird child = new Bird(tempBirds[index]);
            child.Mutate();
            return child;
        }

        private void Play()
        {
            closestPipe = FindClosestPipe();

            foreach (var bird in birds)
            {
                bird.Think(closestPipe.Up.Y + closestPipe.Up.Height, closestPipe.Down.Y, closestPipe.Up_X - (85));
                bird.score++;
            }
        }

        private void CalculateFitness()
        {
            int sum = 0;
            foreach (var bird in tempBirds)
            {
                sum += bird.score;
            }

            foreach (var bird in tempBirds)
            {
                bird.fitness = (double)bird.score / sum;
            }
        }
        private Pipe closestPipe;
        private void Game_Tick(object sender, EventArgs e)
        {
            if (birds.Count == 0)
            {
                NextGeneration();
                tempBirds.Clear();
                if (CounterLabelData > HighScoreLabelData)
                {
                    HighScoreLabelData = CounterLabelData;
                    HighScoreLabel.Text = "High:" + HighScoreLabelData.ToString();
                }
                CounterLabelData = 0;
                CounterLabel.Text = CounterLabelData.ToString();
            }

            PopulationLabelData = birds.Count;
            PopulationLabel.Text = "Live:" + PopulationLabelData.ToString();

            Invalidate();
            Gravity();
            DetectCollusion();
            MovePipes();
            DetectOffScreenPipes();
            Play();
        }

        private List<Pipe> pipes;

        private List<Bird> tempBirds;

        private List<Bird> birds;
        private void Window_Paint(object sender, PaintEventArgs e)
        {
            foreach (var bird in birds)
            {
                bird.Draw(e.Graphics);
            }

            foreach (var pipe in pipes)
            {
                pipe.Draw(e.Graphics);
            }
        }
    }
}