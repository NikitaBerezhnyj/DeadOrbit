using System.Collections.Generic;
using DeadOrbit.Entities;

namespace DeadOrbit.Data
{
    public class WorldData
    {
        public List<BaseStation> BaseStations;
        public Beacon Beacon;

        public WorldData(List<BaseStation> bases, Beacon beacon)
        {
            BaseStations = bases;
            Beacon = beacon;
        }
    }
}
