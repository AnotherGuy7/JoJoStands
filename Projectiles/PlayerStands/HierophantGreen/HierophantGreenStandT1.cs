using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class HierophantGreenStandT1 : StandClass
    {
        public override int ShootTime => 40;
        public override int ProjectileDamage => 12;
        public override int HalfStandHeight => 30;
        public override Vector2 StandOffset => Vector2.Zero;
        public override int TierNumber => 1;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override string PoseSoundName => "ItsTheVictorWhoHasJustice";
        public override string SpawnSoundName => "Hierophant Green";
        public override bool CanUseSaladDye => true;
        public override bool CanUseRangeIndicators => false;
        private const float AutoModeDetectionDistance = 18f * 16f;

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

            Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
            if (Main._rand.Next(1, 6 + 1) == 1)
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(0f, HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.GreenTorch);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.2f;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        if (!mPlayer.canStandBasicAttack)
                            return;

                        attacking = true;
                        currentAnimationState = AnimationState.Attack;
                        if (shootCount <= 0)
                        {
                            shootCount += newShootTime;
                            int direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);
                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;

                            float numberProjectiles = 3;        //incraeses by 1 each tier
                            float rotation = MathHelper.ToRadians(15);      //increases by 5 every tier
                            float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                perturbedSpeed *= randomSpeedOffset;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), newProjectileDamage, 2f, player.whoAmI);
                                Main.projectile[projIndex].netUpdate = true;
                            }
                            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                            Projectile.netUpdate = true;
                            if (player.velocity.X == 0f)
                                player.ChangeDir(direction);
                        }
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehind();
                else
                    GoInFront();
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
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;

                            float numberProjectiles = 3;
                            float rotation = MathHelper.ToRadians(15);
                            float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                perturbedSpeed *= randomSpeedOffset;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), (int)((ProjectileDamage * mPlayer.standDamageBoosts) * 0.9f), 2f, player.whoAmI);
                                Main.projectile[projIndex].netUpdate = true;
                            }
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
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/HierophantGreen", "HierophantGreen_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 3, 20, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 3, 15, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 2, 15, true);
        }
    }
}