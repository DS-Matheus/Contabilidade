﻿namespace Contabilidade.Forms
{
    partial class frmRelatorios
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
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(344, 129);
            label1.Name = "label1";
            label1.Size = new Size(113, 24);
            label1.TabIndex = 1;
            label1.Text = "Relatórios";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(316, 189);
            label2.Name = "label2";
            label2.Size = new Size(169, 24);
            label2.TabIndex = 1;
            label2.Text = "Relatório diário";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(317, 217);
            label3.Name = "label3";
            label3.Size = new Size(167, 24);
            label3.TabIndex = 1;
            label3.Text = "Razão analitico";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(346, 245);
            label4.Name = "label4";
            label4.Size = new Size(109, 24);
            label4.TabIndex = 1;
            label4.Text = "Balancete";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(295, 273);
            label5.Name = "label5";
            label5.Size = new Size(210, 24);
            label5.TabIndex = 1;
            label5.Text = "Balancete de Conta";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Lucida Sans", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(152, 297);
            label6.Name = "label6";
            label6.Size = new Size(496, 24);
            label6.TabIndex = 1;
            label6.Text = "Consultar saldo (se não for ficar em Consultas)";
            // 
            // frmRelatorios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "frmRelatorios";
            Text = "Relatorios";
            Load += frmRelatorios_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}