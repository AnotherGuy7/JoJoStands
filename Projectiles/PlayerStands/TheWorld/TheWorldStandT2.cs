using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheWorld
{
    public class TheWorldStandT2 : StandClass
    {
        public override int punchDamage => 42;
        public override int punchTime => 10;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;
        public override string punchSoundName => "Muda";
        public override string poseSoundName => "ComeAsCloseAsYouLike";
        public override string spawnSoundName => "The World";
        public override bool CanUseSaladDye => true;
        public override StandType standType => StandType.Melee;

        private bool abilityPose = false;
        private int timestopPoseTimer = 0;
        private int timestopStartDelay = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            /*if (Projectile.spriteDirection == 1)
            {
                DrawOffsetX = -10;
            }
            if (Projectile.spriteDirection == -1)
            {
                DrawOffsetX = -60;
            }
            DrawOriginOffsetY = -halfStandHeight;*/

            if (SpecialKeyPressed() && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && timestopStartDelay <= 0)
            {
                if (JoJoStands.JoJoStandsSounds == null)
                    timestopStartDelay = 120;
                else
                {
                    SoundStyle zawarudo = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/TheWorld");
                    zawarudo.Volume = MyPlayer.ModSoundsVolume;
                    SoundEngine.PlaySound(zawarudo, Projectile.position);
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
                idleFrames = false;
                attackFrames = false;
                abilityPose = true;
                Main.mouseLeft = false;
                Main.mouseRight = false;
                if (timestopPoseTimer <= 1)
                    abilityPose = false;
            }
            if (mPlayer.timestopActive && !mPlayer.timestopOwner)
                return;

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
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
            if (mPlayer.standAutoMode)
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
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (abilityPose)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("AbilityPose");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/TheWorld", "/TheWorld_" + animationName);

            if (animationName == "Idle")
            {
                if (mPlayer.currentTextureDye == MyPlayer.StandTextureDye.Salad)
                    AnimateStand(animationName, 4, 15, true);
                else
                    AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "AbilityPose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
    }
}