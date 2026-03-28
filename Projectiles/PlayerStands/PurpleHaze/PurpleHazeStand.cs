using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.PurpleHaze
{
    public abstract class PurpleHazeStand : StandClass
    {
        // —ability flags, no implementation we just got the gates, change later to more appropriate names
        protected virtual bool CanReleaseVirus => false;
        protected virtual bool CanInfectOnHit => false;
        protected virtual bool CanAOEBurst => false;

        // —tier overrides—
        public override float MaxDistance => 98f;
        public override int PunchDamage => 23;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 37;
        public override int FistID => 0;
        public override int TierNumber => 1;
        //public override string PunchSoundName => "ubasha";
        public override string PoseSoundName => "PurpleHazePose";
        public override string SpawnSoundName => "PurpleHaze";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/PurpleHaze/PurpleHaze_Punch_";
        public override Vector2 PunchSize => new Vector2(44, 12);
        public override bool CanUsePart4Dye => false;
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

        public override void OnDyeChanged()
        {
            punchTextures = new Texture2D[AmountOfPunchVariants];
            for (int v = 0; v < AmountOfPunchVariants; v++)
                punchTextures[v] = ModContent.Request<Texture2D>(PunchTexturePath + (v + 1), AssetRequestMode.ImmediateLoad).Value;
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
            else if (currentAnimationState == AnimationState.SecondaryAbility)
                PlayAnimation("CapsuleShot");
            else if (currentAnimationState == AnimationState.Special)
                PlayAnimation("Special");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/PurpleHaze", "PurpleHaze_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "CapsuleShot")
                AnimateStand(animationName, 11, 15, true);
            else if (animationName == "Special")
                AnimateStand(animationName, 13, 15, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 600, true);
        }
    }
}