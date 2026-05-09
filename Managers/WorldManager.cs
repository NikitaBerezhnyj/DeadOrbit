using System;
using System.Collections.Generic;
using DeadOrbit.Core;
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
        private List<ResourceNode> _resources;
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
            _resources = world.Resources;
            _beacon = world.Beacon;
            _player = new Player(13, 9);
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            var collidables = new List<ICollidable>();
            collidables.AddRange(_resources);

            _player.ResolveCollisions(collidables);

            foreach (var b in _baseStations)
                b.Check(_player);
            _beacon.Check(_baseStations);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var r in _resources)
                r.Draw(spriteBatch);
            foreach (var b in _baseStations)
                b.Draw(spriteBatch);
            _beacon.Draw(spriteBatch);
            _player.Draw(spriteBatch);
        }
    }
}
