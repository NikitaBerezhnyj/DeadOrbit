using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities
{
    public class WoodNode : ResourceNode
    {
        public WoodNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 3, Color.SaddleBrown) { }
    }
}
