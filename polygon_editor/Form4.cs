using System;
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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            if (polygon_editor.chosenPointRel.relations.Count == 0)
            {
                domainUpDown1.Items.Add("");
            }
            else
            {
                polygon_editor.chosenPointRel.relations.Reverse();
                foreach (var number in polygon_editor.chosenPointRel.relations)
                {
                    domainUpDown1.Items.Add(number.ToString());
                }
                polygon_editor.chosenPointRel.relations.Reverse();
            }
            domainUpDown1.SelectedIndex = polygon_editor.chosenPointRel.relations.Count - 1 >= 0 ? polygon_editor.chosenPointRel.relations.Count - 1 : 0;
        }

        private void APPLY_Click(object sender, EventArgs e)
        {
            polygon_editor.chosenRelation = Convert.ToInt32(domainUpDown1.SelectedItem);
            this.Close();
        }

        private void CANCEL_Click(object sender, EventArgs e)
        {
            polygon_editor.chosenRelation = -1;
            this.Close();
        }
    }
}
