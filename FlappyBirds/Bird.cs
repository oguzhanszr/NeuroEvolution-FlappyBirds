using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        public Bird(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Body = new RectangleF(x, y, width, height);
            this.isDead = false;

            gravity = 0;
            velocity = 0.15f;
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
            g.FillEllipse(Brushes.LightSlateGray, Body);
        }
    }
}