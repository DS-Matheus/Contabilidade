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
            button1 = new Button();
            label3 = new Label();
            label2 = new Label();
            dtpFinal = new DateTimePicker();
            dtpInicial = new DateTimePicker();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(315, 277);
            button1.Name = "button1";
            button1.Size = new Size(250, 38);
            button1.TabIndex = 31;
            button1.Text = "Visualizar relatório";
            button1.UseVisualStyleBackColor = true;
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
            // frmBalanceteGeral
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(button1);
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

        private Button button1;
        private Label label3;
        private Label label2;
        private DateTimePicker dtpFinal;
        private DateTimePicker dtpInicial;
    }
}