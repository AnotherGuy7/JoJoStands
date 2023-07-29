using JoJoStands.Dusts;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.BadCompany
{
    public class BadCompanyTank : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 14;
            Projectile.shouldFallThrough = false;
        }

        public override StandAttackType StandType => StandAttackType.Ranged;
        public override Vector2 StandOffset => new Vector2(8f, -12f);

        public int updateTimer = 0;

        private bool setStats = false;
        private int projectileDamage = 0;
        private float shootSpeed = 12f;
        private int shootTime = 0;
        private float speedRandom = 0f;
        private float barrelRotation;
        private Texture2D barrelTexture;
        private readonly Vector2 BarrelOrigin = new Vector2(3, 5);
        private readonly Vector2 BarrelPlacementOffset = new Vector2(12f, -1.5f);

        public override void AI()
        {
            SelectAnimation();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Projectile.frameCounter++;
            if (mPlayer.standOut && mPlayer.standTier != 0)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

            if (!setStats)
            {
                if (Projectile.ai[0] == 2f)
                {
                    projectileDamage = 76;
                    shootTime = 200;
                }
                else if (Projectile.ai[0] == 3f)
                {
                    projectileDamage = 125;
                    shootTime = 160;
                }
                else if (Projectile.ai[0] == 4f)
                {
                    projectileDamage = 187;
                    shootTime = 120;
                }
                speedRandom = Main.rand.NextFloat(-0.03f, 0.03f);
                setStats = true;

                int amountOfParticles = Main.rand.Next(1, 2);
                int[] dustTypes = new int[3] { ModContent.DustType<StandSummonParticles>(), ModContent.DustType<StandSummonShine1>(), ModContent.DustType<StandSummonShine2>() };
                Vector2 dustSpawnOffset = StandOffset;
                dustSpawnOffset.X *= Projectile.spriteDirection;
                for (int i = 0; i < amountOfParticles; i++)
                {
                    int dustType = dustTypes[Main.rand.Next(0, 3)];
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Scale: (float)Main.rand.Next(80, 120) / 100f);
                }
                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                MovementAI();
                if (Projectile.owner == Main.myPlayer)
                {
                    barrelRotation = (Main.MouseWorld - Projectile.Center).ToRotation();
                    Projectile.direction = 1;
                    if (Main.MouseWorld.X <= Projectile.position.X)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    if (Main.mouseLeft && mPlayer.canStandBasicAttack && player.whoAmI == Main.myPlayer && !BadCompanyUnitsUI.Visible)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += shootTime - mPlayer.standSpeedBoosts;
                            SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<BadCompanyTankRocket>(), (int)(projectileDamage * mPlayer.standDamageBoosts), 1f, Projectile.owner, (int)(projectileDamage * mPlayer.standDamageBoosts));
                            Main.projectile[projIndex].netUpdate = true;
                        }
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                MovementAI();
                float detectionDistance = (22 + (2 * mPlayer.standTier)) * 16f;
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
                        Projectile.spriteDirection = Projectile.direction = 1;
                    else
                        Projectile.spriteDirection = Projectile.direction = -1;
                    barrelRotation = (target.Center - Projectile.Center).ToRotation();

                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - mPlayer.standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 shootVel = target.Center - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Y -= Projectile.Distance(target.position) / 110f;
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<BadCompanyTankRocket>(), (int)(projectileDamage * mPlayer.standDamageBoosts), 1f, Projectile.owner, (int)(projectileDamage * mPlayer.standDamageBoosts));
                        Main.projectile[projIndex].netUpdate = true;
                    }
                }
                else
                {
                    if (Projectile.velocity.X > 0f)
                        Projectile.spriteDirection = Projectile.direction = 1;
                    else if (Projectile.velocity.X < 0f)
                        Projectile.spriteDirection = Projectile.direction = -1;

                    if (Projectile.direction == -1)
                        barrelRotation = MathHelper.Pi;
                    else
                        barrelRotation = 0f;
                }

            }
            if (!mPlayer.standOut)
                Projectile.Kill();
            Projectile.tileCollide = !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.velocity.Y = 0f;
                Projectile.position.Y -= 2f;
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(barrelRotation);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            barrelRotation = reader.ReadSingle();
        }

        public override void PostDrawExtras()
        {
            if (barrelTexture == null && Main.netMode != NetmodeID.Server)
                barrelTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanyTank_Barrel", AssetRequestMode.ImmediateLoad);
            Vector2 origin = BarrelOrigin;
            float currentRota = barrelRotation + MathHelper.Pi;
            SpriteEffects effect = SpriteEffects.None;
            if (currentRota < MathHelper.PiOver2 || currentRota > MathHelper.Pi * 3 / 2f)
            {
                effect = SpriteEffects.FlipVertically;
                origin = new Vector2(2f, 3f);
            }
            Vector2 offset = BarrelPlacementOffset;
            if (currentAnimationState == AnimationState.Idle && Projectile.frame == 1)
                offset.Y -= 2;
            if (Projectile.spriteDirection == -1)
                offset.Y += 2;
            Main.EntitySpriteDraw(barrelTexture, Projectile.Center + (offset * new Vector2(Projectile.spriteDirection, 1f)) - Main.screenPosition, null, Color.White, currentRota + MathHelper.Pi, origin, Projectile.scale, effect, 0f);
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
            Vector2 directionToPlayer = player.Center - Projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - Projectile.position.X);
            if (!WorldGen.SolidTile((int)(player.position.X / 16f), (int)(player.position.Y / 16f) + 4))
                Projectile.ai[0] = 1f;
            else
                Projectile.ai[0] = 0f;

            if (Projectile.position.X > player.position.X)
                Projectile.direction = -1;
            else
                Projectile.direction = 1;

            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.ai[0] == 0f)
            {
                currentAnimationState = AnimationState.Idle;
                Projectile.tileCollide = true;
                if (Projectile.velocity.Y < 6f)
                    Projectile.velocity.Y += 0.3f;

                if (xDist >= IdleRange)
                    Projectile.velocity.X = directionToPlayer.X * xDist / 14;
                else
                    Projectile.velocity.X *= 0.96f + speedRandom;
            }

            Projectile.velocity *= 0.98f;
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (Projectile.ai[0] == 1f)        //Flying
            {
                currentAnimationState = AnimationState.Special;
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

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Special)
                PlayAnimation("Carry");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanyTank_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 2, 15, true);
            else if (animationName == "Carry")
                AnimateStand(animationName, 2, 15, true);
        }
    }
}