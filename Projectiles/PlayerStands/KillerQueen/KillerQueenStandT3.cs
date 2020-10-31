using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueen
{
    public class KillerQueenStandT3 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override int punchDamage => 59;
        public override int altDamage => 68;
        public override int punchTime => 10;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 5f;
        public override float maxAltDistance => 232f;     //about 10 tiles
        public override string poseSoundName => "IWouldntLose";
        public override int standType => 1;

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
            SelectAnimation();
            UpdateStandInfo();
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
                    StayBehind();
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
                if (savedTarget != null && touchedTargetDistance > newMaxDistance + 8f && npcExplosionTimer <= 0)       //if the target leaves and the bomb won't damage you, detonate the enemy
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
                    Vector2 velocity = targetPos - projectile.position;
                    velocity.Normalize();
                    projectile.velocity = velocity * 4f;
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newPunchTime;
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
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(newPunchDamage * 0.9f), 3f, Main.myPlayer, fistWhoAmI, tierNumber);
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

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            if (touchedTile)
            {
                Texture2D texture = mod.GetTexture("Extras/Bomb");
                spriteBatch.Draw(texture, savedPosition - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
            }
            return true;
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
                if (projectile.frame >= 4)      //cause it should only click once
                {
                    secondaryAbilityFrames = false;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/KillerQueen/KillerQueen_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 20, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 6, 18, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 2, true);
            }
        }
    }
}