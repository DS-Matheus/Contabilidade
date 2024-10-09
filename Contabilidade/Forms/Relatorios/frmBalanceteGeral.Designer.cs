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
            btnVisualizar = new Button();
            label3 = new Label();
            label2 = new Label();
            dtpFinal = new DateTimePicker();
            dtpInicial = new DateTimePicker();
            label4 = new Label();
            txtSubtitulo = new TextBox();
            SuspendLayout();
            // 
            // btnVisualizar
            // 
            btnVisualizar.Location = new Point(315, 329);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(250, 38);
            btnVisualizar.TabIndex = 31;
            btnVisualizar.Text = "Visualizar relatório";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += btnVisualizar_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(315, 230);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 26;
            label3.Text = "Data Final";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(315, 186);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 25;
            label2.Text = "Data Inicial";
            // 
            // dtpFinal
            // 
            dtpFinal.Location = new Point(315, 248);
            dtpFinal.Name = "dtpFinal";
            dtpFinal.Size = new Size(250, 23);
            dtpFinal.TabIndex = 24;
            // 
            // dtpInicial
            // 
            dtpInicial.Location = new Point(315, 204);
            dtpInicial.Name = "dtpInicial";
            dtpInicial.Size = new Size(250, 23);
            dtpInicial.TabIndex = 23;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(315, 282);
            label4.Name = "label4";
            label4.Size = new Size(131, 15);
            label4.TabIndex = 52;
            label4.Text = "Subtítulo personalizado";
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Location = new Point(315, 300);
            txtSubtitulo.MaxLength = 110;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(445, 23);
            txtSubtitulo.TabIndex = 51;
            // 
            // frmBalanceteGeral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(label4);
            Controls.Add(txtSubtitulo);
            Controls.Add(btnVisualizar);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dtpFinal);
            Controls.Add(dtpInicial);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmBalanceteGeral";
            Text = "Balancete Geral";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVisualizar;
        private Label label3;
        private Label label2;
        private DateTimePicker dtpFinal;
        private DateTimePicker dtpInicial;
        private Label label4;
        private TextBox txtSubtitulo;
    }
}