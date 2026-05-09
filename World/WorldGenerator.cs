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

                int roll = rnd.Next(100);

                ResourceNode resource = roll switch
                {
                    < 20 => new CoalNode(tx, ty),
                    < 50 => new StoneNode(tx, ty),
                    _ => new WoodNode(tx, ty),
                };

                resources.Add(resource);
            }

            return new WorldData(baseStations, beacon, resources);
        }
    }
}
