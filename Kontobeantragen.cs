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
    public partial class Kontobeantragen : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=efeates2008;database=nordstern_bank;");

        public Kontobeantragen()
        {
            InitializeComponent(); 
            LoadKonten();
        }

        private void LoadKonten()
        {
            conn.Open();
            string query = "SELECT * FROM kontobeantragungen";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            conn.Close();
        }

        private void annehmen_Click(object sender, EventArgs e)
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
            string beruf = row.Cells["beruf"].Value.ToString();
            string abschluss = row.Cells["abschluss"].Value.ToString();
            string staats = row.Cells["staatsangehoerigkeit"].Value.ToString();
            string passId = row.Cells["pass_id"].Value.ToString();
            string einkommen = row.Cells["einkommen"].Value.ToString();

            string loginPasswort = row.Cells["login_passwort"].Value.ToString();
            string kartenPasswort = row.Cells["karten_passwort"].Value.ToString();

            string kontotyp = row.Cells["kontotyp"].Value.ToString();

            int newCustomerId = 0;

            conn.Open();

            MySqlCommand cmd1 = new MySqlCommand(
                "INSERT INTO customers(vorname, nachname, beruf, abschluss, staatsangehoerigkeit, pass_id, einkommen, erstellt_am) " +
                "VALUES(@v, @n, @b, @a, @s, @p, @e, NOW()); SELECT LAST_INSERT_ID();",
                conn);

            cmd1.Parameters.AddWithValue("@v", vorname);
            cmd1.Parameters.AddWithValue("@n", nachname);
            cmd1.Parameters.AddWithValue("@b", beruf);
            cmd1.Parameters.AddWithValue("@a", abschluss);
            cmd1.Parameters.AddWithValue("@s", staats);
            cmd1.Parameters.AddWithValue("@p", passId);
            cmd1.Parameters.AddWithValue("@e", einkommen);

            newCustomerId = Convert.ToInt32(cmd1.ExecuteScalar());

            MySqlCommand cmd2 = new MySqlCommand(
                "INSERT INTO customer_credentials(customer_id, login_passwort, karten_passwort) " +
                "VALUES(@id, @pw, @kp)",
                conn);

            cmd2.Parameters.AddWithValue("@id", newCustomerId);
            cmd2.Parameters.AddWithValue("@pw", loginPasswort);
            cmd2.Parameters.AddWithValue("@kp", kartenPasswort);

            cmd2.ExecuteNonQuery();

            MySqlCommand cmd3 = new MySqlCommand(
                "INSERT INTO accounts(customer_id, kontotyp, erstellt_am) " +
                "VALUES(@cid, @kt, NOW())",
                conn);

            cmd3.Parameters.AddWithValue("@cid", newCustomerId);
            cmd3.Parameters.AddWithValue("@kt", kontotyp);

            cmd3.ExecuteNonQuery();

            MySqlCommand cmd4 = new MySqlCommand(
                "UPDATE kontobeantragungen SET status='angenommen' WHERE antrag_id=@id",
                conn);
            cmd4.Parameters.AddWithValue("@id", antragId);
            cmd4.ExecuteNonQuery();

            conn.Close();

            LoadKonten();
            MessageBox.Show("Kontoantrag erfolgreich angenommen!");

            if (dataGridView1.SelectedRows.Count == 0) return;

            row = dataGridView1.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["antrag_id"].Value);

            AntragService s = new AntragService();

            int customerId = s.AddCustomer(row);
            s.AddCredentials(customerId);
            s.DeleteApplication(id, "kontobeantragungen");

            LoadKonten(); 
            MessageBox.Show("Konto wurde genehmigt! Kunde ID: " + customerId); 
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
            MySqlCommand cmd = new MySqlCommand("DELETE FROM kontobeantragungen WHERE antrag_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", antragId);
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadKonten();
            MessageBox.Show("Antrag wurde gelöscht.");
        }
    }
}
