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
using System.Data.SqlClient;

namespace chess
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }
        private byte direction = 0;
        private short index = 83;
        private Timer BackgroundWorker = new Timer();
        private SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\programepath.mdf;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True;");
        Form1 Main;
        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            Main = new Form1();
            BackgroundWorker.Interval = 2000;
            BackgroundWorker.Tick += BackgroundWorker_Tick;
            this.BackColor = Color.FromArgb(17, 18, 20);
            label1.ForeColor = Color.FromArgb(88, 88, 88);
            slidingLines1.Scale(0, 1, 0.7f);
            slidingLines1.changeLocation(0, 0, 10);
            slidingLines1.changeLocation(1, 0, -20);
            slidingLines1.changeLocation(2, 0, -40);
            slidingLines1.changeLocation(3, 0, -30);
            slidingLines1.Slide(5, 1, 50);
            TextTimer.Interval = 80;
            TextTimer.Start();
            BackgroundWorker.Start();
        }

        private void BackgroundWorker_Tick(object sender, EventArgs e)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.OpenAsync();
                }
                else
                {
                    BackgroundWorker.Stop();
                    this.Hide();
                    Main.ShowDialog();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }



        private void slidingLines1_Load(object sender, EventArgs e)
        {

        }

        private void slidingLines1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TextTimer_Tick(object sender, EventArgs e)
        {
            if (direction == 0)
            {
                index -= 2;
                label1.ForeColor = Color.FromArgb(index, index, index);
                if (index <= 50)
                {
                    direction = 1;
                }
            }
            else
            {
                index += 2;
                label1.ForeColor = Color.FromArgb(index, index, index);
                if (index >= 88)
                {
                    direction = 0;
                }
            }
        }

        private void slidingLines1_Load_1(object sender, EventArgs e)
        {

        }
    }
}

