using System;
using System.Collections.Generic;
using System.Drawing;
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
        }
    }
}
