using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    //Zelfs met serieuze namen moet er minstens 1 grote zijn
    internal class PictureEditorToCorrectSize
    {
        Bitmap CorrectImg;
        public PictureEditorToCorrectSize(Settings setting, Image img)
        {
            Bitmap input = new Bitmap(img);
            Bitmap temp = new Bitmap(setting.TEXTURE_SIZE, setting.TEXTURE_SIZE);
            
            int pixel_scale_width = (int)img.Width / setting.TEXTURE_SIZE;
            int pixel_scale_height = (int)img.Width / setting.TEXTURE_SIZE;

            List<Color> colors = new List<Color>();

            for (int i = 0;  i < setting.TEXTURE_SIZE; i++)
            {
                for (int j = 0; j < setting.TEXTURE_SIZE; j++)
                {
                    

                    for (int k = 0; k < pixel_scale_height; k++)
                    {
                        for (int l = 0; l < pixel_scale_width; l++)
                        {
                            int x = (i * pixel_scale_height) + l;
                            int y = (j * pixel_scale_width) + k;

                            if (x > temp.Width || y > temp.Height) break;
                            colors.Add(input.GetPixel(x, y));
                        }
                    }

                    temp.SetPixel(i, j, AverageColor(colors));

                    colors.Clear();
                }
            }

            this.CorrectImg = temp;
        }

        private Color AverageColor(List<Color> colors)
        {
            int count = colors.Count;
            if (count == 0) return Color.Transparent;

            int sumR = 0, sumG = 0, sumB = 0, sumA = 0;

            foreach (Color c in colors)
            {
                sumR += c.R;
                sumG += c.G;
                sumB += c.B;
                sumA += c.A;
            }

            return Color.FromArgb(
                sumA / count,
                sumR / count,
                sumG / count,
                sumB / count
            );
        }
        public Bitmap GetBMP()
        {
            return CorrectImg;
        }
    }
}
