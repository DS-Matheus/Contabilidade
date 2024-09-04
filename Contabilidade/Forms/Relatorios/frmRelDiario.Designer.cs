namespace Contabilidade.Forms.Relatorios
{
    partial class frmRelDiario
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
            label2 = new Label();
            dtpData = new DateTimePicker();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(326, 262);
            button1.Name = "button1";
            button1.Size = new Size(229, 23);
            button1.TabIndex = 44;
            button1.Text = "Visualizar relatório";
            button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(326, 215);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 38;
            label2.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Location = new Point(326, 233);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(229, 23);
            dtpData.TabIndex = 36;
            // 
            // frmRelDiario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(dtpData);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmRelDiario";
            Text = "Relatório Diário";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label2;
        private DateTimePicker dtpData;
    }
}