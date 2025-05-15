using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    internal class ImageGrabber
    {
        public ImageGrabber() { }

        public Image FindImg(string path) 
        {

            string[] formats = { "png", "apng", "jpeg", "jpg", "avif", "gif", "jfif", "pjpeg", "pjp", "svg", "webp", "bmp", "ico", "cur", "tif", "tiff" };

            foreach (string format in formats)
            {
                try
                {
                    return Image.FromFile($"{path}.{format}");
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
            }
            return null;
        }
    }
}
