using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class Player
    {
        double x;
        double y;
        double hor_angle;
        double vert_angle;
        //Delta time is nodig zodat de movement onafhankelijk is van de fps, en dus ook een afstand berekend wordt sinds de laatste frame.
        double delta_time;
        Map map;

        Settings setting;
        Timer CreateStart = new Timer();
        public bool CanStartDrawing { get; private set; }

        public Player(Settings game_settings, Map game_map)
        {
            CanStartDrawing = false;

            this.setting = game_settings;
            this.map = game_map;

            this.hor_angle = game_settings.PLAYER_ANGLE;
            this.delta_time = 1;
            this.vert_angle = 0;

            CreateStart.Tick += (sender, e) => CreateStartPos();
            CreateStart.Interval = 500;
            CreateStart.Start();
        }
        public void CreateStartPos()
        {
            if (map.IsFinished)
            {
                for (int i = 0; i < map.map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.map.GetLength(1); j++)
                    {
                        if (map.map[i, j] == 0)
                        {
                            x = j * setting.PLAYER_MAP_SCALE + setting.PLAYER_MAP_SCALE/2;
                            y = i * setting.PLAYER_MAP_SCALE + setting.PLAYER_MAP_SCALE/2;
                            CreateStart.Stop();
                            CanStartDrawing = true;
                            return;
                        }
                    }
                }

            }
        }

        public PointF GetLoc()
        {
            return new PointF((float)x, (float)y);
        }

        public double GetAngle()
        {
            return hor_angle;
        }

        public double GetVertAngle()
        {
            return vert_angle;
        }

        public double GetHorAngle()
        {
            return hor_angle;
        }

        public void Left()
        {
            double tx = setting.PLAYER_SPEED * Math.Sin(hor_angle) * delta_time;
            double ty = -setting.PLAYER_SPEED * Math.Cos(hor_angle) * delta_time;
            CheckWallCollision(tx, ty);
        }

        public void Right()
        {
            double tx = -setting.PLAYER_SPEED * Math.Sin(hor_angle) * delta_time;
            double ty = setting.PLAYER_SPEED * Math.Cos(hor_angle) * delta_time;
            CheckWallCollision(tx, ty);
        }

        public void Forward(bool sprint)
        {
            int speed_multiplier = 1;
            if (sprint) speed_multiplier = 4;

            double tx = setting.PLAYER_SPEED * Math.Cos(hor_angle) * delta_time * speed_multiplier;
            double ty = setting.PLAYER_SPEED * Math.Sin(hor_angle) * delta_time * speed_multiplier;
            CheckWallCollision(tx, ty);
        }

        public void Back()
        {
            double tx = -setting.PLAYER_SPEED * Math.Cos(hor_angle) * delta_time;
            double ty = -setting.PLAYER_SPEED * Math.Sin(hor_angle) * delta_time;
            CheckWallCollision(tx, ty);
        }

        public void TurnRight()
        {
            hor_angle += setting.PLAYER_TURNING_SPEED * delta_time;
            hor_angle %= 360;
        }

        public void RotateLR(float MouseDist)
        {
            hor_angle -= (setting.PLAYER_TURNING_SPEED * (MouseDist / 500)) % 360;
        }

        public void RotateUD(float MouseDist)
        {
            vert_angle += (setting.PLAYER_TURNING_SPEED * MouseDist);
            if (vert_angle > 9 * setting.HEIGHT) vert_angle = 9 * setting.HEIGHT;
            if (vert_angle < -9 * setting.HEIGHT) vert_angle = -9 * setting.HEIGHT;
        }

        public void TurnLeft()
        {
            hor_angle -= (setting.PLAYER_TURNING_SPEED * delta_time) % 360;
        }

        public void UpdateDT(double deltaT)
        {
            this.delta_time = deltaT;
        }

        public void CheckWallCollision(double new_x, double new_y)
        {
            if (new_y > 0)
            {
                if (map.map[(int)Math.Floor((y + new_y + setting.MINIMUM_WALL_PLAYER_DISTANCE) / setting.PLAYER_MAP_SCALE), (int)Math.Floor((x) / setting.PLAYER_MAP_SCALE)] == 0)
                {
                    y += new_y;
                }
            }
            else
            {
                if (map.map[(int)Math.Floor((y + new_y - setting.MINIMUM_WALL_PLAYER_DISTANCE) / setting.PLAYER_MAP_SCALE), (int)Math.Floor((x) / setting.PLAYER_MAP_SCALE)] == 0)
                {
                    y += new_y;
                }
            }

            if (new_x > 0)
            {
                if (map.map[(int)Math.Floor((y) / setting.PLAYER_MAP_SCALE), (int)Math.Floor((x + new_x + setting.MINIMUM_WALL_PLAYER_DISTANCE) / setting.PLAYER_MAP_SCALE)] == 0)
                {
                    x += new_x;
                }
            }
            else
            {
                if (map.map[(int)Math.Floor((y) / setting.PLAYER_MAP_SCALE), (int)Math.Floor((x + new_x - setting.MINIMUM_WALL_PLAYER_DISTANCE) / setting.PLAYER_MAP_SCALE)] == 0)
                {
                    x += new_x;
                }
            }
        }
    }
}
