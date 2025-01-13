namespace Contabilidade.Forms.Cadastros
{
    partial class frmUsuarios
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
            dgvUsuarios = new DataGridView();
            btnCriar = new Button();
            btnExcluir = new Button();
            btnEditar = new Button();
            cbbFiltrar = new ComboBox();
            txtFiltrar = new TextBox();
            btnImprimir = new Button();
            groupBox1 = new GroupBox();
            ID = new DataGridViewTextBoxColumn();
            Usuário = new DataGridViewTextBoxColumn();
            Senha = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToOrderColumns = true;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { ID, Usuário, Senha });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Lucida Sans", 10.5F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle3;
            dgvUsuarios.Location = new Point(12, 12);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(856, 385);
            dgvUsuarios.TabIndex = 0;
            // 
            // btnCriar
            // 
            btnCriar.Anchor = AnchorStyles.Bottom;
            btnCriar.Font = new Font("Lucida Sans", 10.5F);
            btnCriar.Location = new Point(12, 410);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(250, 35);
            btnCriar.TabIndex = 1;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Bottom;
            btnExcluir.Font = new Font("Lucida Sans", 10.5F);
            btnExcluir.Location = new Point(268, 410);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(250, 35);
            btnExcluir.TabIndex = 2;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnEditar
            // 
            btnEditar.Anchor = AnchorStyles.Bottom;
            btnEditar.Font = new Font("Lucida Sans", 10.5F);
            btnEditar.Location = new Point(12, 453);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(250, 35);
            btnEditar.TabIndex = 3;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbFiltrar.Font = new Font("Lucida Sans", 10.5F);
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Usuário", "Senha" });
            cbbFiltrar.Location = new Point(6, 22);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(332, 24);
            cbbFiltrar.TabIndex = 9;
            cbbFiltrar.SelectedIndexChanged += cbbFiltrar_SelectedIndexChanged;
            // 
            // txtFiltrar
            // 
            txtFiltrar.Font = new Font("Lucida Sans", 10.5F);
            txtFiltrar.Location = new Point(6, 51);
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(332, 24);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // btnImprimir
            // 
            btnImprimir.Anchor = AnchorStyles.Bottom;
            btnImprimir.Font = new Font("Lucida Sans", 10.5F);
            btnImprimir.Location = new Point(268, 453);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(250, 35);
            btnImprimir.TabIndex = 12;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom;
            groupBox1.Controls.Add(cbbFiltrar);
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox1.Location = new Point(524, 403);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 85);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por";
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
            // Usuário
            // 
            Usuário.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Usuário.DataPropertyName = "nome";
            Usuário.HeaderText = "Usuário";
            Usuário.Name = "Usuário";
            Usuário.ReadOnly = true;
            // 
            // Senha
            // 
            Senha.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Senha.DataPropertyName = "senha";
            Senha.HeaderText = "Senha";
            Senha.Name = "Senha";
            Senha.ReadOnly = true;
            // 
            // frmUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(groupBox1);
            Controls.Add(btnImprimir);
            Controls.Add(btnEditar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvUsuarios);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmUsuarios";
            Text = "Usuários";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dgvUsuarios;
        private Button btnCriar;
        private Button btnExcluir;
        private Button btnEditar;
        private ComboBox cbbFiltrar;
        private TextBox txtFiltrar;
        private Button btnImprimir;
        private GroupBox groupBox1;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Usuário;
        private DataGridViewTextBoxColumn Senha;
    }
}