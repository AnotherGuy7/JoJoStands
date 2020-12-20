using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class SteelBallP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.LightDisc);
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 3;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.maxPenetrate = 2;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Items.Hamon.HamonPlayer hamonPlayer = Main.player[projectile.owner].GetModPlayer<Items.Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5 && Main.rand.Next(0, 101) <= 25)
            {
                target.AddBuff(mod.BuffType("Spin"), 40);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.oldVelocity = new Vector2(0);
            return base.OnTileCollide(oldVelocity);
        }

        public override void AI()
        {
            projectile.rotation += (float)projectile.direction * 0.8f;
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 128, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }
    }
}