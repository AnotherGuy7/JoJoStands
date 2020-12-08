using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Nail : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/GeneralBullet"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 12;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(mod.BuffType("Infected"), 60 * 9);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                {
                    target.AddBuff(mod.BuffType("Infected"), 10 * 60);
                }
            }
        }
    }
}