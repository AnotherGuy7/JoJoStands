using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class StarPlatinumMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 74;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.melee = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        protected float shootSpeed = 16f;       //how fast the projectile the minion shoots goes
        static bool normalFrames = false;
        static bool attackFrames = false;
        int shootCount = 0;
        int centerTimer = 0;

        public override void AI()       //changed this to ExampleMod's HoverShooter...
        {
            Player player = Main.player[projectile.owner];      //defining who the owner of the projectile is
            if (player.active && player.GetModPlayer<MyPlayer>().MinionCurrentlyActive == false)
            {
                player.GetModPlayer<MyPlayer>().MinionCurrentlyActive = true;
                projectile.timeLeft = 5;
            }
            else
            {
                player.GetModPlayer<MyPlayer>().MinionCurrentlyActive = false;
            }
            if (!player.GetModPlayer<MyPlayer>().StandControlActive)        //check SHA's AI stuff cause it detects things withing a certain range limit
            {
                bool target = false;
                Vector2 targetPos = projectile.position;
                Vector2 direction = targetPos - projectile.position;
                normalFrames = true;
                if (!target || (!attackFrames && normalFrames))     //how it sticks behind the player
                {
                    Vector2 vector131 = player.Center;      //Stardust Guardian's little piece of movement
                    vector131.X -= (float)((5 + player.width / 2) * player.direction);
                    vector131.Y -= 25f;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.direction = (projectile.spriteDirection = player.direction);
                }
                normalFrames = true;
                float targetDist = 98f;
                for (int k = 0; k < 200; k++)       //the targeting system
                {
                    NPC npc = Main.npc[k];
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, projectile.Center);
                        if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = distance;
                            targetPos = npc.Center;
                            target = true;
                        }
                    }
                }
                SelectFrame();
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f;
                    if (Main.rand.NextBool(3))
                    {
                        projectile.ai[1] += 1f;
                    }
                }
                if (projectile.ai[1] > 4f)      //the shooting cooldown
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
                if (projectile.ai[0] == 0f)
                {
                    if (target && direction.Length() < 98f)
                    {
                        attackFrames = true;
                        normalFrames = false;
                        if ((targetPos - projectile.Center).X > 0f)     //the go to an enemy if it's within 5 blocks of you
                        {
                            projectile.spriteDirection = projectile.direction = 1;
                            projectile.velocity.X = 2f;
                        }
                        else if ((targetPos - projectile.Center).X < 0f)
                        {
                            projectile.spriteDirection = projectile.direction = -1;
                            projectile.velocity.X = -2f;
                        }
                        if (projectile.ai[1] == 0f)
                        {
                            projectile.ai[1] = 1f;
                            if (Main.myPlayer == projectile.owner)
                            {
                                Vector2 shootVel = targetPos - projectile.Center;
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                if (projectile.direction == 1)
                                {
                                    shootVel *= shootSpeed;
                                }
                                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), projectile.damage, 3f, Main.myPlayer, 0f, 0f);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        attackFrames = false;
                        normalFrames = true;
                    }
                }
            }
            if (player.GetModPlayer<MyPlayer>().StandControlActive && MyPlayer.StandControlBinds && !MyPlayer.StandControlMouse)
            {
                SelectFrame();
                normalFrames = true;
                attackFrames = false;
                if (JoJoStands.StandControlUp.Current)
                {
                    projectile.velocity.Y -= 0.3f;
                }
                if (JoJoStands.StandControlDown.Current)
                {
                    projectile.velocity.Y += 0.3f;
                }
                if (JoJoStands.StandControlLeft.Current)
                {
                    projectile.velocity.X -= 0.3f;
                    projectile.spriteDirection = -1;
                    projectile.direction = -1;
                }
                if (JoJoStands.StandControlRight.Current)
                {
                    projectile.velocity.X += 0.3f;
                    projectile.spriteDirection = 1;
                    projectile.direction = 1;
                }
                if (JoJoStands.StandControlAttack.Current)
                {
                    attackFrames = true;
                    normalFrames = false;
                    if (shootCount <= 0)
                    {
                        shootCount += 4;
                        Vector2 targetPos = projectile.position;
                        float shootVel = projectile.direction;
                        if (projectile.direction == 1)
                        {
                            shootVel *= 10f;
                        }
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel, 0f, mod.ProjectileType("Fists"), projectile.damage, 3f, Main.myPlayer, 0f, 0f);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                if (!JoJoStands.StandControlUp.Current && !JoJoStands.StandControlDown.Current && !JoJoStands.StandControlLeft.Current && !JoJoStands.StandControlRight.Current)
                {
                    projectile.velocity.X *= 0.5f;       //so that it stops floating away at 0.3 velocity...
                    projectile.velocity.Y *= 0.5f;
                }
                projectile.netUpdate = true;
            }
            if (player.GetModPlayer<MyPlayer>().StandControlActive && MyPlayer.StandControlMouse && !MyPlayer.StandControlBinds)
            {
                SelectFrame();
                normalFrames = true;
                attackFrames = false;
                if (Main.MouseWorld.X >= projectile.position.X && projectile.position.X != (Main.MouseWorld.X / 16f))
                {
                    projectile.spriteDirection = 1;
                    projectile.direction = 1;
                }
                if (Main.MouseWorld.X <= projectile.position.X && projectile.position.X != (Main.MouseWorld.X / 16f))
                {
                    projectile.spriteDirection = -1;
                    projectile.direction = -1;
                }
                if (Main.mouseLeft)
                {
                    if (projectile.position.X <= Main.MouseWorld.X)
                    {
                        projectile.velocity.X = 5f;
                    }
                    if (projectile.position.X >= Main.MouseWorld.X)
                    {
                        projectile.velocity.X = -5f;
                    }
                    if (projectile.position.Y >= Main.MouseWorld.Y)
                    {
                        projectile.velocity.Y = -5f;
                    }
                    if (projectile.position.Y <= Main.MouseWorld.Y)
                    {
                        projectile.velocity.Y = 5f;
                    }
                    if (projectile.position.X == (Main.MouseWorld.X / 16f))
                    {
                        projectile.velocity.X = 0f;
                    }
                    if (projectile.position.Y == (Main.MouseWorld.Y / 16f))
                    {
                        projectile.velocity.Y = 0f;
                    }
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
                if (Main.mouseRight)
                {
                    attackFrames = true;
                    normalFrames = false;
                    if (shootCount <= 0)
                    {
                        shootCount += 4;
                        Vector2 targetPos = Main.MouseWorld;
                        Vector2 shootVel = targetPos - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        if (projectile.direction == 1)
                        {
                            shootVel *= shootSpeed;
                        }
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), projectile.damage, 3f, Main.myPlayer, 0f, 0f);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
            }
            if (player.GetModPlayer<MyPlayer>().StandControlActive && (MyPlayer.StandControlBinds || MyPlayer.StandControlMouse))       //when using stand control, to limit how far the stand can go
            {
                shootCount--;
                Vector2 direction = player.Center - projectile.Center;
                float distanceTo = direction.Length();
                if (distanceTo > 98f)      //if the projectiles position are greater than distanceTo, make it stay on distanceTo
                {
                    if (projectile.position.X <= player.position.X)
                    {
                        projectile.position = new Vector2(projectile.position.X + 1, projectile.position.Y);
                        projectile.velocity = Vector2.Zero;
                    }
                    if (projectile.position.X >= player.position.X)
                    {
                        projectile.position = new Vector2(projectile.position.X - 1, projectile.position.Y);
                        projectile.velocity = Vector2.Zero;
                    }
                    if (projectile.position.Y >= player.position.Y)
                    {
                        projectile.position = new Vector2(projectile.position.X, projectile.position.Y - 1);
                        projectile.velocity = Vector2.Zero;
                    }
                    if (projectile.position.Y <= player.position.Y)
                    {
                        projectile.position = new Vector2(projectile.position.X, projectile.position.Y + 1);
                        projectile.velocity = Vector2.Zero;
                    }
                    centerTimer++;
                }
                else
                {
                    centerTimer = 0;
                }

            }
            if (shootCount <= 0)
            {
                shootCount = 0;
            }
            if (centerTimer >= 300)
            {
                player.GetModPlayer<MyPlayer>().StandControlActive = false;
                MyPlayer.StandControlBinds = false;
                MyPlayer.StandControlMouse = false;
                projectile.position = player.position;
                centerTimer = 0;
            }
        }
 
        public virtual void SelectFrame()
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= 4)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 3)
                {
                    projectile.frame = 4;
                }
                if (projectile.frame >= 8)
                {
                    projectile.frame = 4;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 12)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                if (projectile.frameCounter >= 12)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 10)
                {
                    projectile.frame = 8;
                }
                if (projectile.frame <= 7)
                {
                    projectile.frame = 8;
                }
            }
        }
    }
}