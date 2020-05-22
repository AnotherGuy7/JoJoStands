using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
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
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.destroyAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 93)
                {
                    target.AddBuff(BuffID.OnFire, 60 * 3);
                }
            }
            if (mPlayer.greaterDestroyEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(BuffID.CursedInferno, 60 * 10);
                }
            }
        }
    }
}