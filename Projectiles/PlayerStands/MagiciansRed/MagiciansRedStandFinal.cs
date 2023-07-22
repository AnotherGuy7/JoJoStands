using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.MagiciansRed
{
    public class MagiciansRedStandFinal : StandClass
    {
        public override float ProjectileSpeed => 8f;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override int ProjectileDamage => 95;
        public override int ShootTime => 14;
        public override int HalfStandHeight => 35;
        public override Vector2 StandOffset => Vector2.Zero;
        public override int TierNumber => 4;
        public override string PoseSoundName => "ThePowerToWieldFlameAtWill";
        public override string SpawnSoundName => "Magicians Red";
        public override bool CanUseRangeIndicators => false;

        private int ChanceToDebuff = 60;
        private int DebuffDuration = 8 * 60;
        private int secondRingTimer = 0;
        public static readonly SoundStyle RedBindSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/RedBind");
        public static readonly SoundStyle CrossfireHurricaneSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/CrossfireHurricaneSpecial");

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

            if (!attacking)
                StayBehind();
            else
                GoInFront();

            secondaryAbility = player.ownedProjectileCounts[ModContent.ProjectileType<RedBind>()] != 0;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility)
                    {
                        if (!mPlayer.canStandBasicAttack)
                        {
                            currentAnimationState = AnimationState.Idle;
                            return;
                        }

                        attacking = true;
                        currentAnimationState = AnimationState.Attack;
                        Projectile.netUpdate = true;
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
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !secondaryAbility && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    secondaryAbility = true;
                    if (JoJoStands.SoundsLoaded)
                    {
                        SoundStyle redBind = RedBindSound;
                        redBind.Volume = JoJoStands.ModSoundsVolume;
                        SoundEngine.PlaySound(redBind, Projectile.position);
                    }

                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= 16f;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<RedBind>(), newProjectileDamage, 3f, Projectile.owner, Projectile.whoAmI, DebuffDuration - 60);
                    Main.projectile[projIndex].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                    Projectile.netUpdate = true;
                }
                if (secondaryAbility)
                    currentAnimationState = AnimationState.SecondaryAbility;
                if (SpecialKeyPressed())
                {
                    for (int p = 0; p < 12; p++)
                    {
                        float angle = MathHelper.ToRadians((360f / 12f) * p);
                        Vector2 position = player.Center + (angle.ToRotationVector2() * 48f);
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), position - new Vector2(26f, 25f), Vector2.Zero, ModContent.ProjectileType<CrossfireHurricaneAnkh>(), newProjectileDamage * 2, 4f, Projectile.owner, 24f * 16f, angle);
                        Main.projectile[projIndex].netUpdate = true;
                        Main.projectile[projIndex].timeLeft += 10 * p;
                        Projectile.netUpdate = true;
                    }
                    if (JoJoStands.SoundsLoaded)
                    {
                        SoundStyle crossfireHurricaneSound = CrossfireHurricaneSound;
                        crossfireHurricaneSound.Volume = JoJoStands.ModSoundsVolume;
                        SoundEngine.PlaySound(crossfireHurricaneSound, Projectile.Center);
                    }
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(45));
                    secondRingTimer = 1;
                }
                if (secondRingTimer != 0)
                {
                    secondRingTimer++;
                    if (secondRingTimer >= 40)
                    {
                        for (int p = 0; p < 25; p++)
                        {
                            float angle = MathHelper.ToRadians((360 / 25f) * p);
                            Vector2 position = player.Center + (angle.ToRotationVector2() * 48f);
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), position - new Vector2(26f, 25f), Vector2.Zero, ModContent.ProjectileType<CrossfireHurricaneAnkh>(), newProjectileDamage, 4f, Projectile.owner, 16f * 16f, -angle);
                            Main.projectile[projIndex].netUpdate = true;
                            Main.projectile[projIndex].timeLeft += 180 + (5 * p);
                            Projectile.netUpdate = true;
                        }
                        secondRingTimer = 0;
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                NPC target = FindNearestTarget(350f);
                if (target != null)
                {
                    currentAnimationState = AnimationState.Attack;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.position.X < 0f)
                        Projectile.spriteDirection = Projectile.direction = -1;

                    Projectile.spriteDirection = Projectile.direction;

                    Projectile.velocity = target.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 4f;
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.position - Projectile.Center;
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
                    currentAnimationState = AnimationState.Idle;
            }

            if (Main.rand.Next(0, 5 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position + new Vector2(0, -HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, Scale: 2.1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 1.4f;
            }
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
                PlayAnimation("Secondary");
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
            else if (animationName == "Secondary")
                AnimateStand(animationName, 4, 15, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
        }
    }
}