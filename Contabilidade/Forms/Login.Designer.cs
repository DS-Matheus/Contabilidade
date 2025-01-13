namespace Contabilidade
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnRestaurar = new Button();
            btnBackup = new Button();
            btnRenomearBD = new Button();
            btnExcluirBD = new Button();
            btnCriarBD = new Button();
            cbbBD = new ComboBox();
            gpbInfoUsuario = new GroupBox();
            chbVisibilidadeSenha = new CheckBox();
            btnEntrar = new Button();
            txtSenha = new TextBox();
            label2 = new Label();
            txtNome = new TextBox();
            label1 = new Label();
            pnlSidebar = new Panel();
            pictureBox1 = new PictureBox();
            label5 = new Label();
            label3 = new Label();
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            label4 = new Label();
            btnFechar = new Button();
            pnlLogin = new Panel();
            groupBox1.SuspendLayout();
            gpbInfoUsuario.SuspendLayout();
            pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnlBarraTitulo.SuspendLayout();
            pnlLogin.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRestaurar);
            groupBox1.Controls.Add(btnBackup);
            groupBox1.Controls.Add(btnRenomearBD);
            groupBox1.Controls.Add(btnExcluirBD);
            groupBox1.Controls.Add(btnCriarBD);
            groupBox1.Controls.Add(cbbBD);
            groupBox1.Font = new Font("Lucida Sans", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(17, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 145);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Banco de Dados";
            // 
            // btnRestaurar
            // 
            btnRestaurar.Cursor = Cursors.Hand;
            btnRestaurar.Font = new Font("Lucida Sans", 10.5F);
            btnRestaurar.Location = new Point(153, 103);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(88, 31);
            btnRestaurar.TabIndex = 5;
            btnRestaurar.Text = "Restaurar";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            // 
            // btnBackup
            // 
            btnBackup.Cursor = Cursors.Hand;
            btnBackup.Font = new Font("Lucida Sans", 10.5F);
            btnBackup.Location = new Point(59, 103);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(88, 31);
            btnBackup.TabIndex = 4;
            btnBackup.Text = "Backup";
            btnBackup.UseVisualStyleBackColor = true;
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRenomearBD
            // 
            btnRenomearBD.Cursor = Cursors.Hand;
            btnRenomearBD.Font = new Font("Lucida Sans", 10.5F);
            btnRenomearBD.Location = new Point(187, 66);
            btnRenomearBD.Name = "btnRenomearBD";
            btnRenomearBD.Size = new Size(89, 31);
            btnRenomearBD.TabIndex = 3;
            btnRenomearBD.Text = "Renomear";
            btnRenomearBD.UseVisualStyleBackColor = true;
            btnRenomearBD.Click += btnRenomearBD_Click;
            // 
            // btnExcluirBD
            // 
            btnExcluirBD.Cursor = Cursors.Hand;
            btnExcluirBD.Font = new Font("Lucida Sans", 10.5F);
            btnExcluirBD.Location = new Point(102, 66);
            btnExcluirBD.Name = "btnExcluirBD";
            btnExcluirBD.Size = new Size(75, 31);
            btnExcluirBD.TabIndex = 2;
            btnExcluirBD.Text = "Excluir";
            btnExcluirBD.UseVisualStyleBackColor = true;
            btnExcluirBD.Click += btnExcluirBD_Click;
            // 
            // btnCriarBD
            // 
            btnCriarBD.Cursor = Cursors.Hand;
            btnCriarBD.Font = new Font("Lucida Sans", 10.5F);
            btnCriarBD.Location = new Point(17, 66);
            btnCriarBD.Name = "btnCriarBD";
            btnCriarBD.Size = new Size(75, 31);
            btnCriarBD.TabIndex = 1;
            btnCriarBD.Text = "Criar";
            btnCriarBD.UseVisualStyleBackColor = true;
            btnCriarBD.Click += btnCriarBD_Click;
            // 
            // cbbBD
            // 
            cbbBD.Cursor = Cursors.IBeam;
            cbbBD.DisplayMember = "dsadas";
            cbbBD.Font = new Font("Lucida Sans", 10.5F);
            cbbBD.FormattingEnabled = true;
            cbbBD.Location = new Point(17, 31);
            cbbBD.Margin = new Padding(20);
            cbbBD.MaxLength = 30;
            cbbBD.Name = "cbbBD";
            cbbBD.Size = new Size(260, 24);
            cbbBD.Sorted = true;
            cbbBD.TabIndex = 0;
            // 
            // gpbInfoUsuario
            // 
            gpbInfoUsuario.Controls.Add(chbVisibilidadeSenha);
            gpbInfoUsuario.Controls.Add(btnEntrar);
            gpbInfoUsuario.Controls.Add(txtSenha);
            gpbInfoUsuario.Controls.Add(label2);
            gpbInfoUsuario.Controls.Add(txtNome);
            gpbInfoUsuario.Controls.Add(label1);
            gpbInfoUsuario.Font = new Font("Lucida Sans", 12F, FontStyle.Bold);
            gpbInfoUsuario.Location = new Point(17, 172);
            gpbInfoUsuario.Name = "gpbInfoUsuario";
            gpbInfoUsuario.Size = new Size(300, 156);
            gpbInfoUsuario.TabIndex = 3;
            gpbInfoUsuario.TabStop = false;
            gpbInfoUsuario.Text = "Informações de Usuário";
            // 
            // chbVisibilidadeSenha
            // 
            chbVisibilidadeSenha.AutoSize = true;
            chbVisibilidadeSenha.Font = new Font("Lucida Sans", 10.5F);
            chbVisibilidadeSenha.Location = new Point(17, 127);
            chbVisibilidadeSenha.Name = "chbVisibilidadeSenha";
            chbVisibilidadeSenha.Size = new Size(104, 20);
            chbVisibilidadeSenha.TabIndex = 2;
            chbVisibilidadeSenha.Text = "Exibir senha";
            chbVisibilidadeSenha.UseVisualStyleBackColor = true;
            chbVisibilidadeSenha.CheckedChanged += chbVisibilidadeSenha_CheckedChanged;
            // 
            // btnEntrar
            // 
            btnEntrar.Cursor = Cursors.Hand;
            btnEntrar.Font = new Font("Lucida Sans", 10.5F);
            btnEntrar.Location = new Point(202, 120);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(75, 30);
            btnEntrar.TabIndex = 3;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = true;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // txtSenha
            // 
            txtSenha.Font = new Font("Lucida Sans", 10.5F);
            txtSenha.Location = new Point(17, 91);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(260, 24);
            txtSenha.TabIndex = 1;
            txtSenha.UseSystemPasswordChar = true;
            txtSenha.KeyPress += txtSenha_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(17, 73);
            label2.Name = "label2";
            label2.Size = new Size(47, 16);
            label2.TabIndex = 2;
            label2.Text = "Senha";
            // 
            // txtNome
            // 
            txtNome.Font = new Font("Lucida Sans", 10.5F);
            txtNome.Location = new Point(17, 43);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(260, 24);
            txtNome.TabIndex = 0;
            txtNome.KeyPress += txtNome_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10.5F);
            label1.Location = new Point(17, 25);
            label1.Name = "label1";
            label1.Size = new Size(45, 16);
            label1.TabIndex = 0;
            label1.Text = "Nome";
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(50, 52, 77);
            pnlSidebar.Controls.Add(pictureBox1);
            pnlSidebar.Controls.Add(label5);
            pnlSidebar.Controls.Add(label3);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 30);
            pnlSidebar.Margin = new Padding(0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(260, 340);
            pnlSidebar.TabIndex = 5;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.User;
            pictureBox1.Location = new Point(56, 61);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(148, 164);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Lucida Sans", 10F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(89, 313);
            label5.Name = "label5";
            label5.Size = new Size(83, 16);
            label5.TabIndex = 4;
            label5.Text = "Versão: 1.1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Forte", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(66, 228);
            label3.Name = "label3";
            label3.Size = new Size(128, 52);
            label3.TabIndex = 0;
            label3.Text = "Login";
            // 
            // pnlBarraTitulo
            // 
            pnlBarraTitulo.BackColor = Color.FromArgb(38, 38, 59);
            pnlBarraTitulo.Controls.Add(btnMinimizar);
            pnlBarraTitulo.Controls.Add(label4);
            pnlBarraTitulo.Controls.Add(btnFechar);
            pnlBarraTitulo.Dock = DockStyle.Top;
            pnlBarraTitulo.Location = new Point(0, 0);
            pnlBarraTitulo.Name = "pnlBarraTitulo";
            pnlBarraTitulo.Size = new Size(595, 30);
            pnlBarraTitulo.TabIndex = 4;
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
            btnMinimizar.Location = new Point(535, 0);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(30, 30);
            btnMinimizar.TabIndex = 1;
            btnMinimizar.Text = "--";
            btnMinimizar.UseVisualStyleBackColor = true;
            btnMinimizar.Click += btnMinimizar_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Lucida Sans", 10F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(12, 7);
            label4.Name = "label4";
            label4.Size = new Size(178, 16);
            label4.TabIndex = 1;
            label4.Text = "Sistema de Contabilidade ";
            label4.MouseDown += label4_MouseDown;
            // 
            // btnFechar
            // 
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Dock = DockStyle.Right;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(565, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 1;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // pnlLogin
            // 
            pnlLogin.BackColor = SystemColors.ButtonFace;
            pnlLogin.Controls.Add(groupBox1);
            pnlLogin.Controls.Add(gpbInfoUsuario);
            pnlLogin.Dock = DockStyle.Fill;
            pnlLogin.Location = new Point(260, 30);
            pnlLogin.Name = "pnlLogin";
            pnlLogin.Size = new Size(335, 340);
            pnlLogin.TabIndex = 1;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(232, 241, 242);
            ClientSize = new Size(595, 370);
            Controls.Add(pnlLogin);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Contabilidade - Login";
            Load += frmLogin_Load;
            groupBox1.ResumeLayout(false);
            gpbInfoUsuario.ResumeLayout(false);
            gpbInfoUsuario.PerformLayout();
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            pnlLogin.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox gpbInfoUsuario;
        private ComboBox cbbBD;
        private Button btnRenomearBD;
        private Button btnExcluirBD;
        private Button btnCriarBD;
        private Button btnEntrar;
        private TextBox txtSenha;
        private Label label2;
        private TextBox txtNome;
        private Label label1;
        private Panel pnlSidebar;
        private Label label3;
        private Panel pnlBarraTitulo;
        private Panel pnlLogin;
        private Button btnFechar;
        private Label label4;
        private Button btnMinimizar;
        private CheckBox chbVisibilidadeSenha;
        private Button btnRestaurar;
        private Button btnBackup;
        private Label label5;
        private PictureBox pictureBox1;
    }
}
