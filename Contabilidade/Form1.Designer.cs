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
            btnRenomearBD = new Button();
            btnExcluirBD = new Button();
            btnCriarBD = new Button();
            cbbBD = new ComboBox();
            groupBox2 = new GroupBox();
            chbVisibilidadeSenha = new CheckBox();
            btnEntrar = new Button();
            txtSenha = new TextBox();
            label2 = new Label();
            txtNome = new TextBox();
            label1 = new Label();
            panel1 = new Panel();
            label3 = new Label();
            panel2 = new Panel();
            btnMinimizar = new Button();
            label4 = new Label();
            btnFechar = new Button();
            panel3 = new Panel();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRenomearBD);
            groupBox1.Controls.Add(btnExcluirBD);
            groupBox1.Controls.Add(btnCriarBD);
            groupBox1.Controls.Add(cbbBD);
            groupBox1.Font = new Font("Lucida Sans", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(17, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 112);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Banco de Dados";
            // 
            // btnRenomearBD
            // 
            btnRenomearBD.Cursor = Cursors.Hand;
            btnRenomearBD.Font = new Font("Lucida Sans", 9.75F);
            btnRenomearBD.Location = new Point(187, 66);
            btnRenomearBD.Name = "btnRenomearBD";
            btnRenomearBD.Size = new Size(89, 31);
            btnRenomearBD.TabIndex = 3;
            btnRenomearBD.Text = "Renomear";
            btnRenomearBD.UseVisualStyleBackColor = true;
            // 
            // btnExcluirBD
            // 
            btnExcluirBD.Cursor = Cursors.Hand;
            btnExcluirBD.Font = new Font("Lucida Sans", 9.75F);
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
            btnCriarBD.Font = new Font("Lucida Sans", 9.75F);
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
            cbbBD.Font = new Font("Lucida Sans", 9.75F);
            cbbBD.FormattingEnabled = true;
            cbbBD.Location = new Point(17, 31);
            cbbBD.Margin = new Padding(20);
            cbbBD.Name = "cbbBD";
            cbbBD.Size = new Size(260, 23);
            cbbBD.Sorted = true;
            cbbBD.TabIndex = 0;
            cbbBD.TextChanged += cbbBD_TextChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chbVisibilidadeSenha);
            groupBox2.Controls.Add(btnEntrar);
            groupBox2.Controls.Add(txtSenha);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtNome);
            groupBox2.Controls.Add(label1);
            groupBox2.Font = new Font("Lucida Sans", 12F, FontStyle.Bold);
            groupBox2.Location = new Point(17, 140);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(300, 156);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Informações de Usuário";
            // 
            // chbVisibilidadeSenha
            // 
            chbVisibilidadeSenha.AutoSize = true;
            chbVisibilidadeSenha.Font = new Font("Lucida Sans", 9.75F);
            chbVisibilidadeSenha.Location = new Point(17, 127);
            chbVisibilidadeSenha.Name = "chbVisibilidadeSenha";
            chbVisibilidadeSenha.Size = new Size(102, 19);
            chbVisibilidadeSenha.TabIndex = 2;
            chbVisibilidadeSenha.Text = "Exibir senha";
            chbVisibilidadeSenha.UseVisualStyleBackColor = true;
            chbVisibilidadeSenha.CheckedChanged += chbVisibilidadeSenha_CheckedChanged;
            // 
            // btnEntrar
            // 
            btnEntrar.Cursor = Cursors.Hand;
            btnEntrar.Font = new Font("Lucida Sans", 9.75F);
            btnEntrar.Location = new Point(201, 120);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(75, 30);
            btnEntrar.TabIndex = 3;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = true;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // txtSenha
            // 
            txtSenha.Font = new Font("Lucida Sans", 9.75F);
            txtSenha.Location = new Point(17, 91);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(260, 23);
            txtSenha.TabIndex = 1;
            txtSenha.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 9.75F);
            label2.Location = new Point(17, 73);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 2;
            label2.Text = "Senha";
            // 
            // txtNome
            // 
            txtNome.Font = new Font("Lucida Sans", 9.75F);
            txtNome.Location = new Point(17, 47);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(260, 23);
            txtNome.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 9.75F);
            label1.Location = new Point(17, 29);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 0;
            label1.Text = "Nome";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(19, 41, 61);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 30);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 309);
            panel1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Forte", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(232, 241, 242);
            label3.Location = new Point(66, 22);
            label3.Name = "label3";
            label3.Size = new Size(128, 52);
            label3.TabIndex = 0;
            label3.Text = "Login";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnMinimizar);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(btnFechar);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(595, 30);
            panel2.TabIndex = 4;
            panel2.MouseDown += panel2_MouseDown;
            // 
            // btnMinimizar
            // 
            btnMinimizar.Cursor = Cursors.Hand;
            btnMinimizar.Dock = DockStyle.Right;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
            label4.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 7);
            label4.Name = "label4";
            label4.Size = new Size(156, 16);
            label4.TabIndex = 1;
            label4.Text = "Sistema de Contabilidade ";
            // 
            // btnFechar
            // 
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Dock = DockStyle.Right;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFechar.Location = new Point(565, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 1;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += button1_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(232, 241, 242);
            panel3.Controls.Add(groupBox1);
            panel3.Controls.Add(groupBox2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(260, 30);
            panel3.Name = "panel3";
            panel3.Size = new Size(335, 309);
            panel3.TabIndex = 1;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(232, 241, 242);
            ClientSize = new Size(595, 339);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Contabilidade - Login";
            Load += frmLogin_Load;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ComboBox cbbBD;
        private Button btnRenomearBD;
        private Button btnExcluirBD;
        private Button btnCriarBD;
        private Button btnEntrar;
        private TextBox txtSenha;
        private Label label2;
        private TextBox txtNome;
        private Label label1;
        private Panel panel1;
        private Label label3;
        private Panel panel2;
        private Panel panel3;
        private Button btnFechar;
        private Label label4;
        private Button btnMinimizar;
        private CheckBox chbVisibilidadeSenha;
    }
}
