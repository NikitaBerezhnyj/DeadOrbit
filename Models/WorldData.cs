using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities;
using DeadOrbit.Entities.Enemies;
using DeadOrbit.Entities.Structures;

namespace DeadOrbit.Models
{
    public class WorldData
    {
        public List<BaseStation> BaseStations;
        public Beacon Beacon;
        public List<ResourceNode> Resources;
        public List<Enemy> Enemies;
        public TileMap TileMap;

        public WorldData(
            List<BaseStation> bases,
            Beacon beacon,
            List<ResourceNode> resources,
            List<Enemy> enemies,
            TileMap tileMap
        )
        {
            BaseStations = bases;
            Beacon = beacon;
            Resources = resources;
            Enemies = enemies;
            TileMap = tileMap;
        }
    }
}
