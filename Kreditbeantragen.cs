using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient; 

namespace nordenstern_bank
{
    public partial class Kreditbeantragen : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=efeates2008;database=nordstern_bank;");

        public Kreditbeantragen()
        {
            InitializeComponent();
            LoadKredite();
        }

        private void LoadKredite()  
        {
            conn.Open();
            string query = "SELECT * FROM kreditbeantragungen";
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
            string kartenNummer = row.Cells["karten_nummer"].Value.ToString();
            string kartenPasswort = row.Cells["karten_passwort"].Value.ToString();

            string kreditbetrag = row.Cells["kreditbetrag"].Value.ToString();
            string laufzeit = row.Cells["laufzeit_monate"].Value.ToString();
            string zinssatz = row.Cells["zinssatz"].Value.ToString();
            string rate = row.Cells["rate"].Value.ToString();
            string faelligkeit = row.Cells["faelligkeit"].Value.ToString();
            string bezahlt = row.Cells["bezahlt"].Value.ToString();

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

            int newCustomerId = Convert.ToInt32(cmd1.ExecuteScalar());

            MySqlCommand cmd2 = new MySqlCommand(
                "INSERT INTO customer_credentials(customer_id, login_passwort, karten_nummer, karten_passwort) " +
                "VALUES(@id, @pw, @kn, @kp)",
                conn);

            cmd2.Parameters.AddWithValue("@id", newCustomerId);
            cmd2.Parameters.AddWithValue("@pw", loginPasswort);
            cmd2.Parameters.AddWithValue("@kn", kartenNummer);
            cmd2.Parameters.AddWithValue("@kp", kartenPasswort);

            cmd2.ExecuteNonQuery();

            MySqlCommand cmd3 = new MySqlCommand(
                "INSERT INTO loans(customer_id, kreditbetrag, laufzeit_monate, zinssatz, status, erstellt_am) " +
                "VALUES(@cid, @kb, @lm, @zs, 'aktiv', NOW()); SELECT LAST_INSERT_ID();",
                conn);

            cmd3.Parameters.AddWithValue("@cid", newCustomerId);
            cmd3.Parameters.AddWithValue("@kb", kreditbetrag);
            cmd3.Parameters.AddWithValue("@lm", laufzeit);
            cmd3.Parameters.AddWithValue("@zs", zinssatz);

            int loanId = Convert.ToInt32(cmd3.ExecuteScalar());

            MySqlCommand cmd4 = new MySqlCommand(
                "INSERT INTO loan_payments(loan_id, rate, faelligkeit, bezahlt, bezahlt_am) " +
                "VALUES(@lid, @r, @f, @bez, NULL)",
                conn);

            cmd4.Parameters.AddWithValue("@lid", loanId);
            cmd4.Parameters.AddWithValue("@r", rate);
            cmd4.Parameters.AddWithValue("@f", faelligkeit);
            cmd4.Parameters.AddWithValue("@bez", bezahlt);

            cmd4.ExecuteNonQuery();

            MySqlCommand cmd5 = new MySqlCommand(
                "UPDATE kreditbeantragungen SET status='angenommen' WHERE antrag_id=@id",
                conn);

            cmd5.Parameters.AddWithValue("@id", antragId);
            cmd5.ExecuteNonQuery();

            conn.Close();

            LoadKredite();
            MessageBox.Show("Kreditantrag erfolgreich angenommen!");

            if (dataGridView1.SelectedRows.Count == 0) return;

            row = dataGridView1.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["antrag_id"].Value);

            AntragService s = new AntragService();

            int customerId = s.AddCustomer(row); 
            s.AddCredentials(customerId);         
            s.DeleteApplication(id, "kreditbeantragungen"); 

            LoadKredite();
            MessageBox.Show("Kredit wurde genehmigt! Neuer Kunde ID: " + customerId);
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
            MySqlCommand cmd = new MySqlCommand("DELETE FROM kreditbeantragungen WHERE antrag_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", antragId);
            cmd.ExecuteNonQuery();
            conn.Close();

            LoadKredite();
            MessageBox.Show("Antrag wurde gelöscht.");
        }
    }
}
