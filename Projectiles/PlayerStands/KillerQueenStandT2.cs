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
    public class KillerQueenStandT2 : StandClass
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

        public override int punchDamage => 35;
        public override int altDamage => 40;
        public override int punchTime => 11;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 5f;
        public override float maxAltDistance => 201f;     //about 10 tiles


        public float npcDistance = 0f;
        public float mouseToPlayerDistance = 0f;
        public Vector2 savedPosition = Vector2.Zero;
        public bool touchedTile = false;
        public int timeAfterTouch = 0;
        public int explosionTimer = 0;

        public static NPC savedTarget = null;
        public int npcExplosionTimer = 0;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
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
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehind();
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
                        secondaryAbilityFrames = true;
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
                    secondaryAbilityFrames = false;
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
                    secondaryAbilityFrames = true;
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
                            shootCount += punchTime - modPlayer.standSpeedBoosts;
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
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), (maxDistance + mPlayer.standRangeBoosts) / 122.5f, SpriteEffects.None, 0);
                if (maxAltDistance != 0f)
                {
                    spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.Orange * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), (maxAltDistance + mPlayer.standRangeBoosts) / 160f, SpriteEffects.None, 0);
                }
            }
            if (touchedTile)
            {
                Texture2D texture = mod.GetTexture("Extras/Bomb");
                spriteBatch.Draw(texture, savedPosition - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
            }
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                secondaryAbilityFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= punchTime - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
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
            if (secondaryAbilityFrames)
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
                    secondaryAbilityFrames = false;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                secondaryAbilityFrames = false;
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
                secondaryAbilityFrames = false;
                projectile.frame = 9;
            }
        }
    }
}