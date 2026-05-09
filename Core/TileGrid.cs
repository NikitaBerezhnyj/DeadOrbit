using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public static class TileGrid
    {
        public const int TileSize = 32;

        public static Vector2 ToWorld(int tileX, int tileY) =>
            new Vector2(tileX * TileSize, tileY * TileSize);

        public static Vector2 ToWorld(GridPosition pos) => ToWorld(pos.X, pos.Y);

        public static GridPosition ToGrid(Vector2 worldPos) =>
            new GridPosition((int)(worldPos.X / TileSize), (int)(worldPos.Y / TileSize));
    }
}
