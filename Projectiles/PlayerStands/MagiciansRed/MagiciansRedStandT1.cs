using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.MagiciansRed
{
    public class MagiciansRedStandT1 : StandClass
    {
        public override float ProjectileSpeed => 8f;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override int ProjectileDamage => 16;
        public override int ShootTime => 20;
        public override int HalfStandHeight => 35;
        public override Vector2 StandOffset => Vector2.Zero;
        public override int TierNumber => 1;
        public override string PoseSoundName => "ThePowerToWieldFlameAtWill";
        public override string SpawnSoundName => "Magicians Red";
        public override bool CanUseRangeIndicators => false;

        private int ChanceToDebuff = 25;
        private int DebuffDuration = 5 * 60;
        private const float AutoModeDetectionDistance = 24f * 16f;

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
                if (!attacking)
                    StayBehind();
                else
                    GoInFront();

                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        if (!mPlayer.canStandBasicAttack)
                        {
                            currentAnimationState = AnimationState.Idle;
                            return;
                        }

                        attacking = true;
                        currentAnimationState = AnimationState.Attack;
                        if (shootCount <= 0)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<FireAnkh>(), newProjectileDamage, 3f, Projectile.owner, ChanceToDebuff, DebuffDuration);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                NPC target = FindNearestTarget(AutoModeDetectionDistance);
                if (target != null)
                {
                    attacking = true;
                    currentAnimationState = AnimationState.Attack;
                    int direction = target.Center.X < Projectile.Center.X ? -1 : 1;
                    GoInFront(direction);
                    Projectile.spriteDirection = Projectile.direction = direction;
                    Projectile.velocity = target.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 4f;
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<FireAnkh>(), newProjectileDamage, 3f, Projectile.owner, ChanceToDebuff, DebuffDuration);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    StayBehind();
                    attacking = false;
                    currentAnimationState = AnimationState.Idle;
                }
            }

            if (Main.rand.Next(0, 20 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position + new Vector2(0, -HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, Scale: 2.1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 1.4f;
            }
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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/MagiciansRed/MagiciansRed_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 15, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newShootTime / 2, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
        }
    }
}