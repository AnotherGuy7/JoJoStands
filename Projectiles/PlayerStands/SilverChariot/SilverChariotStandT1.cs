using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotStandT1 : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 12;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 10;
        public override int TierNumber => 1;
        public override string PoseSoundName => "SilverChariot";
        public override string SpawnSoundName => "Silver Chariot";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_Stab_";
        public override Vector2 PunchSize => new Vector2(20, 10);
        public override PunchSpawnData PunchData => new PunchSpawnData()
        {
            standardPunchOffset = new Vector2(6f, 0f),
            minimumLifeTime = 6,
            maximumLifeTime = 12,
            minimumTravelDistance = 20,
            maximumTravelDistance = 48,
            bonusAfterimageAmount = 0
        };
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
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 5, newPunchTime / 2, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 12, true);
        }
    }
}