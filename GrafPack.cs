using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GrafPack
{
    public partial class GrafPack : Form
    {
        //Declare MainMenu object
        private MainMenu mainMenu;
        
        //Declare and initialise bool variables to identify shape to be drawn
        private bool selectSquareStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectCircleStatus = false;
        private bool selectShapeStatus = false;

        //Declare point variables used to draw shapes
        private Point one;
        private Point two;

        //Declare count variable to be used to select shapes on the screen
        int selectionCount = 0;

        //Declare a shape list to store all shapes drawn to the screen
        private List<Shape> shapes;

        //Declare and innitialise bool variable to determine whether the mouse is held down or not
        private bool isMouseDown = false;



        public GrafPack()
        {
            InitializeComponent();
            //Initialise window size and background color
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;

            //Initialise menu object and declare menu items
            mainMenu = new MainMenu();
            MenuItem createItem = new MenuItem();
            MenuItem selectItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem circleItem = new MenuItem();
            MenuItem deleteItem = new MenuItem();
            MenuItem transformItem = new MenuItem();
            MenuItem rotateItem = new MenuItem();
            MenuItem reflectItem = new MenuItem();
            MenuItem translateItem = new MenuItem();

            //Assign Text values to menu items
            createItem.Text = "&Create";
            transformItem.Text = "&Transform";
            selectItem.Text = "&Select";
            deleteItem.Text = "&Delete";
            squareItem.Text = "&Square";
            triangleItem.Text = "&Triangle";
            circleItem.Text = "&Circle";
            rotateItem.Text = "&Rotate";
            reflectItem.Text = "&Reflect";
            translateItem.Text = "&Translate";
            
            //Add menu items to the mainMenu object
            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(transformItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(deleteItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(circleItem);
            transformItem.MenuItems.Add(rotateItem);
            transformItem.MenuItems.Add(reflectItem);
            transformItem.MenuItems.Add(translateItem);

            //Add mouse click listeners to the menu items
            selectItem.Click += new System.EventHandler(this.selectShape);
            deleteItem.Click += new System.EventHandler(this.deleteShape);
            squareItem.Click += new System.EventHandler(this.selectSquare);
            triangleItem.Click += new System.EventHandler(this.selectTriangle);
            circleItem.Click += new System.EventHandler(this.selectCircle);
            rotateItem.Click += new System.EventHandler(this.rotateShape);
            reflectItem.Click += new System.EventHandler(this.reflectShape);
            translateItem.Click += new System.EventHandler(this.translateShape);

            //assign menu object to this form's menu
            this.Menu = mainMenu;

            //Initialise shape list
            shapes = new List<Shape>();

            //Add a keyboard listener to the form
            this.KeyUp += GrafPack_KeyUp;
        }

        private void selectSquare(object sender, EventArgs e)
        {
            selectSquareStatus = true;
            selectShapeStatus = false;
        }

        private void selectTriangle(object sender, EventArgs e)
        {
            selectTriangleStatus = true;
            selectShapeStatus = false;
        }

        private void selectCircle(object sender, EventArgs e)
        {
            selectCircleStatus = true;
            selectShapeStatus = false;
        }

        private void selectShape(object sender, EventArgs e)
        {
            selectShapeStatus = true;
            selectionCount = 0;
            selectingShape();
        }

        private void deleteShape(object sender, EventArgs e)
        {
            selectShapeStatus = false;
            if(shapes.Count > 0)shapes.RemoveAt(selectionCount);
            drawShapes();
        }

        private void translateShape(object sender, EventArgs e)
        {
            selectShapeStatus = false;
            try
            {
                //Get angle of rotation from user
                int transX = int.Parse(Interaction.InputBox("Translate X coordinates by: ",
                    "Translation", "0"));
                int transY = int.Parse(Interaction.InputBox("Translate Y coordinates by: ",
                    "Translation", "0"));

                //translate shape with given constants
                translateShape(transX, transY);

                //Redraw shapes to show rotation
                drawShapes();
            }
            catch (Exception exception)
            { }
        }

        private void reflectShape(object sender, EventArgs e)
        {
        }

        private void rotateShape(object sender, EventArgs e)
        {
            selectShapeStatus=false;
            try
            {
                //Get angle of rotation from user
                int angle = int.Parse(Interaction.InputBox("Enter angle of Rotation",
                    "Rotation", "0"));

                //rotate shape with provided angle
                rotateShape(angle);

                //Redraw shapes to show rotation
                drawShapes();
            }
            catch (Exception exception)
            { }
        }

        private void GrafPack_Load(object sender, EventArgs e)
        {

        }

        private void GrafPack_MouseDown(object sender, MouseEventArgs e)
        {
            //Mouse down event handler method
            isMouseDown = true;

            one = e.Location;
        }

        private void GrafPack_MouseMove(object sender, MouseEventArgs e)
        {
            //Mouse move event handler method
            if (isMouseDown)
            {
                two = e.Location;
            }
        }

        private void GrafPack_MouseUp(object sender, MouseEventArgs e)
        {
            //Mouse up event handler method
            if (isMouseDown)
            {
                two = e.Location;

                isMouseDown = false;

                addShape();

                drawShapes();
            }
        }

        private void GrafPack_KeyUp(object sender, KeyEventArgs e)
        {
            if (selectShapeStatus)
            {
                if (e.KeyCode == Keys.Up)
                {
                    selectionCount++;
                    if (selectionCount > shapes.Count) selectionCount = shapes.Count;
                    selectingShape();
                }

                if (e.KeyCode == Keys.Down)
                {
                    selectionCount--;
                    if (selectionCount < 0) selectionCount = 0;
                    selectingShape();
                }
            }
        }

        private void selectingShape()
        {
            //method to selet a shape on the screen
            Refresh();

            int count = 0;
            Graphics g = this.CreateGraphics();
            Pen blackpen = new Pen(Color.Black);

            //Declare and initialise selection pen
            Pen selectionPen = new Pen(Color.Red, 5);
            selectionPen.Alignment = PenAlignment.Center;

            foreach (Shape shape in shapes)
            {
                shape.draw(g, blackpen);
                if (count == selectionCount)
                {
                    shape.draw(g, selectionPen);
                }

                count++;
            }
        }

        private void rotateShape(int angle)
        {
            int count = 0;

            foreach (Shape shape in shapes)
            {
                if (count == selectionCount)
                {
                    shape.rotate(angle);
                    break;
                }
                count++;
            }
        }

        private void translateShape(int transX, int transY)
        {
            int count = 0;

            foreach (Shape shape in shapes)
            {
                if (count == selectionCount)
                {
                    shape.translate(transX, transY);
                    break;
                }
                count++;
            }
        }

        private void addShape()
        {
            if (selectSquareStatus == true)
            {
                selectSquareStatus = false;
                //add shape to shapes list
                shapes.Add(new Square(one, two));
            }

            if (selectTriangleStatus == true)
            {
                selectTriangleStatus = false;
                //add shape to shapes list
                shapes.Add(new Triangle(one, two));
            }

            if (selectCircleStatus == true)
            {
                selectCircleStatus = false;
                //add shape to shapes list
                shapes.Add(new Circle(one, two));
            }
        }

        private void drawShapes()
        {
            //method to draw shapes on the screen

            Refresh();

            Graphics g = this.CreateGraphics();
            Pen blackpen = new Pen(Color.Black);

            foreach (Shape shape in shapes)
            {
                shape.draw(g, blackpen);
            }
        }

        //Method to start the application
        public static void Main()
        {
            Application.Run(new GrafPack());
        }
    }

    abstract class Shape
    {
        // This is the base class for Shapes in the application. It should allow an array or LL
        // to be created containing different kinds of shapes.
        protected double xDiff, yDiff, xMid, yMid;   // range and mid points of x & y 

        public Shape()   // constructor
        {            
        }

        protected virtual void calculateRangesAndMidpoints()
        {
        }

        public virtual void draw(Graphics g, Pen blackPen)
        {
        }

        public virtual void rotate(int angle)
        {
        }

        public virtual void translate(int transX, int transY)
        {
        }
    }
    
    class Square : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        Point keyPt, oppPt, newPt1, newPt2;      // these points identify opposite corners of the square

        public Square(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }

        protected override void calculateRangesAndMidpoints()
        {
            // calculate ranges and mid points
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;
        }

	    public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners

            calculateRangesAndMidpoints();

            //assign new found vertices
            newPt1.X = (int)(xMid + yDiff / 2);
            newPt1.Y = (int)(yMid - xDiff / 2);

            newPt2.X = (int)(xMid - yDiff / 2);
            newPt2.Y = (int)(yMid + xDiff / 2);

            // draw square
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)newPt1.X, (int)newPt1.Y);
            g.DrawLine(blackPen, (int)newPt1.X, (int)newPt1.Y, (int)oppPt.X, (int)oppPt.Y);
            g.DrawLine(blackPen, (int)oppPt.X, (int)oppPt.Y, (int)newPt2.X, (int)newPt2.Y);
            g.DrawLine(blackPen, (int)newPt2.X, (int)newPt2.Y, (int)keyPt.X, (int)keyPt.Y);
        }

        public override void rotate(int angle)
        {
            Point[] points = { keyPt, oppPt, newPt1, newPt2};
            Point[] matrix = new Point[points.Length];

            int centreX, centreY;
            int sumX = 0;
            int sumY = 0;

            // get centre
            foreach (Point p in points)
            {
                sumX += p.X;
                sumY += p.Y;
            }

            centreX = sumX / points.Length;
            centreY = sumY / points.Length;

            // translate shape to origin
            for (int i = 0; i < points.Length; i++)
            {
                matrix[i].X = points[i].X - centreX;
                matrix[i].Y = points[i].Y - centreY;
            }

            float cosa = (float)Math.Cos(angle * Math.PI / 180.0);
            float sina = (float)Math.Sin(angle * Math.PI / 180.0);

            for (int i = 0; i < points.Length; i++)
            {
                float X = matrix[i].X * cosa - matrix[i].Y * sina;
                float Y = matrix[i].X * sina + matrix[i].Y * cosa;

                matrix[i].X = (int)X + centreX;
                matrix[i].Y = (int)Y + centreY;
            }

            //Update Square vertices after rotation
            keyPt = matrix[0];
            oppPt = matrix[1];
            newPt1 = matrix[2];
            newPt2 = matrix[3];
        }

        public override void translate(int transX, int transY)
        {
            //translate shape vertices
            keyPt.X += transX;
            keyPt.Y += transY;

            oppPt.X += transX;
            oppPt.Y += transY;

            newPt1.X += transX;
            newPt1.Y += transY;

            newPt2.X += transX;
            newPt2.Y += transY;
        }
    }

    class Triangle : Shape
    {
        //This class contains the specific details for a triangle defined in terms of opposite corners
        Point keyPt, oppPt, newPt;      // these points identify opposite corners of the triangle

        public Triangle(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }

        protected override void calculateRangesAndMidpoints()
        {
            // calculate ranges and mid points
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;
        }

        public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the triangle by calculating the positions of the other 2 corners

            calculateRangesAndMidpoints();

            //assign new found vertex
            newPt.X = (int)(xMid + yDiff / 2);
            newPt.Y = (int)(yMid - xDiff / 2);

            // draw triangle
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)newPt.X, (int)newPt.Y);
            g.DrawLine(blackPen, (int)newPt.X, (int)newPt.Y, (int)oppPt.X, (int)oppPt.Y);
            g.DrawLine(blackPen, (int)oppPt.X, (int)oppPt.Y, (int)keyPt.X, (int)keyPt.Y);
        }

        public override void rotate(int angle)
        {
            Point[] points = { keyPt, oppPt, newPt };
            Point[] matrix = new Point[points.Length];

            int centreX, centreY;
            int sumX = 0;
            int sumY = 0;

            // get centre
            foreach (Point p in points)
            {
                sumX += p.X;
                sumY += p.Y;
            }

            centreX = sumX / points.Length;
            centreY = sumY / points.Length;

            // translate shape to origin
            for (int i = 0; i < points.Length; i++)
            {
                matrix[i].X = points[i].X - centreX;
                matrix[i].Y = points[i].Y - centreY;
            }

            float cosa = (float)Math.Cos(angle * Math.PI / 180.0);
            float sina = (float)Math.Sin(angle * Math.PI / 180.0);

            for (int i = 0; i < points.Length; i++)
            {
                float X = matrix[i].X * cosa - matrix[i].Y * sina;
                float Y = matrix[i].X * sina + matrix[i].Y * cosa;

                matrix[i].X = (int)X + centreX;
                matrix[i].Y = (int)Y + centreY;
            }

            //Update Square vertices after rotation
            keyPt = matrix[0];
            oppPt = matrix[1];
            newPt = matrix[2];
        }
        public override void translate(int transX, int transY)
        {
            //translate shape vertices
            keyPt.X += transX;
            keyPt.Y += transY;

            oppPt.X += transX;
            oppPt.Y += transY;

            newPt.X += transX;
            newPt.Y += transY;
        }
    }

    class Circle : Shape
    {
        //This class contains the specific details for a triangle defined in terms of opposite corners
        Point keyPt, oppPt;      // these points identify opposite corners of the triangle

        public Circle(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }

        private void putPixel(Graphics g, Point pixel, Color brushColor)
        {
            Brush aBrush = new SolidBrush(brushColor);
            // FillRectangle call fills at location x y and is 1 pixel high by 1 pixel wide
            g.FillRectangle(aBrush, pixel.X, pixel.Y, 1, 1);
        }

        protected override void calculateRangesAndMidpoints()
        {
            // calculate ranges and mid points
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;
        }

        public override void draw(Graphics g, Pen drawingPen)
        {
            // This method draws a circle 2 endpoints of the diameter
            double distance;   // distance between the two given points
            int radius;        // radius of the circle
            Point pixel = new Point();

            // calculate ranges and mid points
            calculateRangesAndMidpoints();

            //Find radius of the circle
            //Mid point of the line crossing both vertices
            distance = Math.Sqrt(((xDiff * xDiff) + (yDiff * yDiff)));
            radius = (int)(Math.Round(distance / 2));

            int x = 0;
            int y = radius;
            int d = 3 - 2 * radius;  // initial value

            while (x <= y)
            {
                // put pixel in each octant
                pixel.X = (int)(x + xMid);
                pixel.Y = (int)(y + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(y + xMid);
                pixel.Y = (int)(x + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(y + xMid);
                pixel.Y = (int)(-x + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(x + xMid);
                pixel.Y = (int)(-y + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(-x + xMid);
                pixel.Y = (int)(-y + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(-y + xMid);
                pixel.Y = (int)(-x + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(-y + xMid);
                pixel.Y = (int)(x + yMid);
                putPixel(g, pixel, drawingPen.Color);

                pixel.X = (int)(-x + xMid);
                pixel.Y = (int)(y + yMid);
                putPixel(g, pixel, drawingPen.Color);

                // update d value 
                if (d <= 0)
                {
                    d = d + 4 * x + 6;
                }
                else
                {
                    d = d + 4 * (x - y) + 10;
                    y--;
                }
                x++;
            }
        }

        public override void rotate(int angle)
        {
            //Rotating a circle will always give the same circle
            //Therefore when rotating a circle it is left the way it is
        }

        public override void translate(int transX, int transY)
        {
            //translate shape vertices
            keyPt.X += transX;
            keyPt.Y += transY;

            oppPt.X += transX;
            oppPt.Y += transY;
        }
    }
}


