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
        private Pen pen = new Pen(Color.Black, 1);
        private SolidBrush sbBlack = new SolidBrush(Color.Black);
        private SolidBrush sbRed = new SolidBrush(Color.Red);

        private int chosenButton;

        private List<Point> points = new List<Point>();
        private List<List<Point>> polygons = new List<List<Point>>();

        private const int radius = 4;

        private bool colorPoint = false;
        private Point toColor = new Point();
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
                    var result = FindPointInPoints(e.X, e.Y);
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
                        points.Add(new Point(e.X, e.Y));
                    }

                    System.Diagnostics.Debug.WriteLine($"points = {points.Count}");
                    System.Diagnostics.Debug.WriteLine($"polygons = {polygons.Count}");
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
                }
            }

            DrawCanvas(e.X, e.Y);

        }

        private (bool, Point) FindPointInPoints(int mouseX, int mouseY)
        {
            foreach(var point in points)
            {
                if ((mouseX - point.X) * (mouseX - point.X) + (mouseY - point.Y) * (mouseY - point.Y) <= (2 * radius) * (2 * radius)) return (true, point);
            }
            return (false, new Point());
        }

        private (bool, List<Point>, Point) FindPointInPolygons(int x, int y)
        {
            foreach(var polygon in polygons)
            {
                foreach(var point in polygon)
                {
                    if ((point.X - x) * (point.X - x) + (point.Y - y) * (point.Y - y) <= (2 * radius) * (2 * radius)) return (true, polygon, point);
                }
            }
            return (false, null, new Point());
        }

        void DrawCanvas(int mouseX = 0, int mouseY = 0)
        {

            drawArea = new Bitmap(Canvas.Width, Canvas.Height);
            Canvas.Image = drawArea;
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
            }
            // Debug.WriteLine("test");

            using (Graphics g = Graphics.FromImage(drawArea))
            {
                // draw all poygons
                foreach (var polygon in polygons)
                {
                    Point[] arr = polygon.ToArray();

                    foreach (var point in polygon)
                    {
                        
                        if (colorPoint && point == toColor)
                        {
                            g.DrawEllipse(pen, point.X - 2 * radius, point.Y - 2 * radius, 4 * radius, 4 * radius);
                            g.FillEllipse(sbRed, point.X - 2 * radius, point.Y - 2 * radius, 4 * radius, 4 * radius);

                        }
                        else
                        {
                            g.DrawEllipse(pen, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                            g.FillEllipse(sbBlack, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                        }
                    }

                    g.DrawPolygon(pen, arr);
                }
            }
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                // connect all points that are not yet in any polygon
                for(int i = 0; i < points.Count - 1; i++)
                {
                    g.DrawLine(pen, points[i], points[i + 1]);
                }
                for(int i = 0; i < points.Count; i++)
                {
                    if (colorPoint && toColor == points[i] && i != points.Count - 1)
                    {
                        g.DrawEllipse(pen, points[i].X - 2 * radius, points[i].Y - 2 * radius, 4 * radius, 4 * radius);
                        g.FillEllipse(sbRed, points[i].X - 2 * radius, points[i].Y - 2 * radius, 4 * radius, 4 * radius);
                    }
                    else
                    {
                        g.DrawEllipse(pen, points[i].X - radius, points[i].Y - radius, 2 * radius, 2 * radius);
                        g.FillEllipse(sbBlack, points[i].X - radius, points[i].Y - radius, 2 * radius, 2 * radius);
                    }
                    
                }
                // draw line to mouse while creating new polygon
                if(chosenButton == 1 && points.Count != 0) g.DrawLine(pen, points[points.Count - 1], new Point(mouseX, mouseY));
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

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(chosenButton == 1)
            {
                var result = FindPointInPoints(e.X, e.Y);
                if(result.Item1)
                {
                    colorPoint = true;
                    toColor = result.Item2;
                }
                else
                {
                    colorPoint = false;
                }
            }
            else if(chosenButton == 2)
            {
                var result = FindPointInPolygons(e.X, e.Y);
                if(result.Item1)
                {
                    colorPoint = true;
                    toColor = result.Item3;
                }
                else
                {
                    colorPoint = false;
                }
            }
            DrawCanvas(e.X, e.Y);
        }
    }
}
