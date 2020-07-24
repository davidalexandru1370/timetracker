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
    public class RenderContextMenuStrip : ToolStripProfessionalRenderer
    {
        public RenderContextMenuStrip() : base(new MyColors())
        {

        }
    }

    class MyColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(1, 11, 19); }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(1, 11, 19); }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(1, 11, 19); }
        }
        public override Color MenuItemBorder
        {
            get
            {
                return Color.FromArgb(1, 11, 19);
            }
        }
        


    }





}
