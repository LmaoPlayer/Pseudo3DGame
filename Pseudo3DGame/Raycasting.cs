using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    internal class Raycasting
    {
        Settings setting;
        double player_angle;
        PointF player_pos;
        Point map_pos;
        Map map;
        double depth;
        double cos;
        double sin;

        public Raycasting(Settings setting, Map map) 
        {
            this.setting = setting;
            this.player_angle = setting.PLAYER_ANGLE;
            this.player_pos = setting.PLAYER_START;
            this.map_pos = new Point((int)Math.Floor(setting.PLAYER_START.X / setting.PLAYER_MAP_SCALE), (int)Math.Floor(setting.PLAYER_START.Y / setting.PLAYER_MAP_SCALE));
            this.map = map;
        }
        private PointF RayCast(double ray_angle)
        {
            sin = Math.Sin(ray_angle);
            cos = Math.Cos(ray_angle);

            //horizontal raycast
            double y_hor;
            double new_y;
            if (sin > 0)
            {
                y_hor = map_pos.Y + 1;
                new_y = 1;
            }
            else
            {
                y_hor = map_pos.Y - 0.00001F;
                new_y = -1;
            }

            double depth_hor = (y_hor - (player_pos.Y/setting.PLAYER_MAP_SCALE)) / sin;
            double x_hor = (player_pos.X/setting.PLAYER_MAP_SCALE) + (depth_hor * cos);

            double new_depth = new_y / sin;
            double new_x = new_depth * cos;

            for (int i = 0; i < setting.MAX_DEPTH; i++)
            {
                Point tile_hor = new Point((int)x_hor, (int)y_hor);
                if (tile_hor.X >= 0 && tile_hor.X < map.map.GetLength(1) && tile_hor.Y >= 0 && tile_hor.Y < map.map.GetLength(0))
                {
                    if (map.map[tile_hor.Y, tile_hor.X] == 1)
                    {
                        break;
                    }
                }
                else { break; }

                x_hor += new_x;
                y_hor += new_y;
                depth_hor += new_depth;
            }

            //vertical raycast
            double x_vert;
            double v_new_x = 0;
            if (cos > 0)
            {
                x_vert = map_pos.X + 1;
                v_new_x = 1;
            }
            else
            {
                x_vert = map_pos.X - 0.000001F;
                v_new_x = -1;
            }

            double depth_vert = (x_vert - (player_pos.X / setting.PLAYER_MAP_SCALE)) / cos;
            double y_vert = (player_pos.Y/setting.PLAYER_MAP_SCALE) + (depth_vert * sin);

            double v_new_depth = v_new_x / cos;
            double v_new_y = v_new_depth * sin;

            for (int i = 0; i < setting.MAX_DEPTH; i++)
            {
                Point tile_vert = new Point((int)x_vert, (int)y_vert);
                if (tile_vert.X >= 0 && tile_vert.X < map.map.GetLength(1) && tile_vert.Y >= 0 && tile_vert.Y < map.map.GetLength(0))
                {
                    if (map.map[tile_vert.Y, tile_vert.X] == 1)
                    {
                        break;
                    }
                }
                else { break; }

                x_vert += v_new_x;
                y_vert += v_new_y;
                depth_vert += v_new_depth;
            }
            //Get the needed depth
            depth = depth_vert < depth_hor?depth_vert:depth_hor;

            

            return new PointF(player_pos.X + setting.PLAYER_MAP_SCALE * (float)depth * (float)cos, player_pos.Y + setting.PLAYER_MAP_SCALE * (float)depth * (float)sin);
        }
        public void UpdateAngle(double angle)
        {
            player_angle = angle;
        }

        public void UpdateCoords(PointF player_pos)
        {
            this.player_pos = player_pos;
            this.map_pos = new Point((int)player_pos.X / setting.PLAYER_MAP_SCALE, (int)player_pos.Y/setting.PLAYER_MAP_SCALE);
        }

        public PointF[] Draw()
        {
            PointF[] temp = new PointF[setting.NUM_RAYS];

            double ray_angle = player_angle - setting.HALF_FOV + 0.0001;
            for (int ray = 0; ray < setting.NUM_RAYS; ray++)
            {

                temp[ray] = RayCast(ray_angle);
                ray_angle += setting.DELTA_ANGLE;
            }

            return temp;
        }
    }
}
