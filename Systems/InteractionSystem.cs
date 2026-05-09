using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Systems
{
    public static class InteractionSystem
    {
        private const float InteractRange = TileGrid.TileSize * 1.2f;

        public static DroppedItem TryMine(Player player, IEnumerable<ResourceNode> nodes)
        {
            var target = FindTarget(player, nodes);
            if (target == null)
                return null;

            string required = GetRequiredTool(target);
            string held = player.Inventory.ActiveItem?.Name ?? "";

            if (held != required)
            {
                Console.WriteLine($"[MINE] Потрібен {required}, а в руках {held}");
                return null;
            }

            target.Mine(1);
            Console.WriteLine($"[MINE] {target.GetType().Name} HP: {target.HP}");

            if (target.IsDestroyed)
            {
                Console.WriteLine($"[MINE] {target.GetType().Name} знищено!");
                return new DroppedItem(target.Position, target.GetDrop());
            }

            return null;
        }

        public static void TryPickup(Player player, List<DroppedItem> items)
        {
            var playerCenter = player.Position + new Vector2(TileGrid.TileSize / 2f);

            foreach (var item in items)
            {
                if (item.IsPickedUp)
                    continue;

                var itemCenter = item.Position + new Vector2(TileGrid.TileSize / 2f);
                float dist = Vector2.Distance(playerCenter, itemCenter);

                if (dist < TileGrid.TileSize * 1.5f)
                {
                    player.Inventory.TryAdd(item.Item);
                    item.IsPickedUp = true;
                    Console.WriteLine($"[PICKUP] Підібрано: {item.Item}");
                }
            }
        }

        private static ResourceNode FindTarget(Player player, IEnumerable<ResourceNode> nodes)
        {
            var center = player.Position + new Vector2(TileGrid.TileSize / 2f);
            ResourceNode best = null;
            float bestDist = float.MaxValue;

            foreach (var node in nodes)
            {
                if (node.IsDestroyed)
                    continue;

                var nodeCenter = node.Position + new Vector2(TileGrid.TileSize / 2f);
                float dist = Vector2.Distance(center, nodeCenter);

                if (dist > InteractRange)
                    continue;

                if (player.FacingDirection != Vector2.Zero)
                {
                    var toNode = Vector2.Normalize(nodeCenter - center);
                    float dot = Vector2.Dot(player.FacingDirection, toNode);
                    if (dot < 0.3f)
                        continue;
                }

                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = node;
                }
            }

            return best;
        }

        private static string GetRequiredTool(ResourceNode node) =>
            node switch
            {
                CoalNode => "Pickaxe",
                StoneNode => "Pickaxe",
                WoodNode => "Axe",
                _ => "",
            };
    }
}
