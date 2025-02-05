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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRelDiario));
            btnVisualizar = new Button();
            label2 = new Label();
            dtpData = new DateTimePicker();
            label3 = new Label();
            txtSubtitulo = new TextBox();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnVisualizar
            // 
            btnVisualizar.Font = new Font("Lucida Sans", 10.5F);
            btnVisualizar.Location = new Point(115, 199);
            btnVisualizar.Name = "btnVisualizar";
            btnVisualizar.Size = new Size(419, 41);
            btnVisualizar.TabIndex = 44;
            btnVisualizar.Text = "Visualizar relatório";
            btnVisualizar.UseVisualStyleBackColor = true;
            btnVisualizar.Click += btnVisualizar_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Lucida Sans", 10.5F);
            label2.Location = new Point(115, 79);
            label2.Name = "label2";
            label2.Size = new Size(38, 16);
            label2.TabIndex = 38;
            label2.Text = "Data";
            // 
            // dtpData
            // 
            dtpData.Font = new Font("Lucida Sans", 10.5F);
            dtpData.Location = new Point(115, 97);
            dtpData.Name = "dtpData";
            dtpData.Size = new Size(419, 24);
            dtpData.TabIndex = 36;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Lucida Sans", 10.5F);
            label3.Location = new Point(115, 131);
            label3.Name = "label3";
            label3.Size = new Size(163, 16);
            label3.TabIndex = 50;
            label3.Text = "Subtítulo personalizado";
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Font = new Font("Lucida Sans", 10.5F);
            txtSubtitulo.Location = new Point(115, 149);
            txtSubtitulo.MaxLength = 110;
            txtSubtitulo.Multiline = true;
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(419, 39);
            txtSubtitulo.TabIndex = 49;
            txtSubtitulo.KeyPress += txtSubtitulo_KeyPress;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnVisualizar);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(dtpData);
            panel1.Controls.Add(txtSubtitulo);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(115, 90);
            panel1.Name = "panel1";
            panel1.Size = new Size(650, 320);
            panel1.TabIndex = 51;
            // 
            // frmRelDiario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmRelDiario";
            Text = "Relatório Diário";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnVisualizar;
        private Label label2;
        private DateTimePicker dtpData;
        private Label label3;
        private TextBox txtSubtitulo;
        private Panel panel1;
    }
}