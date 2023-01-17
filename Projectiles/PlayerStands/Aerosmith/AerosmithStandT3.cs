using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Aerosmith
{
    public class AerosmithStandT3 : StandClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/Aerosmith/Aerosmith"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 40;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.netImportant = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 0;
            Projectile.ignoreWater = true;
        }

        public override float ProjectileSpeed => 12f;

        public override int ProjectileDamage => 63;
        public override int ShootTime => 10;      //+2 every tier
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override string PoseSoundName => "VolareVia";
        public override string SpawnSoundName => "Aerosmith";

        private bool bombless = false;
        private bool fallingFromSpace = false;
        private bool remoteMode = false;
        private int frameMultUpdate = 0;
        private int leftMouse = 0;
        private int rightMouse = 0;
        private int accelerationTimer = 0;
        private const int AccelerationTime = (int)(1.5f * 60);
        private const float MaxFlightSpeed = 9f;

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                frameMultUpdate = 2;
        }
        public override void AI()
        {
            SelectFrame();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (accelerationTimer > 0)
                accelerationTimer--;

            mPlayer.aerosmithWhoAmI = Projectile.whoAmI;
            mPlayer.standRemoteMode = remoteMode;
            newProjectileDamage = (int)(newProjectileDamage * MathHelper.Clamp(1f - (Projectile.Distance(player.Center) / (350f * 16f)), 0.5f, 1f));

            fallingFromSpace = Projectile.position.Y < (Main.worldSurface * 0.35) * 16f;
            if (fallingFromSpace)
            {
                Projectile.frameCounter = 0;
                Projectile.velocity.Y += 0.3f;
                Projectile.netUpdate = true;
            }
            Vector2 rota = Projectile.Center - new Vector2(mouseX, mouseY);
            Projectile.rotation = (-rota * Projectile.direction).ToRotation();
            bombless = player.HasBuff(ModContent.BuffType<AbilityCooldown>());
            Projectile.tileCollide = true;

            if (Projectile.velocity.X > 0.5f)
                Projectile.spriteDirection = 1;
            if (Projectile.velocity.X < -0.5f)
                Projectile.spriteDirection = -1;

            if (leftMouse > 0)
                leftMouse--;
            if (rightMouse > 0)
                rightMouse--;

            if (remoteMode)
            {
                player.aggro -= 1200;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
            }
            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    leftMouse = 10;

                    float mouseDistance = Vector2.Distance(new Vector2(mouseX, mouseY), Projectile.Center);

                    Projectile.spriteDirection = Projectile.direction;
                    accelerationTimer += 2;
                    if (accelerationTimer >= AccelerationTime)
                        accelerationTimer = AccelerationTime;

                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = new Vector2(mouseX, mouseY) - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= MaxFlightSpeed + player.moveSpeed;
                        Projectile.velocity *= accelerationTimer / (float)AccelerationTime;
                    }
                    else
                    {
                        Projectile.velocity = new Vector2(mouseX, mouseY) - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= (mouseDistance * (MaxFlightSpeed + player.moveSpeed)) / 40f;
                        Projectile.velocity *= accelerationTimer / (float)AccelerationTime;
                    }
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
                    if (Projectile.Distance(player.Center) > 80f)
                    {
                        Projectile.velocity *= 0.95f;
                        Projectile.netUpdate = true;
                    }
                }
                if (remoteMode && Projectile.velocity.X >= 8f)
                {
                    int amountOfCloudDusts = Main.rand.Next(1, 3 + 1);
                    for (int i = 0; i < amountOfCloudDusts; i++)
                    {
                        float speedScale = Main.rand.Next(-24, -16 + 1) / 10f;
                        speedScale *= 4f;
                        float dustScale = Main.rand.Next(8, 12 + 1) / 10f;
                        int dustIndex = Dust.NewDust(Projectile.Center - (Main.ScreenSize.ToVector2() / 2f) + new Vector2(12 * 16f * Projectile.direction, 0f), Main.screenWidth, Main.screenHeight, DustID.Cloud, Projectile.velocity.X * speedScale, Scale: dustScale);
                        Main.dust[dustIndex].noGravity = true;

                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    rightMouse = 10;

                    if (mouseX > Projectile.Center.X)
                        Projectile.spriteDirection = 1;
                    if (mouseX < Projectile.Center.X)
                        Projectile.spriteDirection = -1;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = new Vector2(mouseX, mouseY) - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 32f;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                    }
                }
                if (!remoteMode && leftMouse == 0 && rightMouse == 0)
                {
                    if (Projectile.Distance(player.Center) < 60f)
                    {
                        if (Projectile.position.X >= player.position.X + 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) - 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = -2f;
                            Projectile.spriteDirection = Projectile.direction = -1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.X < player.position.X - 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) + 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = 2f;
                            Projectile.spriteDirection = Projectile.direction = 1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.Y > player.position.Y + 2f)
                        {
                            Projectile.velocity.Y = -2f;
                        }
                        if (Projectile.position.Y < player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 2f;
                        }
                        if (Projectile.position.Y < player.position.Y + 2f && Projectile.position.Y > player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 0f;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (Projectile.Distance(player.Center) > 80f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 9f + player.moveSpeed;
                        Projectile.netUpdate = true;
                    }
                    if (Projectile.Distance(player.Center) >= 60f && Projectile.Distance(player.Center) <= 80f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= ((Projectile.Distance(player.Center) - 55f) * (9f + player.moveSpeed)) / 20;
                        Projectile.netUpdate = true;
                    }
                }
                if (SpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer)
                {
                    remoteMode = !remoteMode;
                    if (remoteMode)
                        Main.NewText("Remote Mode: Active");
                    else
                        Main.NewText("Remote Mode: Disabled");
                }
                if (SecondSpecialKeyPressedNoCooldown() && !bombless)
                {
                    shootCount += newShootTime;
                    Projectile.frame = 2;
                    frameMultUpdate = 2;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AerosmithBomb>(), 0, 3f, Projectile.owner, 568 * (float)mPlayer.standDamageBoosts);
                    Main.projectile[proj].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                }
            }
            if (mPlayer.standAutoMode)
            {
                remoteMode = false;
                Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
                NPC target = FindNearestTarget(350f);
                if (target == null)
                {
                    if (Projectile.Distance(player.Center) < 60f)
                    {
                        if (Projectile.position.X >= player.position.X + 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) - 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = -2f;
                            Projectile.spriteDirection = Projectile.direction = -1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.X < player.position.X - 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) + 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = 2f;
                            Projectile.spriteDirection = Projectile.direction = 1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.Y > player.position.Y + 2f)
                        {
                            Projectile.velocity.Y = -2f;
                        }
                        if (Projectile.position.Y < player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 2f;
                        }
                        if (Projectile.position.Y < player.position.Y + 2f && Projectile.position.Y > player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 0f;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (Projectile.Distance(player.Center) > 80f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 9f + player.moveSpeed;
                        Projectile.netUpdate = true;
                    }
                    if (Projectile.Distance(player.Center) >= 60f && Projectile.Distance(player.Center) <= 80f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= ((Projectile.Distance(player.Center) - 55f) * (9f + player.moveSpeed)) / 20;
                        Projectile.netUpdate = true;
                    }
                }
                if (target != null)
                {
                    if (Projectile.Distance(target.Center) > 45f)
                    {
                        Projectile.velocity = target.position - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 8f;

                        Projectile.direction = 1;
                        if (Projectile.velocity.X < 0f)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;
                        Projectile.netUpdate = true;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                            Main.projectile[proj].netUpdate = true;
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(bombless);
            writer.Write(remoteMode);
            writer.Write(frameMultUpdate);
            writer.Write(leftMouse);
            writer.Write(rightMouse);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bombless = reader.ReadBoolean();
            remoteMode = reader.ReadBoolean();
            frameMultUpdate = reader.ReadInt32();
            leftMouse = reader.ReadInt32();
            rightMouse = reader.ReadInt32();
        }

        public void SelectFrame()
        {
            Projectile.frame = frameMultUpdate;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                frameMultUpdate += 1;
                Projectile.frameCounter = 0;
                if (!bombless)
                {
                    if (frameMultUpdate > 1)
                        frameMultUpdate = 0;
                }
                if (bombless)
                {
                    if (frameMultUpdate > 3)
                        frameMultUpdate = 2;
                }
            }
        }
    }
}