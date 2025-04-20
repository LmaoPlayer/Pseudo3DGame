using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

//SCREAMING_SNAKE_CASE = constant
//CamelCase = class
//snake_case = variable

namespace Pseudo3DGame
{
    public partial class Form1 : Form
    {
        Timer clock;

        PictureBox f;

        //Initialise settings
        Settings game_settings = new Settings();

        //Initialise Player
        Player character;

        //Initialise RayCasting
        Raycasting rays;

        //import map
        Map game_map = new Map();

        //Meerdere keys tergelijk
        HashSet<Keys> pressed_keys = new HashSet<Keys>();

        //Delta_Time
        Stopwatch stopwatch = new Stopwatch();

        //Main function
        public Form1()
        {
            character = new Player(game_settings, game_map);
            rays = new Raycasting(game_settings, game_map);

            //zet resolutie
            this.Size = new Size(game_settings.WIDTH+16, game_settings.HEIGHT+39);

            //Maak een game clock: 1 seconde delen door FPS
            clock = new Timer();
            clock.Interval = 1000 / game_settings.FPS;
            clock.Tick += (sender, e) => { GameUpdater(); HandleKeys(); };

            //Maak veld aan (F van Field)
            f = new PictureBox();
            f.Size = new Size(game_settings.WIDTH, game_settings.HEIGHT);
            int temp = 0;
            f.Paint += (sender, e) => { DrawScreen(e, temp); };
            Controls.Add(f);

            //DT
            stopwatch.Start();

            //Start spel
            clock.Start();

            //Event Checker
            this.KeyDown += (sender, e) => TestKeyDown(e);
            this.KeyUp += (sender, e) => TestKeyUp(e);

        }

        public void GameUpdater()
        {
            rays.UpdateAngle(character.GetAngle());
            rays.UpdateCoords(character.GetLoc());

            double delta = stopwatch.Elapsed.TotalSeconds;
            delta = Math.Min(delta, 0.1);
            stopwatch.Restart();
            character.UpdateDT(delta);
            f.Invalidate();
        }

        

        public void DrawScreen(PaintEventArgs e, int temp)
        {
            Graphics g = e.Graphics;
            Pen B = new Pen(Color.Black, 2);
            Pen P = new Pen(Color.Yellow, 2);

            //Teken de map
            for (int map_length = 0; map_length < game_map.map.GetLength(0); map_length++)
            {
                for (int map_width = 0; map_width < game_map.map.GetLength(1); map_width ++)
                {
                    if (game_map.map[map_length, map_width] == 1) g.DrawRectangle(B, new Rectangle(map_width*game_settings.PLAYER_MAP_SCALE, map_length* game_settings.PLAYER_MAP_SCALE, game_settings.PLAYER_MAP_SCALE, game_settings.PLAYER_MAP_SCALE));
                }

            }

            PointF playerP = character.GetLoc();

            g.DrawEllipse(B, new RectangleF(playerP.X - 5, playerP.Y - 5, 10, 10));
            //Console.WriteLine(character.GetMapLoc());
            g.DrawLine(B, playerP.X, playerP.Y, playerP.X+(40 * (float)Math.Cos(character.GetAngle())), playerP.Y + (40*(float)Math.Sin(character.GetAngle())));

            int step = 4; // draw every 4th ray
            var ray_points = rays.Draw();
            for (int i = 0; i < ray_points.Length; i += step)
            {
                PointF hit = ray_points[i];
                g.DrawLine(P, playerP.X, playerP.Y, hit.X, hit.Y);
            }

            B.Dispose();
            P.Dispose();
        }

        private void TestKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                clock.Stop();
                this.Close();
            }
            else
            {
                pressed_keys.Add(e.KeyCode);
                HandleKeys();
            }
                
        }

        private void TestKeyUp(KeyEventArgs e)
        {
            pressed_keys.Remove(e.KeyCode);
        }

        private void HandleKeys()
        {
            if (pressed_keys.Contains(Keys.W) && pressed_keys.Contains(Keys.ShiftKey))
            {
            }
            else if (pressed_keys.Contains(Keys.W))
            {
            }
            if (pressed_keys.Contains(Keys.Q)) character.Left();
            if (pressed_keys.Contains(Keys.D)) character.Right();
            if (pressed_keys.Contains(Keys.Z)) character.Forward();
            if (pressed_keys.Contains(Keys.S)) character.Back();
            if (pressed_keys.Contains(Keys.Right)) character.TurnRight();
            if (pressed_keys.Contains(Keys.Left)) character.TurnLeft();

            
        }
    }
}
