﻿namespace Contabilidade.Forms.Cadastros
{
    partial class frmHistoricos
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            groupBox1 = new GroupBox();
            txtFiltrar = new TextBox();
            btnImprimir = new Button();
            btnEditar = new Button();
            btnExcluir = new Button();
            btnCriar = new Button();
            dgvHistoricos = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Histórico = new DataGridViewTextBoxColumn();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistoricos).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtFiltrar);
            groupBox1.Location = new Point(524, 421);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(344, 66);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar por histórico";
            // 
            // txtFiltrar
            // 
            txtFiltrar.Location = new Point(6, 25);
            txtFiltrar.MaxLength = 100;
            txtFiltrar.Name = "txtFiltrar";
            txtFiltrar.Size = new Size(332, 23);
            txtFiltrar.TabIndex = 10;
            txtFiltrar.TextChanged += txtFiltrar_TextChanged;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(268, 457);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(250, 30);
            btnImprimir.TabIndex = 18;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(12, 457);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(250, 30);
            btnEditar.TabIndex = 17;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(268, 421);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(250, 30);
            btnExcluir.TabIndex = 16;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnCriar
            // 
            btnCriar.Location = new Point(12, 421);
            btnCriar.Name = "btnCriar";
            btnCriar.Size = new Size(250, 30);
            btnCriar.TabIndex = 15;
            btnCriar.Text = "Criar";
            btnCriar.UseVisualStyleBackColor = true;
            btnCriar.Click += btnCriar_Click;
            // 
            // dgvHistoricos
            // 
            dgvHistoricos.AllowUserToAddRows = false;
            dgvHistoricos.AllowUserToDeleteRows = false;
            dgvHistoricos.AllowUserToOrderColumns = true;
            dgvHistoricos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistoricos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistoricos.Columns.AddRange(new DataGridViewColumn[] { ID, Histórico });
            dgvHistoricos.Location = new Point(12, 12);
            dgvHistoricos.MultiSelect = false;
            dgvHistoricos.Name = "dgvHistoricos";
            dgvHistoricos.ReadOnly = true;
            dgvHistoricos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistoricos.Size = new Size(856, 403);
            dgvHistoricos.TabIndex = 14;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            ID.DataPropertyName = "id";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ID.DefaultCellStyle = dataGridViewCellStyle1;
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Visible = false;
            // 
            // Histórico
            // 
            Histórico.DataPropertyName = "historico";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Histórico.DefaultCellStyle = dataGridViewCellStyle2;
            Histórico.HeaderText = "Histórico";
            Histórico.Name = "Histórico";
            Histórico.ReadOnly = true;
            // 
            // frmHistoricos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(880, 500);
            Controls.Add(groupBox1);
            Controls.Add(btnImprimir);
            Controls.Add(btnEditar);
            Controls.Add(btnExcluir);
            Controls.Add(btnCriar);
            Controls.Add(dgvHistoricos);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmHistoricos";
            Text = "Históricos";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistoricos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtFiltrar;
        private Button btnImprimir;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnCriar;
        private DataGridView dgvHistoricos;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Histórico;
    }
}