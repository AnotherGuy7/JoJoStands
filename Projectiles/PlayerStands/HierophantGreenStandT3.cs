using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.PlayerStands
{  
    public class HierophantGreenStandT3 : StandClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/HierophantGreenStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 9;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 0;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override int shootTime => 20;
        public override int projectileDamage => 56;
        public override int halfStandHeight => 20;

        public bool spawningField = false;
        public float numberSpawned = 0;
        public bool linkShot = false;
        public bool linkShotForSpecial = false;
        public Vector2 formPosition = Vector2.Zero;

        public override void AI()
        {
            SelectFrame();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            Vector2 vector131 = player.Center;
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 35, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            projectile.scale = ((50 - player.ownedProjectileCounts[mod.ProjectileType("EmeraldStringPoint2")]) * 2f) / 100f;
            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
                    Main.mouseRight = false;        //so that the player can't just stop time while punching
                    projectile.netUpdate = true;
                    if (shootCount <= 0)
                    {
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        shootCount += shootTime - modPlayer.standSpeedBoosts;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        float numberProjectiles = 5;
                        float rotation = MathHelper.ToRadians(25);
                        float random = Main.rand.NextFloat(-6f, 6f);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + random, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Emerald"), (int)(projectileDamage * modPlayer.standDamageBoosts), 3f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && shootCount <= 0 && !linkShot && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    linkShot = true;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                    shootCount += 15;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    Vector2 perturbedSpeed = new Vector2(shootVel.X + Main.rand.NextFloat(-3f, 3f), shootVel.Y);
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("EmeraldStringPoint"), 0, 3f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                }
                if (Main.mouseRight && shootCount <= 0 && linkShot && projectile.scale >= 0.5f && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    linkShot = false;
                    Main.PlaySound(SoundID.Item21, projectile.position);
                    shootCount += 15;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    Vector2 perturbedSpeed = new Vector2(shootVel.X + Main.rand.NextFloat(-3f, 3f), shootVel.Y);
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("EmeraldStringPoint2"), 0, 3f, player.whoAmI, 24f);
                    Main.projectile[proj].netUpdate = true;
                }
                if (SpecialKeyPressed() && player.ownedProjectileCounts[mod.ProjectileType("EmeraldStringPoint2")] <= 0 && !spawningField)
                {
                    spawningField = true;
                    formPosition = projectile.position;
                }
                if (spawningField)
                {
                    float randomRadius = Main.rand.NextFloat(-20f, 21f);
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                    projectile.frame = 8;
                    Vector2 offset = formPosition + (randomRadius.ToRotationVector2() * 288f);     //33 tiles

                    if (numberSpawned < 70f && shootCount <= 0 && !linkShotForSpecial)        //35 tendrils, the number spawned limit /2 is the wanted amount
                    {
                        shootCount += 2;
                        int proj = Projectile.NewProjectile(offset, Vector2.Zero, mod.ProjectileType("EmeraldStringPoint"), 0, 2f, player.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        Main.projectile[proj].tileCollide = false;
                        linkShotForSpecial = true;
                        numberSpawned++;
                    }
                    if (numberSpawned < 70f && shootCount <= 0 && linkShotForSpecial)
                    {
                        shootCount += 2;
                        int proj = Projectile.NewProjectile(offset, Vector2.Zero, mod.ProjectileType("EmeraldStringPoint2"), 0, 2f, player.whoAmI, 24f);
                        Main.projectile[proj].netUpdate = true;
                        Main.projectile[proj].tileCollide = false;
                        linkShotForSpecial = false;
                        numberSpawned++;
                    }
                    if (numberSpawned >= 70f)
                    {
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(30));
                        spawningField = false;
                        formPosition = Vector2.Zero;
                    }
                }
                else
                {
                    numberSpawned = 0;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                float targetDist = 350f;
                if (target == null)
                {
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
                    attackFrames = true;
                    normalFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (shootCount <= 0)
                    {
                        shootCount += shootTime - modPlayer.standSpeedBoosts;
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        if (Main.myPlayer == projectile.owner)
                        {
                            Vector2 shootVel = targetPos - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            float numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(25);
                            float random = Main.rand.NextFloat(-6f, 6f);
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = new Vector2(shootVel.X + random, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Emerald"), (int)((projectileDamage * modPlayer.standDamageBoosts) * 0.9f), 3f, player.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                            }
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    normalFrames = true;
                    attackFrames = false;
                }
            }
        }

        public void SelectFrame()
        {
            projectile.frameCounter++;
            if (normalFrames)
            {
                if (projectile.frameCounter >= 20)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 3)
                {
                    projectile.frame = 0;
                }
            }
            if (attackFrames)
            {
                if (projectile.frameCounter >= 15)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 3;
                }
                if (projectile.frame <= 2)
                {
                    projectile.frame = 3;
                }
            }
        }
    }
}