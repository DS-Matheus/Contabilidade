namespace Contabilidade.Forms.Relatorios
{
    partial class frmSaldo
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            groupBox1 = new GroupBox();
            cbbNivel = new ComboBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            dgvContas = new DataGridView();
            Nível = new DataGridViewTextBoxColumn();
            Conta = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            btnVisualizar = new Button();
            label2 = new Label();
            dtpData = new DateTimePicker();
            label1 = new Label();
            txtConta = new TextBox();
            label3 = new Label();
            txtSubtitulo = new TextBox();
            chkCaixa = new CheckBox();
            chkSaldosZero = new CheckBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom;
            groupBox1.Controls.Add(cbbNivel);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Font = new Font("Lucida Sans", 11.5F);
            groupBox1.Location = new Point(12, 403);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 86);
            groupBox1.TabIndex = 46;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // cbbNivel
            // 
            cbbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNivel.Font = new Font("Lucida Sans", 10.5F);
            cbbNivel.FormattingEnabled = true;
            cbbNivel.Items.AddRange(new object[] { "Ambos", "Analítico", "Sintético" });
            cbbNivel.Location = new Point(6, 51);
            cbbNivel.Name = "cbbNivel";
            cbbNivel.Size = new Size(238, 24);
            cbbNivel.TabIndex = 19;
            cbbNivel.Visible = false;
            cbbNivel.SelectedIndexChanged += cbbNivel_SelectedIndexChanged;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.Font = new Font("Lucida Sans", 10.5F);
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Conta", "Descrição", "Nível" });
            cbbFiltrar.Location = new Point(6, 22);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(238, 24);
            cbbFiltrar.TabIndex = 9;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Font = new Font("Lucida Sans", 10.5F);
            txtFiltrar.Location = new Point(6, 51);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(238, 24);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // dgvContas
            // 
            dgvContas.AllowUserToAddRows = false;
            dgvContas.AllowUserToDeleteRows = false;
            dgvContas.AllowUserToOrderColumns = true;
            dgvContas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvContas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Lucida Sans", 10.5F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvContas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvContas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvContas.Columns.AddRange(new DataGridViewColumn[] { Nível, Conta, Descrição });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Lucida Sans", 10.5F);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvContas.DefaultCellStyle = dataGridViewCellStyle5;
            dgvContas.Location = new Point(12, 12);
            dgvContas.MultiSelect = false;
            dgvContas.Name = "dgvContas";
            dgvContas.ReadOnly = true;
            dgvContas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvContas.Size = new Size(856, 385);
            dgvContas.TabIndex = 45;
            dgvContas.CellDoubleClick += dgvContas_CellDoubleClick;
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
            Nível.Width = 65;
            // 
            // Conta
            // 
            Conta.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Conta.DataPropertyName = "conta";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Conta.DefaultCellStyle = dataGridViewCellStyle3;
            Conta.HeaderText = "Conta";
            Conta.Name = "Conta";
            Conta.ReadOnly = true;
            Conta.Width = 72;
            // 
            // Descrição
            // 
            Descrição.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Descrição.DataPropertyName = "descricao";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Descrição.DefaultCellStyle = dataGridViewCellStyle4;
            Descrição.HeaderText = "Descrição";
            Descrição.Name = "Descrição";
            Descrição.ReadOnly = true;
            // 
            // btnVisualizar
            // 
            btnVisualizar.Anchor = AnchorStyles.Bottom;
            btnVisualizar.Font = new Font("Lucida Sans", 10.5F);
            btnVisualizar.Location = new Point(693, 448);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(175, 41);
            btnVisualizar.TabIndex = 44;
            btnVisualizar.Text = "Visualizar relatório";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += btnVisualizar_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom;
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(423, 403);
            label2.Name = "label2";
            label2.Size = new Size(38, 16);
            label2.TabIndex = 38;
            label2.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Anchor = AnchorStyles.Bottom;
            dtpData.Font = new Font("Lucida Sans", 10.5F);
            dtpData.Format = DateTimePickerFormat.Short;
            dtpData.Location = new Point(423, 421);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(131, 24);
            dtpData.TabIndex = 36;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10.5F);
            label1.Location = new Point(268, 403);
            label1.Name = "label1";
            label1.Size = new Size(131, 16);
            label1.TabIndex = 35;
            label1.Text = "Conta Selecionada";
            // 
            // txtConta
            // 
            txtConta.Anchor = AnchorStyles.Bottom;
            txtConta.Enabled = false;
            txtConta.Font = new Font("Lucida Sans", 10.5F);
            txtConta.Location = new Point(268, 421);
            txtConta.Name = "txtConta";
            txtConta.Size = new Size(149, 24);
            txtConta.TabIndex = 34;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom;
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(268, 447);
            label3.Name = "label3";
            label3.Size = new Size(163, 16);
            label3.TabIndex = 48;
            label3.Text = "Subtítulo personalizado";
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Anchor = AnchorStyles.Bottom;
            txtSubtitulo.Font = new Font("Lucida Sans", 10.5F);
            txtSubtitulo.Location = new Point(268, 465);
            txtSubtitulo.MaxLength = 110;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(419, 24);
            txtSubtitulo.TabIndex = 47;
            // 
            // chkCaixa
            // 
            chkCaixa.Anchor = AnchorStyles.Bottom;
            chkCaixa.AutoSize = true;
            chkCaixa.Font = new Font("Lucida Sans", 10.5F);
            chkCaixa.Location = new Point(670, 402);
            chkCaixa.Name = "chkCaixa";
            chkCaixa.Size = new Size(163, 20);
            chkCaixa.TabIndex = 49;
            chkCaixa.Text = "Incluir valor do caixa";
            chkCaixa.UseVisualStyleBackColor = true;
            // 
            // chkSaldosZero
            // 
            chkSaldosZero.Anchor = AnchorStyles.Bottom;
            chkSaldosZero.AutoSize = true;
            chkSaldosZero.Font = new Font("Lucida Sans", 10.5F);
            chkSaldosZero.Location = new Point(670, 426);
            chkSaldosZero.Name = "chkSaldosZero";
            chkSaldosZero.Size = new Size(198, 20);
            chkSaldosZero.TabIndex = 50;
            chkSaldosZero.Text = "Incluir contas com saldo 0";
            chkSaldosZero.UseVisualStyleBackColor = true;
            // 
            // frmSaldo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(chkSaldosZero);
            Controls.Add(chkCaixa);
            Controls.Add(label3);
            Controls.Add(txtSubtitulo);
            Controls.Add(groupBox1);
            Controls.Add(dgvContas);
            Controls.Add(btnVisualizar);
            Controls.Add(label2);
            Controls.Add(dtpData);
            Controls.Add(label1);
            Controls.Add(txtConta);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmSaldo";
            Text = "Saldo";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbbNivel;
        private ComboBox cbbFiltrar;
        private TextBox txtFiltrar;
        private DataGridView dgvContas;
        private Button btnVisualizar;
        private Label label2;
        private DateTimePicker dtpData;
        private Label label1;
        private TextBox txtConta;
        private Label label3;
        private TextBox txtSubtitulo;
        private CheckBox chkCaixa;
        private CheckBox chkSaldosZero;
        private DataGridViewTextBoxColumn Nível;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Descrição;
    }
}