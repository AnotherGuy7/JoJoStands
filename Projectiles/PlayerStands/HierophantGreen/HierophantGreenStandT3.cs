using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.HierophantGreen
{  
    public class HierophantGreenStandT3 : StandClass
    {
        public override int shootTime => 20;
        public override int projectileDamage => 56;
        public override int halfStandHeight => 30;
        public override int standOffset => 0;
        public override int standType => 2;
        public override string poseSoundName => "ItsTheVictorWhoHasJustice";
        public override string spawnSoundName => "Hierophant Green";

        private bool spawningField = false;
        private int numberSpawned = 0;
        private bool pointShot = false;
        private bool remotelyControlled = false;
        private bool linkShotForSpecial = false;
        private Vector2 formPosition = Vector2.Zero;

        private const float MaxRemoteModeDistance = 45f * 16f;

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

            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 35, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            projectile.scale = ((50 - player.ownedProjectileCounts[mod.ProjectileType("EmeraldStringPointConnector")]) * 2f) / 100f;

            if (!mPlayer.standAutoMode && !remotelyControlled)
            {
                if (Main.mouseLeft && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
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

                        float numberProjectiles = 5;
                        float rotation = MathHelper.ToRadians(25);
                        float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            perturbedSpeed *= randomSpeedOffset;
                            int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("Emerald"), newProjectileDamage, 3f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        projectile.netUpdate = true;
                    }
                }
                if (!Main.mouseLeft && player.whoAmI == Main.myPlayer)        //The reason it's not an else is because it would count the owner part too
                {
                    normalFrames = true;
                    attackFrames = false;
                }
                if (!attackFrames)
                    StayBehind();
                else
                    GoInFront();

                if (Main.mouseRight && shootCount <= 0 && projectile.scale >= 0.5f && !playerHasAbilityCooldown && projectile.owner == Main.myPlayer)
                {
                    shootCount += 30;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("BindingEmeraldString"), newProjectileDamage / 2, 0f, projectile.owner, 25);
                    Main.projectile[proj].netUpdate = true;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(5));
                }

                if (SpecialKeyPressed() && player.ownedProjectileCounts[mod.ProjectileType("EmeraldStringPointConnector")] <= 0 && !spawningField)
                {
                    spawningField = true;
                    formPosition = projectile.position;
                    if (JoJoStands.SoundsLoaded)
                    {
                        Main.PlaySound(JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/EmeraldSplash"));
                    }
                }
                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0)
                {
                    shootCount += 30;
                    remotelyControlled = true;
                }
            }
            if (!mPlayer.standAutoMode && remotelyControlled)
            {
                mPlayer.standRemoteMode = true;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    projectile.velocity = Main.MouseWorld - projectile.Center;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 4.5f;

                    projectile.direction = 1;
                    if (Main.MouseWorld.X < projectile.Center.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.netUpdate = true;
                    projectile.spriteDirection = projectile.direction;
                    LimitDistance(MaxRemoteModeDistance);
                }
                if (!Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    projectile.velocity *= 0.78f;
                    projectile.netUpdate = true;
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;

                        projectile.direction = 1;
                        if (Main.MouseWorld.X < projectile.Center.X)
                        {
                            projectile.direction = -1;
                        }
                        projectile.spriteDirection = projectile.direction;

                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;

                        float numberProjectiles = 5;
                        float rotation = MathHelper.ToRadians(25);
                        float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            perturbedSpeed *= randomSpeedOffset;
                            int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("Emerald"), newProjectileDamage, 3f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (projectile.owner == Main.myPlayer)
                    {
                        attackFrames = false;
                        normalFrames = true;
                    }
                }
                if (SpecialKeyPressed() && shootCount <= 0 && projectile.scale >= 0.5f)
                {
                    pointShot = !pointShot;
                    int connectorType = mod.ProjectileType("EmeraldStringPoint");
                    if (!pointShot)
                        connectorType = mod.ProjectileType("EmeraldStringPointConnector");

                    shootCount += 15;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(projectile.Center, shootVel, connectorType, 0, 3f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                    projectile.netUpdate = true;
                }

                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0)
                {
                    shootCount += 30;
                    remotelyControlled = false;
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

                            float numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(25);
                            float randomSpeedOffset = (100f + Main.rand.NextFloat(-6f, 6f)) / 100f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                perturbedSpeed *= randomSpeedOffset;
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
                LimitDistance(MaxRemoteModeDistance);
            }

            if (spawningField && projectile.owner == Main.myPlayer)
            {
                float randomRadius = Main.rand.NextFloat(-20f, 21f);
                Vector2 pointPosition = formPosition + (randomRadius.ToRotationVector2() * 288f);     //33 tiles

                if (numberSpawned < 70 && shootCount <= 0 && !linkShotForSpecial)        //35 tendrils, the number spawned limit /2 is the wanted amount
                {
                    shootCount += 5;
                    numberSpawned += 1;
                    int proj = Projectile.NewProjectile(pointPosition, Vector2.Zero, mod.ProjectileType("EmeraldStringPoint"), 0, 2f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    Main.projectile[proj].tileCollide = false;
                    linkShotForSpecial = true;
                }
                if (numberSpawned < 70 && shootCount <= 0 && linkShotForSpecial)
                {
                    shootCount += 5;
                    numberSpawned += 1;
                    int proj = Projectile.NewProjectile(pointPosition, Vector2.Zero, mod.ProjectileType("EmeraldStringPointConnector"), 0, 2f, player.whoAmI, 24f);
                    Main.projectile[proj].netUpdate = true;
                    Main.projectile[proj].tileCollide = false;
                    linkShotForSpecial = false;
                }
                if (numberSpawned >= 70)
                {
                    numberSpawned = 0;
                    spawningField = false;
                    formPosition = Vector2.Zero;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(30));
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