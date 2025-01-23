namespace Contabilidade
{
    partial class frmCriarBD
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
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            lblTitulo = new Label();
            btnFechar = new Button();
            panel1 = new Panel();
            txtBancoDados = new TextBox();
            label3 = new Label();
            txtSenha = new TextBox();
            label2 = new Label();
            txtUsuario = new TextBox();
            label1 = new Label();
            btnCriar = new Button();
            pnlBarraTitulo.SuspendLayout();
            panel1.SuspendLayout();
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
            pnlBarraTitulo.Size = new Size(398, 30);
            pnlBarraTitulo.TabIndex = 6;
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
            btnMinimizar.Location = new Point(338, 0);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(30, 30);
            btnMinimizar.TabIndex = 5;
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
            lblTitulo.Size = new Size(151, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Criar banco de dados";
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
            btnFechar.Location = new Point(368, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 6;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtBancoDados);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtSenha);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtUsuario);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnCriar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(398, 220);
            panel1.TabIndex = 7;
            // 
            // txtBancoDados
            // 
            txtBancoDados.Font = new Font("Lucida Sans", 10.5F);
            txtBancoDados.Location = new Point(31, 36);
            txtBancoDados.MaxLength = 30;
            txtBancoDados.Name = "txtBancoDados";
            txtBancoDados.Size = new Size(335, 24);
            txtBancoDados.TabIndex = 1;
            txtBancoDados.KeyPress += txtBancoDados_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(31, 18);
            label3.Margin = new Padding(3, 10, 3, 0);
            label3.Name = "label3";
            label3.Size = new Size(115, 16);
            label3.TabIndex = 5;
            label3.Text = "Banco de dados";
            // 
            // txtSenha
            // 
            txtSenha.Font = new Font("Lucida Sans", 10.5F);
            txtSenha.Location = new Point(31, 134);
            txtSenha.MaxLength = 30;
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(335, 24);
            txtSenha.TabIndex = 3;
            txtSenha.KeyPress += txtSenha_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(31, 116);
            label2.Margin = new Padding(3, 5, 3, 0);
            label2.Name = "label2";
            label2.Size = new Size(47, 16);
            label2.TabIndex = 3;
            label2.Text = "Senha";
            // 
            // txtUsuario
            // 
            txtUsuario.Font = new Font("Lucida Sans", 10.5F);
            txtUsuario.Location = new Point(31, 85);
            txtUsuario.MaxLength = 20;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(335, 24);
            txtUsuario.TabIndex = 2;
            txtUsuario.KeyPress += txtUsuario_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10.5F);
            label1.Location = new Point(31, 67);
            label1.Margin = new Padding(3, 5, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(56, 16);
            label1.TabIndex = 1;
            label1.Text = "Usuário";
            // 
            // btnCriar
            // 
            btnCriar.Font = new Font("Lucida Sans", 10.5F);
            btnCriar.Location = new Point(32, 170);
            btnCriar.Margin = new Padding(3, 10, 3, 3);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(335, 32);
            btnCriar.TabIndex = 4;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // frmCriarBD
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(398, 250);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmCriarBD";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmCriarBD";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private Panel panel1;
        private TextBox txtBancoDados;
        private Label label3;
        private TextBox txtSenha;
        private Label label2;
        private TextBox txtUsuario;
        private Label label1;
        private Button btnCriar;
    }
}