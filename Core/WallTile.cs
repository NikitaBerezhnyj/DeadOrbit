using DeadOrbit.Interfaces;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public readonly struct WallTile : ICollidable
    {
        public Rectangle Bounds { get; }
        public bool BlocksMovement => true;

        public WallTile(int tileX, int tileY)
        {
            Bounds = new Rectangle(
                tileX * TileGrid.TileSize,
                tileY * TileGrid.TileSize,
                TileGrid.TileSize,
                TileGrid.TileSize
            );
        }
    }
}
