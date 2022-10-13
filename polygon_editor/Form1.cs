using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace polygon_editor
{
    public partial class polygon_editor : Form
    {
        private Bitmap drawArea;
        private Pen pen;

        private List<Point> points = new List<Point>();
        private List<List<Point>> polygons = new List<List<Point>>();
        public polygon_editor()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            Debug.WriteLine("chuj");

            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = drawArea;
            
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
            }

            pen = new Pen(Brushes.Black, 3);

            for(int i = 100; i < 250; i++)
            {
                for(int j = 100; j < 250; j++)
                {
                    drawArea.SetPixel(i, j, Color.Blue);
                }
            }

            Point p1 = new Point(100, 100);
            Point p2 = new Point(150, 100);
            Point p3 = new Point(200, 300);
            Point p4 = new Point(400, 400);
            Point[] temp = { p1, p2, p3, p4 };
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.DrawPolygon(pen, temp);
            }
        }

        private void Canvas_Click(object sender, EventArgs e)
        {
            // Point p = new Point()
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            var result = FindPoint(points, p);
            if (result.Item1)
            {
                if(result.Item2 == points[0])
                {
                    polygons.Add(points);
                    points = new List<Point>();
                }
                else
                {
                    MessageBox.Show("text", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                points.Add(p);
            }

            System.Diagnostics.Debug.WriteLine($"points = {points.Count}");
            System.Diagnostics.Debug.WriteLine($"polygons = {polygons.Count}");

            DrawCanvas();

        }

        private (bool, Point) FindPoint(List<Point> points, Point p)
        {
            foreach(var point in points)
            {
                if (Math.Sqrt((p.X - point.X) * (p.X - point.X) + (p.Y - point.Y) * (p.Y - point.Y)) <= 10) return (true, point);
            }
            return (false, new Point());
        }

        void DrawCanvas()
        {

            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = drawArea;
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
            }
            Debug.WriteLine("test");
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                foreach (var polygon in polygons)
                {
                    Point[] arr = polygon.ToArray();

                    g.DrawPolygon(pen, arr);
                }
            }
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                for(int i = 0; i < points.Count - 1; i++)
                {
                    g.DrawLine(pen, points[i], points[i + 1]);
                }
            }
        }
    }
}
