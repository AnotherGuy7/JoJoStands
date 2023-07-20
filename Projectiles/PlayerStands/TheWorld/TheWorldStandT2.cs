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
        public override int PunchDamage => 42;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 44;
        public override int FistWhoAmI => 1;
        public override int TierNumber => 2;
        public override string PunchSoundName => "Muda";
        public override string PoseSoundName => "ComeAsCloseAsYouLike";
        public override string SpawnSoundName => "The World";
        public override bool CanUseSaladDye => true;
        public override StandAttackType StandType => StandAttackType.Melee;

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
                if (!JoJoStands.SoundsLoaded || !JoJoStands.SoundsModAbilityVoicelines)
                    timestopStartDelay = 120;
                else
                {
                    SoundStyle zawarudo = TheWorldStandFinal.TheWorldTimestopSound;
                    zawarudo.Volume = JoJoStands.ModSoundsVolume;
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
                abilityPose = true;
                Main.mouseLeft = false;
                Main.mouseRight = false;
                if (timestopPoseTimer <= 1)
                    abilityPose = false;
            }
            if (mPlayer.timestopActive && !mPlayer.timestopOwner)
                return;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                }
                if (!attacking)
                {
                    StayBehind();
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            if (abilityPose)
                currentAnimationState = AnimationState.Special;
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
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Special)
                PlayAnimation("AbiliyPose");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/TheWorld", "TheWorld_" + animationName);

            if (animationName == "Idle")
            {
                if (mPlayer.currentTextureDye == MyPlayer.StandTextureDye.Salad)
                    AnimateStand(animationName, 4, 15, true);
                else
                    AnimateStand(animationName, 2, 30, true);
            }
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "AbilityPose")
                AnimateStand(animationName, 1, 10, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }
    }
}