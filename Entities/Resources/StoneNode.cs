using DeadOrbit.Core;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class StoneNode : ResourceNode
    {
        public StoneNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 5, Color.SlateGray) { }

        public override InventoryItem GetDrop() =>
            new InventoryItem(
                "Stone",
                ItemType.Resource,
                1,
                Color.SlateGray,
                SpriteSourceMap.Stone
            );

        protected override Rectangle? GetSpriteSource() => SpriteSourceMap.StoneNode;
    }
}
