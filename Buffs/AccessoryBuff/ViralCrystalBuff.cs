using JoJoStands.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class ViralCrystalBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Viral Crystal");
            // Description.SetDefault("A mystical gold-silver crystal is watching over you.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ViralCrystal>()] == 0)
                Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<ViralCrystal>(), 0, 0f, player.whoAmI);
        }
    }
}