using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class GEFrog : ModProjectile
    {

        public override string Texture { get { return "Terraria/NPC_" + NPCID.Frog; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 10;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private const float searchDistance = 18f * 16f;

        private int maxReflection = 15;
        private int jumpTimer = 0;
        private bool walking = false;

        public override void AI()
        {
            SelectFrame();
            jumpTimer--;
            if (jumpTimer <= 0)
            {
                jumpTimer = 0;
            }
            projectile.velocity.Y += 1.5f;
            if (projectile.velocity.Y >= 6f)
            {
                projectile.velocity.Y = 6f;
            }

            maxReflection = (int)projectile.ai[0] * 15;
            if (projectile.ai[1] == 0f)
            {
                projectile.penetrate = 1;
                projectile.ai[1] = 4f;
            }
            if (projectile.ai[1] == 1f)
            {
                projectile.penetrate = 2;
                projectile.ai[1] = 4f;
            }
            if (projectile.ai[1] == 2f)
            {
                projectile.penetrate = 3;
                projectile.ai[1] = 4f;
            }
            if (projectile.ai[1] == 3f)
            {
                projectile.penetrate = 4;
                projectile.ai[1] = 4f;
            }
            if (projectile.velocity.X == 0f)
            {
                walking = false;
            }
            else
            {
                walking = true;
            }

            NPC npcTarget = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC possibleTarget = Main.npc[n];
                if (possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.townNPC && possibleTarget.type != NPCID.TargetDummy && possibleTarget.type != NPCID.CultistTablet)
                {
                    npcTarget = possibleTarget;
                    float distance = projectile.Distance(possibleTarget.Center);
                    if (distance < searchDistance)
                    {
                        npcTarget = possibleTarget;
                    }
                    break;
                }
            }
            if (npcTarget != null)
            {
                if (npcTarget.position.X > projectile.position.X)
                {
                    projectile.velocity.X = 1f;
                    projectile.direction = 1;
                    projectile.spriteDirection = -1;
                }
                if (npcTarget.position.X < projectile.position.X)
                {
                    projectile.velocity.X = -1f;
                    projectile.direction = -1;
                    projectile.spriteDirection = 1;
                }
                if (WorldGen.SolidTile((int)(projectile.Center.X / 16f) + projectile.direction, (int)(projectile.Center.Y / 16f)) && jumpTimer <= 0)
                {
                    jumpTimer = 40;
                    projectile.velocity.Y = -10f;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                projectile.velocity.X = 0f;
            }

            Player player = Main.player[projectile.owner];
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    Player otherPlayer = Main.player[otherProj.owner];
                    if (otherProj.active && projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (projectile.owner != otherProj.owner && player.team != otherPlayer.team && projectile.damage < maxReflection)
                        {
                            Dust.NewDust(Main.projectile[p].position + Main.projectile[p].velocity, projectile.width, projectile.height, DustID.FlameBurst, Main.projectile[p].velocity.X * -0.5f, Main.projectile[p].velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " missed his target and hit " + player.name + "'s damage-reflecting frog."), otherProj.damage, 1, true);
                            otherProj.Kill();
                            projectile.Kill();
                        }
                        if (projectile.owner != otherProj.owner && player.team != otherPlayer.team && projectile.damage > maxReflection)
                        {
                            otherProj.Kill();
                            projectile.Kill();
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (damage <= maxReflection)
            {
                damage = target.damage - Main.rand.Next(0, 3);
            }
            if (damage > maxReflection)
            {
                damage = maxReflection;
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath1, projectile.position);
        }

        private void SelectFrame()
        {
            projectile.frameCounter++;
            if (walking)
            {
                if (projectile.frameCounter >= 8)
                {
                    projectile.frameCounter = 0;
                    projectile.frame += 1;
                }
                if (projectile.frame >= 10)
                {
                    projectile.frame = 6;
                }
            }
            else
            {
                if (projectile.frameCounter >= 12)
                {
                    projectile.frameCounter = 0;
                    projectile.frame += 1;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}