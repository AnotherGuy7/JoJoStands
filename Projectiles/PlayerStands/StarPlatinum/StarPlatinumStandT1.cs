using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.StarPlatinum
{
    public class StarPlatinumStandT1 : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 23;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 1;
        public override string PunchSoundName => "Ora";
        public override string PoseSoundName => "YareYareDaze";
        public override string SpawnSoundName => "Star Platinum";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/StarPlatinum/StarPlatinum_Punch_";
        public override Vector2 PunchSize => new Vector2(44, 12);
        public override bool CanUsePart4Dye => true;
        public override StandAttackType StandType => StandAttackType.Melee;

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

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                        Punch();
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehind();
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
                BasicPunchAI();

            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
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
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/StarPlatinum", "StarPlatinum_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 2, 12, true);
        }
    }
}