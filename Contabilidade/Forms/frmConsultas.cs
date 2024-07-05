﻿using Contabilidade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contabilidade.Forms
{
    public partial class frmConsultas : Form
    {
        public frmConsultas()
        {
            InitializeComponent();
        }

        private void frmConsultas_Load(object sender, EventArgs e)
        {
            CarregarTema();
        }

        private void CarregarTema()
        {
            foreach (Control botoes in this.Controls)
            {
                if (botoes.GetType() == typeof(Button))
                {
                    Button btn = (Button)botoes;
                    btn.BackColor = TemaCores.corPrimaria;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = TemaCores.corSecundaria;
                }
            }
            label1.ForeColor = TemaCores.corSecundaria;
            label2.ForeColor = TemaCores.corPrimaria;
            label3.ForeColor = TemaCores.corPrimaria;
        }
    }
}
