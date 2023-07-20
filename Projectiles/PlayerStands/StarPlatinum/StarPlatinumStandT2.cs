using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StarPlatinum
{
    public class StarPlatinumStandT2 : StandClass
    {
        public override int PunchDamage => 56;
        public override int PunchTime => 10;
        public override int AltDamage => 65;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 2;
        public override string PunchSoundName => "Ora";
        public override string PoseSoundName => "YareYareDaze";
        public override string SpawnSoundName => "Star Platinum";
        public override bool CanUsePart4Dye => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            Flick,
            Pose
        }

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
                secondaryAbility = player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] != 0;
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0)
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
                    StayBehindWithAbility();
                }
                if (Main.mouseRight && shootCount <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<StarFinger>()] == 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 120;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= ProjectileSpeed;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StarFinger>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[projIndex].netUpdate = true;
                    Projectile.netUpdate = true;
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                PunchAndShootAI(ModContent.ProjectileType<StarFinger>(), shootMax: 1);
            }

            if (secondaryAbility)
                currentAnimationState = AnimationState.Secondary;
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