using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FlappyBirds
{
    public class Pipe
    {
        public Rectangle Up { get; set; }

        public int Up_X { get; set; }
        public int Up_Y { get; set; }

        public Rectangle Down { get; set; }

        public int Down_X { get; set; }
        public int Down_Y { get; set; }

        public int Interval { get; set; }

        public Pipe(Rectangle up, Rectangle down, int interval)
        {
            this.Up = new Rectangle(up.X, up.Y, up.Width, up.Height);
            this.Down = new Rectangle(down.X, down.Y + interval + up.Height, down.Width, down.Height);
            this.Up_X = up.X;
            this.Up_Y = up.Y;
            this.Down_X = down.X;
            this.Down_Y = down.Y;
            this.Interval = interval;
        }

        public void Left()
        {
            this.Up_X -= 2*3;
            this.Down_X -= 2*3;
            Update();
        }
        

        public void Update()
        {
            this.Up = new Rectangle(Up_X, Up_Y, Up.Width, Up.Height);
            this.Down = new Rectangle(Down_X, Down_Y + this.Interval + Up.Height, Down.Width, Down.Height);
        }

        public static Pipe RandomPipe(int width, int interval, int screenHeight, int screenWidth)
        {
            Random rnd = new Random();
            int height1 = rnd.Next(50, screenHeight - 150);
            int height2 = screenHeight - height1;
            Pipe pipe = new Pipe(new Rectangle(screenWidth + 50, 0, width, height1),
             new Rectangle(screenWidth + 50, 0, width, height2), interval);
            return pipe;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangles(Brushes.LightGreen, new Rectangle[] { Up, Down });
        }
    }
}