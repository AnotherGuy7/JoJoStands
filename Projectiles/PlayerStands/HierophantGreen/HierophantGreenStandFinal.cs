using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.HierophantGreen
{
    public class HierophantGreenStandFinal : StandClass
    {
        public override int shootTime => 15;
        public override int projectileDamage => 72;
        public override int halfStandHeight => 25;
        public override int standOffset => 0;
        public override int standType => 2;
        public override string poseSoundName => "ItsTheVictorWhoHasJustice";
        public override string spawnSoundName => "Hierophant Green";

        private bool spawningField = false;
        private int numberSpawned = 0;
        private bool linkShot = false;
        private bool linkShotForSpecial = false;
        private Vector2 formPosition = Vector2.Zero;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 35, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            projectile.scale = ((50 - player.ownedProjectileCounts[mod.ProjectileType("EmeraldStringPoint2")]) * 2f) / 100f;

            Vector2 vector131 = player.Center;
            if (!attackFrames)
            {
                vector131.X -= (float)((15 + player.width / 2) * player.direction);
            }
            if (attackFrames)
            {
                vector131.X -= (float)((15 + player.width / 2) * (player.direction * -1));
            }
            vector131.Y -= 5f;
            projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = (projectile.spriteDirection = player.direction);

            if (mPlayer.standOut)
            {
                projectile.timeLeft = 2;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
                    projectile.netUpdate = true;
                    if (shootCount <= 0)
                    {
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        float numberProjectiles = 6;
                        float rotation = MathHelper.ToRadians(30);
                        float randomSpeedOffset = Main.rand.NextFloat(-6f, 6f);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("Emerald"), newProjectileDamage, 3f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
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
                if (Main.mouseRight && shootCount <= 0 && !linkShot && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    shootCount += 15;
                    linkShot = true;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;

                    Vector2 perturbedSpeed = new Vector2(shootVel.X + Main.rand.NextFloat(-3f, 3f), shootVel.Y);
                    int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("EmeraldStringPoint"), 0, 3f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                }
                if (Main.mouseRight && shootCount <= 0 && linkShot && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    shootCount += 15;
                    linkShot = false;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;

                    Vector2 perturbedSpeed = new Vector2(shootVel.X + Main.rand.NextFloat(-3f, 3f), shootVel.Y);
                    int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("EmeraldStringPoint2"), 0, 3f, player.whoAmI, 40f);
                    Main.projectile[proj].netUpdate = true;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                }
                if (SpecialKeyPressed() && player.ownedProjectileCounts[mod.ProjectileType("EmeraldStringPoint2")] <= 0 && !spawningField)
                {
                    spawningField = true;
                    formPosition = projectile.position;
                    if (JoJoStands.SoundsLoaded)
                    {
                        Main.PlaySound(JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/EmeraldSplash"));
                    }
                }
                if (spawningField)
                {
                    float randomRadius = Main.rand.NextFloat(-20f, 21f);
                    Vector2 offset = formPosition + (randomRadius.ToRotationVector2() * 288f);     //33 tiles

                    if (numberSpawned < 100 && shootCount <= 0 && !linkShotForSpecial)        //50 tendrils, the number spawned limit /2 is the wanted amount
                    {
                        shootCount += 2;
                        numberSpawned += 1;
                        int proj = Projectile.NewProjectile(offset, Vector2.Zero, mod.ProjectileType("EmeraldStringPoint"), 0, 2f, player.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        Main.projectile[proj].tileCollide = false;
                        linkShotForSpecial = true;
                    }
                    if (numberSpawned < 100 && shootCount <= 0 && linkShotForSpecial)
                    {
                        shootCount += 2;
                        numberSpawned += 1;
                        int proj = Projectile.NewProjectile(offset, Vector2.Zero, mod.ProjectileType("EmeraldStringPoint2"), 0, 2f, player.whoAmI, 40f);
                        Main.projectile[proj].netUpdate = true;
                        Main.projectile[proj].tileCollide = false;
                        linkShotForSpecial = false;
                    }
                    if (numberSpawned >= 100f)
                    {
                        numberSpawned = 0;
                        spawningField = false;
                        formPosition = Vector2.Zero;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(30));
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
                    if (target.position.X - projectile.Center.X < 0)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        if (Main.myPlayer == projectile.owner)
                        {
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            float numberProjectiles = 6;
                            float rotation = MathHelper.ToRadians(30);
                            float randomSpeedOffset = Main.rand.NextFloat(-6f, 6f);
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("Emerald"), (int)((projectileDamage * mPlayer.standDamageBoosts) * 0.9f), 3f, player.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                            }
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
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode || spawningField)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/HierophantGreen/HierophantGreen_" + animationName);

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