using System;
using System.Collections.Generic;
using DeadOrbit.Data;
using DeadOrbit.Entities;
using Microsoft.Xna.Framework;

namespace DeadOrbit.World
{
    public static class WorldGenerator
    {
        public static WorldData Generate(int seed)
        {
            var rnd = new Random(seed);

            var baseStations = new List<BaseStation>();

            for (int i = 0; i < 3; i++)
            {
                baseStations.Add(
                    new BaseStation(new Vector2(rnd.Next(50, 700), rnd.Next(50, 400)))
                );
            }

            var beacon = new Beacon(new Vector2(380, 20));

            return new WorldData(baseStations, beacon);
        }
    }
}
