namespace Contabilidade.Forms.Lancamentos
{
    partial class frmDatasCalcula
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
            dtpDataInicial = new DateTimePicker();
            label4 = new Label();
            btnConfirmar = new Button();
            dtpDataFinal = new DateTimePicker();
            label1 = new Label();
            pnlBarraTitulo.SuspendLayout();
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
            pnlBarraTitulo.Size = new Size(300, 30);
            pnlBarraTitulo.TabIndex = 8;
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
            btnMinimizar.Location = new Point(240, 0);
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
            lblTitulo.Size = new Size(166, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Informe o período desejado";
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
            btnFechar.Location = new Point(270, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 5;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // dtpDataInicial
            // 
            dtpDataInicial.Location = new Point(16, 64);
            dtpDataInicial.Name = "dtpDataInicial";
            dtpDataInicial.Size = new Size(269, 23);
            dtpDataInicial.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 46);
            label4.Margin = new Padding(5);
            label4.Name = "label4";
            label4.Size = new Size(65, 15);
            label4.TabIndex = 11;
            label4.Text = "Data Inicial";
            // 
            // btnConfirmar
            // 
            btnConfirmar.Location = new Point(98, 149);
            btnConfirmar.Margin = new Padding(3, 10, 3, 3);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(105, 32);
            btnConfirmar.TabIndex = 10;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // dtpDataFinal
            // 
            dtpDataFinal.Location = new Point(16, 113);
            dtpDataFinal.Name = "dtpDataFinal";
            dtpDataFinal.Size = new Size(269, 23);
            dtpDataFinal.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 95);
            label1.Margin = new Padding(5);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 13;
            label1.Text = "Data Final";
            // 
            // frmDatasCalcula
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 200);
            Controls.Add(dtpDataFinal);
            Controls.Add(label1);
            Controls.Add(dtpDataInicial);
            Controls.Add(label4);
            Controls.Add(btnConfirmar);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmDatasCalcula";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmDatasCalcula";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private DateTimePicker dtpDataInicial;
        private Label label4;
        private Button btnConfirmar;
        private DateTimePicker dtpDataFinal;
        private Label label1;
    }
}