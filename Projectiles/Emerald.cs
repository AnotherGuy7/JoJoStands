using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Emerald : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.25f;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 61, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
        }
    }
}