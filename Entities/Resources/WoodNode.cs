using DeadOrbit.Core;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities
{
    public class WoodNode : ResourceNode
    {
        public WoodNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 3, Color.SaddleBrown) { }

        public override InventoryItem GetDrop() =>
            new InventoryItem("Wood", ItemType.Resource, 1, Color.SandyBrown, SpriteSourceMap.Wood);
    }
}
