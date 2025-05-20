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
        public int MAP_WIDTH { get; private set; }
        public int MAP_HEIGHT { get; private set; }


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
        public float PLAYER_SPEED { get; private set; }
        public float MINIMUM_WALL_PLAYER_DISTANCE { get; private set; }
        public float PLAYER_TURNING_SPEED { get; }
        public int PLAYER_MAP_SCALE { get; private set; }

        public double FOV { get; }
        public double HALF_FOV { get; }
        public int NUM_RAYS { get; }
        public double HALF_NUM_RAYS { get; }
        public double DELTA_ANGLE { get; }
        public double MAX_DEPTH { get; }

        public double DIV_BY_ZERO_ERROR { get; }

        public double SCREEN_DIST { get; }
        public double WALL_SCALE { get; }
        
        public int TEXTURE_SIZE { get; }

        public Settings(int width, int height)
        {
            this.FPS = 60;

            this.WIDTH = width;
            this.HEIGHT = height;

            int tempWidth = 40;
            ChangedMap(tempWidth, tempWidth * HEIGHT / WIDTH);
            //this.MAP_WIDTH = 14;

            //this.PLAYER_MAP_SCALE = WIDTH / MAP_WIDTH;
            //this.MAP_HEIGHT = MAP_WIDTH*HEIGHT/WIDTH;

            this.Selected = KeyBoards.ZQSD;


            this.PLAYER_START = new PointF(1.5F * PLAYER_MAP_SCALE, 1.5F * PLAYER_MAP_SCALE);
            this.PLAYER_ANGLE = 45;
            //this.PLAYER_SPEED = (100/MAP_HEIGHT)*10;
            this.PLAYER_TURNING_SPEED = 1F;
            

            this.FOV = Math.PI/3;
            this.HALF_FOV = FOV/2;
            this.NUM_RAYS = WIDTH / 2;
            //this.NUM_RAYS = 50;
            this.HALF_NUM_RAYS = NUM_RAYS/2;
            this.DELTA_ANGLE = FOV/NUM_RAYS;
            this.MAX_DEPTH = 100;

            this.DIV_BY_ZERO_ERROR = 0.000001F;

            this.SCREEN_DIST = (WIDTH/2) / (Math.Tan(HALF_FOV));
            this.WALL_SCALE = WIDTH / NUM_RAYS;

            this.TEXTURE_SIZE = 256;
            //this.MINIMUM_WALL_PLAYER_DISTANCE = PLAYER_MAP_SCALE / 6;
        }


        public void ChangedMap(int newWidth, int newHeight)
        {
            MAP_WIDTH = newWidth;
            MAP_HEIGHT = newHeight;


            int tempAddition = (int)Math.Ceiling((decimal)Math.Max(MAP_WIDTH, MAP_HEIGHT)/Math.Min(MAP_HEIGHT, MAP_WIDTH))*2;
            PLAYER_MAP_SCALE = Math.Min(WIDTH / (MAP_WIDTH+tempAddition), HEIGHT/(MAP_HEIGHT+tempAddition));
            PLAYER_SPEED = (100 / Math.Min(MAP_HEIGHT, MAP_WIDTH))*10;
            MINIMUM_WALL_PLAYER_DISTANCE = PLAYER_MAP_SCALE/6;
        }
    }
}
