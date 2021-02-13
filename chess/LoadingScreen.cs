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

        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(17, 18, 20);
            slidingLines1.changeLocation(0, 0, -10);
            slidingLines1.changeLocation(1, 0, -20);
            slidingLines1.changeLocation(2, 0, -40);
            slidingLines1.changeLocation(3, 0, -30);
            slidingLines1.Slide(6,1,70);
        }

        private void slidingLines1_Load(object sender, EventArgs e)
        {
            
        }

        private void linesAnimation_Tick(object sender, EventArgs e)
        {
            
        }

        private void slidingLines1_Paint(object sender, PaintEventArgs e)
        {

        }

       



        //private void generateLines()
        //{
        //    GraphicsPath line = new GraphicsPath();
        //    line.AddRectangle(new Rectangle(,,))

        //}

    }
}
