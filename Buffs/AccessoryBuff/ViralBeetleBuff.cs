using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class ViralBeetleBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Beetle");
            Description.SetDefault("The otherworldy virus you have conquered now manifested itself and protects you.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        private int beetleSpawnTimer = 0;

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ViralBeetleProjectile>()] < 3)
            {
                beetleSpawnTimer++;
                if (beetleSpawnTimer >= 180)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ViralBeetleProjectile>(), 124, 14f, player.whoAmI, player.ownedProjectileCounts[ModContent.ProjectileType<ViralBeetleProjectile>()] + 1);
                    beetleSpawnTimer = 0;
                }
            }
            else
            {
                beetleSpawnTimer = 0;
            }
        }
    }
}