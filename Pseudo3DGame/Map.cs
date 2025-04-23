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
            { 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
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
                    if (j == 0) tempMap[i, j] = 1;
                    else if (i == 0) tempMap[i, j] = 1;
                    else if (j == setting.MAP_WIDTH - 1) tempMap[i, j] = 1;
                    else if (i == setting.MAP_HEIGHT - 1) tempMap[i, j] = 1;
                    else if (j == 1 && i == 1) tempMap[i, j] = 0;
                    else
                    {
                        int tempNum = rnd.Next(0, 6);

                        if (tempNum == 5)
                        {
                            tempMap[i, j] = 1;
                        } 
                    }
                }
            }

            this.map = tempMap;
            Console.WriteLine("Map gen done.");
        }
        
    }
}
