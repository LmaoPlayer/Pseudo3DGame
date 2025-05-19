using Pseudo3DGame.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseudo3DGame
{
    internal class Map
    {
        public List<int[]> PossibleSpawnLocations;
        Settings setting;
        //0 is vloer, 1 is muur. Gekozen om makkelijk te kunnen onderscheiden
        public int[,] map =
        {
            { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 3, 2, 3, 3, 0, 2, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 0, 0, 0, 0, 2, 0, 1, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0 },
            { 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 }
        };


        public Map(Settings setting)
        {
            this.setting = setting;
            GenerateRandom();
        }

        public void GenerateRandom()
        {
            PossibleSpawnLocations = new List<int[]>();
            Random rnd = new Random();
            int[,] tempMap = new int[setting.MAP_HEIGHT, setting.MAP_WIDTH];

            for (int i = 0; i < setting.MAP_HEIGHT; i++)
            {
                for (int j = 0; j < setting.MAP_WIDTH; j++)
                {
                    int t = rnd.Next(1, 4);


                    if (j == 0) tempMap[i, j] = t;
                    else if (i == 0) tempMap[i, j] = t;
                    else if (j == setting.MAP_WIDTH - 1) tempMap[i, j] = t;
                    else if (i == setting.MAP_HEIGHT - 1) tempMap[i, j] = t;
                    if (j == 1 && i == 1) tempMap[i, j] = 0;
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
                        else
                        {
                            PossibleSpawnLocations.Add(new int[] { i, j });
                        }
                    }
                }
            }
            this.map = tempMap;
            Console.WriteLine("Map gen done.");
        }

        public void GenerateWithFile(string[] map_to_use)
        {

            for (int i = 0; i < map_to_use.Length;  i++)
            {
                
            }


            map = new int[map_to_use.GetLength(0) + 2, map_to_use.GetLength(0) + 2];


        }
    }
}
