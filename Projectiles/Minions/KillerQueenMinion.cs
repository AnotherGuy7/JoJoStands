using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class KillerQueenMinion : ModProjectile
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
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.melee = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        protected float shootSpeed = 16f;       //how fast the projectile the minion shoots goes
        static bool normalFrames = false;
        static bool attackFrames = false;
        static bool clickFrames = false;
        int bombcounter = 0;
        int attacknumber = 0;
        int shootCount = 0;
        int centerTimer = 0;
        int explosionTimer = 0;
        int secondClickTimer = 80;      //how long it'll wait for you to attack again
        int numberOfClicks = 0;         //how many times you've clicked
        bool clicked = false;           //if you've clicked once
        public static bool touchedTarget = false;

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
                Vector2 targetPos = projectile.position;        //does change to npc.Center
                float distanceAway = 2f;
                Vector2 vector131 = player.Center;
                vector131.X -= (float)((12 + player.width / 2) * player.direction);
                vector131.Y -= 25f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = (projectile.spriteDirection = player.direction);
                bool target = false;
                float targetDist = 800f;        //this is so high so that the right-click doesn't mess up

                for (int k = 0; k < 200; k++)
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
                            distanceAway = Vector2.Distance(npc.Center, player.Center);
                        }
                        else
                        {
                            touchedTarget = false;
                        }
                    }
                }
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f;
                    if (Main.rand.NextBool(3))
                    {
                        projectile.ai[1] += 1f;
                    }
                }
                if (projectile.ai[1] > 8f)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
                SelectFrame();
                normalFrames = true;
                attackFrames = false;
                if (target && distanceAway > 102f && touchedTarget)       //if the target leaves and the bomb won't damage you, detonate the enemy
                {
                    clickFrames = true;
                    attackFrames = false;
                    normalFrames = false;
                    explosionTimer++;
                    if (explosionTimer == 5)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                    }
                    if (explosionTimer >= 90)
                    {
                        int bomb = Projectile.NewProjectile(targetPos, Vector2.Zero, ProjectileID.GrenadeIII, 205, 3f, Main.myPlayer, 0f, 0f);
                        Main.projectile[bomb].timeLeft = 2;
                        Main.projectile[bomb].netUpdate = true;
                        explosionTimer = 0;
                        touchedTarget = false;
                    }
                }
                if (target && distanceAway < 98f)     //if the target is still in close range
                {
                    attackFrames = true;
                    normalFrames = false;
                    clickFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
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
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("KQMinionFists"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
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
            if (player.GetModPlayer<MyPlayer>().StandControlActive)        //a little different from the other stand's stand controls
            {
                if (MyPlayer.StandControlBinds && !MyPlayer.StandControlMouse)
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
                    if (JoJoStands.StandControlAttack.JustPressed)
                    {
                        numberOfClicks += 1;
                    }
                    if (JoJoStands.StandControlAttack.Current && (numberOfClicks == 0 || numberOfClicks == 1))
                    {
                        attackFrames = true;
                        normalFrames = false;
                        if (shootCount <= 0)
                        {
                            shootCount += 8;
                            Vector2 targetPos = projectile.position;
                            float shootVel = projectile.direction;
                            if (projectile.direction == 1)
                            {
                                shootVel *= 10f;
                            }
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel, 0f, mod.ProjectileType("KQMinionFists"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                    if (JoJoStands.StandControlAttack.Current && numberOfClicks >= 2 && Collision.SolidCollision(Main.MouseWorld, 1, 1))        //alt use stuff
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += 60;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                            int bomb = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, ProjectileID.GrenadeIII, 205, 3f, Main.myPlayer, 0f, 0f);
                            Main.projectile[bomb].timeLeft = 2;
                            Main.projectile[bomb].netUpdate = true;
                        }
                    }
                    if (!JoJoStands.StandControlUp.Current && !JoJoStands.StandControlDown.Current && !JoJoStands.StandControlLeft.Current && !JoJoStands.StandControlRight.Current)
                    {
                        projectile.velocity.X *= 0.5f;
                        projectile.velocity.Y *= 0.5f;
                    }
                    projectile.netUpdate = true;
                }
                if (MyPlayer.StandControlMouse && !MyPlayer.StandControlBinds)
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
                        if (!clicked)
                        {
                            numberOfClicks += 1;
                            clicked = true;
                        }
                    }
                    else
                    {
                        clicked = false;
                    }
                    if (Main.mouseRight && (numberOfClicks == 0 || numberOfClicks == 1))
                    {
                        attackFrames = true;
                        normalFrames = false;
                        secondClickTimer -= 3;      //make it go down faster
                        if (shootCount <= 0)
                        {
                            shootCount += 8;
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
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("KQMinionFists"), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                    if (Main.mouseRight && numberOfClicks >= 2)
                    {
                        if (shootCount <= 0 && Collision.SolidCollision(Main.MouseWorld, 1, 1))
                        {
                            shootCount += 60;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                            int bomb = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, ProjectileID.GrenadeIII, 205, 3f, Main.myPlayer, 0f, 0f);
                            Main.projectile[bomb].timeLeft = 2;
                            Main.projectile[bomb].netUpdate = true;
                        }
                    }
                }
                if (MyPlayer.StandControlBinds || MyPlayer.StandControlMouse)       //when using stand control, to limit how far the stand can go
                {
                    shootCount--;
                    if (numberOfClicks != 0)
                    {
                        secondClickTimer--;
                    }
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
            }
            if (shootCount <= 0)
            {
                shootCount = 0;
            }
            if (secondClickTimer <= 0)
            {
                secondClickTimer = 80;
                numberOfClicks = 0;
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
                clickFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 10)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 6)
                {
                    projectile.frame = 7;
                }
                if (projectile.frame >= 9)
                {
                    projectile.frame = 7;
                }
            }
            if (clickFrames) 
            {
                normalFrames = false;
                attackFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 40)      //18 to match it up with the explosion if you want
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 7)      //cause it should only click once
                {
                    projectile.frame = 2;
                    clickFrames = false;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                clickFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                clickFrames = false;
                projectile.frame = 9;
            }
        }
    }
}