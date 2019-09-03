using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class GEScorpion : ModProjectile
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.ScorpionBlack; } }

        public bool walking = false;

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
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            SelectFrame();
            NPC npcTarget = null;
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
            Vector2 move = Vector2.Zero;
            float distance = 400f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].type != NPCID.CultistTablet)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    npcTarget = Main.npc[k];
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
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
 
        public void SelectFrame()
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