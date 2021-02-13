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
        private Timer animationTick = new Timer();
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

        private int direction, amount; //animation variables 
        private Graphics g;
        public slidingLines()
        {
            InitializeComponent();
        }

        private void slidingLines_Load(object sender, EventArgs e)
        {
            generateLines();
            animationTick.Tick += AnimationTick_Tick;
        }


        private void AnimationTick_Tick(object sender, EventArgs e) //animation tick basically slides lines up and down
        {
            // MessageBox.Show("merge merge");
            Matrix m = new Matrix();
            //   m.Translate(0, -amount);
            if (direction == 1)
            {
                m.Translate(0, -amount);
            }
            else
            {
                m.Translate(0, amount);
            }
            for (int i = 0; i < lines.Count; i++)
            {

                if (direction == 1 && lines[i].PathPoints.Select(p1 => p1.Y).Min() - amount > amount)//going up
                {
                    lines[i].Transform(m);
                    Invalidate();
                }
                else
                {
                    direction = 0;

                }
                if (direction == 0 && lines[i].PathPoints.Select(p1 => p1.Y).Max() + amount < this.Height)
                {
                    lines[i].Transform(m);
                    Invalidate();
                }
                else
                {
                    direction = 1;
                }
                /*
             
            if (direction == 1 && lines[i].PathPoints.Select(p1 => p1.Y).Max() - amount < this.Height) //going up
                {
                    lines[i].Transform(m);
                }
                else
                {
                    direction = 1 - direction;
                }
                if (direction == 0 && lines[i].PathPoints.Select(p1 => p1.Y).Min() + amount > this.Height) //going down
                {
                    MessageBox.Show("merge,merge");
                    lines[i].Transform(m);
                }
                else
                {
                    direction = 1 - direction;
                }
                */
            }
        }

        private void generateLines()
        {
            for (int i = 1; i <= _numbOfLines; i++)
            {
                template = new GraphicsPath();
                template.StartFigure();
                template.AddRectangle(new Rectangle(2 * i * distance, this.Height - 90, 7, 70));
                template.CloseAllFigures();
                template.StartFigure();
                template.AddArc(new RectangleF(2 * i * distance - 0.3f, this.Height - 95, 7.5f, 10), 0, -180);
                template.CloseAllFigures();
                template.StartFigure();
                template.AddArc(new RectangleF(2 * i * distance - 0.4f, this.Height - 25, 7.5f, 10), 0, 180);
                template.CloseAllFigures();
                lines.Add(template);
            }
        }

        public void changeLocation(int index, float offsetX, float offsetY)
        {
            try
            {
                Matrix m = new Matrix();
                m.Translate(offsetX, offsetY);
                lines[index].Transform(m);
                this.Invalidate();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.StackTrace);
                throw;
            }
        }

        public void Scale(int index, float offsetX, float offsetY)
        {
            Matrix m = new Matrix();
            m.Scale(offsetX, offsetY);
            lines[index].Transform(m);
            this.Invalidate();
        }


        private void slidingLines_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            foreach (GraphicsPath line in lines)
            {
                e.Graphics.FillPath(new SolidBrush(Color.FromArgb(236, 23, 247)), line);
            }
        }




        /// <summary>
        /// Slide transition of lines
        /// </summary>
        /// <param name="amount">offset added</param>
        /// <param name="direction">1-up , 0-down</param>
        /// <param name="Milliseconds">Time in milliseconds for transition</param>
        public void Slide(int Amount, int Direction, int Milliseconds)
        {
            animationTick.Interval = Milliseconds;
            amount = Amount;
            direction = Direction;
            animationTick.Start();
        }
    }
}

