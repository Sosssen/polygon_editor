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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // TODO: count relations from all edges not only from selected
            int minNotContained = -1;
            for (int i = 0; i < polygon_editor.relationsDict.Count + 1; i++)
            {
                minNotContained = i;
                if (!polygon_editor.relationsDict.ContainsKey(minNotContained)) break;
            }
            List<int> temp = polygon_editor.relationsDict.Keys.ToList();
            temp.Add(minNotContained);
            temp.Sort();
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                domainUpDown1.Items.Add(temp[i]);
            }
            domainUpDown1.SelectedIndex = temp.Count - 1;
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
