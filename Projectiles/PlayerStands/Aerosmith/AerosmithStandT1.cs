using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
 
namespace JoJoStands.Projectiles.PlayerStands.Aerosmith
{  
    public class AerosmithStandT1 : StandClass
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
        public override int projectileDamage => 10;
        public override int shootTime => 12;      //+2 every tier

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
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            modPlayer.aerosmithWhoAmI = projectile.whoAmI;

            if (projectile.position.Y < (Main.worldSurface * 0.35) * 16f)
            {
                fallingFromSpace = true;
            }
            else
            {
                fallingFromSpace = false;
            }
            if (fallingFromSpace)
            {
                projectile.frameCounter = 0;
                projectile.velocity.Y += 0.3f;
                projectile.netUpdate = true;
            }
            Vector2 rota = projectile.Center - Main.MouseWorld;
            projectile.rotation = (-rota * projectile.direction).ToRotation();
            projectile.tileCollide = true;

            if (!modPlayer.StandAutoMode)
            {
                projectile.tileCollide = true;
                modPlayer.controllingAerosmith = true;
                float ScreenX = (float)Main.screenWidth / 2f;
                float ScreenY = (float)Main.screenHeight / 2f;
                modPlayer.aerosmithCamPosition = projectile.position + new Vector2(ScreenX, ScreenY);
                modPlayer.aerosmithCamPosition = new Vector2(projectile.position.X - ScreenX, projectile.position.Y - ScreenY);

                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !fallingFromSpace)
                {
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = Main.MouseWorld - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 10f;
                        if (Main.MouseWorld.X > projectile.position.X + 5)
                        {
                            projectile.direction = 1;
                        }
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
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
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
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, ProjectileID.Bullet, newProjectileDamage, 3f, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                projectile.rotation = (projectile.velocity * projectile.direction).ToRotation();
                NPC target = null;
                Vector2 targetPos = projectile.position;
                float targetDist = 350f;
                if (target == null)
                {
                    if (projectile.Distance(player.Center) < 80f)
                    {
                        if (projectile.ai[0] == 0f)
                        {
                            projectile.velocity.X = -2f;
                            projectile.spriteDirection = projectile.direction = -1;
                        }
                        if (projectile.ai[0] == 1f)
                        {
                            projectile.velocity.X = 2f;
                            projectile.spriteDirection = projectile.direction = 1;
                        }
                        if (projectile.position.X >= player.position.X + 50f || WorldGen.SolidTile((int)(projectile.position.X / 16) - 3, (int)(projectile.position.Y / 16f) + 1))
                        {
                            projectile.ai[0] = 0f;
                        }
                        if (projectile.position.X < player.position.X - 50f || WorldGen.SolidTile((int)(projectile.position.X / 16) + 3, (int)(projectile.position.Y / 16f) + 1))
                        {
                            projectile.ai[0] = 1f;
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
                        }
                    }
                    else
                    {
                        projectile.tileCollide = false;
                        projectile.velocity = player.Center - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 8f;
                    }
                    for (int k = 0; k < 200; k++)       //the targeting system
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                if (npc.boss)       //is gonna try to detect bosses over anything
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                                else        //if it fails to detect a boss, it'll detect the next best thing
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                            }
                        }
                    }
                }
                if (target != null)
                {
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (projectile.Distance(target.Center) > 45f)
                    {
                        projectile.velocity = targetPos - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 8f;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Main.PlaySound(SoundID.Item11, projectile.position);
                            Vector2 shootVel = targetPos - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, ProjectileID.Bullet, newProjectileDamage, 3f, projectile.owner);
                            Main.projectile[proj].netUpdate = true;
                        }
                    }
                }
            }
            projectile.netUpdate = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void SelectFrame()
        {
            projectile.frameCounter++;
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