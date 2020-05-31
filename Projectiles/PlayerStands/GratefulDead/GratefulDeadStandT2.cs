using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;
using System.IO;
using JoJoStands.Buffs.ItemBuff;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT2 : StandClass
    {

        public override float shootSpeed => 16f;
        public bool grabFrames = false;
        public bool secondaryFrames = false;
        public override float maxDistance => 98f;
        public override int punchDamage => 41;
        public override int punchTime => 12;
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 8f;
        public override float tierNumber => 1f;
        public override int standOffset => -4;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
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
                if (!attackFrames && !secondaryFrames && !grabFrames)
                {
                    StayBehind();
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && shootCount <= 0  && !grabFrames)
                {
                    LimitDistance();        //has to be in the bottom of the method so that it applies to velocity
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
                    if (!Main.mouseRight)
                    {
                        shootCount += 30;
                        secondaryFrames = false;
                        projectile.ai[0] = -1f;
                    }
                }
                ///If the player is holding right click, the player is the owner of the projectile, GD can shoot, and the GD isn't grabbing
                ///Limit the Distance it can go out for
                ///Move toward the mouse at a speed of 5
                ///Get the distance between GD and the mouse, if it's greater than 40 allow GD to move and if it's less than 40 stop him from moving
                ///Start animating GD with Secondary frames
                ///Search for NPCs near GD that he can grab and if so, make projectile.ai[0] the targets index, as well as start grabbing
                ///If ricght click isn't being held anymore, add 30 to shootCount so that GD can't grab immediately after, stop his animation, and make it so he stops grabbing the NPC he's grabbing
                if (grabFrames && projectile.ai[0] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)projectile.ai[0]];
                    npc.direction = -projectile.direction;
                    npc.position = projectile.position + new Vector2(5f * projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(mod.BuffType("Old2"), 10);
                    if (!Main.mouseRight || !npc.active)
                    {
                        LimitDistance();            //what... why. This would limit the distance only when grab stops, so it runs just once when you stop grabbing
                        grabFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
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
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 1, 1, true);
            }
            if (animationName == "Grab")
            {
                AnimationStates(animationName, 3, 12, true, true, 2, 2);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 12, true);
            }
        }
    }
}