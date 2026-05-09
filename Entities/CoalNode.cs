using DeadOrbit.Core;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities
{
    public class CoalNode : ResourceNode
    {
        public CoalNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 3, Color.DarkGray) { }

        public override InventoryItem GetDrop() =>
            new InventoryItem("Coal", ItemType.Resource, 1, Color.DarkGray);
    }
}
