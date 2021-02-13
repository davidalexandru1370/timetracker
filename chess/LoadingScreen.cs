using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace chess
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }
        private byte direction = 0;
        private short index = 83;

        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(17, 18, 20);
            label1.ForeColor = Color.FromArgb(88, 88, 88);
            slidingLines1.Scale(0, 1, 0.7f);
            slidingLines1.changeLocation(0, 0, 10);
            slidingLines1.changeLocation(1, 0, -20);
            slidingLines1.changeLocation(2, 0, -40);
            slidingLines1.changeLocation(3, 0, -30);
            slidingLines1.Slide(4, 1, 70);
            TextTimer.Interval = 100;
            TextTimer.Start();
        }

        private void slidingLines1_Load(object sender, EventArgs e)
        {

        }

        private void slidingLines1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TextTimer_Tick(object sender, EventArgs e)
        {
            if (direction == 0)
            {
                index -= 2;
                label1.ForeColor = Color.FromArgb(index, index, index);
                if (index <= 50)
                {
                    direction = 1;
                }
            }
            else
            {
                index+=2;
                label1.ForeColor = Color.FromArgb(index, index, index);
                if (index >= 88)
                {
                    direction = 0;
                }
            }
        }

        //private void generateLines()
        //{
        //    GraphicsPath line = new GraphicsPath();
        //    line.AddRectangle(new Rectangle(,,))

        //}

    }
}
