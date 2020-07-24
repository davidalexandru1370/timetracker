using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;

namespace chess
{
    public partial class UserControl1 : UserControl
    {
        private SwitchButton button = new SwitchButton();
        private GraphicsPath circle = new GraphicsPath();
        private GraphicsPath old = new GraphicsPath();
        private Graphics g;
        private Brush color = Brushes.Tomato;
        private bool IsOn = false;

        public bool Active
        {
            get
            {
                return IsOn;
            }
            set
            {
                IsOn = value;
            }
        }

        private GraphicsPath graphicsPath = new GraphicsPath();//rounded rectangle
        private float x = 3f;
        private Timer transition = new Timer();
        private int stare = 0;
        private string[] text = { "OFF", "ON" };
        private int first = 1;

        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.Click += UserControl1_Click;
            transition.Interval = 35;
            transition.Tick += transition_Tick;

            if (IsOn == true)
            {
                x += this.ClientRectangle.Right / 2 - 2;
                color = Brushes.ForestGreen;
                stare = 1;
                Invalidate();
            }

            //transition.Tick += new EventHandler((transition, EventArgs) => Transition_Tick(sender, e, stare));
        }



        private void transition_Tick(object sender, EventArgs e) // if stare == 0 turn it on otherwise turn it off
        {
            if (stare == 0)
            {
                if (x + 1.25f < this.ClientRectangle.Right / 2)
                {
                    x += 5f;
                    this.Invalidate();
                }
                else
                {
                    transition.Stop();
                    stare = 1 - stare;
                }
            }

            else
            {
                if (x > 4)
                {
                    x -= 5f;
                    this.Invalidate();
                }
                else
                {
                    transition.Stop();
                    stare = 1 - stare;

                }
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, 64, 33, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            generate(color);
            generate_circle();
            draw_text();
        }


        private void draw_text()
        {
            if (IsOn == false)
            {
                g.DrawString(text[0], new Font("Arial", 7.25f), color, new PointF(this.ClientRectangle.Location.X + x, this.ClientRectangle.Location.Y + 10));
            }
            else
            {
                g.DrawString(text[1], new Font("Arial", 7.25f), color, new PointF(this.ClientRectangle.Location.X + x + 4.5f, this.ClientRectangle.Location.Y + 10));
            }
        }

        private void generate_circle()
        {
            circle = new GraphicsPath();
            circle.StartFigure();
            circle.AddEllipse(ClientRectangle.Location.X + x, ClientRectangle.Location.Y + 2, ClientRectangle.Height - 5, ClientRectangle.Height - 5);
            circle.CloseFigure();
            g.FillPath(Brushes.Gray, circle);
        }

        private void generate(Brush br)
        {
            //  Invalidate();
            graphicsPath = new GraphicsPath();
            Rectangle rectangle = this.ClientRectangle;
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rectangle.Location.X, rectangle.Location.Y, 22, rectangle.Height - 1, 90, 180);
            graphicsPath.CloseAllFigures();
            graphicsPath.AddArc(rectangle.Right - 23, rectangle.Location.Y, 22, rectangle.Height - 1, 90, -180);
            graphicsPath.CloseAllFigures();
            graphicsPath.AddRectangle(new Rectangle(new Point(rectangle.Location.X + 11, rectangle.Location.Y), new Size(rectangle.Size.Width - 23, rectangle.Size.Height)));
            g.FillPath(br, graphicsPath);
        }

        private void UserControl1_Click(object sender, EventArgs e)
        {
            if (transition.Enabled == true)
            {
                return;
            }
            this.Invalidate();


            if (IsOn == false)
            {
                IsOn = true;
                color = Brushes.ForestGreen;
                if (transition.Enabled == true)
                {
                    transition.Stop();
                    stare = 1 - stare;
                }
                else
                {
                    transition.Start();
                }

            }

            else
            {
                IsOn = false;
                color = Brushes.Tomato;

                if (transition.Enabled == true)
                {
                    transition.Stop();
                    stare = 1 - stare;
                }
                else
                {
                    transition.Start();
                }

            }



        }




    }
}
