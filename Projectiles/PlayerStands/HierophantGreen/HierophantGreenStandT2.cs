using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.HierophantGreen
{
    public class HierophantGreenStandT2 : StandClass
    {
        public override int ShootTime => 30;
        public override int ProjectileDamage => 32;
        public override int HalfStandHeight => 30;
        public override int StandOffset => 0;
        public override int TierNumber => 2;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override string PoseSoundName => "ItsTheVictorWhoHasJustice";
        public override string SpawnSoundName => "Hierophant Green";
        public override bool CanUseSaladDye => true;

        private bool pointShot = false;
        private bool remoteControlled = false;

        private const float MaxRemoteModeDistance = 40f * 16f;

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
            if (Main._rand.Next(1, 5 + 1) == 1)
            {
                int index = Dust.NewDust(Projectile.position - new Vector2(0f, HalfStandHeight), Projectile.width, HalfStandHeight * 2, DustID.GreenTorch);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 0.2f;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    if (!mPlayer.canStandBasicAttack)
                        return;

                    attackFrames = true;
                    idleFrames = false;
                    Projectile.netUpdate = true;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        int direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;

                        float numberProjectiles = 4;        //incraeses by 1 each tier
                        float rotation = MathHelper.ToRadians(20);      //increases by 3 every tier
                        float randomSpeedOffset = Main.rand.NextFloat(-6f, 6f);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), newProjectileDamage, 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                        Projectile.netUpdate = true;
                        if (player.velocity.X == 0f)
                            player.ChangeDir(direction);
                    }
                }
                if (!Main.mouseLeft && player.whoAmI == Main.myPlayer)        //The reason it's not an else is because it would count the owner part too
                {
                    idleFrames = true;
                    attackFrames = false;
                }
                if (!attackFrames)
                    StayBehind();
                else
                    GoInFront();

                if (Main.mouseRight && shootCount <= 0 && Projectile.scale >= 0.5f && !playerHasAbilityCooldown && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 30;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    shootVel.Normalize();
                    shootVel *= ProjectileSpeed;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<BindingEmeraldString>(), newProjectileDamage / 2, 0f, Projectile.owner, 20);
                    Main.projectile[proj].netUpdate = true;
                    SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                }

                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0)
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
                    Projectile.velocity *= 3.5f;

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
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    idleFrames = false;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;

                        Projectile.direction = 1;
                        if (Main.MouseWorld.X < Projectile.Center.X)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;

                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;

                        float numberProjectiles = 4;        //incraeses by 1 each tier
                        float rotation = MathHelper.ToRadians(20);      //increases by 3 every tier
                        float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            perturbedSpeed *= randomSpeedOffset;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), newProjectileDamage, 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        attackFrames = false;
                        idleFrames = true;
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
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, connectorType, 0, 3f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                    Projectile.netUpdate = true;
                }

                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0)
                {
                    shootCount += 30;
                    remoteControlled = false;
                    mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                StayBehind();

                NPC target = FindNearestTarget(350f);
                if (target != null)
                {
                    attackFrames = true;
                    idleFrames = false;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0)
                    {
                        Projectile.direction = -1;
                    }
                    Projectile.spriteDirection = Projectile.direction;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Vector2 shootVel = target.position - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;

                            float numberProjectiles = 4;
                            float rotation = MathHelper.ToRadians(20);
                            float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                perturbedSpeed *= randomSpeedOffset;
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Emerald>(), (int)((ProjectileDamage * mPlayer.standDamageBoosts) * 0.9f), 2f, player.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                            }
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    idleFrames = true;
                    attackFrames = false;
                }
                LimitDistance(MaxRemoteModeDistance);
            }
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
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/HierophantGreen", "/HierophantGreen_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 3, 20, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 3, 15, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 2, 15, true);
            }
        }
    }
}