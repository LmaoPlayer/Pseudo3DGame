using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    internal class Settings
    {
        //FPS = Frames Per Second
        public int FPS { get; }

        //Resolution: 
        public int WIDTH { get; }
        public int HEIGHT { get; }

        //Om met verschillende keyboards te spelen/testen
        public enum KeyBoards
        {
            WASD,
            ZQSD
        }
        public KeyBoards Selected { get; }

        //Speler start positie
        public PointF PLAYER_START { get; }
        public int PLAYER_ANGLE { get; }
        public float PLAYER_SPEED { get; }
        public float PLAYER_TURNING_SPEED { get; }


        public Settings()
        {
            this.FPS = 120;

            this.WIDTH = 1600;
            this.HEIGHT = 900;

            this.Selected = KeyBoards.ZQSD;

            this.PLAYER_START = new PointF(100F, 500);
            this.PLAYER_ANGLE = 45;
            this.PLAYER_SPEED = 2F;
            this.PLAYER_TURNING_SPEED = 0.1F;
        }
    }
}
