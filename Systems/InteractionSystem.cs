using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities;
using DeadOrbit.Entities.Items;
using DeadOrbit.Entities.Resources;
using DeadOrbit.Rendering;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Systems
{
    public static class InteractionSystem
    {
        private const float InteractRange = TileGrid.TileSize * 1.2f;

        public static DroppedItem TryMine(
            Player player,
            IEnumerable<ResourceNode> nodes,
            ParticleSystem particles = null
        )
        {
            string held = player.InventoryManager.ActiveItem?.Name ?? "";

            player.PlaySwingAnimation();

            var target = FindTarget(player, nodes);
            if (target == null)
                return null;

            string required = GetRequiredTool(target);

            if (held != required)
            {
                Console.WriteLine($"[MINE] Потрібен {required}, а в руках {held}");
                return null;
            }

            target.Mine(1, particles);

            if (target.IsDestroyed)
                return new DroppedItem(target.Position, target.GetDrop(), applyImpulse: true);

            return null;
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
