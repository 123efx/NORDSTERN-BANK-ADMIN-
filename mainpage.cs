using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nordenstern_bank
{
    public partial class loadingpage : Form
    {

        private Timer timer1;
        private int progressValue = 0; 
        public loadingpage()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None; 
            this.WindowState = FormWindowState.Maximized; 
            this.TopMost = true;

            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += Timer_Tick;
            timer.Start();
        } 

      private void Timer_Tick(object sender, EventArgs e)
        {
            progressValue++;
            progressBar1.Value = progressValue;

            if (progressValue >= 100)
            {
                timer.Stop(); 
                anapage main = new anapage();
                main.Show(); 
                this.Hide(); 
            }
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
        } 
    }
} 
