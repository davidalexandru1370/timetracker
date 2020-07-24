using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace chess
{
    class ShiningText
    {
        private Graphics graphics;
        private string text;
        private int posX, posY;
        private int height, width;


        ShiningText(string txt, int x, int y, int _height, int _width)
        {
          //  graphics = g;
            text = txt;
            posX = x;
            posY = y;
            height = _height;
            width = _width;
        }







        //private void draw()
        //{
        //    Bitmap bmp = new Bitmap(height, width);
        //    GraphicsPath graphicsPath = new GraphicsPath();
        //    graphicsPath.AddString(text, new FontFamily("Arial"), (int)FontStyle.Regular, 25, new Point(posX, posY), StringFormat.GenericTypographic);
        //    graphics = Graphics.FromImage(bmp);
        //    Matrix mx = new Matrix(1.0f / 5, 0, 0, 1.0f / 5, -(1.0f / 5), -(1.0f / 5));
        //    graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //    graphics.Transform = mx;
        //    Pen p = new Pen(Color.)




        //}




    }
}
