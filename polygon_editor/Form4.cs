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

            domainUpDown1.Items.Add("5");
            domainUpDown1.SelectedItem = "";
        }

        private void APPLY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
