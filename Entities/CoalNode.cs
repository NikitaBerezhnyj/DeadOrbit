using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities
{
    public class CoalNode : ResourceNode
    {
        public CoalNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 3, Color.DarkGray) { }
    }
}
