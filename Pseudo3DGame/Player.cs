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
        double angle;
        //Delta time is nodig zodat de movement onafhankelijk is van de fps, en dus ook een afstand berekend wordt sinds de laatste frame.
        double delta_time;
        Map map;

        Settings setting;

        public Player(Settings game_settings, Map game_map) 
        {
            this.x = game_settings.PLAYER_START.X;
            this.y = game_settings.PLAYER_START.Y;
            this.angle = game_settings.PLAYER_ANGLE;
            this.setting = game_settings;
            this.map = game_map;

            this.delta_time = 1;
        }

        public PointF GetLoc()
        {
            return new PointF((float)x, (float)y);
        }

        public double GetAngle()
        {
            return angle;
        }

        public void Left()
        {
            double tx = setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
            double ty = -setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
            CheckWallCollision(tx, ty);
        }

        public void Right()
        {
            double tx = -setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
            double ty = setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
            CheckWallCollision(tx, ty);
        }

        public void Forward(bool sprint)
        {
            int speed_multiplier = 1;
            if (sprint) speed_multiplier = 4;

            double tx = setting.PLAYER_SPEED * Math.Cos(angle) * delta_time * speed_multiplier;
            double ty = setting.PLAYER_SPEED * Math.Sin(angle) * delta_time * speed_multiplier;
            CheckWallCollision(tx, ty);
        }

        public void Back()
        {
            double tx = -setting.PLAYER_SPEED * Math.Cos(angle) * delta_time;
            double ty = -setting.PLAYER_SPEED * Math.Sin(angle) * delta_time;
            CheckWallCollision(tx, ty);
        }

        public void TurnRight()
        {
            angle += setting.PLAYER_TURNING_SPEED * delta_time;
            angle %= 360;
        }

        public void TurnLeft()
        {
            angle -= (setting.PLAYER_TURNING_SPEED * delta_time) % 360;
        }

        public void UpdateDT(double deltaT)
        {
            this.delta_time = deltaT;
        }

        public void CheckWallCollision(double new_x, double new_y)
        {
            if (map.map[(int)Math.Floor((y + new_y) / setting.PLAYER_MAP_SCALE_Y), (int)Math.Floor((x) / setting.PLAYER_MAP_SCALE_X)] == 0)
            {
                y += new_y;
            }
            if (map.map[(int)Math.Floor((y) / setting.PLAYER_MAP_SCALE_Y), (int)Math.Floor((x + new_x) / setting.PLAYER_MAP_SCALE_X)] == 0)
            {
                x += new_x;
            }
        }
    }
}
