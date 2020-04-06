using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

/*
 * Gary Mejia and Ji Hwan Park
 * CIS 3309
 * Date Due: 4/5/2020
 * 
 * Program Identification
 * LunarLander-Game Class
 * 
 * This class processes the games information such as the module and terrain. It also holds the hashset which is used to register the players movement. 
 * This class holds the 
 */
namespace LunarLander
{
    class Game
    {
        public PauseReason Reason;
        Font statsFont;
        Bitmap bmp;
        Pen whitePen;
        Pen landingPen;
        HashSet<GameKeys> keys;
        private static String imageName = "lander";
        public Bitmap sprite = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageName);           //Loads sprite from resources
        Terrain terrain = new Terrain();                                                                    //Creates new terrain
        Player newPlayer;

        public Game(Player newPlayer)
        {
            bmp = new Bitmap(1600, 1000);                                               //Sets the size to x:1600   y:1000                                    
            statsFont = new Font(FontFamily.GenericMonospace, 25, GraphicsUnit.Pixel);  //Fonts used for displaying screen text
            terrain = new Terrain();                                                    //Creates a terrain instance
            terrain.TotalHeight = 1600;                                                 //sets the height of the terrain
            terrain.GenerateTerrain();                                                  //Creates the points used to map the terrain
            Module = new Module();                                                      //New module(Lander) instance
            Module.Game = this;                                                         //Passes the current instance of the game to the module. Used to access hashset                                                         
            Paused = false;                                                             //Current state of the game. Paused or not Paused
            keys = new HashSet<GameKeys>();                                             //Gamekeys hashset
            whitePen = Pens.White;                                                      //color for lines connecting terrains points
            landingPen = new Pen(Brushes.White, 3);                                     //Landing strip lines
            this.newPlayer = newPlayer;                                                 //Saves player instance passed from the lunar lander class
            Module.Sprite = sprite;                                                     //Passes the bitmap of the sprite to the module class
        }
        
        //Getters and setters for Module and Paused
        public Module Module { get; set; }
        public bool Paused { get; set; }

        /*
         * Called from lunarlander class to add key from hashset
         */
        public void KeyDown(KeyEventArgs e)
        {
            keys.Add(GetKeyFromKeyEventArgs(e));                    //Registers a valid key to hashset
        }
        /*
         * Called from lunarlander class to remove key from hashset
         */
        public void KeyUp(KeyEventArgs e)
        {
            keys.Remove(GetKeyFromKeyEventArgs(e));                 //removes a valid key from hashset
        }


        /*
         * Checks to see if key is valid. If key isn't valid it is categorized
         * as unkown
         */
        private GameKeys GetKeyFromKeyEventArgs(KeyEventArgs e)
        {
            GameKeys key;
            switch (e.KeyData)
            {
                case Keys.Up:
                    key = GameKeys.Up;
                    break;
                case Keys.Left:
                    key = GameKeys.Left;
                    break;
                case Keys.Right:
                    key = GameKeys.Right;
                    break;
                default:
                    key = GameKeys.Unknown;
                    break;
            }
            return key;
        }

        //Gives the module class access to the hashset to see which keys are being pressed
        public bool IsKeyDown(GameKeys key)
        {
            return keys.Contains(key);
        }
        
        //Updates the modules information as well as checks to see if it crashed or landed
        public void Update(TimeSpan ts)
        {
            //update all game objects
            if (!Paused)
                Module.Update(ts);
            //check for collisions
            for (int i = 0; i < terrain.points.Count - 1; i++)
            {
                //check for intersection with terrain
                if (Module.IntersectsWithLine(terrain.points[i], terrain.points[i + 1]))
                {
                    //check if we crashed 
                    Paused = true;
                    Reason = PauseReason.Crashed;

                    //if it's exactly above the terrain then it landed
                    if (terrain.points[i].Y == terrain.points[i + 1].Y)
                        if ((Module.sY < Module.MaxLandingSpeed) && (Module.IsRotatedForLanding))
                            Reason = PauseReason.Landed;
                }
            }
        }
        /*
         * Draws the screen. This includes drawing the sprite based on location, the terrain, and the game information to the screen
         */
        public Bitmap Draw()
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {

                g.Clear(Color.Black);
                
                //Draw the ground by drawing a line through all the points in the terrain
                for (int i = 0; i < terrain.points.Count - 1; i++)
                    //Check to see 
                    g.DrawLine((terrain.points[i].Y == terrain.points[i + 1].Y) ? landingPen : whitePen, terrain.points[i], terrain.points[i + 1]);



                //transform graphics object by the rotation
                Matrix m = new Matrix();
                m.RotateAt((float)ConvertAngle.RadiansToDegrees(Module.Rotation), Module.Location);
                g.Transform = m;
                g.DrawImage(Module.Sprite, Module.LocationGraphics);
                //restore transformation
                m.Reset();
                g.Transform = m;

                
                //draw stats
                const int posStatX = 0;
                g.DrawString(DateTime.Now.ToLongTimeString(), statsFont, Brushes.White, (bmp.Width) / 2, 0);
                g.DrawString("Fuel: " + Module.Fuel, statsFont, Brushes.White, posStatX, 0);
                g.DrawString(string.Format("Lander: x={0:0.00} y={1:0.00}", Module.X, Module.Y, ConvertAngle.RadiansToDegrees(Module.Rotation)), statsFont, Brushes.White, posStatX, 30);
                g.DrawString(string.Format("Speed: x={0:0.00} y={1:0.00}", Module.sX, Module.sY, 0), statsFont, Brushes.White, posStatX, 60);
                g.DrawString("Score:   " + newPlayer.getScore().ToString(), statsFont, Brushes.White, 1300, 10);
                //display reason if paused
                if (Paused)
                    g.DrawString(Reason.ToString(), new Font(statsFont, FontStyle.Bold), Brushes.White, (bmp.Width) / 2, (bmp.Height) / 2 - 200);
            }
            return bmp;
        }
        public enum PauseReason
        {
            Paused, Crashed, Landed
        }
    }
}