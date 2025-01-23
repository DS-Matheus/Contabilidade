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
            lblData = new Label();
            lblRelogio = new Label();
            picLogo = new PictureBox();
            lblBanco = new Label();
            lblUsuario = new Label();
            btnFechar = new Button();
            pnlMenuLateral = new Panel();
            btnSistema = new Button();
            pnlBackup = new Panel();
            btnRestaurar = new Button();
            btnBackup = new Button();
            pnlRelatorios = new Panel();
            btnSaldo = new Button();
            btnBalanceteConta = new Button();
            btnBalanceteGeral = new Button();
            btnRelAnalitico = new Button();
            btnRelDiario = new Button();
            btnRelatorios = new Button();
            btnLancamentos = new Button();
            pnlCadastros = new Panel();
            btnUsuarios = new Button();
            btnHistoricos = new Button();
            btnContas = new Button();
            btnLogoff = new Button();
            btnCadastro = new Button();
            btnMinimizar = new Button();
            pnlTitulo = new Panel();
            btnMaximizar = new Button();
            btnFecharFormFilho = new Button();
            lblTitulo = new Label();
            pnlDesktop = new Panel();
            calendario = new MonthCalendar();
            label2 = new Label();
            label1 = new Label();
            timerRelogio = new System.Windows.Forms.Timer(components);
            pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlMenuLateral.SuspendLayout();
            pnlBackup.SuspendLayout();
            pnlRelatorios.SuspendLayout();
            pnlCadastros.SuspendLayout();
            pnlTitulo.SuspendLayout();
            pnlDesktop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLogo
            // 
            pnlLogo.BackColor = Color.FromArgb(39, 39, 58);
            pnlLogo.Controls.Add(lblData);
            pnlLogo.Controls.Add(lblRelogio);
            pnlLogo.Controls.Add(picLogo);
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Location = new Point(0, 0);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(203, 80);
            pnlLogo.TabIndex = 5;
            // 
            // lblData
            // 
            lblData.Anchor = AnchorStyles.None;
            lblData.AutoSize = true;
            lblData.Font = new Font("Microsoft Sans Serif", 12F);
            lblData.ForeColor = Color.Gainsboro;
            lblData.Location = new Point(57, 46);
            lblData.Name = "lblData";
            lblData.Size = new Size(89, 20);
            lblData.TabIndex = 3;
            lblData.Text = "00/00/0000";
            // 
            // lblRelogio
            // 
            lblRelogio.Anchor = AnchorStyles.None;
            lblRelogio.AutoSize = true;
            lblRelogio.Font = new Font("Microsoft Sans Serif", 20F);
            lblRelogio.ForeColor = Color.Gainsboro;
            lblRelogio.Location = new Point(41, 16);
            lblRelogio.Name = "lblRelogio";
            lblRelogio.Size = new Size(120, 31);
            lblRelogio.TabIndex = 0;
            lblRelogio.Text = "00:00:00";
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
            lblBanco.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblBanco.BackColor = Color.Transparent;
            lblBanco.Font = new Font("Lucida Sans", 11.25F);
            lblBanco.ForeColor = Color.Black;
            lblBanco.Location = new Point(182, 458);
            lblBanco.Name = "lblBanco";
            lblBanco.Size = new Size(304, 22);
            lblBanco.TabIndex = 2;
            lblBanco.Text = "Banco";
            lblBanco.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUsuario
            // 
            lblUsuario.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Font = new Font("Lucida Sans", 11.25F);
            lblUsuario.ForeColor = Color.Black;
            lblUsuario.Location = new Point(628, 458);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(222, 22);
            lblUsuario.TabIndex = 1;
            lblUsuario.Text = "Usuário";
            lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnFechar
            // 
            btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatStyle = FlatStyle.Flat;
            btnFechar.Font = new Font("Microsoft Sans Serif", 11F);
            btnFechar.ForeColor = SystemColors.Control;
            btnFechar.Location = new Point(820, 25);
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
            pnlMenuLateral.Controls.Add(btnSistema);
            pnlMenuLateral.Controls.Add(pnlBackup);
            pnlMenuLateral.Controls.Add(pnlRelatorios);
            pnlMenuLateral.Controls.Add(btnRelatorios);
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
            // btnSistema
            // 
            btnSistema.BackColor = Color.FromArgb(51, 51, 76);
            btnSistema.Dock = DockStyle.Bottom;
            btnSistema.FlatAppearance.BorderSize = 0;
            btnSistema.FlatStyle = FlatStyle.Flat;
            btnSistema.Font = new Font("Lucida Sans", 11.25F);
            btnSistema.ForeColor = Color.Gainsboro;
            btnSistema.ImageAlign = ContentAlignment.MiddleLeft;
            btnSistema.Location = new Point(0, 520);
            btnSistema.Name = "btnSistema";
            btnSistema.Padding = new Padding(10, 0, 0, 0);
            btnSistema.Size = new Size(203, 40);
            btnSistema.TabIndex = 18;
            btnSistema.Text = "Sistema";
            btnSistema.TextAlign = ContentAlignment.MiddleLeft;
            btnSistema.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSistema.UseVisualStyleBackColor = false;
            btnSistema.Click += btnSistema_Click;
            // 
            // pnlBackup
            // 
            pnlBackup.Controls.Add(btnRestaurar);
            pnlBackup.Controls.Add(btnBackup);
            pnlBackup.Dock = DockStyle.Bottom;
            pnlBackup.Location = new Point(0, 560);
            pnlBackup.Name = "pnlBackup";
            pnlBackup.Size = new Size(203, 80);
            pnlBackup.TabIndex = 17;
            pnlBackup.Visible = false;
            // 
            // btnRestaurar
            // 
            btnRestaurar.BackColor = Color.FromArgb(61, 61, 91);
            btnRestaurar.Dock = DockStyle.Top;
            btnRestaurar.FlatAppearance.BorderSize = 0;
            btnRestaurar.FlatStyle = FlatStyle.Flat;
            btnRestaurar.Font = new Font("Lucida Sans", 10F);
            btnRestaurar.ForeColor = Color.Gainsboro;
            btnRestaurar.ImageAlign = ContentAlignment.MiddleLeft;
            btnRestaurar.Location = new Point(0, 40);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Padding = new Padding(20, 0, 0, 0);
            btnRestaurar.Size = new Size(203, 40);
            btnRestaurar.TabIndex = 9;
            btnRestaurar.Text = "Restaurar";
            btnRestaurar.TextAlign = ContentAlignment.MiddleLeft;
            btnRestaurar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRestaurar.UseVisualStyleBackColor = false;
            btnRestaurar.Click += btnRestaurar_Click;
            // 
            // btnBackup
            // 
            btnBackup.BackColor = Color.FromArgb(61, 61, 91);
            btnBackup.Dock = DockStyle.Top;
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.Font = new Font("Lucida Sans", 10F);
            btnBackup.ForeColor = Color.Gainsboro;
            btnBackup.ImageAlign = ContentAlignment.MiddleLeft;
            btnBackup.Location = new Point(0, 0);
            btnBackup.Name = "btnBackup";
            btnBackup.Padding = new Padding(20, 0, 0, 0);
            btnBackup.Size = new Size(203, 40);
            btnBackup.TabIndex = 8;
            btnBackup.Text = "Backup";
            btnBackup.TextAlign = ContentAlignment.MiddleLeft;
            btnBackup.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnBackup.UseVisualStyleBackColor = false;
            btnBackup.Click += btnBackup_Click;
            // 
            // pnlRelatorios
            // 
            pnlRelatorios.BackColor = Color.FromArgb(62, 62, 92);
            pnlRelatorios.Controls.Add(btnSaldo);
            pnlRelatorios.Controls.Add(btnBalanceteConta);
            pnlRelatorios.Controls.Add(btnBalanceteGeral);
            pnlRelatorios.Controls.Add(btnRelAnalitico);
            pnlRelatorios.Controls.Add(btnRelDiario);
            pnlRelatorios.Dock = DockStyle.Top;
            pnlRelatorios.Location = new Point(0, 320);
            pnlRelatorios.Name = "pnlRelatorios";
            pnlRelatorios.Size = new Size(203, 200);
            pnlRelatorios.TabIndex = 16;
            pnlRelatorios.Visible = false;
            // 
            // btnSaldo
            // 
            btnSaldo.BackColor = Color.FromArgb(61, 61, 91);
            btnSaldo.Dock = DockStyle.Top;
            btnSaldo.FlatAppearance.BorderSize = 0;
            btnSaldo.FlatStyle = FlatStyle.Flat;
            btnSaldo.Font = new Font("Lucida Sans", 10F);
            btnSaldo.ForeColor = Color.Gainsboro;
            btnSaldo.ImageAlign = ContentAlignment.MiddleLeft;
            btnSaldo.Location = new Point(0, 160);
            btnSaldo.Name = "btnSaldo";
            btnSaldo.Padding = new Padding(20, 0, 0, 0);
            btnSaldo.Size = new Size(203, 40);
            btnSaldo.TabIndex = 11;
            btnSaldo.Text = "Saldo de Contas";
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
            btnBalanceteConta.Font = new Font("Lucida Sans", 10F);
            btnBalanceteConta.ForeColor = Color.Gainsboro;
            btnBalanceteConta.ImageAlign = ContentAlignment.MiddleLeft;
            btnBalanceteConta.Location = new Point(0, 120);
            btnBalanceteConta.Name = "btnBalanceteConta";
            btnBalanceteConta.Padding = new Padding(20, 0, 0, 0);
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
            btnBalanceteGeral.Font = new Font("Lucida Sans", 10F);
            btnBalanceteGeral.ForeColor = Color.Gainsboro;
            btnBalanceteGeral.ImageAlign = ContentAlignment.MiddleLeft;
            btnBalanceteGeral.Location = new Point(0, 80);
            btnBalanceteGeral.Name = "btnBalanceteGeral";
            btnBalanceteGeral.Padding = new Padding(20, 0, 0, 0);
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
            btnRelAnalitico.Font = new Font("Lucida Sans", 10F);
            btnRelAnalitico.ForeColor = Color.Gainsboro;
            btnRelAnalitico.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelAnalitico.Location = new Point(0, 40);
            btnRelAnalitico.Name = "btnRelAnalitico";
            btnRelAnalitico.Padding = new Padding(20, 0, 0, 0);
            btnRelAnalitico.Size = new Size(203, 40);
            btnRelAnalitico.TabIndex = 8;
            btnRelAnalitico.Text = "Razão Analítico";
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
            btnRelDiario.Font = new Font("Lucida Sans", 10F);
            btnRelDiario.ForeColor = Color.Gainsboro;
            btnRelDiario.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelDiario.Location = new Point(0, 0);
            btnRelDiario.Name = "btnRelDiario";
            btnRelDiario.Padding = new Padding(20, 0, 0, 0);
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
            btnRelatorios.Font = new Font("Lucida Sans", 11.25F);
            btnRelatorios.ForeColor = Color.Gainsboro;
            btnRelatorios.ImageAlign = ContentAlignment.MiddleLeft;
            btnRelatorios.Location = new Point(0, 280);
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
            // btnLancamentos
            // 
            btnLancamentos.BackColor = Color.FromArgb(51, 51, 76);
            btnLancamentos.Dock = DockStyle.Top;
            btnLancamentos.FlatAppearance.BorderSize = 0;
            btnLancamentos.FlatStyle = FlatStyle.Flat;
            btnLancamentos.Font = new Font("Lucida Sans", 11.25F);
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
            btnUsuarios.Font = new Font("Lucida Sans", 10F);
            btnUsuarios.ForeColor = Color.Gainsboro;
            btnUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.Location = new Point(0, 80);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(20, 0, 0, 0);
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
            btnHistoricos.Font = new Font("Lucida Sans", 10F);
            btnHistoricos.ForeColor = Color.Gainsboro;
            btnHistoricos.ImageAlign = ContentAlignment.MiddleLeft;
            btnHistoricos.Location = new Point(0, 40);
            btnHistoricos.Name = "btnHistoricos";
            btnHistoricos.Padding = new Padding(20, 0, 0, 0);
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
            btnContas.Font = new Font("Lucida Sans", 10F);
            btnContas.ForeColor = Color.Gainsboro;
            btnContas.ImageAlign = ContentAlignment.MiddleLeft;
            btnContas.Location = new Point(0, 0);
            btnContas.Name = "btnContas";
            btnContas.Padding = new Padding(20, 0, 0, 0);
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
            btnLogoff.Font = new Font("Lucida Sans", 11.25F);
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
            btnCadastro.Font = new Font("Lucida Sans", 11.25F);
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
            btnMinimizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimizar.Cursor = Cursors.Hand;
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatStyle = FlatStyle.Flat;
            btnMinimizar.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            btnMinimizar.ForeColor = SystemColors.Control;
            btnMinimizar.Location = new Point(748, 25);
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
            pnlTitulo.Controls.Add(btnMaximizar);
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
            // btnMaximizar
            // 
            btnMaximizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximizar.Cursor = Cursors.Hand;
            btnMaximizar.FlatAppearance.BorderSize = 0;
            btnMaximizar.FlatStyle = FlatStyle.Flat;
            btnMaximizar.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            btnMaximizar.ForeColor = SystemColors.Control;
            btnMaximizar.Location = new Point(784, 25);
            btnMaximizar.Name = "btnMaximizar";
            btnMaximizar.Size = new Size(30, 30);
            btnMaximizar.TabIndex = 5;
            btnMaximizar.Text = "◧";
            btnMaximizar.UseVisualStyleBackColor = true;
            btnMaximizar.Click += btnMaximizar_Click;
            // 
            // btnFecharFormFilho
            // 
            btnFecharFormFilho.Dock = DockStyle.Left;
            btnFecharFormFilho.FlatAppearance.BorderSize = 0;
            btnFecharFormFilho.FlatStyle = FlatStyle.Flat;
            btnFecharFormFilho.Font = new Font("Microsoft Sans Serif", 15F);
            btnFecharFormFilho.Image = Properties.Resources.home;
            btnFecharFormFilho.Location = new Point(0, 0);
            btnFecharFormFilho.Name = "btnFecharFormFilho";
            btnFecharFormFilho.Size = new Size(75, 80);
            btnFecharFormFilho.TabIndex = 4;
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
            pnlDesktop.Controls.Add(lblUsuario);
            pnlDesktop.Controls.Add(lblBanco);
            pnlDesktop.Controls.Add(calendario);
            pnlDesktop.Controls.Add(label2);
            pnlDesktop.Controls.Add(label1);
            pnlDesktop.Dock = DockStyle.Fill;
            pnlDesktop.Location = new Point(220, 80);
            pnlDesktop.Margin = new Padding(80);
            pnlDesktop.Name = "pnlDesktop";
            pnlDesktop.Size = new Size(880, 500);
            pnlDesktop.TabIndex = 8;
            // 
            // calendario
            // 
            calendario.Anchor = AnchorStyles.None;
            calendario.CalendarDimensions = new Size(3, 2);
            calendario.FirstDayOfWeek = Day.Sunday;
            calendario.Font = new Font("Lucida Sans", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            calendario.Location = new Point(94, 96);
            calendario.Margin = new Padding(40, 40, 40, 5);
            calendario.Name = "calendario";
            calendario.ScrollChange = 6;
            calendario.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Font = new Font("Forte", 15F);
            label2.Location = new Point(27, 458);
            label2.Name = "label2";
            label2.Size = new Size(152, 22);
            label2.TabIndex = 3;
            label2.Text = "Banco de dados:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Forte", 15F);
            label1.Location = new Point(492, 458);
            label1.Name = "label1";
            label1.Size = new Size(130, 22);
            label1.TabIndex = 2;
            label1.Text = "Usuário ativo:";
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
            StartPosition = FormStartPosition.CenterParent;
            Text = "PainelPrincipal";
            WindowState = FormWindowState.Maximized;
            pnlLogo.ResumeLayout(false);
            pnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlMenuLateral.ResumeLayout(false);
            pnlBackup.ResumeLayout(false);
            pnlRelatorios.ResumeLayout(false);
            pnlCadastros.ResumeLayout(false);
            pnlTitulo.ResumeLayout(false);
            pnlDesktop.ResumeLayout(false);
            pnlDesktop.PerformLayout();
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
        private MonthCalendar calendario;
        private Label lblRelogio;
        private System.Windows.Forms.Timer timerRelogio;
        private Label label1;
        private Label label2;
        private PictureBox picLogo;
        private Button btnSistema;
        private Panel pnlBackup;
        private Button btnRestaurar;
        private Button btnBackup;
        private Button btnMaximizar;
        private Label lblData;
    }
}