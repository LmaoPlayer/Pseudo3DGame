using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class SettingsMenu : MenuInh
    {
        public SettingsMenu(Settings game_settings, Panel given_panel)
        {
            menu_screen = given_panel;
        }
    }
}
