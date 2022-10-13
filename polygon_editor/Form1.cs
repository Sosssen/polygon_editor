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

        private int chosenButton;

        private List<Point> points = new List<Point>();
        private List<List<Point>> polygons = new List<List<Point>>();

        private const int radius = 8;
        public polygon_editor()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;


            CREATE.BackColor = Color.LightBlue;
            MODIFY.BackColor = SystemColors.Control;
            chosenButton = 1;

            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = drawArea;
            
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (chosenButton == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point p = new Point(e.X, e.Y);
                    var result = FindPointInPoints(points, p);
                    if (result.Item1)
                    {
                        if (result.Item2 == points[0])
                        {
                            if (points.Count <= 2)
                            {
                                MessageBox.Show("co najmniej 3 wierzcholki", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                polygons.Add(points);
                                points = new List<Point>();
                            }
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
            }
            // TODO: what to do if polygon was not finished
            else if (chosenButton == 2)
            {
                if (e.Button == MouseButtons.Left)
                {

                }
                else if (e.Button == MouseButtons.Right)
                {
                    // TODO: name result items for readability?
                    var result = FindPointInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        result.Item2.Remove(result.Item3);

                        if (result.Item2.Count < 3)
                        {
                            polygons.Remove(result.Item2);
                        }
                    }

                    DrawCanvas();
                }
            }

        }

        private (bool, Point) FindPointInPoints(List<Point> points, Point p)
        {
            foreach(var point in points)
            {
                if ((p.X - point.X) * (p.X - point.X) + (p.Y - point.Y) * (p.Y - point.Y) <= radius * radius) return (true, point);
            }
            return (false, new Point());
        }

        private (bool, List<Point>, Point) FindPointInPolygons(int x, int y)
        {
            foreach(var polygon in polygons)
            {
                foreach(var point in polygon)
                {
                    if ((point.X - x) * (point.X - x) + (point.Y - y) * (point.Y - y) <= radius * radius) return (true, polygon, point);
                }
            }
            return (false, null, new Point());
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

        private void CREATE_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = Color.LightBlue;
            MODIFY.BackColor = SystemColors.Control;
            chosenButton = 1;
        }

        private void MODIFY_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = Color.LightBlue;
            chosenButton = 2;
        }
    }
}
