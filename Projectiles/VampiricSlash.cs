using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class VampiricSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
            drawOriginOffsetY = 15;
            projectile.scale = (int)1.5;
            projectile.timeLeft = 18;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = Main.projFrames[projectile.type];
            }
            Vector2 centerOffset = new Vector2((player.width / 2f) * player.direction, -24f);
            if (player.direction == -1)
            {
                centerOffset.X -= 24f;
            }
            projectile.position = player.Center + centerOffset;
            projectile.spriteDirection = projectile.direction = player.direction;

            projectile.frameCounter++;
            if (projectile.frameCounter >= 6)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            player.velocity.X *= 0.5f;

            if (target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                int newDamage = damage / 4;
                if (newDamage < player.statLifeMax - player.statLife)
                {
                    player.statLife += newDamage;
                    player.HealEffect(newDamage, true);
                }
                if (newDamage >= player.statLifeMax - player.statLife)
                {
                    int healthReduction = player.statLifeMax - player.statLife;
                    int healingAmount = newDamage - healthReduction;
                    player.statLife += healingAmount;
                    player.HealEffect(healingAmount, true);
                }
            }
        }
    }
}