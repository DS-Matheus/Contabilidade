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
            components = new System.ComponentModel.Container();
            pnlLogo = new Panel();
            picLogo = new PictureBox();
            lblBanco = new Label();
            lblUsuario = new Label();
            btnFechar = new Button();
            pnlMenuLateral = new Panel();
            pnlRelatorios = new Panel();
            btnRelUsuarios = new Button();
            btnSaldo = new Button();
            btnBalanceteConta = new Button();
            btnBalanceteGeral = new Button();
            btnRelAnalitico = new Button();
            btnRelDiario = new Button();
            btnRelatorios = new Button();
            pnlLancamentos = new Panel();
            btnTransporte = new Button();
            btnMovimentacao = new Button();
            btnLancamentos = new Button();
            pnlCadastros = new Panel();
            btnUsuarios = new Button();
            btnHistoricos = new Button();
            btnContas = new Button();
            btnLogoff = new Button();
            btnCadastro = new Button();
            btnMinimizar = new Button();
            pnlTitulo = new Panel();
            btnFecharFormFilho = new Button();
            lblTitulo = new Label();
            pnlDesktop = new Panel();
            panel1 = new Panel();
            label2 = new Label();
            label1 = new Label();
            calendario = new MonthCalendar();
            lblRelogio = new Label();
            timerRelogio = new System.Windows.Forms.Timer(components);
            pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlMenuLateral.SuspendLayout();
            pnlRelatorios.SuspendLayout();
            pnlLancamentos.SuspendLayout();
            pnlCadastros.SuspendLayout();
            pnlTitulo.SuspendLayout();
            pnlDesktop.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLogo
            // 
            pnlLogo.BackColor = Color.FromArgb(39, 39, 58);
            pnlLogo.Controls.Add(picLogo);
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Location = new Point(0, 0);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(203, 80);
            pnlLogo.TabIndex = 5;
            // 
            // picLogo
            // 
            picLogo.Dock = DockStyle.Fill;
            picLogo.Location = new Point(0, 0);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(203, 80);
            picLogo.TabIndex = 2;
            picLogo.TabStop = false;
            // 
            // lblBanco
            // 
            lblBanco.BackColor = Color.Transparent;
            lblBanco.Font = new Font("Lucida Sans", 10F);
            lblBanco.ForeColor = Color.Black;
            lblBanco.Location = new Point(185, 24);
            lblBanco.Name = "lblBanco";
            lblBanco.Size = new Size(304, 22);
            lblBanco.TabIndex = 2;
            lblBanco.Text = "Banco";
            lblBanco.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUsuario
            // 
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Font = new Font("Lucida Sans", 10F);
            lblUsuario.ForeColor = Color.Black;
            lblUsuario.Location = new Point(631, 24);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(222, 22);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuário";
            lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
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
            pnlMenuLateral.AutoScroll = true;
            pnlMenuLateral.BackColor = Color.FromArgb(39, 39, 58);
            pnlMenuLateral.Controls.Add(pnlRelatorios);
            pnlMenuLateral.Controls.Add(btnRelatorios);
            pnlMenuLateral.Controls.Add(pnlLancamentos);
            pnlMenuLateral.Controls.Add(btnLancamentos);
            pnlMenuLateral.Controls.Add(pnlCadastros);
            pnlMenuLateral.Controls.Add(btnLogoff);
            pnlMenuLateral.Controls.Add(btnCadastro);
            pnlMenuLateral.Controls.Add(pnlLogo);
            pnlMenuLateral.Dock = DockStyle.Left;
            pnlMenuLateral.Location = new Point(0, 0);
            pnlMenuLateral.Margin = new Padding(0);
            pnlMenuLateral.Name = "pnlMenuLateral";
            pnlMenuLateral.Size = new Size(220, 580);
            pnlMenuLateral.TabIndex = 6;
            // 
            // pnlRelatorios
            // 
            pnlRelatorios.BackColor = Color.FromArgb(62, 62, 92);
            pnlRelatorios.Controls.Add(btnRelUsuarios);
            pnlRelatorios.Controls.Add(btnSaldo);
            pnlRelatorios.Controls.Add(btnBalanceteConta);
            pnlRelatorios.Controls.Add(btnBalanceteGeral);
            pnlRelatorios.Controls.Add(btnRelAnalitico);
            pnlRelatorios.Controls.Add(btnRelDiario);
            pnlRelatorios.Dock = DockStyle.Top;
            pnlRelatorios.Location = new Point(0, 400);
            pnlRelatorios.Name = "pnlRelatorios";
            pnlRelatorios.Size = new Size(203, 240);
            pnlRelatorios.TabIndex = 16;
            pnlRelatorios.Visible = false;
            // 
            // btnRelUsuarios
            // 
            btnRelUsuarios.BackColor = Color.FromArgb(61, 61, 91);
            btnRelUsuarios.Dock = DockStyle.Top;
            btnRelUsuarios.FlatAppearance.BorderSize = 0;
            btnRelUsuarios.FlatStyle = FlatStyle.Flat;
            btnRelUsuarios.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRelUsuarios.ForeColor = Color.Gainsboro;
            btnRelUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelUsuarios.Location = new Point(0, 200);
            btnRelUsuarios.Name = "btnRelUsuarios";
            btnRelUsuarios.Padding = new Padding(30, 0, 0, 0);
            btnRelUsuarios.Size = new Size(203, 40);
            btnRelUsuarios.TabIndex = 12;
            btnRelUsuarios.Text = "Usuários";
            btnRelUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnRelUsuarios.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRelUsuarios.UseVisualStyleBackColor = false;
            btnRelUsuarios.Click += btnRelUsuarios_Click;
            // 
            // btnSaldo
            // 
            btnSaldo.BackColor = Color.FromArgb(61, 61, 91);
            btnSaldo.Dock = DockStyle.Top;
            btnSaldo.FlatAppearance.BorderSize = 0;
            btnSaldo.FlatStyle = FlatStyle.Flat;
            btnSaldo.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSaldo.ForeColor = Color.Gainsboro;
            btnSaldo.ImageAlign = ContentAlignment.MiddleLeft;
            btnSaldo.Location = new Point(0, 160);
            btnSaldo.Name = "btnSaldo";
            btnSaldo.Padding = new Padding(30, 0, 0, 0);
            btnSaldo.Size = new Size(203, 40);
            btnSaldo.TabIndex = 11;
            btnSaldo.Text = "Saldo";
            btnSaldo.TextAlign = ContentAlignment.MiddleLeft;
            btnSaldo.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSaldo.UseVisualStyleBackColor = false;
            btnSaldo.Click += btnSaldo_Click;
            // 
            // btnBalanceteConta
            // 
            btnBalanceteConta.BackColor = Color.FromArgb(61, 61, 91);
            btnBalanceteConta.Dock = DockStyle.Top;
            btnBalanceteConta.FlatAppearance.BorderSize = 0;
            btnBalanceteConta.FlatStyle = FlatStyle.Flat;
            btnBalanceteConta.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBalanceteConta.ForeColor = Color.Gainsboro;
            btnBalanceteConta.ImageAlign = ContentAlignment.MiddleLeft;
            btnBalanceteConta.Location = new Point(0, 120);
            btnBalanceteConta.Name = "btnBalanceteConta";
            btnBalanceteConta.Padding = new Padding(30, 0, 0, 0);
            btnBalanceteConta.Size = new Size(203, 40);
            btnBalanceteConta.TabIndex = 10;
            btnBalanceteConta.Text = "Balancete de Conta";
            btnBalanceteConta.TextAlign = ContentAlignment.MiddleLeft;
            btnBalanceteConta.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnBalanceteConta.UseVisualStyleBackColor = false;
            btnBalanceteConta.Click += btnBalanceteConta_Click;
            // 
            // btnBalanceteGeral
            // 
            btnBalanceteGeral.BackColor = Color.FromArgb(61, 61, 91);
            btnBalanceteGeral.Dock = DockStyle.Top;
            btnBalanceteGeral.FlatAppearance.BorderSize = 0;
            btnBalanceteGeral.FlatStyle = FlatStyle.Flat;
            btnBalanceteGeral.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBalanceteGeral.ForeColor = Color.Gainsboro;
            btnBalanceteGeral.ImageAlign = ContentAlignment.MiddleLeft;
            btnBalanceteGeral.Location = new Point(0, 80);
            btnBalanceteGeral.Name = "btnBalanceteGeral";
            btnBalanceteGeral.Padding = new Padding(30, 0, 0, 0);
            btnBalanceteGeral.Size = new Size(203, 40);
            btnBalanceteGeral.TabIndex = 9;
            btnBalanceteGeral.Text = "Balancete Geral";
            btnBalanceteGeral.TextAlign = ContentAlignment.MiddleLeft;
            btnBalanceteGeral.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnBalanceteGeral.UseVisualStyleBackColor = false;
            btnBalanceteGeral.Click += btnBalanceteGeral_Click;
            // 
            // btnRelAnalitico
            // 
            btnRelAnalitico.BackColor = Color.FromArgb(61, 61, 91);
            btnRelAnalitico.Dock = DockStyle.Top;
            btnRelAnalitico.FlatAppearance.BorderSize = 0;
            btnRelAnalitico.FlatStyle = FlatStyle.Flat;
            btnRelAnalitico.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRelAnalitico.ForeColor = Color.Gainsboro;
            btnRelAnalitico.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelAnalitico.Location = new Point(0, 40);
            btnRelAnalitico.Name = "btnRelAnalitico";
            btnRelAnalitico.Padding = new Padding(30, 0, 0, 0);
            btnRelAnalitico.Size = new Size(203, 40);
            btnRelAnalitico.TabIndex = 8;
            btnRelAnalitico.Text = "Analítico";
            btnRelAnalitico.TextAlign = ContentAlignment.MiddleLeft;
            btnRelAnalitico.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRelAnalitico.UseVisualStyleBackColor = false;
            btnRelAnalitico.Click += btnRelAnalitico_Click;
            // 
            // btnRelDiario
            // 
            btnRelDiario.BackColor = Color.FromArgb(61, 61, 91);
            btnRelDiario.Dock = DockStyle.Top;
            btnRelDiario.FlatAppearance.BorderSize = 0;
            btnRelDiario.FlatStyle = FlatStyle.Flat;
            btnRelDiario.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRelDiario.ForeColor = Color.Gainsboro;
            btnRelDiario.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelDiario.Location = new Point(0, 0);
            btnRelDiario.Name = "btnRelDiario";
            btnRelDiario.Padding = new Padding(30, 0, 0, 0);
            btnRelDiario.Size = new Size(203, 40);
            btnRelDiario.TabIndex = 7;
            btnRelDiario.Text = "Diário";
            btnRelDiario.TextAlign = ContentAlignment.MiddleLeft;
            btnRelDiario.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRelDiario.UseVisualStyleBackColor = false;
            btnRelDiario.Click += btnRelDiario_Click;
            // 
            // btnRelatorios
            // 
            btnRelatorios.BackColor = Color.FromArgb(51, 51, 76);
            btnRelatorios.Dock = DockStyle.Top;
            btnRelatorios.FlatAppearance.BorderSize = 0;
            btnRelatorios.FlatStyle = FlatStyle.Flat;
            btnRelatorios.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRelatorios.ForeColor = Color.Gainsboro;
            btnRelatorios.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelatorios.Location = new Point(0, 360);
            btnRelatorios.Name = "btnRelatorios";
            btnRelatorios.Padding = new Padding(10, 0, 0, 0);
            btnRelatorios.Size = new Size(203, 40);
            btnRelatorios.TabIndex = 15;
            btnRelatorios.Text = "Relatórios";
            btnRelatorios.TextAlign = ContentAlignment.MiddleLeft;
            btnRelatorios.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRelatorios.UseVisualStyleBackColor = false;
            btnRelatorios.Click += btnRelatorios_Click;
            // 
            // pnlLancamentos
            // 
            pnlLancamentos.BackColor = Color.FromArgb(62, 62, 92);
            pnlLancamentos.Controls.Add(btnTransporte);
            pnlLancamentos.Controls.Add(btnMovimentacao);
            pnlLancamentos.Dock = DockStyle.Top;
            pnlLancamentos.Location = new Point(0, 280);
            pnlLancamentos.Name = "pnlLancamentos";
            pnlLancamentos.Size = new Size(203, 80);
            pnlLancamentos.TabIndex = 14;
            pnlLancamentos.Visible = false;
            // 
            // btnTransporte
            // 
            btnTransporte.BackColor = Color.FromArgb(61, 61, 91);
            btnTransporte.Dock = DockStyle.Top;
            btnTransporte.FlatAppearance.BorderSize = 0;
            btnTransporte.FlatStyle = FlatStyle.Flat;
            btnTransporte.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTransporte.ForeColor = Color.Gainsboro;
            btnTransporte.ImageAlign = ContentAlignment.MiddleLeft;
            btnTransporte.Location = new Point(0, 40);
            btnTransporte.Name = "btnTransporte";
            btnTransporte.Padding = new Padding(30, 0, 0, 0);
            btnTransporte.Size = new Size(203, 40);
            btnTransporte.TabIndex = 8;
            btnTransporte.Text = "Transportar Saldo";
            btnTransporte.TextAlign = ContentAlignment.MiddleLeft;
            btnTransporte.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnTransporte.UseVisualStyleBackColor = false;
            btnTransporte.Click += btnTransporte_Click;
            // 
            // btnMovimentacao
            // 
            btnMovimentacao.BackColor = Color.FromArgb(61, 61, 91);
            btnMovimentacao.Dock = DockStyle.Top;
            btnMovimentacao.FlatAppearance.BorderSize = 0;
            btnMovimentacao.FlatStyle = FlatStyle.Flat;
            btnMovimentacao.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMovimentacao.ForeColor = Color.Gainsboro;
            btnMovimentacao.ImageAlign = ContentAlignment.MiddleLeft;
            btnMovimentacao.Location = new Point(0, 0);
            btnMovimentacao.Name = "btnMovimentacao";
            btnMovimentacao.Padding = new Padding(30, 0, 0, 0);
            btnMovimentacao.Size = new Size(203, 40);
            btnMovimentacao.TabIndex = 7;
            btnMovimentacao.Text = "Movimentação";
            btnMovimentacao.TextAlign = ContentAlignment.MiddleLeft;
            btnMovimentacao.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnMovimentacao.UseVisualStyleBackColor = false;
            btnMovimentacao.Click += btnMovimentacao_Click;
            // 
            // btnLancamentos
            // 
            btnLancamentos.BackColor = Color.FromArgb(51, 51, 76);
            btnLancamentos.Dock = DockStyle.Top;
            btnLancamentos.FlatAppearance.BorderSize = 0;
            btnLancamentos.FlatStyle = FlatStyle.Flat;
            btnLancamentos.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLancamentos.ForeColor = Color.Gainsboro;
            btnLancamentos.ImageAlign = ContentAlignment.MiddleLeft;
            btnLancamentos.Location = new Point(0, 240);
            btnLancamentos.Name = "btnLancamentos";
            btnLancamentos.Padding = new Padding(10, 0, 0, 0);
            btnLancamentos.Size = new Size(203, 40);
            btnLancamentos.TabIndex = 13;
            btnLancamentos.Text = "Lançamentos";
            btnLancamentos.TextAlign = ContentAlignment.MiddleLeft;
            btnLancamentos.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLancamentos.UseVisualStyleBackColor = false;
            btnLancamentos.Click += btnLancamentos_Click;
            // 
            // pnlCadastros
            // 
            pnlCadastros.BackColor = Color.FromArgb(62, 62, 92);
            pnlCadastros.Controls.Add(btnUsuarios);
            pnlCadastros.Controls.Add(btnHistoricos);
            pnlCadastros.Controls.Add(btnContas);
            pnlCadastros.Dock = DockStyle.Top;
            pnlCadastros.Location = new Point(0, 120);
            pnlCadastros.Name = "pnlCadastros";
            pnlCadastros.Size = new Size(203, 120);
            pnlCadastros.TabIndex = 12;
            pnlCadastros.Visible = false;
            // 
            // btnUsuarios
            // 
            btnUsuarios.BackColor = Color.FromArgb(61, 61, 91);
            btnUsuarios.Dock = DockStyle.Top;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUsuarios.ForeColor = Color.Gainsboro;
            btnUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.Location = new Point(0, 80);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(30, 0, 0, 0);
            btnUsuarios.Size = new Size(203, 40);
            btnUsuarios.TabIndex = 9;
            btnUsuarios.Text = "Usuários";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUsuarios.UseVisualStyleBackColor = false;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnHistoricos
            // 
            btnHistoricos.BackColor = Color.FromArgb(61, 61, 91);
            btnHistoricos.Dock = DockStyle.Top;
            btnHistoricos.FlatAppearance.BorderSize = 0;
            btnHistoricos.FlatStyle = FlatStyle.Flat;
            btnHistoricos.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHistoricos.ForeColor = Color.Gainsboro;
            btnHistoricos.ImageAlign = ContentAlignment.MiddleLeft;
            btnHistoricos.Location = new Point(0, 40);
            btnHistoricos.Name = "btnHistoricos";
            btnHistoricos.Padding = new Padding(30, 0, 0, 0);
            btnHistoricos.Size = new Size(203, 40);
            btnHistoricos.TabIndex = 8;
            btnHistoricos.Text = "Históricos";
            btnHistoricos.TextAlign = ContentAlignment.MiddleLeft;
            btnHistoricos.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHistoricos.UseVisualStyleBackColor = false;
            btnHistoricos.Click += btnHistoricos_Click;
            // 
            // btnContas
            // 
            btnContas.BackColor = Color.FromArgb(61, 61, 91);
            btnContas.Dock = DockStyle.Top;
            btnContas.FlatAppearance.BorderSize = 0;
            btnContas.FlatStyle = FlatStyle.Flat;
            btnContas.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnContas.ForeColor = Color.Gainsboro;
            btnContas.ImageAlign = ContentAlignment.MiddleLeft;
            btnContas.Location = new Point(0, 0);
            btnContas.Name = "btnContas";
            btnContas.Padding = new Padding(30, 0, 0, 0);
            btnContas.Size = new Size(203, 40);
            btnContas.TabIndex = 7;
            btnContas.Text = "Contas";
            btnContas.TextAlign = ContentAlignment.MiddleLeft;
            btnContas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnContas.UseVisualStyleBackColor = false;
            btnContas.Click += btnContas_Click;
            // 
            // btnLogoff
            // 
            btnLogoff.BackColor = Color.FromArgb(51, 51, 76);
            btnLogoff.Dock = DockStyle.Bottom;
            btnLogoff.FlatAppearance.BorderSize = 0;
            btnLogoff.FlatStyle = FlatStyle.Flat;
            btnLogoff.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogoff.ForeColor = Color.Gainsboro;
            btnLogoff.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogoff.Location = new Point(0, 640);
            btnLogoff.Name = "btnLogoff";
            btnLogoff.Padding = new Padding(10, 0, 0, 0);
            btnLogoff.Size = new Size(203, 40);
            btnLogoff.TabIndex = 11;
            btnLogoff.Text = "Menu Inicial";
            btnLogoff.TextAlign = ContentAlignment.MiddleLeft;
            btnLogoff.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLogoff.UseVisualStyleBackColor = false;
            btnLogoff.Click += btnLogoff_Click;
            // 
            // btnCadastro
            // 
            btnCadastro.BackColor = Color.FromArgb(51, 51, 76);
            btnCadastro.Dock = DockStyle.Top;
            btnCadastro.FlatAppearance.BorderSize = 0;
            btnCadastro.FlatStyle = FlatStyle.Flat;
            btnCadastro.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCadastro.ForeColor = Color.Gainsboro;
            btnCadastro.ImageAlign = ContentAlignment.MiddleLeft;
            btnCadastro.Location = new Point(0, 80);
            btnCadastro.Name = "btnCadastro";
            btnCadastro.Padding = new Padding(10, 0, 0, 0);
            btnCadastro.Size = new Size(203, 40);
            btnCadastro.TabIndex = 6;
            btnCadastro.Text = "Cadastros";
            btnCadastro.TextAlign = ContentAlignment.MiddleLeft;
            btnCadastro.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCadastro.UseVisualStyleBackColor = false;
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
            pnlTitulo.Controls.Add(btnFechar);
            pnlTitulo.Controls.Add(btnMinimizar);
            pnlTitulo.Controls.Add(lblTitulo);
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
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Forte", 25F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(880, 80);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Painel Principal";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.MouseDown += lblTitulo_MouseDown;
            // 
            // pnlDesktop
            // 
            pnlDesktop.Controls.Add(panel1);
            pnlDesktop.Controls.Add(calendario);
            pnlDesktop.Controls.Add(lblRelogio);
            pnlDesktop.Dock = DockStyle.Fill;
            pnlDesktop.Location = new Point(220, 80);
            pnlDesktop.Margin = new Padding(80);
            pnlDesktop.Name = "pnlDesktop";
            pnlDesktop.Size = new Size(880, 500);
            pnlDesktop.TabIndex = 8;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblUsuario);
            panel1.Controls.Add(lblBanco);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 432);
            panel1.Name = "panel1";
            panel1.Size = new Size(880, 68);
            panel1.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Forte", 15F);
            label2.Location = new Point(27, 22);
            label2.Name = "label2";
            label2.Size = new Size(152, 22);
            label2.TabIndex = 3;
            label2.Text = "Banco de dados:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Forte", 15F);
            label1.Location = new Point(495, 22);
            label1.Name = "label1";
            label1.Size = new Size(130, 22);
            label1.TabIndex = 2;
            label1.Text = "Usuário ativo:";
            // 
            // calendario
            // 
            calendario.FirstDayOfWeek = Day.Sunday;
            calendario.Location = new Point(327, 148);
            calendario.Margin = new Padding(40, 40, 40, 5);
            calendario.Name = "calendario";
            calendario.TabIndex = 1;
            // 
            // lblRelogio
            // 
            lblRelogio.AutoSize = true;
            lblRelogio.Font = new Font("Digital-7 Mono", 26.2499962F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblRelogio.Location = new Point(364, 315);
            lblRelogio.Name = "lblRelogio";
            lblRelogio.Size = new Size(153, 37);
            lblRelogio.TabIndex = 0;
            lblRelogio.Text = "00:00:00";
            // 
            // timerRelogio
            // 
            timerRelogio.Tick += timerRelogio_Tick;
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
            pnlLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlMenuLateral.ResumeLayout(false);
            pnlRelatorios.ResumeLayout(false);
            pnlLancamentos.ResumeLayout(false);
            pnlCadastros.ResumeLayout(false);
            pnlTitulo.ResumeLayout(false);
            pnlDesktop.ResumeLayout(false);
            pnlDesktop.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLogo;
        private Button btnFechar;
        private Label lblUsuario;
        private Panel pnlMenuLateral;
        private Button btnMinimizar;
        private Label lblBanco;
        private Panel pnlTitulo;
        private Label lblTitulo;
        private Button btnLogoff;
        private Panel pnlDesktop;
        private Button btnFecharFormFilho;
        private Button btnCadastro;
        private Button btnLancamentos;
        private Panel pnlCadastros;
        private Button btnUsuarios;
        private Button btnHistoricos;
        private Button btnContas;
        private Panel pnlRelatorios;
        private Button btnSaldo;
        private Button btnBalanceteConta;
        private Button btnBalanceteGeral;
        private Button btnRelAnalitico;
        private Button btnRelDiario;
        private Button btnRelatorios;
        private Panel pnlLancamentos;
        private Button btnTransporte;
        private Button btnMovimentacao;
        private Button btnRelUsuarios;
        private MonthCalendar calendario;
        private Label lblRelogio;
        private System.Windows.Forms.Timer timerRelogio;
        private Label label1;
        private Panel panel1;
        private Label label2;
        private PictureBox picLogo;
    }
}