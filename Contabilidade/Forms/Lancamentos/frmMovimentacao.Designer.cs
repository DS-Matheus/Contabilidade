namespace Contabilidade.Forms.Lancamentos
{
    partial class frmMovimentacao
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            groupBox1 = new GroupBox();
            txtFiltrar2 = new TextBox();
            cbbFiltrar2 = new ComboBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            btnImprimir = new Button();
            btnEditar = new Button();
            btnExcluir = new Button();
            btnCriar = new Button();
            dgvMovimentacao = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Conta = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            Valor = new DataGridViewTextBoxColumn();
            Data = new DataGridViewTextBoxColumn();
            Histórico = new DataGridViewTextBoxColumn();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimentacao).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtFiltrar2);
            groupBox1.Controls.Add(cbbFiltrar2);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Location = new Point(524, 409);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 79);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // txtFiltrar2
            // 
            txtFiltrar2.Location = new Point(178, 49);
            txtFiltrar2.Name = "txtFiltrar2";
            txtFiltrar2.Size = new Size(160, 23);
            txtFiltrar2.TabIndex = 13;
            // 
            // cbbFiltrar2
            // 
            cbbFiltrar2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar2.FormattingEnabled = true;
            cbbFiltrar2.Items.AddRange(new object[] { "menor que", "maior que", "entre" });
            cbbFiltrar2.Location = new Point(178, 20);
            cbbFiltrar2.Name = "cbbFiltrar2";
            cbbFiltrar2.Size = new Size(160, 23);
            cbbFiltrar2.TabIndex = 12;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Conta", "Descrição", "Valor", "Débito", "Crédito", "Data", "Histórico" });
            cbbFiltrar.Location = new Point(6, 20);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(332, 23);
            cbbFiltrar.TabIndex = 11;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Location = new Point(6, 49);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(332, 23);
            txtFiltrar.TabIndex = 10;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(268, 453);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(250, 30);
            btnImprimir.TabIndex = 24;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(12, 453);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(250, 30);
            btnEditar.TabIndex = 23;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(268, 419);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(250, 30);
            btnExcluir.TabIndex = 22;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(12, 419);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(250, 30);
            btnCriar.TabIndex = 21;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            // 
            // dgvMovimentacao
            // 
            dgvMovimentacao.AllowUserToAddRows = false;
            dgvMovimentacao.AllowUserToDeleteRows = false;
            dgvMovimentacao.AllowUserToOrderColumns = true;
            dgvMovimentacao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovimentacao.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMovimentacao.Columns.AddRange(new DataGridViewColumn[] { ID, Conta, Descrição, Valor, Data, Histórico });
            dgvMovimentacao.Location = new Point(12, 12);
            dgvMovimentacao.MultiSelect = false;
            dgvMovimentacao.Name = "dgvMovimentacao";
            dgvMovimentacao.ReadOnly = true;
            dgvMovimentacao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovimentacao.Size = new Size(856, 391);
            dgvMovimentacao.TabIndex = 20;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            ID.DataPropertyName = "id";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ID.DefaultCellStyle = dataGridViewCellStyle2;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Width = 43;
            // 
            // Conta
            // 
            Conta.DataPropertyName = "fk_conta";
            Conta.HeaderText = "Conta";
            Conta.Name = "Conta";
            Conta.ReadOnly = true;
            // 
            // Descrição
            // 
            Descrição.DataPropertyName = "descricao";
            Descrição.HeaderText = "Descrição";
            Descrição.Name = "Descrição";
            Descrição.ReadOnly = true;
            // 
            // Valor
            // 
            Valor.DataPropertyName = "valor";
            Valor.HeaderText = "Valor";
            Valor.Name = "Valor";
            Valor.ReadOnly = true;
            // 
            // Data
            // 
            Data.DataPropertyName = "data";
            Data.HeaderText = "Data";
            Data.Name = "Data";
            Data.ReadOnly = true;
            // 
            // Histórico
            // 
            Histórico.DataPropertyName = "historico";
            Histórico.HeaderText = "Histórico";
            Histórico.Name = "Histórico";
            Histórico.ReadOnly = true;
            // 
            // frmMovimentacao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(groupBox1);
            Controls.Add(btnImprimir);
            Controls.Add(btnEditar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvMovimentacao);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmMovimentacao";
            Text = "Movimentação";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimentacao).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtFiltrar;
        private Button btnImprimir;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnCriar;
        private DataGridView dgvMovimentacao;
        private ComboBox cbbFiltrar;
        private TextBox txtFiltrar2;
        private ComboBox cbbFiltrar2;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Descrição;
        private DataGridViewTextBoxColumn Valor;
        private DataGridViewTextBoxColumn Data;
        private DataGridViewTextBoxColumn Histórico;
    }
}