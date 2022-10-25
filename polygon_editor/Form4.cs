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

            if (polygon_editor.selectedPointRel.relations.Count == 0)
            {
                domainUpDown1.Items.Add("");
            }
            else
            {
                polygon_editor.selectedPointRel.relations.Reverse();
                foreach (var number in polygon_editor.selectedPointRel.relations)
                {
                    domainUpDown1.Items.Add(number.ToString());
                }
                polygon_editor.selectedPointRel.relations.Reverse();
            }
            domainUpDown1.SelectedIndex = polygon_editor.selectedPointRel.relations.Count - 1 >= 0 ? polygon_editor.selectedPointRel.relations.Count - 1 : 0;
        }

        private void APPLY_Click(object sender, EventArgs e)
        {
            polygon_editor.selectedRelation = Convert.ToInt32(domainUpDown1.SelectedItem);
            this.Close();
        }

        private void CANCEL_Click(object sender, EventArgs e)
        {
            polygon_editor.selectedRelation = -1;
            this.Close();
        }
    }
}
