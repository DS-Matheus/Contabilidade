namespace Contabilidade.Forms.Relatorios
{
    partial class frmBalanceteGeral
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBalanceteGeral));
            btnVisualizar = new Button();
            label3 = new Label();
            label2 = new Label();
            dtpFinal = new DateTimePicker();
            dtpInicial = new DateTimePicker();
            label4 = new Label();
            txtSubtitulo = new TextBox();
            chkSaldosZero = new CheckBox();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnVisualizar
            // 
            btnVisualizar.Font = new Font("Lucida Sans", 10.5F);
            btnVisualizar.Location = new Point(115, 238);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(419, 38);
            btnVisualizar.TabIndex = 31;
            btnVisualizar.Text = "Visualizar relatório";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += btnVisualizar_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(115, 93);
            label3.Name = "label3";
            label3.Size = new Size(72, 16);
            label3.TabIndex = 26;
            label3.Text = "Data Final";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(115, 43);
            label2.Name = "label2";
            label2.Size = new Size(78, 16);
            label2.TabIndex = 25;
            label2.Text = "Data Inicial";
            // 
            // dtpFinal
            // 
            dtpFinal.Font = new Font("Lucida Sans", 10.5F);
            dtpFinal.Location = new Point(115, 112);
            dtpFinal.Name = "dtpFinal";
            dtpFinal.Size = new Size(419, 24);
            dtpFinal.TabIndex = 24;
            // 
            // dtpInicial
            // 
            dtpInicial.Font = new Font("Lucida Sans", 10.5F);
            dtpInicial.Location = new Point(115, 62);
            dtpInicial.Name = "dtpInicial";
            dtpInicial.Size = new Size(419, 24);
            dtpInicial.TabIndex = 23;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Lucida Sans", 10.5F);
            label4.Location = new Point(115, 144);
            label4.Name = "label4";
            label4.Size = new Size(163, 16);
            label4.TabIndex = 52;
            label4.Text = "Subtítulo personalizado";
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Font = new Font("Lucida Sans", 10.5F);
            txtSubtitulo.Location = new Point(115, 162);
            txtSubtitulo.MaxLength = 110;
            txtSubtitulo.Multiline = true;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(419, 39);
            txtSubtitulo.TabIndex = 51;
            // 
            // chkSaldosZero
            // 
            chkSaldosZero.AutoSize = true;
            chkSaldosZero.Font = new Font("Lucida Sans", 10.5F);
            chkSaldosZero.Location = new Point(225, 211);
            chkSaldosZero.Name = "chkSaldosZero";
            chkSaldosZero.Size = new Size(198, 20);
            chkSaldosZero.TabIndex = 53;
            chkSaldosZero.Text = "Incluir contas com saldo 0";
            chkSaldosZero.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = SystemColors.Control;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnVisualizar);
            panel1.Controls.Add(chkSaldosZero);
            panel1.Controls.Add(dtpInicial);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(dtpFinal);
            panel1.Controls.Add(txtSubtitulo);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(115, 89);
            panel1.Name = "panel1";
            panel1.Size = new Size(650, 320);
            panel1.TabIndex = 54;
            // 
            // frmBalanceteGeral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(880, 500);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmBalanceteGeral";
            Text = "Balancete Geral";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnVisualizar;
        private Label label3;
        private Label label2;
        private DateTimePicker dtpFinal;
        private DateTimePicker dtpInicial;
        private Label label4;
        private TextBox txtSubtitulo;
        private CheckBox chkSaldosZero;
        private Panel panel1;
    }
}