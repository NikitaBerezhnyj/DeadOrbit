// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using DeadOrbit.Managers;
// using DeadOrbit.Entities;
// using System;

// namespace DeadOrbit
// {
//     public class Game1 : Game
//     {
//         private GraphicsDeviceManager _graphics;
//         private SpriteBatch _spriteBatch;
//         private Texture2D _pixel;

//         private Player _player;
//         private WorldManager _worldManager;

//         public Game1()
//         {
//             _graphics = new GraphicsDeviceManager(this);
//             Content.RootDirectory = "Content";
//             IsMouseVisible = true;
//         }

//         protected override void Initialize()
//         {
//             var random = new Random();
//             int seed = random.Next(10000, 99999);

//             _pixel = new Texture2D(GraphicsDevice, 1, 1);
//             _pixel.SetData(new[] { Color.White });

//             _player = new Player(new Vector2(400, 300));
//             _worldManager = new WorldManager(seed); // Seed передаємо сюди

//             base.Initialize();
//         }

//         protected override void Update(GameTime gameTime)
//         {
//             _player.Update(gameTime);
//             _worldManager.Update(_player);

//             base.Update(gameTime);
//         }

//         protected override void Draw(GameTime gameTime)
//         {
//             GraphicsDevice.Clear(Color.Green);

//             _spriteBatch.Begin();

//             _worldManager.Draw(_spriteBatch, _pixel);
//             _player.Draw(_spriteBatch, _pixel);

//             _spriteBatch.End();
//             base.Draw(gameTime);
//         }

//         protected override void LoadContent()
//         {
//             _spriteBatch = new SpriteBatch(GraphicsDevice);
//         }
//     }
// }

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadOrbit.Managers;
using DeadOrbit.Entities;
using System;

namespace DeadOrbit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;

        private Player _player;
        private WorldManager _worldManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var random = new Random();
            int seed = random.Next(10000, 99999);

            _worldManager = new WorldManager(seed);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // створюємо pixel тут (правильніше)
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // тепер передаємо texture в Player
            _player = new Player(new Vector2(400, 300), _pixel);
        }

        protected override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _worldManager.Update(_player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            _worldManager.Draw(_spriteBatch, _pixel);
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}