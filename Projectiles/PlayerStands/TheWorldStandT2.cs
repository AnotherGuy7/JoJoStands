using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class TheWorldStandT2 : StandClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/TheWorldStand"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override int punchDamage => 42;
        public override int punchTime => 10;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;

        public bool abilityPose = false;
        public int timestopPoseTimer = 0;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
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
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = -10;
            }
            if (projectile.spriteDirection == -1)
            {
                drawOffsetX = -60;
            }
            drawOriginOffsetY = -halfStandHeight;
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")) && projectile.owner == Main.myPlayer)
            {
                timestopPoseTimer = 60;
                Timestop(2);
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
                if (!attackFrames)
                {
                    StayBehind();
                }
                if (timestopPoseTimer > 0)
                {
                    timestopPoseTimer--;
                    normalFrames = false;
                    attackFrames = false;
                    abilityPose = true;
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                    if (timestopPoseTimer <= 1)
                    {
                        abilityPose = false;
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
            writer.Write(abilityPose);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            abilityPose = reader.ReadBoolean();
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= punchTime - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
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
            if (abilityPose)
            {
                projectile.frame = 6;
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                projectile.frame = 7;
            }
        }
    }
}