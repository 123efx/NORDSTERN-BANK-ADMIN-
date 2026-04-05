using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace nordenstern_bank
{

    public partial class login : Form
    {
        private int versuche = 0;

        public login()
        {
            InitializeComponent();
            passworttext.PasswordChar = '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (RoleManager.Login(passworttext.Text))
            {
                RoleManager.UserName = admintext.Text;

                loadingpage mainpage = new loadingpage();
                mainpage.Show();
                this.Hide();
            }
            else
            {
                versuche++;
                MessageBox.Show($"Falsches Passwort! Versuch {versuche}/5");

                if (versuche >= 5)
                {
                    DateTime sperrzeit = DateTime.Now.AddMinutes(10);
                    File.WriteAllText("sperre.txt", sperrzeit.ToString());

                    MessageBox.Show("Zu viele Fehlversuche! Programm 10 Minuten gesperrt.");
                    Application.Exit();
                }
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            string pfad = "sperre.txt";

            if (File.Exists(pfad))
            {
                DateTime sperrzeit = DateTime.Parse(File.ReadAllText(pfad));

                if (DateTime.Now < sperrzeit)
                {
                    MessageBox.Show("Programm gesperrt bis: " + sperrzeit);
                    Application.Exit();
                }
                else
                {
                    File.Delete(pfad);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
} 
