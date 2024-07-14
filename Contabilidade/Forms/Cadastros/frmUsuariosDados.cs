﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmUsuariosDados : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmUsuariosDados(string titulo, string usuario, string senha)
        {
            InitializeComponent();

            this.Text = titulo;
            this.lblTitulo.Text = titulo;

            txtUsuario.Text = usuario;
            txtSenha.Text = senha;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Se o usuário não for válido
            if (!frmLogin.verificarUsuario(txtUsuario.Text))
            {
                txtUsuario.Text = "";
                txtUsuario.Focus();
            }
            // Se a senha não fo válida
            else if (!frmLogin.verificarSenha(txtSenha.Text))
            {
                txtSenha.Text = "";
                txtSenha.Focus();
            }
            // Se o usuário já existir
            else if (frmUsuarios.verificarExistenciaUsuario(txtUsuario.Text))
            {
                MessageBox.Show("O usuário informado já existe!", "Erro ao informar usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Text = "";
                txtUsuario.Focus();
            }
            else
            {
                // Envia os dados para o formulário pai
                frmUsuarios.usuario = txtUsuario.Text;
                frmUsuarios.senha = txtSenha.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void pnlBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void lblTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}