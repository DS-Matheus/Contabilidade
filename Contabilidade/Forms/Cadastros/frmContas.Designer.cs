namespace Contabilidade.Forms.Cadastros
{
    partial class frmContas
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
            dgvContas = new DataGridView();
            Conta = new DataGridViewTextBoxColumn();
            Nível = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            cbbNivel = new ComboBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            btnImprimir = new Button();
            btnEditar = new Button();
            btnExcluir = new Button();
            btnCriar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvContas).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
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
            dgvContas.Size = new Size(856, 390);
            dgvContas.TabIndex = 0;
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
            // groupBox1
            // 
            groupBox1.Controls.Add(cbbNivel);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Location = new Point(524, 403);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 85);
            groupBox1.TabIndex = 18;
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
            cbbNivel.Size = new Size(332, 23);
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
            cbbFiltrar.Size = new Size(332, 23);
            cbbFiltrar.TabIndex = 9;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Location = new Point(6, 51);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(332, 23);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(268, 453);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(250, 35);
            btnImprimir.TabIndex = 17;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(12, 453);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(250, 35);
            btnEditar.TabIndex = 16;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(268, 410);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(250, 35);
            btnExcluir.TabIndex = 15;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(12, 410);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(250, 35);
            btnCriar.TabIndex = 14;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // frmContas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(groupBox1);
            Controls.Add(btnImprimir);
            Controls.Add(btnEditar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvContas);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmContas";
            Text = "Contas";
            ((System.ComponentModel.ISupportInitialize)dgvContas).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvContas;
        private GroupBox groupBox1;
        private ComboBox cbbFiltrar;
        private TextBox txtFiltrar;
        private Button btnImprimir;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnCriar;
        private ComboBox cbbNivel;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Nível;
        private DataGridViewTextBoxColumn Descrição;
    }
}