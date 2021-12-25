using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanySoldier : StandClass
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 26;
        }

        public override int standType => 2;
        public override float shootSpeed => 12f;

        public int updateTimer = 0;

        private bool setStats = false;
        private new int projectileDamage = 0;
        private new int shootTime = 0;
        private float speedRandom = 0f;     //So the AI isn't always the same
        private int centerDistance = 30;      //Height of the center of the projectile (used for 

        public override void AI()
        {
            SelectAnimation();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!setStats)
            {
                if (projectile.ai[0] == 1f)
                {
                    projectileDamage = 9;
                    shootTime = 90;
                }
                else if (projectile.ai[0] == 2f)
                {
                    projectileDamage = 18;
                    shootTime = 80;
                }
                else if (projectile.ai[0] == 3f)
                {
                    projectileDamage = 31;
                    shootTime = 70;
                }
                else if (projectile.ai[0] == 4f)
                {
                    projectileDamage = 40;
                    shootTime = 60;
                }
                shootTime += Main.rand.Next(0, 15 + 1);
                speedRandom = Main.rand.NextFloat(-0.05f, 0.05f);
                setStats = true;

                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
                }
            }

            if (!mPlayer.standAutoMode)
            {
                MovementAI();
                if (projectile.ai[0] == 0f)     //Here because it's different for Auto Mode
                {
                    if (Main.MouseWorld.Y > projectile.position.Y + centerDistance)
                    {
                        PlayAnimation("AimDown");
                    }
                    else if (Main.MouseWorld.Y < projectile.position.Y - centerDistance)
                    {
                        PlayAnimation("AimUp");
                    }
                }
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
                {
                    projectile.direction = 1;
                    if (Main.MouseWorld.X <= projectile.position.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts + Main.rand.Next(-3, 3 + 1);
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, ProjectileID.Bullet, projectileDamage, 3f, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(17f * 16f);
                if (target != null)
                {
                    if (projectile.ai[0] == 0f)
                    {
                        if (target.position.Y > projectile.position.Y + centerDistance)
                        {
                            PlayAnimation("AimDown");
                        }
                        if (target.position.Y < projectile.position.Y - centerDistance)
                        {
                            PlayAnimation("AimUp");
                        }
                    }

                    projectile.direction = 1;
                    if (target.position.X <= projectile.position.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts + Main.rand.Next(-3, 3 + 1);
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 shootVel = target.Center - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, ProjectileID.Bullet, projectileDamage, 3f, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
                else
                {
                    MovementAI();
                }
            }

            projectile.tileCollide = !Collision.SolidCollision(projectile.position, projectile.width, projectile.height);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
        }

        private const float IdleRange = 8 * 16f;
        private const float MaxFlyingIdleDistance = 6 * 16f;

        private void MovementAI()       //Pretty much the pet AI
        {
            Player player = Main.player[projectile.owner];
            Vector2 directionToPlayer = player.Center - projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - projectile.position.X);
            if (!WorldGen.SolidTile((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f) + 2))
            {
                projectile.ai[0] = 1f;
            }
            else
            {
                projectile.ai[0] = 0f;
            }

            if (projectile.position.X > player.position.X)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;

            if (projectile.ai[0] == 0f)
            {
                PlayAnimation("Walk");
                projectile.tileCollide = true;
                if (projectile.velocity.Y < 6f)
                {
                    projectile.velocity.Y += 0.3f;
                }

                if (xDist >= IdleRange)
                {
                    projectile.velocity.X = directionToPlayer.X * xDist / 14;
                }
                else
                {
                    projectile.velocity.X *= 0.96f + speedRandom;
                }
            }
            float distance = Vector2.Distance(player.Center, projectile.Center);
            if (projectile.ai[0] == 1f)        //Flying
            {
                PlayAnimation("Parachute");
                if (distance >= MaxFlyingIdleDistance)
                {
                    if (Math.Abs(player.velocity.X) > 1f || Math.Abs(player.velocity.Y) > 1f)
                    {
                        directionToPlayer *= distance / 16f;
                        projectile.velocity = directionToPlayer;
                    }
                    else
                    {
                        directionToPlayer *= (0.9f + speedRandom) * (distance / 60f);
                        projectile.velocity = directionToPlayer;
                    }
                }
            }
            if (distance >= 300f)        //Out of range
            {
                projectile.tileCollide = false;
                directionToPlayer *= distance / 90f;
                projectile.velocity += directionToPlayer;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/BadCompany/BadCompanySoldier_" + animationName);

            if (animationName == "Parachute")
            {
                AnimateStand(animationName, 1, 120, true);
            }
            if (animationName == "Prone")
            {
                AnimateStand(animationName, 1, 120, true);
            }
            if (animationName == "AimUp")
            {
                AnimateStand(animationName, 1, 120, true);
            }
            if (animationName == "AimDown")
            {
                AnimateStand(animationName, 1, 120, true);
            }
            if (animationName == "Walk")
            {
                AnimateStand(animationName, 4, 20 - (int)projectile.velocity.X, true);
            }
        }
    }
}