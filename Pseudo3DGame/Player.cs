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
        //Delta time is nodig zodat de movement onafhankelijk is van de fps, en dus ook een afstand berekend wordt sinds de laatste frame.
        int delta_time;

        Settings setting;

        public Player(Settings game_settings) 
        {
            this.x = game_settings.PLAYER_START.X;
            this.y = game_settings.PLAYER_START.Y;
            this.angle = game_settings.PLAYER_ANGLE;
            setting = game_settings;

            this.delta_time = 1;
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
            x += setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
            y -= setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
        }

        public void Right()
        {
            x -= setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
            y += setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
        }

        public void Forward()
        {
            x += setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
            y += setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
        }

        public void Back()
        {
            x -= setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
            y -= setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
        }

        public void TurnRight()
        {
            angle += setting.PLAYER_TURNING_SPEED * delta_time;
        }

        public void TurnLeft()
        {
            angle -= setting.PLAYER_TURNING_SPEED * delta_time;
        }

        public void ResetDT()
        {
            this.delta_time = 1;
        }

        public void IncreaseDT()
        {
            this.delta_time += 1;
        }
    }
}
