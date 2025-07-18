﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class MapMenu : MenuInh
    {
        public event EventHandler OpenMapFolder;
        public event EventHandler ApplyMapEvent;
        public event EventHandler EscapeKeyPressedMaps;
        public event EventHandler SaveMap;

        public TextBox WidthTXT;
        public TextBox HeightTXT;

        public CheckedListBox MapList { get; }
        public MapMenu(Settings game_settings, Panel given_panel, Form1 form, Font font)
        {
            menu_screen = given_panel;
            Hide();

            Button OpenMapFolderButton = new Button();
            OpenMapFolderButton.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            OpenMapFolderButton.Location = new Point(menu_screen.Width / 14, menu_screen.Width / 10);
            //OpenRPFolder.Click += (sender, e) => PauzeFunction();
            OpenMapFolderButton.Text = "Maps Folder";
            OpenMapFolderButton.Font = font;
            OpenMapFolderButton.Click += (sender, e) => OpenMapFolder?.Invoke(this, EventArgs.Empty);
            OpenMapFolderButton.BackColor = Color.White;
            menu_screen.Controls.Add(OpenMapFolderButton);


            Button Back = new Button();
            Back.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            Back.Location = new Point(menu_screen.Width / 2, menu_screen.Width / 10);
            //OpenRPFolder.Click += (sender, e) => PauzeFunction();
            Back.Text = "Return";
            Back.Font = font;
            Back.Click += (sender, e) => EscapeKeyPressedMaps?.Invoke(this, EventArgs.Empty);
            Back.BackColor = Color.White;
            menu_screen.Controls.Add(Back);


            MapList = new CheckedListBox();
            MapList.Size = new Size((game_settings.WIDTH / 7) * 2, ((game_settings.HEIGHT / 40) * 8));
            MapList.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 40) * 13);
            MapList.Font = font;
            MapList.BackColor = Color.White;
            menu_screen.Controls.Add(MapList);

            Button ApplyMap = new Button();
            ApplyMap.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            ApplyMap.Location = new Point(menu_screen.Width / 2, (menu_screen.Width / 5) * 3);
            ApplyMap.Font = font;
            ApplyMap.Text = "Apply Map";
            ApplyMap.Click += (sender, e) => ApplyMapEvent?.Invoke(this, EventArgs.Empty);
            ApplyMap.BackColor = Color.White;
            menu_screen.Controls.Add(ApplyMap);


            Button Refresh = new Button();
            Refresh.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            Refresh.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 5) * 3);
            Refresh.Font = font;
            Refresh.Text = "Refresh List";
            Refresh.Click += (sender, e) => SetupMapList();
            Refresh.BackColor = Color.White;
            menu_screen.Controls.Add(Refresh);



            Button Save = new Button();
            Save.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            Save.Location = new Point(menu_screen.Width / 14, (menu_screen.Width/5)*4);
            Save.Font = font;
            Save.Text = "Save Current Map";
            Save.Click += (sender, e) => SaveMap?.Invoke(this, EventArgs.Empty);
            Save.BackColor = Color.White;
            menu_screen.Controls.Add(Save);

            WidthTXT = new TextBox();
            WidthTXT.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 20));
            WidthTXT.Location = new Point(menu_screen.Width / 2, (menu_screen.Width / 5) * 4);
            WidthTXT.Font = font;
            WidthTXT.Text = "Set Width";
            WidthTXT.BackColor = Color.White;
            menu_screen.Controls.Add(WidthTXT);


            HeightTXT = new TextBox();
            HeightTXT.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 20));
            HeightTXT.Location = new Point(menu_screen.Width / 2, (menu_screen.Width / 8) * 7);
            HeightTXT.Font = font;
            HeightTXT.Text = "Set Height";
            HeightTXT.BackColor = Color.White;
            menu_screen.Controls.Add(HeightTXT);


            foreach (Control control in menu_screen.Controls)
            {
                control.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Escape) EscapeKeyPressedMaps?.Invoke(this, EventArgs.Empty); };
            }
        }

        public void SetupMapList()
        {
            MapList.Items.Clear();

            string[] dir = Directory.GetFiles("Maps");

            MapList.Items.Add("Random");

            foreach (string Pack in dir)
            {
                string temp = Pack.Split('\\')[1];
                temp = temp.Split('.')[0];
                MapList.Items.Add(temp);
            }
        }
    }
}
