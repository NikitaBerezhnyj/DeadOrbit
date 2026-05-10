using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public static class SpriteSourceMap
    {
        private const int S = 32;

        public static Rectangle Sword = new(0 * S, 7 * S, S, S);
        public static Rectangle Pickaxe = new(1 * S, 7 * S, S, S);
        public static Rectangle Axe = new(2 * S, 7 * S, S, S);
        public static Rectangle Stone = new(0 * S, 8 * S, S, S);
        public static Rectangle Coal = new(1 * S, 8 * S, S, S);
        public static Rectangle Wood = new(2 * S, 8 * S, S, S);
        public static Rectangle StoneNode = new(0 * S, 2 * S, S, S);
        public static Rectangle CoalNode = new(1 * S, 2 * S, S, S);
    }
}
