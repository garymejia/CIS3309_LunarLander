using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

/*
 * Gary Mejia and Ji Hwan Park
 * CIS 3309
 * Date Due: 4/5/2020
 * 
 * Program Identification
 * LunarLander-LunarLander Class
 * 
 * This class is responsible for processing the event listeners, updating the screen using the draw method
 * from the game class and controlling the players score based off their progress
 */

namespace LunarLander
{
    public partial class LunarLander : Form
    {
        public LunarLander()
        {
            InitializeComponent();
            //Set the screen in the middle
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(150, 100);
        }


        static Player player = new Player();                            //Creates a new player
        Game game = new Game(player);                                   //Creates an instance of the game 

        /*
         * KeyDown has to be registered in a hash set so that multiple keys can be read at once
         */
        private void keyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:                                       //Changes the players score depening on if they landed or crashed
                    if (game.Reason == Game.PauseReason.Landed)
                    {
                        player.landed();
                    }
                    else
                    {
                        player.reset();
                    }
                    game = new Game(player);                            //new Game
                    picInstructions.Enabled = true;                     //Enables user to click on the instructions icon
                    break;
                case Keys.Space:
                    if (game.Paused == true && game.Reason == Game.PauseReason.Paused)
                    {
                        game.Paused = false;
                    }
                    else if(game.Paused == false)
                    {
                        game.Paused = true;
                    }
                    break;
                case Keys.Back:
                    this.Close();                                       //Closes program if they click backspace key
                    break;
                default:
                    game.KeyDown(e);                                    //Registers any other key to hash set in game class
                    break;
            }
        }

        /*
         * The key up method will remove the keys from the hash set
         */
        private void keyUp(object sender, KeyEventArgs e)
        {
            game.KeyUp(e);
        }

        /*
        * Executes the timer class on intervals of 50
        * This update will update the sprite and current status of the game (Pause or not paused)
        */
        private void timer(object sender, EventArgs e)
        {
            game.Update(new TimeSpan(updateTimer.Interval * 10000));             //updates the sprites location and rotation, current status of the game
            gameScreen.Image = game.Draw();                                      //Draws picturebox screen with the games current information
                
            //Prevents user from clicking instructions when they crash. If they did it would allow them to continue the game 
            if (game.Reason == Game.PauseReason.Crashed)
            {
                picInstructions.Enabled = false;
            }
        }

        /*
         * Calls the instructions form and pauses the game
         * When the form closes the game is unpaused
         */
        private void instructions_Click(object sender, EventArgs e)
        {   
            game.Reason = Game.PauseReason.Paused;
            game.Paused = true;

            Instructions instructions = new Instructions();
            instructions.ShowDialog();
            game.Paused = false;
        }
    }
}
