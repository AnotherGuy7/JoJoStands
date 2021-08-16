using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanyChopper : StandClass
    {

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 26;
        }

        public override int standType => 2;

        public int updateTimer = 0;

        private bool setStats = false;
        private new int projectileDamage = 0;
        private new int shootTime = 0;
        private int chopperInaccuracy = 0;

        public override void AI()
        {
            SelectAnimation();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut && modPlayer.badCompanyTier != 0)
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
                if (projectile.ai[0] == 3f)
                {
                    projectileDamage = 49;
                    shootTime = 12;
                    chopperInaccuracy = 25;
                }
                else if (projectile.ai[0] == 4f)
                {
                    projectileDamage = 62;
                    shootTime = 7;
                    chopperInaccuracy = 15;
                }
                setStats = true;

                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
                }
            }

            if (!modPlayer.StandAutoMode)
            {
                MovementAI();
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - modPlayer.standSpeedBoosts + Main.rand.Next(0, 6 + 1);
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 chopperInaccuracyVector = new Vector2(Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1), Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1));
                        Vector2 shootVel = (Main.MouseWorld + chopperInaccuracyVector) - projectile.Center;
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
            if (modPlayer.StandAutoMode)
            {
                MovementAI();
                NPC target = null;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (!npc.friendly && !npc.immortal && npc.lifeMax > 5 && projectile.Distance(npc.Center) <= 26f * 16f)
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
                        shootCount += shootTime - modPlayer.standSpeedBoosts + Main.rand.Next(0, 6 + 1);
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 chopperInaccuracyVector = new Vector2(Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1), Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1));
                        Vector2 shootVel = (target.Center + chopperInaccuracyVector) - projectile.Center;
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
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
        }

        private const float MaxRange = 300f;
        private const float IdleRange = 80f;        //Range in which the chopper is idle

        private void MovementAI()       //Pretty much the pet AI
        {
            Player player = Main.player[projectile.owner];
            PlayAnimation("Chopper");
            Vector2 directionToPlayer = player.Center - projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;

            if (projectile.position.X > player.position.X)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;
            float distance = Vector2.Distance(player.Center, projectile.Center);
            if (distance >= IdleRange)
            {
                if (Math.Abs(player.velocity.X) > 1f || Math.Abs(player.velocity.Y) > 1f)
                {
                    directionToPlayer *= distance / 16f;
                    projectile.velocity = directionToPlayer;
                }
                else
                {
                    directionToPlayer *= 0.9f * (distance / 60f);
                    projectile.velocity = directionToPlayer;
                }
            }
            if (distance >= MaxRange)        //Out of range
            {
                projectile.tileCollide = false;
                directionToPlayer *= distance / 90f;
                projectile.velocity += directionToPlayer;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/BadCompany/BadCompanyChopper");

            AnimateStand(animationName, 2, 15, true);
        }
    }
}