using DeadOrbit.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Core
{
    public abstract class GameObject
    {
        public Vector2 Position;

        protected void PlaceOnGrid(int tileX, int tileY)
        {
            Position = TileGrid.ToWorld(new GridPosition(tileX, tileY));
        }

        public GridPosition GridPos =>
            TileGrid.ToGrid(Position + new Vector2(TileGrid.TileSize / 2f));

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
