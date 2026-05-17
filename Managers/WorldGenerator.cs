using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities;
using DeadOrbit.Entities.Enemies;
using DeadOrbit.Entities.Structures;
using DeadOrbit.Models;

namespace DeadOrbit.Managers
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

            var tileMap = GenerateTileMap(rnd);
            var baseStations = GenerateBaseStations(rnd, usedTiles);
            var resources = GenerateResources(rnd, usedTiles);
            var enemies = GenerateEnemies(rnd, usedTiles);
            var beacon = new Beacon(WorldWidth / 2, 1);

            return new WorldData(baseStations, beacon, resources, enemies, tileMap);
        }

        private static int GetGroundVariant(Random rnd)
        {
            int roll = rnd.Next(100);
            return roll switch
            {
                < 85 => 0,
                < 90 => 1,
                < 95 => 2,
                _ => 3,
            };
        }

        private static TileMap GenerateTileMap(Random rnd)
        {
            var map = new TileMap(WorldWidth, WorldHeight);

            for (int x = 0; x < WorldWidth; x++)
            for (int y = 0; y < WorldHeight; y++)
                map.Set(x, y, TileType.Ground, GetGroundVariant(rnd));

            for (int x = 0; x < WorldWidth; x++)
            {
                map.Set(x, 0, TileType.Wall, rnd.Next(4));
                map.Set(x, WorldHeight - 1, TileType.Wall, rnd.Next(4));
            }
            for (int y = 0; y < WorldHeight; y++)
            {
                map.Set(0, y, TileType.Wall, rnd.Next(4));
                map.Set(WorldWidth - 1, y, TileType.Wall, rnd.Next(4));
            }

            return map;
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
                Enemy enemy = rnd.Next(100) < 60 ? new Beetle(x, y) : new Crawler(x, y);
                enemies.Add(enemy);
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
