using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class ResourcePacksMenu : MenuInh
    {
        public event EventHandler OpenRPFolder;
        public event EventHandler BackFromRP;
        public event EventHandler ApplyTextures;

        public CheckedListBox RPList { get; }
        public ResourcePacksMenu(Settings game_settings, Panel given_panel, Form1 form, Font font)
        {
            menu_screen = given_panel;
            Hide();


            Button OpenRPFolderButton = new Button();
            OpenRPFolderButton.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            OpenRPFolderButton.Location = new Point(menu_screen.Width / 14, menu_screen.Width / 10);
            //OpenRPFolder.Click += (sender, e) => PauzeFunction();
            OpenRPFolderButton.Text = "ResourcePack Folder";
            OpenRPFolderButton.Font = font;
            OpenRPFolderButton.Click += (sender, e) => OpenRPFolder?.Invoke(this, EventArgs.Empty);
            OpenRPFolderButton.BackColor = Color.White;
            menu_screen.Controls.Add(OpenRPFolderButton);


            Button Back = new Button();
            Back.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            Back.Location = new Point(menu_screen.Width / 2, menu_screen.Width / 10);
            //OpenRPFolder.Click += (sender, e) => PauzeFunction();
            Back.Text = "Return";
            Back.Font = font;
            Back.Click += (sender, e) => BackFromRP?.Invoke(this, EventArgs.Empty);
            Back.BackColor = Color.White;
            menu_screen.Controls.Add(Back);


            RPList = new CheckedListBox();
            RPList.Size = new Size((game_settings.WIDTH / 7) * 2, ((game_settings.HEIGHT / 40)*8));
            RPList.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 40) * 13);
            RPList.Font = font;
            RPList.BackColor = Color.White;
            menu_screen.Controls.Add(RPList);

            Button ApplyRP = new Button();
            ApplyRP.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            ApplyRP.Location = new Point(menu_screen.Width / 2, (menu_screen.Width / 10) * 7);
            ApplyRP.Font = font;
            ApplyRP.Text = "Apply Resources";
            ApplyRP.Click += (sender, e) => ApplyTextures?.Invoke(this, EventArgs.Empty);
            ApplyRP.BackColor = Color.White;
            menu_screen.Controls.Add(ApplyRP);


            Button Refresh = new Button();
            Refresh.Size = new Size((game_settings.WIDTH / 7), (game_settings.HEIGHT / 10));
            Refresh.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 10) * 7);
            Refresh.Font = font;
            Refresh.Text = "Refresh List";
            Refresh.Click += (sender, e) => SetupRPList();
            Refresh.BackColor = Color.White;
            menu_screen.Controls.Add(Refresh);
        }

        public void SetupRPList()
        {
            RPList.Items.Clear();

            string[] dir = Directory.GetDirectories("ResourcePacks");

            foreach (string Pack in dir)
            {
                string temp = Pack.Split('\\')[1];
                RPList.Items.Add(temp);
            }
        }
    }
}
