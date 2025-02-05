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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLancamentos));
            groupBox1 = new GroupBox();
            dtpData2 = new DateTimePicker();
            cbbFiltrarValores = new ComboBox();
            nudValor2 = new NumericUpDown();
            cbbFiltrarDatas = new ComboBox();
            cbbFiltrar = new ComboBox();
            dtpData1 = new DateTimePicker();
            nudValor1 = new NumericUpDown();
            txtFiltrar = new TextBox();
            btnEditar = new Button();
            btnExcluir = new Button();
            btnCriar = new Button();
            dgvLancamentos = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Data = new DataGridViewTextBoxColumn();
            Valor = new DataGridViewTextBoxColumn();
            Conta = new DataGridViewTextBoxColumn();
            Descrição = new DataGridViewTextBoxColumn();
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
            groupBox1.Anchor = AnchorStyles.Bottom;
            groupBox1.Controls.Add(dtpData2);
            groupBox1.Controls.Add(cbbFiltrarValores);
            groupBox1.Controls.Add(nudValor2);
            groupBox1.Controls.Add(cbbFiltrarDatas);
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(dtpData1);
            groupBox1.Controls.Add(nudValor1);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox1.Location = new Point(524, 409);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 79);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
            // 
            // dtpData2
            // 
            dtpData2.Font = new Font("Lucida Sans", 10.5F);
            dtpData2.Format = DateTimePickerFormat.Short;
            dtpData2.Location = new Point(176, 49);
            dtpData2.Name = "dtpData2";
            dtpData2.Size = new Size(162, 24);
            dtpData2.TabIndex = 16;
            dtpData2.Visible = false;
            dtpData2.ValueChanged += dtpData2_ValueChanged;
            // 
            // cbbFiltrarValores
            // 
            cbbFiltrarValores.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrarValores.Font = new Font("Lucida Sans", 10.5F);
            cbbFiltrarValores.FormattingEnabled = true;
            cbbFiltrarValores.Items.AddRange(new object[] { "Sem filtro", "Somente débitos", "Somente créditos", "Valores iguais a", "Valores menores que", "Valores maiores que", "Valores entre" });
            cbbFiltrarValores.Location = new Point(176, 20);
            cbbFiltrarValores.Name = "cbbFiltrarValores";
            cbbFiltrarValores.Size = new Size(162, 24);
            cbbFiltrarValores.TabIndex = 13;
            cbbFiltrarValores.Visible = false;
            cbbFiltrarValores.SelectedIndexChanged += cbbFiltrarValores_SelectedIndexChanged;
            // 
            // nudValor2
            // 
            nudValor2.DecimalPlaces = 2;
            nudValor2.Font = new Font("Lucida Sans", 10.5F);
            nudValor2.Location = new Point(176, 49);
            nudValor2.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudValor2.Minimum = new decimal(new int[] { 999999999, 0, 0, -2147352576 });
            nudValor2.Name = "nudValor2";
            nudValor2.Size = new Size(162, 24);
            nudValor2.TabIndex = 18;
            nudValor2.ThousandsSeparator = true;
            nudValor2.Visible = false;
            nudValor2.ValueChanged += nudValor2_ValueChanged;
            // 
            // cbbFiltrarDatas
            // 
            cbbFiltrarDatas.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrarDatas.Font = new Font("Lucida Sans", 10.5F);
            cbbFiltrarDatas.FormattingEnabled = true;
            cbbFiltrarDatas.Items.AddRange(new object[] { "Sem filtro", "Data igual a", "Datas anteriores a", "Datas posteriores a", "Data entre" });
            cbbFiltrarDatas.Location = new Point(176, 20);
            cbbFiltrarDatas.Name = "cbbFiltrarDatas";
            cbbFiltrarDatas.Size = new Size(162, 24);
            cbbFiltrarDatas.TabIndex = 12;
            cbbFiltrarDatas.Visible = false;
            cbbFiltrarDatas.SelectedIndexChanged += cbbFiltrarDatas_SelectedIndexChanged;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.Font = new Font("Lucida Sans", 10.5F);
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Sem filtro", "Conta", "Descrição", "Histórico", "Data", "Valor" });
            cbbFiltrar.Location = new Point(6, 20);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(332, 24);
            cbbFiltrar.TabIndex = 11;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // dtpData1
            // 
            dtpData1.Font = new Font("Lucida Sans", 10.5F);
            dtpData1.Format = DateTimePickerFormat.Short;
            dtpData1.Location = new Point(6, 49);
            dtpData1.Name = "dtpData1";
            dtpData1.Size = new Size(332, 24);
            dtpData1.TabIndex = 15;
            dtpData1.Visible = false;
            dtpData1.ValueChanged += dtpData1_ValueChanged;
            // 
            // nudValor1
            // 
            nudValor1.DecimalPlaces = 2;
            nudValor1.Font = new Font("Lucida Sans", 10.5F);
            nudValor1.Location = new Point(6, 49);
            nudValor1.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudValor1.Minimum = new decimal(new int[] { 999999999, 0, 0, -2147352576 });
            nudValor1.Name = "nudValor1";
            nudValor1.Size = new Size(332, 24);
            nudValor1.TabIndex = 17;
            nudValor1.ThousandsSeparator = true;
            nudValor1.Visible = false;
            nudValor1.ValueChanged += nudValor1_ValueChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Font = new Font("Lucida Sans", 10.5F);
            txtFiltrar.Location = new Point(6, 49);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(332, 24);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // btnEditar
            // 
            btnEditar.Anchor = AnchorStyles.Bottom;
            btnEditar.Font = new Font("Lucida Sans", 10.5F);
            btnEditar.Location = new Point(12, 453);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(250, 35);
            btnEditar.TabIndex = 23;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Bottom;
            btnExcluir.Font = new Font("Lucida Sans", 10.5F);
            btnExcluir.Location = new Point(268, 416);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(250, 35);
            btnExcluir.TabIndex = 22;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnCriar
            // 
            btnCriar.Anchor = AnchorStyles.Bottom;
            btnCriar.Font = new Font("Lucida Sans", 10.5F);
            btnCriar.Location = new Point(12, 416);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(250, 35);
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
            dgvLancamentos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLancamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvLancamentos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvLancamentos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLancamentos.Columns.AddRange(new DataGridViewColumn[] { ID, Data, Valor, Conta, Descrição, ID_Historico, Histórico });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Lucida Sans", 10.5F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvLancamentos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvLancamentos.Location = new Point(12, 12);
            dgvLancamentos.MultiSelect = false;
            dgvLancamentos.Name = "dgvLancamentos";
            dgvLancamentos.ReadOnly = true;
            dgvLancamentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLancamentos.Size = new Size(856, 390);
            dgvLancamentos.TabIndex = 20;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            ID.DataPropertyName = "id";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ID.DefaultCellStyle = dataGridViewCellStyle2;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Visible = false;
            // 
            // Data
            // 
            Data.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Data.DataPropertyName = "data";
            Data.HeaderText = "Data";
            Data.Name = "Data";
            Data.ReadOnly = true;
            Data.Width = 66;
            // 
            // Valor
            // 
            Valor.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            Valor.DataPropertyName = "valor";
            Valor.HeaderText = "Valor";
            Valor.Name = "Valor";
            Valor.ReadOnly = true;
            Valor.Width = 5;
            // 
            // Conta
            // 
            Conta.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Conta.DataPropertyName = "conta";
            Conta.HeaderText = "Conta";
            Conta.Name = "Conta";
            Conta.ReadOnly = true;
            Conta.Width = 76;
            // 
            // Descrição
            // 
            Descrição.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Descrição.DataPropertyName = "descricao";
            Descrição.HeaderText = "Descrição";
            Descrição.Name = "Descrição";
            Descrição.ReadOnly = true;
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
            Histórico.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Histórico.DataPropertyName = "historico";
            Histórico.HeaderText = "Histórico";
            Histórico.Name = "Histórico";
            Histórico.ReadOnly = true;
            // 
            // btnCalcular
            // 
            btnCalcular.Anchor = AnchorStyles.Bottom;
            btnCalcular.Font = new Font("Lucida Sans", 10.5F);
            btnCalcular.Location = new Point(268, 453);
            btnCalcular.Name = "btnCalcular";
            btnCalcular.Size = new Size(250, 35);
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
            Controls.Add(btnEditar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvLancamentos);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnCriar;
        private DataGridView dgvLancamentos;
        private ComboBox cbbFiltrar;
        private Button btnCalcular;
        private ComboBox cbbFiltrarValores;
        private ComboBox cbbFiltrarDatas;
        private DateTimePicker dtpData2;
        private DateTimePicker dtpData1;
        private NumericUpDown nudValor1;
        private NumericUpDown nudValor2;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Data;
        private DataGridViewTextBoxColumn Valor;
        private DataGridViewTextBoxColumn Conta;
        private DataGridViewTextBoxColumn Descrição;
        private DataGridViewTextBoxColumn ID_Historico;
        private DataGridViewTextBoxColumn Histórico;
    }
}