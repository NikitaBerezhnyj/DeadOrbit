using DeadOrbit.Models;
using DeadOrbit.Rendering;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities.Resources
{
    public class WoodNode : ResourceNode
    {
        public WoodNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 3, Color.SaddleBrown) { }

        public override InventoryItem GetDrop() =>
            new InventoryItem("Wood", ItemType.Resource, 1, Color.SandyBrown, SpriteSourceMap.Wood);

        protected override Rectangle? GetSpriteSource() => null;
    }
}
