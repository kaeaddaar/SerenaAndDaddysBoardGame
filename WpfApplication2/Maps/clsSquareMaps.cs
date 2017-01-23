using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using System.Windows.Media;

using System.IO;


namespace appGameBoardTest
{   

    public class clsSquareMaps
    {
        public static Tile getTile(Point3D p3D, Brush B)
        {
            Tile tmpTile = new Tile(appGameBoardTest.clsModels.GetGeo(p3D, B));
            tmpTile.p3D = p3D;
            tmpTile.B = B;
            return tmpTile;
        }

        public static List<Tile> File_Main()
        {
            // string path = @"C:\Docs\_Projects\GameBoard\Map.txt";
            string path = @"..\..\Map.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                    sw.WriteLine("DLDLDLDLDLDLDLDLDLDL");
                    sw.WriteLine("LDLDLDLDLDLDLDLDLDLD");
                }
            }

            int lineNum = 0;
            var locs = new List<Tile>();

            // Open the file to read from. 
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    lineNum = lineNum + 1;
                    //Console.WriteLine(s);
                    int ColCount = 0;
                    foreach (var ch in s)
                    {
                        ColCount++;
                        if (""+ch == "L")
                        {
                            locs.Add(getTile(new Point3D(ColCount, lineNum, 0), Brushes.LightGray));
                        }
                        else if (""+ch == "D")
                        {
                            locs.Add(getTile(new Point3D(ColCount, lineNum, 0), Brushes.DarkGray));
                        }

                    }
                }
            }
            return locs;
        }

        public static List<Tile> Board20x20()
        {
            var locs = new List<Tile>();
            int colour = 0;

            for (int i = 1; i <= 20; i++)
            {
                for (int j = 1; j <= 20; j++)
                {
                    colour = colour + 1;
                    if (((colour + (i % 2)) % 2) == 0)
                    {
                        locs.Add(getTile(new Point3D(j, i, 0), Brushes.LightGray));
                    }
                    else
                    {
                        locs.Add(getTile(new Point3D(j, i, 0), Brushes.DarkGray));
                    }

                    if (colour >= 20) { colour = 0; }
                }
            }
            return locs;
        }

        public static List<Tile> BoardXbyY(int countX, int countY)
        {
            var locs = new List<Tile>();
            int colour = 0;

            for (int i = 1; i <= countY; i++)
            {
                for (int j = 1; j <= countX; j++)
                {
                    colour = colour + 1;
                    if (((colour + (i % 2)-(countX % 2)) % 2) == 0)
                    {
                        locs.Add(getTile(new Point3D(j, i, 0), Brushes.LightGray));
                    }
                    else
                    {
                        locs.Add(getTile(new Point3D(j, i, 0), Brushes.DarkGray));
                    }

                    if (colour >= countX) { colour = 0; }
                }
            }
            return locs;
        }

        public static List<Tile> chessBoard()
        {
            var locs = new List<Tile>();
            
            locs.Add(getTile(new Point3D(1, 1, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(2, 1, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(3, 1, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(4, 1, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(5, 1, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(6, 1, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(7, 1, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(8, 1, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(9, 1, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(10, 1, 0), Brushes.DarkGray));

            locs.Add(getTile(new Point3D(1, 2, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(2, 2, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(3, 2, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(4, 2, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(5, 2, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(6, 2, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(7, 2, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(8, 2, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(9, 2, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(10, 2, 0), Brushes.LightGray));

            locs.Add(getTile(new Point3D(1, 3, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(2, 3, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(3, 3, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(4, 3, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(5, 3, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(6, 3, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(7, 3, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(8, 3, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(9, 3, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(10, 3, 0), Brushes.DarkGray));

            locs.Add(getTile(new Point3D(1, 4, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(2, 4, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(3, 4, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(4, 4, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(5, 4, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(6, 4, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(7, 4, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(8, 4, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(9, 4, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(10, 4, 0), Brushes.LightGray));

            locs.Add(getTile(new Point3D(1, 5, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(2, 5, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(3, 5, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(4, 5, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(5, 5, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(6, 5, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(7, 5, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(8, 5, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(9, 5, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(10, 5, 0), Brushes.DarkGray));

            locs.Add(getTile(new Point3D(1, 6, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(2, 6, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(3, 6, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(4, 6, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(5, 6, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(6, 6, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(7, 6, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(8, 6, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(9, 6, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(10, 6, 0), Brushes.LightGray));

            locs.Add(getTile(new Point3D(1, 7, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(2, 7, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(3, 7, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(4, 7, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(5, 7, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(6, 7, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(7, 7, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(8, 7, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(9, 7, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(10, 7, 0), Brushes.DarkGray));

            locs.Add(getTile(new Point3D(1, 8, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(2, 8, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(3, 8, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(4, 8, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(5, 8, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(6, 8, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(7, 8, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(8, 8, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(9, 8, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(10, 8, 0), Brushes.LightGray));

            locs.Add(getTile(new Point3D(1, 9, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(2, 9, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(3, 9, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(4, 9, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(5, 9, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(6, 9, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(7, 9, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(8, 9, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(9, 9, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(10, 9, 0), Brushes.DarkGray));

            locs.Add(getTile(new Point3D(1, 10, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(2, 10, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(3, 10, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(4, 10, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(5, 10, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(6, 10, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(7, 10, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(8, 10, 0), Brushes.LightGray));
            locs.Add(getTile(new Point3D(9, 10, 0), Brushes.DarkGray));
            locs.Add(getTile(new Point3D(10, 10, 0), Brushes.LightGray));

            return locs;
        }
    }
 
}
