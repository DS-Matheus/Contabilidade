﻿namespace Contabilidade.Forms.Relatorios
{
    partial class frmBalanceteConta
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            btnVisualizar = new Button();
            groupBox1 = new GroupBox();
            cbbNivel = new ComboBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            dgvContas = new DataGridView();
            Conta = new DataGridViewTextBoxColumn();
            Nível = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            label4 = new Label();
            txtSubtitulo = new TextBox();
            label3 = new Label();
            label2 = new Label();
            dtpFinal = new DateTimePicker();
            dtpInicial = new DateTimePicker();
            label1 = new Label();
            txtConta = new TextBox();
            chkSaldosZero = new CheckBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).BeginInit();
            SuspendLayout();
            // 
            // btnVisualizar
            // 
            btnVisualizar.Location = new Point(748, 452);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(120, 36);
            btnVisualizar.TabIndex = 10;
            btnVisualizar.Text = "Visualizar relatório";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += btnVisualizar_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbbNivel);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Location = new Point(12, 403);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 85);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // cbbNivel
            // 
            cbbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNivel.FormattingEnabled = true;
            cbbNivel.Items.AddRange(new object[] { "Ambos", "Analítico", "Sintético" });
            cbbNivel.Location = new Point(6, 51);
            cbbNivel.Name = "cbbNivel";
            cbbNivel.Size = new Size(238, 23);
            cbbNivel.TabIndex = 19;
            cbbNivel.Visible = false;
            cbbNivel.SelectedIndexChanged += cbbNivel_SelectedIndexChanged;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Conta", "Descrição", "Nível" });
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
            dgvContas.Columns.AddRange(new DataGridViewColumn[] { Conta, Nível, Descrição });
            dgvContas.Location = new Point(12, 12);
            dgvContas.MultiSelect = false;
            dgvContas.Name = "dgvContas";
            dgvContas.ReadOnly = true;
            dgvContas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvContas.Size = new Size(856, 385);
            dgvContas.TabIndex = 19;
            dgvContas.CellDoubleClick += dgvContas_CellDoubleClick;
            // 
            // Conta
            // 
            Conta.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Conta.DataPropertyName = "conta";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Conta.DefaultCellStyle = dataGridViewCellStyle1;
            Conta.HeaderText = "Conta";
            Conta.Name = "Conta";
            Conta.ReadOnly = true;
            Conta.Width = 64;
            // 
            // Nível
            // 
            Nível.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Nível.DataPropertyName = "nivel";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Nível.DefaultCellStyle = dataGridViewCellStyle2;
            Nível.HeaderText = "Nível";
            Nível.Name = "Nível";
            Nível.ReadOnly = true;
            Nível.Width = 59;
            // 
            // Descrição
            // 
            Descrição.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Descrição.DataPropertyName = "descricao";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Descrição.DefaultCellStyle = dataGridViewCellStyle3;
            Descrição.HeaderText = "Descrição";
            Descrição.Name = "Descrição";
            Descrição.ReadOnly = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(423, 403);
            label4.Name = "label4";
            label4.Size = new Size(131, 15);
            label4.TabIndex = 58;
            label4.Text = "Subtítulo personalizado";
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Location = new Point(423, 421);
            txtSubtitulo.MaxLength = 110;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(445, 23);
            txtSubtitulo.TabIndex = 57;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(423, 447);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 56;
            label3.Text = "Data Final";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(268, 447);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 55;
            label2.Text = "Data Inicial";
            // 
            // dtpFinal
            // 
            dtpFinal.Format = DateTimePickerFormat.Short;
            dtpFinal.Location = new Point(423, 465);
            dtpFinal.Name = "dtpFinal";
            dtpFinal.Size = new Size(149, 23);
            dtpFinal.TabIndex = 54;
            // 
            // dtpInicial
            // 
            dtpInicial.Format = DateTimePickerFormat.Short;
            dtpInicial.Location = new Point(268, 465);
            dtpInicial.Name = "dtpInicial";
            dtpInicial.Size = new Size(149, 23);
            dtpInicial.TabIndex = 53;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(268, 403);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 52;
            label1.Text = "Conta Selecionada";
            // 
            // txtConta
            // 
            txtConta.Enabled = false;
            txtConta.Location = new Point(268, 421);
            txtConta.Name = "txtConta";
            txtConta.Size = new Size(149, 23);
            txtConta.TabIndex = 51;
            // 
            // chkSaldosZero
            // 
            chkSaldosZero.AutoSize = true;
            chkSaldosZero.Location = new Point(578, 469);
            chkSaldosZero.Name = "chkSaldosZero";
            chkSaldosZero.Size = new Size(164, 19);
            chkSaldosZero.TabIndex = 59;
            chkSaldosZero.Text = "Incluir contas com saldo 0";
            chkSaldosZero.UseVisualStyleBackColor = true;
            // 
            // frmBalanceteConta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(chkSaldosZero);
            Controls.Add(label4);
            Controls.Add(txtSubtitulo);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dtpFinal);
            Controls.Add(dtpInicial);
            Controls.Add(label1);
            Controls.Add(txtConta);
            Controls.Add(groupBox1);
            Controls.Add(dgvContas);
            Controls.Add(btnVisualizar);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmBalanceteConta";
            Text = "Balancete de Conta";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnVisualizar;
        private GroupBox groupBox1;
        private ComboBox cbbNivel;
        private ComboBox cbbFiltrar;
        private TextBox txtFiltrar;
        private DataGridView dgvContas;
        private Label label4;
        private TextBox txtSubtitulo;
        private Label label3;
        private Label label2;
        private DateTimePicker dtpFinal;
        private DateTimePicker dtpInicial;
        private Label label1;
        private TextBox txtConta;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Nível;
        private DataGridViewTextBoxColumn Descrição;
        private CheckBox chkSaldosZero;
    }
}