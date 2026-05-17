using DeadOrbit.Core;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities.Enemies
{
    public class Crawler : Enemy
    {
        public Crawler(int tileX, int tileY)
            : base(tileX, tileY, hp: 8, damage: 2, speed: 35f, aggroRange: TileGrid.TileSize * 7f)
        {
            AttackCooldown = 2.0f;
        }

        public override InventoryItem GetDrop() =>
            new InventoryItem("Slime", ItemType.Resource, 1, Color.GreenYellow);

        protected override Color GetBodyColor() => Color.MediumPurple;
    }
}
