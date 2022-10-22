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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            textBox.Text = String.Format("{0:0.00}", polygon_editor.edgeLength);
        }

        private void CANCEL_Click(object sender, EventArgs e)
        {
            polygon_editor.edgeLengthChanged = false;
            this.Close();
        }

        // TODO: why this button is highlighted?
        private void APPLY_Click(object sender, EventArgs e)
        {
            polygon_editor.edgeLengthChanged = true;
            polygon_editor.edgeLength = Convert.ToDouble(textBox.Text);
            this.Close();
        }
    }
}
