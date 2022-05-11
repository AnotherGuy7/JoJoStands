using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanyTank : StandClass
    {

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 14;
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
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Projectile.frameCounter++;
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
            {
                Projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

            if (!setStats)
            {
                if (Projectile.ai[0] == 2f)
                {
                    projectileDamage = 45;
                    shootTime = 200;
                }
                else if (Projectile.ai[0] == 3f)
                {
                    projectileDamage = 67;
                    shootTime = 160;
                }
                else if (Projectile.ai[0] == 4f)
                {
                    projectileDamage = 86;
                    shootTime = 120;
                }
                speedRandom = Main.rand.NextFloat(-0.05f, 0.05f);
                setStats = true;

                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
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
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<BadCompanyTankRocket>(), 1, 1f, Projectile.owner, projectileDamage);
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
                        if (!npc.friendly && !npc.immortal && npc.lifeMax > 5 && Projectile.Distance(npc.Center) <= 14f * 16f)
                        {
                            target = npc;
                            break;
                        }
                    }
                }
                if (target != null)
                {
                    if (target.position.X >= Projectile.position.X)
                    {
                        Projectile.spriteDirection = Projectile.direction = 1;
                    }
                    else
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 shootVel = target.Center - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Y -= Projectile.Distance(target.position) / 110f;
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<BadCompanyTankRocket>(), 1, 1f, Projectile.owner, projectileDamage);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
            Projectile.tileCollide = !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
        }

        private const float IdleRange = 15f * 16f;
        private const float MaxFlyingIdleDistance = 10f * 16f;

        private void MovementAI()       //Pretty much the pet AI
        {
            Player player = Main.player[Projectile.owner];
            PlayAnimation("Tank");
            Vector2 directionToPlayer = player.Center - Projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - Projectile.position.X);
            if (!WorldGen.SolidTile((int)(player.position.X / 16f), (int)(player.position.Y / 16f) + 4))
            {
                Projectile.ai[0] = 1f;
            }
            else
            {
                Projectile.ai[0] = 0f;
            }

            if (Projectile.position.X > player.position.X)
            {
                Projectile.direction = -1;
            }
            else
            {
                Projectile.direction = 1;
            }
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.ai[0] == 0f)
            {
                Projectile.tileCollide = true;
                if (Projectile.velocity.Y < 6f)
                {
                    Projectile.velocity.Y += 0.3f;
                }

                if (xDist >= IdleRange)
                {
                    Projectile.velocity.X = directionToPlayer.X * xDist / 14;
                }
                else
                {
                    Projectile.velocity.X *= 0.96f + speedRandom;
                }
            }
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (Projectile.ai[0] == 1f)        //Flying
            {
                if (distance >= MaxFlyingIdleDistance)
                {
                    if (Math.Abs(player.velocity.X) > 1f || Math.Abs(player.velocity.Y) > 1f)
                    {
                        directionToPlayer *= distance / 16f;
                        Projectile.velocity = directionToPlayer;
                    }
                    else
                    {
                        directionToPlayer *= (0.9f + speedRandom) * (distance / 60f);
                        Projectile.velocity = directionToPlayer;
                    }
                }
            }
            if (distance >= 300f)        //Out of range
            {
                Projectile.tileCollide = false;
                Projectile.velocity = (player.velocity * 1.4f) + directionToPlayer;
                if (distance >= 360f)
                    Projectile.Center = player.Center;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("Projectiles/PlayerStands/BadCompany/BadCompanyTank");

            AnimateStand(animationName, 2, 36 - (int)Projectile.velocity.X, true);
        }
    }
}