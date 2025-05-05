using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class EscapeMenu
    {
        Panel menu = new Panel();
        public EscapeMenu(Settings game_settings, Panel given_panel)
        {
            menu = given_panel;
            menu.Size = new Size(game_settings.WIDTH / 3, (game_settings.HEIGHT / 5) * 3);
            menu.Location = new Point(game_settings.WIDTH / 3, game_settings.HEIGHT / 5);

            Font font = new Font("Serif", (int)(game_settings.HEIGHT / 200) * 5, FontStyle.Bold);


            Button Resume = new Button();
            Resume.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Resume.Location = new Point(menu.Width/14, menu.Width/10);
            //Resume.Click += (sender, e) => PauzeFunction();
            Resume.Text = "Continue";
            Resume.Font = font;
            menu.Controls.Add(Resume);

            Button setting_button = new Button();
            setting_button.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            setting_button.Location = new Point(menu.Width / 14, (menu.Width / 10)*4);
            setting_button.Text = "Settings";
            setting_button.Font = font;
            menu.Controls.Add(setting_button);

            Button Quit = new Button();
            Quit.Size = new Size((game_settings.WIDTH / 7) * 2, (game_settings.HEIGHT / 10));
            Quit.Location = new Point(menu.Width / 14, (menu.Width / 10)*7);
            //Quit.Click += (sender, e) => { clock.Stop(); Exit(); };
            Quit.Text = "Quit Game";
            Quit.Font = font;
            menu.Controls.Add(Quit);
        }
    }
}
