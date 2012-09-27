using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{

    public static class TileTypes
    {
        public static int
            Wall = 0,
            Floor = 1;
    }

    internal class CavePackage
    {
        public int[][] m_Cave1;
        public int[][] m_Cave2;

        public int
            r1_Cuttoff = 10,
            r2_Cuttoff = 10,
            sizeX,
            sizeY,
            fillProb = 45,
            Seed = DateTime.Now.Millisecond;

        public Random r;

    }

    //http://roguebasin.roguelikedevelopment.org/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
    public static class CaveGenerator
    {

        public static int[][] Generate(int w, int h, int itt)
        {
            CavePackage pkg = new CavePackage();

            pkg.sizeX = w; 
            pkg.sizeY = h;
            pkg.r = new Random(pkg.Seed);

            Initialization(pkg);
            
            for(int i = 0; i < itt; ++i)
                Itterate(pkg);

            return pkg.m_Cave1;
        }

        private static void Initialization(CavePackage pkg)
        {
            int 
                xi = 0,
                yi = 0;
            pkg.m_Cave1 = new int[pkg.sizeY][];
            pkg.m_Cave2 = new int[pkg.sizeY][];
            for (yi = 0; yi < pkg.sizeY; ++yi)
            {
                pkg.m_Cave1[yi] = new int[pkg.sizeX];
                pkg.m_Cave2[yi] = new int[pkg.sizeX];
            }
            for (yi = 1; yi < pkg.sizeY - 1; ++yi)
                for (xi = 1; xi < pkg.sizeX - 1; ++xi)
                    pkg.m_Cave1[yi][xi] = randPick(pkg);

            for (yi = 1; yi < pkg.sizeY; ++yi)
                for (xi = 1; xi < pkg.sizeX; ++xi)
                    pkg.m_Cave2[yi][xi] = TileTypes.Wall;

            for (yi = 0; yi < pkg.sizeY; ++yi)
                pkg.m_Cave1[yi][0] = TileTypes.Wall;
            for (xi = 0; xi < pkg.sizeX; ++xi)
                pkg.m_Cave1[0][xi] = TileTypes.Wall;
        }

        private static void Itterate(CavePackage pkg)
        {
            int xi, yi, ii, jj;

            for (yi = 1; yi < pkg.sizeY - 1; yi++)
                for (xi = 1; xi < pkg.sizeX - 1; xi++)
                {
                    int adjcount_r1 = 0,
                        adjcount_r2 = 0;

                    for (ii = -1; ii <= 1; ii++)
                        for (jj = -1; jj <= 1; jj++)
                        {
                            if (pkg.m_Cave1[yi + ii][xi + jj] != TileTypes.Floor)
                                adjcount_r1++;
                        }
                    for (ii = yi - 2; ii <= yi + 2; ii++)
                        for (jj = xi - 2; jj <= xi + 2; jj++)
                        {
                            if (Math.Abs(ii - yi) == 2 && Math.Abs(jj - xi) == 2)
                                continue;
                            if (ii < 0 || jj < 0 || ii >= pkg.sizeY || jj >= pkg.sizeX)
                                continue;
                            if (pkg.m_Cave1[ii][jj] != TileTypes.Floor)
                                adjcount_r2++;
                        }
                    if (adjcount_r1 >= pkg.r1_Cuttoff || adjcount_r2 <= pkg.r2_Cuttoff)
                        pkg.m_Cave2[yi][xi] = TileTypes.Wall;
                    else
                        pkg.m_Cave2[yi][xi] = TileTypes.Floor;
                }
            for (yi = 1; yi < pkg.sizeY - 1; yi++)
                for (xi = 1; xi < pkg.sizeX - 1; xi++)
                    pkg.m_Cave1[yi][xi] = pkg.m_Cave2[yi][xi];

        }

        private static int randPick(CavePackage pkg)
        {
            if (pkg.r.Next(100) < pkg.fillProb)
                return TileTypes.Wall;
            else 
                return TileTypes.Floor;
        }

    }
}
