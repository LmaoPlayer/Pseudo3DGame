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
        double projected_height;

        public Raycasting(Settings setting, Map map) 
        {
            this.setting = setting;
            this.player_angle = setting.PLAYER_ANGLE;
            this.player_pos = setting.PLAYER_START;
            this.map_pos = new Point((int)Math.Floor(setting.PLAYER_START.X / setting.PLAYER_MAP_SCALE), (int)Math.Floor(setting.PLAYER_START.Y / setting.PLAYER_MAP_SCALE));
            this.map = map;
        }
        
        public void UpdateAngle(double angle)
        {
            player_angle = angle;
        }

        public double GetProjHeight()
        {
            return projected_height;
        }

        public void UpdateCoords(PointF player_pos)
        {
            this.player_pos = player_pos;
            this.map_pos = new Point((int)player_pos.X / setting.PLAYER_MAP_SCALE, (int)player_pos.Y/setting.PLAYER_MAP_SCALE);
        }

        private float[] RayCast3D(double ray_angle)
        {
            sin = Math.Sin(ray_angle);
            cos = Math.Cos(ray_angle);



            //horizontal raycast
            int wall_picture_hor = 0;
            double y_hor;
            double new_y;
            if (sin > 0)
            {
                y_hor = map_pos.Y + 1;
                new_y = 1;
            }
            else
            {
                y_hor = map_pos.Y - setting.DIV_BY_ZERO_ERROR;
                new_y = -1;
            }

            double depth_hor = (y_hor - (player_pos.Y / setting.PLAYER_MAP_SCALE)) / sin;
            double x_hor = (player_pos.X / setting.PLAYER_MAP_SCALE) + (depth_hor * cos);

            double new_depth = new_y / sin;
            double new_x = new_depth * cos;

            for (int i = 0; i < setting.MAX_DEPTH; i++)
            {
                Point tile_hor = new Point((int)x_hor, (int)y_hor);
                if (tile_hor.X >= 0 && tile_hor.X < map.map.GetLength(1) && tile_hor.Y >= 0 && tile_hor.Y < map.map.GetLength(0))
                {
                    if (map.map[tile_hor.Y, tile_hor.X] != 0)
                    {
                        wall_picture_hor = map.map[tile_hor.Y, tile_hor.X] - 1;
                        break;
                    }
                }
                else { break; }

                x_hor += new_x;
                y_hor += new_y;
                depth_hor += new_depth;
            }

            //vertical raycast
            int wall_picture_ver = 0;
            double x_vert;
            double v_new_x = 0;
            if (cos > 0)
            {
                x_vert = map_pos.X + 1;
                v_new_x = 1;
            }
            else
            {
                x_vert = map_pos.X - setting.DIV_BY_ZERO_ERROR;
                v_new_x = -1;
            }

            double depth_vert = (x_vert - (player_pos.X / setting.PLAYER_MAP_SCALE)) / cos;
            double y_vert = (player_pos.Y / setting.PLAYER_MAP_SCALE) + (depth_vert * sin);

            double v_new_depth = v_new_x / cos;
            double v_new_y = v_new_depth * sin;

            for (int i = 0; i < setting.MAX_DEPTH; i++)
            {
                Point tile_vert = new Point((int)x_vert, (int)y_vert);
                if (tile_vert.X >= 0 && tile_vert.X < map.map.GetLength(1) && tile_vert.Y >= 0 && tile_vert.Y < map.map.GetLength(0))
                {
                    if (map.map[tile_vert.Y, tile_vert.X] != 0)
                    {
                        wall_picture_ver = map.map[tile_vert.Y, tile_vert.X] - 1;
                        break;
                    }
                }
                else { break; }

                x_vert += v_new_x;
                y_vert += v_new_y;
                depth_vert += v_new_depth;
            }
            //Get the needed depth
            depth = depth_vert < depth_hor ? depth_vert : depth_hor;

            float offset = 0;

            int wall_pic = 0;

            if (depth_vert < depth_hor)
            {
                if (cos < 0) offset = 1 - ((float)y_vert % 1);
                else if (cos > 0) offset = ((float)y_vert % 1);
                wall_pic = wall_picture_ver;
            }
            else
            {
                if (sin > 0) offset = 1 - ((float)x_hor % 1);
                else if (sin < 0) offset = ((float)x_hor % 1);
                wall_pic = wall_picture_hor;
            }

            //Verwijder het FishBowl effect: muren worden bol als je dicht komt staan
            depth *= Math.Cos(player_angle-ray_angle);

            //Hoogte van de muren in het 3D veld
            projected_height = setting.SCREEN_DIST / (depth + setting.DIV_BY_ZERO_ERROR);

            //Muren die groter dan het scherm zijn worden kleiner geprojecteerd
            //projected_height = Math.Min(projected_height, setting.HEIGHT);

            //return new PointF(player_pos.X + setting.PLAYER_MAP_SCALE * (float)depth * (float)cos, player_pos.Y + setting.PLAYER_MAP_SCALE * (float)depth * (float)sin);


            return new float[]{ (float)(setting.HEIGHT / 2 - (projected_height / 2)), (float)projected_height, (float)depth, offset, wall_pic };
        }
        public float[,] Draw3D()
        {
            float[,] temp = new float[setting.NUM_RAYS, 7];
            float[] temp_point;

            double ray_angle = player_angle - setting.HALF_FOV + 0.0001;
            for (int ray = 0; ray < setting.NUM_RAYS; ray++)
            {
                temp_point = RayCast3D(ray_angle);


                //temp_point.Y = Math.Min(temp_point.Y, setting.HEIGHT*2);

                temp[ray, 0] = (float)(ray * setting.WALL_SCALE);
                temp[ray, 1] = (float)temp_point[0]; //(float)(setting.HEIGHT / 2 - (projected_height / 2)),
                temp[ray, 2] = (float)setting.WALL_SCALE;
                temp[ray, 3] = (float)temp_point[1]; //(float)projected_height)
                temp[ray, 4] = temp_point[2]; //depth
                temp[ray, 5] = temp_point[3];
                temp[ray, 6] = temp_point[4];

                ray_angle += setting.DELTA_ANGLE;
            }

            //Console.WriteLine(temp.GetLength(0));
            return temp;
        }
        public PointF[] Draw2D()
        {
            PointF[] temp = new PointF[setting.NUM_RAYS];

            double ray_angle = player_angle - setting.HALF_FOV + 0.0001;
            for (int ray = 0; ray < setting.NUM_RAYS; ray++)
            {

                temp[ray] = RayCast2D(ray_angle);
                ray_angle += setting.DELTA_ANGLE;
            }

            return temp;
        }
        private PointF RayCast2D(double ray_angle)
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
                y_hor = map_pos.Y - setting.DIV_BY_ZERO_ERROR;
                new_y = -1;
            }

            double depth_hor = (y_hor - (player_pos.Y / setting.PLAYER_MAP_SCALE)) / sin;
            double x_hor = (player_pos.X / setting.PLAYER_MAP_SCALE) + (depth_hor * cos);

            double new_depth = new_y / sin;
            double new_x = new_depth * cos;

            for (int i = 0; i < setting.MAX_DEPTH; i++)
            {
                Point tile_hor = new Point((int)x_hor, (int)y_hor);
                if (tile_hor.X >= 0 && tile_hor.X < map.map.GetLength(1) && tile_hor.Y >= 0 && tile_hor.Y < map.map.GetLength(0))
                {
                    if (map.map[tile_hor.Y, tile_hor.X] != 0)
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
                x_vert = map_pos.X - setting.DIV_BY_ZERO_ERROR;
                v_new_x = -1;
            }

            double depth_vert = (x_vert - (player_pos.X / setting.PLAYER_MAP_SCALE)) / cos;
            double y_vert = (player_pos.Y / setting.PLAYER_MAP_SCALE) + (depth_vert * sin);

            double v_new_depth = v_new_x / cos;
            double v_new_y = v_new_depth * sin;

            for (int i = 0; i < setting.MAX_DEPTH; i++)
            {
                Point tile_vert = new Point((int)x_vert, (int)y_vert);
                if (tile_vert.X >= 0 && tile_vert.X < map.map.GetLength(1) && tile_vert.Y >= 0 && tile_vert.Y < map.map.GetLength(0))
                {
                    if (map.map[tile_vert.Y, tile_vert.X] != 0)
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
            depth = depth_vert < depth_hor ? depth_vert : depth_hor;



            return new PointF(player_pos.X + setting.PLAYER_MAP_SCALE * (float)depth * (float)cos, player_pos.Y + setting.PLAYER_MAP_SCALE * (float)depth * (float)sin);
        }
    }
}
