using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 

namespace nordenstern_bank
{
    public partial class auditlog : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=efeates2008;database=nordstern_bank;");
        DataTable table = new DataTable();
        public auditlog()
        {
            InitializeComponent();
            Tabelle(); 
        }

        private void Tabelle()
        {
            conn.Open();
            string query = "SELECT * FROM auditlog"; 
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            conn.Close();
        }
    }
}
