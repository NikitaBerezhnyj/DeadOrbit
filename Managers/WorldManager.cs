using System;
using System.Collections.Generic;
using DeadOrbit.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadOrbit.Managers
{
    public class WorldManager
    {
        private List<Base> _bases;
        private Beacon _beacon;
        private int _seed;

        public WorldManager(int seed)
        {
            _seed = seed;
            _bases = new List<Base>();
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            Random rnd = new Random(_seed);
            _bases.Clear();

            for (int i = 0; i < 3; i++)
            {
                _bases.Add(new Base(new Rectangle(rnd.Next(50, 700), rnd.Next(50, 400), 40, 40)));
            }
            _beacon = new Beacon(new Rectangle(380, 20, 50, 50));
        }

        public void Update(Player player)
        {
            foreach (var b in _bases)
                b.Update(player);

            _beacon.Update(_bases);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            foreach (var b in _bases)
            {
                b.Draw(spriteBatch, pixel);
            }
            _beacon.Draw(spriteBatch, pixel);
        }
    }
}
