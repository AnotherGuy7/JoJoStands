using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class KillerQueenStandFinal : ModProjectile      //has 2 poses
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/KillerQueenStand"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public Vector2 velocityAddition = Vector2.Zero;
        public float mouseDistance = 0f;
        protected float shootSpeed = 16f;
        public bool normalFrames = false;
        public bool attackFrames = false;
        public bool clickFrames = false;
        public float maxDistance = 0f;
        public int punchDamage = 74;
        public int altDamage = 86;
        public int shootCount = 0;
        public int punchTime = 11;
        public int explosionTimer = 0;
        public int halfStandHeight = 37;

        public float npcDistance = 0f;
        public float mouseToPlayerDistance = 0f;
        public Vector2 savedPosition = Vector2.Zero;
        public bool touchedTile = false;
        public int timeAfterTouch = 0;
        public float maxAltDistance = 0f;     //about 10 tiles

        public static NPC savedTarget = null;
        public int npcExplosionTimer = 0;
        public float fistWhoAmI = 5f;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            if (timeAfterTouch > 0)
            {
                timeAfterTouch--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = -10;
            }
            if (projectile.spriteDirection == -1)
            {
                drawOffsetX = -60;
            }
            drawOriginOffsetY = -halfStandHeight;
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
                    clickFrames = false;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
                    if (Main.MouseWorld.X > projectile.position.X)
                    {
                        projectile.spriteDirection = 1;
                        projectile.direction = 1;
                    }
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.spriteDirection = -1;
                        projectile.direction = -1;
                    }
                    if (projectile.position.X < Main.MouseWorld.X - 5f)
                    {
                        velocityAddition.X = 5f;
                    }
                    if (projectile.position.X > Main.MouseWorld.X + 5f)
                    {
                        velocityAddition.X = -5f;
                    }
                    if (projectile.position.X > Main.MouseWorld.X - 5f && projectile.position.X < Main.MouseWorld.X + 5f)
                    {
                        velocityAddition.X = 0f;
                    }
                    if (projectile.position.Y > Main.MouseWorld.Y + 5f)
                    {
                        velocityAddition.Y = -5f;
                    }
                    if (projectile.position.Y < Main.MouseWorld.Y - 5f)
                    {
                        velocityAddition.Y = 5f;
                    }
                    if (projectile.position.Y < Main.MouseWorld.Y + 5f && projectile.position.Y > Main.MouseWorld.Y - 5f)
                    {
                        velocityAddition.Y = 0f;
                    }
                    mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    if (shootCount <= 0)
                    {
                        shootCount += punchTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(punchDamage * modPlayer.standDamageBoosts), 5f, Main.myPlayer, fistWhoAmI, 4f);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    Vector2 vector131 = player.Center;
                    vector131.X -= (float)((12 + player.width / 2) * player.direction);
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.direction = (projectile.spriteDirection = player.direction);
                    projectile.rotation = 0;
                    normalFrames = true;
                    attackFrames = false;
                }
                if (Main.mouseRight && shootCount <= 0 && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    attackFrames = false;
                    normalFrames = false;
                    if (Collision.SolidCollision(Main.MouseWorld, 1, 1) && mouseToPlayerDistance < maxAltDistance && timeAfterTouch <= 0 && !touchedTile)
                    {
                        timeAfterTouch = 60;
                        savedPosition = Main.MouseWorld;
                        touchedTile = true;
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                    }
                    if (timeAfterTouch <= 0 && touchedTile)
                    {
                        clickFrames = true;
                        int projectile = Projectile.NewProjectile(savedPosition, Vector2.Zero, ProjectileID.GrenadeIII, (int)(altDamage * modPlayer.standDamageBoosts), 50f, Main.myPlayer);
                        Main.projectile[projectile].friendly = true;
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
                        touchedTile = false;
                        savedPosition = Vector2.Zero;
                    }
                }
                else
                {
                    clickFrames = false;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                if (npcExplosionTimer >= 0)
                {
                    npcExplosionTimer--;
                }
                if (!attackFrames)
                {
                    Vector2 vector131 = player.Center;
                    vector131.X -= (float)((12 + player.width / 2) * player.direction);
                    projectile.direction = (projectile.spriteDirection = player.direction);
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.rotation = 0;
                }
                float targetDist = maxDistance * 1.5f;
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
                float touchedTargetDistance = 0f;
                if (savedTarget != null)
                {
                    touchedTargetDistance = Vector2.Distance(player.Center, savedTarget.Center);
                    if (!savedTarget.active)
                    {
                        savedTarget = null;
                    }
                }
                if (savedTarget == null)
                {
                    explosionTimer = 0;
                    npcExplosionTimer = 0;
                }
                if (savedTarget != null && touchedTargetDistance > maxDistance + 8f && npcExplosionTimer <= 0)       //if the target leaves and the bomb won't damage you, detonate the enemy
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
                        int bomb = Projectile.NewProjectile(savedTarget.position, Vector2.Zero, ProjectileID.GrenadeIII, (int)(altDamage * modPlayer.standDamageBoosts), 3f, Main.myPlayer);
                        Main.projectile[bomb].timeLeft = 2;
                        Main.projectile[bomb].netUpdate = true;
                        explosionTimer = 0;
                        npcExplosionTimer = 360;
                        savedTarget = null;
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
                    if (targetPos.X > projectile.position.X)
                    {
                        projectile.velocity.X = 4f;
                    }
                    if (targetPos.X < projectile.position.X)
                    {
                        projectile.velocity.X = -4f;
                    }
                    if (targetPos.Y > projectile.position.Y)
                    {
                        projectile.velocity.Y = 4f;
                    }
                    if (targetPos.Y < projectile.position.Y)
                    {
                        projectile.velocity.Y = -4f;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += punchTime;
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
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)((punchDamage * modPlayer.standDamageBoosts) * 0.9f), 3f, Main.myPlayer, fistWhoAmI);
                            Main.projectile[proj].netUpdate = true;
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
            if (!touchedTile)
            {
                mouseToPlayerDistance = Vector2.Distance(Main.MouseWorld, player.Center);
            }
            if (touchedTile && MyPlayer.AutomaticActivations)
            {
                for (int i = 0; i < 200; i++)
                {
                    npcDistance = Vector2.Distance(Main.npc[i].Center, savedPosition);
                    if (npcDistance < 50f && touchedTile)       //or youd need to go from its center, add half its width to the direction its facing, and then add 16 (also with direction) -- Direwolf
                    {
                        int projectile = Projectile.NewProjectile(savedPosition, Vector2.Zero, ProjectileID.GrenadeIII, (int)(altDamage * modPlayer.standDamageBoosts), 50f, Main.myPlayer);
                        Main.projectile[projectile].friendly = true;
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
                        touchedTile = false;
                        savedPosition = Vector2.Zero;
                    }
                }
            }

            Vector2 direction = player.Center - projectile.Center;
            float distanceTo = direction.Length();
            maxDistance = 98f + modPlayer.standRangeBoosts;
            maxAltDistance = 262f + modPlayer.standRangeBoosts;
            if (distanceTo > maxDistance)
            {
                if (projectile.position.X <= player.position.X - 15f)
                {
                    projectile.velocity = player.velocity + new Vector2(0.8f, 0f);
                }
                if (projectile.position.X >= player.position.X + 15f)
                {
                    projectile.velocity = player.velocity + new Vector2(-0.8f, 0f);
                }
                if (projectile.position.Y >= player.position.Y + 15f)
                {
                    projectile.velocity = player.velocity + new Vector2(0f, -0.8f);
                }
                if (projectile.position.Y <= player.position.Y - 15f)
                {
                    projectile.velocity = player.velocity + new Vector2(0f, 0.8f);
                }
            }
            if (distanceTo >= maxDistance + 22f)
            {
                if (!modPlayer.StandAutoMode)
                {
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                projectile.Center = player.Center;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(normalFrames);
            writer.Write(attackFrames);
            writer.Write(clickFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            normalFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
            clickFrames = reader.ReadBoolean();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxDistance / 122.5f, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.Orange * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxAltDistance / 160f, SpriteEffects.None, 0);

            }
            if (touchedTile)
            {
                Texture2D texture = mod.GetTexture("Extras/Bomb");
                spriteBatch.Draw(texture, savedPosition - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
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
                if (projectile.frameCounter >= punchTime)
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
                if (projectile.frameCounter >= 18)      //18 to match it up with the explosion if you want
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