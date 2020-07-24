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
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Threading;
using wf = System.Windows.Forms;

namespace chess
{
    public partial class notifcation : Form
    {
        private int direction = 0; // 0 = up   1 = down
        private wf.Timer floating = new wf.Timer();
        private float stage = 0f;
        Screen screen;
        int val;
        private string _Path;
        //{
        //    get => _Path;
        //    set => _Path = value;
        //}

        private string _Name;
        //{
        //    //get => _Name;
        //    //set => _Name = value;
        //}

        private int Corner;
        //{
        //    get => Corner;
        //    set => Corner = value;
        //}

        private enum directie
        {
            Up,
            Down
        };

        public notifcation(string path, string name, int corner)
        {
            InitializeComponent();
            _Name = name;
            _Path = path;
            Corner = corner;
        }

        private void notifcation_Load(object sender, EventArgs e)
        {
            this.CreateGraphics().SmoothingMode = SmoothingMode.HighQuality;
            this.FormBorderStyle = FormBorderStyle.None;
            floating.Interval = 25;
            screen = Screen.FromControl(this);
            label2.Text = "Is now running:";
            label3.Text = _Name;
            pictureBox1.Image = Icon.ExtractAssociatedIcon(_Path).ToBitmap();
            floating.Tick += Floating_Tick;
        }


        private void Floating_Tick(object sender, EventArgs e) // asta urca si coboara
        {
            if (direction == 1)
            {
                this.Location = new Point(this.Location.X, this.Location.Y + val);
                stage += 10;
                if (stage == 80)
                {
                    direction = 1 - direction;
                    Thread.Sleep(1500);
                }
            }
            else
            {
                this.Location = new Point(this.Location.X, this.Location.Y - val);
                stage -= 10;
                if (stage == 0)
                {
                    direction = 1 - direction;
                    floating.Stop();
                    this.Close();
                }
            }
        }


        /*
       
          ____              ____     __             __
         /| \ \            / /\ \    \ \           / /   
        | |  \ \          / /  \ \    \ \         / /
        | |   \ \        / /____\ \    \ \       / /
        | |    \ \      / /      \ \    \ \     / /
        | |     \ \    / /        \ \    \ \   / /
        \_|_____/_/   /_/          \_\    \ \_/ /
         \_______/                         \___/
             */

        protected override void OnLoad(EventArgs e)
        {
            this.TopMost = true;
            this.TopLevel = true;
            base.OnLoad(e);
            switch (Corner)
            {
                case 1:
                    leftTopCorner();
                    break;
                case 2:
                    rightTopCorner();
                    break;
                case 3:
                    downRightCorner();
                    break;
                case 4:
                    downLeftCorner();
                    break;
                default:
                    this.Close();
                    break;
            }

        }

        private void notifcation_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void leftTopCorner()
        {
            this.Location = new Point(0, -80);
            direction = 1;
            val = 10;
            floating.Start();
        }

        private void rightTopCorner()
        {
            this.Location = new Point(screen.Bounds.Right - this.Size.Width, -80);
            direction = 1;
            val = 10;
            floating.Start();
        }

        private void downRightCorner()
        {
            this.Location = new Point(screen.Bounds.Right - this.Size.Width, screen.Bounds.Bottom + 15);
            direction = 1;
            val = -10;
            floating.Start();
        }

        private void downLeftCorner()
        {
            this.Location = new Point(0, screen.Bounds.Bottom + 20);
            direction = 1;
            val = -10;
            floating.Start();
        }


    }
}
