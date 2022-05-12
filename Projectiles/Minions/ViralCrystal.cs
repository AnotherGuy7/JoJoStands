using JoJoStands.Buffs.AccessoryBuff;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
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
            Projectile.width = 22;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            projectileYOffset += 0.03f;
            if (projectileYOffset >= 360f)
                projectileYOffset = 0f;

            if (player.HasBuff(ModContent.BuffType<ViralCrystalBuff>()))
                Projectile.timeLeft = 2;
            if (shootTimer > 0)
                shootTimer--;

            Lighting.AddLight(Projectile.Center, 1f, 1f, 0.5f);
            Projectile.position = player.Center - new Vector2(Projectile.width / 2f, player.height + 18f + ((float)Math.Sin(projectileYOffset) * 4f));
            Projectile.spriteDirection = player.direction;

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC potentialTarget = Main.npc[n];
                if (potentialTarget.active && !potentialTarget.immortal && !potentialTarget.townNPC && potentialTarget.lifeMax > 5 && Projectile.Distance(potentialTarget.position) <= (30 * 16))
                {
                    target = potentialTarget;
                }
            }

            if (target != null)
            {
                if (shootTimer <= 0)
                {
                    shootTimer += 60;
                    Vector2 shootVel = target.Center - Projectile.Center;
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<ViralCrystalProjectile>(), 81, 4f, Projectile.owner);
                    SoundEngine.PlaySound(3, Projectile.Center, 5);
                }
            }
        }
    }
}