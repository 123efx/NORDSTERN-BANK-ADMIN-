    using iTextSharp.text;
    using iTextSharp.text.pdf; 
    using MySql.Data;
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO; 
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Diagnostics;

namespace nordenstern_bank
    {
        public partial class anapage : Form
        {
            List<int> changedCustomers = new List<int>();
            List<int> changedCredentials = new List<int>();
            List<int> changedPayments = new List<int>();
            List<int> changedLoans = new List<int>();
            List<int> changedTransactions = new List<int>();
            List<int> changedAccounts = new List<int>();

            MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=efeates2008;database=nordstern_bank;");
            DataTable table = new DataTable();

            private static readonly BaseColor GOLD = new BaseColor(255, 215, 0);
            private static readonly BaseColor BANK_BLAU = new BaseColor(10, 25, 60);

        public anapage()
            {
                InitializeComponent();

                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Maximized;

                LoadLastCustomerId();
                LoadLoanCount();
                OffeneAnträge();

                LoadCustomers();
                LoadAccounts();
                LoadCredentials();
                LoadTransactions();
                LoadLoans();
                LoadPayments();

            ApplyPermissions();
        }

        private void ExportAndPrintCustomersPdf()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "Customers.pdf");

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(tempFile, FileMode.Create));
            doc.Open();

            PdfPTable table = new PdfPTable(dataGridView1.ColumnCount)
            {
                WidthPercentage = 100
            };

            foreach (DataGridViewColumn column in dataGridView1.Columns)
                table.AddCell(new Phrase(column.HeaderText));

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                foreach (DataGridViewCell cell in row.Cells)
                    table.AddCell(new Phrase(cell.Value?.ToString() ?? ""));
            }

            doc.Add(table);
            doc.Close();

            try
            {
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    Verb = "print",
                    FileName = tempFile,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process p = new Process();
                p.StartInfo = info;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Drucken fehlgeschlagen: " + ex.Message);
            }
        }

        private void printbutton_Click(object sender, EventArgs e)
        {
            ExportAndPrintCustomersPdf();
        }

        private void LoadAccounts()
            {
                conn.Open();
                string query = "SELECT * FROM accounts";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView6.DataSource = table;
                conn.Close();
            }

            private void LoadCustomers()
            {
                conn.Open();
                string query = "SELECT * FROM customers";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView1.DataSource = table;
                conn.Close();
            }

            private void LoadCredentials()
            {
                conn.Open();
                string query = "SELECT * FROM customer_credentials";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView2.DataSource = table;
                conn.Close();
            }

            private void LoadTransactions()
            {
                conn.Open();
                string query = "SELECT * FROM transactions";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView5.DataSource = table;
                conn.Close();
            }

            private void LoadPayments()
            {
                conn.Open();
                string query = "SELECT * FROM loan_payments";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView3.DataSource = table;
                conn.Close();
            }

            private void LoadLoans()
            {
                conn.Open();
                string query = "SELECT * FROM loans";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView4.DataSource = table;
                conn.Close();
            }

            private void anapage_Load(object sender, EventArgs e)
            {
                LoadCustomers();
                LoadAccounts();
                LoadCredentials();
                LoadTransactions();
                LoadLoans();
                LoadPayments();
            }

            private void KreditButton_Click(object sender, EventArgs e)
            {
                Kreditbeantragen kreditbeantragen = new Kreditbeantragen();
                kreditbeantragen.ShowDialog();
            }

            private void KontoButton_Click(object sender, EventArgs e)
            {
                Kontobeantragen kontobeantragen = new Kontobeantragen();
                kontobeantragen.ShowDialog();
            }

            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
            }

            private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
            {
                if (dataGridView1.IsCurrentCellDirty)
                {
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }

            private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (!changedCustomers.Contains(e.RowIndex))
                    changedCustomers.Add(e.RowIndex);

                speichern1.Visible = true;  
                zurücksetzen1.Visible = true;
            }

            private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (!changedCredentials.Contains(e.RowIndex))
                    changedCredentials.Add(e.RowIndex);

                speichern2.Visible = true;
                zurücksetzen2.Visible = true;
            }

            private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (!changedPayments.Contains(e.RowIndex))
                    changedPayments.Add(e.RowIndex);

                speichern3.Visible = true;
                zurücksetzen3.Visible = true;
            }

            private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (!changedLoans.Contains(e.RowIndex))
                    changedLoans.Add(e.RowIndex);

                speichern4.Visible = true;
                zurücksetzen4.Visible = true;
            }

            private void dataGridView5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (!changedTransactions.Contains(e.RowIndex))
                    changedTransactions.Add(e.RowIndex);

                speichern5.Visible = true;
                zurücksetzen5.Visible = true; 
            }

            private void dataGridView6_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                if (!changedAccounts.Contains(e.RowIndex))
                    changedAccounts.Add(e.RowIndex);

                speichern6.Visible = true;
                zurücksetzen6.Visible = true; 
            }

            private void speichern1_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            conn.Open();

                foreach (int rowIndex in changedCustomers)
                {
                    DataGridViewRow row = dataGridView1.Rows[rowIndex];

                    int customerId = Convert.ToInt32(row.Cells["customer_id"].Value);
                    string vorname = row.Cells["vorname"].Value.ToString();
                    string nachname = row.Cells["nachname"].Value.ToString();
                    string beruf = row.Cells["beruf"].Value.ToString();
                    string abschluss = row.Cells["abschluss"].Value.ToString();
                    string staats = row.Cells["staatsangehoerigkeit"].Value.ToString();
                    string passId = row.Cells["pass_id"].Value.ToString();
                    string einkommen = row.Cells["einkommen"].Value.ToString();

                    string query =
                        "UPDATE customers SET vorname=@v, nachname=@n, beruf=@b, abschluss=@a, " +
                        "staatsangehoerigkeit=@s, pass_id=@p, einkommen=@e " +
                        "WHERE customer_id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@v", vorname);
                    cmd.Parameters.AddWithValue("@n", nachname);
                    cmd.Parameters.AddWithValue("@b", beruf);
                    cmd.Parameters.AddWithValue("@a", abschluss);
                    cmd.Parameters.AddWithValue("@s", staats);
                    cmd.Parameters.AddWithValue("@p", passId);
                    cmd.Parameters.AddWithValue("@e", einkommen);
                    cmd.Parameters.AddWithValue("@id", customerId);

                    cmd.ExecuteNonQuery();
                }

            string auditSql = "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r, @a, @t)";

            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Kundendaten bearbeitet");
            auditCmd.Parameters.AddWithValue("@t", "customers");
            auditCmd.ExecuteNonQuery();

            conn.Close();
                changedCustomers.Clear();
                zurücksetzen1.Visible = false;
                speichern1.Visible = false;

                MessageBox.Show("Änderungen erfolgreich gespeichert!");
            }

            private void speichern2_Click(object sender, EventArgs e)
            {

            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            conn.Open();

                foreach (int rowIndex in changedCredentials)
                {
                    DataGridViewRow row = dataGridView2.Rows[rowIndex];

                    int credentialId = Convert.ToInt32(row.Cells["credential_id"].Value);
                    int customerId = Convert.ToInt32(row.Cells["customer_id"].Value);
                    string loginPw = row.Cells["login_passwort"].Value.ToString();
                    string kartenPw = row.Cells["karten_passwort"].Value.ToString();

                    string query =
                        "UPDATE customer_credentials SET login_passwort=@lp, karten_passwort=@kp WHERE credential_id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@lp", loginPw);
                    cmd.Parameters.AddWithValue("@kp", kartenPw);
                    cmd.Parameters.AddWithValue("@id", credentialId);

                    cmd.ExecuteNonQuery();
                }

            string auditSql = "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r, @a, @t)";

            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Zugangsdaten bearbeitet");
            auditCmd.Parameters.AddWithValue("@t", "customer_credentials");
            auditCmd.ExecuteNonQuery();

            conn.Close();
                changedCredentials.Clear();
                zurücksetzen2.Visible = false;
                speichern2.Visible = false;

                MessageBox.Show("Credentials gespeichert!");
            }

            private void speichern3_Click(object sender, EventArgs e)
            {

            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            conn.Open();

                foreach (int rowIndex in changedPayments)
                {
                    DataGridViewRow row = dataGridView3.Rows[rowIndex];

                    int paymentId = Convert.ToInt32(row.Cells["payment_id"].Value);
                    decimal rate = Convert.ToDecimal(row.Cells["rate"].Value);
                    DateTime faellig = Convert.ToDateTime(row.Cells["faelligkeit"].Value);
                    string bezahlt = row.Cells["bezahlt"].Value.ToString();

                    string query =
                        "UPDATE loan_payments SET rate=@r, faelligkeit=@f, bezahlt=@b WHERE payment_id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@r", rate);
                    cmd.Parameters.AddWithValue("@f", faellig);
                    cmd.Parameters.AddWithValue("@b", bezahlt);
                    cmd.Parameters.AddWithValue("@id", paymentId);

                    cmd.ExecuteNonQuery();
                }

            string auditSql = "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r, @a, @t)";

            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Kreditraten bearbeitet");
            auditCmd.Parameters.AddWithValue("@t", "loan_payments");
            auditCmd.ExecuteNonQuery();

            conn.Close();
                changedPayments.Clear();
                speichern3.Visible = false;
                zurücksetzen3.Visible = false;

                MessageBox.Show("Zahlungen gespeichert!");
            }

            private void speichern4_Click(object sender, EventArgs e)
            {

            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            conn.Open();

                foreach (int rowIndex in changedLoans)
                {
                    DataGridViewRow row = dataGridView4.Rows[rowIndex];

                    int loanId = Convert.ToInt32(row.Cells["loan_id"].Value);
                    decimal betrag = Convert.ToDecimal(row.Cells["betrag"].Value);
                    decimal zinssatz = Convert.ToDecimal(row.Cells["zinssatz"].Value);

                    string query =
                        "UPDATE loans SET betrag=@b, zinssatz=@z WHERE loan_id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@b", betrag);
                    cmd.Parameters.AddWithValue("@z", zinssatz);
                    cmd.Parameters.AddWithValue("@id", loanId);

                    cmd.ExecuteNonQuery();
                }

            string auditSql = "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r, @a, @t)";

            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Kredite bearbeitet");
            auditCmd.Parameters.AddWithValue("@t", "loans");
            auditCmd.ExecuteNonQuery();  

            conn.Close();
                changedLoans.Clear();
                speichern4.Visible = false;
                zurücksetzen4.Visible = false;

                MessageBox.Show("Kredite gespeichert!");
            }

            private void speichern5_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            conn.Open();

            foreach (int rowIndex in changedAccounts)
            {
                DataGridViewRow row = dataGridView5.Rows[rowIndex];

                int accId = Convert.ToInt32(row.Cells["account_id"].Value);
                decimal betrag = Convert.ToDecimal(row.Cells["betrag"].Value); 
                string art = row.Cells["art"].Value.ToString();  

                string query =
                    "UPDATE transactions SET betrag=@b, art=@a WHERE account_id=@id"; 

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@b", betrag);
                cmd.Parameters.AddWithValue("@a", art); 

                cmd.ExecuteNonQuery();
            }

            string auditSql = "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r, @a, @t)";

            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Transaktionen bearbeitet");
            auditCmd.Parameters.AddWithValue("@t", "transactions");
            auditCmd.ExecuteNonQuery();

            conn.Close();
            changedAccounts.Clear();
            speichern6.Visible = false;
            zurücksetzen6.Visible = false;

            MessageBox.Show("Accounts gespeichert!");
        }

            private void speichern6_Click(object sender, EventArgs e)
            {
                if (RoleManager.CurrentRole == "azubi")
                {
                    MessageBox.Show("Keine Berechtigung!");
                    return;
                }

                conn.Open();

                foreach (int rowIndex in changedAccounts)
                {
                    DataGridViewRow row = dataGridView6.Rows[rowIndex];

                    int accId = Convert.ToInt32(row.Cells["account_id"].Value);
                    decimal kontostand = Convert.ToDecimal(row.Cells["kontostand"].Value);
                    string kontotyp = row.Cells["kontotyp"].Value.ToString();

                    string query =
                        "UPDATE accounts SET kontostand=@k, kontotyp=@t WHERE account_id=@id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@k", kontostand);
                    cmd.Parameters.AddWithValue("@t", kontotyp);
                    cmd.Parameters.AddWithValue("@id", accId);
                    cmd.ExecuteNonQuery();
                }

                string auditSql =
                    "INSERT INTO auditlog (rolle, aktion, tabellenname) " +
                    "VALUES (@rolle, @aktion, @tabelle)";

                MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
                auditCmd.Parameters.AddWithValue("@rolle", RoleManager.CurrentRole);
                auditCmd.Parameters.AddWithValue("@aktion", "Mehrere Accounts bearbeitet");
                auditCmd.Parameters.AddWithValue("@tabelle", "accounts");
                auditCmd.ExecuteNonQuery();

                conn.Close();

                changedAccounts.Clear();
                speichern6.Visible = false;
                zurücksetzen6.Visible = false;

                MessageBox.Show("Accounts gespeichert!");
            }

            private void zurücksetzen1_Click(object sender, EventArgs e)
            {
                if (RoleManager.CurrentRole == "azubi")
                {
                    MessageBox.Show("Keine Berechtigung!");
                    return;
                }
                LoadCustomers();
                    changedCustomers.Clear();
                    zurücksetzen1.Visible = false;
                    speichern1.Visible = false;

                    MessageBox.Show("Änderungen wurden zurückgesetzt.");
            }

            private void zurücksetzen2_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            LoadCredentials();
                changedCredentials.Clear();
                zurücksetzen2.Visible = false;
                speichern2.Visible = false;

                MessageBox.Show("Änderungen wurden zurückgesetzt.");
            }

            private void zurücksetzen3_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            LoadPayments();
                changedPayments.Clear();
                zurücksetzen3.Visible = false;
                speichern3.Visible = false;

                MessageBox.Show("Änderungen wurden zurückgesetzt.");
            }

            private void zurücksetzen4_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            LoadLoans();
                changedLoans.Clear();
                zurücksetzen4.Visible = false;
                speichern4.Visible = false;

                MessageBox.Show("Änderungen wurden zurückgesetzt.");
            }

            private void zurücksetzen5_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            LoadTransactions();
                changedTransactions.Clear();
                zurücksetzen5.Visible = false;
                speichern5.Visible = false;

                MessageBox.Show("Änderungen wurden zurückgesetzt.");
            }

            private void zurücksetzen6_Click(object sender, EventArgs e)
            {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }
            LoadAccounts();
                changedAccounts.Clear();
                zurücksetzen6.Visible = false;
                speichern6.Visible = false;

                MessageBox.Show("Änderungen wurden zurückgesetzt.");
            }

            private void SearchInAllColumns(DataGridView dgv, string text)
            {
                if (dgv.DataSource == null) return;

                CurrencyManager cm = (CurrencyManager)BindingContext[dgv.DataSource];
                DataView dv = (DataView)cm.List;

                if (string.IsNullOrWhiteSpace(text))
                {
                    dv.RowFilter = "";
                    return;
                }

                StringBuilder filter = new StringBuilder();

                foreach (DataColumn col in dv.Table.Columns)
                {
                    if (col.DataType == typeof(string))
                        filter.Append($"{col.ColumnName} LIKE '%{text}%' OR ");
                    else
                        filter.Append($"CONVERT({col.ColumnName}, 'System.String') LIKE '%{text}%' OR ");
                }

                if (filter.Length > 4)
                    filter.Length -= 4;

                dv.RowFilter = filter.ToString();
            }

            private void SearchAllTables(string text)
            {
                SearchInAllColumns(dataGridView1, text);
                SearchInAllColumns(dataGridView2, text);
                SearchInAllColumns(dataGridView3, text);
                SearchInAllColumns(dataGridView4, text);
                SearchInAllColumns(dataGridView5, text);
                SearchInAllColumns(dataGridView6, text);
            }

            private void suchen_TextChanged(object sender, EventArgs e)
            {
                SearchAllTables(suchen.Text);
            }

        private void löschen1_Click(object sender, EventArgs e)
        {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            int customerId = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["customer_id"].Value);

            conn.Open();

            string sql = "DELETE FROM customers WHERE customer_id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", customerId);
            cmd.ExecuteNonQuery();

            string auditSql =
                "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r,@a,@t)";
            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Kunde gelöscht (ID " + customerId + ")");
            auditCmd.Parameters.AddWithValue("@t", "customers");
            auditCmd.ExecuteNonQuery();

            conn.Close();

            LoadCustomers();
            MessageBox.Show("Kunde erfolgreich gelöscht.");
        }

        private void löschen2_Click(object sender, EventArgs e)
        {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            int customerId = Convert.ToInt32(
                dataGridView2.CurrentRow.Cells["customer_id"].Value);

            conn.Open();

            string sql = "DELETE FROM customer_credentials WHERE customer_id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", customerId);
            cmd.ExecuteNonQuery();

            string auditSql =
                "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r,@a,@t)";
            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Credentials gelöscht (Customer ID " + customerId + ")");
            auditCmd.Parameters.AddWithValue("@t", "customer_credentials");
            auditCmd.ExecuteNonQuery();

            conn.Close();

            LoadCredentials();
            MessageBox.Show("Credentials gelöscht.");
        }

        private void löschen3_Click(object sender, EventArgs e)
        {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            int paymentId = Convert.ToInt32(
                dataGridView3.CurrentRow.Cells["payment_id"].Value);

            conn.Open();

            string sql = "DELETE FROM loan_payments WHERE payment_id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", paymentId);
            cmd.ExecuteNonQuery();

            string auditSql =
                "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r,@a,@t)";
            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Loan-Payment gelöscht (ID " + paymentId + ")");
            auditCmd.Parameters.AddWithValue("@t", "loan_payments");
            auditCmd.ExecuteNonQuery();

            conn.Close();

            LoadPayments();
            MessageBox.Show("Zahlung gelöscht.");
        }

        private void löschen4_Click(object sender, EventArgs e)
        {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            int loanId = Convert.ToInt32(
                dataGridView4.CurrentRow.Cells["loan_id"].Value);

            conn.Open();

            string sql = "DELETE FROM loans WHERE loan_id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", loanId);
            cmd.ExecuteNonQuery();

            string auditSql =
                "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r,@a,@t)";
            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Kredit gelöscht (ID " + loanId + ")");
            auditCmd.Parameters.AddWithValue("@t", "loans");
            auditCmd.ExecuteNonQuery();

            conn.Close();

            LoadLoans();
            MessageBox.Show("Kredit gelöscht.");
        }

        private void löschen5_Click(object sender, EventArgs e)
        {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            int transactionId = Convert.ToInt32(
                dataGridView5.CurrentRow.Cells["transaction_id"].Value);

            conn.Open();

            string sql = "DELETE FROM transactions WHERE transaction_id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", transactionId);
            cmd.ExecuteNonQuery();

            string auditSql =
                "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r,@a,@t)";
            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Transaktion gelöscht (ID " + transactionId + ")");
            auditCmd.Parameters.AddWithValue("@t", "transactions");
            auditCmd.ExecuteNonQuery();

            conn.Close();

            LoadTransactions();
            MessageBox.Show("Transaktion gelöscht.");
        }

        private void löschen6_Click(object sender, EventArgs e)
        {
            if (RoleManager.CurrentRole == "azubi")
            {
                MessageBox.Show("Keine Berechtigung!");
                return;
            }

            int accountId = Convert.ToInt32(
                dataGridView6.CurrentRow.Cells["account_id"].Value);

            conn.Open();

            string sql = "DELETE FROM accounts WHERE account_id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", accountId);
            cmd.ExecuteNonQuery();

            string auditSql =
                "INSERT INTO auditlog (rolle, aktion, tabellenname) VALUES (@r,@a,@t)";
            MySqlCommand auditCmd = new MySqlCommand(auditSql, conn);
            auditCmd.Parameters.AddWithValue("@r", RoleManager.CurrentRole);
            auditCmd.Parameters.AddWithValue("@a", "Account gelöscht (ID " + accountId + ")");
            auditCmd.Parameters.AddWithValue("@t", "accounts");
            auditCmd.ExecuteNonQuery();

            conn.Close();

            LoadAccounts();
            MessageBox.Show("Account gelöscht.");
        }

        private void accountbeantragung_Click(object sender, EventArgs e)
            {
                accountbeantragung accountbeantragung = new accountbeantragung();
                accountbeantragung.ShowDialog(); 
            }

            private void button1_Click(object sender, EventArgs e)
            {
                Manuellantrag manuellantrag = new Manuellantrag();
                manuellantrag.ShowDialog();
            }

            private void LoadLastCustomerId()
            {
                conn.Open();

                string query = "SELECT customer_id FROM customers ORDER BY customer_id DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                object result = cmd.ExecuteScalar();

                conn.Close();

                if (result != null)
                    anzahlkunden.Text = result.ToString();
                else
                    anzahlkunden.Text = "0"; 
            }

            private void LoadLoanCount()
            {
                conn.Open();

                string query = "SELECT COUNT(loan_id) FROM loans";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                object result = cmd.ExecuteScalar();

                conn.Close();

                if (result != null)
                    anzahlkredit.Text = result.ToString();
                else
                    anzahlkredit.Text = "0";
            }

            private void OffeneAnträge()
            {
                int konto = 0;
                int kredit = 0;
                int account = 0;

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(antrag_id) FROM kontobeantragungen", conn))
                {
                    konto = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(antrag_id) FROM kreditbeantragungen", conn))
                {
                    kredit = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(antrag_id) FROM accountbeantragungen", conn))
                {
                    account = Convert.ToInt32(cmd.ExecuteScalar());
                }

                conn.Close();

                int gesamtanträge = konto + kredit + account;
                anzahlkredit.Text = gesamtanträge.ToString();
            }

            private void anzahlkunden_TextChanged(object sender, EventArgs e)
            {

            }

            private void SetReadOnly(DataGridView dgv)
            {
                dgv.ReadOnly = true; 
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
            }

            private void ApplyPermissions()
            {

                if (RoleManager.CurrentRole == "admin")
                    return;

                if (RoleManager.CurrentRole == "mitarbeiter")
                {
                    KreditButton.Visible = false;
                    KontoButton.Visible = false;
                    accountbeantragung.Visible = false;
                    button1.Visible = false;
                    auditlog.Visible = false;

                    return;
                }

                if (RoleManager.CurrentRole == "azubi")
                {
                    KreditButton.Visible = false;
                    KontoButton.Visible = false;
                    accountbeantragung.Visible = false;
                    button1.Visible = false;
                    auditlog.Visible = false;

                    speichern1.Visible = false; 
                    speichern2.Visible = false;
                    speichern3.Visible = false;
                    speichern4.Visible = false;
                    speichern5.Visible = false;
                    speichern6.Visible = false;

                    zurücksetzen1.Visible = false;
                    zurücksetzen2.Visible = false;
                    zurücksetzen3.Visible = false;
                    zurücksetzen4.Visible = false;
                    zurücksetzen5.Visible = false;
                    zurücksetzen6.Visible = false;

                    löschen1.Visible = false;
                    löschen2.Visible = false;
                    löschen3.Visible = false;
                    löschen4.Visible = false;
                    löschen5.Visible = false;
                    löschen6.Visible = false;

                    SetReadOnly(dataGridView1);
                    SetReadOnly(dataGridView2);
                    SetReadOnly(dataGridView3);
                    SetReadOnly(dataGridView4);
                    SetReadOnly(dataGridView5);
                    SetReadOnly(dataGridView6);
                }
            }

        private void auditlog_Click(object sender, EventArgs e)
        {
            auditlog auditlog = new auditlog();
            auditlog.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
        }
    }
}