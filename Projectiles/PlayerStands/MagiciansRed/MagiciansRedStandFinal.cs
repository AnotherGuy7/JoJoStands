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
        public override float shootSpeed => 8f;
        public override int standType => 2;
        public override int projectileDamage => 95;
        public override int shootTime => 14;
        public override int halfStandHeight => 35;
        public override int standOffset => 0;
        public override string poseSoundName => "ThePowerToWieldFlameAtWill";
        public override string spawnSoundName => "Magicians Red";

        private int chanceToDebuff = 60;
        private int debuffDuration = 480;
        private int secondRingTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            bool redBindActive = secondaryAbilityFrames = player.ownedProjectileCounts[ModContent.ProjectileType<RedBind>()] != 0;
            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !redBindActive)
                {
                    attackFrames = true;
                    Projectile.netUpdate = true;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<FireAnkh>(), newProjectileDamage, 3f, Projectile.owner, chanceToDebuff, debuffDuration);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !redBindActive && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    secondaryAbilityFrames = true;
                    if (JoJoStands.SoundsLoaded)
                    {
                        SoundStyle redBind = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/RedBind");
                        redBind.Volume = MyPlayer.ModSoundsVolume;
                        SoundEngine.PlaySound(redBind, Projectile.position);
                    }

                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 16f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<RedBind>(), newProjectileDamage, 3f, Projectile.owner, Projectile.whoAmI, debuffDuration - 60);
                    Main.projectile[proj].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                    Projectile.netUpdate = true;
                }
                if (SpecialKeyPressed())
                {
                    for (int p = 1; p <= 50; p++)
                    {
                        float radius = p * 5;
                        Vector2 offset = player.Center + (radius.ToRotationVector2() * 48f);
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset.X, offset.Y, 0f, 0f, ModContent.ProjectileType<CrossfireHurricaneAnkh>(), newProjectileDamage, 5f, Projectile.owner, 48f, radius);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                    if (JoJoStands.SoundsLoaded)
                    {
                        SoundStyle crossFireHurricane = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/CrossfireHurricaneSpecial");
                        crossFireHurricane.Volume = MyPlayer.ModSoundsVolume;
                        SoundEngine.PlaySound(crossFireHurricane, Projectile.Center);
                    }
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(25));
                    secondRingTimer = 1;
                }
                if (secondRingTimer != 0)
                {
                    secondRingTimer++;
                    if (secondRingTimer >= 40)
                    {
                        for (int p = 1; p <= 25; p++)
                        {
                            float radius = p * 5;
                            Vector2 offset = player.Center + (radius.ToRotationVector2() * 48f);
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), offset.X, offset.Y, 0f, 0f, ModContent.ProjectileType<CrossfireHurricaneAnkh>(), newProjectileDamage, 5f, Projectile.owner, 48f, -radius);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                        secondRingTimer = 0;
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(350f);
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;

                    Projectile.direction = 1;
                    if (target.position.X - Projectile.position.X < 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
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
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<FireAnkh>(), newProjectileDamage, 3f, Projectile.owner, chanceToDebuff, debuffDuration);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    normalFrames = true;
                    attackFrames = false;
                }
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/MagiciansRed/MagiciansRed_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newShootTime / 2, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}