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
    public class WhitesnakeStandFinal : StandClass
    {
        public override int punchDamage => 88;
        public override int altDamage => 82;
        public override int punchTime => 11;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 9f;
        public override int standType => 1;
        public override float maxDistance => 147f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
		public override string poseSoundName => "YouWereTwoSecondsTooLate";

        private int updateTimer = 0;
        private bool stealFrames = false;
        private bool waitingForEnemyFrames = false;

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
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !stealFrames)
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
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("MeltYourHeart"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        secondaryAbilityFrames = false;
                    }
                }
                if (!attackFrames && !stealFrames && !waitingForEnemyFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (SpecialKeyCurrent() && projectile.owner == Main.myPlayer && shootCount <= 0 && !stealFrames)
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
                    waitingForEnemyFrames = true;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active)
                        {
                            if (projectile.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                projectile.ai[0] = npc.whoAmI;
                                stealFrames = true;
                            }
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.active)
                            {
                                if (projectile.Distance(otherPlayer.Center) <= 30f & otherPlayer.team != player.team && otherPlayer.whoAmI != player.whoAmI)
                                {
                                    projectile.ai[1] = otherPlayer.whoAmI;
                                    stealFrames = true;
                                }
                            }
                        }
                    }
                    LimitDistance();
                }
                if (stealFrames && projectile.ai[0] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)projectile.ai[0]];
                    npc.direction = -projectile.direction;
                    npc.position = projectile.position + new Vector2(-6f * projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    if (projectile.frame == 4)
                    {
                        npc.AddBuff(mod.BuffType("Stolen"), 30 * 60);
                    }
                    if (projectile.frame == 6)
                    {
                        stealFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 60;
                    }
                    if (!npc.active)
                    {
                        stealFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (stealFrames && projectile.ai[1] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    Player otherPlayer = Main.player[(int)projectile.ai[1]];
                    otherPlayer.direction = -projectile.direction;
                    otherPlayer.position = projectile.position + new Vector2(-6f * projectile.direction, -2f - otherPlayer.height / 3f);
                    otherPlayer.velocity = Vector2.Zero;
                    if (projectile.frame == 4)      //this is the frame where the disc has just been stolen
                    {
                        otherPlayer.AddBuff(mod.BuffType("Stolen"), 30 * 60);
                    }
                    if (projectile.frame == 6)      //anim ended
                    {
                        stealFrames = false;
                        projectile.ai[1] = -1f;
                        shootCount += 60;
                    }
                    if (!otherPlayer.active)
                    {
                        stealFrames = false;
                        projectile.ai[1] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (!SpecialKeyCurrent() && ((stealFrames || waitingForEnemyFrames) || projectile.ai[1] != -1f))
                {
                    stealFrames = false;
                    waitingForEnemyFrames = false;
                    projectile.ai[0] = -1f;
                    projectile.ai[1] = -1f;
                    shootCount += 30;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(stealFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            stealFrames = reader.ReadBoolean();
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
            if (waitingForEnemyFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Steal");
                projectile.frame = 0;
            }
            if (stealFrames)
            {
                normalFrames = false;
                attackFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("Steal");
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
            if (animationName == "Steal")
            {
                AnimationStates(animationName, 7, 15, false);
            }
        }
    }
}