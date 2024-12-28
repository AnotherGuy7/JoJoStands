using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ClackerProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 180;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.maxPenetrate = 2;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                if (target.HasBuff(ModContent.BuffType<Vampire>()))
                    target.AddBuff(ModContent.BuffType<Sunburn>(), 2 * 60);
            }
        }

        public override void AI()
        {
            Projectile.rotation *= 0f;
            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.frame = 0;
        }
    }
}