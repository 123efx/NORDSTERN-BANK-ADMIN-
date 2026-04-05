using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nordenstern_bank
{
    public class AntragService
    {
        private string connStr = "server=localhost;user=root;password=efeates2008;database=nordstern_bank;";

        public int AddCustomer(DataGridViewRow row)
        {
            int newId = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string q = @"INSERT INTO customers 
                        (vorname, nachname, beruf, abschluss, staatsangehoerigkeit, pass_id, einkommen, erstellt_am)
                         VALUES (@v, @n, @b, @a, @s, @p, @e, @t);
                         SELECT LAST_INSERT_ID();";

                MySqlCommand cmd = new MySqlCommand(q, conn);

                cmd.Parameters.AddWithValue("@v", row.Cells["vorname"].Value);
                cmd.Parameters.AddWithValue("@n", row.Cells["nachname"].Value);
                cmd.Parameters.AddWithValue("@b", row.Cells["beruf"].Value);
                cmd.Parameters.AddWithValue("@a", row.Cells["abschluss"].Value);
                cmd.Parameters.AddWithValue("@s", row.Cells["staatsangehoerigkeit"].Value);
                cmd.Parameters.AddWithValue("@p", row.Cells["pass_id"].Value);
                cmd.Parameters.AddWithValue("@e", row.Cells["einkommen"].Value);
                cmd.Parameters.AddWithValue("@t", DateTime.Now);

                newId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return newId;
        }

        public void AddCredentials(int customerId)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string q = @"INSERT INTO customer_credentials
                        (customer_id, login_passwort, karten_nummer, karten_passwort)
                         VALUES (@id, @pw, @kn, @kp)";

                MySqlCommand cmd = new MySqlCommand(q, conn);

                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.Parameters.AddWithValue("@pw", Helper.CreatePassword());
                cmd.Parameters.AddWithValue("@kn", Helper.CreateKartenNummer());
                cmd.Parameters.AddWithValue("@kp", Helper.CreatePassword());

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteApplication(int id, string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    $"DELETE FROM {tableName} WHERE antrag_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
} 
