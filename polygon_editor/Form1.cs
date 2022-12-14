// Jakub Sosnowski https://github.com/Sosssen

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


namespace polygon_editor
{
    public partial class polygon_editor : Form
    {
        int errorCounter = 0;
        public static double edgeLength = 0;
        public static bool edgeLengthChanged = false;
        public static MyPoint selectedPointRel = null;
        public static int selectedRelation = -1;
        public static Dictionary<int, List<MyPoint>> relationsDict = new();
        Graph relationsGraph = null;

        private Bitmap drawArea;
        private readonly Pen pen = new(Color.Black, 1);
        private readonly Pen redPen = new(Color.Red, 6);
        private readonly SolidBrush sbBlack = new(Color.Black);
        private readonly SolidBrush sbRed = new(Color.Red);
        private readonly SolidBrush sbGreen = new(Color.Green);

        private int chosenButton;

        private List<MyPoint> points = new();
        public static List<List<MyPoint>> polygons = new();

        private const int radius = 4;

        private bool colorPoint = false;
        private MyPoint pointToColor = new();

        private bool colorEdge = false;
        private MyPoint edgeToColor = new();

        private bool colorPolygon = false;
        private List<MyPoint> polygonToColor = null;

        private int moving = 0;
        private (int, int) pointToMove;
        private (int, int) edgeToMove;
        private MyPoint startingPoint = new();
        private MyPoint startingPointA = new();
        private MyPoint startingPointB = new();
        private List<MyPoint> polygonToMove = null;
        private List<MyPoint> polygonToMoveCopy = null;

        private bool bresenham = false;
        public polygon_editor()
        {

            InitializeComponent();
            this.Text = "Polygon Editor";
            this.Icon = Properties.Resources.pe_icon;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

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
            CLEAR.BackColor = SystemColors.Control;
            SCENE.BackColor = SystemColors.Control;
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
            if (bresenham)
            {
                MessageBox.Show("Turn off rendering with Bresenham's algorithm", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                                MessageBox.Show("Polygon need at least 3 nodes", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show("You can connect only with first node", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var result = FindPointInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        int idx = result.Item2.IndexOf(result.Item3);
                        result.Item2[(idx - 1) % result.Item2.Count].length = -1.0;
                        result.Item3.removeRelations();
                        result.Item2[(idx - 1) % result.Item2.Count].removeRelations();
                        result.Item2.Remove(result.Item3);
                        
                        if (result.Item2.Count < 3)
                        {
                            foreach (var point in result.Item2)
                            {
                                point.removeRelations();
                            }
                            polygons.Remove(result.Item2);
                        }
                        createNewGraph();
                    }
                    else
                    {
                        var result2 = FindEdgeInPolygons(e.X, e.Y);
                        if (result2.Item1)
                        {
                            int idx = result2.Item2.IndexOf(result2.Item3);
                            result2.Item2[(idx - 1) % result2.Item2.Count].length = -1.0;
                            result2.Item3.removeRelations();
                            result2.Item4.removeRelations();
                            result2.Item2[(idx - 1) % result2.Item2.Count].removeRelations();
                            result2.Item2.Remove(result2.Item3);
                            result2.Item2.Remove(result2.Item4);

                            if (result2.Item2.Count < 3)
                            {
                                foreach (var point in result2.Item2)
                                {
                                    point.removeRelations();
                                }
                                polygons.Remove(result2.Item2);
                            }
                            createNewGraph();
                        }
                        else
                        {
                            var result3 = FindPolygon(e.X, e.Y);
                            if(result3.Item1)
                            {
                                foreach (var point in result3.Item2)
                                {
                                    point.removeRelations();
                                }
                                polygons.Remove(result3.Item2);
                            }
                            createNewGraph();
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
                        createNewGraph();
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
                        edgeLength = getDistance(p1.x, p1.y, p2.x, p2.y);
                        Form2 form = new Form2();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog(this);

                        if (edgeLengthChanged)
                        {
                            if (checkIfCanAddNewLimit(result.Item2) || result.Item3.relations.Count > 0)
                            {
                                result.Item3.length = edgeLength;
                                correctPoints();
                            }
                            else
                            {
                                MessageBox.Show("Too many limits", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    var result = FindEdgeInPolygons(e.X, e.Y);
                    if (result.Item1)
                    {
                        result.Item3.length = -1.0;
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
                        selectedPointRel = result.Item3;

                        Form3 form = new Form3();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();

                        if (selectedRelation != -1)
                        {
                            if (checkIfCanAddNewLimit(result.Item2) || result.Item3.relations.Count > 0)
                            {


                                if (!relationsDict.ContainsKey(selectedRelation))
                                {
                                    relationsDict.Add(selectedRelation, new List<MyPoint>());
                                    relationsDict[selectedRelation].Add(result.Item3);
                                    result.Item3.relations.Add(selectedRelation);
                                }
                                else
                                {
                                    if (relationsDict[selectedRelation].Contains(result.Item3))
                                    {
                                        MessageBox.Show("This relation already exists on this edge", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        if (relationsDict[selectedRelation].Count == 2)
                                        {
                                            MessageBox.Show("There are 2 edges in this relation already", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            if (!checkIfCanAddRelation(selectedRelation, result.Item3))
                                            {
                                                MessageBox.Show("You can't add this relation to this edge", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            else
                                            {
                                                createNewGraph();
                                                int idx1 = relationsDict[selectedRelation][0].countIndex();
                                                int idx2 = result.Item3.countIndex();
                                                relationsGraph.AddEdge(idx1, idx2);
                                                if (!isAcyclic(relationsGraph))
                                                {
                                                    MessageBox.Show("There is cycle", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    relationsGraph.RemoveEdge(idx1, idx2);
                                                }
                                                else
                                                {
                                                    relationsDict[selectedRelation].Add(result.Item3);
                                                    sortRelations();
                                                    result.Item3.relations.Add(selectedRelation);
                                                    result.Item3.relations.Sort();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Too many limits", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        correctPoints();
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
                        selectedPointRel = result.Item3;

                        Form4 form = new Form4();
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();

                        if (selectedRelation != -1)
                        {
                            result.Item3.relations.Remove(selectedRelation);
                            relationsDict[selectedRelation].Remove(result.Item3);
                            if (relationsDict[selectedRelation].Count == 0)
                            {
                                relationsDict.Remove(selectedRelation);
                            }
                        }
                    }
                }
                correctPoints();
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
            try
            {
                drawArea.Dispose();
                drawArea = new Bitmap(Canvas.Width, Canvas.Height);
                Canvas.Image = drawArea;
                using (Graphics g = Graphics.FromImage(drawArea))
                {
                    g.Clear(Color.White);
                }

                if (bresenham)
                {
                    foreach (var polygon in polygons)
                    {
                        for (int i = 0; i < polygon.Count; i++)
                        {
                            var p1 = polygon[i];
                            var p2 = polygon[(i + 1) % polygon.Count];
                            drawLine(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                }
                else
                {

                    using (Graphics g = Graphics.FromImage(drawArea))
                    {
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

                                }
                                if (polygon[i].length != -1.0)
                                {
                                    text += "len. = ";
                                    text += polygon[i].length.ToString();
                                }
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
            }
            catch
            {
                MessageBox.Show("Node is too far from canva", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorCounter++;
                if (errorCounter >= 5)
                {
                    MessageBox.Show("Can't repair this state, app is gonna close", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                polygons[pointToMove.Item1][pointToMove.Item2].x = mouseX;
                polygons[pointToMove.Item1][pointToMove.Item2].y = mouseY;
                correctPoints();
                moving = 0;
            }
        }

        private void CREATE_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = Color.LightBlue;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            chosenButton = 1;
        }

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
                    MyPoint p = polygons[pointToMove.Item1][pointToMove.Item2];
                    p.x = e.X;
                    p.y = e.Y;
                    correctPoints();
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
                    correctPoints();
                }
                else if (moving == 3)
                {
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
                    correctPoints();
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

        int areIntersecting(float v1x1, float v1y1, float v1x2, float v1y2, float v2x1, float v2y1, float v2x2, float v2y2)
        {
            float d1, d2;
            float a1, a2, b1, b2, c1, c2;

            a1 = v1y2 - v1y1;
            b1 = v1x1 - v1x2;
            c1 = (v1x2 * v1y1) - (v1x1 * v1y2);

            d1 = (a1 * v2x1) + (b1 * v2y1) + c1;
            d2 = (a1 * v2x2) + (b1 * v2y2) + c1;

            if (d1 > 0 && d2 > 0) return 0;
            if (d1 < 0 && d2 < 0) return 0;

            a2 = v2y2 - v2y1;
            b2 = v2x1 - v2x2;
            c2 = (v2x2 * v2y1) - (v2x1 * v2y2);

            d1 = (a2 * v1x1) + (b2 * v1y1) + c2;
            d2 = (a2 * v1x2) + (b2 * v1y2) + c2;

            if (d1 > 0 && d2 > 0) return 0;
            if (d1 < 0 && d2 < 0) return 0;

            if ((a1 * b2) - (a2 * b1) == 0.0f) return 2;

            return 1;
        }

        void sortRelations()
        {
            foreach (var key in relationsDict.Keys)
            {
                if (relationsDict[key].Count == 2 && relationsDict[key][1].countIndex() < relationsDict[key][0].countIndex())
                {
                    MyPoint temp = relationsDict[key][0];
                    relationsDict[key][0] = relationsDict[key][1];
                    relationsDict[key][1] = temp;
                }
            }
        }

        void correctPoints()
        {
            int max = 0;
            foreach (var key in relationsDict.Keys)
            {
                if (key > max) max = key;
            }
            int[] relations = new int[max + 1];
            for (int i = 0; i < relations.Length; i++)
            {
                relations[i] = 0;
            }
            foreach (var polygon in polygons)
            {
                int idx;
                for (idx = 0; idx < polygon.Count; idx++)
                {
                    if (polygon[idx].length != -1.0) continue;
                    bool mark = false;
                    foreach(var key in relationsDict.Keys)
                    {
                        if (relationsDict[key][0] == polygon[idx] || (relationsDict[key].Count == 2 && relationsDict[key][1] == polygon[idx]))
                        {
                            mark = true;
                            break;
                        }
                    }
                    if (mark) continue;
                    break;
                }
                for (int i = 0; i < polygon.Count; i++)
                {
                    int newIdx = (idx + 1 + i) % polygon.Count;
                    foreach (var relation in polygon[newIdx].relations)
                    {
                        relations[relation]++;
                        if (relations[relation] == 1) continue;
                        MyPoint p1 = relationsDict[relation][0];
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
                        MyPoint p3 = relationsDict[relation][1];
                        MyPoint p4 = polygon[(polygon.IndexOf(p3) + 1) % polygon.Count];

                        double len = getDistance(p3.x, p3.y, p4.x, p4.y);

                        if (p1.x == p2.x)
                        {
                            p4.y = p3.y;
                        }
                        else if (p1.y == p2.y)
                        {
                            p4.x = p3.x;
                        }
                        else
                        {
                            double a1 = (double)(p1.y - p2.y) / (double)(p1.x - p2.x);
                            double a2 = -1.0 / a1;
                            double b = p3.y - a2 * p3.x;
                            if (Math.Abs(p4.x - p3.x) < Math.Abs(p4.y - p3.y))
                            {
                                p4.x = (int)((p4.y - b) / a2);
                            }
                            else
                            {
                                p4.y = (int)(a2 * p4.x + b);
                            }
                        }

                        double newLen = getDistance(p3.x, p3.y, p4.x, p4.y);
                        double scale = len / newLen;
                        double lengthX = p4.x - p3.x;
                        double lengthY = p4.y - p3.y;
                        lengthX *= scale;
                        lengthY *= scale;
                        p4.x = (int)(p3.x + lengthX);
                        p4.y = (int)(p3.y + lengthY);

                    }
                    if (polygon[newIdx].length != -1.0)
                    {
                        MyPoint p = polygon[(newIdx + 1) % polygon.Count];
                        double dist = getDistance(polygon[newIdx].x, polygon[newIdx].y, p.x, p.y);
                        double scale = polygon[newIdx].length / dist;
                        double lengthX = p.x - polygon[newIdx].x;
                        double lengthY = p.y - polygon[newIdx].y;
                        lengthX *= scale;
                        lengthY *= scale;
                        p.x = (int)(polygon[newIdx].x + lengthX);
                        p.y = (int)(polygon[newIdx].y + lengthY);
                    }
                }
            }
        }

        bool checkIfCanAddNewLimit(List<MyPoint> polygon)
        {
            int counter = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].length != -1.0)
                {
                    counter++;
                    continue;
                }
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

        bool checkIfCanAddRelation(int relation, MyPoint point)
        {
            var point2 = relationsDict[relation][0];
            int counter1 = 0;
            int counter2 = 0;
            relationsDict[relation].Add(point);
            sortRelations();
            foreach (var key in relationsDict.Keys)
            {
                if (relationsDict[key].Count == 2 && relationsDict[key][1] == point)
                {
                    counter1++;
                }
                else if(relationsDict[key].Count == 2 && relationsDict[key][1] == point2)
                {
                    counter2++;
                }
            }
            relationsDict[relation].Remove(point);
            if (counter1 > 1 || counter2 > 1) return false;
            return true;
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

        public class MyPoint
        {
            public int x;
            public int y;
            public int index = -1;
            public double length = -1.0;

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

            public int countIndex()
            {
                int ret = 0;
                foreach (var polygon in polygon_editor.polygons)
                {
                    int idx;
                    for (idx = 0; idx < polygon.Count; idx++)
                    {
                        if (polygon[idx].length != -1.0) continue;
                        bool mark = false;
                        foreach (var key in relationsDict.Keys)
                        {
                            if (relationsDict[key][0] == polygon[idx] || (relationsDict[key].Count == 2 && relationsDict[key][1] == polygon[idx]))
                            {
                                mark = true;
                                break;
                            }
                        }
                        if (mark) continue;
                        break;
                    }
                    for (int i = 0; i < polygon.Count; i++)
                    {
                        if (polygon[(idx + i) % polygon.Count] == this) return ret;
                        ret++;
                    }
                }
                return -1;
            }

            public void removeRelations()
            {
                foreach (var key in relationsDict.Keys)
                {
                    relationsDict[key].Remove(this);
                    if (relationsDict[key].Count == 0)
                    {
                        relationsDict.Remove(key);
                    }
                }
                relations = new List<int>();
            }

            public static implicit operator Point(MyPoint p) => new Point(p.x, p.y);
        }

        void createNewGraph()
        {
            int counter = 0;
            foreach (var polygon in polygons)
            {
                counter += polygon.Count;
            }
            relationsGraph = new Graph(counter);
            foreach (var key in relationsDict.Keys)
            {
                if (relationsDict[key].Count == 2)
                {
                    int idx1 = relationsDict[key][0].countIndex();
                    int idx2 = relationsDict[key][1].countIndex();
                    relationsGraph.AddEdge(idx1, idx2);
                }
            }
        }

        private void CLEAR_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            CLEAR.BackColor = SystemColors.Control;
            SCENE.BackColor = SystemColors.Control;
            chosenButton = 0;

            if (bresenham)
            {
                MessageBox.Show("Turn off rendering with Bresenham's algorithm", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            points = new List<MyPoint>();
            polygons = new List<List<MyPoint>>();
            relationsDict = new Dictionary<int, List<MyPoint>>();

            DrawCanvas();
        }

        private void SCENE_Click(object sender, EventArgs e)
        {
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            CLEAR.BackColor = SystemColors.Control;
            SCENE.BackColor = SystemColors.Control;
            chosenButton = 0;

            if (bresenham)
            {
                MessageBox.Show("Turn off rendering with Bresenham's algorithm", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            points = new List<MyPoint>();
            polygons = new List<List<MyPoint>>();
            relationsDict = new Dictionary<int, List<MyPoint>>();

            List<MyPoint> list1 = new List<MyPoint>();
            List<MyPoint> list2 = new List<MyPoint>();

            list1.Add(new MyPoint(300, 100));
            list1.Add(new MyPoint(100, 400));
            list1.Add(new MyPoint(400, 200));
            list1[0].length = 300.0;

            polygons.Add(list1);

            list2.Add(new MyPoint(250, 500));
            list2.Add(new MyPoint(200, 600));
            list2.Add(new MyPoint(500, 650));
            list2.Add(new MyPoint(550, 550));
            list2.Add(new MyPoint(400, 450));
            list2[3].length = 200.0;

            polygons.Add(list2);

            List<MyPoint> rel1 = new List<MyPoint>();
            rel1.Add(list1[2]);
            list1[2].relations.Add(0);
            rel1.Add(list2[0]);
            list2[0].relations.Add(0);

            relationsDict.Add(0, rel1);

            List<MyPoint> rel2 = new List<MyPoint>();
            rel2.Add(list2[2]);
            list2[2].relations.Add(1);
            rel2.Add(list2[4]);
            list2[4].relations.Add(1);

            relationsDict.Add(1, rel2);

            sortRelations();
            createNewGraph();
            correctPoints();

            DrawCanvas();
        }

        bool isChecked = false;
        private void BRESENHAM_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = BRESENHAM.Checked;
        }

        private void BRESENHAM_Click(object sender, EventArgs e)
        {
            if (BRESENHAM.Checked && !isChecked)
                BRESENHAM.Checked = false;
            else
            {
                BRESENHAM.Checked = true;
                isChecked = false;
            }
            CREATE.BackColor = SystemColors.Control;
            MODIFY.BackColor = SystemColors.Control;
            MIDDLE_INSERT.BackColor = SystemColors.Control;
            SET_LENGTH.BackColor = SystemColors.Control;
            ADD_REL.BackColor = SystemColors.Control;
            REMOVE_REL.BackColor = SystemColors.Control;
            CLEAR.BackColor = SystemColors.Control;
            SCENE.BackColor = SystemColors.Control;
            chosenButton = 0;
            bresenham = BRESENHAM.Checked;
            DrawCanvas();
        }

        private void drawLine(int x0, int y0, int x1, int y1)
        {
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                {
                    drawLineLow(x1, y1, x0, y0);
                }
                else
                {
                    drawLineLow(x0, y0, x1, y1);
                }
            }
            else
            {
                if (y0 > y1)
                {
                    drawLineHigh(x1, y1, x0, y0);
                }
                else
                {
                    drawLineHigh(x0, y0, x1, y1);
                }
            }
        }

        private void drawLineLow(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int sy = 1;
            if (dy < 0)
            {
                sy = -1;
                dy = -dy;
            }
            int D = 2 * dy - dx;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                if (x >= 0 && y >= 0 && x < drawArea.Width && y < drawArea.Height) drawArea.SetPixel(x, y, Color.Black);
                if (D > 0)
                {
                    y += sy;
                    D += 2 * (dy - dx);
                }
                else
                {
                    D += 2 * dy;
                }
            }
        }

        private void drawLineHigh(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int sx = 1;
            if (dx < 0)
            {
                sx = -1;
                dx = -dx;
            }
            int D = 2 * dx - dy;
            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                if (x >= 0 && y >= 0 && x < drawArea.Width && y < drawArea.Height) drawArea.SetPixel(x, y, Color.Black);
                if (D > 0)
                {
                    x += sx;
                    D += 2 * (dx - dy);
                }
                else
                {
                    D += 2 * dx;
                }
            }
        }
    }
}
