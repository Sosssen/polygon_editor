﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace polygon_editor
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // TODO: count relations from all edges not only from selected
            numericUpDown1.Maximum = polygon_editor.chosenPointRel.relations.Count;
        }

        private void APPLY_Click(object sender, EventArgs e)
        {
            polygon_editor.chosenRelation = Convert.ToInt32(numericUpDown1.Value);
            this.Close();
        }

        private void CANCEL_Click(object sender, EventArgs e)
        {
            polygon_editor.chosenRelation = -1;
            this.Close();
        }
    }
}
