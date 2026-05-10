using System;
using System.Collections.Generic;
using System.IO;
using DeadOrbit.Core;
using DeadOrbit.Entities;
using DeadOrbit.Entities.Enemies;
using DeadOrbit.Entities.Items;
using DeadOrbit.Entities.Structures;
using DeadOrbit.Interfaces;
using DeadOrbit.Rendering;
using DeadOrbit.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Managers
{
    public class WorldManager
    {
        private TileMap _tileMap;
        private Player _player;
        private Camera _camera;
        private List<BaseStation> _baseStations;
        private List<ResourceNode> _resources;
        private List<DroppedItem> _droppedItems = new();
        private List<Enemy> _enemies;
        private Beacon _beacon;

        private readonly ParticleSystem _particles = new();

        private HotbarRenderer _hotbar;
        private PlayerHealthRenderer _healthRenderer;

        private HashSet<GridPosition> _blockedTiles = new();

        private Texture2D _pixel;
        private int _seed;

        public WorldManager()
        {
            _seed = new Random().Next(10000, 99999);
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            using var stream = File.OpenRead("Assets/raw/tileset.png");
            GameResources.Tileset = Texture2D.FromStream(graphicsDevice, stream);

            GameResources.Pixel = _pixel;

            GameResources.DefaultFont = content.Load<SpriteFont>("Fonts/DefaultFont");

            var world = WorldGenerator.Generate(_seed);
            _tileMap = world.TileMap;
            _baseStations = world.BaseStations;
            _resources = world.Resources;
            _beacon = world.Beacon;
            _enemies = world.Enemies;
            _player = new Player(13, 9);
            _camera = new Camera(graphicsDevice);
            _hotbar = new HotbarRenderer(_player.InventoryManager, graphicsDevice);
            _healthRenderer = new PlayerHealthRenderer(_player);

            RebuildBlockedTiles();
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            var wallsNearPlayer = _tileMap.GetNearbyWalls(_player.Position);

            var staticCollidables = new List<ICollidable>();
            staticCollidables.AddRange(_resources.FindAll(r => !r.IsDestroyed));
            staticCollidables.AddRange(wallsNearPlayer);

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
                if (enemy.IsDefeated)
                    continue;

                var drop = enemy.UpdateAI(gameTime, _player, _blockedTiles);
                if (drop != null)
                {
                    _droppedItems.Add(drop);
                    Console.WriteLine($"[DROP] Додано {drop.Item.Name} на {drop.Position}");
                }

                var enemyCollidables = new List<ICollidable>(staticCollidables);

                enemyCollidables.AddRange(_tileMap.GetNearbyWalls(enemy.Position));
                enemyCollidables.AddRange(_enemies.FindAll(e => e != enemy && !e.IsDefeated));
                enemy.ResolveCollisions(enemyCollidables);
            }

            var playerCollidables = new List<ICollidable>(staticCollidables);
            playerCollidables.AddRange(_enemies.FindAll(e => !e.IsDefeated));
            _player.ResolveCollisions(playerCollidables);

            foreach (var r in _resources)
                r.Update(gameTime);

            _particles.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (InputSystem.UsePressed)
            {
                var drop = InteractionSystem.TryMine(_player, _resources, _particles);

                if (drop != null)
                {
                    _droppedItems.Add(drop);
                    Console.WriteLine($"[DROP] Mining drop: {drop.Item.Name} на {drop.Position}");
                    RebuildBlockedTiles();
                }
            }

            if (InputSystem.AttackPressed)
            {
                var drop = CombatSystem.TryPlayerAttack(_player, _enemies);

                if (drop != null)
                    _droppedItems.Add(drop);
            }

            _droppedItems.RemoveAll(d => d.IsPickedUp);

            foreach (var d in _droppedItems)
            {
                d.Update(gameTime);
                d.UpdateAttraction(gameTime, _player.Position);
            }

            foreach (var d in _droppedItems)
            {
                if (d.IsPickedUp || d.PickupDelay > 0)
                    continue;
                if (d.IsInPickupRange(_player.Position))
                {
                    _player.InventoryManager.TryAdd(d.Item);
                    d.IsPickedUp = true;
                    Console.WriteLine($"[PICKUP] Підібрано: {d.Item}");
                }
            }
            _droppedItems.RemoveAll(d => d.IsPickedUp);

            foreach (var station in _baseStations)
                station.Check(_player);

            _beacon.Check(_baseStations);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Matrix transform = _camera.GetTransform(_player.Position);

            spriteBatch.Begin(transformMatrix: transform);

            _tileMap.Draw(spriteBatch);

            foreach (var r in _resources)
                r.Draw(spriteBatch);
            foreach (var b in _baseStations)
                b.Draw(spriteBatch);
            foreach (var d in _droppedItems)
                d.Draw(spriteBatch);
            foreach (var e in _enemies)
                e.Draw(spriteBatch);
            _beacon.Draw(spriteBatch);
            _particles.Draw(spriteBatch);
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

            for (int x = 0; x < _tileMap.Width; x++)
            for (int y = 0; y < _tileMap.Height; y++)
                if (_tileMap.IsWall(x, y))
                    _blockedTiles.Add(new GridPosition(x, y));

            foreach (var r in _resources)
                if (!r.IsDestroyed)
                    _blockedTiles.Add(r.GridPos);
            foreach (var b in _baseStations)
                _blockedTiles.Add(b.GridPos);
            _blockedTiles.Add(_beacon.GridPos);
        }
    }
}
