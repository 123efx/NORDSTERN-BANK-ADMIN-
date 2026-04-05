using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nordenstern_bank
{
    public partial class accountbeantragung : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=efeates2008;database=nordstern_bank;");

        public accountbeantragung()
        {
            InitializeComponent();
            LoadAccounts(); 
        }

        private void LoadAccounts()
        {
            conn.Open();
            string query = "SELECT * FROM accountbeantragungen";
            MySqlCommand command = new MySqlCommand(query, conn); 
            MySqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            conn.Close();
        }

        private void ablehnen_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Bitte einen Antrag auswählen.");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            int antragId = Convert.ToInt32(row.Cells["antrag_id"].Value);
             
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM accountbeantragungen WHERE antrag_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", antragId);
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadAccounts();
            MessageBox.Show("Antrag wurde gelöscht.");
        }

        private void annehmen_Click(object  obj, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Bitte einen Antrag auswählen.");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            int antragId = Convert.ToInt32(row.Cells["antrag_id"].Value);
            string vorname = row.Cells["vorname"].Value.ToString();
            string nachname = row.Cells["nachname"].Value.ToString();
            string kontotyp = row.Cells["kontotyp"].Value.ToString();
            decimal kontostand = Convert.ToDecimal(row.Cells["kontostand"].Value);

            conn.Open();

            MySqlCommand cmd1 = new MySqlCommand(
                "INSERT INTO customers (vorname, nachname, erstellt_am) VALUES (@v, @n, NOW()); SELECT LAST_INSERT_ID();",
                conn);

            cmd1.Parameters.AddWithValue("@v", vorname);
            cmd1.Parameters.AddWithValue("@n", nachname);

            int customerId = Convert.ToInt32(cmd1.ExecuteScalar());

            MySqlCommand cmd2 = new MySqlCommand(
                "INSERT INTO accounts (customer_id, kontotyp, kontostand, iban, erstellt_am) " +
                "VALUES (@cid, @kt, @ks, @iban, NOW())",
                conn);

            cmd2.Parameters.AddWithValue("@cid", customerId);
            cmd2.Parameters.AddWithValue("@kt", kontotyp);
            cmd2.Parameters.AddWithValue("@ks", kontostand);
            cmd2.Parameters.AddWithValue("@iban", Helper.CreateIBAN());

            cmd2.ExecuteNonQuery();

            MySqlCommand cmd3 = new MySqlCommand(
                "INSERT INTO customer_credentials (customer_id, karten_nummer, login_passwort, karten_passwort) " +
                "VALUES (@id, @kn, @lp, @kp)",
                conn);

            cmd3.Parameters.AddWithValue("@id", customerId);
            cmd3.Parameters.AddWithValue("@kn", Helper.CreateKartenNummer());
            cmd3.Parameters.AddWithValue("@lp", Helper.CreatePassword());
            cmd3.Parameters.AddWithValue("@kp", Helper.CreatePassword());

            cmd3.ExecuteNonQuery();

            MySqlCommand cmd4 = new MySqlCommand(
                "DELETE FROM accountbeantragungen WHERE antrag_id=@id",
                conn);

            cmd4.Parameters.AddWithValue("@id", antragId);
            cmd4.ExecuteNonQuery();

            conn.Close();

            LoadAccounts();
            MessageBox.Show($"Konto genehmigt! Kunde ID: {customerId}");
        }
    }
}
