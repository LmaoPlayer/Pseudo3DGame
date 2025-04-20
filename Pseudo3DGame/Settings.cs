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
        public int PLAYER_MAP_SCALE { get; }

        public double FOV { get; }
        public double HALF_FOV { get; }
        public int NUM_RAYS { get; }
        public double HALF_NUM_RAYS { get; }
        public double DELTA_ANGLE { get; }
        public double MAX_DEPTH { get; }

        public double DIV_BY_ZERO_ERROR { get; }

        public double SCREEN_DIST { get; }
        public double WALL_SCALE { get; }


        public Settings()
        {
            this.FPS = 30;

            this.WIDTH = 1600;
            this.HEIGHT = 900;

            this.Selected = KeyBoards.ZQSD;

            this.PLAYER_START = new PointF(150, 500);
            this.PLAYER_ANGLE = 45;
            this.PLAYER_SPEED = 75F;
            this.PLAYER_TURNING_SPEED = 3F;
            this.PLAYER_MAP_SCALE = 100;

            this.FOV = Math.PI/3;
            this.HALF_FOV = FOV/2;
            //this.NUM_RAYS = WIDTH / 2;
            this.NUM_RAYS = 100;
            this.HALF_NUM_RAYS = NUM_RAYS/2;
            this.DELTA_ANGLE = FOV/NUM_RAYS;
            this.MAX_DEPTH = 20;

            this.DIV_BY_ZERO_ERROR = 0.000001F;

            this.SCREEN_DIST = (WIDTH / 2) / (Math.Tan(HALF_FOV));
            this.WALL_SCALE = WIDTH / NUM_RAYS;

        }
    }
}
