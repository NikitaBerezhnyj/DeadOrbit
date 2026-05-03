using System;
using System.Collections.Generic;
using DeadOrbit.Data;
using DeadOrbit.Entities;
using DeadOrbit.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Managers
{
    public class WorldManager
    {
        private Player _player;
        private List<BaseStation> _baseStations;
        private Beacon _beacon;

        private Texture2D _pixel;
        private int _seed;

        public WorldManager()
        {
            _seed = new Random().Next(10000, 99999);
        }

        public void Load(GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            GameResources.Pixel = _pixel;

            var world = WorldGenerator.Generate(_seed);

            _baseStations = world.BaseStations;
            _beacon = world.Beacon;

            _player = new Player(new Vector2(400, 300));
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            foreach (var b in _baseStations)
                b.Check(_player);

            _beacon.Check(_baseStations);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var b in _baseStations)
                b.Draw(spriteBatch);

            _beacon.Draw(spriteBatch);
            _player.Draw(spriteBatch);
        }
    }
}
