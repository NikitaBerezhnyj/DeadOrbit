using System;
using System.Collections.Generic;
using DeadOrbit.Data;
using DeadOrbit.Entities;

namespace DeadOrbit.World
{
    public static class WorldGenerator
    {
        public static WorldData Generate(int seed)
        {
            var rnd = new Random(seed);
            var baseStations = new List<BaseStation>();

            var usedTiles = new HashSet<(int, int)>();

            for (int i = 0; i < 3; i++)
            {
                int tx,
                    ty;
                do
                {
                    tx = rnd.Next(2, 20);
                    ty = rnd.Next(2, 12);
                } while (usedTiles.Contains((tx, ty)));

                usedTiles.Add((tx, ty));
                baseStations.Add(new BaseStation(tx, ty));
            }

            var beacon = new Beacon(12, 1);

            var resources = new List<ResourceNode>();
            for (int i = 0; i < 8; i++)
            {
                int tx,
                    ty;
                do
                {
                    tx = rnd.Next(1, 22);
                    ty = rnd.Next(1, 14);
                } while (usedTiles.Contains((tx, ty)));

                usedTiles.Add((tx, ty));

                if (rnd.Next(2) == 0)
                    resources.Add(new CoalNode(tx, ty));
                else
                    resources.Add(new StoneNode(tx, ty));
            }

            return new WorldData(baseStations, beacon, resources);
        }
    }
}
