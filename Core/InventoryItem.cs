using Microsoft.Xna.Framework;

namespace DeadOrbit.Core
{
    public enum ItemType
    {
        None,
        Tool,
        Resource,
    }

    public class InventoryItem
    {
        public string Name;
        public ItemType Type;
        public int Count;
        public Color Color;

        public InventoryItem(string name, ItemType type, int count, Color color)
        {
            Name = name;
            Type = type;
            Count = count;
            Color = color;
        }

        public bool IsEmpty => Type == ItemType.None;

        public override string ToString() =>
            Type == ItemType.None ? "[empty]"
            : Count > 1 ? $"{Name} x{Count}"
            : Name;
    }
}
