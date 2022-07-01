using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanyChopper : StandClass
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 26;
        }

        public override StandType standType => StandType.Ranged;

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
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Projectile.frameCounter++;
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

            if (!setStats)
            {
                if (Projectile.ai[0] == 3f)
                {
                    projectileDamage = 49;
                    shootTime = 12;
                    chopperInaccuracy = 25;
                }
                else if (Projectile.ai[0] == 4f)
                {
                    projectileDamage = 62;
                    shootTime = 7;
                    chopperInaccuracy = 15;
                }
                setStats = true;

                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
                }
            }

            if (!mPlayer.standAutoMode)
            {
                MovementAI();
                if (Main.mouseLeft && mPlayer.canStandBasicAttack && player.whoAmI == Main.myPlayer)
                {
                    if (Main.MouseWorld.X >= Projectile.position.X)
                        Projectile.spriteDirection = Projectile.direction = 1;
                    else
                        Projectile.spriteDirection = Projectile.direction = -1;

                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts + Main.rand.Next(0, 6 + 1);
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 chopperInaccuracyVector = new Vector2(Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1), Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1));
                        Vector2 shootVel = (Main.MouseWorld + chopperInaccuracyVector) - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ProjectileID.Bullet, projectileDamage, 3f, Projectile.owner);
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
                        if (!npc.friendly && !npc.immortal && npc.lifeMax > 5 && Projectile.Distance(npc.Center) <= 26f * 16f)
                        {
                            target = npc;
                            break;
                        }
                    }
                }
                if (target != null)
                {
                    if (target.position.X >= Projectile.position.X)
                        Projectile.spriteDirection = Projectile.direction = 1;
                    else
                        Projectile.spriteDirection = Projectile.direction = -1;
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts + Main.rand.Next(0, 6 + 1);
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 chopperInaccuracyVector = new Vector2(Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1), Main.rand.Next(-chopperInaccuracy, chopperInaccuracy + 1));
                        Vector2 shootVel = (target.Center + chopperInaccuracyVector) - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ProjectileID.Bullet, projectileDamage, 3f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
            Projectile.tileCollide = !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
        }

        private const float MaxRange = 360f;
        private const float IdleRange = 140f;        //Range in which the chopper is idle

        private void MovementAI()       //Pretty much the pet AI
        {
            Player player = Main.player[Projectile.owner];
            PlayAnimation("Chopper");
            Vector2 directionToPlayer = player.Center - Projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;

            if (Projectile.position.X > player.position.X)
                Projectile.direction = -1;
            else
                Projectile.direction = 1;
            Projectile.spriteDirection = Projectile.direction;

            Projectile.velocity *= 0.99f;
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (distance >= IdleRange)
            {
                if (Math.Abs(player.velocity.X) > 1f || Math.Abs(player.velocity.Y) > 1f)
                {
                    directionToPlayer *= distance / (IdleRange / 2f);
                    Projectile.velocity += directionToPlayer;
                }
                else
                {
                    directionToPlayer *= 0.5f * (distance / 160f);
                    Projectile.velocity += directionToPlayer;
                }
            }
            if (distance >= MaxRange)        //Out of range
            {
                Projectile.tileCollide = false;
                directionToPlayer *= distance / 90f;
                Projectile.velocity += directionToPlayer;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanyChopper");

            AnimateStand(animationName, 2, 15, true);
        }
    }
}