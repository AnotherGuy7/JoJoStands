using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class TheWorldStandT3 : ModProjectile      //has 2 poses
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/TheWorldStand"; }
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
        public bool knifeFrames = false;
        public bool abilityPose = false;
        public int timestopPoseTimer = 0;
        public float maxDistance = 0f;
        public int punchDamage = 68;
        public int altDamage = 47;
        public int shootCount = 0;
        public int punchTime = 11;
        public int halfStandHeight = 44;
        public bool throwKnife = false;
        public float fistWhoAmI = 1f;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
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
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")) && projectile.owner == Main.myPlayer)
            {
                timestopPoseTimer = 60;
                modPlayer.TimestopEffectDurationTimer = 60;
                modPlayer.TheWorldEffect = true;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_start"));
                player.AddBuff(mod.BuffType("TheWorldBuff"), 300, true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModNetHandler.effectSync.SendTimestop(256, player.whoAmI, true, player.whoAmI);
                }
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    attackFrames = true;
                    normalFrames = false;
                    knifeFrames = false;
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
                    Vector2 vector131 = player.Center;
                    if (!knifeFrames)
                    {
                        vector131.X -= (float)((12 + player.width / 2) * player.direction);
                        projectile.direction = (projectile.spriteDirection = player.direction);
                    }
                    else
                    {
                        vector131.X += (float)((12 + player.width / 2) * player.direction);
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
                    }
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.rotation = 0;
                    normalFrames = true;
                    attackFrames = false;
                    knifeFrames = false;
                }
                if (Main.mouseRight && player.HasItem(mod.ItemType("Knife")) && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    knifeFrames = true;
                    normalFrames = false;
                    attackFrames = false;
                    if (shootCount <= 0 && projectile.frame == 9)
                    {
                        shootCount += 28;
                        float rotationk = MathHelper.ToRadians(15);
                        float numberKnives = 3;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 100f;
                        for (int i = 0; i < numberKnives; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Knife"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                    }
                }
                if (timestopPoseTimer > 0)
                {
                    timestopPoseTimer--;
                    normalFrames = false;
                    attackFrames = false;
                    knifeFrames = false;
                    abilityPose = true;
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                    if (timestopPoseTimer <= 1)
                    {
                        abilityPose = false;
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                Vector2 vector131 = player.Center;
                float targetDist = maxDistance * 3f;
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
                if (targetDist > (maxDistance * 1.5f) || throwKnife || target == null)
                {
                    normalFrames = true;
                    attackFrames = false;
                    knifeFrames = false;
                    if (throwKnife)
                    {
                        vector131.X += (float)((12 + player.width / 2) * player.direction);
                    }
                    else
                    {
                        vector131.X -= (float)((12 + player.width / 2) * player.direction);
                        projectile.spriteDirection = projectile.direction = player.direction;
                    }
                    vector131.Y -= -35f + halfStandHeight;
                    projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                    projectile.velocity *= 0.8f;
                    projectile.rotation = 0;
                }
                if (target != null)
                {
                    if (targetDist < (maxDistance * 1.5f))
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
                    else if (targetDist > (maxDistance * 1.5f) && player.HasItem(mod.ItemType("Knife")))
                    {
                        if (MyPlayer.AutomaticActivations)
                        {
                            throwKnife = true;
                        }
                        else if (Main.rand.Next(0, 76) <= 1)
                        {
                            throwKnife = true;
                        }
                    }
                }
                if (throwKnife)
                {
                    knifeFrames = true;
                    attackFrames = false;
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
                        if (Main.myPlayer == projectile.owner)
                        {
                            if (shootCount <= 0 && projectile.frame == 9)
                            {
                                shootCount += 28;
                                Vector2 shootVel = targetPos - projectile.Center - new Vector2(0f, 2f);
                                if (shootVel == Vector2.Zero)
                                {
                                    shootVel = new Vector2(0f, 1f);
                                }
                                shootVel.Normalize();
                                shootVel *= shootSpeed;
                                int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, shootVel.X, shootVel.Y, mod.ProjectileType("Knife"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, player.whoAmI);
                                Main.projectile[proj].netUpdate = true;
                                projectile.netUpdate = true;
                                player.ConsumeItem(mod.ItemType("Knife"));
                            }
                        }
                    }
                }
            }

            Vector2 direction = player.Center - projectile.Center;      //needs to be down here so velocity changes get applied when the stand enters the rim
            float distanceTo = direction.Length();
            maxDistance = 98f + modPlayer.standRangeBoosts;
            if (distanceTo > maxDistance)
            {
                if (projectile.position.X <= player.position.X - 15f)
                {
                    ////projectile.position = new Vector2(projectile.position.X + 0.2f, projectile.position.Y);
                    projectile.velocity = player.velocity + new Vector2(0.8f, 0f);
                }
                if (projectile.position.X >= player.position.X + 15f)
                {
                    //projectile.position = new Vector2(projectile.position.X - 0.2f, projectile.position.Y);
                    projectile.velocity = player.velocity + new Vector2(-0.8f, 0f);
                }
                if (projectile.position.Y >= player.position.Y + 15f)
                {
                    // projectile.position = new Vector2(projectile.position.X, projectile.position.Y - 0.2f);
                    projectile.velocity = player.velocity + new Vector2(0f, -0.8f);
                }
                if (projectile.position.Y <= player.position.Y - 15f)
                {
                    ///projectile.position = new Vector2(projectile.position.X, projectile.position.Y + 0.2f);
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

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");        //the initial tile amount the indicator covers is 20 tiles, 320 pixels, border is included in the measurements
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), maxDistance / 122.5f, SpriteEffects.None, 0);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(normalFrames);
            writer.Write(attackFrames);
            writer.Write(knifeFrames);
            writer.Write(abilityPose);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            normalFrames = reader.ReadBoolean();
            attackFrames = reader.ReadBoolean();
            knifeFrames = reader.ReadBoolean();
            abilityPose = reader.ReadBoolean();
        }

        public virtual void SelectFrame()
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= 12)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
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
            if (knifeFrames)
            {
                if (projectile.frameCounter >= 28)
                {
                    if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().StandAutoMode && projectile.frame == 9)
                    {
                        throwKnife = false;
                    }
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
            if (abilityPose)
            {
                projectile.frame = 6;
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                knifeFrames = false;
                projectile.frame = 7;
            }
        }
    }
}