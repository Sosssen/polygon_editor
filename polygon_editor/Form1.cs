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
        private Pen pen = new Pen(Color.Black, 3);
        private SolidBrush sb = new SolidBrush(Color.Black);

        private List<Point> points = new List<Point>();
        private List<List<Point>> polygons = new List<List<Point>>();

        private int radius = 8;
        public polygon_editor()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            InitializeComponent();
            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = drawArea;
            
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
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
                if ((p.X - point.X) * (p.X - point.X) + (p.Y - point.Y) * (p.Y - point.Y) <= radius * radius) return (true, point);
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

                    foreach(var point in polygon)
                    {
                        g.DrawEllipse(pen, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                        g.FillEllipse(sb, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                    }

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
                for(int i = 0; i < points.Count; i++)
                {
                    g.DrawEllipse(pen, points[i].X - radius, points[i].Y - radius, 2 * radius, 2 * radius);
                    g.FillEllipse(sb, points[i].X - radius, points[i].Y - radius, 2 * radius, 2 * radius);
                }
            }
        }
    }
}
