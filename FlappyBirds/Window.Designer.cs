
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBirds
{
    partial class Window
    {
        private Timer Game { get; set; }

        private Timer GeneratePipes { get; set; }
        private void InitializeComponents()
        {
            this.Paint += Window_Paint;
            this.Game.Interval = 1;
            this.Game.Tick += Game_Tick;

            this.GeneratePipes.Interval = 1300;
            this.GeneratePipes.Tick += GeneratePipes_Tick;

            bird = new Bird(50, 300, 35, 35);

            pipes = new List<Pipe>();
            this.KeyDown += On_KeyDown;
        }

        private void GeneratePipes_Tick(object sender, EventArgs e)
        {
            pipes.Add(Pipe.RandomPipe(100, 120, 550, 750));
        }

        private void On_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                bird.Up();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Game.Start();
                GeneratePipes.Start();
            }
            else if(e.KeyCode == Keys.R)
            {
                Game.Stop();
                GeneratePipes.Stop();
                pipes.Clear();
                bird = new Bird(50, 300, 35, 35);
            }
        }

        private void Game_Tick(object sender, EventArgs e)
        {
            Invalidate();
            bird.Down(); //gravity


            foreach (var pipe in pipes)
            {
                if (bird.Body.IntersectsWith(pipe.Down))
                {
                    bird.X -= 3;
                    bird.Y -= 3;

                    bird.Update();
                }
                else if(bird.Body.IntersectsWith(pipe.Up))
                {
                    bird.X -= 3;
                    bird.Y += 3;

                    bird.Update();
                }
            }


            foreach (var pipe in pipes)
            {
                pipe.Left();
            }
        }

        private List<Pipe> pipes;
        private Bird bird;
        private void Window_Paint(object sender, PaintEventArgs e)
        {
            bird.Draw(e.Graphics);

            foreach (var pipe in pipes)
            {
                pipe.Draw(e.Graphics);
            }
        }
    }
}