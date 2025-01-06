namespace Contabilidade.Forms.Cadastros
{
    partial class frmDefinirCabecalho
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
            txtSubtitulo = new TextBox();
            label2 = new Label();
            txtTitulo = new TextBox();
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
            pnlBarraTitulo.Size = new Size(398, 30);
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
            btnMinimizar.Location = new Point(338, 0);
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
            lblTitulo.Font = new Font("Lucida Sans", 10.5F);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 7);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(203, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Definir cabeçalho do relatório";
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
            btnFechar.TabIndex = 5;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtSubtitulo);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtTitulo);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnSalvar);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(398, 210);
            panel1.TabIndex = 8;
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Font = new Font("Lucida Sans", 10F);
            txtSubtitulo.Location = new Point(32, 113);
            txtSubtitulo.MaxLength = 100;
            txtSubtitulo.Multiline = true;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(336, 40);
            txtSubtitulo.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10F);
            label2.Location = new Point(32, 95);
            label2.Name = "label2";
            label2.Size = new Size(65, 16);
            label2.TabIndex = 5;
            label2.Text = "Subtítulo";
            // 
            // txtTitulo
            // 
            txtTitulo.Font = new Font("Lucida Sans", 10F);
            txtTitulo.Location = new Point(32, 39);
            txtTitulo.MaxLength = 100;
            txtTitulo.Multiline = true;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(336, 40);
            txtTitulo.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 10F);
            label1.Location = new Point(32, 21);
            label1.Name = "label1";
            label1.Size = new Size(44, 16);
            label1.TabIndex = 1;
            label1.Text = "Título";
            // 
            // btnSalvar
            // 
            btnSalvar.Font = new Font("Lucida Sans", 10F);
            btnSalvar.Location = new Point(32, 166);
            btnSalvar.Margin = new Padding(3, 10, 3, 3);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(336, 32);
            btnSalvar.TabIndex = 3;
            btnSalvar.Text = "Definir cabeçalho";
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // frmDefinirCabecalho
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(398, 240);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmDefinirCabecalho";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmObterSubtítulo";
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
        private TextBox txtTitulo;
        private Label label1;
        private Button btnSalvar;
        private TextBox txtSubtitulo;
        private Label label2;
    }
}