using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace LunarLander
{

    /*
     * 
     * Gary Mejia and Ji Hwan Park
     * CIS 3309
     * Date Due: 4/5/2020
     * 
     * Program Identification
     * LunarLander-Module Class
     * 
        This class is responsible for handling
        the events associated with
        the ship(module) itself. It will factor in the gravity,
        the max speed in which the ship may land not crash,
        fuel, and more.

        In addition, it implements equations, getting, and setting points
        of the rectangle where the module is contained inside.
        This will make sure the landing, turning, and thrusting of the
        ship is working properly.        
    */

    class Module
    {
        //Constructor
        public Module()
        {
            //sets the starting fuel and coordinates
            Fuel = 300;
            X = 800;        
            Y = 200;
        }

        public const float MaxLandingSpeed = 15;                    //Max speed the module can be at when landing
        const float heightsize = 25;                                //Height of the sprites container
        const float widthsize = 20;                                 //Width of the sprites container
        double Gravity = 6;                                         //Gravity or speed that the module will be continuously be falling at
        double ThrustSpeed = 30;                                    //speed of the module
        const double RotationBurnedFuel = 0.5;                      //amount of fuel being burned when module is rotated

        //Getters and setters for game and sprite
        public Bitmap Sprite { get; set; }
        public Game Game { get; set; }

        //getter and setter for the speed the module moves on x plane
        public double sX { get; set; }
        //getter and setter for the speed the module moves on y plane
        public double sY { get; set; }
        //getter setter for JUST the location of x plane itself
        public double X { get; set; }

        //getter setter for JUST the location of x plane itself
        public double Y { get; set; }

        //getter setter for the fuel
        public double Fuel { get; set; }


        //getter setter for the angle of the ship in rads
        public double Rotation { get; set; }


        //returns a point with the coordiantes of the lander 
        public PointF Location
        {
            get
            {
                return new PointF((float)X, (float)Y);
            }
        }

        // Location of "top left" point of sprite
        //Constantly updated as the x and y coordinate changes
        public PointF LocationGraphics
        {
            get
            {
                return new PointF((float)X - widthsize / 2, (float)Y - heightsize / 2);
            }
        }

        //The rectangle encapsulates the sprite. Using the rectangle we are able to detect 
        //when the module intersects the terrain
        public RectangleF Rectangle
        {
            get
            {
                return new RectangleF((float)X - widthsize / 2, (float)Y - heightsize / 2, widthsize, heightsize);
            }
        }

        //getter setter for the angle of the ship in degrees
        public double RotationDegrees
        {
            get { return Rotation / Math.PI * 180; }
        }

        //moves the module 
        //takes into account the gravity
        public void Update(TimeSpan ts)
        {
            //gravity
            sY += Gravity * ts.TotalSeconds;

            //thrust (only when up, right, and left pressed down)
            if (Game.IsKeyDown(GameKeys.Up))
                Thrust(ts.TotalSeconds);
            if (Game.IsKeyDown(GameKeys.Right))
                RotateLeft(ts.TotalSeconds);
            if (Game.IsKeyDown(GameKeys.Left))
                RotateRight(ts.TotalSeconds);

            //calculates new location
            X += sX * ts.TotalSeconds;
            Y += sY * ts.TotalSeconds;

        }


        //this applies the rotation of the rectangle to the left
        //when the "left" arrow is pressed down
        private void RotateLeft(double totalSeconds)
        {
            if (Fuel < 0)
                return;
            Rotation -= 3 * totalSeconds;
            //decrease fuel
            Fuel -= RotationBurnedFuel;
        }

        //this applies the rotation of the rectangle to the right
        //when the "right" arrow is pressed down
        private void RotateRight(double totalSeconds)
        {
            if (Fuel < 0)
                return;

            Rotation += 3 * totalSeconds;
            //decrease fuel
            Fuel -= RotationBurnedFuel;
        }


        //This method handles the thrust of the module (when the player pressed the up key)
        private void Thrust(double totalSeconds)
        {
            //checks tank
            if (Fuel < 0)
                return;
            //lander rotation 
            //add thrust
            sY -= Math.Sin(ConvertAngle.NormalizeAngle(Rotation + Math.PI / 2)) * totalSeconds * ThrustSpeed;
            sX -= Math.Cos(ConvertAngle.NormalizeAngle(Rotation + Math.PI / 2)) * totalSeconds * ThrustSpeed;
            //decrease fuel as thrust is active (key is being pressed)
            Fuel--;
        }
        //Used with game class to check if the module intersects with the terrain
        //get all 4 points of our bounding rectangle and rotate them around its center
        public bool IntersectsWithLine(PointF a, PointF b)
        {
            //top left
            if (TestPointUnderLine(a, b, new PointF(Rectangle.Left, Rectangle.Top)))
                return true;

            //top right
            if (TestPointUnderLine(a, b, new PointF(Rectangle.Right, Rectangle.Top)))
                return true;

            //bottom left
            if (TestPointUnderLine(a, b, new PointF(Rectangle.Left, Rectangle.Bottom)))
                return true;

            //bottom right
            if (TestPointUnderLine(a, b, new PointF(Rectangle.Right, Rectangle.Bottom)))
                return true;


            return false; //return false otherwise
        }

        //Check if point is under a line
        private bool TestPointUnderLine(PointF left, PointF right, PointF point)
        {
            //skip if point is not in x-range of our line
            if (point.X < left.X || point.X > right.X)
                return false;

            //we want to get coords of each point on line by using y=a*x+b

            //a is increment for each X
            float a = (right.Y - left.Y) / (right.X - left.X);
            //b is the starting y
            float b = left.Y;

            //we make test as if point started at x=0 so we shift its X location relative to start of the line
            //and store it in newX
            float newX = point.X - left.X;

            //if point is "under" the line, we 
            return (point.Y > (a * newX + b));
        }
        //depending if the degree rotation is greater than 350 degrees, or less than 10 degrees return true or false
        public bool IsRotatedForLanding
        {
            get
            {
                return ((RotationDegrees > 350) || (RotationDegrees < 10));
            }
        }
    }
}