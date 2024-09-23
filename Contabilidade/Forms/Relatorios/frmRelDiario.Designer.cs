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
            btnVisualizar = new Button();
            label2 = new Label();
            dtpData = new DateTimePicker();
            label3 = new Label();
            txtSubtitulo = new TextBox();
            SuspendLayout();
            // 
            // btnVisualizar
            // 
            btnVisualizar.Location = new Point(366, 294);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(148, 41);
            btnVisualizar.TabIndex = 44;
            btnVisualizar.Text = "Visualizar relatório";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += btnVisualizar_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(231, 184);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 38;
            label2.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Location = new Point(231, 202);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(252, 23);
            dtpData.TabIndex = 36;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(231, 231);
            label3.Name = "label3";
            label3.Size = new Size(131, 15);
            label3.TabIndex = 50;
            label3.Text = "Subtítulo personalizado";
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Location = new Point(231, 249);
            txtSubtitulo.MaxLength = 110;
            txtSubtitulo.Multiline = true;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(419, 39);
            txtSubtitulo.TabIndex = 49;
            txtSubtitulo.KeyPress += txtSubtitulo_KeyPress;
            // 
            // frmRelDiario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(label3);
            Controls.Add(txtSubtitulo);
            Controls.Add(btnVisualizar);
            Controls.Add(label2);
            Controls.Add(dtpData);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmRelDiario";
            Text = "Relatório Diário";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVisualizar;
        private Label label2;
        private DateTimePicker dtpData;
        private Label label3;
        private TextBox txtSubtitulo;
    }
}