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
        public static double edgeLength = 0;
        // public static List<int> test = new List<int>();

        private Bitmap drawArea;
        private Pen pen = new Pen(Color.Black, 1);
        private Pen redPen = new Pen(Color.Red, 6);
        private SolidBrush sbBlack = new SolidBrush(Color.Black);
        private SolidBrush sbRed = new SolidBrush(Color.Red);
        private SolidBrush sbGreen = new SolidBrush(Color.Green);

        private int chosenButton;

        private List<MyPoint> points = new List<MyPoint>();
        private List<List<MyPoint>> polygons = new List<List<MyPoint>>();

        private const int radius = 4;

        private bool colorPoint = false;
        private MyPoint pointToColor = new MyPoint();

        private bool colorEdge = false;
        private MyPoint edgeToColor = new MyPoint();

        private bool colorPolygon = false;
        private List<MyPoint> polygonToColor = null;

        // 0 - nothing, 1 - point, 2 - edge, 3 - polygon
        private int moving = 0;
        private (int, int) pointToMove;
        private (int, int) edgeToMove;
        private MyPoint startingPoint = new MyPoint();
        private MyPoint startingPointA = new MyPoint();
        private MyPoint startingPointB = new MyPoint();
        private List<MyPoint> polygonToMove = null;
        private List<MyPoint> polygonToMoveCopy = null;
        public polygon_editor()
        {
            InitializeComponent();

            //test.Add(5);
            //test.Add(6);
            //test.Add(1);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            // doing this to fix bug with scaling in Windows xddd
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = (int)(screen.Width / 1.5);
            int h = (int)(screen.Height / 1.5);
            this.Size = new Size(w, h);


            CREATE.BackColor = Color.LightBlue;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
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
                                points = new List<MyPoint>();
                            }
                        }
                        else
                        {
                            MessageBox.Show("możesz łączyć tylko z pierwszym", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        points.Add(new MyPoint(e.X, e.Y));
                    }

                    System.Diagnostics.Debug.WriteLine($"points = {points.Count}");
                    System.Diagnostics.Debug.WriteLine($"polygons = {polygons.Count}");
                }
                else if (e.Button == MouseButtons.Right)
                {
                    points = new List<MyPoint>();
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
                            startingPoint = new MyPoint(e.X, e.Y);
                            startingPointA = new MyPoint(polygons[edgeToMove.Item1][edgeToMove.Item2]);
                            startingPointB = new MyPoint(polygons[edgeToMove.Item1][(edgeToMove.Item2 + 1) % polygons[edgeToMove.Item1].Count]);
                            
                            // Debug.WriteLine($"P1: {pointToMove1.Item1}, {pointToMove1.Item2}, P2: {pointToMove2.Item1}, {pointToMove2.Item2}");
                        }
                        else
                        {
                            var result3 = FindPolygon(e.X, e.Y);
                            if (result3.Item1)
                            {
                                moving = 3;
                                colorPolygon = false;
                                polygonToMove = result3.Item2;
                                polygonToMoveCopy = new List<MyPoint>(result3.Item2);
                                startingPoint = new MyPoint(e.X, e.Y);
                            }
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
                    else
                    {
                        var result2 = FindEdgeInPolygons(e.X, e.Y);
                        if (result2.Item1)
                        {
                            result2.Item2.Remove(result2.Item3);
                            result2.Item2.Remove(result2.Item4);

                            if (result2.Item2.Count < 3)
                            {
                                polygons.Remove(result2.Item2);
                            }
                        }
                        else
                        {
                            var result3 = FindPolygon(e.X, e.Y);
                            if(result3.Item1)
                            {
                                polygons.Remove(result3.Item2);
                            }
                        }
                    }
                }
            }
            else if (chosenButton == 3)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        MyPoint newPoint = new MyPoint((result.Item3.x + result.Item4.x) / 2, (result.Item3.y + result.Item4.y) / 2);
                        int i = result.Item2.IndexOf(result.Item3);
                        result.Item2.Insert(i + 1, newPoint);
                    }
                }
            }
            else if (chosenButton == 4)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        MyPoint p1 = result.Item3;
                        MyPoint p2 = result.Item4;
                        // TODO: change result to p1,p2...
                        edgeLength = getDistance(result.Item3.x, result.Item3.y, result.Item4.x, result.Item4.y);
                        double temp = edgeLength;
                        Form2 form = new Form2();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        Debug.WriteLine($"{edgeLength}");
                        //double a = (p1.Y - p2.Y) / (p1.X - p2.X);
                        //double b = p1.Y - a * p1.X;
                        // TODO: maybe better precision?
                        double scale = edgeLength / temp;
                        double lengthX = p2.x - p1.x;
                        double lengthY = p2.y - p1.y;
                        lengthX *= scale;
                        lengthY *= scale;
                        double x = p1.x + lengthX;
                        double y = p1.y + lengthY;
                        result.Item2[result.Item2.IndexOf(p2)] = new MyPoint((int)x, (int)y);

                    }
                }
            }
            else if (chosenButton == 5)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        Form3 form = new Form3();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();
                    }
                }
            }
            else if (chosenButton == 6)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        Form3 form = new Form3();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();
                    }
                }
            }


            DrawCanvas(e.X, e.Y);

        }

        private (bool, MyPoint) FindPointInPoints(int mouseX, int mouseY)
        {
            foreach(var point in points)
            {
                if ((mouseX - point.x) * (mouseX - point.x) + (mouseY - point.y) * (mouseY - point.y) <= (2 * radius) * (2 * radius)) return (true, point);
            }
            return (false, new MyPoint());
        }

        private (bool, List<MyPoint>, MyPoint) FindPointInPolygons(int x, int y)
        {
            foreach(var polygon in polygons)
            {
                foreach(var point in polygon)
                {
                    if ((point.x - x) * (point.x - x) + (point.y - y) * (point.y - y) <= (2 * radius) * (2 * radius)) return (true, polygon, point);
                }
            }
            return (false, null, null);
        }

        void DrawCanvas(int mouseX = 0, int mouseY = 0)
        {
            drawArea.Dispose();
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
                    MyPoint[] arr = polygon.ToArray();
                    Point[] newArr = new Point[arr.Length];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        newArr[i] = arr[i];
                    }

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
                    else if (colorPolygon)
                    {
                        if (polygon == polygonToColor)
                        {
                            g.DrawPolygon(redPen, newArr);
                        }
                        else
                        {
                            g.DrawPolygon(pen, newArr);
                        }
                    }
                    else
                    {
                        g.DrawPolygon(pen, newArr);
                    }

                    foreach (var point in polygon)
                    {
                        if (chosenButton == 2 && colorPoint && point == pointToColor)
                        {
                            g.DrawEllipse(pen, point.x - 2 * radius, point.y - 2 * radius, 4 * radius, 4 * radius);
                            g.FillEllipse(sbRed, point.x - 2 * radius, point.y - 2 * radius, 4 * radius, 4 * radius);

                        }
                        else
                        {
                            g.DrawEllipse(pen, point.x - radius, point.y - radius, 2 * radius, 2 * radius);
                            g.FillEllipse(sbBlack, point.x - radius, point.y - radius, 2 * radius, 2 * radius);
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
                        g.DrawEllipse(pen, points[i].x - 2 * radius, points[i].y - 2 * radius, 4 * radius, 4 * radius);
                        g.FillEllipse(sbGreen, points[i].x - 2 * radius, points[i].y - 2 * radius, 4 * radius, 4 * radius);
                    }
                    else
                    {
                        g.DrawEllipse(pen, points[i].x - radius, points[i].y - radius, 2 * radius, 2 * radius);
                        g.FillEllipse(sbBlack, points[i].x - radius, points[i].y - radius, 2 * radius, 2 * radius);
                    }
                    
                }
                // draw line to mouse while creating new polygon
                if(chosenButton == 1 && points.Count != 0) g.DrawLine(pen, points[points.Count - 1], new MyPoint(mouseX, mouseY));
            }
        }

        // TODO: change buttons to list and change indexing from 0 not from 1
        private void CREATE_Click(object sender, EventArgs e)
        {
            // 
            CREATE.BackColor = Color.LightBlue;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            chosenButton = 1;
        }

        // TODO: clearing points in every function, maybe clear it elsewhere?
        private void MODIFY_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = Color.LightBlue;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            chosenButton = 2;

            points = new List<MyPoint>();
            DrawCanvas();
        }

        private void MIDDLE_INSERT_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = Color.LightBlue;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            chosenButton = 3;

            points = new List<MyPoint>();
            DrawCanvas();
        }

        private void SET_LENGTH_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = Color.LightBlue;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            chosenButton = 4;

            points = new List<MyPoint>();
            DrawCanvas();
        }

        private void ADD_REL_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = Color.LightBlue;
            REMOVE_REL.BackColor = SystemColors.Control;
            chosenButton = 5;

            points = new List<MyPoint>();
            DrawCanvas();
        }

        private void REMOVE_REL_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = Color.LightBlue;
            chosenButton = 6;

            points = new List<MyPoint>();
            DrawCanvas();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (chosenButton == 1)
            {
                var result = FindPointInPoints(e.X, e.Y);
                if (result.Item1)
                {
                    colorPoint = true;
                    pointToColor = result.Item2;
                }
                else
                {
                    colorPoint = false;
                }
            }
            else if (chosenButton == 2)
            {
                if (moving == 0)
                {
                    var result = FindPointInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        colorPoint = true;
                        colorEdge = false;
                        colorPolygon = false;
                        pointToColor = result.Item3;
                    }
                    else
                    {
                        var result2 = FindEdgeInPolygons(e.X, e.Y);
                        if (result2.Item1)
                        {
                            colorPoint = false;
                            colorEdge = true;
                            colorPolygon = false;
                            edgeToColor = result2.Item3;
                        }
                        else
                        {
                            var result3 = FindPolygon(e.X, e.Y);
                            if (result3.Item1)
                            {
                                colorPoint = false;
                                colorEdge = false;
                                colorPolygon = true;
                                polygonToColor = result3.Item2;
                            }
                            else
                            {
                                colorPoint = false;
                                colorEdge = false;
                                colorPolygon = false;
                            }
                        }
                    }
                }
                else if (moving == 1)
                {
                    polygons[pointToMove.Item1][pointToMove.Item2] = new MyPoint(e.X, e.Y);
                }
                else if (moving == 2)
                {
                    int diffX = startingPoint.x - e.X;
                    int diffY = startingPoint.y - e.Y;
                    MyPoint a = new MyPoint(startingPointA);
                    MyPoint b = new MyPoint(startingPointB);
                    a.x -= diffX;
                    a.y -= diffY;
                    b.x -= diffX;
                    b.y -= diffY;
                    polygons[edgeToMove.Item1][edgeToMove.Item2] = a;
                    polygons[edgeToMove.Item1][(edgeToMove.Item2 + 1) % polygons[edgeToMove.Item1].Count] = b;
                }
                else if (moving == 3)
                {
                    // TODO: maybe foreach and changing fields of var point?
                    for (int i = 0; i < polygonToMove.Count; i++)
                    {
                        int diffX = startingPoint.x - e.X;
                        int diffY = startingPoint.y - e.Y;
                        MyPoint temp = new MyPoint(polygonToMoveCopy[i]);
                        temp.x -= diffX;
                        temp.y -= diffY;
                        polygonToMove[i] = temp;
                    }
                }
            }
            else if (chosenButton == 3)
            {
                if (moving == 0)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        colorEdge = true;
                        edgeToColor = result.Item3;
                    }
                    else
                    {
                        colorEdge = false;
                    }
                }
            }
            else if (chosenButton == 4)
            {
                if (moving == 0)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        colorEdge = true;
                        edgeToColor = result.Item3;
                    }
                    else
                    {
                        colorEdge = false;
                    }
                }
            }
            else if (chosenButton == 5)
            {
                if (moving == 0)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        colorEdge = true;
                        edgeToColor = result.Item3;
                    }
                    else
                    {
                        colorEdge = false;
                    }
                }
            }
            else if (chosenButton == 6)
            {
                if (moving == 0)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        colorEdge = true;
                        edgeToColor = result.Item3;
                    }
                    else
                    {
                        colorEdge = false;
                    }
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

        private (bool, List<MyPoint>, MyPoint, MyPoint) FindEdgeInPolygons(int x, int y)
        {
            foreach (var polygon in polygons)
            {
                // TODO: find better way (more accurate) to calculate this
                for (int i = 0; i < polygon.Count; i++)
                {
                    MyPoint A = polygon[i];
                    MyPoint B = polygon[(i + 1) % polygon.Count];
                    int minX = A.x <= B.x ? A.x : B.x;
                    int minY = A.y <= B.y ? A.y : B.y;
                    int maxX = A.x > B.x ? A.x : B.x;
                    int maxY = A.y > B.y ? A.y : B.y;
                    if (x < minX || x > maxX || y < minY || y > maxY) continue;
                    double a = getDistance(x, y, B.x, B.y);
                    double b = getDistance(x, y, A.x, A.y);
                    double c = getDistance(A.x, A.y, B.x, B.y);
                    double p = (a + b + c) / 2;
                    double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                    if (2 * S / c < 6) return (true, polygon, A, B);
                }
            }
            return (false, null, new MyPoint(), new MyPoint());
        }

        private (bool, List<MyPoint>) FindPolygon(int x ,int y)
        {
            int counter;
            Debug.WriteLine("run findpolygon");
            foreach (var polygon in polygons)
            {
                counter = 0;
                for (int i = 0; i < polygon.Count; i++)
                {
                    MyPoint A = polygon[i];
                    MyPoint B = polygon[(i + 1) % polygon.Count];
                    if (areIntersecting(0, 0, x, y, A.x, A.y, B.x, B.y) == 1) counter++;
                }
                Debug.WriteLine($"Polygon number {polygons.IndexOf(polygon)}, counter: {counter}");
                if (counter % 2 == 1) return (true, polygon);
            }
            return (false, null);
        }

        int areIntersecting(
    float v1x1, float v1y1, float v1x2, float v1y2,
    float v2x1, float v2y1, float v2x2, float v2y2
)
        {
            float d1, d2;
            float a1, a2, b1, b2, c1, c2;

            // Convert vector 1 to a line (line 1) of infinite length.
            // We want the line in linear equation standard form: A*x + B*y + C = 0
            // See: http://en.wikipedia.org/wiki/Linear_equation
            a1 = v1y2 - v1y1;
            b1 = v1x1 - v1x2;
            c1 = (v1x2 * v1y1) - (v1x1 * v1y2);

            // Every point (x,y), that solves the equation above, is on the line,
            // every point that does not solve it, is not. The equation will have a
            // positive result if it is on one side of the line and a negative one 
            // if is on the other side of it. We insert (x1,y1) and (x2,y2) of vector
            // 2 into the equation above.
            d1 = (a1 * v2x1) + (b1 * v2y1) + c1;
            d2 = (a1 * v2x2) + (b1 * v2y2) + c1;

            // If d1 and d2 both have the same sign, they are both on the same side
            // of our line 1 and in that case no intersection is possible. Careful, 
            // 0 is a special case, that's why we don't test ">=" and "<=", 
            // but "<" and ">".
            if (d1 > 0 && d2 > 0) return 0;
            if (d1 < 0 && d2 < 0) return 0;

            // The fact that vector 2 intersected the infinite line 1 above doesn't 
            // mean it also intersects the vector 1. Vector 1 is only a subset of that
            // infinite line 1, so it may have intersected that line before the vector
            // started or after it ended. To know for sure, we have to repeat the
            // the same test the other way round. We start by calculating the 
            // infinite line 2 in linear equation standard form.
            a2 = v2y2 - v2y1;
            b2 = v2x1 - v2x2;
            c2 = (v2x2 * v2y1) - (v2x1 * v2y2);

            // Calculate d1 and d2 again, this time using points of vector 1.
            d1 = (a2 * v1x1) + (b2 * v1y1) + c2;
            d2 = (a2 * v1x2) + (b2 * v1y2) + c2;

            // Again, if both have the same sign (and neither one is 0),
            // no intersection is possible.
            if (d1 > 0 && d2 > 0) return 0;
            if (d1 < 0 && d2 < 0) return 0;

            // If we get here, only two possibilities are left. Either the two
            // vectors intersect in exactly one point or they are collinear, which
            // means they intersect in any number of points from zero to infinite.
            if ((a1 * b2) - (a2 * b1) == 0.0f) return 2;

            // If they are not collinear, they must intersect in exactly one point.
            return 1;
        }
        public class MyPoint
        {
            // TODO: change to setters?
            public int x;
            public int y;

            public MyPoint()
            {
                this.x = 0;
                this.y = 0;
            }

            public MyPoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public MyPoint(MyPoint p)
            {
                this.x = p.x;
                this.y = p.y;
            }

            public static implicit operator Point(MyPoint p) => new Point(p.x, p.y);
        }
    }
}
