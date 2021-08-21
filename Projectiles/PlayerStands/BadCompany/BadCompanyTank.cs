using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanyTank : StandClass
    {

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 14;
        }

        public override int standType => 2;

        public int updateTimer = 0;

        private bool setStats = false;
        private new int projectileDamage = 0;
        private new float shootSpeed = 12f;
        private new int shootTime = 0;
        private float speedRandom = 0f;

        public override void AI()
        {
            SelectAnimation();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!setStats)
            {
                if (projectile.ai[0] == 2f)
                {
                    projectileDamage = 45;
                    shootTime = 200;
                }
                else if (projectile.ai[0] == 3f)
                {
                    projectileDamage = 67;
                    shootTime = 160;
                }
                else if (projectile.ai[0] == 4f)
                {
                    projectileDamage = 86;
                    shootTime = 120;
                }
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
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts;
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("BadCompanyTankRocket"), 1, 1f, projectile.owner, projectileDamage);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                MovementAI();
                NPC target = null;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (!npc.friendly && !npc.immortal && npc.lifeMax > 5 && projectile.Distance(npc.Center) <= 14f * 16f)
                        {
                            target = npc;
                            break;
                        }
                    }
                }
                if (target != null)
                {
                    if (target.position.X >= projectile.position.X)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts;
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 shootVel = target.Center - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Y -= projectile.Distance(target.position) / 110f;
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("BadCompanyTankRocket"), 1, 1f, projectile.owner, projectileDamage);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
        }

        private const float IdleRange = 15f * 16f;
        private const float MaxFlyingIdleDistance = 10f * 16f;

        private void MovementAI()       //Pretty much the pet AI
        {
            Player player = Main.player[projectile.owner];
            PlayAnimation("Tank");
            Vector2 directionToPlayer = player.Center - projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - projectile.position.X);
            if (!WorldGen.SolidTile((int)(player.position.X / 16f), (int)(player.position.Y / 16f) + 4))
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
                standTexture = mod.GetTexture("Projectiles/PlayerStands/BadCompany/BadCompanyTank");

            AnimateStand(animationName, 1, 20 - (int)projectile.velocity.X, true);
        }
    }
}