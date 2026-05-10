using System;
using System.Collections.Generic;
using DeadOrbit.Core;
using DeadOrbit.Entities;
using DeadOrbit.Entities.Enemies;
using DeadOrbit.Entities.Items;
using DeadOrbit.Models;
using Microsoft.Xna.Framework;

namespace DeadOrbit.Systems
{
    public static class CombatSystem
    {
        private const float AttackRangeMultiplier = 1.5f;
        private const int DefaultPlayerDamage = 2;

        public static DroppedItem TryPlayerAttack(Player player, List<Enemy> enemies)
        {
            if (!CanAttack(player))
            {
                ShowNoWeaponMessage(player, enemies);
                return null;
            }

            Enemy target = FindTarget(player, enemies);

            if (target == null)
                return null;

            return HitEnemy(target, player, DefaultPlayerDamage);
        }

        private static bool CanAttack(Player player)
        {
            return player.InventoryManager.ActiveItem?.Type == ItemType.Weapon;
        }

        private static Enemy FindTarget(Player player, List<Enemy> enemies)
        {
            Vector2 playerCenter = GetCenter(player.Position);

            foreach (var enemy in enemies)
            {
                if (enemy.IsDefeated)
                    continue;

                Vector2 enemyCenter = GetCenter(enemy.Position);

                float distance = Vector2.Distance(playerCenter, enemyCenter);

                if (distance <= TileGrid.TileSize * AttackRangeMultiplier)
                    return enemy;
            }

            return null;
        }

        private static void ShowNoWeaponMessage(Player player, List<Enemy> enemies)
        {
            Vector2 playerCenter = GetCenter(player.Position);

            bool enemyNearby = enemies.Exists(enemy =>
            {
                if (enemy.IsDefeated)
                    return false;

                Vector2 enemyCenter = GetCenter(enemy.Position);

                return Vector2.Distance(playerCenter, enemyCenter)
                    <= TileGrid.TileSize * AttackRangeMultiplier;
            });

            if (enemyNearby)
                Console.WriteLine("[COMBAT] Потрібен меч щоб атакувати!");
        }

        public static DroppedItem HitEnemy(Enemy enemy, Player player, int damage)
        {
            Vector2 playerCenter = GetCenter(player.Position);
            Vector2 enemyCenter = GetCenter(enemy.Position);

            Vector2 knockDir = Vector2.Normalize(enemyCenter - playerCenter);

            enemy.TakeDamage(damage, knockDir);

            if (enemy.IsDefeated)
            {
                return new DroppedItem(enemy.Position, enemy.GetDrop());
            }

            return null;
        }

        public static void HitPlayer(Player player, int damage, Vector2 knockbackDir)
        {
            player.TakeDamage(damage, knockbackDir);
        }

        private static Vector2 GetCenter(Vector2 position)
        {
            return position + new Vector2(TileGrid.TileSize / 2f, TileGrid.TileSize / 2f);
        }
    }
}
