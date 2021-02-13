using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel.DataAnnotations;

namespace chess
{
    public partial class slidingLines : UserControl
    {
        private GraphicsPath template = new GraphicsPath();
        public List<GraphicsPath> lines = new List<GraphicsPath>();

        [Range(1, 10, ErrorMessage = "Invalid input!"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Appearance")]
        public int numbOfLines
        {
            get
            {
                return _numbOfLines;
            }
            set
            {
                _numbOfLines = value;
            }
        }


        private int _numbOfLines = 1;
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Description("Distance between 2 lines on the X-axis"), Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
            }
        }
        private int distance = 0;
        //[Category("Appearance"), Description("Set the color of the lines"), Browsable(true)]
        //public Brush _color;
        //private Brush color
        //{
        //    get
        //    {
        //        return _color;
        //    }
        //    set
        //    {
        //        _color = value;
        //    }
        //}

        public slidingLines()
        {
            InitializeComponent();
        }

        private void slidingLines_Load(object sender, EventArgs e)
        {
            generateLines();
        }

        //private void initializeLine()
        //{
        //    template.StartFigure();
        //    template.AddRectangle(new Rectangle(5, this.Height - 10, 10, 10));
        //    template.CloseFigure();
        //    lines.Add(template);
        //}

        private void generateLines()
        {
            for (int i = 1; i <= _numbOfLines; i++)
            {
                template = new GraphicsPath();
                template.StartFigure();
                template.AddRectangle(new Rectangle(2 * i * distance, this.Height - 70, 7, 70));
                template.CloseAllFigures();
                template.StartFigure();
                template.AddArc(new RectangleF(2 * i * distance - 0.3f, this.Height - 75, 7.5f, 10), 0, -180);
                template.CloseAllFigures();
                template.StartFigure();
                template.AddArc(new RectangleF(2 * i * distance - 0.4f, this.Height - 5, 7.5f, 10), 0, 180);
                template.CloseAllFigures();
                lines.Add(template);
            }
        }

        private void slidingLines_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            foreach (GraphicsPath line in lines)
            {
                e.Graphics.FillPath(new SolidBrush(Color.FromArgb(236, 23, 247)), line);
            }
            Matrix m = new Matrix();
            m.Translate(0, 10);
            lines[0].Transform(m);
        }
    }
}

