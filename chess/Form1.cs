using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.Common;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using th = System.Threading;

namespace chess
{
    public partial class Form1 : Form
    {
        int index;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        string cale;
        OpenFileDialog ofd = new OpenFileDialog();
        ListViewItem item = new ListViewItem();
        HashSet<string> lista = new HashSet<string>();
        List<string> processList = new List<string>();
        Dictionary<string, string> NotifcationList = new Dictionary<string, string>();
        List<int> pos = new List<int>();
        Timer time = new Timer();
        Timer loop = new Timer();
        bool ison;
        Random rand = new Random();
        int m, h;
        int p = 0;
        string safefilename;
        string processactual;
        string caleactuala;
        DateTime saptasta = DateTime.Now;
        DateTime today;
        DateTime anothertoday;
        DateTime thisweek = DateTime.Now.Date;
        DateTime thismonth = DateTime.Now;
        Image imagine_setari;
        int idweek = 0;
        int ore_zilnice = 0;
        int minute_zilnice = 0;
        Point picturebox_center;
        //   readonly string[] zile = { "Duminica", "Luni", "Marti", "Miercuri", "Joi", "Vineri", "Sambata" };
        Settings setari = new Settings();
        bool off = true;
        Timer rotateimage = new Timer();
        float degree = 0;
        int status = 0;
        Pen panel2_color = Pens.Gray;
        GraphicsPath textbox_button_X = new GraphicsPath();
        Rectangle rect4 = new Rectangle();

        public Form1()
        {
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\programepath.mdf;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True;");
            InitializeComponent();
            //   stergere();
            label5.Text = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadingScreen form50 = new LoadingScreen();
            form50.Show();
            fillista();
            fillProcessList();
            contextMenuStrip1.Renderer = new RenderContextMenuStrip();
            contextMenuStrip2.Renderer = new RenderContextMenuStrip();
            rect4 = new Rectangle(panel4.Right - 25, panel4.Location.Y, 14, 20);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            label7.ForeColor = Color.WhiteSmoke;
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            fill_background_tabcontrol();
            picturebox_center = new Point(pictureBox2.Right - 300, pictureBox2.Bottom - 255);
            checkday("today");
            fillweektable();
            imagine_setari = Image.FromFile(Application.StartupPath + @"\roata.png");
            Graphics g = listView1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            listView1.OwnerDraw = true;
            button2.Enabled = false;
            listView1.FullRowSelect = true;
            listView1.View = View.Details;
            timer1.Interval = 1000;
            listView1.Columns.Add("Nume aplicatie", -2);
            fill();
            listView1.ForeColor = Color.DimGray;
            timer2.Interval = 10;
            loop.Tick += Loop_Tick;
            loop.Interval = 60000;
            loop.Start();
            rotateimage.Tick += Rotateimage_Tick;
            rotateimage.Interval = 50;
            //  timer2.Enabled = true;
            //    timer2.Start();
            timer1.Start();
            notifyIcon1.Visible = true;
            fillContextmenustrip2();
            label6.Text = ore_zilnice.ToString() + " hours and " + minute_zilnice + " minutes";
            button2.Visible = false;
        }

        private void fill_background_tabcontrol()
        {
            Rectangle tabcontrol_background = tabControl1.Bounds;
            using (Graphics graphics = tabControl1.CreateGraphics())
            {
                graphics.FillRectangle(Brushes.Red/*new SolidBrush( Color.FromArgb(1, 11, 19))*/, tabcontrol_background);
            }
        }

        private void close_setari(object sender, FormClosedEventArgs e)
        {
            off = true;
        }

        private void Loop_Tick(object sender, EventArgs e)
        {
            minute_zilnice++;
            label6.Text = ore_zilnice.ToString() + " ore " + minute_zilnice.ToString() + " minute";
            if (minute_zilnice > 60)
            {
                ore_zilnice++;
                minute_zilnice = 0;
                //   chart1.Series[0].Points.AddXY(ore_zilnice, ore_zilnice);
            }
        }



        private void fillProcessList()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            processList = new List<string>();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("Select ProcessName from Folder where DisplayName like '%" + textBox1.Text + "%'", con);
            sda.Fill(dt);
            int i = 0;
            while (i < dt.Rows.Count)
            {
                processList.Add(dt.Rows[i][0].ToString());
                i++;
            }
            con.Close();
        }

        private void checkConnection()
        {
            switch (con.State)
            {
                case ConnectionState.Closed:
                    con.Open();
                    break;
                case ConnectionState.Open:
                    con.Close();
                    break;
                default:
                    break;
            }
        }

        private void fillista()
        {
            lista = new HashSet<string>();
            checkConnection();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("Select DisplayName from Folder where DisplayName like '%" + textBox1.Text + "%' ", con);
            sda.Fill(dt);
            int i = 0;
            while (i < dt.Rows.Count)
            {
                lista.Add(dt.Rows[i][0].ToString());
                i++;
            }
            checkConnection();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //add application
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        public void adaugare(string cale, string name, string displayname)
        {
            if (!string.IsNullOrWhiteSpace(cale))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                try
                {
                    cmd = new SqlCommand("if not exists(Select 1 from Folder where Path ='" + cale + "') begin Insert into Folder(Path,Timeore,Timeminute,ProcessName,DisplayName)  values (@cale,@ore,@minute,@process,@d) end  else begin Select 2 end", con);
                    cmd.Parameters.AddWithValue("@cale", cale.Trim());
                    cmd.Parameters.AddWithValue(@"ore", 0);
                    cmd.Parameters.AddWithValue(@"minute", 0);
                    cmd.Parameters.AddWithValue(@"process", displayname);
                    cmd.Parameters.AddWithValue(@"d", name);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    fillContextmenustrip2();
                }
                catch (Exception)
                {

                }



                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }


                else
                {
                    return;
                }
            }
        }

        public string despartire(string path)
        {
            char split = '\\';
            path.Split(split);
            char split2 = '.';
            string[] b = path.Split(split);
            string[] a = b[b.Length - 1].Split(split2);
            cale = a[0];
            return cale;
        }

        public void fill()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            string search = textBox1.Text;
            using (SqlDataAdapter sda = new SqlDataAdapter("Select DisplayName from Folder where DisplayName like '%" + search + "%'", con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    int i = 0;
                    var imagelist = new ImageList();
                    while (i < dt.Rows.Count)
                    {
                        string txt = "    " + dt.Rows[i][0].ToString();
                        lista.Add(dt.Rows[i][0].ToString());
                        item = new ListViewItem(txt);
                        listView1.Items.Add(item);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sda.Dispose();
                    con.Close();
                }
            }
        }

        public void stergere_init()
        {
            con.Open();
            cmd = new SqlCommand("Delete  from Folder", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Folder,RESEED,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("Delete from Today", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Today,RESEED,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }

        public void stergere(string nume)
        {
            con.Open();
            cmd = new SqlCommand("Delete  from Folder where DisplayName=@name", con);
            cmd.Parameters.AddWithValue(@"name", nume);
            // MessageBox.Show(cmd.ExecuteNonQuery().ToString());
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("Delete from Today", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Today,RESEED,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Folder,RESEED,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Folder,RESEED)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            contextMenuStrip2.Items.RemoveAt(listView1.FocusedItem.Index);
            //cmd = new SqlCommand("ALTER TABLE Folder AUTO_INCREMENT = 1",con);
            //cmd.ExecuteNonQuery();
            //cmd.Dispose();
            con.Close();
        }

        //string[] search()
        //{ 
        //    con.Open();
        //    string[] path = new string[24];
        //    int i = 0;
        //    foreach (int a in listView1.SelectedItems)
        //    {
        //        cmd = new SqlCommand(@"Select * from Folder where Id=@b", con);
        //        cmd.Parameters.AddWithValue(@"b", (a + 1).ToString());
        //        cmd.ExecuteNonQuery();
        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        if (sdr.Read())
        //        {

        //            path[i] = sdr[1].ToString();
        //            i++;
        //        }
        //        sdr.Dispose();
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //    }
        //    con.Close();
        //    return path;
        //}

        public void timp(string path)
        {
            //  time.Start();
            con.Open();
            cmd = new SqlCommand("Select Timeore,Timeminute,ProcessName,Path from Folder where path=@cale", con);
            cmd.Parameters.AddWithValue(@"cale", path);
            //  MessageBox.Show(path);
            SqlDataReader sdr = cmd.ExecuteReader();
            //   MessageBox.Show(sdr[1].ToString());
            //  MessageBox.Show(cale);
            string[] citite = new string[10];
            int j = 0;
            if (sdr.Read())
            {
                citite[0] = sdr[0].ToString();
                citite[1] = sdr[1].ToString();
                citite[2] = sdr[2].ToString();
                citite[3] = sdr[3].ToString();
            }
            if (citite[0] == "0")
            {
                h = 0;
            }
            else
            {
                h = Convert.ToInt32(citite[0]);
            }
            if (citite[1] == "0")
            {
                m = 0;
            }
            else
            {
                m = Convert.ToInt32(citite[1]);
            }

            //  MessageBox.Show(citite[2]);
            con.Close();
            Process[] pvar = Process.GetProcesses();
            string[] procese = new string[500];
            int i = 0;
            processactual = citite[2];
            caleactuala = citite[3];
            //  MessageBox.Show(processactual + "\n" + caleactuala);
            label1.Text = "Played time: " + h.ToString() + "hours " + m.ToString() + " minutes";

        }
        // string cale = Process.GetCurrentProcess().MainModule.FileVersionInfo.FileDescription.ToString();

        private void button2_Click(object sender, EventArgs e)
        {
            launchApp(lista.ElementAt(listView1.FocusedItem.Index));
        }

        private void launchApp(string aidiu)
        {
            //p += 1;
            con.Open();
            // cmd = new SqlCommand("Select Path from Folder where  DisplayName=@b", con);
            cmd = new SqlCommand("Select Path from Folder where displayname = @b", con);
            cmd.Parameters.AddWithValue(@"b", aidiu);
            string path = (string)cmd.ExecuteScalar();
            cmd.Dispose();
            con.Close();
            //  listView1.FocusedItem.ForeColor = Color.Green;
            // listView1.Items[aidiu].ForeColor = Color.Green;
            // listView1.FocusedItem.Font = new Font(listView1.Font, FontStyle.Italic);
            System.Diagnostics.Process.Start(path);
            timp(path);
            pos.Add(p);
            listView1.Invalidate();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }


            using (SqlDataAdapter sda = new SqlDataAdapter("Select Path , Timeore , Timeminute from Folder where DisplayName = '" + lista.ElementAt(listView1.FocusedItem.Index) + "' ", con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    Icon imajine = Icon.ExtractAssociatedIcon(dt.Rows[0][0].ToString());
                    pictureBox1.Image = imajine.ToBitmap();
                    button2.Visible = true;
                    // label1.Text = "Played time: " + dt.Rows[0][1].ToString() + " Hours and " + dt.Rows[0][2].ToString() + " minutes";
                    label1.Text = "Played time: " + dt.Rows[0][1].ToString() + "Hours and " + dt.Rows[0][2].ToString() + " minutes";
                    label5.Text = listView1.FocusedItem.Text;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
            }

            if (listView1.SelectedItems.Count > 0)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Interval == 1000)
            {
                timer1.Interval = 60000;
            }

            Process[] pvar = Process.GetProcesses();
            fillProcessList();
            fillista();
            var items = (from p in pvar join y in processList on p.ProcessName equals y select y).ToHashSet();
            pos = (from p in pvar join y in processList on p.ProcessName equals y select processList.IndexOf(y)).ToList();

            //if (listView1.SelectedItems.Count == 1)
            //{
            //    afisare(listView1.FocusedItem.Index + 1);
            //}

            if (items.Count != 0)
            {
                Queue<notifcation> coada = new Queue<notifcation>();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                foreach (var item in items)
                {
                    try
                    {
                        if (Properties.Settings.Default.Notification == true && !NotifcationList.ContainsKey(item))
                        {
                            cmd = new SqlCommand("Select path,displayname from Folder where processname='" + item + "'", con);
                            cmd.ExecuteNonQuery();
                            SqlDataReader sdr = cmd.ExecuteReader();
                            if (sdr.Read() && this.ContainsFocus == false)
                            {
                                coada.Enqueue(new notifcation(sdr[0].ToString(), sdr[1].ToString(), Properties.Settings.Default.Notification_Place));
                            }
                            NotifcationList[item] = sdr[1].ToString();
                            cmd.Dispose();
                        }
                        cmd = new SqlCommand("Select timeore from Folder where processname='" + item + "'", con);
                        int ore = (int)cmd.ExecuteScalar();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        cmd = new SqlCommand("Select timeminute from Folder where processname='" + item + "'", con);
                        int min = (int)cmd.ExecuteScalar();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        min++;
                        if (min >= 60)
                        {
                            min = 0;
                            ore++;
                        }
                        cmd = new SqlCommand("Update Folder SET Timeore='" + ore.ToString() + "', Timeminute='" + min.ToString() + "' where processname='" + item + "'", con);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    catch (Exception)
                    {

                    }
                }
                con.Close();

                foreach (var item in coada)
                {
                    new th.Thread(() => { item.TopMost = true; item.TopLevel = true; item.ShowDialog(); }).Start();
                }
            }
            listView1.Invalidate();
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            con.Open();
            this.Invalidate();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            string path;
            // cmd = new SqlCommand("Select Path from Folder where id='" + (e.ItemIndex + 1).ToString() + "' ", con);
            cmd = new SqlCommand("Select Path from Folder where DisplayName = '" + lista.ElementAt(e.ItemIndex) + "' ", con);
            path = (string)cmd.ExecuteScalar();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Icon ic = Icon.ExtractAssociatedIcon(path);
            Bitmap bmp = ic.ToBitmap();
            e.Graphics.DrawImage(bmp, e.Item.Position.X, e.Item.Position.Y, 16, 16);
            if (pos.Contains(e.ItemIndex))
            {
                if (e.Item.Selected)
                {
                    e.Graphics.FillRectangle(Brushes.Gainsboro, e.Bounds.X + 24, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1);
                }
                e.Graphics.DrawString(e.Item.Text, new Font("Microsoft Sans Serif", 9.25f), Brushes.GreenYellow, e.Item.Position.X + 5, e.Item.Position.Y);
                //listView1.Invalidate(new Rectangle(e.Item.Position.X + 5, e.Item.Position.Y, listView1.Size.Width, listView1.Size.Height));
            }
            else
            {
                if (NotifcationList.ContainsValue(lista.ElementAt(e.ItemIndex)))
                {
                    var key = NotifcationList.FirstOrDefault(x => x.Value == lista.ElementAt(e.ItemIndex)).Key;
                    NotifcationList.Remove(key);
                }
                if (e.Item.Selected)
                {
                    e.Graphics.FillRectangle(Brushes.DarkViolet, e.Bounds.X + 20, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1);
                }
                e.Graphics.DrawString(e.Item.Text, new Font("Microsoft Sans Serif", 9.25f), Brushes.White, e.Item.Position.X + 5, e.Item.Position.Y);
            }
            con.Close();
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) && e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            catch (Exception)
            {

            }
        }
        ///////////////
        // tabpage 2 //
        //  |        //
        //  |        //
        //  V        //
        ///////////////

        private void button3_Click(object sender, EventArgs e) //raport zilnic
        {
            //chart1.ChartAreas[0].AxisX.Maximum = 24;
            //chart1.ChartAreas[0].AxisX.Minimum = 0;
        }

        private void button4_Click(object sender, EventArgs e) //raport saptamanal
        {
            //chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

        }

        private void button5_Click(object sender, EventArgs e) //raport lunar
        {

        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void checkday(string tag)
        {
            DateTime now = DateTime.Now;
            if (now.Month > thismonth.Month)
            {
                thismonth = now;
                fillmonth();
            }
            con.Open();
            int baga = 0;
            try
            {
                cmd = new SqlCommand("if not exists(Select 1 from Today where ziua ='" + now.Day + "') begin Select 1 end else begin Select 2 end", con);
                baga = (int)cmd.ExecuteScalar();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (baga == 1)
                {
                    delete_today();
                    con.Close();
                    fill_week();
                    con.Open();
                    cmd = new SqlCommand("Insert into Today(ziua,launch,ore,minute) values('" + now.Day + "','" + now.ToString("MM/dd/yyyy hh:mm:ss") + "',0,0)", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    ore_zilnice = 0;
                    minute_zilnice = 1;
                }
                else
                {
                    cmd = new SqlCommand("Select Ore from Today where id =(Select count(*) from Today)", con);
                    ore_zilnice = (int)cmd.ExecuteScalar();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd = new SqlCommand("Select Minute from Today where id=(Select count(*) from Today)", con);
                    minute_zilnice = (int)cmd.ExecuteScalar();
                    if (minute_zilnice == 60)
                    {
                        ore_zilnice++;
                        minute_zilnice = 0;
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    //   cmd.ExecuteNonQuery();
                    //   cmd.Dispose();
                    cmd = new SqlCommand("Insert into Today(ziua,launch,ore,minute) values('" + now.Day + "','" + now.ToString("MM/dd/yyyy hh:mm:ss") + "','" + ore_zilnice.ToString() + "','" + minute_zilnice.ToString() + "')", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception)
            {

            }

            con.Close();
        }

        private void fillmonth()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            int baga = 0;
            cmd = new SqlCommand("if not exists(Select 1 from Year) begin Select 1 end", con);
            baga = (int)cmd.ExecuteScalar();
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            if (baga == 1)
            {
                fill_month();
            }
            else
            {
                cmd = new SqlCommand("Insert into Year(" + DateTime.Now.Year + ") values((Select SUM(SUM(Week1)+SUM(Week2)+SUM(Week3)+SUM(Week4)+SUM(Week5)))) where id ='" + thismonth + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

        }

        private void delete_month()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd = new SqlCommand("Delete from Week", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Week,Reseed,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void fill_month()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            for (int i = 0; i < 12; i++)
            {
                cmd = new SqlCommand("Insert into Year(month) values(0)", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void fill_week()
        {
            con.Open();
            int ziua = (int)DateTime.Now.DayOfWeek;
            if (thisweek.AddDays(7) > DateTime.Now.Date)
            {
                if (idweek > 5)
                {
                    idweek = 1;
                    //fill year
                    fillmonth();
                    //clear week
                    return;
                }
                else
                {
                    idweek++;
                }
            }
            if (ziua == 0)
            {
                ziua = 7;
            }
            if (minute_zilnice >= 29)
            {
                cmd = new SqlCommand("Update Week SET Week" + (idweek).ToString() + "='" + (ore_zilnice + 1).ToString() + "' where id='" + ziua.ToString() + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
            {
                cmd = new SqlCommand("Update Week SET Week" + (idweek).ToString() + "='" + ore_zilnice.ToString() + "' where id='" + ziua.ToString() + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            con.Close();
        }

        private void delete_today()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd = new SqlCommand("Delete from  Today", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Today,RESEED,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // este apelata cand se apasa pe butonul  X sau se apasa alt+f4
        {
            DateTime now = DateTime.Now;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();

                //cmd = new SqlCommand("Update Today SET Stop='" + now.ToString("MM/dd/yyyy hh:mm:ss") + "' , Ore='" + ore_zilnice + "', minute ='" + minute_zilnice + "' where id = (Select count(*) from Today)", con);
                //cmd.ExecuteNonQuery();
                //cmd.Dispose();
                //con.Close();
            }

            //cmd = new SqlCommand("Update Folder SET Timeore='" + ore.ToString() + "', Timeminute='" + min.ToString() + "' where processname='" + item + "'", con);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }


        }

        private void fillweektable()
        {
            //      delete_week();
            int baga = 0;
            con.Open();
            cmd = new SqlCommand("if not exists(Select 1 from Week) begin Select 1 end else begin Select 0 end", con);
            baga = (int)cmd.ExecuteScalar();
            if (baga == 1)
            {
                for (int i = 0; i < 7; i++)
                {
                    cmd = new SqlCommand("Insert into Week(week2,week3,week4,week5,week1) values(0,0,0,0,0) ", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            con.Close();
        }

        private void delete_week()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd = new SqlCommand("Delete from Week", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("DBCC CHECKIDENT(Week,Reseed,0)", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                label6.Text = ore_zilnice.ToString() + " hours and " + minute_zilnice + " minutes";
                SqlDataAdapter sda = new SqlDataAdapter("Select Ore from Today", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                // MessageBox.Show("mere");
                //chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Maroon;
                //chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Maroon;
                //chart1.ChartAreas[0].AxisX.Maximum = 24;
                //chart1.ChartAreas[0].AxisX.Minimum = 0;
                //chart1.ChartAreas[0].AxisY.Maximum = 24;
                //chart1.ChartAreas[0].AxisY.Minimum = 0;
                //   chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                //chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                //   chart1.Series[0].Points.AddXY(5, 5);
                //  chart1.Series[0].Points.AddXY(8, 7);
                int i = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    int ore = (int)dt.Rows[i][0];
                    //      chart1.Series[0].Points.AddXY(ore, ore);
                    //     chart1.Series[0].Points.AddXY((double)dt.Rows[i][0], (double)dt.Rows[i][0]);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //setari.Show();
            if (off == true)
            {
                setari = new Settings();
                setari.FormClosed += close_setari;
                setari.WindowState = FormWindowState.Normal;
                off = false;
                setari.Show();
                setari.BringToFront();
                setari.Focus();
            }
            else
            {
                setari.TopLevel = true;
                setari.BringToFront();
            }
        }

        private void Rotateimage_Tick(object sender, EventArgs e)
        {
            if (status == 0)
            {
                if (degree < 90)
                {
                    degree += 10;
                    pictureBox2.Image = rotateImage(imagine_setari, degree, picturebox_center);
                }
                else
                {
                    status = 1;
                    rotateimage.Stop();
                }
            }
            else
            {
                if (degree > 0)
                {
                    degree -= 10;
                    pictureBox2.Image = rotateImage(imagine_setari, degree, picturebox_center);
                }
                else
                {
                    status = 0;
                    rotateimage.Stop();
                }
            }
        }

        private Bitmap rotateImage(Image image, float degrees, Point offset)
        {
            // Invalidate();
            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            rotatedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics graphics = Graphics.FromImage(rotatedImage);
            graphics.TranslateTransform(offset.X, offset.Y);
            graphics.RotateTransform(degrees);
            graphics.TranslateTransform(-offset.X, -offset.Y);
            graphics.DrawImage(image, new Point(0, 0));
            return rotatedImage;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!contextMenuStrip1.ClientRectangle.Contains(e.Location))
            //{

            //}
            //else
            //{
            //    Cursor = Cursors.Arrow;
            //}
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {

            Cursor = Cursors.Hand;
            if (status == 0)
            {
                rotateimage.Start();
            }
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            if (status == 1)
            {
                rotateimage.Start();
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (panel2_color != Pens.WhiteSmoke)
            {
                panel2.Invalidate();
                panel2_color = Pens.WhiteSmoke;
            }

        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.Invalidate();
            panel2_color = Pens.Gray;
        }



        private void panel2_Paint(object sender, PaintEventArgs e) //108 33  
        {
            Graphics panel2_graphics = e.Graphics;

            //draw the rectangle with the "+" in the center
            /*
            trebe sa fac resize la patrat arata prea urat asa
            */

            panel2_graphics.DrawRectangle(panel2_color, 6, 10, 20, 20);
            panel2_graphics.FillRectangle(panel2_color.Brush, 15.05f, 13, 3f, 14);
            panel2_graphics.FillRectangle(panel2_color.Brush, 10f, 18f, 14, 3f);
            panel2_graphics.DrawString("ADD AN APP", new Font("Arial", 8f), panel2_color.Brush, new PointF(38, 13));
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            ofd = new OpenFileDialog
            {
                //InitialDirectory = @"D:\",
                Title = "Selectati aplicatia",
                DereferenceLinks = false,
                CheckFileExists = true,
                CheckPathExists = true,
                ShowReadOnly = true,
                Filter = "Object Files (*.exe)|*.exe",
                FilterIndex = 0,
            };
            ofd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(ofd.FileName))
            {
                safefilename = despartire(ofd.FileName);
                try
                {
                    IWshRuntimeLibrary.WshShell w = new IWshRuntimeLibrary.WshShell();
                    IWshRuntimeLibrary.IWshShortcut s = (IWshShortcut)w.CreateShortcut(ofd.FileName.Trim());
                    adaugare(s.TargetPath, safefilename, despartire(s.TargetPath));
                }
                catch (Exception)
                {
                    adaugare(ofd.FileName, safefilename, despartire(ofd.SafeFileName));
                }

                if (!string.IsNullOrWhiteSpace(cale))
                {
                    listView1.Clear();
                    listView1.Columns.Add("Nume Aplicatie", -2);
                    fill();
                }
                fillista();
                fillProcessList();
                listView1.Invalidate();
                //     listView1_DrawSubItem(this, new DrawListViewSubItemEventArgs(listView1.CreateGraphics(), item.Bounds, item, null, listView1.Items.Count, 0, null, ListViewItemStates.Default));
            }
        }

        /*
                ANIMATIA DE LOADING SCREEN
                |
                |
                V
                 pulalela n-am mai facut-o
              */




        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {




        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {



        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {



        }

        private void label7_MouseMove(object sender, MouseEventArgs e)
        {
            if (label7.ForeColor != Color.WhiteSmoke)
            {
                label7.ForeColor = Color.Silver;
            }
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            if (label7.ForeColor != Color.WhiteSmoke)
            {
                label7.ForeColor = Color.Gray;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                tabControl1.SelectedTab = tabControl1.TabPages[0];
                label7.ForeColor = Color.WhiteSmoke;
                label8.ForeColor = Color.Gray;
            }
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {

            if (label8.ForeColor != Color.WhiteSmoke)
            {
                label8.ForeColor = Color.Gray;
            }

        }

        private void label8_MouseMove(object sender, MouseEventArgs e)
        {

            if (label8.ForeColor != Color.WhiteSmoke)
            {
                label8.ForeColor = Color.Silver;
            }

        }

        private void label8_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedIndex != 1)
            {
                tabControl1.SelectedTab = tabControl1.TabPages[1];
                label8.ForeColor = Color.WhiteSmoke;
                label7.ForeColor = Color.Gray;
            }

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

            textBox1.BorderStyle = BorderStyle.Fixed3D;
            textbox_button_X.StartFigure();
            textbox_button_X.AddLine(panel4.Right - 25, panel4.Location.Y + 5, panel4.Right - 15, panel4.Location.Y + 18);
            textbox_button_X.CloseFigure();
            textbox_button_X.StartFigure();
            textbox_button_X.AddLine(panel4.Right - 15, panel4.Location.Y + 5, panel4.Right - 25, panel4.Location.Y + 18);
            textbox_button_X.CloseFigure();
            panel4.Invalidate();

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textbox_button_X = new GraphicsPath();
            panel4.Invalidate();

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            if (textbox_button_X.PointCount != 0)
            {
                e.Graphics.DrawPath(Pens.Red, textbox_button_X);
            }

        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (rect4.Contains(e.Location) && textbox_button_X.PointCount != 0)
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            if (rect4.Contains(e.Location) && textbox_button_X.PointCount != 0 && e.Button == MouseButtons.Left)
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text) == true)
                {
                    panel4.Invalidate();
                    textbox_button_X = new GraphicsPath();
                    panel4.Focus();
                    return;
                }
                textBox1.Text = "";
                Cursor = Cursors.Hand;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.IBeam;
            listView1.Clear();
            listView1.Columns.Add("Nume aplicatie", -2);
            lista = new HashSet<string>();
            fill();
            fillProcessList();
            Process[] pvar = Process.GetProcesses();
            pos = (from p in pvar join y in processList on p.ProcessName equals y select processList.IndexOf(y)).ToList();
            listView1.Invalidate();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void listView1_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening_1(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (contextMenuStrip1.Items[0] == e.ClickedItem)
            {
                contextMenuStrip1.Close();
                var result = MessageBox.Show(Owner, "This action cannot be undone!", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    stergere(lista.ElementAt(listView1.FocusedItem.Index));
                    listView1.Clear();
                    listView1.Columns.Add("Nume aplicatie", -2);
                    fill();
                    fillista();
                    fillProcessList();
                    Process[] pvar = Process.GetProcesses();
                    pos = (from p in pvar join y in processList on p.ProcessName equals y select processList.IndexOf(y)).ToList();
                    listView1.Invalidate();
                }
            }
        }

        private void deleteToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            //   Cursor = Cursors.Hand;
            contextMenuStrip1.Items[0].ForeColor = Color.LightGray;
        }

        private void deleteToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            contextMenuStrip1.Items[0].ForeColor = Color.DodgerBlue;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {


        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.ContainsFocus == false && e.Button == MouseButtons.Left)
            {
                this.Show();
                this.Activate();
                this.WindowState = FormWindowState.Normal;
                this.Focus();
            }

        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && contextMenuStrip2.Visible == false)
            {
                notifyIcon1.ContextMenuStrip = contextMenuStrip2;
            }

        }

        private void fillContextmenustrip2()
        {
            contextMenuStrip2.Items.Clear();
            fillista();

            foreach (var itam in lista)
            {
                ToolStripMenuItem tool = new ToolStripMenuItem();
                tool.Text = itam;
                tool.Name = itam;
                tool.ForeColor = Color.DarkTurquoise;
                //tool.MouseMove += exitToolStripMenuItem_MouseMove;
                //tool.MouseLeave += exitToolStripMenuItem_MouseLeave;
                tool.MouseMove += new MouseEventHandler((sender, e) => tool_MouseMove(sender, e, tool));
                tool.MouseLeave += new EventHandler((sender, e) => tool_MouseLeave(sender, e, tool));
                tool.Click += new EventHandler((sender, e) => Tool_Click(sender, e, tool));
                contextMenuStrip2.Items.Add(tool);
            }

            if (lista.Count > 0)
            {
                ToolStripSeparator stripSeparator = new ToolStripSeparator();
                contextMenuStrip2.Items.Add(stripSeparator);
            }

            ToolStripMenuItem exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Name = "Exit";
            exit.ForeColor = Color.DarkTurquoise;
            exit.MouseMove += new MouseEventHandler((sender, e) => tool_MouseMove(sender, e, exit));
            exit.MouseLeave += new EventHandler((sender, e) => tool_MouseLeave(sender, e, exit));
            exit.Click += new EventHandler((sender, e) => exit_Click(sender, e, exit));
            contextMenuStrip2.Items.Add(exit);
        }

        private void exit_Click(object sender, EventArgs e, ToolStripMenuItem exit)
        {
            Application.Exit();
        }

        private void tool_MouseMove(object sender, MouseEventArgs e, ToolStripMenuItem tool)
        {
            tool.ForeColor = Color.Gray;
        }

        private void tool_MouseLeave(object sender, EventArgs e, ToolStripMenuItem tool)
        {
            tool.ForeColor = Color.DarkTurquoise;
        }

        private void Tool_Click(object sender, EventArgs e, ToolStripMenuItem tool)
        {
            launchApp(tool.Text);
        }

        private void contextMenuStrip2_Paint(object sender, PaintEventArgs e)
        {
            //  e.Graphics.DrawLine(new Pen(Brushes.Gray,10), new Point(2, contextMenuStrip2.Bottom - 1), new Point(2, contextMenuStrip2.Right - 1));

        }
        /*-----------------------------------------------------------------------------------------*/




        /*            |
                  |
                  V n-am mai facut nimic aici nici nu cred ca mai fac ceva
        --------------------Drag and drop iteme listview--------------------------------------------------------------------
        */


        //------------------------------------------------------------------------------------------------------------------
        private void timer2_Tick(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }

    }
}

