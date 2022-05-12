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

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<Vampire>()))
                target.AddBuff(ModContent.BuffType<Sunburn>(), 120);
        }

        public override void AI()
        {
            Projectile.rotation *= 0f;
            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.frame = 0;
        }
    }
}