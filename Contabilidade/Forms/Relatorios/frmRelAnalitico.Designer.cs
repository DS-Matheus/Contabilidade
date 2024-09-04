namespace Contabilidade.Forms.Relatorios
{
    partial class frmRelAnalitico
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
            DataGridViewCellStyle dataGridViewCellStyle25 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle26 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle27 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle28 = new DataGridViewCellStyle();
            groupBox1 = new GroupBox();
            cbbNivel = new ComboBox();
            txtFiltrar2 = new TextBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            dgvContas = new DataGridView();
            Nível = new DataGridViewTextBoxColumn();
            Conta = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            Saldo = new DataGridViewTextBoxColumn();
            button1 = new Button();
            label3 = new Label();
            label2 = new Label();
            dtpFinal = new DateTimePicker();
            dtpInicial = new DateTimePicker();
            label1 = new Label();
            txtConta = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbbNivel);
            groupBox1.Controls.Add(txtFiltrar2);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Location = new Point(12, 403);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 85);
            groupBox1.TabIndex = 33;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // cbbNivel
            // 
            cbbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNivel.FormattingEnabled = true;
            cbbNivel.Items.AddRange(new object[] { "Analítico", "Sintético", "Ambos" });
            cbbNivel.Location = new Point(6, 51);
            cbbNivel.Name = "cbbNivel";
            cbbNivel.Size = new Size(238, 23);
            cbbNivel.TabIndex = 19;
            cbbNivel.Visible = false;
            // 
            // txtFiltrar2
            // 
            txtFiltrar2.Location = new Point(129, 51);
            txtFiltrar2.Name = "txtFiltrar2";
            txtFiltrar2.Size = new Size(115, 23);
            txtFiltrar2.TabIndex = 11;
            txtFiltrar2.Visible = false;
            txtFiltrar2.TextChanged += txtFiltrar2_TextChanged;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Conta", "Descrição", "Nível", "Saldo menor que", "Saldo maior que", "Saldo entre" });
            cbbFiltrar.Location = new Point(6, 22);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(238, 23);
            cbbFiltrar.TabIndex = 9;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Location = new Point(6, 51);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(238, 23);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // dgvContas
            // 
            dgvContas.AllowUserToAddRows = false;
            dgvContas.AllowUserToDeleteRows = false;
            dgvContas.AllowUserToOrderColumns = true;
            dgvContas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvContas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvContas.Columns.AddRange(new DataGridViewColumn[] { Nível, Conta, Descrição, Saldo });
            dgvContas.Location = new Point(12, 12);
            dgvContas.MultiSelect = false;
            dgvContas.Name = "dgvContas";
            dgvContas.ReadOnly = true;
            dgvContas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvContas.Size = new Size(856, 385);
            dgvContas.TabIndex = 32;
            dgvContas.CellDoubleClick += dgvContas_CellDoubleClick;
            // 
            // Nível
            // 
            Nível.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Nível.DataPropertyName = "nivel";
            dataGridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Nível.DefaultCellStyle = dataGridViewCellStyle25;
            Nível.HeaderText = "Nível";
            Nível.Name = "Nível";
            Nível.ReadOnly = true;
            Nível.Width = 59;
            // 
            // Conta
            // 
            Conta.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Conta.DataPropertyName = "conta";
            dataGridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Conta.DefaultCellStyle = dataGridViewCellStyle26;
            Conta.HeaderText = "Conta";
            Conta.Name = "Conta";
            Conta.ReadOnly = true;
            Conta.Width = 64;
            // 
            // Descrição
            // 
            Descrição.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Descrição.DataPropertyName = "descricao";
            dataGridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Descrição.DefaultCellStyle = dataGridViewCellStyle27;
            Descrição.HeaderText = "Descrição";
            Descrição.Name = "Descrição";
            Descrição.ReadOnly = true;
            // 
            // Saldo
            // 
            Saldo.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Saldo.DataPropertyName = "saldo";
            dataGridViewCellStyle28.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Saldo.DefaultCellStyle = dataGridViewCellStyle28;
            Saldo.HeaderText = "Saldo";
            Saldo.Name = "Saldo";
            Saldo.ReadOnly = true;
            Saldo.Width = 61;
            // 
            // button1
            // 
            button1.Location = new Point(658, 452);
            button1.Name = "button1";
            button1.Size = new Size(210, 36);
            button1.TabIndex = 31;
            button1.Text = "Visualizar relatório";
            button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(423, 447);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 26;
            label3.Text = "Data Final";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(423, 403);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 25;
            label2.Text = "Data Inicial";
            // 
            // dtpFinal
            // 
            dtpFinal.Location = new Point(423, 465);
            dtpFinal.Name = "dtpFinal";
            dtpFinal.Size = new Size(229, 23);
            dtpFinal.TabIndex = 24;
            // 
            // dtpInicial
            // 
            dtpInicial.Location = new Point(423, 421);
            dtpInicial.Name = "dtpInicial";
            dtpInicial.Size = new Size(229, 23);
            dtpInicial.TabIndex = 23;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(268, 403);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 22;
            label1.Text = "Conta Selecionada";
            // 
            // txtConta
            // 
            txtConta.Enabled = false;
            txtConta.Location = new Point(268, 421);
            txtConta.Name = "txtConta";
            txtConta.Size = new Size(149, 23);
            txtConta.TabIndex = 21;
            // 
            // frmRelAnalitico
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(groupBox1);
            Controls.Add(dgvContas);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dtpFinal);
            Controls.Add(dtpInicial);
            Controls.Add(label1);
            Controls.Add(txtConta);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmRelAnalitico";
            Text = "Relatório Analítico";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbbNivel;
        private TextBox txtFiltrar2;
        private ComboBox cbbFiltrar;
        private TextBox txtFiltrar;
        private DataGridView dgvContas;
        private DataGridViewTextBoxColumn Nível;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Descrição;
        private DataGridViewTextBoxColumn Saldo;
        private Button button1;
        private Label label3;
        private Label label2;
        private DateTimePicker dtpFinal;
        private DateTimePicker dtpInicial;
        private Label label1;
        private TextBox txtConta;
    }
}