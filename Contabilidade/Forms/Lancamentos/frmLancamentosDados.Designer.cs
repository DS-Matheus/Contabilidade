namespace Contabilidade.Forms.Lancamentos
{
    partial class frmLancamentosDados
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
            btnAlterarSinal = new Button();
            txtDescricao = new TextBox();
            label5 = new Label();
            txtHistorico = new TextBox();
            btnSelecionarHistorico = new Button();
            btnSelecionarConta = new Button();
            dtpData = new DateTimePicker();
            nudValor = new NumericUpDown();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtConta = new TextBox();
            label1 = new Label();
            btnSalvar = new Button();
            pnlBarraTitulo.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudValor).BeginInit();
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
            lblTitulo.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(219, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Formulário de dados de lançamentos";
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
            panel1.Controls.Add(btnAlterarSinal);
            panel1.Controls.Add(txtDescricao);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtHistorico);
            panel1.Controls.Add(btnSelecionarHistorico);
            panel1.Controls.Add(btnSelecionarConta);
            panel1.Controls.Add(dtpData);
            panel1.Controls.Add(nudValor);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtConta);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnSalvar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(430, 270);
            panel1.TabIndex = 8;
            // 
            // btnAlterarSinal
            // 
            btnAlterarSinal.Image = Properties.Resources.Search;
            btnAlterarSinal.Location = new Point(380, 43);
            btnAlterarSinal.Name = "btnAlterarSinal";
            btnAlterarSinal.Size = new Size(25, 25);
            btnAlterarSinal.TabIndex = 17;
            btnAlterarSinal.UseVisualStyleBackColor = true;
            btnAlterarSinal.Click += btnAlterarSinal_Click;
            // 
            // txtDescricao
            // 
            txtDescricao.BackColor = Color.White;
            txtDescricao.Enabled = false;
            txtDescricao.Location = new Point(25, 92);
            txtDescricao.MaxLength = 100;
            txtDescricao.Multiline = true;
            txtDescricao.Name = "txtDescricao";
            txtDescricao.Size = new Size(380, 46);
            txtDescricao.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 74);
            label5.Margin = new Padding(5);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 16;
            label5.Text = "Descrição";
            // 
            // txtHistorico
            // 
            txtHistorico.BackColor = Color.White;
            txtHistorico.Enabled = false;
            txtHistorico.Location = new Point(25, 164);
            txtHistorico.Margin = new Padding(5);
            txtHistorico.MaxLength = 100;
            txtHistorico.Multiline = true;
            txtHistorico.Name = "txtHistorico";
            txtHistorico.Size = new Size(342, 40);
            txtHistorico.TabIndex = 14;
            // 
            // btnSelecionarHistorico
            // 
            btnSelecionarHistorico.Image = Properties.Resources.Search;
            btnSelecionarHistorico.Location = new Point(370, 164);
            btnSelecionarHistorico.Name = "btnSelecionarHistorico";
            btnSelecionarHistorico.Size = new Size(35, 40);
            btnSelecionarHistorico.TabIndex = 13;
            btnSelecionarHistorico.UseVisualStyleBackColor = true;
            btnSelecionarHistorico.Click += btnSelecionarHistorico_Click;
            // 
            // btnSelecionarConta
            // 
            btnSelecionarConta.Image = Properties.Resources.Search;
            btnSelecionarConta.Location = new Point(196, 43);
            btnSelecionarConta.Name = "btnSelecionarConta";
            btnSelecionarConta.Size = new Size(25, 25);
            btnSelecionarConta.TabIndex = 12;
            btnSelecionarConta.UseVisualStyleBackColor = true;
            btnSelecionarConta.Click += btnSelecionarConta_Click;
            // 
            // dtpData
            // 
            dtpData.Location = new Point(25, 232);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(269, 23);
            dtpData.TabIndex = 9;
            // 
            // nudValor
            // 
            nudValor.DecimalPlaces = 2;
            nudValor.Location = new Point(225, 43);
            nudValor.Maximum = new decimal(new int[] { 1410065407, 2, 0, 131072 });
            nudValor.Minimum = new decimal(new int[] { 999999999, 0, 0, -2147352576 });
            nudValor.Name = "nudValor";
            nudValor.Size = new Size(151, 23);
            nudValor.TabIndex = 8;
            nudValor.ThousandsSeparator = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 214);
            label4.Margin = new Padding(5);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 6;
            label4.Text = "Data";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(225, 24);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 5;
            label3.Text = "Valor";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 146);
            label2.Margin = new Padding(5);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 3;
            label2.Text = "Histórico";
            // 
            // txtConta
            // 
            txtConta.BackColor = Color.White;
            txtConta.Enabled = false;
            txtConta.Location = new Point(25, 43);
            txtConta.Margin = new Padding(25, 3, 3, 3);
            txtConta.MaxLength = 14;
            txtConta.Name = "txtConta";
            txtConta.Size = new Size(165, 23);
            txtConta.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 25);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 1;
            label1.Text = "Conta";
            // 
            // btnSalvar
            // 
            btnSalvar.Location = new Point(300, 223);
            btnSalvar.Margin = new Padding(3, 10, 3, 3);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(105, 32);
            btnSalvar.TabIndex = 3;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // frmLancamentosDados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(430, 300);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLancamentosDados";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmLancamentosDados";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudValor).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private Panel panel1;
        private Label label3;
        private Label label2;
        private TextBox txtConta;
        private Label label1;
        private Button btnSalvar;
        private Label label4;
        private NumericUpDown nudValor;
        private DateTimePicker dtpData;
        private Button btnSelecionarHistorico;
        private Button btnSelecionarConta;
        private TextBox txtHistorico;
        private TextBox txtDescricao;
        private Label label5;
        private Button btnAlterarSinal;
    }
}