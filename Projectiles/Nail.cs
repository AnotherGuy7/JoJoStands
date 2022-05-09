using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class Nail : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/GeneralBullet"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>());
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;

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