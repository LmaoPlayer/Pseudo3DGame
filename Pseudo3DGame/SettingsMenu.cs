using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class SettingsMenu : MenuInh
    {
        public ResourcePacksMenu RPMenu;
        public event EventHandler OpenRPMenu;
        public event EventHandler ReturnFromSettings;
        public SettingsMenu(Settings game_settings, Panel given_panel, Form1 form, Font font)
        {
            menu_screen = given_panel;



            Button res_pack_settings = new Button();
            res_pack_settings.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            res_pack_settings.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 10) * 4);
            res_pack_settings.Text = "Resource Packs";
            res_pack_settings.Font = font;
            res_pack_settings.BackColor = Color.White;
            res_pack_settings.Click += (sender, e) => OpenRPMenu?.Invoke(this, EventArgs.Empty);
            menu_screen.Controls.Add(res_pack_settings);

            Button Back = new Button();
            Back.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Back.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 10)*7);
            Back.Click += (sender, e) => ReturnFromSettings?.Invoke(this, EventArgs.Empty);
            Back.Text = "Back";
            Back.Font = font;
            Back.BackColor = Color.White;
            menu_screen.Controls.Add(Back);
            menu_screen.Hide();



            Panel RPpanel = new Panel() { Size = new Size(menu_screen.Width, menu_screen.Height), BackColor = Color.Yellow, Location = new Point(menu_screen.Location.X, menu_screen.Location.Y) };
            form.Controls.Add(RPpanel);
            RPMenu = new ResourcePacksMenu(game_settings, RPpanel, form, font);
        }
    }
}
