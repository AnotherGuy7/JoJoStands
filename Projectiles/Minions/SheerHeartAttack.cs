using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{  
    public class SheerHeartAttack : ModProjectile
    {
        public bool saidKocchiwomiro = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 30;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 6000;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            NPC npcTarget = null;
            if (JoJoStands.ItemHotKey.JustPressed)
            {
                projectile.position = Main.LocalPlayer.position;
                projectile.netUpdate = true;
            }
            projectile.velocity.Y += 1.5f;
            if (projectile.velocity.Y >= 6f)
            {
                projectile.velocity.Y = 6f;
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
                if (!saidKocchiwomiro)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Kocchiwomiro"));
                    saidKocchiwomiro = true;
                }
                if (npcTarget.position.X > projectile.position.X)
                {
                    projectile.velocity.X = 2.5f;
                    projectile.direction = 1;
                    projectile.spriteDirection = 1;
                }
                if (npcTarget.position.X < projectile.position.X)
                {
                    projectile.velocity.X = -2.5f;
                    projectile.direction = -1;
                    projectile.spriteDirection = -1;
                }
                if (WorldGen.SolidTile((int)(projectile.Center.X / 16f) + projectile.direction, (int)(projectile.Center.Y / 16f)))
                {
                    projectile.velocity.Y = -6f;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                saidKocchiwomiro = false;
                projectile.velocity.X = 0f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.ai[0] == 0f)
            {
                int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileID.GrenadeIII, 107, 7f, Main.myPlayer);
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;
                Main.player[projectile.owner].AddBuff(mod.BuffType("SheerHeartAttackCooldown"), 600);
                projectile.active = false;
            }
            if (projectile.ai[0] == 1f)
            {
                int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileID.GrenadeIII, 142, 7f, Main.myPlayer);
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;
                projectile.active = false;
                Main.player[projectile.owner].AddBuff(mod.BuffType("SheerHeartAttackCooldown"), 240);
            }
        }
    }
}