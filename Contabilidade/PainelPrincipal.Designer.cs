namespace Contabilidade
{
    partial class frmPainelPrincipal
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
            pnlInfo = new Panel();
            lblBanco = new Label();
            lblUsuario = new Label();
            btnFechar = new Button();
            pnlMenuLateral = new Panel();
            btnLogoff = new Button();
            btnUsuarios = new Button();
            btnConsultas = new Button();
            btnRelatorios = new Button();
            btnLancamentos = new Button();
            btnCadastro = new Button();
            btnMinimizar = new Button();
            pnlTitulo = new Panel();
            btnFecharFormFilho = new Button();
            lblTitulo = new Label();
            pnlDesktop = new Panel();
            label2 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label1 = new Label();
            pnlInfo.SuspendLayout();
            pnlMenuLateral.SuspendLayout();
            pnlTitulo.SuspendLayout();
            pnlDesktop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlInfo
            // 
            pnlInfo.BackColor = Color.FromArgb(39, 39, 58);
            pnlInfo.Controls.Add(lblBanco);
            pnlInfo.Controls.Add(lblUsuario);
            pnlInfo.Dock = DockStyle.Top;
            pnlInfo.Location = new Point(0, 0);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new Size(220, 80);
            pnlInfo.TabIndex = 5;
            // 
            // lblBanco
            // 
            lblBanco.BackColor = Color.Transparent;
            lblBanco.Dock = DockStyle.Top;
            lblBanco.Font = new Font("Lucida Sans", 10F);
            lblBanco.ForeColor = Color.Gainsboro;
            lblBanco.Location = new Point(0, 0);
            lblBanco.Name = "lblBanco";
            lblBanco.Size = new Size(220, 40);
            lblBanco.TabIndex = 2;
            lblBanco.Text = "Banco";
            lblBanco.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lblUsuario
            // 
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Dock = DockStyle.Bottom;
            lblUsuario.Font = new Font("Lucida Sans", 10F);
            lblUsuario.ForeColor = Color.Gainsboro;
            lblUsuario.Location = new Point(0, 40);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(220, 40);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuário";
            lblUsuario.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnFechar
            // 
            btnFechar.Anchor = AnchorStyles.None;
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.Font = new Font("Microsoft Sans Serif", 15F);
            btnFechar.ForeColor = SystemColors.Control;
            btnFechar.Location = new Point(796, 19);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 2;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // pnlMenuLateral
            // 
            pnlMenuLateral.BackColor = Color.FromArgb(51, 51, 76);
            pnlMenuLateral.Controls.Add(btnLogoff);
            pnlMenuLateral.Controls.Add(btnUsuarios);
            pnlMenuLateral.Controls.Add(btnConsultas);
            pnlMenuLateral.Controls.Add(btnRelatorios);
            pnlMenuLateral.Controls.Add(btnLancamentos);
            pnlMenuLateral.Controls.Add(btnCadastro);
            pnlMenuLateral.Controls.Add(pnlInfo);
            pnlMenuLateral.Dock = DockStyle.Left;
            pnlMenuLateral.Location = new Point(0, 0);
            pnlMenuLateral.Margin = new Padding(0);
            pnlMenuLateral.Name = "pnlMenuLateral";
            pnlMenuLateral.Size = new Size(220, 580);
            pnlMenuLateral.TabIndex = 6;
            // 
            // btnLogoff
            // 
            btnLogoff.Dock = DockStyle.Bottom;
            btnLogoff.FlatAppearance.BorderSize = 0;
            btnLogoff.FlatStyle = FlatStyle.Flat;
            btnLogoff.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogoff.ForeColor = Color.Gainsboro;
            btnLogoff.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogoff.Location = new Point(0, 540);
            btnLogoff.Name = "btnLogoff";
            btnLogoff.Size = new Size(220, 40);
            btnLogoff.TabIndex = 11;
            btnLogoff.Text = "Menu Inicial";
            btnLogoff.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLogoff.UseVisualStyleBackColor = true;
            btnLogoff.Click += btnLogoff_Click;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Dock = DockStyle.Top;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUsuarios.ForeColor = Color.Gainsboro;
            btnUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.Location = new Point(0, 240);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(220, 40);
            btnUsuarios.TabIndex = 10;
            btnUsuarios.Text = "Usuários";
            btnUsuarios.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnConsultas
            // 
            btnConsultas.Dock = DockStyle.Top;
            btnConsultas.FlatAppearance.BorderSize = 0;
            btnConsultas.FlatStyle = FlatStyle.Flat;
            btnConsultas.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnConsultas.ForeColor = Color.Gainsboro;
            btnConsultas.ImageAlign = ContentAlignment.MiddleLeft;
            btnConsultas.Location = new Point(0, 200);
            btnConsultas.Name = "btnConsultas";
            btnConsultas.Size = new Size(220, 40);
            btnConsultas.TabIndex = 9;
            btnConsultas.Text = "Consultas";
            btnConsultas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnConsultas.UseVisualStyleBackColor = true;
            btnConsultas.Click += btnConsultas_Click;
            // 
            // btnRelatorios
            // 
            btnRelatorios.Dock = DockStyle.Top;
            btnRelatorios.FlatAppearance.BorderSize = 0;
            btnRelatorios.FlatStyle = FlatStyle.Flat;
            btnRelatorios.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRelatorios.ForeColor = Color.Gainsboro;
            btnRelatorios.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelatorios.Location = new Point(0, 160);
            btnRelatorios.Name = "btnRelatorios";
            btnRelatorios.Size = new Size(220, 40);
            btnRelatorios.TabIndex = 8;
            btnRelatorios.Text = "Relatórios";
            btnRelatorios.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRelatorios.UseVisualStyleBackColor = true;
            btnRelatorios.Click += btnRelatorios_Click;
            // 
            // btnLancamentos
            // 
            btnLancamentos.Dock = DockStyle.Top;
            btnLancamentos.FlatAppearance.BorderSize = 0;
            btnLancamentos.FlatStyle = FlatStyle.Flat;
            btnLancamentos.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLancamentos.ForeColor = Color.Gainsboro;
            btnLancamentos.ImageAlign = ContentAlignment.MiddleLeft;
            btnLancamentos.Location = new Point(0, 120);
            btnLancamentos.Name = "btnLancamentos";
            btnLancamentos.Size = new Size(220, 40);
            btnLancamentos.TabIndex = 7;
            btnLancamentos.Text = "Lançamentos";
            btnLancamentos.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLancamentos.UseVisualStyleBackColor = true;
            btnLancamentos.Click += btnLancamentos_Click;
            // 
            // btnCadastro
            // 
            btnCadastro.Dock = DockStyle.Top;
            btnCadastro.FlatAppearance.BorderSize = 0;
            btnCadastro.FlatStyle = FlatStyle.Flat;
            btnCadastro.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCadastro.ForeColor = Color.Gainsboro;
            btnCadastro.ImageAlign = ContentAlignment.MiddleLeft;
            btnCadastro.Location = new Point(0, 80);
            btnCadastro.Name = "btnCadastro";
            btnCadastro.Size = new Size(220, 40);
            btnCadastro.TabIndex = 6;
            btnCadastro.Text = "Cadastros";
            btnCadastro.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCadastro.UseVisualStyleBackColor = true;
            btnCadastro.Click += btnCadastro_Click;
            // 
            // btnMinimizar
            // 
            btnMinimizar.Anchor = AnchorStyles.None;
            btnMinimizar.Cursor = Cursors.Hand;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            btnMinimizar.ForeColor = SystemColors.Control;
            btnMinimizar.Location = new Point(748, 22);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(30, 30);
            btnMinimizar.TabIndex = 3;
            btnMinimizar.Text = "--";
            btnMinimizar.UseVisualStyleBackColor = true;
            btnMinimizar.Click += btnMinimizar_Click;
            // 
            // pnlTitulo
            // 
            pnlTitulo.BackColor = Color.FromArgb(0, 150, 136);
            pnlTitulo.Controls.Add(btnFecharFormFilho);
            pnlTitulo.Controls.Add(lblTitulo);
            pnlTitulo.Controls.Add(btnFechar);
            pnlTitulo.Controls.Add(btnMinimizar);
            pnlTitulo.Dock = DockStyle.Top;
            pnlTitulo.Location = new Point(220, 0);
            pnlTitulo.Name = "pnlTitulo";
            pnlTitulo.Size = new Size(880, 80);
            pnlTitulo.TabIndex = 7;
            pnlTitulo.MouseDown += pnlTitulo_MouseDown;
            // 
            // btnFecharFormFilho
            // 
            btnFecharFormFilho.Dock = DockStyle.Left;
            btnFecharFormFilho.FlatAppearance.BorderSize = 0;
            btnFecharFormFilho.FlatStyle = FlatStyle.Flat;
            btnFecharFormFilho.Font = new Font("Microsoft Sans Serif", 15F);
            btnFecharFormFilho.Location = new Point(0, 0);
            btnFecharFormFilho.Name = "btnFecharFormFilho";
            btnFecharFormFilho.Size = new Size(75, 80);
            btnFecharFormFilho.TabIndex = 4;
            btnFecharFormFilho.Text = "X";
            btnFecharFormFilho.UseVisualStyleBackColor = true;
            btnFecharFormFilho.Click += btnFecharFormFilho_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.Anchor = AnchorStyles.None;
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Forte", 25F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(313, 22);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(255, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Painel Principal";
            lblTitulo.MouseDown += lblTitulo_MouseDown;
            // 
            // pnlDesktop
            // 
            pnlDesktop.Controls.Add(label2);
            pnlDesktop.Controls.Add(label5);
            pnlDesktop.Controls.Add(label4);
            pnlDesktop.Controls.Add(label3);
            pnlDesktop.Controls.Add(label1);
            pnlDesktop.Dock = DockStyle.Fill;
            pnlDesktop.Location = new Point(220, 80);
            pnlDesktop.Name = "pnlDesktop";
            pnlDesktop.Size = new Size(880, 500);
            pnlDesktop.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(265, 182);
            label2.Name = "label2";
            label2.Size = new Size(351, 24);
            label2.TabIndex = 2;
            label2.Text = "Componentes do painel principal";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(221, 294);
            label5.Name = "label5";
            label5.Size = new Size(439, 24);
            label5.TabIndex = 1;
            label5.Text = "Outros elementos que julgar interessante";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(161, 270);
            label4.Name = "label4";
            label4.Size = new Size(559, 24);
            label4.TabIndex = 1;
            label4.Text = "Usuário atual (será movido da onde está atualmente)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(169, 246);
            label3.Name = "label3";
            label3.Size = new Size(542, 24);
            label3.TabIndex = 1;
            label3.Text = "Banco atual (será movido da onde está atualmente)";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(397, 222);
            label1.Name = "label1";
            label1.Size = new Size(86, 24);
            label1.TabIndex = 1;
            label1.Text = "Relógio";
            // 
            // frmPainelPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 580);
            Controls.Add(pnlDesktop);
            Controls.Add(pnlTitulo);
            Controls.Add(pnlMenuLateral);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "frmPainelPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PainelPrincipal";
            pnlInfo.ResumeLayout(false);
            pnlMenuLateral.ResumeLayout(false);
            pnlTitulo.ResumeLayout(false);
            pnlTitulo.PerformLayout();
            pnlDesktop.ResumeLayout(false);
            pnlDesktop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlInfo;
        private Button btnFechar;
        private Label lblUsuario;
        private Panel pnlMenuLateral;
        private Button btnMinimizar;
        private Label lblBanco;
        private Panel pnlTitulo;
        private Label lblTitulo;
        private Button btnCadastro;
        private Button btnLogoff;
        private Button btnUsuarios;
        private Button btnConsultas;
        private Button btnRelatorios;
        private Button btnLancamentos;
        private Panel pnlDesktop;
        private Button btnFecharFormFilho;
        private Label label2;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label1;
    }
}