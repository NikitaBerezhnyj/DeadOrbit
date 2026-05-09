using System.Collections.Generic;
using DeadOrbit.Entities;

namespace DeadOrbit.Data
{
    public class WorldData
    {
        public List<BaseStation> BaseStations;
        public Beacon Beacon;
        public List<ResourceNode> Resources;

        public WorldData(List<BaseStation> bases, Beacon beacon, List<ResourceNode> resources)
        {
            BaseStations = bases;
            Beacon = beacon;
            Resources = resources;
        }
    }
}
