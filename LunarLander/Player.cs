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
     * LunarLander-Player Class
     * 
     * This class is in charge of holding the players information, their score. The class can either retrieve or increase their score. 
     * It also resets the score if they didnt land the moduule
     */
    class Player
    {
        private  int score;
        //Constructor
        public Player()
        {
            score = 0;
        }
        //Increase score
        public void landed()
        {
            score += 100;
        }
        //Retrieve score
        public int getScore()
        {
            return score;
        }
        //Reset score
        public void reset()
        {
            score = 0;
        }
    }
}
