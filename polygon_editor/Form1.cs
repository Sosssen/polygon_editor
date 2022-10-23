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
using ASD.Graphs;

// TODO: predefined canva with 2 polygons
// TODO: maybe button to create new, plain canva?

namespace polygon_editor
{
    public partial class polygon_editor : Form
    {
        Random rnd = new Random();
        // TODO: chosen -> selected
        public static double edgeLength = 0;
        public static bool edgeLengthChanged = false;
        public static MyPoint chosenPointRel = null;
        public static int chosenRelation = -1;
        public static Dictionary<int, List<MyPoint>> relationsDict = new Dictionary<int, List<MyPoint>>();
        Graph relationsGraph = null;
        List<MyPoint> allPoints = null;

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
            // TODO: change this!
            colorPoint = false;
            colorEdge = false;
            colorPolygon = false;
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
                                createNewGraph();
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
                            
                        }
                        else
                        {
                            var result3 = FindPolygon(e.X, e.Y);
                            if (result3.Item1)
                            {
                                moving = 3;
                                colorPolygon = false;
                                polygonToMove = result3.Item2;
                                polygonToMoveCopy = new List<MyPoint>();
                                for (int i = 0; i  < result3.Item2.Count; i++)
                                {
                                    polygonToMoveCopy.Add(new MyPoint(result3.Item2[i]));
                                }
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
                        int idx = result.Item2.IndexOf(result.Item3);
                        result.Item2[(idx - 1) % result.Item2.Count].length = -1.0;
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
                            int idx = result2.Item2.IndexOf(result2.Item3);
                            result2.Item2[(idx - 1) % result2.Item2.Count].length = -1.0;
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
                        result.Item3.length = -1.0;
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
                    // TODO: trycatch this
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        MyPoint p1 = result.Item3;
                        MyPoint p2 = result.Item4;
                        // TODO: change result to p1,p2...
                        edgeLength = getDistance(p1.x, p1.y, p2.x, p2.y);
                        Form2 form = new Form2();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        if (edgeLengthChanged)
                        {
                            if (checkIfCanAddNewLength(result.Item2))
                            {
                                result.Item3.length = edgeLength;
                                correctPointByLength(result.Item2);
                            }
                            else
                            {
                                MessageBox.Show("za dużo ograniczonych krawędzi", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        // TODO: maybe better precision?

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
                        chosenPointRel = result.Item3;

                        Form3 form = new Form3();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();

                        // TODO: can't add more than 2 edges to 1 relation
                        if (chosenRelation != -1)
                        {
                            if (checkIfCanAddNewRelation(result.Item2) || result.Item3.relations.Count > 0)
                            {


                                if (!relationsDict.ContainsKey(chosenRelation))
                                {
                                    relationsDict.Add(chosenRelation, new List<MyPoint>());
                                    relationsDict[chosenRelation].Add(result.Item3);
                                    result.Item3.relations.Add(chosenRelation);
                                    sortRelations();
                                }
                                else
                                {
                                    if (relationsDict[chosenRelation].Contains(result.Item3))
                                    {
                                        MessageBox.Show("taka relacja już istnieje", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        if (relationsDict[chosenRelation].Count == 2)
                                        {
                                            MessageBox.Show("już są 2 relacje", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            int idx1 = allPoints.IndexOf(relationsDict[chosenRelation][0]);
                                            int idx2 = allPoints.IndexOf(result.Item3);
                                            relationsGraph.AddEdge(idx1, idx2);
                                            if (!isAcyclic(relationsGraph))
                                            {
                                                MessageBox.Show("powstał cykl", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                relationsGraph.RemoveEdge(idx1, idx2);
                                            }
                                            else
                                            {
                                                relationsDict[chosenRelation].Add(result.Item3);
                                                result.Item3.relations.Add(chosenRelation);
                                                result.Item3.relations.Sort();
                                                sortRelations();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("za dużo relacji", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            // relationsDict[chosenRelation].Add(result.Item3);
                        }
                        correctPointByLength(result.Item2);
                    }
                    
                }
                printRelations();
            }
            else if (chosenButton == 6)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        chosenPointRel = result.Item3;

                        Form4 form = new Form4();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();

                        if (chosenRelation != -1)
                        {
                            result.Item3.relations.Remove(chosenRelation);
                            relationsDict[chosenRelation].Remove(result.Item3);
                            if (relationsDict[chosenRelation].Count == 0)
                            {
                                relationsDict.Remove(chosenRelation);
                            }
                        }
                    }
                }
                correctPointByLength(null);
                printRelations();
            }


            DrawCanvas(e.X, e.Y);

        }

        void printRelations()
        {

            foreach (var key in relationsDict.Keys)
            {
                if (relationsDict[key].Count == 1)
                {
                    Debug.WriteLine($"key:{key}, idx:{relationsDict[key][0].index}");
                }
                else
                {
                    Debug.WriteLine($"key:{key}, idx1:{relationsDict[key][0].index}, idx2:{relationsDict[key][1].index}");
                }
            }
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
            try
            {
                drawArea.Dispose();
                drawArea = new Bitmap(Canvas.Width, Canvas.Height);
                Canvas.Image = drawArea;
                using (Graphics g = Graphics.FromImage(drawArea))
                {
                    g.Clear(Color.White);
                }

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
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        g.DrawLine(pen, points[i], points[i + 1]);
                    }
                    for (int i = 0; i < points.Count; i++)
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
                    if (chosenButton == 1 && points.Count != 0) g.DrawLine(pen, points[points.Count - 1], new MyPoint(mouseX, mouseY));
                }
                using (Graphics g = Graphics.FromImage(drawArea))
                {
                    foreach (var polygon in polygons)
                    {
                        for (int i = 0; i < polygon.Count; i++)
                        {
                            string text = "";
                            if (polygon[i].relations.Count > 0)
                            {

                                foreach (var number in polygon[i].relations)
                                {
                                    text += number.ToString();
                                    text += " ";
                                }
                                // text = text.Substring(0, text.Length - 1);

                            }
                            if (polygon[i].length != -1.0)
                            {
                                text += "len. = ";
                                text += polygon[i].length.ToString();
                            }
                            //text += " idx: ";
                            //text += polygon[i].index.ToString();
                            Point middle = new Point((polygon[i].x + polygon[(i + 1) % polygon.Count].x) / 2, (polygon[i].y + polygon[(i + 1) % polygon.Count].y) / 2);
                            using (Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                            {
                                StringFormat sf = new StringFormat();
                                sf.LineAlignment = StringAlignment.Center;
                                sf.Alignment = StringAlignment.Center;
                                g.DrawString(text, font1, sbBlack, middle.X, middle.Y, sf);
                            }
                        }
                    }
                }
            }
            catch(OverflowException e)
            {
                MessageBox.Show("wierzchołek znalazł się zbyt daleko miejsca do rysowania", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                polygons[pointToMove.Item1][pointToMove.Item2].x = mouseX;
                polygons[pointToMove.Item1][pointToMove.Item2].y = mouseY;
                correctPointByLength(polygons[pointToMove.Item1]);
                moving = 0;
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
                    // TODO: when moving point/edge it shouldn't be colored anymore
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
                    MyPoint p = polygons[pointToMove.Item1][pointToMove.Item2];
                    p.x = e.X;
                    p.y = e.Y;
                    correctPointByLength(polygons[pointToMove.Item1]);
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
                    polygons[edgeToMove.Item1][edgeToMove.Item2].x = a.x;
                    polygons[edgeToMove.Item1][edgeToMove.Item2].y = a.y;
                    polygons[edgeToMove.Item1][(edgeToMove.Item2 + 1) % polygons[edgeToMove.Item1].Count].x = b.x;
                    polygons[edgeToMove.Item1][(edgeToMove.Item2 + 1) % polygons[edgeToMove.Item1].Count].y = b.y;
                    correctPointByLength(polygons[edgeToMove.Item1]);
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
                        polygonToMove[i].x = temp.x;
                        polygonToMove[i].y = temp.y;
                    }
                    correctPointByLength(polygonToMove);
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
            foreach (var polygon in polygons)
            {
                counter = 0;
                for (int i = 0; i < polygon.Count; i++)
                {
                    MyPoint A = polygon[i];
                    MyPoint B = polygon[(i + 1) % polygon.Count];
                    if (areIntersecting(0, 0, x, y, A.x, A.y, B.x, B.y) == 1) counter++;
                }
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

        // TODO: no arguments there
        void correctPointByLength(List<MyPoint> jkasdbf)
        {
            foreach (var polygon in polygons)
            {
                for (int i = 0; i < polygon.Count; i++)
                {
                    foreach (var key in relationsDict.Keys)
                    {
                        if (relationsDict[key].Count == 2 && relationsDict[key][1] == polygon[i])
                        {
                            // drawing perpendicular
                            // TODO: field in MyPoint to polygon?
                            // JEBIE SIĘ PRZY ŁĄCZENIU OSTATNIEGO Z PIERWSZYM -- DLACZEGO????
                            MyPoint p1 = relationsDict[key][0];
                            List<MyPoint> polygonWithP1 = null;
                            foreach (var p in polygons)
                            {
                                if (p.Contains(p1))
                                {
                                    polygonWithP1 = p;
                                    break;
                                }
                            }
                            MyPoint p2 = polygonWithP1[(polygonWithP1.IndexOf(p1) + 1) % polygonWithP1.Count];
                            MyPoint p3 = relationsDict[key][1];
                            MyPoint p4 = polygon[(polygon.IndexOf(p3) + 1) % polygon.Count];
                            if(p1 == p4)
                            {
                                MyPoint temp = p3;
                                p3 = p4;
                                p4 = temp;
                            }

                            if (p1.x == p2.x)
                            {
                                Debug.WriteLine($"first {rnd.Next()}");
                                p4.y = p3.y;
                            }
                            else
                            {
                                double a1 = (double)(p1.y - p2.y) / (double)(p1.x - p2.x);
                                double a2 = -1.0 / a1;
                                double b = p3.y - a2 * p3.x;
                                p4.y = (int)(a2 * p4.x + b);
                                Debug.WriteLine($"key:{key}, a1:{a1}, a2:{a2}, {rnd.Next()}");
                            }
                        }
                    }
                    if (polygon[i].length != -1.0)
                    {
                        MyPoint p = polygon[(i + 1) % polygon.Count];
                        double dist = getDistance(polygon[i].x, polygon[i].y, p.x, p.y);
                        double scale = polygon[i].length / dist;
                        double lengthX = p.x - polygon[i].x;
                        double lengthY = p.y - polygon[i].y;
                        lengthX *= scale;
                        lengthY *= scale;
                        p.x = (int)(polygon[i].x + lengthX);
                        p.y = (int)(polygon[i].y + lengthY);
                    }
                }
            }
        }

        bool checkIfCanAddNewLength(List<MyPoint> polygon)
        {
            int counter = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].length != -1.0)
                {
                    counter++;
                }
            }
            if (counter == polygon.Count - 1) return false;
            return true;
        }

        bool checkIfCanAddNewRelation(List<MyPoint> polygon)
        {
            int counter = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                foreach (var val in relationsDict.Values)
                {
                    if (val.Contains(polygon[i]))
                    {
                        counter++;
                        break;
                    }
                }
            }
            if (counter == polygon.Count - 1) return false;
            return true;
        }

        void sortRelations()
        {
            foreach (var key in relationsDict.Keys)
            {
                if (relationsDict[key].Count == 2 && relationsDict[key][1].index < relationsDict[key][0].index)
                {
                    MyPoint temp = relationsDict[key][0];
                    relationsDict[key][0] = relationsDict[key][1];
                    relationsDict[key][1] = temp;
                    Debug.WriteLine("sorted");
                }
            }
        }

        public bool isAcyclic(Graph g)
        {

            ASD.PriorityQueue<int, Edge> queue = new ASD.PriorityQueue<int, Edge>();


            foreach (var e in g.DFS().SearchAll())
            {
                if (e.From > e.To)
                {
                    queue.Insert(e, 1);
                }
            }



            ASD.UnionFind uf = new ASD.UnionFind(g.VertexCount);

            while (queue.Count > 0)
            {
                var e = queue.Extract();
                if (uf.Find(e.From) != uf.Find(e.To))
                {
                    uf.Union(e.From, e.To);
                }
                else
                {
                    return false;
                }

            }

            return true;
        }

        // TODO: add length field - when set, moving an edge doesn't change its length
        public class MyPoint
        {
            // TODO: change to setters?
            // TODO: make new List<int> not to be in every constructor
            public int x;
            public int y;
            public int index = -1;
            public double length = -1.0;
            // public double lengthPrev = -1.0;

            public List<int> relations = new List<int>();

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

        // TODO: new graph when deleting vertex
        void createNewGraph()
        {
            int counter = 0;
            foreach (var polygon in polygons)
            {
                counter += polygon.Count;
            }
            relationsGraph = new Graph(counter);
            allPoints = new List<MyPoint>();
            counter = 0;
            foreach (var polygon in polygons)
            {
                foreach (var point in polygon)
                {
                    allPoints.Add(point);
                    point.index = counter;
                    counter++;
                }
            }
            foreach (var key in relationsDict.Keys)
            {
                if (relationsDict[key].Count == 2)
                {
                    int idx1 = allPoints.IndexOf(relationsDict[key][0]);
                    int idx2 = allPoints.IndexOf(relationsDict[key][1]);
                    relationsGraph.AddEdge(idx1, idx2);
                }
            }
        }
    }
}
