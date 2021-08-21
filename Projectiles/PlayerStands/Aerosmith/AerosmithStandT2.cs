using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.Aerosmith
{
    public class AerosmithStandT2 : StandClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/Aerosmith/AerosmithStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 40;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 0;
            projectile.ignoreWater = true;
        }

        public override float shootSpeed => 12f;
        public override int projectileDamage => 42;
        public override int shootTime => 10;      //+2 every tier
        public override int standType => 2;
        public override string poseSoundName => "VolareVia";
        public override string spawnSoundName => "Aerosmith";

        private bool bombless = false;
        private bool fallingFromSpace = false;

        public override void AI()
        {
            SelectFrame();
            UpdateStandInfo();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
            {
                projectile.timeLeft = 2;
            }
            mPlayer.aerosmithWhoAmI = projectile.whoAmI;
            newProjectileDamage = (int)(newProjectileDamage * MathHelper.Clamp(1f - (projectile.Distance(player.Center) / (350f * 16f)), 0.5f, 1f));

            fallingFromSpace = projectile.position.Y < (Main.worldSurface * 0.35) * 16f;
            if (fallingFromSpace)
            {
                projectile.frameCounter = 0;
                projectile.velocity.Y += 0.3f;
                projectile.netUpdate = true;
            }
            Vector2 rota = projectile.Center - Main.MouseWorld;
            projectile.rotation = (-rota * projectile.direction).ToRotation();
            bombless = player.HasBuff(mod.BuffType("AbilityCooldown"));
            projectile.tileCollide = true;

            if (!mPlayer.standAutoMode)
            {
                projectile.tileCollide = true;
                mPlayer.standRemoteMode = true;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);

                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !fallingFromSpace)
                {
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = Main.MouseWorld - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 10f;

                        projectile.direction = 1;
                        if (Main.MouseWorld.X < projectile.position.X - 5)
                        {
                            projectile.direction = -1;
                        }
                        projectile.spriteDirection = projectile.direction;
                    }
                    else
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    projectile.netUpdate = true;
                }
                else
                {
                    if (!fallingFromSpace)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    projectile.rotation = 0f;
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Main.PlaySound(SoundID.Item11, projectile.position);
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, ProjectileID.Bullet, newProjectileDamage, 3f, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
                if (SpecialKeyPressedNoCooldown() && !bombless && projectile.owner == Main.myPlayer)
                {
                    shootCount += newShootTime;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(15));
                    int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("AerosmithBomb"), 0, 3f, projectile.owner, (projectileDamage + 21) * (float)mPlayer.standDamageBoosts);
                    Main.projectile[proj].netUpdate = true;
                }
            }
            if (mPlayer.standAutoMode)
            {
                projectile.rotation = (projectile.velocity * projectile.direction).ToRotation();
                NPC target = FindNearestTarget(350f);
                if (target == null)
                {
                    if (projectile.Distance(player.Center) < 80f)
                    {
                        if (projectile.position.X >= player.position.X + 50f || WorldGen.SolidTile((int)(projectile.position.X / 16) - 3, (int)(projectile.position.Y / 16f) + 1))
                        {
                            projectile.velocity.X = -2f;
                            projectile.spriteDirection = projectile.direction = -1;
                            projectile.netUpdate = true;
                        }
                        if (projectile.position.X < player.position.X - 50f || WorldGen.SolidTile((int)(projectile.position.X / 16) + 3, (int)(projectile.position.Y / 16f) + 1))
                        {
                            projectile.velocity.X = 2f;
                            projectile.spriteDirection = projectile.direction = 1;
                            projectile.netUpdate = true;
                        }
                        if (projectile.position.Y > player.position.Y + 2f)
                        {
                            projectile.velocity.Y = -2f;
                        }
                        if (projectile.position.Y < player.position.Y - 2f)
                        {
                            projectile.velocity.Y = 2f;
                        }
                        if (projectile.position.Y < player.position.Y + 2f && projectile.position.Y > player.position.Y - 2f)
                        {
                            projectile.velocity.Y = 0f;
                            projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        projectile.tileCollide = false;
                        projectile.velocity = player.Center - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 8f;
                    }
                }
                if (target != null)
                {
                    if (projectile.Distance(target.Center) > 45f)
                    {
                        projectile.velocity = target.position - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 8f;

                        projectile.direction = 1;
                        if (projectile.velocity.X < 0f)
                            projectile.direction = -1;
                        projectile.spriteDirection = projectile.direction;
                        projectile.netUpdate = true;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Main.PlaySound(SoundID.Item11, projectile.position);
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, ProjectileID.Bullet, newProjectileDamage, 3f, projectile.owner);
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
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bombless = reader.ReadBoolean();
        }

        public void SelectFrame()
        {
            projectile.frameCounter++;
            if (bombless)
            {
                if (projectile.frameCounter >= 8)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 2;
                }
            }
            else
            {
                if (projectile.frameCounter >= 8)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}