using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class MenuInh
    {
        public Panel menu_screen;
        public MenuInh()
        {

        }

        public void Show()
        {
            menu_screen.Show();
        }
        public void Hide()
        {
            menu_screen.Hide();
        }
    }
}
