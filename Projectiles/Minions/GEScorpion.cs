using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class GEScorpion : ModProjectile
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.ScorpionBlack; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.penetrate = 6;
            projectile.timeLeft = 800;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            drawOriginOffsetY = -5;
        }

        private const float DetectionDistance = 20f * 16f;
        private bool walking = false;

        public override void AI()
        {
            SelectFrame();
            projectile.ai[0]--;
            if (projectile.ai[0] <= 0f)
            {
                projectile.ai[0] = 0f;
            }
            projectile.velocity.Y += 1.5f;
            if (projectile.velocity.Y >= 6f)
            {
                projectile.velocity.Y = 6f;
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
            for (int n = 0; n < 200; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TargetDummy && npc.type != NPCID.CultistTablet)
                {
                    npcTarget = npc;
                    float distance = projectile.Distance(npc.Center);
                    if (distance < DetectionDistance)
                    {
                        npcTarget = npc;
                    }
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
                if (WorldGen.SolidTile((int)(projectile.Center.X / 16f) + projectile.direction, (int)(projectile.Center.Y / 16f)) && projectile.ai[0] <= 0f)
                {
                    projectile.ai[0] = 50f;
                    projectile.velocity.Y = -6f;
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
                        if (projectile.owner != otherProj.owner && player.team != otherPlayer.team && projectile.damage < 75)
                        {
                            Dust.NewDust(Main.projectile[p].position + Main.projectile[p].velocity, projectile.width, projectile.height, DustID.FlameBurst, Main.projectile[p].velocity.X * -0.5f, Main.projectile[p].velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " couldn't resist hurting " + player.name + "'s damage-reflecting scorpion."), otherProj.damage, 1, true);
                            otherProj.Kill();
                            projectile.Kill();
                        }
                        if (projectile.owner != otherProj.owner && player.team != otherPlayer.team && projectile.damage > 75)
                        {
                            otherProj.Kill();
                            projectile.Kill();
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (damage <= 75)
            {
                damage = target.damage - Main.rand.Next(0, 2);
                target.AddBuff(BuffID.Poisoned, 120);
            }
            if (damage > 75)
            {
                damage = 75;
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath1, projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            else
            {
                projectile.frame = 0;
            }
        }
    }
}