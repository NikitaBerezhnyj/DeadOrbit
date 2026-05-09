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
                "Sword",
                ItemType.Weapon,
                1,
                Microsoft.Xna.Framework.Color.WhiteSmoke
            );
            Slots[3] = new InventoryItem(
                "Coal",
                ItemType.Resource,
                3,
                Microsoft.Xna.Framework.Color.DarkGray
            );
            Slots[4] = new InventoryItem(
                "Stone",
                ItemType.Resource,
                5,
                Microsoft.Xna.Framework.Color.SlateGray
            );
            Slots[5] = new InventoryItem(
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

        public bool TryAdd(InventoryItem incoming)
        {
            foreach (var slot in Slots)
            {
                if (!slot.IsEmpty && slot.Name == incoming.Name && slot.Type == ItemType.Resource)
                {
                    slot.Count += incoming.Count;
                    Console.WriteLine($"[INV] {incoming.Name}: тепер {slot.Count}");
                    return true;
                }
            }

            for (int i = 0; i < Size; i++)
            {
                if (Slots[i].IsEmpty)
                {
                    Slots[i] = incoming;
                    Console.WriteLine($"[INV] Новий слот: {incoming.Name}");
                    return true;
                }
            }

            Console.WriteLine("[INV] Інвентар повний!");
            return false;
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
