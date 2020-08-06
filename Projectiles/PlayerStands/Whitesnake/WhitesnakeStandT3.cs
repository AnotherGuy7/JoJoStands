using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Whitesnake
{
    public class WhitesnakeStandT3 : StandClass
    {
        public override int punchDamage => 69;
        public override int altDamage => 63;
        public override int punchTime => 12;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 9f;
        public override int standType => 1;
        public override float maxDistance => 147f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities

        private int updateTimer = 0;
        private bool grabFrames = false;
        private bool secondaryFrames = false;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
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
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && shootCount <= 0 && projectile.owner == Main.myPlayer)
                {
                    projectile.frame = 0;
                    secondaryAbilityFrames = true;
                }
                if (secondaryAbilityFrames)
                {
                    Main.mouseLeft = false;
                    if (projectile.frame >= 4)
                    {
                        shootCount += 120;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 8f;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("MeltYourHeart"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        secondaryAbilityFrames = false;
                    }
                }
                if (!attackFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (SpecialKeyCurrent() && projectile.owner == Main.myPlayer && shootCount <= 0 && !grabFrames)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 5f;
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + projectile.velocity;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    secondaryFrames = true;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active)
                        {
                            if (projectile.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                projectile.ai[0] = npc.whoAmI;
                                grabFrames = true;
                            }
                        }
                    }
                    LimitDistance();
                }
                if (grabFrames && projectile.ai[0] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)projectile.ai[0]];
                    npc.direction = -projectile.direction;
                    npc.position = projectile.position + new Vector2(5f * projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(BuffID.Confused, 2);
                    if (!npc.active)
                    {
                        grabFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (!SpecialKeyCurrent() && (grabFrames || secondaryFrames))
                {
                    grabFrames = false;
                    projectile.ai[0] = -1f;
                    shootCount += 30;
                    secondaryFrames = false;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(grabFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            grabFrames = reader.ReadBoolean();
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/Whitesnake/Whitesnake_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 3, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 5, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 10, true);
            }
        }
    }
}