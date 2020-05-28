using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;
using System.IO;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT2 : StandClass
    {

        public override float shootSpeed => 16f;
        public bool grabFrames = false;
        public bool secondaryFrames = false;
        public override float maxDistance => 98f;
        public override int punchDamage => 16;
        public override int punchTime => 12;
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 8f;
        public override float tierNumber => 1f;
        public override int standOffset => -30;
        public int updateTimer = 0;


        public override void AI()
        {
            SelectAnimation();
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
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !secondaryFrames && !grabFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames && !secondaryFrames)
                {
                    StayBehind();
                }

            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
            if (Main.mouseRight && projectile.owner == Main.myPlayer && shootCount <= 0 && !grabFrames)     //this should be under if (!modPlayer.StandAutoMode); Why 
            {
                secondaryFrames = true;     //what is the purpose of this? All this does is make it not work (as it sets grabFrames to false and grabFrames needs to be true for grabs to work)
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
            }
            if (grabFrames && projectile.ai[0] != -1f)    
            {
                projectile.velocity = Vector2.Zero;
                NPC npc = Main.npc[(int)projectile.ai[0]];
                npc.direction = -projectile.direction;
                npc.position = projectile.position + new Vector2(5f * projectile.direction, -2f - npc.height / 3f);
                npc.velocity = Vector2.Zero;
                if (projectile.frame == 7)      //This can't be reached, grab sheet only has 3 frames; Also why is GD supposed to punch the enemy away anyway?
                {
                    npc.StrikeNPC(punchDamage, 7f, projectile.direction, true);
                    shootCount += 180;
                    projectile.ai[0] = -1f;
                    grabFrames = false;
                }
                PlayAnimation("Grab");      //this is handled in SelectAnimation automatically
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(grabFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
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
            standTexture = mod.GetTexture("Projectiles/PlayerStands/GratefulDead/GratefulDead_" + animationName);
            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, punchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 1, 1, true);
            }
            if (animationName == "Grab")
            {
                AnimationStates(animationName, 3, 12, true, true, 3, 3);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 2, 12, true);
            }
        }

    }
}