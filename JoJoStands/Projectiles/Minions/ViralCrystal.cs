using System;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class ViralCrystal : ModProjectile
    {
        private int shootTimer = 0;
        private const float shootSpeed = 10f;
        private NPC npcTarget = null;
        private float projectileYOffset = 0f;

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.hostile = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectileYOffset += 0.03f;
            if (projectileYOffset >= 360f)
                projectileYOffset = 0f;

            if (player.HasBuff(mod.BuffType("ViralCrystalBuff")))
            {
                projectile.timeLeft = 2;
            }
            if (shootTimer > 0)
            {
                shootTimer--;
            }
            Lighting.AddLight(projectile.Center, 1f, 1f, 0.5f);
            projectile.position = player.Center - new Vector2(projectile.width / 2f, player.height + 18f + ((float)Math.Sin(projectileYOffset) * 4f));
            projectile.spriteDirection = player.direction;

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC potentialTarget = Main.npc[n];
                if (potentialTarget.active && !potentialTarget.immortal && !potentialTarget.townNPC && potentialTarget.lifeMax > 5 && projectile.Distance(potentialTarget.position) <= (30 * 16))
                {
                    target = potentialTarget;
                }
            }

            if (target != null)
            {
                if (shootTimer <= 0)
                {
                    shootTimer += 60;
                    Vector2 shootVel = target.Center - projectile.Center;
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("ViralCrystalProjectile"), 81, 4f, projectile.owner);
                    Main.PlaySound(3, projectile.Center, 5);
                }
            }
        }
    }
}