namespace Contabilidade.Forms.Cadastros
{
    partial class frmContasDados
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
            cbbNivel = new ComboBox();
            label3 = new Label();
            txtDescricao = new TextBox();
            label2 = new Label();
            txtConta = new TextBox();
            label1 = new Label();
            btnSalvar = new Button();
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
            pnlBarraTitulo.Size = new Size(430, 30);
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
            btnMinimizar.Location = new Point(370, 0);
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
            lblTitulo.Font = new Font("Lucida Sans", 10F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(213, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Formulário de dados de contas";
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
            btnFechar.Location = new Point(400, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 5;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(cbbNivel);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtDescricao);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtConta);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnSalvar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(430, 196);
            panel1.TabIndex = 7;
            // 
            // cbbNivel
            // 
            cbbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbNivel.Font = new Font("Lucida Sans", 10.5F);
            cbbNivel.FormattingEnabled = true;
            cbbNivel.Items.AddRange(new object[] { "Analítico", "Sintético" });
            cbbNivel.Location = new Point(234, 37);
            cbbNivel.Name = "cbbNivel";
            cbbNivel.Size = new Size(171, 24);
            cbbNivel.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(234, 19);
            label3.Name = "label3";
            label3.Size = new Size(40, 16);
            label3.TabIndex = 5;
            label3.Text = "Nível";
            // 
            // txtDescricao
            // 
            txtDescricao.Font = new Font("Lucida Sans", 10.5F);
            txtDescricao.Location = new Point(25, 87);
            txtDescricao.MaxLength = 100;
            txtDescricao.Multiline = true;
            txtDescricao.Name = "txtDescricao";
            txtDescricao.Size = new Size(380, 46);
            txtDescricao.TabIndex = 2;
            txtDescricao.KeyPress += txtDescricao_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(25, 69);
            label2.Name = "label2";
            label2.Size = new Size(73, 16);
            label2.TabIndex = 3;
            label2.Text = "Descrição";
            // 
            // txtConta
            // 
            txtConta.BackColor = Color.White;
            txtConta.Font = new Font("Lucida Sans", 10.5F);
            txtConta.Location = new Point(25, 37);
            txtConta.Margin = new Padding(25, 3, 3, 3);
            txtConta.MaxLength = 15;
            txtConta.Name = "txtConta";
            txtConta.Size = new Size(171, 24);
            txtConta.TabIndex = 1;
            txtConta.TextChanged += txtConta_TextChanged;
            txtConta.KeyPress += txtConta_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10.5F);
            label1.Location = new Point(25, 19);
            label1.Name = "label1";
            label1.Size = new Size(47, 16);
            label1.TabIndex = 1;
            label1.Text = "Conta";
            // 
            // btnSalvar
            // 
            btnSalvar.Font = new Font("Lucida Sans", 10.5F);
            btnSalvar.Location = new Point(25, 146);
            btnSalvar.Margin = new Padding(3, 10, 3, 3);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(380, 32);
            btnSalvar.TabIndex = 3;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // frmContasDados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(430, 226);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmContasDados";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmContasDados";
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
        private TextBox txtDescricao;
        private Label label2;
        private TextBox txtConta;
        private Label label1;
        private Button btnSalvar;
        private ComboBox cbbNivel;
        private Label label3;
    }
}