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
        private Camera _camera;
        private List<BaseStation> _baseStations;
        private List<ResourceNode> _resources;
        private List<DroppedItem> _droppedItems = new();
        private List<Enemy> _enemies;
        private Beacon _beacon;

        private HotbarRenderer _hotbar;
        private PlayerHealthRenderer _healthRenderer;

        private HashSet<GridPosition> _blockedTiles = new();

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
            _enemies = world.Enemies;
            _player = new Player(13, 9);
            _camera = new Camera(graphicsDevice);
            _hotbar = new HotbarRenderer(_player.Inventory, graphicsDevice);
            _healthRenderer = new PlayerHealthRenderer(_player);

            RebuildBlockedTiles();
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            var staticCollidables = new List<ICollidable>();

            staticCollidables.AddRange(_resources.FindAll(r => !r.IsDestroyed));

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);

                if (enemy.IsDefeated)
                    continue;

                var drop = enemy.UpdateAI(gameTime, _player, _blockedTiles);

                if (drop != null)
                    _droppedItems.Add(drop);

                var enemyCollidables = new List<ICollidable>(staticCollidables);

                enemyCollidables.AddRange(_enemies.FindAll(e => e != enemy && !e.IsDefeated));

                enemy.ResolveCollisions(enemyCollidables);
            }

            var playerCollidables = new List<ICollidable>(staticCollidables);

            playerCollidables.AddRange(_enemies.FindAll(e => !e.IsDefeated));

            _player.ResolveCollisions(playerCollidables);

            if (InputSystem.UsePressed)
            {
                var drop = InteractionSystem.TryMine(_player, _resources);

                if (drop != null)
                {
                    _droppedItems.Add(drop);
                    RebuildBlockedTiles();
                }
            }

            if (InputSystem.AttackPressed)
            {
                var drop = CombatSystem.TryPlayerAttack(_player, _enemies);

                if (drop != null)
                    _droppedItems.Add(drop);
            }

            InteractionSystem.TryPickup(_player, _droppedItems);

            _droppedItems.RemoveAll(d => d.IsPickedUp);

            foreach (var station in _baseStations)
                station.Check(_player);

            _beacon.Check(_baseStations);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Matrix transform = _camera.GetTransform(_player.Position);

            spriteBatch.Begin(transformMatrix: transform);

            spriteBatch.Draw(
                GameResources.Pixel,
                new Rectangle(0, 0, TileGrid.WorldPixelW, TileGrid.WorldPixelH),
                new Color(34, 85, 34)
            );

            foreach (var r in _resources)
                r.Draw(spriteBatch);
            foreach (var b in _baseStations)
                b.Draw(spriteBatch);
            foreach (var d in _droppedItems)
                d.Draw(spriteBatch);
            foreach (var e in _enemies)
                e.Draw(spriteBatch);
            _beacon.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            _hotbar.Draw(spriteBatch);
            _healthRenderer.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void RebuildBlockedTiles()
        {
            _blockedTiles.Clear();
            foreach (var r in _resources)
                if (!r.IsDestroyed)
                    _blockedTiles.Add(r.GridPos);
            foreach (var b in _baseStations)
                _blockedTiles.Add(b.GridPos);
            _blockedTiles.Add(_beacon.GridPos);
        }
    }
}
