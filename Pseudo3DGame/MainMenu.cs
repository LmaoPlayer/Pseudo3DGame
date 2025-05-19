using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class MainMenu : MenuInh
    {
        public event EventHandler QuitClick;
        public event EventHandler SettingsClick;
        
        int CurrentMenuLayer;

        public SettingsMenu setting_menu;

        public MainMenu(Settings game_settings, Panel given_panel, Form1 form, Font font)
        {
            menu_screen = given_panel;
            menu_screen.BackColor = Color.Red;
            menu_screen.Size = new Size(game_settings.WIDTH / 3, (game_settings.HEIGHT / 5) * 3);
            menu_screen.Location = new Point(game_settings.WIDTH / 3, game_settings.HEIGHT / 5);



            Button Resume = new Button();
            Resume.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Resume.Location = new Point(menu_screen.Width/14, menu_screen.Width/10);
            //Resume.Click += (sender, e) => PauzeFunction();
            Resume.Text = "Continue";
            Resume.Font = font;
            Resume.Click += (sender, e) => setting_menu.RPMenu.EscapeTheMenu();
            Resume.BackColor = Color.White;
            menu_screen.Controls.Add(Resume);

            Button setting_button = new Button();
            setting_button.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            setting_button.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 10)*4);
            setting_button.Text = "Settings";
            setting_button.Font = font;
            setting_button.BackColor = Color.White;
            setting_button.Click += (sender, e) => SettingsClick?.Invoke(this, EventArgs.Empty);
            menu_screen.Controls.Add(setting_button);

            Button Quit = new Button();
            Quit.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Quit.Location = new Point(menu_screen.Width / 14, (menu_screen.Width / 10)*7);
            Quit.Click += (sender, e) => QuitClick?.Invoke(this, EventArgs.Empty);
            Quit.Text = "Quit Game";
            Quit.Font = font;
            Quit.BackColor = Color.White;
            menu_screen.Controls.Add(Quit);
            menu_screen.Hide();

            Panel setting_panel = new Panel() { Size = new Size(menu_screen.Width, menu_screen.Height), BackColor = Color.Blue, Location = new Point(menu_screen.Location.X, menu_screen.Location.Y) };
            //settings_menu = new SettingsMenu(game_settings, setting_panel);
            form.Controls.Add(setting_panel);
            setting_panel.Hide();
            setting_menu = new SettingsMenu(game_settings, setting_panel, form, font);


            foreach (Control control in menu_screen.Controls)
            {
                control.KeyDown += (sender, e) => setting_menu.RPMenu.EscapeTheMenu(e);
            }
        }
    }
}