namespace Contabilidade.Forms.Mensagens
{
    partial class frmCustomMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomMessage));
            pnlBarraTitulo = new Panel();
            btnMinimizar = new Button();
            lblTitulo = new Label();
            btnFechar = new Button();
            panel2 = new Panel();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            panel1 = new Panel();
            imgAviso = new PictureBox();
            imgInfo = new PictureBox();
            imgSucesso = new PictureBox();
            imgErro = new PictureBox();
            lblMensagem = new Label();
            pnlBarraTitulo.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgAviso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgInfo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgSucesso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgErro).BeginInit();
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
            pnlBarraTitulo.Size = new Size(553, 30);
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
            btnMinimizar.Location = new Point(493, 0);
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
            lblTitulo.Size = new Size(139, 16);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Título da mensagem";
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
            btnFechar.Location = new Point(523, 0);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(30, 30);
            btnFechar.TabIndex = 5;
            btnFechar.Text = "X";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ButtonFace;
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 142);
            panel2.Name = "panel2";
            panel2.Size = new Size(553, 40);
            panel2.TabIndex = 9;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.None;
            button3.Location = new Point(320, 5);
            button3.Name = "button3";
            button3.Size = new Size(75, 30);
            button3.TabIndex = 2;
            button3.Text = "Cancelar";
            button3.UseVisualStyleBackColor = true;
            button3.Visible = false;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.None;
            button2.Location = new Point(239, 5);
            button2.Name = "button2";
            button2.Size = new Size(75, 30);
            button2.TabIndex = 1;
            button2.Text = "Não";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.None;
            button1.Location = new Point(158, 5);
            button1.Name = "button1";
            button1.Size = new Size(75, 30);
            button1.TabIndex = 0;
            button1.Text = "Sim";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLightLight;
            panel1.Controls.Add(lblMensagem);
            panel1.Controls.Add(imgAviso);
            panel1.Controls.Add(imgInfo);
            panel1.Controls.Add(imgSucesso);
            panel1.Controls.Add(imgErro);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 30);
            panel1.Name = "panel1";
            panel1.Size = new Size(553, 152);
            panel1.TabIndex = 8;
            // 
            // imgAviso
            // 
            imgAviso.Image = (Image)resources.GetObject("imgAviso.Image");
            imgAviso.Location = new Point(12, 31);
            imgAviso.Name = "imgAviso";
            imgAviso.Size = new Size(50, 50);
            imgAviso.SizeMode = PictureBoxSizeMode.Zoom;
            imgAviso.TabIndex = 6;
            imgAviso.TabStop = false;
            imgAviso.Visible = false;
            // 
            // imgInfo
            // 
            imgInfo.Image = (Image)resources.GetObject("imgInfo.Image");
            imgInfo.Location = new Point(12, 31);
            imgInfo.Name = "imgInfo";
            imgInfo.Size = new Size(50, 50);
            imgInfo.SizeMode = PictureBoxSizeMode.Zoom;
            imgInfo.TabIndex = 5;
            imgInfo.TabStop = false;
            imgInfo.Visible = false;
            // 
            // imgSucesso
            // 
            imgSucesso.Image = (Image)resources.GetObject("imgSucesso.Image");
            imgSucesso.Location = new Point(12, 31);
            imgSucesso.Name = "imgSucesso";
            imgSucesso.Size = new Size(50, 50);
            imgSucesso.SizeMode = PictureBoxSizeMode.Zoom;
            imgSucesso.TabIndex = 4;
            imgSucesso.TabStop = false;
            imgSucesso.Visible = false;
            // 
            // imgErro
            // 
            imgErro.Image = (Image)resources.GetObject("imgErro.Image");
            imgErro.Location = new Point(12, 31);
            imgErro.Name = "imgErro";
            imgErro.Size = new Size(50, 50);
            imgErro.SizeMode = PictureBoxSizeMode.Zoom;
            imgErro.TabIndex = 3;
            imgErro.TabStop = false;
            imgErro.Visible = false;
            // 
            // lblMensagem
            // 
            lblMensagem.AutoSize = true;
            lblMensagem.Location = new Point(77, 13);
            lblMensagem.MaximumSize = new Size(462, 87);
            lblMensagem.Name = "lblMensagem";
            lblMensagem.Size = new Size(158, 16);
            lblMensagem.TabIndex = 7;
            lblMensagem.Text = "Mensagem de exemplo";
            lblMensagem.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmCustomMessage
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(553, 182);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pnlBarraTitulo);
            Font = new Font("Lucida Sans", 10.5F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmCustomMessage";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmMensagemTripla";
            pnlBarraTitulo.ResumeLayout(false);
            pnlBarraTitulo.PerformLayout();
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgAviso).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgInfo).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgSucesso).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgErro).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBarraTitulo;
        private Button btnMinimizar;
        private Label lblTitulo;
        private Button btnFechar;
        private Panel panel2;
        private Button button2;
        private Button button1;
        private Panel panel1;
        private Button button3;
        private PictureBox imgErro;
        private PictureBox imgSucesso;
        private PictureBox imgAviso;
        private PictureBox imgInfo;
        private Label lblMensagem;
    }
}