using Pseudo3DGame.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pseudo3DGame
{
    internal class Map
    {
        public List<int[]> PossibleSpawnLocations;
        Settings setting;
        public bool IsFinished = false;
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
            IsFinished = false;
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
                    //if (j == 1 && i == 1) tempMap[i, j] = 0;
                    //else if (j == 2 && i == 1) tempMap[i, j] = 0;
                    //else if (j == 1 && i == 2) tempMap[i, j] = 0;
                    //else if (j == 2 && i == 2) tempMap[i, j] = 0;
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
            IsFinished = true;
        }

        public void GenerateWithFile(string[] map_to_use)
        {
            IsFinished = false;
            List<int[]> temp_map = new List<int[]>();
            List<int> temp_row = new List<int>();


            char SplitOn = ',';
            if (map_to_use[0].Contains(";")) SplitOn = ';';
            else if (map_to_use[0].Contains("/")) SplitOn = '/';
            if (map_to_use[0].Contains("\\")) SplitOn = '\\';


            for (int i = 1; i < map_to_use.GetLength(0); i++)
            {
                string[] rowSTRListNeg1;
                string[] rowSTRList0;
                string[] rowSTRList1;

                


                if (i > 1 && i < map_to_use.GetLength(0)) rowSTRListNeg1 = map_to_use[i - 2].Split(SplitOn);
                else rowSTRListNeg1 = new string[0];

                if (i != 0 && i != map_to_use.GetLength(0) - 1) rowSTRList0 = map_to_use[i - 1].Split(SplitOn);
                else rowSTRList0 = new string[0];

                if (i < map_to_use.GetLength(0) - 2) rowSTRList1 = map_to_use[i].Split(SplitOn);
                else rowSTRList1 = new string[0];


                //if (i == map_to_use.GetLength(0) + 1)
                //{
                //    int breakpoint = 0;
                //}

                for (int j = 0; j < Math.Max(Math.Max(rowSTRList1.Length, rowSTRList0.Length), rowSTRListNeg1.Length); j++)
                {
                    int temp = 0;

                    if (rowSTRList0.Length > j)
                    {
                        if (int.TryParse(rowSTRList0[j], out temp))
                        {
                            if ((i == 1 || j == 0 || i == map_to_use.GetLength(0) || j == Math.Max(Math.Max(rowSTRList1.Length, rowSTRList0.Length), rowSTRListNeg1.Length)) && temp == 0) temp_row.Add(1);
                            else temp_row.Add(temp);
                        }
                        else temp_row.Add(1);
                    }
                    else temp_row.Add(1);
                }
                temp_map.Add(temp_row.ToArray());
                temp_row.Clear();
            }


            int x = 0;
            
            for (int i = 0; i < temp_map.Count; i++)
            {
                if (x < temp_map[i].Length) x = temp_map[i].Length;
            }

            setting.ChangedMap(x, map_to_use.GetLength(0));

            map = new int[temp_map.Count, x];


            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    try
                    {
                        map[i, j] = temp_map[i][j];
                    }
                    catch
                    {
                        map[i, j] = 1;
                    }
                }
            }


            IsFinished = true;
        }

        public void SaveMap()
        {
            int Again = 1;
            
            for (int i = 0; i < Again; i++)
            {
                if (File.Exists($"Maps/Map{i + 1}.csv"))
                {
                    Again ++;
                }
                else
                {
                    WriteMap($"Maps/Map{i + 1}.csv");
                    MessageBox.Show("Finished");
                    break;

                    
                }
            }
        }
        public void WriteMap(string path)
        {
            List<string> tempMap = new List<string>();
            string tempRow = "";
            for (int i = 0; i < map.GetLength(0); i++)
            {

                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (j != map.GetLength(1) - 1) tempRow += $"{map[i, j]},";
                    else tempRow += $"{map[i, j]}";
                }
                tempMap.Add(tempRow);
                tempRow = "";
            }
            File.WriteAllLines(path, tempMap);
        }
    }
}
