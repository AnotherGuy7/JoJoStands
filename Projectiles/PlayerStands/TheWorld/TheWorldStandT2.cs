using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands.TheWorld
{
    public class TheWorldStandT2 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override int punchDamage => 42;
        public override int punchTime => 10;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;
        public override string punchSoundName => "Muda";

        private bool abilityPose = false;
        private int timestopPoseTimer = 0;
        private int updateTimer = 0;
        private int timestopStartDelay = 0;

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
            if (SpecialKeyPressed() && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                if (JoJoStands.JoJoStandsSounds == null)
                    timestopStartDelay = 120;
                else
                {
                    Terraria.Audio.LegacySoundStyle zawarudo = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/TheWorld");
                    zawarudo.WithVolume(MyPlayer.soundVolume);
                    Main.PlaySound(zawarudo, projectile.position);
                    timestopStartDelay = 1;
                }
            }
            if (timestopStartDelay != 0)
            {
                timestopStartDelay++;
                if (timestopStartDelay >= 120)
                {
                    Timestop(2);
                    timestopPoseTimer = 60;
                    timestopStartDelay = 0;
                }
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
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(abilityPose);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            abilityPose = reader.ReadBoolean();
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
            if (abilityPose)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("AbilityPose");
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
            standTexture = mod.GetTexture("Projectiles/PlayerStands/TheWorld/TheWorld_" + animationName);
            if (animationName == "Idle")
            {
                AnimationStates(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "AbilityPose")
            {
                AnimationStates(animationName, 1, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 10, true);
            }
        }
    }
}