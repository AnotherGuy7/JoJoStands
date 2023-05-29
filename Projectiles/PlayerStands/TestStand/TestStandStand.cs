using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TestStand
{
    public class TestStandStand : StandClass
    {
        public override int PunchDamage => 70;
        public override int HalfStandHeight => 37;
        public override int PunchTime => 7;
        public override StandAttackType StandType => StandAttackType.Melee;

        private int timestopPoseTimer = 0;
        private bool attacking = false;

        /*ripple effect info
        private int rippleCount = 3;
        private int rippleSize = 5;
        private int rippleSpeed = 15;
        private float distortStrength = 100f;*/

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            Special,
            Pose
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            //rippleEffectTimer--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
            {
                Timestop(30);
                timestopPoseTimer += 60;
            }
            if (timestopPoseTimer > 0)
            {
                timestopPoseTimer--;
                idleFrames = false;
                attacking = false;
                Projectile.frame = 6;
                Main.mouseLeft = false;
                Main.mouseRight = false;
            }
            if (mPlayer.timestopActive && !mPlayer.timestopOwner)
                return;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        attacking = false;
                        currentAnimationState = StandClass.AnimationState.Attack;
                    }
                }
                if (!attacking)
                {
                    StayBehind();
                    currentAnimationState = StandClass.AnimationState.Idle;
                }
                /*i
                }f (rippleEffectTimer <= 0)
                {
                    Filters.Scene["Shockwave"].Deactivate();
                    rippleEffectTimer = 0;
                }*/
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
            }

            if (currentAnimationState == StandClass.AnimationState.Idle)
            {
                PlayAnimation("Idle");
            }
            else if (currentAnimationState == StandClass.AnimationState.Attack)
            {
                PlayAnimation("Attack");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/TestStand/TestStand_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
        }
    }
}