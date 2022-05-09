using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class Emerald : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.25f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Main.rand.Next(0, 1 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 61, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 60 * 9);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                }
            }
        }
    }
}