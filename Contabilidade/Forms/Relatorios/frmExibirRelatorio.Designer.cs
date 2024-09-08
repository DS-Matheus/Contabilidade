namespace Contabilidade.Forms.Relatorios
{
    partial class frmExibirRelatorio
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
            dgvRelatorio = new DataGridView();
            btnImprimir = new Button();
            label1 = new Label();
            txtTitulo = new TextBox();
            txtSubtitulo = new TextBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRelatorio).BeginInit();
            SuspendLayout();
            // 
            // dgvRelatorio
            // 
            dgvRelatorio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRelatorio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvRelatorio.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRelatorio.Location = new Point(12, 12);
            dgvRelatorio.Name = "dgvRelatorio";
            dgvRelatorio.Size = new Size(776, 390);
            dgvRelatorio.TabIndex = 0;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(661, 408);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(127, 30);
            btnImprimir.TabIndex = 1;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 416);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 2;
            label1.Text = "Título";
            // 
            // txtTitulo
            // 
            txtTitulo.Location = new Point(55, 413);
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(260, 23);
            txtTitulo.TabIndex = 3;
            // 
            // txtSubtitulo
            // 
            txtSubtitulo.Location = new Point(395, 413);
            txtSubtitulo.Name = "txtSubtitulo";
            txtSubtitulo.Size = new Size(260, 23);
            txtSubtitulo.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(334, 416);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 4;
            label2.Text = "Subtítulo";
            // 
            // frmExibirRelatorio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtSubtitulo);
            Controls.Add(label2);
            Controls.Add(txtTitulo);
            Controls.Add(label1);
            Controls.Add(btnImprimir);
            Controls.Add(dgvRelatorio);
            Name = "frmExibirRelatorio";
            Text = "frmExibirRelatorio";
            ((System.ComponentModel.ISupportInitialize)dgvRelatorio).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvRelatorio;
        private Button btnImprimir;
        private Label label1;
        private TextBox txtTitulo;
        private TextBox txtSubtitulo;
        private Label label2;
    }
}