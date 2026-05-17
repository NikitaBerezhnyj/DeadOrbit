using Microsoft.Xna.Framework;

namespace DeadOrbit.Models
{
    public class InventoryItem
    {
        public string Name;
        public ItemType Type;
        public int Count;
        public Color Color;
        public Rectangle? SpriteSource;

        public InventoryItem(
            string name,
            ItemType type,
            int count,
            Color color,
            Rectangle? spriteSource = null
        )
        {
            Name = name;
            Type = type;
            Count = count;
            Color = color;
            SpriteSource = spriteSource;
        }

        public bool IsEmpty => Type == ItemType.None;

        public override string ToString() =>
            Type == ItemType.None ? "[empty]"
            : Count > 1 ? $"{Name} x{Count}"
            : Name;
    }
}
