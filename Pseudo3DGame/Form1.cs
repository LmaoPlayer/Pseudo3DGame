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
using static System.Windows.Forms.AxHost;
using System.IO;

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
        Map game_map;

        //Meerdere keys tergelijk
        HashSet<Keys> pressed_keys = new HashSet<Keys>();

        //Delta_Time
        Stopwatch stopwatch = new Stopwatch();

        //MousePointCheck
        Point center;

        //2D en 3D optie zonder de code aan te passen.
        int dialog;

        PictureEditorToCorrectSize[] walls;

        //Main function
        public Form1()
        {
            //Debug met 2D
            dialog = MessageBox.Show("Would you like the 3D preview?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes ? 1 : 0;

            //setup
            game_map = new Map(game_settings);
            character = new Player(game_settings, game_map);
            rays = new Raycasting(game_settings, game_map);

            //zet resolutie
            this.Size = new Size(game_settings.WIDTH+16, game_settings.HEIGHT+39);

            //Test met meerdere achtervoegsels
            Image tempwall1;
            try
            {
                 tempwall1 = Image.FromFile("Textures/wall1.png");
            }
            catch (FileNotFoundException)
            {
                try
                {
                    tempwall1 = Image.FromFile("Textures/wall1.jpeg");
                }
                catch (FileNotFoundException)
                {
                    tempwall1 = Image.FromFile("Textures/wall1.jpg");
                }
            }

            Image tempwall2;
            try
            {
                tempwall2 = Image.FromFile("Textures/wall2.png");
            }
            catch (FileNotFoundException)
            {
                try
                {
                    tempwall2 = Image.FromFile("Textures/wall2.jpeg");
                }
                catch (FileNotFoundException)
                {
                    tempwall2 = Image.FromFile("Textures/wall2.jpg");
                }
            }



            //Laad de images.
            walls = new PictureEditorToCorrectSize[] { new PictureEditorToCorrectSize(game_settings, tempwall1), new PictureEditorToCorrectSize(game_settings, tempwall2), new PictureEditorToCorrectSize(game_settings, Image.FromFile("Textures/Wall3.png")) };

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

            //Verstop de cursor
            Cursor.Hide();

            //Sla de center 1 keer op
            center = this.PointToScreen(new Point(game_settings.WIDTH / 2, game_settings.HEIGHT / 2));

            Timer mouse_timer = new Timer();
            mouse_timer.Interval = 1;
            mouse_timer.Tick += (sender, e) => MouseHandler();
            mouse_timer.Start();
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

            Cursor.Position = center;
        }

        

        public void DrawScreen(PaintEventArgs e, int temp)
        {
            Graphics g = e.Graphics;
            
            SolidBrush P = new SolidBrush(Color.Black);
            Pen RayPen = new Pen(Color.Yellow, 2);
            SolidBrush BG = new SolidBrush(Color.FromArgb(255, 255, 255));


            g.FillRectangle(BG, 0, 0, game_settings.WIDTH, game_settings.HEIGHT);


            if (dialog == 1) Draw3D(g);
            else Draw2D(g, P, RayPen);


            BG.Dispose();
            P.Dispose();
            RayPen.Dispose();
        }

        private void Draw2D(Graphics g, Brush P, Pen RayPen)
        {
            //Teken de map
            for (int map_length = 0; map_length < game_map.map.GetLength(0); map_length++)
            {
                for (int map_width = 0; map_width < game_map.map.GetLength(1); map_width++)
                {
                    if (game_map.map[map_length, map_width] == 1) g.FillRectangle(P, new Rectangle(map_width * game_settings.PLAYER_MAP_SCALE, map_length * game_settings.PLAYER_MAP_SCALE, game_settings.PLAYER_MAP_SCALE, game_settings.PLAYER_MAP_SCALE));
                }

            }

            PointF playerP = character.GetLoc();
            float player_angle = (float)(character.GetAngle()*Math.PI/180);

            g.FillEllipse(P, new RectangleF(playerP.X - 5, playerP.Y - 5, 10, 10));
            //Console.WriteLine(character.GetMapLoc());

            // draw every 4th ray: less lag
            //int step = 4;
            int step = 1;
            var ray_points = rays.Draw2D();
            for (int i = 0; i < ray_points.Length; i += step)
            {
                PointF hit = ray_points[i];
                g.DrawLine(RayPen, playerP.X, playerP.Y, hit.X, hit.Y);
            }
        }

            
        private void Draw3D(Graphics g)
        {
            Bitmap bmp = walls[2].GetBMP();
            float[,] ray_points = rays.Draw3D();
            double vert_angle = character.GetVertAngle();

            for (int i = 0; i < ray_points.GetLength(0); i++)
            {
                //Fog effect
                //int Col = Math.Abs((int)((Math.Pow(ray_points[i, 4], 5) / 255)*0.2F));
                //if (Col > 255) Col = 255;
                //Brush B = new SolidBrush(Color.FromArgb(Col, Col, Col));

                //Teken de rechthoeken op de plaatsen waar de raycasts uitkomen
                //g.FillRectangle(B, ray_points[i, 0], ray_points[i, 1]+(float)vert_angle, ray_points[i, 2], ray_points[i, 3]);
                int sliceX = Math.Min((int)(ray_points[i, 5] * game_settings.TEXTURE_SIZE), game_settings.TEXTURE_SIZE - (int)ray_points[i, 2]);

                int sliceY = 0;
                
                Bitmap SavedWallPiece = bmp.Clone(new Rectangle(sliceX, sliceY, (int)ray_points[i, 2], bmp.Height), bmp.PixelFormat);
                g.DrawImage(SavedWallPiece, new RectangleF(ray_points[i, 0], ray_points[i, 1] + (float)vert_angle, ray_points[i, 2], ray_points[i, 3]));
                //B.Dispose();
            }

            using (Pen p = new Pen(Color.Gray))
            {
                g.DrawLine(p, game_settings.WIDTH / 2 - 5, game_settings.HEIGHT / 2, game_settings.WIDTH / 2 + 5, game_settings.HEIGHT /2);
                g.DrawLine(p, game_settings.WIDTH / 2, game_settings.HEIGHT / 2 - 5, game_settings.WIDTH / 2, game_settings.HEIGHT / 2 + 5);
            }
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
            if (pressed_keys.Contains(Keys.Z))
            {
                if (pressed_keys.Contains(Keys.ControlKey)) character.Forward(true);
                else character.Forward(false);
            }
            if (pressed_keys.Contains(Keys.S)) character.Back();
            if (pressed_keys.Contains(Keys.Right)) character.TurnRight();
            if (pressed_keys.Contains(Keys.Left)) character.TurnLeft();
            if (pressed_keys.Contains(Keys.Up)) character.RotateUD(30);
            if (pressed_keys.Contains(Keys.Down)) character.RotateUD(-30);
        }

        private void MouseHandler()
        {
            character.RotateLR((center.X - Cursor.Position.X)*0.5F);

            if (dialog == 1) character.RotateUD((center.Y - Cursor.Position.Y) * 0.5F);
        }
    }
}
