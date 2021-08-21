using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT2 : StandClass
    {
        public override float shootSpeed => 16f;
        public override float maxDistance => 98f;
        public override int punchDamage => 41;
        public override int punchTime => 12;
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 8f;
        public override float tierNumber => 1f;
        public override int standOffset => -4;
        public override int standType => 1;
        public override string poseSoundName => "OnceWeDecideToKillItsDone";
        public override string spawnSoundName => "The Grateful Dead";

        private int updateTimer = 0;
        private bool grabFrames = false;
        private bool secondaryFrames = false;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !secondaryFrames && !grabFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames && !secondaryFrames && !grabFrames)
                {
                    StayBehind();
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && shootCount <= 0 && !grabFrames)
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
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
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
                    npc.AddBuff(mod.BuffType("Old2"), 2);
                    if (!npc.active)
                    {
                        grabFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (!Main.mouseRight && (grabFrames || secondaryFrames))
                {
                    grabFrames = false;
                    projectile.ai[0] = -1f;
                    shootCount += 30;
                    secondaryFrames = false;
                }
            }
            if (mPlayer.standAutoMode)
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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
                PlayAnimation("Idle");
            }
            if (secondaryFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (grabFrames)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryFrames = false;
                PlayAnimation("Grab");

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
                standTexture = mod.GetTexture("Projectiles/PlayerStands/GratefulDead/GratefulDead_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 1, 1, true);
            }
            if (animationName == "Grab")
            {
                AnimateStand(animationName, 3, 12, true, 2, 2);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }

    }
}