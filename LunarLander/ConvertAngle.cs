using System;
using System.Collections.Generic;
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
     * LunarLander-ConvertAngle Class 
     * 
     * Used to rotate the rectangle
     *  
     */
    class ConvertAngle
    {
        // Normalizes the angle
        public static double NormalizeAngle(double angle)
        {
            while (angle < 0)
                angle += 2 * Math.PI;
            while (angle > 2 * Math.PI)
                angle -= 2 * Math.PI;
            return angle;
        }

        /// Converts angle from radians to degrees.
        public static double RadiansToDegrees(double angle)
        {
            return angle / Math.PI * 180;
        }

    }
}