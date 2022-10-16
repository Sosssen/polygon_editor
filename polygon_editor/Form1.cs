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
        private Pen redPen = new Pen(Color.Red, 6);
        private SolidBrush sbBlack = new SolidBrush(Color.Black);
        private SolidBrush sbRed = new SolidBrush(Color.Red);
        private SolidBrush sbGreen = new SolidBrush(Color.Green);

        private int chosenButton;

        private List<Point> points = new List<Point>();
        private List<List<Point>> polygons = new List<List<Point>>();

        private const int radius = 4;

        private bool colorPoint = false;
        private Point pointToColor = new Point();

        private bool colorEdge = false;
        private Point edgeToColor = new Point();

        // 0 - nothing, 1 - point, 2 - edge, 3 - polygon
        private int moving = 0;
        private (int, int) pointToMove;
        private (int, int) edgeToMove;
        private Point startingPoint = new Point();
        private Point startingPointA = new Point();
        private Point startingPointB = new Point();
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
                            MessageBox.Show("możesz łączyć tylko z pierwszym", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var result = FindPointInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        moving = 1;
                        int indexOfPolygon = polygons.IndexOf(result.Item2);
                        pointToMove = (indexOfPolygon, polygons[indexOfPolygon].IndexOf(result.Item3));
                        Debug.WriteLine($"{pointToMove.Item1}, {pointToMove.Item2}");
                    }
                    else
                    {
                        var result2 = FindEdgeInPolygons(e.X, e.Y);
                        if(result2.Item1)
                        {
                            moving = 2;
                            int indexOfPolygon = polygons.IndexOf(result2.Item2);
                            edgeToMove = (indexOfPolygon, polygons[indexOfPolygon].IndexOf(result2.Item3));
                            startingPoint = new Point(e.X, e.Y);
                            startingPointA = polygons[edgeToMove.Item1][edgeToMove.Item2];
                            startingPointB = polygons[edgeToMove.Item1][(edgeToMove.Item2 + 1) % polygons[edgeToMove.Item1].Count];
                            
                            // Debug.WriteLine($"P1: {pointToMove1.Item1}, {pointToMove1.Item2}, P2: {pointToMove2.Item1}, {pointToMove2.Item2}");
                        }
                    }
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

                    if (colorEdge)
                    {
                        for (int i = 0; i < polygon.Count; i++)
                        {
                            if (polygon[i] == edgeToColor)
                            {
                                Debug.WriteLine("rysuje inna krawedz");
                                g.DrawLine(redPen, polygon[i], polygon[(i + 1) % polygon.Count]);
                            }
                            else
                            {
                                g.DrawLine(pen, polygon[i], polygon[(i + 1) % polygon.Count]);
                            }
                        }
                    }
                    else
                    {
                        g.DrawPolygon(pen, arr);
                    }

                    foreach (var point in polygon)
                    {
                        if (chosenButton == 2 && colorPoint && point == pointToColor)
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
                    if (chosenButton == 1 && colorPoint && pointToColor == points[i] && i == 0 && points.Count > 2)
                    {
                        g.DrawEllipse(pen, points[i].X - 2 * radius, points[i].Y - 2 * radius, 4 * radius, 4 * radius);
                        g.FillEllipse(sbGreen, points[i].X - 2 * radius, points[i].Y - 2 * radius, 4 * radius, 4 * radius);
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
            // 
            CREATE.BackColor = Color.LightBlue;
            MODIFY.BackColor = SystemColors.Control;
            chosenButton = 1;
        }

        private void MODIFY_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = Color.LightBlue;
            chosenButton = 2;

            points = new List<Point>();
            DrawCanvas();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(chosenButton == 1)
            {
                var result = FindPointInPoints(e.X, e.Y);
                if(result.Item1)
                {
                    colorPoint = true;
                    pointToColor = result.Item2;
                }
                else
                {
                    colorPoint = false;
                }
            }
            else if(chosenButton == 2)
            {
                if (moving == 0)
                {
                    var result = FindPointInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        colorEdge = false;
                        colorPoint = true;
                        pointToColor = result.Item3;
                    }
                    else
                    {
                        var result2 = FindEdgeInPolygons(e.X, e.Y);
                        if (result2.Item1)
                        {
                            colorPoint = false;
                            colorEdge = true;
                            edgeToColor = result2.Item3;
                        }
                        else
                        {
                            colorPoint = false;
                            colorEdge = false;
                        }
                    }
                }
                else if (moving == 1)
                {
                    polygons[pointToMove.Item1][pointToMove.Item2] = new Point(e.X, e.Y);
                }
                else if (moving == 2)
                {
                    int diffX = startingPoint.X - e.X;
                    int diffY = startingPoint.Y - e.Y;
                    Point a = startingPointA;
                    Point b = startingPointB;
                    a.X -= diffX;
                    a.Y -= diffY;
                    b.X -= diffX;
                    b.Y -= diffY;
                    polygons[edgeToMove.Item1][edgeToMove.Item2] = a;
                    polygons[edgeToMove.Item1][(edgeToMove.Item2 + 1) % polygons[edgeToMove.Item1].Count] = b;
                }
            }
            DrawCanvas(e.X, e.Y);
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            moving = 0;
        }

        private double getDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        private (bool, List<Point>, Point, Point) FindEdgeInPolygons(int x, int y)
        {
            foreach (var polygon in polygons)
            {
                // TODO: find better way (more accurate) to calculate this
                for (int i = 0; i < polygon.Count; i++)
                {
                    Point A = polygon[i];
                    Point B = polygon[(i + 1) % polygon.Count];
                    int minX = A.X <= B.X ? A.X : B.X;
                    int minY = A.Y <= B.Y ? A.Y : B.Y;
                    int maxX = A.X > B.X ? A.X : B.X;
                    int maxY = A.Y > B.Y ? A.Y : B.Y;
                    if (x < minX || x > maxX || y < minY || y > maxY) continue;
                    double a = getDistance(x, y, B.X, B.Y);
                    double b = getDistance(x, y, A.X, A.Y);
                    double c = getDistance(A.X, A.Y, B.X, B.Y);
                    double p = (a + b + c) / 2;
                    double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                    if (2 * S / c < 6) return (true, polygon, A, B);
                }
            }
            return (false, null, new Point(), new Point());
        }
    }
}
