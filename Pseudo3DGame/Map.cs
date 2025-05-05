using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    internal class Map
    {
        //0 is vloer, 1 is muur. Gekozen om makkelijk te kunnen onderscheiden
        public int[,] map =
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 3, 2, 3, 3, 0, 2, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };


        public Map(Settings setting)
        {
            Random rnd = new Random();
            int[,] tempMap = new int[setting.MAP_HEIGHT,setting.MAP_WIDTH];

            for (int i = 0; i < setting.MAP_HEIGHT; i++)
            {
                for (int j = 0; j < setting.MAP_WIDTH; j++)
                {
                    int t = rnd.Next(1, 4);


                    if (j == 0) tempMap[i, j] = t;
                    else if (i == 0) tempMap[i, j] = t;
                    else if (j == setting.MAP_WIDTH - 1) tempMap[i, j] = t;
                    else if (i == setting.MAP_HEIGHT - 1) tempMap[i, j] = t;
                    else if (j == 1 && i == 1) tempMap[i, j] = 0;
                    else if (j == 2 && i == 1) tempMap[i, j] = 0;
                    else if (j == 1 && i == 2) tempMap[i, j] = 0;
                    else if (j == 2 && i == 2) tempMap[i, j] = 0;
                    else
                    {
                        int tempNum = rnd.Next(0, 6);

                        if (tempNum == 5)
                        {
                            tempMap[i, j] = t;
                        } 
                    }
                }
            }

            this.map = tempMap;
            Console.WriteLine("Map gen done.");
        }
        
    }
}
