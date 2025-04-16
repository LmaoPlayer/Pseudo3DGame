using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    internal class Player
    {
        double x;
        double y;
        float angle;

        Settings setting;

        public Player(Settings game_settings) 
        {
            this.x = game_settings.PLAYER_START.X;
            this.y = game_settings.PLAYER_START.Y;
            this.angle = game_settings.PLAYER_ANGLE;
            setting = game_settings;
        }

        public PointF GetLoc()
        {
            return new PointF((float)x, (float)y);
        }

        public Point GetMapLoc()
        {
            return new Point((int)x/100, (int)y/100);
        }

        public float GetAngle()
        {
            return angle;
        }

        public void Left()
        {
            x += setting.PLAYER_SPEED * Math.Sin(angle);
            y -= setting.PLAYER_SPEED * Math.Cos(angle);
        }

        public void Right()
        {
            x -= setting.PLAYER_SPEED * Math.Sin(angle);
            y += setting.PLAYER_SPEED * Math.Cos(angle);
        }

        public void Forward()
        {
            x += setting.PLAYER_SPEED * Math.Cos(angle);
            y += setting.PLAYER_SPEED * Math.Sin(angle);
        }

        public void Back()
        {
            x -= setting.PLAYER_SPEED * Math.Cos(angle);
            y -= setting.PLAYER_SPEED * Math.Sin(angle);
        }

        public void TurnRight()
        {
            angle += setting.PLAYER_TURNING_SPEED;
        }

        public void TurnLeft()
        {
            angle -= setting.PLAYER_TURNING_SPEED;
        }
    }
}
