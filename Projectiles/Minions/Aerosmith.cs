using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class Aerosmith : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 45;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 0;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            inertia = 3f;
            shoot = 14;
            shootSpeed = 22f;
            projectile.melee = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        protected float idleAccel = 0.05f;
        protected float spacingMult = 1f;
        protected float viewDist = 300f;       //minion view Distance
        protected float chaseDist = 150f;       //how far the minion can go
        protected float chaseAccel = 6f;        //how fast the minion chases the target
        protected float inertia = 40f;
        protected float shootCool = 6f;       //how fast the minion can shoot
        protected float shootSpeed;     //how fast the projectile the minion shoots goes
        protected int shoot;
        int shootcount = 0;
        int centerTimer = 0;
        int directionCounter = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.active && player.GetModPlayer<MyPlayer>().MinionCurrentlyActive == false)
            {
                player.GetModPlayer<MyPlayer>().MinionCurrentlyActive = true;
                projectile.timeLeft = 5;
            }
            else
            {
                player.GetModPlayer<MyPlayer>().MinionCurrentlyActive = false;
            }
            if (!player.GetModPlayer<MyPlayer>().StandControlActive)
            {
                float spacing = (float)projectile.width * spacingMult;
                for (int k = 0; k < 1000; k++)
                {
                    Projectile otherProj = Main.projectile[k];
                    if (k != projectile.whoAmI && otherProj.active && otherProj.owner == projectile.owner && otherProj.type == projectile.type && System.Math.Abs(projectile.position.X - otherProj.position.X) + System.Math.Abs(projectile.position.Y - otherProj.position.Y) < spacing)
                    {
                        if (projectile.position.X < Main.projectile[k].position.X)
                        {
                            projectile.velocity.X -= idleAccel;
                        }
                        else
                        {
                            projectile.velocity.X += idleAccel;
                        }
                        if (projectile.position.Y < Main.projectile[k].position.Y)
                        {
                            projectile.velocity.Y -= idleAccel;
                        }
                        else
                        {
                            projectile.velocity.Y += idleAccel;
                        }
                    }
                }
                Vector2 targetPos = projectile.position;
                float targetDist = viewDist;
                bool target = false;
                projectile.tileCollide = true;
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, projectile.Center);
                        if ((distance < targetDist || !target) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = distance;
                            targetPos = npc.Center;
                            target = true;
                        }
                    }
                }
                if (Vector2.Distance(player.Center, projectile.Center) > (target ? 1000f : 500f))
                {
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
                if (projectile.ai[0] == 1f)
                {
                    projectile.tileCollide = false;
                }
                if (target && projectile.ai[0] == 0f)
                {
                    Vector2 direction = targetPos - projectile.Center;
                    if (direction.Length() > chaseDist)
                    {
                        direction.Normalize();
                        projectile.velocity = (projectile.velocity * inertia + direction * chaseAccel) / (inertia + 1);
                    }
                    else
                    {
                        projectile.velocity *= (float)Math.Pow(0.97, 40.0 / inertia);
                    }
                }
                else
                {
                    if (!Collision.CanHitLine(projectile.Center, 1, 1, player.Center, 1, 1))
                    {
                        projectile.ai[0] = 1f;
                    }
                    float speed = 6f;
                    if (projectile.ai[0] == 1f)
                    {
                        speed = 15f;
                    }
                    Vector2 center = projectile.Center;
                    Vector2 direction = player.Center - center;
                    projectile.ai[1] = 3600f;
                    projectile.netUpdate = true;
                    int num = 1;
                    for (int k = 0; k < projectile.whoAmI; k++)
                    {
                        if (Main.projectile[k].active && Main.projectile[k].owner == projectile.owner && Main.projectile[k].type == projectile.type)
                        {
                            num++;
                        }
                    }
                    float distanceTo = direction.Length();
                    if (distanceTo > 200f && speed < 9f)
                    {
                        speed = 15f;
                    }
                    if (distanceTo < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                    {
                        projectile.ai[0] = 0f;
                        projectile.netUpdate = true;
                    }
                    if (distanceTo > 2000f)
                    {
                        projectile.Center = player.Center;
                    }
                    if (distanceTo > 48f)
                    {
                        direction.Normalize();
                        direction *= speed;
                        float temp = inertia / 2f;
                        projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
                    }
                    else
                    {
                        projectile.direction = Main.player[projectile.owner].direction;
                        projectile.velocity *= (float)Math.Pow(0.9, 40.0 / inertia);
                    }
                }
                projectile.rotation = projectile.velocity.X * 0.05f;
                SelectFrame();
                CreateDust();
                if (projectile.velocity.X > 0f)
                {
                    projectile.spriteDirection = (projectile.direction = 1);
                }
                else if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = (projectile.direction = -1);
                }
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f;
                    if (Main.rand.Next(3) == 0)
                    {
                        projectile.ai[1] += 1f;
                    }
                }
                if (projectile.ai[1] > shootCool)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
                if (projectile.ai[0] == 0f)
                {
                    if (target)
                    {
                        if ((targetPos - projectile.Center).X > 0f)
                        {
                            projectile.spriteDirection = (projectile.direction = 1);
                        }
                        else if ((targetPos - projectile.Center).X < 0f)
                        {
                            projectile.spriteDirection = (projectile.direction = -1);
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
                                shootVel *= shootSpeed;
                                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, shoot, 71, 1f, Main.myPlayer, 0f, 0f);
                                Main.projectile[proj].timeLeft = 300;
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                            }
                        }
                    }
                }
            }
            if (player.GetModPlayer<MyPlayer>().StandControlActive && MyPlayer.StandControlBinds && !MyPlayer.StandControlMouse)
            {
                shootcount--;
                SelectFrame();
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
                    if (shootcount <= 0)
                    {
                        shootcount += 6;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, ProjectileID.Bullet, 71, 1f, Main.myPlayer, 0f, 0f);
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
                shootcount--;
                SelectFrame();
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
                if (Main.MouseWorld.X >= projectile.position.X)
                {
                    projectile.spriteDirection = 1;
                    projectile.direction = 1;
                }
                if (Main.MouseWorld.X <= projectile.position.X)
                {
                    projectile.spriteDirection = -1;
                    projectile.direction = -1;
                }
                if (Main.mouseRight)
                {
                    SelectFrame();
                    if (shootcount <= 0)
                    {
                        shootcount += 6;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, ProjectileID.Bullet, 71, 1f, Main.myPlayer, 0f, 0f); Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                projectile.netUpdate = true;
            }
            if (player.GetModPlayer<MyPlayer>().StandControlActive && (MyPlayer.StandControlBinds || MyPlayer.StandControlMouse))       //when using stand control, to limit how far the stand can go
            {
                Vector2 direction = player.Center - projectile.Center;
                float distanceTo = direction.Length();
                if (distanceTo > 250f)      //if the projectiles position are greater than distanceTo, make it stay on distanceTo
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
            if (shootcount <= 0)
            {
                shootcount = 0;
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

        public void CreateDust()
        {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
        }
 
        public void SelectFrame()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 3;
            }
        }
    }
}