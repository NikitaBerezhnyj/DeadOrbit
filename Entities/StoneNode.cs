using Microsoft.Xna.Framework;

namespace DeadOrbit.Entities
{
    public class StoneNode : ResourceNode
    {
        public StoneNode(int tileX, int tileY)
            : base(tileX, tileY, hp: 5, Color.SlateGray) { }
    }
}
