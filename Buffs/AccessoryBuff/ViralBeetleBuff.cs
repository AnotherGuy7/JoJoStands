using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class ViralBeetleBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Viral Beetle");
            Description.SetDefault("The otherworldy virus you have conquered now manifested itself and protects you.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        private int beetleSpawnTimer = 0;

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("ViralBeetleProjectile")] < 3)
            {
                beetleSpawnTimer++;
                if (beetleSpawnTimer >= 180)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("ViralBeetleProjectile"), 124, 14f, player.whoAmI, player.ownedProjectileCounts[mod.ProjectileType("ViralBeetleProjectile")] + 1);
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