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
            dgvUsuarios = new DataGridView();
            btnCriar = new Button();
            btnExcluir = new Button();
            btnEditar = new Button();
            btnFiltrar = new Button();
            txtUsuario = new TextBox();
            label1 = new Label();
            txtSenha = new TextBox();
            label2 = new Label();
            cbbFiltrar = new ComboBox();
            nome = new DataGridViewTextBoxColumn();
            senha = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToOrderColumns = true;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { nome, senha });
            dgvUsuarios.Location = new Point(12, 12);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(596, 437);
            dgvUsuarios.TabIndex = 0;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(614, 139);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(236, 30);
            btnCriar.TabIndex = 1;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(614, 175);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(236, 30);
            btnExcluir.TabIndex = 2;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(614, 211);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(236, 30);
            btnEditar.TabIndex = 3;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Location = new Point(614, 312);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(236, 30);
            btnFiltrar.TabIndex = 4;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(614, 30);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(236, 23);
            txtUsuario.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(614, 12);
            label1.Name = "label1";
            label1.Size = new Size(47, 15);
            label1.TabIndex = 6;
            label1.Text = "Usuário";
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(614, 74);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(236, 23);
            txtSenha.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(614, 56);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 8;
            label2.Text = "Senha";
            // 
            // cbbFiltrar
            // 
            cbbFiltrar.FormattingEnabled = true;
            cbbFiltrar.Items.AddRange(new object[] { "Usuário", "Senha" });
            cbbFiltrar.Location = new Point(614, 283);
            cbbFiltrar.Name = "cbbFiltrar";
            cbbFiltrar.Size = new Size(236, 23);
            cbbFiltrar.TabIndex = 9;
            // 
            // nome
            // 
            nome.HeaderText = "nome";
            nome.Name = "nome";
            nome.ReadOnly = true;
            // 
            // senha
            // 
            senha.HeaderText = "senha";
            senha.Name = "senha";
            senha.ReadOnly = true;
            // 
            // frmUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(864, 461);
            Controls.Add(cbbFiltrar);
            Controls.Add(label2);
            Controls.Add(txtSenha);
            Controls.Add(label1);
            Controls.Add(txtUsuario);
            Controls.Add(btnEditar);
            Controls.Add(btnFiltrar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvUsuarios);
            Name = "frmUsuarios";
            Text = "Usuários";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DataGridView dgvUsuarios;
        private Button btnCriar;
        private Button btnExcluir;
        private Button btnEditar;
        private Button btnFiltrar;
        private TextBox txtUsuario;
        private Label label1;
        private TextBox txtSenha;
        private Label label2;
        private ComboBox cbbFiltrar;
        private DataGridViewTextBoxColumn nome;
        private DataGridViewTextBoxColumn senha;
    }
}