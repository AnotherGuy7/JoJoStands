using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
 
namespace JoJoStands.Projectiles.PlayerStands.HierophantGreen
{  
    public class HierophantGreenStandT2 : StandClass
    {
        public override int shootTime => 30;
        public override int projectileDamage => 32;
        public override int halfStandHeight => 25;
        public override int standOffset => 0;
        public override int standType => 2;
        public override string poseSoundName => "ItsTheVictorWhoHasJustice";
        public override string spawnSoundName => "Hierophant Green";

        private bool linkShot = false;
        private bool remotelyControlled = false;

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

            Vector2 playerCenter = player.Center;
            if (!attackFrames)
            {
                playerCenter.X -= (float)((15 + player.width / 2) * player.direction);
            }
            if (attackFrames)
            {
                playerCenter.X -= (float)((15 + player.width / 2) * (player.direction * -1));
            }
            playerCenter.Y -= 5f;
            projectile.Center = Vector2.Lerp(projectile.Center, playerCenter, 0.2f);
            projectile.velocity *= 0.8f;
            projectile.direction = (projectile.spriteDirection = player.direction);

            if (mPlayer.standOut)
            {
                projectile.timeLeft = 2;
            }

            if (!mPlayer.standAutoMode && !remotelyControlled)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
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

                        float numberProjectiles = 4;        //incraeses by 1 each tier
                        float rotation = MathHelper.ToRadians(20);      //increases by 3 every tier
                        float randomSpeedOffset = Main.rand.NextFloat(-6f, 6f);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("Emerald"), newProjectileDamage, 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        Main.PlaySound(SoundID.Item21, projectile.position);
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
                if (Main.mouseRight && shootCount <= 0 && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    linkShot = true;
                    shootCount += 15;
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

                if (SecondSpecialKeyPressed())
                    remotelyControlled = !remotelyControlled;
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
                    projectile.velocity *= 8f;

                    if (Vector2.Distance(projectile.Center, player.Center) >= 40f * 16f)
                    {
                        projectile.velocity = player.Center - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 0.8f;
                    }
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
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

                        float numberProjectiles = 4;        //incraeses by 1 each tier
                        float rotation = MathHelper.ToRadians(20);      //increases by 3 every tier
                        float randomSpeedOffset = Main.rand.NextFloat(-6f, 6f);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, mod.ProjectileType("Emerald"), newProjectileDamage, 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        projectile.netUpdate = true;
                    }
                }
                if (SpecialKeyPressed() && shootCount <= 0 && projectile.scale >= 0.5f)
                {
                    linkShot = !linkShot;
                    int connectorType = mod.ProjectileType("EmeraldStringPoint");
                    if (!linkShot)
                        connectorType = mod.ProjectileType("EmeraldStringPoint2");

                    shootCount += 15;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    Vector2 perturbedSpeed = new Vector2(shootVel.X + Main.rand.NextFloat(-3f, 3f), shootVel.Y);
                    int proj = Projectile.NewProjectile(projectile.Center, perturbedSpeed, connectorType, 0, 3f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                }

                if (SecondSpecialKeyPressed())
                    remotelyControlled = !remotelyControlled;
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

                            float numberProjectiles = 4;
                            float rotation = MathHelper.ToRadians(20);
                            float randomSpeedOffset = Main.rand.NextFloat(-6f, 6f);
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Emerald"), (int)((projectileDamage * mPlayer.standDamageBoosts) * 0.9f), 2f, player.whoAmI);
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
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
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