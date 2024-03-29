﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace GrafPack
{
    public partial class GrafPack : Form
    {
        //Declare MainMenu object
        private MainMenu mainMenu;
        
        //Declare and initialise bool variables to identify shape to be drawn
        private bool selectSquareStatus = false;
        private bool selectRectangleStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectCircleStatus = false;
        private bool selectShapeStatus = false;
        private bool selectAllShapesStatus = false;

        //Declare point variables used to draw shapes
        private Point one;
        private Point two;

        //Declare count variable to be used to select shapes on the screen
        int selectionCount = 0;

        //Declare a shape list to store all shapes drawn to the screen
        private List<Shape> shapes;
        //Declare a shape object to draw draw the shape progressively as the user
        private Shape currentView;

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
            MenuItem selectOneItem = new MenuItem();
            MenuItem selectAllItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem rectangleItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem circleItem = new MenuItem();
            MenuItem deleteItem = new MenuItem();
            MenuItem transformItem = new MenuItem();
            MenuItem rotateItem = new MenuItem();
            MenuItem reflectItem = new MenuItem();
            MenuItem translateItem = new MenuItem();
            MenuItem scaleItem = new MenuItem();
            MenuItem exitItem = new MenuItem();

            //Assign Text values to menu items
            createItem.Text = "&Create";
            transformItem.Text = "&Transform";
            selectItem.Text = "&Select";
            selectOneItem.Text = "&Select";
            selectAllItem.Text = "&Select All";
            deleteItem.Text = "&Delete";
            squareItem.Text = "&Square";
            rectangleItem.Text = "&Rectangle";
            triangleItem.Text = "&Triangle";
            circleItem.Text = "&Circle";
            rotateItem.Text = "&Rotate";
            reflectItem.Text = "&Reflect";
            translateItem.Text = "&Translate";
            scaleItem.Text = "&Scale";
            exitItem.Text = "&Exit";
            
            //Add menu items to the mainMenu object
            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(transformItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(deleteItem);
            mainMenu.MenuItems.Add(exitItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(rectangleItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(circleItem);
            transformItem.MenuItems.Add(rotateItem);
            transformItem.MenuItems.Add(reflectItem);
            transformItem.MenuItems.Add(translateItem);
            transformItem.MenuItems.Add(scaleItem);
            selectItem.MenuItems.Add(selectOneItem);
            selectItem.MenuItems.Add(selectAllItem);

            //Add mouse click listeners to the menu items
            selectOneItem.Click += new System.EventHandler(this.selectShapeHandler);
            selectAllItem.Click += new System.EventHandler(this.selectAllShapes);
            deleteItem.Click += new System.EventHandler(this.deleteShape);
            exitItem.Click += new System.EventHandler(this.exitApplication);
            squareItem.Click += new System.EventHandler(this.selectSquare);
            rectangleItem.Click += new System.EventHandler(this.selectRectangle);
            triangleItem.Click += new System.EventHandler(this.selectTriangle);
            circleItem.Click += new System.EventHandler(this.selectCircle);
            rotateItem.Click += new System.EventHandler(this.rotateShape);
            reflectItem.Click += new System.EventHandler(this.reflectShape);
            translateItem.Click += new System.EventHandler(this.translateShape);
            scaleItem.Click += new System.EventHandler(this.scaleShape);

            //assign menu object to this form's menu
            this.Menu = mainMenu;

            //Initialise shape list
            shapes = new List<Shape>();

            //Add a keyboard listener to the form
            this.KeyUp += GrafPack_KeyUp;
        }

        private void exitApplication(object sender, EventArgs e)
        {
            //method to close the application
            Application.Exit();
        }

        private void selectSquare(object sender, EventArgs e)
        {
            //method to handle mouse clicks on Square option under Create menu
            selectSquareStatus = true;
            selectShapeStatus = false;
            selectAllShapesStatus = false;
        }

        private void selectRectangle(object sender, EventArgs e)
        {
            //method to handle rectangle clicks on Square option under Create menu
            selectRectangleStatus = true;
            selectShapeStatus = false;
            selectAllShapesStatus = false;
        }

        private void selectTriangle(object sender, EventArgs e)
        {
            //method to handle mouse clicks on triangle option under Create menu
            selectTriangleStatus = true;
            selectShapeStatus = false;
            selectAllShapesStatus = false;
        }

        private void selectCircle(object sender, EventArgs e)
        {
            //method to handle mouse clicks on Circle option under Create menu
            selectCircleStatus = true;
            selectShapeStatus = false;
            selectAllShapesStatus = false;
        }

        private void selectShapeHandler(object sender, EventArgs e)
        {
            //method to handle mouse clicks on select option under Create menu
            selectShapeStatus = true;
            selectionCount = 0;
            selectShape();
        }

        private void selectAllShapes(object sender, EventArgs e)
        {
            //method to all shapes on the screen
            selectAllShapesStatus = true;
            //method to select all shapes
            Refresh();

            Graphics g = this.CreateGraphics();
            Pen blackpen = new Pen(Color.Black);

            //Declare and initialise selection pen
            Pen selectionPen = new Pen(Color.Red, 5);
            selectionPen.Alignment = PenAlignment.Center;

            foreach (Shape shape in shapes)
            {
                shape.draw(g, selectionPen);    //Redraw selected shapes with red pen
            }
        }

        private void deleteShape(object sender, EventArgs e)
        {
            //method to delete shape

            if (selectShapeStatus)
            {
                //delete only one selected shape
                selectShapeStatus = false;
                if (shapes.Count > 0) shapes.RemoveAt(selectionCount);
            }

            if(selectAllShapesStatus)
            {
                //delete all shapes
                selectAllShapesStatus = false;
                shapes.Clear();
            }

            drawShapes();
        }

        private void translateShape(object sender, EventArgs e)
        {
            //method to translate shapes

            selectShapeStatus = false;
            try
            {
                //Get angle of rotation from user
                int transX = int.Parse(Interaction.InputBox("Translate X coordinates by: ",
                    "Translation", "0"));
                int transY = int.Parse(Interaction.InputBox("Translate Y coordinates by: ",
                    "Translation", "0"));

                //translate shape with given constants
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
            catch (Exception exception)
            { }

            Refresh();
            //Redraw shapes to show translation
            drawShapes();
        }

        private void reflectShape(object sender, EventArgs e)
        {
            //method to reflect selected shape

            selectShapeStatus = false;
            try
            {
                //Get reflection mirror from user
                string mirror = Interaction.InputBox("In this application a shape can be reflected with respect to three mirrors" +
                    "given by the following lines:\n 1. the X-axis (X)\n 2. the Y-axis (Y)\n 3. the line Y=X (XY)\n 4. the line " +
                    "through the origin (O)" +
                    "\n Enter the mirror line of your choice as shown using brackets above. Make sure to use capital letters.\n",
                    "Reflection");

                //rotate shape in respect to given mirror
                int count = 0;
                int[] windowSize = {this.Width, this.Height};

                foreach (Shape shape in shapes)
                {
                    if (count == selectionCount)
                    {
                        shape.reflect(mirror, windowSize);
                        break;
                    }
                    count++;
                }
            }
            catch (Exception exception)
            { }

            Refresh();
            //Redraw shapes to show reflection
            drawShapes();
        }

        private void rotateShape(object sender, EventArgs e)
        {
            //method to rotate selected shape

            selectShapeStatus=false;
            try
            {
                //Get angle of rotation from user
                int angle = int.Parse(Interaction.InputBox("Enter angle of Rotation",
                    "Rotation", "0"));

                //rotate shape with provided angle
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
            catch (Exception exception)
            { }

            Refresh();
            //Redraw shapes to show rotation
            drawShapes();
        }

        private void scaleShape(object sender, EventArgs e)
        {
            //method to scale a selected shape

            selectShapeStatus = false;
            try
            {
                MessageBox.Show("Ensure not to use large scaling factors to avoid the shape being scaled" +
                    "beyond the screen surface");

                //Get scaling factor from user
                float scaleFactor = float.Parse(Interaction.InputBox("Enter scaling factor for selected shape",
                    "Scaling", "0"));

                //rotate shape with provided angle
                int count = 0;

                foreach (Shape shape in shapes)
                {
                    if (count == selectionCount)
                    {
                        shape.scale(scaleFactor);
                        break;
                    }
                    count++;
                }
            }
            catch (Exception exception)
            { }

            Refresh();
            //Redraw shapes to show scaling
            drawShapes();
        }

        private void selectShape()
        {
            //method to select a shape on the screen using the keyboard
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
                    shape.draw(g, selectionPen);    //Redraw selected shape with new pen
                }

                count++;
            }
        }

        private void GrafPack_Load(object sender, EventArgs e)
        {

        }

        private void GrafPack_MouseDown(object sender, MouseEventArgs e)
        {
            //Mouse down event handler method

            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;

                one = e.Location;
            }
        }

        private void GrafPack_MouseMove(object sender, MouseEventArgs e)
        {
            //Mouse move event handler method
            if (isMouseDown)
            {
                two = e.Location;

                Refresh();

                //draw other shapes currently on the screen
                drawShapes();

                //draw current view
                if (selectSquareStatus == true)
                {
                    currentView = new Square(one, two);
                    currentView.draw(this.CreateGraphics(), new Pen(Color.Black));
                }

                if (selectTriangleStatus == true)
                {
                    currentView = new Triangle(one, two);
                    currentView.draw(this.CreateGraphics(), new Pen(Color.Black));
                }

                if (selectCircleStatus == true)
                {
                    currentView = new Circle(one, two);
                    currentView.draw(this.CreateGraphics(), new Pen(Color.Black));
                }

                if (selectRectangleStatus == true)
                {
                    currentView = new Rectangle(one, two);
                    currentView.draw(this.CreateGraphics(), new Pen(Color.Black));
                }
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
                    if (selectionCount > shapes.Count) selectionCount = shapes.Count - 1;
                    selectShape();
                }

                if (e.KeyCode == Keys.Down)
                {
                    selectionCount--;
                    if (selectionCount < 0) selectionCount = 0;
                    selectShape();
                }
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

            if (selectRectangleStatus == true)
            {
                selectRectangleStatus = false;
                //add shape to shapes list
                shapes.Add(new Rectangle(one, two));
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
        protected bool creating;                     // boolean to determine whether the shape is being created for the first time 
                                                     // or being redrawn

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

        public virtual void scale(float scaleFactor)
        {
        }

        public virtual void reflect(string mirror, int[] windowSize)
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
            this.creating = true;
        }

        protected override void calculateRangesAndMidpoints()
        {
            // calculate ranges and mid points to obtain the other two vertices of a the square
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;
        }

	    public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners

            if (creating)
            {
                creating = false;
                calculateRangesAndMidpoints();

                //assign new found vertices
                newPt1.X = (int)(xMid + yDiff / 2);
                newPt1.Y = (int)(yMid - xDiff / 2);

                newPt2.X = (int)(xMid - yDiff / 2);
                newPt2.Y = (int)(yMid + xDiff / 2);
            }
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

        public override void scale(float scaleFactor)
        {
            //scale shape vertices
            keyPt.X = (int)Math.Round(keyPt.X * scaleFactor);
            keyPt.Y = (int)Math.Round(keyPt.Y * scaleFactor);

            oppPt.X = (int)Math.Round(oppPt.X * scaleFactor);
            oppPt.Y = (int)Math.Round(oppPt.Y * scaleFactor);

            newPt1.X = (int)Math.Round(newPt1.X * scaleFactor);
            newPt1.Y = (int)Math.Round(newPt1.Y * scaleFactor);

            newPt2.X = (int)Math.Round(newPt2.X * scaleFactor);
            newPt2.Y = (int)Math.Round(newPt2.Y * scaleFactor);
        }

        public override void reflect(string mirror, int[] windowSize)
        {
            Point[] points = { keyPt, oppPt, newPt1, newPt2 };
            Point[] matrix = new Point[points.Length];

            if (mirror.Equals("X"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            if (mirror.Equals("Y"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = points[i].Y;
                }
            }

            if (mirror.Equals("XY"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].Y;
                    matrix[i].Y = points[i].X;
                }
            }

            if (mirror.Equals("O"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            //Update Square vertices after rotation
            keyPt = matrix[0];
            oppPt = matrix[1];
            newPt1 = matrix[2];
            newPt2 = matrix[3];
        }
    }

    class Rectangle : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        Point keyPt, oppPt, newPt1, newPt2;      // these points identify opposite corners of the square

        public Rectangle(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
            this.creating = true;
        }

        public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners

            if (creating)
            {
                creating = false;

                // (x1,y1),(x1,y2),(x2,y2),(x2,y1)
                //assign new found vertices
                newPt1.X = keyPt.X;
                newPt1.Y = oppPt.Y;

                newPt2.X = oppPt.X;
                newPt2.Y = keyPt.Y;
            }
            // draw square
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)newPt1.X, (int)newPt1.Y);
            g.DrawLine(blackPen, (int)newPt1.X, (int)newPt1.Y, (int)oppPt.X, (int)oppPt.Y);
            g.DrawLine(blackPen, (int)oppPt.X, (int)oppPt.Y, (int)newPt2.X, (int)newPt2.Y);
            g.DrawLine(blackPen, (int)newPt2.X, (int)newPt2.Y, (int)keyPt.X, (int)keyPt.Y);
        }

        public override void rotate(int angle)
        {
            Point[] points = { keyPt, oppPt, newPt1, newPt2 };
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

        public override void scale(float scaleFactor)
        {
            //scale shape vertices
            keyPt.X = (int)Math.Round(keyPt.X * scaleFactor);
            keyPt.Y = (int)Math.Round(keyPt.Y * scaleFactor);

            oppPt.X = (int)Math.Round(oppPt.X * scaleFactor);
            oppPt.Y = (int)Math.Round(oppPt.Y * scaleFactor);

            newPt1.X = (int)Math.Round(newPt1.X * scaleFactor);
            newPt1.Y = (int)Math.Round(newPt1.Y * scaleFactor);

            newPt2.X = (int)Math.Round(newPt2.X * scaleFactor);
            newPt2.Y = (int)Math.Round(newPt2.Y * scaleFactor);
        }

        public override void reflect(string mirror, int[] windowSize)
        {
            Point[] points = { keyPt, oppPt, newPt1, newPt2 };
            Point[] matrix = new Point[points.Length];

            if (mirror.Equals("X"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            if (mirror.Equals("Y"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = points[i].Y;
                }
            }

            if (mirror.Equals("XY"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].Y;
                    matrix[i].Y = points[i].X;
                }
            }

            if (mirror.Equals("O"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            //Update Square vertices after rotation
            keyPt = matrix[0];
            oppPt = matrix[1];
            newPt1 = matrix[2];
            newPt2 = matrix[3];
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
            this.creating = true;
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

            if (creating)
            {
                creating = false;
                calculateRangesAndMidpoints();

                //assign new found vertex
                newPt.X = (int)(xMid + yDiff / 2);
                newPt.Y = (int)(yMid - xDiff / 2);
            }

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

        public override void scale(float scaleFactor)
        {
            //scale shape vertices
            keyPt.X = (int)Math.Round(keyPt.X * scaleFactor);
            keyPt.Y = (int)Math.Round(keyPt.Y * scaleFactor);

            oppPt.X = (int)Math.Round(oppPt.X * scaleFactor);
            oppPt.Y = (int)Math.Round(oppPt.Y * scaleFactor);

            newPt.X = (int)Math.Round(newPt.X * scaleFactor);
            newPt.Y = (int)Math.Round(newPt.Y * scaleFactor);
        }

        public override void reflect(string mirror, int[] windowSize)
        {
            Point[] points = { keyPt, oppPt, newPt };
            Point[] matrix = new Point[points.Length];

            if (mirror.Equals("X"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            if (mirror.Equals("Y"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = points[i].Y;
                }
            }

            if (mirror.Equals("XY"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].Y;
                    matrix[i].Y = points[i].X;
                }
            }

            if (mirror.Equals("O"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            //Update Square vertices after rotation
            keyPt = matrix[0];
            oppPt = matrix[1];
            newPt = matrix[2];
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
            this.creating = true;
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
            //regardless the code written below is just for comparison purposes
            Point[] points = { keyPt, oppPt };
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
        }

        public override void translate(int transX, int transY)
        {
            //translate shape vertices
            keyPt.X += transX;
            keyPt.Y += transY;

            oppPt.X += transX;
            oppPt.Y += transY;
        }

        public override void scale(float scaleFactor)
        {
            //scale shape vertices
            keyPt.X = (int)Math.Round(keyPt.X * scaleFactor);
            keyPt.Y = (int)Math.Round(keyPt.Y * scaleFactor);

            oppPt.X = (int)Math.Round(oppPt.X * scaleFactor);
            oppPt.Y = (int)Math.Round(oppPt.Y * scaleFactor);
        }

        public override void reflect(string mirror, int[] windowSize)
        {
            Point[] points = { keyPt, oppPt };
            Point[] matrix = new Point[points.Length];

            if (mirror.Equals("X"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            if (mirror.Equals("Y"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = points[i].Y;
                }
            }

            if (mirror.Equals("XY"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = points[i].Y;
                    matrix[i].Y = points[i].X;
                }
            }

            if (mirror.Equals("O"))
            {
                for (int i = 0; i < points.Length; i++)
                {
                    matrix[i].X = windowSize[0] - points[i].X;
                    matrix[i].Y = windowSize[1] - points[i].Y;
                }
            }

            //Update Square vertices after rotation
            keyPt = matrix[0];
            oppPt = matrix[1];
        }
    }
}


