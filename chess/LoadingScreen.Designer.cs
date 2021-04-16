namespace chess
{
    partial class LoadingScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.TextTimer = new System.Windows.Forms.Timer(this.components);
            this.slidingLines1 = new chess.slidingLines();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(155, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "LOADING...";
            // 
            // TextTimer
            // 
            this.TextTimer.Tick += new System.EventHandler(this.TextTimer_Tick);
            // 
            // slidingLines1
            // 
            this.slidingLines1.Distance = 10;
            this.slidingLines1.Location = new System.Drawing.Point(141, 12);
            this.slidingLines1.Name = "slidingLines1";
            this.slidingLines1.numbOfLines = 5;
            this.slidingLines1.Size = new System.Drawing.Size(136, 179);
            this.slidingLines1.TabIndex = 0;
            this.slidingLines1.Load += new System.EventHandler(this.slidingLines1_Load_1);
            this.slidingLines1.Paint += new System.Windows.Forms.PaintEventHandler(this.slidingLines1_Paint);
            // 
            // LoadingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(443, 301);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.slidingLines1);
            this.ForeColor = System.Drawing.Color.Gray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingScreen";
            this.Text = "TimeTracker";
            this.Load += new System.EventHandler(this.LoadingScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private slidingLines slidingLines1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer TextTimer;

        #endregion

        //   private slidingLines slidingLines1;
    }
}