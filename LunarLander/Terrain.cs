using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarLander
{
    /*
    * 
    * Gary Mejia and Ji Hwan Park
    * CIS 3309
    * Date Due: 4/5/2020
    * 
    * Program Identification
    * LunarLander-Terrain Class
    * 

    * This class is responsible for creating the points of a random terrain. The class holds a list of points which will be used in the
    * game class. The points will be connected to create the terrain
    */
    class Terrain
    {
        public int numOfPoints = 0;                     //Number of points
        public int TotalHeight = 100;                   //Max height of the terrain
        public int Width = 1700;                        //width that the terrain will span
        const int landingStripWidth = 30;               //size of the landing strips
        public List<PointF> points;                     //List holding all of the terrains points

        //Constructor
        public Terrain()
        {
            points = new List<PointF>();
        }

        
        //This method generates a unique random set of points for the terrain. 
        //The terrain replicates a sine wave with periods of random wavelengths and amplitudes
        public void GenerateTerrain()
        {
            Random rnd = new Random();

            double amp = 0.2;                                               //amplitude of wave
            double waveLength = rnd.NextDouble() * (.03 - .01) + 0.01;      //waveLength of wave

            //Previous period and current period are used to check different waves
            int previousPeriod = 0;
            int currentPeriod = 0;
            float tempHeight = 0;

            //our terrain is left list of connected dots in random height < 1/2 of screen
            for (int w = 0; w < Width; w += rnd.Next(1, 40))        //randomly chooses the x coordinate based on its range and as long as its under the width
            {
                previousPeriod = (Width - w) / 200;
                //new sizes for new wave
                if (currentPeriod != previousPeriod)
                {
                    //unique random amplitude 
                    amp = rnd.NextDouble() * (1 - 0.1) + .1; ;
                }
                currentPeriod = previousPeriod;
                //apply new numbers to the sine wave
                tempHeight = (Convert.ToSingle((amp * Math.Sin(waveLength * w) + 1) * 200)) + 350;
                points.Add(new PointF(w, tempHeight));
            }
            
            numOfPoints = points.Count();                           //Number of points in list
            int landingsCount = 4;                                  //Number of landing strips
            for (int i = 0; i < landingsCount; i++)
            {
                int idx = rnd.Next(numOfPoints);
                try
                {                                                   //due to the points being randomized, the space between each pair of points might not be enough for landing strips
                    while ((points[idx + 1].X - points[idx].X) < 30)         
                    {
                        idx = rnd.Next(numOfPoints);
                    }
                }
                catch                   
                {
                    break;
                }
                points.Insert(idx + 1, (new PointF(points[idx].X + landingStripWidth, points[idx].Y)));     //Insert landing strips
            }
            points.Add(new PointF(Width, rnd.Next(TotalHeight)));
        }
    }
}