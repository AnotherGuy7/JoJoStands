using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.HierophantGreen
{
    public class HierophantGreenStandT3 : StandClass
    {
        public override int ShootTime => 20;
        public override int ProjectileDamage => 56;
        public override int HalfStandHeight => 30;
        public override Vector2 StandOffset => Vector2.Zero;
        public override int TierNumber => 3;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override string PoseSoundName => "ItsTheVictorWhoHasJustice";
        public override string SpawnSoundName => "Hierophant Green";
        public override bool CanUseSaladDye => true;
        public override bool CanUseRangeIndicators => false;

        private bool spawningField = false;
        private int amountOfLinksCreated = 0;
        private bool pointShot = false;
        private bool remoteControlled = false;
        private bool linkShotForSpecial = false;
        private Vector2 formPosition = Vector2.Zero;

        private const float MaxRemoteModeDistance = 45f * 16f;
        private const float EmeraldSplashRadius = 20f * 16f;
        private const float AmountOfEmeraldSplashLinks = 35;
        private const float AutoModeDetectionDistance = 22f * 16f;

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
            Projectile.scale = ((50 - player.ownedProjectileCounts[ModContent.ProjectileType<EmeraldStringPointConnector>()]) * 2f) / 100f;
            if (Main._rand.Next(1, 4 + 1) == 1)
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(0f, HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.GreenTorch);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.2f;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && mPlayer.canStandBasicAttack && Projectile.scale >= 0.5f)
                    {
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

                            float numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(25);
                            float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                perturbedSpeed *= randomSpeedOffset;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), newProjectileDamage, 3f, player.whoAmI);
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

                    if (Main.mouseRight && shootCount <= 0 && Projectile.scale >= 0.5f && !playerHasAbilityCooldown)
                    {
                        shootCount += 30;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<BindingEmeraldString>(), newProjectileDamage / 2, 0f, Projectile.owner, 25);
                        Main.projectile[projIndex].netUpdate = true;
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                    }
                }

                if (!attacking)
                    StayBehind();
                else
                    GoInFront();

                if (SpecialKeyPressed() && player.ownedProjectileCounts[ModContent.ProjectileType<EmeraldStringPointConnector>()] <= 0 && !spawningField)
                {
                    spawningField = true;
                    formPosition = Projectile.position;
                    if (JoJoStands.SoundsLoaded)
                        SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/EmeraldSplash"), Projectile.Center);
                }
                if (SecondSpecialKeyPressed(false) && shootCount <= 0)
                {
                    shootCount += 30;
                    remoteControlled = true;
                    mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 4.5f;

                    Projectile.direction = 1;
                    if (Main.MouseWorld.X < Projectile.Center.X)
                        Projectile.direction = -1;
                    Projectile.netUpdate = true;
                    Projectile.spriteDirection = Projectile.direction;
                    LimitDistance(MaxRemoteModeDistance);
                }
                if (!Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Projectile.velocity *= 0.78f;
                    Projectile.netUpdate = true;
                }
                if (Main.mouseRight && mPlayer.canStandBasicAttack && Projectile.scale >= 0.5f && Projectile.owner == Main.myPlayer)
                {
                    currentAnimationState = AnimationState.Attack;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;

                        Projectile.direction = 1;
                        if (Main.MouseWorld.X < Projectile.Center.X)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;

                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;

                        float numberProjectiles = 5;
                        float rotation = MathHelper.ToRadians(25);
                        float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            perturbedSpeed *= randomSpeedOffset;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), newProjectileDamage, 3f, player.whoAmI);
                            Main.projectile[projIndex].netUpdate = true;
                        }
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        currentAnimationState = AnimationState.Idle;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (SpecialKeyPressed() && shootCount <= 0 && Projectile.scale >= 0.5f)
                {
                    pointShot = !pointShot;
                    int connectorType = ModContent.ProjectileType<EmeraldStringPoint>();
                    if (!pointShot)
                        connectorType = ModContent.ProjectileType<EmeraldStringPointConnector>();

                    shootCount += 15;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= ProjectileSpeed;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, connectorType, 0, 3f, player.whoAmI);
                    Main.projectile[projIndex].netUpdate = true;
                    SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                    Projectile.netUpdate = true;
                }

                if (SecondSpecialKeyPressed(false) && shootCount <= 0)
                {
                    shootCount += 30;
                    remoteControlled = false;
                    mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;
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
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.Center);
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;

                            float numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(25);
                            float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                perturbedSpeed *= randomSpeedOffset;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), (int)((ProjectileDamage * mPlayer.standDamageBoosts) * 0.9f), 3f, player.whoAmI);
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

                LimitDistance(MaxRemoteModeDistance);
            }

            if (spawningField && Projectile.owner == Main.myPlayer)
            {
                float randomAngle = MathHelper.ToRadians(Main.rand.Next(0, 360 + 1));
                Vector2 pointPosition = formPosition + (randomAngle.ToRotationVector2() * EmeraldSplashRadius);
                currentAnimationState = AnimationState.Pose;

                if (amountOfLinksCreated < AmountOfEmeraldSplashLinks && shootCount <= 0)
                {
                    if (!linkShotForSpecial)        //35 tendrils, the number spawned limit /2 is the wanted amount
                    {
                        shootCount += 5;
                        linkShotForSpecial = true;
                        int emeraldIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pointPosition, Vector2.Zero, ModContent.ProjectileType<EmeraldStringPoint>(), 0, 2f, player.whoAmI);
                        Main.projectile[emeraldIndex].netUpdate = true;
                        Main.projectile[emeraldIndex].tileCollide = false;

                    }
                    else
                    {
                        shootCount += 5;
                        amountOfLinksCreated += 1;
                        linkShotForSpecial = false;
                        int emeraldIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pointPosition, Vector2.Zero, ModContent.ProjectileType<EmeraldStringPointConnector>(), 0, 2f, player.whoAmI);
                        Main.projectile[emeraldIndex].netUpdate = true;
                        Main.projectile[emeraldIndex].tileCollide = false;
                        int amountOfDusts = Main._rand.Next(1, 4 + 1);
                        for (int i = 0; i < amountOfDusts; i++)
                        {
                            int dustIndex = Dust.NewDust(pointPosition, 18, 18, DustID.GreenTorch);
                            Main.dust[dustIndex].noGravity = true;
                            Main.dust[dustIndex].velocity = Vector2.Zero;
                        }
                    }
                }
                if (amountOfLinksCreated >= AmountOfEmeraldSplashLinks)
                {
                    amountOfLinksCreated = 0;
                    spawningField = false;
                    formPosition = Vector2.Zero;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                }
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(remoteControlled);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            remoteControlled = reader.ReadBoolean();
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