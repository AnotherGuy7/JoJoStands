using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueenBTD
{
    public class KillerQueenBTDStand : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override float shootSpeed => 4f;
        public int projectileDamage = 180;      //not overriden cause it has to change sometimes
        public override int shootTime => 60;
        public override int halfStandHeight => 37;

        public override void AI()
        {
            SelectFrame();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = -10;
            }
            if (Main.dayTime)
            {
                projectileDamage = 180;
            }
            if (!Main.dayTime)
            {
                projectileDamage = 158;
            }
            drawOriginOffsetY = -halfStandHeight;

            Vector2 vector131 = player.Center;
            if (!attackFrames)
            {
                vector131.X -= (float)((15 + player.width / 2) * player.direction);
            }
            if (attackFrames)
            {
                vector131.X -= (float)((15 + player.width / 2) * (player.direction * -1));
            }
            vector131.Y -= -35f + halfStandHeight;
            projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = projectile.spriteDirection = player.direction;
            projectile.rotation = 0;

            if (SpecialKeyPressed() && !player.HasBuff(mod.BuffType("BitesTheDust")))
            {
                player.AddBuff(mod.BuffType("BitesTheDust"), 10);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BiteTheDustEffect"));
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].friendly)
                    {
                        Main.npc[k].life -= Main.rand.Next(90, 136);
                        Main.npc[k].StrikeNPC(Main.rand.Next(90, 136), 0f, Main.npc[k].direction, true);
                        Main.npc[k].netUpdate = true;
                    }
                }
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
                {
                    attackFrames = true;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                }
                else if (!secondaryAbilityFrames)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
                {
                    secondaryAbilityFrames = true;
                    projectile.ai[0] = 1f;      //to detonate all bombos
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                float targetDist = 350f;
                if (target == null)
                {
                    for (int k = 0; k < 200; k++)       //the targeting system
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                if (npc.boss)       //is gonna try to detect bosses over anything
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                                else        //if it fails to detect a boss, it'll detect the next best thing
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                            }
                        }
                    }
                }
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (attackFrames && projectile.frame == 6 && shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += shootTime - modPlayer.standSpeedBoosts;
                            Vector2 shootVel = targetPos - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Bubble"), (int)(projectileDamage * modPlayer.standDamageBoosts), 3f, Main.myPlayer, 0f, projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    normalFrames = true;
                    attackFrames = false;
                }
            }
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                secondaryAbilityFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= shootTime - modPlayer.standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame == 6 && !modPlayer.StandAutoMode)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - modPlayer.standSpeedBoosts;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Bubble"), (int)(projectileDamage * modPlayer.standDamageBoosts), 5f, Main.myPlayer, 1f, projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                if (projectile.frame >= 9)
                {
                    projectile.frame = 2;
                    attackFrames = false;
                }
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 18)      //18 to match it up with the explosion if you want
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 8)
                {
                    projectile.frame = 9;
                }
                if (projectile.frame >= 12)      //cause it should only click once
                {
                    projectile.frame = 9;
                    projectile.ai[0] = 0f;
                    secondaryAbilityFrames = false;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                secondaryAbilityFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
            /*if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode && normalFrames)
            {
                normalFrames = false;
                attackFrames = false;
                clickFrames = false;
                projectile.frame = 9;
            }*/
        }
    }
}