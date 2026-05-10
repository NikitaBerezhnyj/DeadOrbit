using System.Collections.Generic;
using DeadOrbit.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Core
{
    public enum TileType
    {
        Ground,
        Wall,
    }

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

    public class TileMap
    {
        private readonly int[,] _tiles;
        private readonly TileType[,] _types;
        public int Width { get; }
        public int Height { get; }

        private static readonly Rectangle[] GroundFrames =
        {
            new(0 * 32, 0 * 32, 32, 32),
            new(1 * 32, 0 * 32, 32, 32),
            new(2 * 32, 0 * 32, 32, 32),
            new(3 * 32, 0 * 32, 32, 32),
        };

        private static readonly Rectangle[] WallFrames =
        {
            new(0 * 32, 1 * 32, 32, 32),
            new(1 * 32, 1 * 32, 32, 32),
            new(2 * 32, 1 * 32, 32, 32),
            new(3 * 32, 1 * 32, 32, 32),
        };

        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new int[width, height];
            _types = new TileType[width, height];
        }

        public void Set(int x, int y, TileType type, int variant = 0)
        {
            _types[x, y] = type;
            _tiles[x, y] = variant;
        }

        public bool IsWall(int x, int y) => _types[x, y] == TileType.Wall;

        public List<ICollidable> GetNearbyWalls(Vector2 worldPos, int radius = 2)
        {
            var result = new List<ICollidable>();
            var center = TileGrid.ToGrid(worldPos);

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    int x = center.X + dx;
                    int y = center.Y + dy;

                    if (x < 0 || y < 0 || x >= Width || y >= Height)
                        continue;

                    if (_types[x, y] == TileType.Wall)
                        result.Add(new WallTile(x, y));
                }
            }

            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var source = _types[x, y] switch
                    {
                        TileType.Wall => WallFrames[_tiles[x, y]],
                        _ => GroundFrames[_tiles[x, y]],
                    };

                    var dest = new Rectangle(
                        x * TileGrid.TileSize,
                        y * TileGrid.TileSize,
                        TileGrid.TileSize,
                        TileGrid.TileSize
                    );

                    spriteBatch.Draw(GameResources.Tileset, dest, source, Color.White);
                }
            }
        }
    }
}
