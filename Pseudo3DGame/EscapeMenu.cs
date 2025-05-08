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
    internal class EscapeMenu
    {


        Panel menu = new Panel();
        
        public event EventHandler ResumeClick;
        public event EventHandler QuitClick;
        public event EventHandler SettingsClick;
        
        bool pause;

        SettingsMenu settings_menu;
        Panel setting_panel;

        public EscapeMenu(Settings game_settings, Panel given_panel)
        {
            menu = given_panel;
            menu.BackColor = Color.Red;
            menu.Size = new Size(game_settings.WIDTH / 3, (game_settings.HEIGHT / 5) * 3);
            menu.Location = new Point(game_settings.WIDTH / 3, game_settings.HEIGHT / 5);


            Font font = new Font("Serif", (int)(game_settings.HEIGHT / 200) * 5, FontStyle.Bold);


            Button Resume = new Button();
            Resume.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Resume.Location = new Point(menu.Width/14, menu.Width/10);
            //Resume.Click += (sender, e) => PauzeFunction();
            Resume.Text = "Continue";
            Resume.Font = font;
            Resume.Click += (sender, e) => ResumeClick.Invoke(this, EventArgs.Empty);
            Resume.BackColor = Color.White;
            menu.Controls.Add(Resume);

            Button setting_button = new Button();
            setting_button.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            setting_button.Location = new Point(menu.Width / 14, (menu.Width / 10)*4);
            setting_button.Text = "Settings";
            setting_button.Font = font;
            setting_button.KeyDown += (sender, e) => { if (e.KeyCode == Keys.Escape) ResumeClick.Invoke(this, EventArgs.Empty); };
            setting_button.BackColor = Color.White;
            //setting_button.Click += (sender, e) => SettingsClick.Invoke(this, EventArgs.Empty);
            menu.Controls.Add(setting_button);

            Button Quit = new Button();
            Quit.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Quit.Location = new Point(menu.Width / 14, (menu.Width / 10)*7);
            Quit.Click += (sender, e) => QuitClick.Invoke(this, EventArgs.Empty);
            Quit.Text = "Quit Game";
            Quit.Font = font;
            Quit.BackColor = Color.White;
            menu.Controls.Add(Quit);
            menu.Hide();

            setting_panel = new Panel() { Size = new Size(menu.Width, menu.Height), Location = new Point(menu.Location.X, menu.Location.Y), BackColor = menu.BackColor };
            settings_menu = new SettingsMenu(game_settings, setting_panel);

        }

        public void PauzeInvoke(bool pause)
        {
            if (pause) menu.Show();
            else menu.Hide();

            this.pause = pause;
        }

        public void SettingsInvoke(bool settingsOpen)
        {
            if (settingsOpen) menu.Hide();
            else menu.Show();
        }
    }
}