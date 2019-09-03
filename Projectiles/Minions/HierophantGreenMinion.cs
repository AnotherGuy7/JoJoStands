using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class HierophantGreenMinion : ModProjectile
    {
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
            shoot = mod.ProjectileType("Emrald");
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        protected float shootCool = 24f;       //how fast the minion can shoot
        protected float shootSpeed = 16f;     //how fast the projectile the minion shoots goes
        protected int shoot;
        int shootcount = 0;
        static bool normalFrames = false;
        static bool attackFrames = false;
        int centerTimer = 0;
        static bool front = false;

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
                Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 35, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
                Vector2 vector131 = player.Center;
                if (!front)
                {
                    vector131.X -= (float)((15 + player.width / 2) * player.direction);
                }
                if (front)
                {
                    vector131.X -= (float)((15 + player.width / 2) * (player.direction * -1));
                }
                vector131.Y -= 15f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = (projectile.spriteDirection = player.direction);

                Vector2 targetPos = projectile.position;
                float targetDist = 350f;
                bool target = false;
                projectile.tileCollide = true;
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    normalFrames = true;
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
                SelectFrame();
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
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        attackFrames = true;
                        front = true;
                        normalFrames = false;
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
                                float numberProjectiles = 3 + Main.rand.Next(1);
                                float rotation = MathHelper.ToRadians(15);
                                for (int i = 0; i < numberProjectiles; i++)
                                {
                                    Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, shoot, 52, 2f, player.whoAmI);
                                    Main.projectile[proj].netUpdate = true;
                                }
                                projectile.netUpdate = true;
                            }
                        }
                    }
                    else
                    {
                        front = false;
                        attackFrames = false;
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
                    if (shootcount <= 0)
                    {
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        shootcount += 24;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        float numberProjectiles = 3 + Main.rand.Next(1);
                        float rotation = MathHelper.ToRadians(15);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, shoot, 52, 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
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
                    if (projectile.position.Y == (Main.MouseWorld.X / 16f))
                    {
                        projectile.velocity.Y = 0f;
                    }
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
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
                if (Main.mouseRight)
                {
                    SelectFrame();
                    attackFrames = true;
                    normalFrames = true;
                    if (shootcount <= 0)
                    {
                        Main.PlaySound(SoundID.Item21, projectile.position);
                        shootcount += 24;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        float numberProjectiles = 3 + Main.rand.Next(1);
                        float rotation = MathHelper.ToRadians(15);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, shoot, 52, 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                        }
                    }
                }
                else
                {
                    attackFrames = false;
                    normalFrames = true;
                }
                projectile.netUpdate = true;
            }
            if (player.GetModPlayer<MyPlayer>().StandControlActive && (MyPlayer.StandControlBinds || MyPlayer.StandControlMouse))       //when using stand control, to limit how far the stand can go
            {
                shootcount--;
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
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)      //4 is the pose frame
            {
                normalFrames = false;
                attackFrames = false;
                if (projectile.frameCounter >= 20)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 9)
                {
                    projectile.frame = 6;
                }
                if (projectile.frame <= 5)
                {
                    projectile.frame = 6;
                }
            }
        }
    }
}