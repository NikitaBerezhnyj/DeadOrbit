using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Data;
using DeadOrbit.Entities;
using DeadOrbit.Systems;
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
        private List<DroppedItem> _droppedItems = new();
        private Beacon _beacon;

        private HotbarRenderer _hotbar;

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
            _hotbar = new HotbarRenderer(_player.Inventory, graphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            var collidables = new List<ICollidable>();
            collidables.AddRange(_resources.FindAll(r => !r.IsDestroyed));
            _player.ResolveCollisions(collidables);

            if (InputSystem.AttackPressed)
            {
                var dropped = InteractionSystem.TryMine(_player, _resources);
                if (dropped != null)
                    _droppedItems.Add(dropped);
            }

            InteractionSystem.TryPickup(_player, _droppedItems);
            _droppedItems.RemoveAll(d => d.IsPickedUp);

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
            foreach (var d in _droppedItems)
                d.Draw(spriteBatch);
            _beacon.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            _hotbar.Draw(spriteBatch);
        }
    }
}
