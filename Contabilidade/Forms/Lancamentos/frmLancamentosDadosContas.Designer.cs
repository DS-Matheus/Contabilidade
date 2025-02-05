namespace Contabilidade.Forms.Lancamentos
{
    partial class frmLancamentosDadosContas
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLancamentosDadosContas));
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            lblTitulo = new Label();
            btnFechar = new Button();
            panel1 = new Panel();
            groupBox2 = new GroupBox();
            label1 = new Label();
            cbbNivel = new ComboBox();
            btnCriar = new Button();
            txtConta = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtDescricao = new TextBox();
            groupBox1 = new GroupBox();
            cbbNivel2 = new ComboBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            dgvContas = new DataGridView();
            Conta = new DataGridViewTextBoxColumn();
            Nível = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            pnlBarraTitulo.SuspendLayout();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).BeginInit();
            SuspendLayout();
            // 
            // pnlBarraTitulo
            // 
            pnlBarraTitulo.BackColor = Color.FromArgb(38, 38, 59);
            pnlBarraTitulo.Controls.Add(btnMinimizar);
            pnlBarraTitulo.Controls.Add(lblTitulo);
            pnlBarraTitulo.Controls.Add(btnFechar);
            pnlBarraTitulo.Dock = DockStyle.Top;
            pnlBarraTitulo.Location = new Point(0, 0);
            pnlBarraTitulo.Name = "pnlBarraTitulo";
            pnlBarraTitulo.Size = new Size(738, 30);
            pnlBarraTitulo.TabIndex = 7;
            pnlBarraTitulo.MouseDown += pnlBarraTitulo_MouseDown;
            // 
            // btnMinimizar
            // 
            btnMinimizar.Cursor = Cursors.Hand;
            btnMinimizar.Dock = DockStyle.Right;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMinimizar.ForeColor = Color.White;
            btnMinimizar.Location = new Point(678, 0);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(30, 30);
            btnMinimizar.TabIndex = 4;
            btnMinimizar.Text = "--";
            btnMinimizar.UseVisualStyleBackColor = true;
            btnMinimizar.Click += btnMinimizar_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Lucida Sans", 10F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(213, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Formulário de dados de contas";
            lblTitulo.MouseDown += lblTitulo_MouseDown;
            // 
            // btnFechar
            // 
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Dock = DockStyle.Right;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(708, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 5;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(dgvContas);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Lucida Sans", 10.5F);
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(738, 520);
            panel1.TabIndex = 8;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(cbbNivel);
            groupBox2.Controls.Add(btnCriar);
            groupBox2.Controls.Add(txtConta);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtDescricao);
            groupBox2.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox2.Location = new Point(28, 358);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(683, 143);
            groupBox2.TabIndex = 21;
            groupBox2.TabStop = false;
            groupBox2.Text = "Criar conta";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10.5F);
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(47, 16);
            label1.TabIndex = 1;
            label1.Text = "Conta";
            // 
            // cbbNivel
            // 
            cbbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNivel.Font = new Font("Lucida Sans", 10.5F);
            cbbNivel.FormattingEnabled = true;
            cbbNivel.Items.AddRange(new object[] { "Analítico", "Sintético" });
            cbbNivel.Location = new Point(221, 36);
            cbbNivel.Name = "cbbNivel";
            cbbNivel.Size = new Size(171, 24);
            cbbNivel.TabIndex = 20;
            // 
            // btnCriar
            // 
            btnCriar.Font = new Font("Lucida Sans", 10.5F);
            btnCriar.Location = new Point(556, 86);
            btnCriar.Margin = new Padding(3, 10, 3, 3);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(115, 46);
            btnCriar.TabIndex = 3;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // txtConta
            // 
            txtConta.Font = new Font("Lucida Sans", 10.5F);
            txtConta.Location = new Point(12, 36);
            txtConta.Margin = new Padding(25, 3, 3, 3);
            txtConta.MaxLength = 15;
            txtConta.Name = "txtConta";
            txtConta.Size = new Size(171, 24);
            txtConta.TabIndex = 1;
            txtConta.TextChanged += txtConta_TextChanged;
            txtConta.KeyPress += txtConta_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(12, 68);
            label2.Name = "label2";
            label2.Size = new Size(73, 16);
            label2.TabIndex = 3;
            label2.Text = "Descrição";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(221, 18);
            label3.Name = "label3";
            label3.Size = new Size(40, 16);
            label3.TabIndex = 5;
            label3.Text = "Nível";
            // 
            // txtDescricao
            // 
            txtDescricao.Font = new Font("Lucida Sans", 10.5F);
            txtDescricao.Location = new Point(12, 86);
            txtDescricao.MaxLength = 100;
            txtDescricao.Multiline = true;
            txtDescricao.Name = "txtDescricao";
            txtDescricao.Size = new Size(538, 46);
            txtDescricao.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbbNivel2);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox1.Location = new Point(28, 267);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(380, 85);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // cbbNivel2
            // 
            cbbNivel2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNivel2.Font = new Font("Lucida Sans", 10.5F);
            cbbNivel2.FormattingEnabled = true;
            cbbNivel2.Items.AddRange(new object[] { "Ambos", "Analítico", "Sintético" });
            cbbNivel2.Location = new Point(6, 51);
            cbbNivel2.Name = "cbbNivel2";
            cbbNivel2.Size = new Size(368, 24);
            cbbNivel2.TabIndex = 19;
            cbbNivel2.Visible = false;
            cbbNivel2.SelectedIndexChanged += cbbNivel2_SelectedIndexChanged;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.Font = new Font("Lucida Sans", 10.5F);
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Conta", "Descrição", "Nível" });
            cbbFiltrar.Location = new Point(6, 22);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(368, 24);
            cbbFiltrar.TabIndex = 9;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Font = new Font("Lucida Sans", 10.5F);
            txtFiltrar.Location = new Point(6, 51);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(368, 24);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // dgvContas
            // 
            dgvContas.AllowUserToAddRows = false;
            dgvContas.AllowUserToDeleteRows = false;
            dgvContas.AllowUserToOrderColumns = true;
            dgvContas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvContas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvContas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvContas.Columns.AddRange(new DataGridViewColumn[] { Conta, Nível, Descrição });
            dgvContas.Location = new Point(28, 20);
            dgvContas.MultiSelect = false;
            dgvContas.Name = "dgvContas";
            dgvContas.ReadOnly = true;
            dgvContas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvContas.Size = new Size(683, 241);
            dgvContas.TabIndex = 7;
            dgvContas.CellDoubleClick += dgvContas_CellDoubleClick;
            // 
            // Conta
            // 
            Conta.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Conta.DataPropertyName = "conta";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Conta.DefaultCellStyle = dataGridViewCellStyle2;
            Conta.HeaderText = "Conta";
            Conta.Name = "Conta";
            Conta.ReadOnly = true;
            Conta.Width = 76;
            // 
            // Nível
            // 
            Nível.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Nível.DataPropertyName = "nivel";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Nível.DefaultCellStyle = dataGridViewCellStyle3;
            Nível.HeaderText = "Nível";
            Nível.Name = "Nível";
            Nível.ReadOnly = true;
            Nível.Width = 68;
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
            // frmLancamentosDadosContas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(738, 550);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmLancamentosDadosContas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmLancamentosDadosContas";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            panel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvContas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private Panel panel1;
        private Label label3;
        private TextBox txtDescricao;
        private Label label2;
        private TextBox txtConta;
        private Label label1;
        private Button btnCriar;
        private DataGridView dgvContas;
        private GroupBox groupBox1;
        private ComboBox cbbNivel2;
        private ComboBox cbbFiltrar;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Nível;
        private DataGridViewTextBoxColumn Descrição;
        private TextBox txtFiltrar;
        private ComboBox cbbNivel;
        private GroupBox groupBox2;
    }
}