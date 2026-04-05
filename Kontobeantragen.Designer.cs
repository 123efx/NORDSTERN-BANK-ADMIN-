namespace nordenstern_bank
{
    partial class Kontobeantragen
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.annehmen = new System.Windows.Forms.Button();
            this.ablehnen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 371);
            this.dataGridView1.TabIndex = 0;
            // 
            // annehmen
            // 
            this.annehmen.Location = new System.Drawing.Point(12, 389);
            this.annehmen.Name = "annehmen";
            this.annehmen.Size = new System.Drawing.Size(368, 49);
            this.annehmen.TabIndex = 1;
            this.annehmen.Text = "Annehmen";
            this.annehmen.UseVisualStyleBackColor = true;
            this.annehmen.Click += new System.EventHandler(this.annehmen_Click);
            // 
            // ablehnen
            // 
            this.ablehnen.Location = new System.Drawing.Point(420, 389);
            this.ablehnen.Name = "ablehnen";
            this.ablehnen.Size = new System.Drawing.Size(368, 49);
            this.ablehnen.TabIndex = 2;
            this.ablehnen.Text = "Ablehnen";
            this.ablehnen.UseVisualStyleBackColor = true;
            this.ablehnen.Click += new System.EventHandler(this.ablehnen_Click);
            // 
            // Kontobeantragen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ablehnen);
            this.Controls.Add(this.annehmen);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Kontobeantragen";
            this.Text = "KONTO BEANTRAGUNGEN";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button annehmen;
        private System.Windows.Forms.Button ablehnen;
    }
}