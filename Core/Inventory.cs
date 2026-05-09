using System;

namespace DeadOrbit.Core
{
    public class Inventory
    {
        public const int Size = 8;
        public InventoryItem[] Slots { get; private set; }
        public int ActiveIndex { get; private set; } = 0;

        public InventoryItem ActiveItem => Slots[ActiveIndex];

        public Inventory()
        {
            Slots = new InventoryItem[Size];

            Slots[0] = new InventoryItem(
                "Axe",
                ItemType.Tool,
                1,
                Microsoft.Xna.Framework.Color.SandyBrown
            );
            Slots[1] = new InventoryItem(
                "Pickaxe",
                ItemType.Tool,
                1,
                Microsoft.Xna.Framework.Color.LightGray
            );
            Slots[2] = new InventoryItem(
                "Coal",
                ItemType.Resource,
                3,
                Microsoft.Xna.Framework.Color.DarkGray
            );
            Slots[3] = new InventoryItem(
                "Stone",
                ItemType.Resource,
                5,
                Microsoft.Xna.Framework.Color.SlateGray
            );
            Slots[4] = new InventoryItem(
                "Wood",
                ItemType.Resource,
                2,
                Microsoft.Xna.Framework.Color.SaddleBrown
            );

            for (int i = 0; i < Size; i++)
                Slots[i] ??= new InventoryItem(
                    "",
                    ItemType.None,
                    0,
                    Microsoft.Xna.Framework.Color.Transparent
                );
        }

        public void Next()
        {
            ActiveIndex = (ActiveIndex + 1) % Size;
            Console.WriteLine($"[INVENTORY] Active: {ActiveItem}");
        }

        public void Prev()
        {
            ActiveIndex = (ActiveIndex - 1 + Size) % Size;
            Console.WriteLine($"[INVENTORY] Active: {ActiveItem}");
        }
    }
}
