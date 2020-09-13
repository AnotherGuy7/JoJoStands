using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.AccessoryBuff
{
    public class ViralCrystalBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Viral Crystal");
            Description.SetDefault("A mystical gold-silver crystal is watching over you.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("ViralCrystal")] == 0)
            {
                Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("ViralCrystal"), 0, 0f, player.whoAmI);
            }
        }
    }
}