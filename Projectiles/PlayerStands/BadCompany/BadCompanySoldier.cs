using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.UI;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanySoldier : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 26;
            Projectile.shouldFallThrough = false;
        }

        public override string poseSoundName => "StandReadyFire";
        public override StandType standType => StandType.Ranged;
        public override float shootSpeed => 12f;

        public int updateTimer = 0;

        private bool setStats = false;
        private new int projectileDamage = 0;
        private new int shootTime = 0;
        private float speedRandom = 0f;     //So the AI isn't always the same
        private int centerDistance = 30;      //Height of the center of the Projectile
        private int stabCooldownTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;
            if (stabCooldownTimer > 0)
                stabCooldownTimer--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.poseSoundName = poseSoundName;
            if (mPlayer.standOut && mPlayer.badCompanyTier != 0)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

            if (!setStats)
            {
                if (Projectile.ai[0] == 1f)
                {
                    projectileDamage = ((int)(9 * mPlayer.standDamageBoosts));
                    shootTime = 90;
                }
                else if (Projectile.ai[0] == 2f)
                {
                    projectileDamage = ((int)(18 * mPlayer.standDamageBoosts));
                    shootTime = 80;
                }
                else if (Projectile.ai[0] == 3f)
                {
                    projectileDamage = ((int)(31 * mPlayer.standDamageBoosts));
                    shootTime = 70;
                }
                else if (Projectile.ai[0] == 4f)
                {
                    projectileDamage = ((int)(40 * mPlayer.standDamageBoosts));
                    shootTime = 60;
                }
                shootTime += Main.rand.Next(0, 15 + 1);
                speedRandom = Main.rand.NextFloat(-0.03f, 0.03f);
                setStats = true;

                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 16, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
                }
            }

            if (!mPlayer.standAutoMode)
            {
                MovementAI();
                if (Projectile.ai[0] == 0f)     //Here because it's different for Auto Mode
                {
                    if (Main.MouseWorld.Y > Projectile.position.Y + centerDistance)
                    {
                        PlayAnimation("AimDown");
                    }
                    else if (Main.MouseWorld.Y < Projectile.position.Y - centerDistance)
                    {
                        PlayAnimation("AimUp");
                    }
                }
                if (Main.mouseLeft && mPlayer.canStandBasicAttack && player.whoAmI == Main.myPlayer && !BadCompanyUnitsUI.Visible)
                {
                    Projectile.direction = 1;
                    if (Main.MouseWorld.X <= Projectile.position.X)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    NPC targetNPC = null;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && npc.lifeMax > 5 && !npc.townNPC && Vector2.Distance(npc.Center, Main.MouseWorld) <= 24f && Vector2.Distance(npc.Center, Projectile.Center) <= 3 * 16f)
                        {
                            targetNPC = npc;
                            break;
                        }
                    }

                    if (targetNPC != null)
                    {
                        Vector2 velocity = targetNPC.Center - Projectile.Center;
                        velocity.Normalize();
                        velocity *= 1.3f;
                        Projectile.velocity = velocity;
                        if (stabCooldownTimer <= 0)
                        {
                            PlayAnimation("Stab");
                            targetNPC.StrikeNPC((int)(projectileDamage * 1.5f), 1.2f, Projectile.damage);
                            stabCooldownTimer += 45 + Main.rand.Next(1, 6 + 1);
                        }
                    }
                    else
                    {
                        if (Main.MouseWorld.X >= Projectile.position.X)
                            Projectile.spriteDirection = Projectile.direction = 1;
                        else
                            Projectile.spriteDirection = Projectile.direction = -1;

                        if (shootCount <= 0)
                        {
                            shootCount += shootTime - mPlayer.standSpeedBoosts + Main.rand.Next(-3, 3 + 1);
                            SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), projectileDamage, 3f, Projectile.owner);
                            Main.projectile[proj].netUpdate = true;
                        }
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(17f * 16f);
                if (target != null)
                {
                    if (Projectile.ai[0] == 0f)
                    {
                        if (target.position.Y > Projectile.position.Y + centerDistance)
                        {
                            PlayAnimation("AimDown");
                        }
                        if (target.position.Y < Projectile.position.Y - centerDistance)
                        {
                            PlayAnimation("AimUp");
                        }
                    }

                    Projectile.direction = 1;
                    if (target.position.X <= Projectile.position.X)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts + Main.rand.Next(-3, 3 + 1);
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 shootVel = target.Center - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), projectileDamage, 3f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
                else
                {
                    MovementAI();
                }
            }

            Projectile.tileCollide = !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.velocity.Y = 0f;
                Projectile.position.Y -= 2f;
            }

            if (mPlayer.poseDuration <= 20 && !player.controlUseItem)
            {
                if (mPlayer.poseDuration >= 18 && shootCount == 0)
                    shootCount += Main.rand.Next(0, 20 + 1);

                if (shootCount <= 0)
                {
                    shootCount += mPlayer.poseDuration + 1;
                    SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0f, -shootSpeed), ProjectileID.Bullet, 1, 0f, Projectile.owner);
                    Main.projectile[proj].netUpdate = true;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
        }

        private const float IdleRange = 8 * 16f;
        private const float MaxFlyingIdleDistance = 6 * 16f;

        private void MovementAI()       //Pretty much the pet AI
        {
            Player player = Main.player[Projectile.owner];
            Vector2 directionToPlayer = player.Center - Projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - Projectile.position.X);
            bool standingOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f) + 1].TileType];
            if (!WorldGen.SolidTile((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f) + 2) && !standingOnPlatform)
            {
                Projectile.ai[0] = 1f;
            }
            else
            {
                Projectile.ai[0] = 0f;
            }

            if (Projectile.position.X > player.position.X)
                Projectile.direction = -1;
            else
                Projectile.direction = 1;
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.ai[0] == 0f)
            {
                PlayAnimation("Walk");
                Projectile.tileCollide = true;
                if (Projectile.velocity.Y < 6f)
                    Projectile.velocity.Y += 0.3f;

                if (xDist >= IdleRange)
                    Projectile.velocity.X = directionToPlayer.X * xDist / 14;
                else
                    Projectile.velocity.X *= 0.96f + speedRandom;
            }

            Projectile.velocity *= 0.99f;
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (Projectile.ai[0] == 1f)        //Flying
            {
                PlayAnimation("Parachute");
                Projectile.velocity.Y += 0.03f;
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
                directionToPlayer *= distance / 90f;
                Projectile.velocity += directionToPlayer;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanySoldier_" + animationName);

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
            if (animationName == "Stab")
            {
                AnimateStand(animationName, 4, 20 - (int)Projectile.velocity.X, true);
            }
            if (animationName == "Walk")
            {
                AnimateStand(animationName, 4, 20 - (int)Projectile.velocity.X, true);
            }
        }
    }
}