using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace chess
{
    public class SwitchButton : Control
    {
        Rectangle rectangle = new Rectangle();



        public SwitchButton()
        {




        }

        public void drawcircle()
        {



        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            rectangle.Location = e.ClipRectangle.Location;
            rectangle.Size = e.ClipRectangle.Size;
            g.DrawRectangle(Pens.Black, rectangle);
        }





    }
}
