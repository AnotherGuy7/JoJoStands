using Microsoft.Xna.Framework;
using Terraria;
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

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            bool redBindActive = secondaryAbilityFrames = player.ownedProjectileCounts[mod.ProjectileType("RedBind")] != 0;
            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !redBindActive)
                {
                    attackFrames = true;
                    projectile.netUpdate = true;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("FireAnkh"), newProjectileDamage, 3f, projectile.owner, chanceToDebuff, debuffDuration);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
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
                if (Main.mouseRight && projectile.owner == Main.myPlayer && !redBindActive && !player.HasBuff(mod.BuffType("AbilityCooldown")))
                {
                    secondaryAbilityFrames = true;
                    if (JoJoStands.SoundsLoaded)
                    {
                        Terraria.Audio.LegacySoundStyle redBind = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/RedBind");
                        redBind.WithVolume(MyPlayer.ModSoundsVolume);
                        Main.PlaySound(redBind, projectile.position);
                    }

                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 16f;
                    int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("RedBind"), newProjectileDamage, 3f, projectile.owner, projectile.whoAmI, debuffDuration - 60);
                    Main.projectile[proj].netUpdate = true;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(10));
                    projectile.netUpdate = true;
                }
                if (SpecialKeyPressed())
                {
                    for (int p = 1; p <= 50; p++)
                    {
                        float radius = p * 5;
                        Vector2 offset = player.Center + (radius.ToRotationVector2() * 48f);
                        int proj = Projectile.NewProjectile(offset.X, offset.Y, 0f, 0f, mod.ProjectileType("CrossfireHurricaneAnkh"), newProjectileDamage, 5f, projectile.owner, 48f, radius);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                    if (JoJoStands.SoundsLoaded)
                    {
                        Terraria.Audio.LegacySoundStyle crossFireHurricane = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/CrossfireHurricaneSpecial");
                        crossFireHurricane.WithVolume(MyPlayer.ModSoundsVolume);
                        Main.PlaySound(crossFireHurricane, projectile.position);
                    }
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(25));
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
                            int proj = Projectile.NewProjectile(offset.X, offset.Y, 0f, 0f, mod.ProjectileType("CrossfireHurricaneAnkh"), newProjectileDamage, 5f, projectile.owner, 48f, -radius);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
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

                    projectile.direction = 1;
                    if (target.position.X - projectile.position.X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    projectile.velocity = target.Center - projectile.Center;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 4f;
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("FireAnkh"), newProjectileDamage, 3f, projectile.owner, chanceToDebuff, debuffDuration);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
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
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
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
                standTexture = mod.GetTexture("Projectiles/PlayerStands/MagiciansRed/MagiciansRed_" + animationName);

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