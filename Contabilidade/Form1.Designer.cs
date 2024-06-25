namespace Contabilidade
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnRenomearBD = new Button();
            btnExcluirBD = new Button();
            btnCriarBD = new Button();
            cbbBD = new ComboBox();
            groupBox2 = new GroupBox();
            btnEntrar = new Button();
            txtSenha = new TextBox();
            label2 = new Label();
            txtNome = new TextBox();
            label1 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRenomearBD);
            groupBox1.Controls.Add(btnExcluirBD);
            groupBox1.Controls.Add(btnCriarBD);
            groupBox1.Controls.Add(cbbBD);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(300, 148);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Banco de Dados";
            // 
            // btnRenomearBD
            // 
            btnRenomearBD.Location = new Point(202, 85);
            btnRenomearBD.Name = "btnRenomearBD";
            btnRenomearBD.Size = new Size(75, 23);
            btnRenomearBD.TabIndex = 3;
            btnRenomearBD.Text = "Renomear";
            btnRenomearBD.UseVisualStyleBackColor = true;
            // 
            // btnExcluirBD
            // 
            btnExcluirBD.Location = new Point(110, 85);
            btnExcluirBD.Name = "btnExcluirBD";
            btnExcluirBD.Size = new Size(75, 23);
            btnExcluirBD.TabIndex = 2;
            btnExcluirBD.Text = "Excluir";
            btnExcluirBD.UseVisualStyleBackColor = true;
            // 
            // btnCriarBD
            // 
            btnCriarBD.Location = new Point(17, 85);
            btnCriarBD.Name = "btnCriarBD";
            btnCriarBD.Size = new Size(75, 23);
            btnCriarBD.TabIndex = 1;
            btnCriarBD.Text = "Criar";
            btnCriarBD.UseVisualStyleBackColor = true;
            // 
            // cbbBD
            // 
            cbbBD.FormattingEnabled = true;
            cbbBD.Location = new Point(17, 39);
            cbbBD.Margin = new Padding(20);
            cbbBD.Name = "cbbBD";
            cbbBD.Size = new Size(260, 23);
            cbbBD.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnEntrar);
            groupBox2.Controls.Add(txtSenha);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtNome);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(12, 166);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(300, 156);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Informações de Usuário";
            // 
            // btnEntrar
            // 
            btnEntrar.Location = new Point(110, 120);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(75, 30);
            btnEntrar.TabIndex = 4;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = true;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(17, 91);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(260, 23);
            txtSenha.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 73);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 2;
            label2.Text = "Senha";
            // 
            // txtNome
            // 
            txtNome.Location = new Point(17, 47);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(260, 23);
            txtNome.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 29);
            label1.Name = "label1";
            label1.Size = new Size(40, 15);
            label1.TabIndex = 0;
            label1.Text = "Nome";
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(330, 339);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Contabilidade - Login";
            Load += frmLogin_Load;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ComboBox cbbBD;
        private Button btnRenomearBD;
        private Button btnExcluirBD;
        private Button btnCriarBD;
        private Button btnEntrar;
        private TextBox txtSenha;
        private Label label2;
        private TextBox txtNome;
        private Label label1;
    }
}
