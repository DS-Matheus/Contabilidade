namespace Contabilidade.Forms.Lancamentos
{
    partial class frmCalcularExibir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalcularExibir));
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            lblTitulo = new Label();
            btnFechar = new Button();
            groupBox1 = new GroupBox();
            txtPeriodo = new TextBox();
            groupBox2 = new GroupBox();
            txtDebitos = new TextBox();
            txtCreditos = new TextBox();
            label4 = new Label();
            label5 = new Label();
            groupBox3 = new GroupBox();
            txtSaldoFinal = new TextBox();
            txtSaldoAnterior = new TextBox();
            label3 = new Label();
            label2 = new Label();
            btnFechar2 = new Button();
            label1 = new Label();
            txtDiferenca = new TextBox();
            groupBox4 = new GroupBox();
            pnlBarraTitulo.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
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
            pnlBarraTitulo.Size = new Size(304, 30);
            pnlBarraTitulo.TabIndex = 9;
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
            btnMinimizar.Location = new Point(244, 0);
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
            lblTitulo.Size = new Size(189, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Verificação de lançamentos";
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
            btnFechar.Location = new Point(274, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 5;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtPeriodo);
            groupBox1.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox1.Location = new Point(15, 36);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(275, 54);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Período";
            // 
            // txtPeriodo
            // 
            txtPeriodo.BackColor = Color.White;
            txtPeriodo.Location = new Point(6, 22);
            txtPeriodo.Name = "txtPeriodo";
            txtPeriodo.ReadOnly = true;
            txtPeriodo.Size = new Size(263, 24);
            txtPeriodo.TabIndex = 0;
            txtPeriodo.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtDebitos);
            groupBox2.Controls.Add(txtCreditos);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label5);
            groupBox2.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox2.Location = new Point(15, 96);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(275, 84);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "Valores de lançamentos";
            // 
            // txtDebitos
            // 
            txtDebitos.BackColor = Color.White;
            txtDebitos.Font = new Font("Lucida Sans", 10.5F);
            txtDebitos.Location = new Point(144, 50);
            txtDebitos.Name = "txtDebitos";
            txtDebitos.ReadOnly = true;
            txtDebitos.Size = new Size(125, 24);
            txtDebitos.TabIndex = 25;
            txtDebitos.TextAlign = HorizontalAlignment.Right;
            // 
            // txtCreditos
            // 
            txtCreditos.BackColor = Color.White;
            txtCreditos.Font = new Font("Lucida Sans", 10.5F);
            txtCreditos.Location = new Point(144, 24);
            txtCreditos.Name = "txtCreditos";
            txtCreditos.ReadOnly = true;
            txtCreditos.Size = new Size(125, 24);
            txtCreditos.TabIndex = 24;
            txtCreditos.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Lucida Sans", 10.5F);
            label4.Location = new Point(80, 53);
            label4.Name = "label4";
            label4.Size = new Size(58, 16);
            label4.TabIndex = 22;
            label4.Text = "Débitos";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Lucida Sans", 10.5F);
            label5.Location = new Point(75, 27);
            label5.Name = "label5";
            label5.Size = new Size(63, 16);
            label5.TabIndex = 20;
            label5.Text = "Créditos";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtSaldoFinal);
            groupBox3.Controls.Add(txtSaldoAnterior);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(label2);
            groupBox3.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox3.Location = new Point(15, 186);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(275, 81);
            groupBox3.TabIndex = 12;
            groupBox3.TabStop = false;
            groupBox3.Text = "Valores em caixa";
            // 
            // txtSaldoFinal
            // 
            txtSaldoFinal.BackColor = Color.White;
            txtSaldoFinal.Font = new Font("Lucida Sans", 10.5F);
            txtSaldoFinal.Location = new Point(144, 46);
            txtSaldoFinal.Name = "txtSaldoFinal";
            txtSaldoFinal.ReadOnly = true;
            txtSaldoFinal.Size = new Size(125, 24);
            txtSaldoFinal.TabIndex = 27;
            txtSaldoFinal.TextAlign = HorizontalAlignment.Right;
            // 
            // txtSaldoAnterior
            // 
            txtSaldoAnterior.BackColor = Color.White;
            txtSaldoAnterior.Font = new Font("Lucida Sans", 10.5F);
            txtSaldoAnterior.Location = new Point(144, 20);
            txtSaldoAnterior.Name = "txtSaldoAnterior";
            txtSaldoAnterior.ReadOnly = true;
            txtSaldoAnterior.Size = new Size(125, 24);
            txtSaldoAnterior.TabIndex = 26;
            txtSaldoAnterior.TextAlign = HorizontalAlignment.Right;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(45, 49);
            label3.Name = "label3";
            label3.Size = new Size(93, 16);
            label3.TabIndex = 18;
            label3.Text = "Saldo final (-)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(16, 23);
            label2.Name = "label2";
            label2.Size = new Size(122, 16);
            label2.TabIndex = 16;
            label2.Text = "Saldo anterior (+)";
            // 
            // btnFechar2
            // 
            btnFechar2.Font = new Font("Lucida Sans", 10.5F);
            btnFechar2.Location = new Point(15, 334);
            btnFechar2.Name = "btnFechar2";
            btnFechar2.Size = new Size(275, 28);
            btnFechar2.TabIndex = 13;
            btnFechar2.Text = "Fechar";
            btnFechar2.UseVisualStyleBackColor = true;
            btnFechar2.Click += btnFechar2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10.5F);
            label1.Location = new Point(68, 25);
            label1.Name = "label1";
            label1.Size = new Size(70, 16);
            label1.TabIndex = 14;
            label1.Text = "Diferença";
            // 
            // txtDiferenca
            // 
            txtDiferenca.BackColor = Color.White;
            txtDiferenca.Font = new Font("Lucida Sans", 10.5F);
            txtDiferenca.Location = new Point(144, 22);
            txtDiferenca.Name = "txtDiferenca";
            txtDiferenca.ReadOnly = true;
            txtDiferenca.Size = new Size(125, 24);
            txtDiferenca.TabIndex = 26;
            txtDiferenca.TextAlign = HorizontalAlignment.Right;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txtDiferenca);
            groupBox4.Controls.Add(label1);
            groupBox4.Font = new Font("Lucida Sans", 10.5F, FontStyle.Bold);
            groupBox4.Location = new Point(15, 273);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(275, 55);
            groupBox4.TabIndex = 27;
            groupBox4.TabStop = false;
            groupBox4.Text = "Resultado";
            // 
            // frmCalcularExibir
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(304, 371);
            Controls.Add(groupBox4);
            Controls.Add(btnFechar2);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmCalcularExibir";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmDadosCalculaExibir";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button btnFechar2;
        private Label label4;
        private Label label5;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtPeriodo;
        private TextBox txtDebitos;
        private TextBox txtCreditos;
        private TextBox txtSaldoFinal;
        private TextBox txtSaldoAnterior;
        private TextBox txtDiferenca;
        private GroupBox groupBox4;
    }
}