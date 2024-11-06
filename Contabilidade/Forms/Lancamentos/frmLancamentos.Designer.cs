namespace Contabilidade.Forms.Lancamentos
{
    partial class frmLancamentos
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
            groupBox1 = new GroupBox();
            nudValor2 = new NumericUpDown();
            nudValor1 = new NumericUpDown();
            dtpData2 = new DateTimePicker();
            dtpData1 = new DateTimePicker();
            cbbFiltrarValores = new ComboBox();
            cbbFiltrarDatas = new ComboBox();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            btnImprimir = new Button();
            btnEditar = new Button();
            btnExcluir = new Button();
            btnCriar = new Button();
            dgvLancamentos = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Data = new DataGridViewTextBoxColumn();
            Conta = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
            Valor = new DataGridViewTextBoxColumn();
            ID_Historico = new DataGridViewTextBoxColumn();
            Histórico = new DataGridViewTextBoxColumn();
            btnCalcular = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudValor2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudValor1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLancamentos).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dtpData2);
            groupBox1.Controls.Add(cbbFiltrarValores);
            groupBox1.Controls.Add(nudValor2);
            groupBox1.Controls.Add(cbbFiltrarDatas);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(dtpData1);
            groupBox1.Controls.Add(nudValor1);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Location = new Point(524, 409);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 79);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // nudValor2
            // 
            nudValor2.DecimalPlaces = 2;
            nudValor2.Location = new Point(176, 49);
            nudValor2.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudValor2.Minimum = new decimal(new int[] { 999999999, 0, 0, -2147352576 });
            nudValor2.Name = "nudValor2";
            nudValor2.Size = new Size(162, 23);
            nudValor2.TabIndex = 18;
            nudValor2.ThousandsSeparator = true;
            nudValor2.Visible = false;
            nudValor2.ValueChanged += nudValor2_ValueChanged;
            // 
            // nudValor1
            // 
            nudValor1.DecimalPlaces = 2;
            nudValor1.Location = new Point(6, 49);
            nudValor1.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudValor1.Minimum = new decimal(new int[] { 999999999, 0, 0, -2147352576 });
            nudValor1.Name = "nudValor1";
            nudValor1.Size = new Size(332, 23);
            nudValor1.TabIndex = 17;
            nudValor1.ThousandsSeparator = true;
            nudValor1.Visible = false;
            nudValor1.ValueChanged += nudValor1_ValueChanged;
            // 
            // dtpData2
            // 
            dtpData2.Format = DateTimePickerFormat.Short;
            dtpData2.Location = new Point(176, 49);
            dtpData2.Name = "dtpData2";
            dtpData2.Size = new Size(162, 23);
            dtpData2.TabIndex = 16;
            dtpData2.Visible = false;
            dtpData2.ValueChanged += dtpData2_ValueChanged;
            // 
            // dtpData1
            // 
            dtpData1.Format = DateTimePickerFormat.Short;
            dtpData1.Location = new Point(6, 49);
            dtpData1.Name = "dtpData1";
            dtpData1.Size = new Size(332, 23);
            dtpData1.TabIndex = 15;
            dtpData1.Visible = false;
            dtpData1.ValueChanged += dtpData1_ValueChanged;
            // 
            // cbbFiltrarValores
            // 
            cbbFiltrarValores.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrarValores.FormattingEnabled = true;
            cbbFiltrarValores.Items.AddRange(new object[] { "Sem filtro", "Somente débitos", "Somente créditos", "Valores iguais a", "Valores menores que", "Valores maiores que", "Valores entre" });
            cbbFiltrarValores.Location = new Point(176, 20);
            cbbFiltrarValores.Name = "cbbFiltrarValores";
            cbbFiltrarValores.Size = new Size(162, 23);
            cbbFiltrarValores.TabIndex = 13;
            cbbFiltrarValores.Visible = false;
            cbbFiltrarValores.SelectedIndexChanged += cbbFiltrarValores_SelectedIndexChanged;
            // 
            // cbbFiltrarDatas
            // 
            cbbFiltrarDatas.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrarDatas.FormattingEnabled = true;
            cbbFiltrarDatas.Items.AddRange(new object[] { "Sem filtro", "Data igual a", "Datas anteriores a", "Datas posteriores a", "Data entre" });
            cbbFiltrarDatas.Location = new Point(176, 20);
            cbbFiltrarDatas.Name = "cbbFiltrarDatas";
            cbbFiltrarDatas.Size = new Size(162, 23);
            cbbFiltrarDatas.TabIndex = 12;
            cbbFiltrarDatas.Visible = false;
            cbbFiltrarDatas.SelectedIndexChanged += cbbFiltrarDatas_SelectedIndexChanged;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Sem filtro", "Conta", "Descrição", "Histórico", "Data", "Valor" });
            cbbFiltrar.Location = new Point(6, 20);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(332, 23);
            cbbFiltrar.TabIndex = 11;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Location = new Point(6, 49);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(332, 23);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(268, 453);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(250, 30);
            btnImprimir.TabIndex = 24;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(12, 453);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(250, 30);
            btnEditar.TabIndex = 23;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(268, 419);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(120, 30);
            btnExcluir.TabIndex = 22;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(12, 419);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(250, 30);
            btnCriar.TabIndex = 21;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // dgvLancamentos
            // 
            dgvLancamentos.AllowUserToAddRows = false;
            dgvLancamentos.AllowUserToDeleteRows = false;
            dgvLancamentos.AllowUserToOrderColumns = true;
            dgvLancamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLancamentos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLancamentos.Columns.AddRange(new DataGridViewColumn[] { ID, Data, Conta, Descrição, Valor, ID_Historico, Histórico });
            dgvLancamentos.Location = new Point(12, 12);
            dgvLancamentos.MultiSelect = false;
            dgvLancamentos.Name = "dgvLancamentos";
            dgvLancamentos.ReadOnly = true;
            dgvLancamentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLancamentos.Size = new Size(856, 391);
            dgvLancamentos.TabIndex = 20;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            ID.DataPropertyName = "id";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ID.DefaultCellStyle = dataGridViewCellStyle1;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Visible = false;
            // 
            // Data
            // 
            Data.DataPropertyName = "data";
            Data.HeaderText = "Data";
            Data.Name = "Data";
            Data.ReadOnly = true;
            // 
            // Conta
            // 
            Conta.DataPropertyName = "conta";
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
            // ID_Historico
            // 
            ID_Historico.DataPropertyName = "id_historico";
            ID_Historico.HeaderText = "ID_Historico";
            ID_Historico.Name = "ID_Historico";
            ID_Historico.ReadOnly = true;
            ID_Historico.Visible = false;
            // 
            // Histórico
            // 
            Histórico.DataPropertyName = "historico";
            Histórico.HeaderText = "Histórico";
            Histórico.Name = "Histórico";
            Histórico.ReadOnly = true;
            // 
            // btnCalcular
            // 
            btnCalcular.Location = new Point(398, 419);
            btnCalcular.Name = "btnCalcular";
            btnCalcular.Size = new Size(120, 30);
            btnCalcular.TabIndex = 26;
            btnCalcular.Text = "Calcular";
            btnCalcular.UseVisualStyleBackColor = true;
            btnCalcular.Click += btnCalcular_Click;
            // 
            // frmLancamentos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(btnCalcular);
            Controls.Add(groupBox1);
            Controls.Add(btnImprimir);
            Controls.Add(btnEditar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvLancamentos);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLancamentos";
            Text = "Movimentação";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudValor2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudValor1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLancamentos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtFiltrar;
        private Button btnImprimir;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnCriar;
        private DataGridView dgvLancamentos;
        private ComboBox cbbFiltrar;
        private Button btnCalcular;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Data;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Descrição;
        private DataGridViewTextBoxColumn Valor;
        private DataGridViewTextBoxColumn ID_Historico;
        private DataGridViewTextBoxColumn Histórico;
        private ComboBox cbbFiltrarValores;
        private ComboBox cbbFiltrarDatas;
        private DateTimePicker dtpData2;
        private DateTimePicker dtpData1;
        private NumericUpDown nudValor1;
        private NumericUpDown nudValor2;
    }
}