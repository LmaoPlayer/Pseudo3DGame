﻿using System;
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
using System.Runtime.Remoting.Channels;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        Settings game_settings;

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

        PictureEditorToCorrectSize Floor;

        PictureEditorToCorrectSize Sky;
        bool EnableHair = false;

        int CurrentMenuLayer = 0;
        MainMenu esc;
        bool MouseVisible = false;

        enum setting_menus { ResPacks, MapSaver}

        setting_menus opened_menu;


        ImageGrabber FindImg = new ImageGrabber();

        //Main function
        public Form1()
        {
            this.DoubleBuffered = true;


            //Debug met 2D
            dialog = MessageBox.Show("Would you like the 3D preview?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes ? 1 : 0;

            this.WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            game_settings = new Settings(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //setup
            game_map = new Map(game_settings);
            character = new Player(game_settings, game_map);
            rays = new Raycasting(game_settings, game_map);

            //zet resulotie als grootte van form
            //this.Size = new Size(game_settings.WIDTH+16, game_settings.HEIGHT+39);
            //this.CenterToScreen();
            //this.Location = new Point((Screen.PrimaryScreen.Bounds.Width-this.Width)/2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);

            //Test met meerdere achtervoegsels
            Image[] TempImages = game_settings.ALL_TEXTURES.Select(texture => FindImg.FindImg($"Textures/{texture}")).ToArray();

            //Image tempfloor = FindImg.FindImg("Textures/floor");


            //Laad de images.
            ImageFixer(TempImages);
            //Floor = new PictureEditorToCorrectSize(game_settings, tempfloor);


            //Maak een game clock: 1 seconde delen door FPS
            clock = new Timer();
            clock.Interval = 1000 / game_settings.FPS;
            clock.Tick += (sender, e) => { GameUpdater(); HandleKeys(); };

            //Maak veld aan (F van Field)
            f = new PictureBox();
            f.Size = new Size(game_settings.WIDTH, game_settings.HEIGHT);
            int temp = 0;
            f.Paint += (sender, e) => { if (character.CanStartDrawing && game_map.IsFinished) DrawScreen(e, temp); };
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


            //Button setup
            Panel menu_screen = new Panel();

            esc = new MainMenu(game_settings, menu_screen, this, new Font("Serif", (int)(game_settings.HEIGHT / 200) * 5, FontStyle.Bold));
            Controls.Add(menu_screen);

            esc.QuitClick += (sender, e) => Exit();
            esc.SettingsClick += (sender, e) => { CurrentMenuLayer = 2; CheckMenuToShow(); };
            esc.setting_menu.OpenRPMenu += (sender, e) => { CurrentMenuLayer = 3; opened_menu = setting_menus.ResPacks; CheckMenuToShow(); };
            esc.setting_menu.OpenMapMenu += (sender, e) => { CurrentMenuLayer = 3; opened_menu = setting_menus.MapSaver; CheckMenuToShow(); };
            esc.setting_menu.RPMenu.OpenRPFolder += (sender, e) => Process.Start("ResourcePacks");
            esc.setting_menu.map_menu.OpenMapFolder += (sender, e) => Process.Start("Maps");
            esc.setting_menu.RPMenu.EscapeKeyPressed += (sender, e) => PauzeFunction();
            esc.setting_menu.RPMenu.ApplyTextures += (sender, e) =>
            {
                if (esc.setting_menu.RPMenu.RPList.CheckedItems.Count > 0)
                {
                    string the_item = esc.setting_menu.RPMenu.RPList.CheckedItems[0].ToString();

                    bool[] can_still_find = Enumerable.Repeat(true, game_settings.ALL_TEXTURES.Length).ToArray();

                    foreach (string item in Directory.GetFiles($"ResourcePacks/{the_item}"))
                    {
                        for (int i = 0; i < game_settings.ALL_TEXTURES.Length; i++)
                        {
                            Match match = Regex.Match(item, $@"{game_settings.ALL_TEXTURES[i]}\.");
                            if (match.Success)
                            {
                                can_still_find[i] = false;
                                TempImages[i] = FindImg.FindImg($"ResourcePacks/{the_item}\\{game_settings.ALL_TEXTURES[i]}");
                            }
                            else if (can_still_find[i]) TempImages[i] = FindImg.FindImg($"Textures/{game_settings.ALL_TEXTURES[i]}");
                        }
                    }
                }
                else
                {
                    TempImages = game_settings.ALL_TEXTURES.Select(texture => FindImg.FindImg($"Textures/{texture}")).ToArray();
                }
                ImageFixer(TempImages);
                f.Invalidate();
            };
            esc.setting_menu.map_menu.ApplyMapEvent += (sender, e) =>
            {
                if (esc.setting_menu.map_menu.MapList.CheckedItems.Count > 0)
                {
                    if (esc.setting_menu.map_menu.MapList.CheckedItems[0].ToString() == "Random")
                    {
                        int TestWidth = game_settings.MAP_WIDTH;
                        int TestHeight = game_settings.MAP_HEIGHT;

                        int.TryParse(esc.setting_menu.map_menu.WidthTXT.Text, out TestWidth);
                        int.TryParse(esc.setting_menu.map_menu.HeightTXT.Text, out TestHeight);

                        game_settings.ChangedMap(TestWidth, TestHeight);
                        game_map.GenerateRandom();
                        character.CreateStartPos();
                    }
                    string the_item = esc.setting_menu.map_menu.MapList.CheckedItems[0].ToString();
                    
                    if (File.Exists($"Maps/{the_item}.csv"))
                    {
                        string[] mapSTR = File.ReadAllLines($"Maps/{the_item}.csv");
                        game_map.GenerateWithFile(mapSTR);
                        character.CreateStartPos();
                    }
                }
                else
                {
                    MessageBox.Show("No Map Selected");
                }
                f.Invalidate();
            };
            esc.setting_menu.RPMenu.RPList.ItemCheck += (sender, e) =>
            {
                for (int i = 0; i < esc.setting_menu.RPMenu.RPList.Items.Count; i++)
                {
                    if (!esc.setting_menu.RPMenu.RPList.GetItemChecked(e.Index))
                    {
                        if (i != e.Index)
                        {
                            esc.setting_menu.RPMenu.RPList.SetItemChecked(i, false);
                        }
                    }
                }
            };
            esc.setting_menu.map_menu.MapList.ItemCheck += (sender, e) =>
            {
                bool ShouldITemporarilyChange = dialog == 0 ? false : true;
                dialog = 0;
                for (int i = 0; i < esc.setting_menu.map_menu.MapList.Items.Count; i++)
                {
                    if (!esc.setting_menu.map_menu.MapList.GetItemChecked(e.Index))
                    {
                        if (i != e.Index)
                        {
                            esc.setting_menu.map_menu.MapList.SetItemChecked(i, false);
                            
                        }
                    }
                }
                rays.UpdateAngle(character.GetAngle());
                rays.UpdateCoords(character.GetLoc());
                f.Invalidate();
                if (ShouldITemporarilyChange) dialog = 1;
                //f.Invalidate();
            };
            esc.setting_menu.map_menu.SaveMap += (sender, e) => { game_map.SaveMap(Interaction.InputBox("File Name:", "File_Name", "")); };

            if (!Directory.Exists("ResourcePacks")) Directory.CreateDirectory("ResourcePacks");
            if (!Directory.Exists("Maps")) Directory.CreateDirectory("Maps");
            esc.setting_menu.RPMenu.SetupRPList();
            esc.setting_menu.map_menu.SetupMapList();

            f.SendToBack();
        }
        public void GameUpdater()
        {
            rays.UpdateAngle(character.GetAngle());
            rays.UpdateCoords(character.GetLoc());
            if (CurrentMenuLayer == 0)
            {
                double delta = stopwatch.Elapsed.TotalSeconds;
                delta = Math.Min(delta, 0.1);
                stopwatch.Restart();
                character.UpdateDT(delta);
                
                Cursor.Position = center;
                Focus();
            }
            f.Invalidate();
        }
        public void DrawScreen(PaintEventArgs e, int temp)
        {
            Graphics g = e.Graphics;

            SolidBrush[] P = { new SolidBrush(Color.Black), new SolidBrush(Color.Red), new SolidBrush(Color.Blue) };
            Pen RayPen = new Pen(Color.Yellow, 2);
            SolidBrush BG = new SolidBrush(Color.FromArgb(255, 255, 255));


            g.FillRectangle(BG, 0, 0, game_settings.WIDTH, game_settings.HEIGHT);

            if (dialog == 1) Draw3D(g);
            else Draw2D(g, P, RayPen);

            //Draw an annoying hair (Yeah, this is the easter egg...)
            if (EnableHair) g.DrawBezier(new Pen(Color.Black), new Point(200, 200), new Point(210, 200), new Point(210, 210), new Point(210, 230));

            BG.Dispose();
            RayPen.Dispose();
        }
        private void Draw2D(Graphics g, Brush[] P, Pen RayPen)
        {
            Bitmap[] bmp = new Bitmap[] { walls[0].GetBMP(), walls[1].GetBMP(), walls[2].GetBMP() };
            //Teken de map
            for (int map_length = 0; map_length < game_map.map.GetLength(0); map_length++)
            {
                if (map_length >= game_map.map.GetLength(0)) break;
                for (int map_width = 0; map_width < game_map.map.GetLength(1); map_width++)
                {
                    if (map_width >= game_map.map.GetLength(1)) break;
                    if (game_map.IsFinished && game_map.map[map_length, map_width] != 0) g.DrawImage(bmp[game_map.map[map_length, map_width] - 1], new Rectangle(map_width * game_settings.PLAYER_MAP_SCALE, map_length * game_settings.PLAYER_MAP_SCALE, game_settings.PLAYER_MAP_SCALE, game_settings.PLAYER_MAP_SCALE));
                }
            }

            PointF playerP = character.GetLoc();
            float player_angle = (float)(character.GetAngle()*Math.PI/180);

            g.FillEllipse(P[0], new RectangleF(playerP.X - game_settings.MINIMUM_WALL_PLAYER_DISTANCE, playerP.Y - game_settings.MINIMUM_WALL_PLAYER_DISTANCE, game_settings.MINIMUM_WALL_PLAYER_DISTANCE*2, game_settings.MINIMUM_WALL_PLAYER_DISTANCE*2));
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
            Bitmap[] bmp = new Bitmap[] { walls[0].GetBMP(), walls[1].GetBMP(), walls[2].GetBMP(), Sky.GetBMP()};
            double vert_angle = character.GetVertAngle();
            double hor_angle = character.GetHorAngle();


            int offsetX = (int)(hor_angle * bmp[3].Width/3) % bmp[3].Width;
            Rectangle temp_rect = new Rectangle((int)((bmp[3].Width - offsetX) * (game_settings.WIDTH / (double)bmp[3].Width)), (int)character.GetVertAngle() - game_settings.HEIGHT / 2, game_settings.WIDTH + 5, game_settings.HEIGHT);
            Rectangle temp_rect2 = new Rectangle((int)((bmp[3].Width - offsetX - bmp[3].Width) * (game_settings.WIDTH / (double)bmp[3].Width)), (int)character.GetVertAngle() - game_settings.HEIGHT / 2, game_settings.WIDTH + 5, game_settings.HEIGHT);


            //int floor_offsetX = (int)(hor_angle * bmp[4].Width) % bmp[4].Width;
            //Rectangle floor_temp_rect = new Rectangle((int)(Math.Min(bmp[4].Width - floor_offsetX, bmp[4].Width) * (game_settings.WIDTH / (double)bmp[4].Width)), (int)character.GetVertAngle() + game_settings.HEIGHT / 2, game_settings.WIDTH + 5, game_settings.HEIGHT);
            //Rectangle floor_temp_rect2 = new Rectangle((int)(Math.Min(bmp[4].Width - floor_offsetX - bmp[4].Width, bmp[4].Width) * (game_settings.WIDTH / (double)bmp[4].Width)), (int)character.GetVertAngle() + game_settings.HEIGHT / 2, game_settings.WIDTH + 5, game_settings.HEIGHT);

            //Floor en sky effect
            g.FillRectangle(new SolidBrush(Color.Firebrick), new Rectangle(0, game_settings.HEIGHT / 2 + (int)character.GetVertAngle(), game_settings.WIDTH, game_settings.HEIGHT/2 - (int)character.GetVertAngle()));
            g.DrawImage(bmp[3], temp_rect);
            g.DrawImage(bmp[3], temp_rect2);

            //g.DrawImage(bmp[4], floor_temp_rect);
            //g.DrawImage(bmp[4], floor_temp_rect2);






            float[,] ray_points = rays.Draw3D();

            int FogR = bmp[3].GetPixel((int)(bmp[3].Width / game_settings.PIXEL_SELECTION), (int)(bmp[3].Height / game_settings.PIXEL_SELECTION)).R;
            int FogB = bmp[3].GetPixel((int)(bmp[3].Width / game_settings.PIXEL_SELECTION), (int)(bmp[3].Height / game_settings.PIXEL_SELECTION)).B;
            int FogG = bmp[3].GetPixel((int)(bmp[3].Width / game_settings.PIXEL_SELECTION), (int)(bmp[3].Height / game_settings.PIXEL_SELECTION)).G;

            for (int i = 0; i < ray_points.GetLength(0); i++)
            {
                //Fog effect
                int Col = Math.Abs((int)((Math.Pow(ray_points[i, 4], 5.5) / 255)*0.2F));
                if (Col > 255) Col = 255;
                //Col = (255 - Col) % 255;
                
                Brush B = new SolidBrush(Color.FromArgb(Col, FogR, FogG, FogB));

                //Teken de rechthoeken op de plaatsen waar de raycasts uitkomen
                
                int textureIndex = (int)ray_points[i, 6];
                int sliceWidth = (int)ray_points[i, 2];
                int fullHeight = bmp[textureIndex].Height;


                int sliceX = Math.Min((int)(ray_points[i, 5] * game_settings.TEXTURE_SIZE), game_settings.TEXTURE_SIZE - sliceWidth);

                float wallScreenY = ray_points[i, 1] + (float)vert_angle;
                float wallHeightOnScreen = ray_points[i, 3];


                float RescaleImgUp = 0;
                float RescaleViewUp = 0;

                if (wallScreenY < 0)
                {
                    RescaleViewUp = -wallScreenY;
                    RescaleImgUp = RescaleViewUp * fullHeight / wallHeightOnScreen;

                    
                    if (RescaleImgUp > fullHeight) RescaleImgUp = fullHeight;
                    if (RescaleViewUp > wallHeightOnScreen) RescaleViewUp = wallHeightOnScreen;
                }

                int sliceY = (int)RescaleImgUp;
                int srcHeight = fullHeight - sliceY;
                float dstHeight = wallHeightOnScreen - RescaleViewUp;

                if (sliceY < 0) sliceY = 0;
                if (srcHeight < 1) srcHeight = 1;
                if (dstHeight < 1) dstHeight = 1;



                Bitmap SavedWallPiece = bmp[textureIndex].Clone(
                    new Rectangle(sliceX, sliceY, sliceWidth, srcHeight),
                    bmp[textureIndex].PixelFormat);

                g.DrawImage(
                    SavedWallPiece,
                    new RectangleF(ray_points[i, 0], wallScreenY + RescaleViewUp, sliceWidth, dstHeight));

                g.FillRectangle(B, ray_points[i, 0], ray_points[i, 1] + (float)vert_angle, ray_points[i, 2], ray_points[i, 3]);
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
                PauzeFunction();
            }
            else
            {
                pressed_keys.Add(e.KeyCode);
                HandleKeys();
            }

            if (e.KeyCode == Keys.L) dialog = 1 - dialog;
        }
        private void TestKeyUp(KeyEventArgs e)
        {
            pressed_keys.Remove(e.KeyCode);
        }
        private void HandleKeys()
        {
            if (CurrentMenuLayer == 0)
            {
                if (pressed_keys.Contains(Keys.Q)) character.Left();
                if (pressed_keys.Contains(Keys.D)) character.Right();
                if (pressed_keys.Contains(Keys.Z))
                {
                    if (pressed_keys.Contains(Keys.ControlKey))
                    {
                        character.Forward(true);
                    }
                    else
                    {
                        character.Forward(false);
                    }
                }
                if (pressed_keys.Contains(Keys.S)) character.Back();
                if (pressed_keys.Contains(Keys.Right)) character.TurnRight();
                if (pressed_keys.Contains(Keys.Left)) character.TurnLeft();
                if (pressed_keys.Contains(Keys.Up)) character.RotateUD(30);
                if (pressed_keys.Contains(Keys.Down)) character.RotateUD(-30);
            }
            if (!EnableHair && pressed_keys.Contains(Keys.R) && pressed_keys.Contains(Keys.P) && pressed_keys.Contains(Keys.Enter)) EnableHair = true;
        }
        private void MouseHandler()
        {
            if (CurrentMenuLayer == 0)
            {
                character.RotateLR((center.X - Cursor.Position.X) * 0.5F);

                if (dialog == 1) character.RotateUD((center.Y - Cursor.Position.Y) * 0.5F);
            }
        }
        private void PauzeFunction()
        {
            if (CurrentMenuLayer == 0) CurrentMenuLayer = 1;
            else CurrentMenuLayer -= 1;
            CheckMenuToShow();
            f.Invalidate();
        }
        private void Exit()
        {
            Close();      
        }
        private void CheckMenuToShow()
        {
            switch (CurrentMenuLayer)
            {
                case 0:
                    esc.Hide();
                    esc.setting_menu.Hide();
                    esc.setting_menu.RPMenu.Hide();
                    esc.setting_menu.map_menu.Hide();
                    break;
                case 1:
                    esc.Show();
                    esc.setting_menu.Hide();
                    esc.setting_menu.RPMenu.Hide();
                    esc.setting_menu.map_menu.Hide();
                    break;
                case 2:
                    esc.Hide();
                    esc.setting_menu.Show();
                    esc.setting_menu.RPMenu.Hide();
                    esc.setting_menu.map_menu.Hide();
                    break;
                case 3:
                    if (opened_menu == setting_menus.ResPacks) { esc.setting_menu.RPMenu.Show(); esc.setting_menu.map_menu.Hide(); }
                    else if (opened_menu == setting_menus.MapSaver) { esc.setting_menu.map_menu.Show(); esc.setting_menu.RPMenu.Hide(); }
                    esc.Hide();
                    esc.setting_menu.Hide();
                    break;
                default:
                    break;
            }
            Focus();

            if (CurrentMenuLayer == 0 && MouseVisible)
            {
                Cursor.Hide();
                MouseVisible = false;
            }
            else if (CurrentMenuLayer != 0 && !MouseVisible)
            {
                Cursor.Show();
                MouseVisible = true;
            }
        }
        private void ImageFixer(Image[] TempImages)
        {
            walls = new PictureEditorToCorrectSize[] { new PictureEditorToCorrectSize(game_settings, TempImages[0]), new PictureEditorToCorrectSize(game_settings, TempImages[1]), new PictureEditorToCorrectSize(game_settings, TempImages[2]) };
            Sky = new PictureEditorToCorrectSize(game_settings, TempImages[3]);
        }
    }
}
