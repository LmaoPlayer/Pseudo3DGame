using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    //Zelfs met serieuze namen moet er minstens 1 grote zijn
    internal class PictureEditorToCorrectSize
    {
        Bitmap CorrectImg;
        public PictureEditorToCorrectSize(Settings setting, Image img)
        {
            this.CorrectImg = new Bitmap(img, setting.TEXTURE_SIZE, setting.TEXTURE_SIZE);
        }
        public Bitmap GetBMP()
        {
            return CorrectImg;
        }
    }
}
