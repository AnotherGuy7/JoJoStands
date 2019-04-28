using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions.StarPlatinum
{  
    public class StarPlatinumMinion : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 32;
            Main.projFrames[projectile.type] = 23;
            projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            shoot = mod.ProjectileType("Fists");
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
            projectile.melee = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            if (player.active && MyPlayer.MinionCurrentlyActive == false)
            {
                MyPlayer.MinionCurrentlyActive = true;
                projectile.timeLeft = 5;
                return true;
            }
            else
            {
                MyPlayer.MinionCurrentlyActive = false;
            }
            return false;
        }

        protected float idleAccel = 0.05f;      //this was basically taken from some video tutorial, just edited my way
        protected float spacingMult = 1f;
        protected float viewDist = 15f;       //minion view Distance
        protected float chaseDist = 5f;       //how far the minion can go
        protected float chaseAccel = 6f;        //how fast the minion chases the target
        protected float inertia = 40f;
        protected float shootCool = 2f;       //how fast the minion can shoot
        protected float shootSpeed = 16f;       //how fast the projectile the minion shoots goes
        protected float chaseLimit = 150f;       //how far the minion can go until it return to the player while not attacking
        protected int shoot;
        static bool normalFrames = false;
        static bool attackFrames = false;
        bool frameReset = false;
        int oneTime = 0;
        int oneTime2 = 0;

        public override void AI()
        {

            Player player = Main.player[projectile.owner];      //defining who the owner of the projectile is
            Vector2 targetCentre = player.Center;       //making the projectiles center the players center
            targetCentre = player.Center + new Vector2(-16f * player.direction, -24 * player.gravDir);      //making the projectiles center to the left and above the player (?) probably, anyway
            projectile.direction = projectile.spriteDirection = player.direction;       //setting the minions direction the same as the players direction

            float spacing = (float)projectile.width * spacingMult;
            for (int k = 0; k < 1000; k++)      //around 1000 distance?
            {
                normalFrames = true;
                Projectile otherProj = Main.projectile[k];      //the enemy
                if (k != projectile.whoAmI && otherProj.active && otherProj.owner == projectile.owner && otherProj.type == projectile.type && System.Math.Abs(projectile.position.X - otherProj.position.X) + System.Math.Abs(projectile.position.Y - otherProj.position.Y) < spacing)
                {
                    if (projectile.position.X < Main.projectile[k].position.X)      //if the position (horizonal) is less than the position of the enemy, which by k is 1000
                    {
                        projectile.velocity.X -= idleAccel;     //to deccelarate by 0.05f
                    }
                    else
                    {
                        projectile.velocity.X += idleAccel;     //to accelarate by 0.05f
                    }
                    if (projectile.position.Y < Main.projectile[k].position.Y)      //if position (vertical) is less than 1000?
                    {
                        projectile.velocity.Y -= idleAccel;     //to deccelarate by 0.05f
                    }
                    else
                    {
                        projectile.velocity.Y += idleAccel;     //to accelarate by 0.05f
                    }
                }
            }
            Vector2 targetPos = projectile.position;        //a variable that wants somethings position to be the projectile position... or something like that?
            float targetDist = viewDist;
            bool target = false;        //determines if theres a target... probably?
            projectile.tileCollide = true;
            for (int k = 0; k < 200; k++)       //it's attack function?
            {
                NPC npc = Main.npc[k];      //now k represents npcs
                normalFrames = true;
                if (npc.CanBeChasedBy(this, false))     //if that NPC can be chased by minions
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
            if (Vector2.Distance(player.Center, projectile.Center) > (target ? 1000f : 500f))       //basically, setting target distance to 500<target<1000... probably. When it's supposed to go to the player
            {
                projectile.ai[0] = 5f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 5f)
            {
                projectile.tileCollide = false;
            }
            if (target && projectile.ai[0] == 0f)
            {
                Vector2 direction = targetPos - projectile.Center;
                attackFrames = true;
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
                    speed = 1f;
                }
                if (distanceTo < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                }
                if (distanceTo > chaseLimit)
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
                projectile.spriteDirection = (projectile.direction = -1);
            }
            else if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = (projectile.direction = 1);
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
                    attackFrames = true;
                    normalFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = (projectile.direction = -1);
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = (projectile.direction = 1);
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
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, shoot, projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    attackFrames = false;
                }
            }
        }

        public virtual void CreateDust()
        {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
        }
 
        public virtual void SelectFrame()       //a custom method made for finding the frames of the projectile, is gotten in Line 181
        {
            projectile.frameCounter++;
            if (frameReset)
            {
                projectile.frameCounter = 0;
            }
            if (attackFrames)       //after 8 is attack frames, 9 is the first attack frame
            {
                normalFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter <= 10)      //if frameCounter is less than or equal to 10
                {
                    projectile.frame++;
                    if (projectile.frame >= 8)
                    {
                        projectile.frame++;
                    }
                    if (projectile.frame <= 8)      //less than or equal to 8
                    {
                        projectile.frame = 9;
                    }
                    if (projectile.frame >= 23)     //if greater than or equal to 23, back to frame 9
                    {
                        projectile.frame = 9;
                    }
                }
                if (projectile.frameCounter >= 10)
                {
                    frameReset = true;
                }
            }
            if (normalFrames)       //first 8 frames are the idle frames
            {
                projectile.frameCounter++;
                if (projectile.frameCounter <= 10)
                {
                    projectile.frame++;
                    if (projectile.frame >= 8)
                    {
                        projectile.frame = 0;
                    }
                }
                if (projectile.frameCounter >= 10)
                {
                    frameReset = true;
                }
            }
        }
    }
}