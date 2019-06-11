using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;

namespace FlappyBirds
{
    public partial class Window : Form
    {

        public Window(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Game = new Timer();
            this.GeneratePipes = new Timer();
            this.DoubleBuffered = true;
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.Text = "NeuroEvolution Flappy Birds";

            

            InitializeComponents();
        }

    }
}