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
        }

        private void slidingLines1_Load(object sender, EventArgs e)
        {

        }



        //private void generateLines()
        //{
        //    GraphicsPath line = new GraphicsPath();
        //    line.AddRectangle(new Rectangle(,,))

        //}

    }
}
