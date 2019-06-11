using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NNSharp.lib;

namespace FlappyBirds
{
    public class Bird
    {
        public RectangleF Body { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool isDead { get; set; }

        public float gravity { get; set; }

        public float velocity { get; set; }

        public int score { get; set; }

        public double fitness { get; set; }
        public NeuralNetwork Brain { get; set; }

        public Bird(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Body = new RectangleF(x, y, width, height);
            this.isDead = false;
            this.Brain = new NeuralNetwork(4, 8, 1);
            this.fitness = 0;
            this.score = 0;

            gravity = 0;
            velocity = 0.15f;
        }

        public Bird(Bird bird)
        {
            this.X = bird.X;
            this.Y = bird.Y;
            this.Width = bird.Width;
            this.Height = bird.Height;
            this.Body = new RectangleF(bird.X, 300, bird.Width, bird.Height);
            this.isDead = false;
            this.Brain = new NeuralNetwork(bird.Brain);
            this.score = 0;
            this.fitness = 0;
            gravity = 0;
            velocity = 0.15f;
            // this.Mutate();
        }

        public void Mutate()
        {
            this.Brain.map(this.Brain.WeightsOfInputHidden, mutateFunction);
            this.Brain.map(this.Brain.WeightsOfHiddenOutput, mutateFunction);
            this.Brain.map(this.Brain.WeighstOfBiasHidden, mutateFunction);
            this.Brain.map(this.Brain.WeightsOfBiasOutput, mutateFunction);
        }
        public double mutateFunction(double x)
        {
            double rate = 0.1;
            Random rnd = new Random(DateTime.Now.Millisecond);
            GaussianRandom r = new GaussianRandom();
            double random = rnd.NextDouble();
            // double random = r.NextGaussian(0, 0.1);


            if (random < rate)
            {
                double offset = r.NextGaussian(0, 0.1) * 0.5;
                return x + offset;
            }
            else
            {
                return x;
            }
        }

        public void Think(double pipeDownY, double pipeUpY, double distance)
        {
            Matrix predict = Brain.feedForward(new Matrix(4, 1, this.Y, pipeDownY, pipeUpY, distance));

            if (predict.Array[0, 0] > 0.5)
            {
                Up();
            }
        }

        public void Up()
        {
            this.gravity = -4;
            Update();
        }

        public void Down()
        {
            this.gravity += 2 * this.velocity;
            this.gravity = Math.Min(5, this.gravity);
            this.Y += 2 * this.gravity;
            Update();
        }

        public void Update()
        {
            this.Body = new RectangleF(this.X, this.Y, this.Width, this.Height);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Orange, Body);
        }
    }
}