using System;
using System.Collections.Generic;
using DeadOrbit.Data;
using DeadOrbit.Entities;

namespace DeadOrbit.World
{
    public static class WorldGenerator
    {
        private const int WorldWidth = 100;
        private const int WorldHeight = 100;

        private const int BorderPadding = 2;

        private const int BaseStationCount = 5;
        private const int ResourceCount = 40;
        private const int EnemyCount = 20;

        public static WorldData Generate(int seed)
        {
            var rnd = new Random(seed);

            var usedTiles = new HashSet<(int x, int y)>();

            var baseStations = GenerateBaseStations(rnd, usedTiles);
            var resources = GenerateResources(rnd, usedTiles);
            var enemies = GenerateEnemies(rnd, usedTiles);

            var beacon = new Beacon(WorldWidth / 2, 1);

            return new WorldData(baseStations, beacon, resources, enemies);
        }

        private static List<BaseStation> GenerateBaseStations(
            Random rnd,
            HashSet<(int x, int y)> usedTiles
        )
        {
            var stations = new List<BaseStation>();

            for (int i = 0; i < BaseStationCount; i++)
            {
                var (x, y) = GetFreeTile(rnd, usedTiles);

                stations.Add(new BaseStation(x, y));
            }

            return stations;
        }

        private static List<ResourceNode> GenerateResources(
            Random rnd,
            HashSet<(int x, int y)> usedTiles
        )
        {
            var resources = new List<ResourceNode>();

            for (int i = 0; i < ResourceCount; i++)
            {
                var (x, y) = GetFreeTile(rnd, usedTiles);

                int roll = rnd.Next(100);

                ResourceNode resource = roll switch
                {
                    < 20 => new CoalNode(x, y),
                    < 50 => new StoneNode(x, y),
                    _ => new WoodNode(x, y),
                };

                resources.Add(resource);
            }

            return resources;
        }

        private static List<Enemy> GenerateEnemies(Random rnd, HashSet<(int x, int y)> usedTiles)
        {
            var enemies = new List<Enemy>();

            for (int i = 0; i < EnemyCount; i++)
            {
                var (x, y) = GetFreeTile(rnd, usedTiles);

                enemies.Add(new Beetle(x, y));
            }

            return enemies;
        }

        private static (int x, int y) GetFreeTile(Random rnd, HashSet<(int x, int y)> usedTiles)
        {
            int x;
            int y;

            do
            {
                x = rnd.Next(BorderPadding, WorldWidth - BorderPadding);
                y = rnd.Next(BorderPadding, WorldHeight - BorderPadding);
            } while (usedTiles.Contains((x, y)));

            usedTiles.Add((x, y));

            return (x, y);
        }
    }
}
