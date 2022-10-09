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
    public partial class polygon_editor : Form
    {
        private Bitmap drawArea;
        private Pen pen;
        public polygon_editor()
        {
            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = drawArea;
            
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
            }

            pen = new Pen(Brushes.Black, 1);

            for(int i = 100; i < 250; i++)
            {
                for(int j = 100; j < 250; j++)
                {
                    drawArea.SetPixel(i, j, Color.Red);
                }
            }

            using (Graphics g = Graphics.FromImage(drawArea))
            {
                Point p1 = new Point(200, 200);
                Point p2 = new Point(300, 300);
                Point p3 = new Point(400, 300);

                Point[] points = { p1, p2, p3 };

                g.DrawPolygon(pen, points);
            }
        }

    }
}
