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
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Win32;

namespace chess
{
    public partial class Settings : Form
    {

        private Rectangle background = new Rectangle();
        private Color mouseover_Color;
        private GraphicsPath[] graphicsPaths = new GraphicsPath[10];
        private Graphics this_graphics;
        int index = 0;
        int ales = 0;
        private Color selectedCornerColor;
        public Settings()
        {
            InitializeComponent();
            //userControl13.Active = true;
            retrieveData();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this_graphics = this.CreateGraphics();
            background = new Rectangle(SB_Notif.Location.X, SB_Notif.Location.Y + 70, 80, 50);
            init_graphicspaths();
            mouseover_Color = Color.Empty;
            selectedCornerColor = Color.FromArgb(1, 11, 19);
        }

        private void userControl13_Load(object sender, EventArgs e)
        {

        }

        private void retrieveData()
        {
            if (Properties.Settings.Default.StartUp == true)
            {
                SB_Startup.Active = true;
            }

            if (Properties.Settings.Default.Notification == true)
            {
                SB_Notif.Active = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUp = SB_Startup.Active;
            Properties.Settings.Default.Notification = SB_Notif.Active;
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Settings_Paint(object sender, PaintEventArgs e)
        {
            int notif_pos = 0;
            /*
             Notification position:
             1- left top corner
             2-right top corner
             3-right down corner
             4-left down corner
            */
            e.Graphics.FillRectangle(Brushes.Gray, background);
            if (SB_Notif.Active == true)
            {
                draw_rectangles(Color.RoyalBlue, background, e.Graphics);
                notif_pos = Properties.Settings.Default.Notification_Place;
            }
            else
            {
                draw_rectangles(Color.SaddleBrown, background, e.Graphics);
            }
        }

        private void draw_rectangles(Color color, Rectangle rect, Graphics graphics)
        {
            /*
               //left top corner
               graphics.FillRectangle(new SolidBrush(color), rect.X, rect.Y, 20, 8);
               graphics.FillRectangle(new SolidBrush(color), rect.X, rect.Y, 8, 20);
               //right top corner     
               graphics.FillRectangle(new SolidBrush(color), rect.X + 60, rect.Y, 20, 8);
               graphics.FillRectangle(new SolidBrush(color), rect.X + 72, rect.Y, 8, 20);
               //left down corner     
               graphics.FillRectangle(new SolidBrush(color), rect.X, rect.Y + 42, 20, 8);
               graphics.FillRectangle(new SolidBrush(color), rect.X, rect.Y + 30, 8, 20);
               //right down corner    
               graphics.FillRectangle(new SolidBrush(color), rect.Right - 20, rect.Bottom - 8, 20, 8);
               graphics.FillRectangle(new SolidBrush(color), rect.Right - 8, rect.Bottom - 20, 8, 20);
            */
            for (int i = 1; i <= 4; i++)
            {
                if (SB_Notif.Active == true && i == Properties.Settings.Default.Notification_Place)
                {
                    graphics.FillPath(new SolidBrush(selectedCornerColor), graphicsPaths[i]);
                    continue;
                }
                if (mouseover_Color != Color.Empty && ales == i)
                {
                    graphics.FillPath(new SolidBrush(mouseover_Color), graphicsPaths[i]);
                }
                else
                {
                    graphics.FillPath(new SolidBrush(color), graphicsPaths[i]);
                }
            }
            // graphics.FillPath(new SolidBrush(color), graphicsPaths[2]);
            // graphics.FillPath(new SolidBrush(color), graphicsPaths[3]);
            //graphics.FillPath(new SolidBrush(color), graphicsPaths[4]);
        }

        private void init_graphicspaths()
        {
            graphicsPaths[1] = new GraphicsPath();
            graphicsPaths[1].AddRectangle(new Rectangle(background.X, background.Y, 20, 8));
            graphicsPaths[1].AddRectangle(new Rectangle(background.X, background.Y + 8, 8, 12));

            graphicsPaths[2] = new GraphicsPath();
            graphicsPaths[2].AddRectangle(new Rectangle(background.X + 60, background.Y, 12, 8));
            graphicsPaths[2].AddRectangle(new Rectangle(background.X + 72, background.Y, 8, 20));

            graphicsPaths[4] = new GraphicsPath();
            graphicsPaths[4].AddRectangle(new Rectangle(background.X, background.Y + 42, 20, 8));
            graphicsPaths[4].AddRectangle(new Rectangle(background.X, background.Y + 30, 8, 12));

            graphicsPaths[3] = new GraphicsPath();
            graphicsPaths[3].AddRectangle(new Rectangle(background.Right - 20, background.Bottom - 8, 12, 8));
            graphicsPaths[3].AddRectangle(new Rectangle(background.Right - 8, background.Bottom - 20, 8, 20));
        }

        private void SB_Notif_Click(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Settings_MouseMove(object sender, MouseEventArgs e)
        {
            int notif_pos = Properties.Settings.Default.Notification_Place;
            index = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (SB_Notif.Active == true && graphicsPaths[i].GetBounds().Contains(e.Location))
                {
                    ales = i;
                    index = 1;
                }
            }

            if (index == 1 && ales != Properties.Settings.Default.Notification_Place)
            {
                Cursor = Cursors.Hand;
                mouseover_Color = Color.Turquoise;
            }
            else
            {
                Cursor = Cursors.Arrow;
                ales = 0;
                mouseover_Color = Color.Empty;
            }
            this.Invalidate();
        }

        private void Settings_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 1; i <= 4; i++)
            {
                if (graphicsPaths[i].GetBounds().Contains(e.Location) && e.Button == MouseButtons.Left)
                {
                    Properties.Settings.Default.Notification_Place = i;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void SB_Startup_Click(object sender, EventArgs e)
        {
            string startupkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            string startupvalue = "chess";
            if (SB_Startup.Active == true)
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(startupkey, true);
                //  rk.DeleteSubKey(Application.ExecutablePath.ToString(), false);
                rk.DeleteValue(startupvalue, false);
            }
            else
            {
                string a = "\"";
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(startupkey, true);
                rk.SetValue(startupvalue, a + Application.ExecutablePath.ToString() + a + @" /background");
            }
        }

        private void SB_Notif_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
