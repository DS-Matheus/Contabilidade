namespace Contabilidade.Forms
{
    partial class frmTransferirBD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransferirBD));
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            lblTitulo = new Label();
            btnFechar = new Button();
            panel1 = new Panel();
            txtBancoDados = new TextBox();
            label3 = new Label();
            btnTransferir = new Button();
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
            btnMinimizar.Location = new Point(338, 0);
            btnMinimizar.Name = "btnMinimizar";
            btnMinimizar.Size = new Size(30, 30);
            btnMinimizar.TabIndex = 3;
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
            lblTitulo.Size = new Size(186, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Renomear banco de dados";
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
            btnFechar.TabIndex = 4;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtBancoDados);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(btnTransferir);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(398, 120);
            panel1.TabIndex = 9;
            // 
            // txtBancoDados
            // 
            txtBancoDados.Font = new Font("Lucida Sans", 10.5F);
            txtBancoDados.Location = new Point(32, 35);
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
            label3.Location = new Point(32, 17);
            label3.Margin = new Padding(3, 10, 3, 0);
            label3.Name = "label3";
            label3.Size = new Size(115, 16);
            label3.TabIndex = 5;
            label3.Text = "Banco de dados";
            // 
            // btnTransferir
            // 
            btnTransferir.Font = new Font("Lucida Sans", 10.5F);
            btnTransferir.Location = new Point(32, 71);
            btnTransferir.Margin = new Padding(3, 10, 3, 3);
            btnTransferir.Name = "btnTransferir";
            btnTransferir.Size = new Size(335, 32);
            btnTransferir.TabIndex = 2;
            btnTransferir.Text = "Transferir";
            btnTransferir.UseVisualStyleBackColor = true;
            btnTransferir.Click += btnTransferir_Click;
            // 
            // frmTransferirBD
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(398, 150);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmTransferirBD";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmTransferirBD";
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
        private Button btnTransferir;
    }
}