namespace Contabilidade.Forms.Lancamentos
{
    partial class frmLancamentosDadosHistorico
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            lblTitulo = new Label();
            btnFechar = new Button();
            panel1 = new Panel();
            groupBox2 = new GroupBox();
            btnCriar = new Button();
            txtHistorico = new TextBox();
            groupBox1 = new GroupBox();
            txtFiltrar = new TextBox();
            dgvHistoricos = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Histórico = new DataGridViewTextBoxColumn();
            pnlBarraTitulo.SuspendLayout();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistoricos).BeginInit();
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
            pnlBarraTitulo.Size = new Size(720, 30);
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
            btnMinimizar.Location = new Point(660, 0);
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
            lblTitulo.Font = new Font("Lucida Sans", 10.5F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(232, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Formulário de dados de históricos";
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
            btnFechar.Location = new Point(690, 0);
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
            panel1.Controls.Add(dgvHistoricos);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(720, 584);
            panel1.TabIndex = 8;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnCriar);
            groupBox2.Controls.Add(txtHistorico);
            groupBox2.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox2.Location = new Point(31, 457);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(659, 115);
            groupBox2.TabIndex = 21;
            groupBox2.TabStop = false;
            groupBox2.Text = "Criar histórico";
            // 
            // btnCriar
            // 
            btnCriar.Font = new Font("Lucida Sans", 10.5F);
            btnCriar.Location = new Point(563, 43);
            btnCriar.Margin = new Padding(3, 10, 3, 3);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(90, 39);
            btnCriar.TabIndex = 6;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // txtHistorico
            // 
            txtHistorico.Font = new Font("Lucida Sans", 10.5F);
            txtHistorico.Location = new Point(6, 23);
            txtHistorico.MaxLength = 300;
            txtHistorico.Multiline = true;
            txtHistorico.Name = "txtHistorico";
            txtHistorico.Size = new Size(551, 78);
            txtHistorico.TabIndex = 4;
            txtHistorico.KeyPress += txtHistorico_KeyPress;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox1.Location = new Point(31, 365);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(659, 86);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por histórico";
            // 
            // txtFiltrar
            // 
            txtFiltrar.Font = new Font("Lucida Sans", 10.5F);
            txtFiltrar.Location = new Point(6, 20);
            txtFiltrar.MaxLength = 300;
            txtFiltrar.Multiline = true;
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(647, 56);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            txtFiltrar.KeyPress += txtFiltrar_KeyPress;
            // 
            // dgvHistoricos
            // 
            dgvHistoricos.AllowUserToAddRows = false;
            dgvHistoricos.AllowUserToDeleteRows = false;
            dgvHistoricos.AllowUserToOrderColumns = true;
            dgvHistoricos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvHistoricos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvHistoricos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistoricos.Columns.AddRange(new DataGridViewColumn[] { ID, Histórico });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Lucida Sans", 10.5F);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvHistoricos.DefaultCellStyle = dataGridViewCellStyle8;
            dgvHistoricos.Location = new Point(31, 18);
            dgvHistoricos.MultiSelect = false;
            dgvHistoricos.Name = "dgvHistoricos";
            dgvHistoricos.ReadOnly = true;
            dgvHistoricos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistoricos.Size = new Size(659, 341);
            dgvHistoricos.TabIndex = 15;
            dgvHistoricos.CellDoubleClick += dgvHistoricos_CellDoubleClick;
            dgvHistoricos.CellMouseClick += dgvHistoricos_CellMouseClick;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            ID.DataPropertyName = "id";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ID.DefaultCellStyle = dataGridViewCellStyle6;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Visible = false;
            // 
            // Histórico
            // 
            Histórico.DataPropertyName = "historico";
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Histórico.DefaultCellStyle = dataGridViewCellStyle7;
            Histórico.HeaderText = "Histórico";
            Histórico.Name = "Histórico";
            Histórico.ReadOnly = true;
            // 
            // frmLancamentosDadosHistorico
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 614);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLancamentosDadosHistorico";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmLancamentosDadosHistorico";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            panel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistoricos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private Panel panel1;
        private Button btnCriar;
        private TextBox txtHistorico;
        private DataGridView dgvHistoricos;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Histórico;
        private GroupBox groupBox1;
        private TextBox txtFiltrar;
        private GroupBox groupBox2;
    }
}