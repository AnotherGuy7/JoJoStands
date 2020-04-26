using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;
using System.IO;

namespace JoJoStands.Projectiles.PlayerStands.GER
{
    public class GoldExperienceRequiemStand : ModProjectile      //has 2 poses
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            //projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 2;
        }

        public Texture2D standTexture;

        public Vector2 velocityAddition = Vector2.Zero;
        public float mouseDistance = 0f;
        protected float shootSpeed = 16f;
        public bool normalFrames = false;       //these bools are needed to sync
        public bool attackFrames = false;
        public bool rockFlickFrames = false;
        public float maxDistance = 0f;
        public int punchDamage = 138;
        public int shootCount = 0;
        public int punchTime = 9;
        public int halfStandHeight = 34;
        public float fistWhoAmI = 3f;
        public float tierNumber = 5f;
        public bool saidAbility = true;
        public int regencounter = 0;
        public int updateTimer = 0;

        public override void AI()
        {
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !rockFlickFrames)
                {
                    attackFrames = true;
                    normalFrames = false;
                    rockFlickFrames = false;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);
                    if (Main.MouseWorld.X > projectile.position.X)
                    {
                        projectile.spriteDirection = 1;
                        projectile.direction = 1;
                        drawOffsetX = 20;
                    }
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.spriteDirection = -1;
                        projectile.direction = -1;
                        drawOffsetX = -20;
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
                        shootCount += punchTime - modPlayer.standSpeedBoosts - modPlayer.standSpeedBoosts;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), (int)(punchDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, fistWhoAmI);
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
                    if (!rockFlickFrames)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                    Vector2 vector131 = player.Center;
                    vector131.X -= (float)((12 + player.width / 2) * player.direction);
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.direction = (projectile.spriteDirection = player.direction);
                    projectile.rotation = 0;
                    drawOffsetX = 0;
                }
                if (!attackFrames && projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 0)
                    {
                        normalFrames = false;
                        attackFrames = false;
                        rockFlickFrames = true;
                    }
                    if (Main.mouseRight && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 1)
                    {
                        Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 65f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, Main.myPlayer, tierNumber);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), 600);
                    }
                    if (Main.mouseRight && modPlayer.GEAbilityNumber == 2 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("DeathLoop")))
                    {
                        player.AddBuff(mod.BuffType("DeathLoop"), 1500);
                    }
                    if (Main.mouseRight && player.velocity == Vector2.Zero && modPlayer.GEAbilityNumber == 3)
                    {
                        regencounter++;
                    }
                    else
                    {
                        regencounter = 0;
                    }
                    if (regencounter > 80)
                    {
                        int healamount = Main.rand.Next(25, 50);
                        player.statLife += healamount;
                        player.HealEffect(healamount);
                        regencounter = 0;
                    }
                    if (Main.mouseRight && modPlayer.GEAbilityNumber == 4 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("BacktoZero")))
                    {
                        player.AddBuff(mod.BuffType("BacktoZero"), 1200);
                        modPlayer.BackToZero = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModNetHandler.effectSync.SendBTZ(256, player.whoAmI, true, player.whoAmI);
                        }
                    }
                }

                if (projectile.owner == Main.myPlayer)
                {
                    if (JoJoStands.SpecialHotKey.JustPressed)
                    {
                        modPlayer.GEAbilityNumber += 1;
                        saidAbility = false;
                    }
                    if (modPlayer.GEAbilityNumber >= 6)
                    {
                        modPlayer.GEAbilityNumber = 0;
                    }
                    if (modPlayer.GEAbilityNumber == 0)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Scorpion Rock");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 1)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Tree");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 2)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Death Loop");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 3)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Limb Recreation");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 4)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Back to Zero");
                            saidAbility = true;
                        }
                    }
                }
                if (rockFlickFrames)
                {
                    normalFrames = false;
                    attackFrames = false;
                    projectile.netUpdate = true;
                    if (projectile.frame == 8 && shootCount <= 0)
                    {
                        shootCount += punchTime - modPlayer.standSpeedBoosts;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 12f;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("GoldExperienceRock"), (int)(punchDamage * modPlayer.standDamageBoosts) + 11, 6f, Main.myPlayer);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), 180);
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
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
            Vector2 direction = player.Center - projectile.Center;
            float distanceTo = direction.Length();
            maxDistance = 98f + modPlayer.standRangeBoosts;
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
            if (attackFrames)
            {
                normalFrames = false;
                rockFlickFrames = false;
                SwitchStatesTo("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                rockFlickFrames = false;
                SwitchStatesTo("Idle");
            }
            if (rockFlickFrames)
            {
                SwitchStatesTo("Flick");
            }
            if (modPlayer.poseMode)
            {
                if (Main.mouseLeft || Main.mouseRight)
                {
                    modPlayer.poseMode = false;
                }
                attackFrames = false;
                normalFrames = false;
                rockFlickFrames = false;
                SwitchStatesTo("Pose");
            }
        }

        public SpriteEffects effects = SpriteEffects.None;

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            if (projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.None;
            }
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxDistance / 122.5f, SpriteEffects.None, 0);
            }
            int frameHeight = standTexture.Height / Main.projFrames[projectile.whoAmI];
            spriteBatch.Draw(standTexture, projectile.Center - Main.screenPosition + new Vector2(drawOffsetX, drawOriginOffsetY), new Rectangle(0, frameHeight * projectile.frame, standTexture.Width, frameHeight), lightColor, 0f, new Vector2(standTexture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackFrames);
            writer.Write(normalFrames);
            writer.Write(rockFlickFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackFrames = reader.ReadBoolean();
            normalFrames = reader.ReadBoolean();
            rockFlickFrames = reader.ReadBoolean();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public virtual void SwitchStatesTo(string animationName)
        {
            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 8, 10, true);
            }
            if (animationName == "Flick")
            {
                AnimationStates(animationName, 17, 11, false);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 200, true, true);     //will loop that frame
            }
        }

        public virtual void AnimationStates(string stateName, int frameAmount, int frameCounterLimit, bool loop, bool loopCertainFrames = false, int loopFrameStart = 0, int loopFrameEnd = 0)
        {
            Main.projFrames[projectile.whoAmI] = frameAmount;
            projectile.frameCounter++;
            standTexture = mod.GetTexture("Projectiles/PlayerStands/GER/GER_" + stateName);
            if (projectile.frameCounter >= frameCounterLimit)
            {
                projectile.frameCounter = 0;
                projectile.frame += 1;
            }
            if (loopCertainFrames)
            {
                if (projectile.frame >= loopFrameEnd)
                {
                    projectile.frame = loopFrameStart;
                }
            }
            if (projectile.frame >= frameAmount && loop)
            {
                projectile.frame = 0;
            }
            if (projectile.frame >= frameAmount && !loop)
            {
                SwitchStatesTo("Idle");
                rockFlickFrames = false;
            }
        }
    }
}