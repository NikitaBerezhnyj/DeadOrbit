using DeadOrbit.Core;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities
{
    public class Beetle : Enemy
    {
        public Beetle(int tileX, int tileY)
            : base(tileX, tileY, hp: 4, damage: 1, speed: 60f, aggroRange: TileGrid.TileSize * 5f)
        { }

        public override InventoryItem GetDrop() =>
            new InventoryItem("Web", ItemType.Resource, 1, Color.WhiteSmoke);

        protected override Color GetBodyColor() => Color.OrangeRed;
    }
}
