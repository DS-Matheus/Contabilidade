namespace Contabilidade.Forms
{
    partial class frmConsultas
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(345, 175);
            label1.Name = "label1";
            label1.Size = new Size(111, 24);
            label1.TabIndex = 1;
            label1.Text = "Consultas";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(368, 228);
            label2.Name = "label2";
            label2.Size = new Size(65, 24);
            label2.TabIndex = 2;
            label2.Text = "Saldo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(186, 252);
            label3.Name = "label3";
            label3.Size = new Size(428, 24);
            label3.TabIndex = 3;
            label3.Text = "Demais consultas que forem necessárias";
            // 
            // frmConsultas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "frmConsultas";
            Text = "Consultas";
            Load += frmConsultas_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
    }
}