using DeadOrbit.Core;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Entities
{
    public class StoneNode : ResourceNode
    {
        public StoneNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 3, Color.DarkGray) { }

        public override InventoryItem GetDrop() =>
            new InventoryItem("Stone", ItemType.Resource, 1, Color.DarkGray, SpriteSourceMap.Stone);

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
                return;

            Vector2 offset = GetShakeOffset();

            var dest = new Rectangle(
                (int)(Position.X + offset.X),
                (int)(Position.Y + offset.Y),
                TileGrid.TileSize,
                TileGrid.TileSize
            );

            spriteBatch.Draw(GameResources.Tileset, dest, SpriteSourceMap.StoneNode, Color.White);
        }
    }
}
