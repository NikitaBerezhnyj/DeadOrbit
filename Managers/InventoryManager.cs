using System;
using DeadOrbit.Models;
using DeadOrbit.Rendering;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Managers
{
    public class InventoryManager
    {
        public const int Size = 8;
        public InventoryItem[] Slots { get; private set; }
        public int ActiveIndex { get; private set; } = 0;

        public InventoryItem ActiveItem => Slots[ActiveIndex];

        public InventoryManager()
        {
            Slots = new InventoryItem[Size];

            Slots[0] = new InventoryItem(
                "Axe",
                ItemType.Tool,
                1,
                Color.SandyBrown,
                SpriteSourceMap.Axe
            );
            Slots[1] = new InventoryItem(
                "Pickaxe",
                ItemType.Tool,
                1,
                Color.LightGray,
                SpriteSourceMap.Pickaxe
            );
            Slots[2] = new InventoryItem(
                "Sword",
                ItemType.Weapon,
                1,
                Color.WhiteSmoke,
                SpriteSourceMap.Sword
            );
            Slots[3] = new InventoryItem(
                "Coal",
                ItemType.Resource,
                3,
                Color.DarkGray,
                SpriteSourceMap.Coal
            );
            Slots[4] = new InventoryItem(
                "Stone",
                ItemType.Resource,
                5,
                Color.SlateGray,
                SpriteSourceMap.Stone
            );
            Slots[5] = new InventoryItem(
                "Wood",
                ItemType.Resource,
                2,
                Color.SaddleBrown,
                SpriteSourceMap.Wood
            );

            for (int i = 0; i < Size; i++)
                Slots[i] ??= new InventoryItem("", ItemType.None, 0, Color.Transparent);
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

        public void SetActive(int index)
        {
            if (index >= 0 && index < Size)
                ActiveIndex = index;
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

        public void DropActive()
        {
            var active = Slots[ActiveIndex];

            if (active == null || active.IsEmpty)
                return;

            active.Count--;

            if (active.Count <= 0)
            {
                ClearActive();
            }
        }

        public void ClearActive()
        {
            Slots[ActiveIndex] = new InventoryItem("", ItemType.None, 0, Color.Transparent);
        }
    }
}
