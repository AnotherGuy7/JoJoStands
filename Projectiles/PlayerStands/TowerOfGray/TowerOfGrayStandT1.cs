using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ModLoader;


namespace JoJoStands.Projectiles.PlayerStands.TowerOfGray
{
    public class TowerOfGrayStandT1 : StandClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/TowerOfGray/TowerOfGrayStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void ExtraSetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 34;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 0;
            Projectile.ignoreWater = true;
        }

        public override int ProjectileDamage => 12;
        public override int ShootTime => 12;
        public override int FistID => 13;
        public override int TierNumber => 1;
        public override string PoseSoundName => "TowerOfGray";
        public override string SpawnSoundName => "Tower of Gray";
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override bool CanUseRangeIndicators => false;

        private float controlRange = 250f;
        private const float ManualModeRange = 20 * 16;
        private const float RemoteModeRange = 56 * 16;
        private const int MovementSafetyDistance = 10;

        private bool returnToPlayer = false;
        private bool returnToRange = false;
        private bool mouseControlled = false;
        private bool stinger = false;
        private bool remoteMode = false;
        private bool arrayClear = false;

        private int travelPointChoose = 0;
        private int travelPointRandY = 30;
        private int travelPointRandX = 60;
        private int returnTimer = 0;
        private int emoteTimer = 0;
        private Vector2 projPos = Vector2.Zero;

        public override void AI()
        {
            SelectFrame();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (Vector2.Distance(player.Center, Main.MouseWorld) <= controlRange * 0.9f && mouseControlled || !mouseControlled || Vector2.Distance(player.Center, Projectile.Center) <= controlRange * 0.9f && mouseControlled)
            {
                if (Projectile.velocity.X > 0.5f)
                    Projectile.spriteDirection = 1;
                if (Projectile.velocity.X < -0.5f)
                    Projectile.spriteDirection = -1;
            }

            if (remoteMode)
            {
                if (emoteTimer <= 0)
                {
                    emoteTimer += 180;
                    EmoteBubble.NewBubble(89, new WorldUIAnchor(player), emoteTimer);
                }
                player.aggro -= 1200;
                player.eyeHelper.BlinkBecausePlayerGotHurt();
                float range = RemoteModeRange;
                if (mPlayer.usedEctoPearl)
                    range *= 1.5f;

                controlRange = range;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                if (shootCount <= 0)
                {
                    shootCount += newShootTime;
                    AttackAI(Main.MouseWorld);
                }
                if (!mouseControlled)
                    MovementAI(Projectile.Center + new Vector2(100f * Projectile.spriteDirection, 0f), 0f);
            }
            else
            {
                float range = ManualModeRange;
                if (mPlayer.usedEctoPearl)
                    range *= 1.5f;

                controlRange = range;
            }

            if (!returnToPlayer && !returnToRange)
                Array.Clear(Projectile.oldPos, 0, Projectile.oldPos.Length);

            if (emoteTimer > 0)
                emoteTimer--;

            if (Vector2.Distance(player.Center, Projectile.Center) <= controlRange * 0.9f)
                Projectile.rotation = Projectile.velocity.X * 0.05f;

            Projectile.tileCollide = true;
            NPC target = FindNearestTarget(controlRange);
            if (!Main.mouseLeft && Projectile.owner == Main.myPlayer)
                mouseControlled = false;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && target == null || mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && !mouseControlled && !remoteMode)
                stinger = false;

            if (!returnToPlayer && !returnToRange) // basic stand control
            {
                if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual || mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
                {
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                    {
                        mouseControlled = true;
                        float remoteDistanceOffset = remoteMode ? 0f : 20f;
                        float mouseDistance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
                        if (Vector2.Distance(Projectile.Center, player.Center) < controlRange - remoteDistanceOffset + MovementSafetyDistance)
                        {
                            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
                            {
                                if (mouseDistance > 25f)
                                    MovementAI(Main.MouseWorld, 18f + player.moveSpeed * 2);
                                else
                                    MovementAI(Main.MouseWorld, (mouseDistance * (18f + player.moveSpeed * 2)) / 25);
                            }
                            else
                            {
                                if (mouseDistance > 120f)
                                    MovementAI(Main.MouseWorld, 18f + player.moveSpeed * 2);
                                else
                                {
                                    Vector2 velocity = Main.MouseWorld - Projectile.Center;
                                    velocity.Normalize();
                                    velocity *= 0.78f;
                                    if (Math.Abs(Projectile.velocity.X - velocity.X) > 6)
                                        velocity.X = (Math.Abs(velocity.X) / velocity.X) * 0.08f;
                                    if (Math.Abs(Projectile.velocity.Y - velocity.Y) > 6)
                                        velocity.Y = (Math.Abs(velocity.Y) / velocity.Y) * 0.08f;

                                    Projectile.velocity += velocity;
                                    if (Projectile.velocity.Length() > 6f)
                                        Projectile.velocity *= 0.92f;
                                }
                            }
                            if (Vector2.Distance(player.Center, Projectile.Center) > controlRange * 0.9f && Vector2.Distance(player.Center, Main.MouseWorld) > controlRange * 0.9f)
                            {
                                Projectile.velocity *= 0f;
                                Projectile.netUpdate = true;
                            }
                        }
                        if (shootCount <= 0 && !remoteMode)
                        {
                            shootCount += newShootTime;
                            AttackAI(Main.MouseWorld);
                        }
                        LimitDistance(controlRange - remoteDistanceOffset);
                    }

                    if (SpecialKeyPressed(false) && Projectile.owner == Main.myPlayer)
                    {
                        remoteMode = !remoteMode;
                        if (remoteMode)
                        {
                            Main.NewText("Remote Mode: Active");
                            mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
                        }
                        else
                        {
                            Main.NewText("Remote Mode: Disabled");
                            mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;
                        }
                    }
                }

                else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto) // automode attack ai (only half of original damage)  
                {
                    remoteMode = false;
                    if (target != null)
                    {
                        stinger = true;
                        if (Projectile.Distance(target.Center) > 50f)
                        {
                            MovementAI(target.Center, 10f);

                            Projectile.direction = 1;
                            if (Projectile.velocity.X < 0f)
                                Projectile.direction = -1;
                            Projectile.spriteDirection = Projectile.direction;
                            Projectile.netUpdate = true;
                        }
                        if (shootCount <= 0)
                        {
                            if (Main.myPlayer == Projectile.owner)
                            {
                                shootCount += newShootTime;
                                Vector2 shootVel = target.Center - Projectile.Center;
                                if (shootVel == Vector2.Zero)
                                    shootVel = new Vector2(0f, 1f);

                                shootVel.Normalize();
                                shootVel *= 0f;
                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newProjectileDamage * 0.5f), 3f, Projectile.owner, FistID, TierNumber);
                                Main.projectile[projIndex].netUpdate = true;
                                Projectile.netUpdate = true;
                            }
                        }
                    }
                }

                if (Vector2.Distance(Projectile.Center, player.Center) > controlRange + 20f && !returnToPlayer && !returnToRange)
                {
                    arrayClear = true;
                    returnToRange = true;
                }
                if (!remoteMode)
                {
                    if (!mouseControlled && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && Projectile.owner == Main.myPlayer || mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && target == null && Projectile.owner == Main.myPlayer) // Idle AI
                    {
                        if (Projectile.Distance(player.Center) >= 100f)
                        {
                            travelPointChoose = 0;
                            travelPointRandY = 30;
                            travelPointRandX = 60;
                            if (Projectile.Distance(player.Center) < 1000f)
                                MovementAI(player.Center, 10f + player.moveSpeed);
                            if (Projectile.Distance(player.Center) >= 100f && Projectile.Distance(player.Center) <= 125f)
                                MovementAI(player.Center, ((Projectile.Distance(player.Center) - 95f) * (10f + player.moveSpeed)) / 25);
                        }
                        if (Projectile.Distance(player.Center) < 100f)
                        {
                            Vector2 rightTravelBound = new Vector2(player.Center.X + travelPointRandX, player.Center.Y - travelPointRandY);
                            Vector2 leftTravelBound = new Vector2(player.Center.X - travelPointRandX, player.Center.Y - travelPointRandY);
                            if (travelPointChoose == 0)
                            {
                                if (Projectile.Distance(rightTravelBound) < Projectile.Distance(leftTravelBound))
                                    travelPointChoose = 1;
                                else
                                    travelPointChoose = 2;
                            }
                            else
                            {
                                if (Projectile.Distance(rightTravelBound) < 3f)
                                {
                                    travelPointChoose = 1;
                                    travelPointRandX = (int)Main.rand.NextFloat(30, 60);
                                    travelPointRandY = (int)Main.rand.NextFloat(-10, 30);
                                }
                                if (Projectile.Distance(leftTravelBound) < 3f)
                                {
                                    travelPointChoose = 2;
                                    travelPointRandX = (int)Main.rand.NextFloat(30, 60);
                                    travelPointRandY = (int)Main.rand.NextFloat(-10, 30);
                                }
                                if (travelPointChoose == 2)
                                    MovementAI(rightTravelBound, 0.25f + player.moveSpeed);
                                else if (travelPointChoose == 1)
                                    MovementAI(leftTravelBound, 0.25f + player.moveSpeed);
                            }
                        }
                    }
                }

                if (!mouseControlled && !remoteMode) //prevents stand from getting stuck
                {
                    if (Projectile.Distance(projPos) > 1f)
                    {
                        projPos = Projectile.Center;
                        returnTimer = 0;
                    }
                    if (Projectile.Distance(projPos) <= 1f)
                        returnTimer++;

                    if (returnTimer >= 90)
                    {
                        arrayClear = true;
                        returnToPlayer = true;
                        returnTimer = 0;
                    }
                }
            }

            if (Projectile.Distance(player.Center) <= controlRange * 0.9f && returnToRange) //if suddenly stand is out of range
            {
                MovementAI(player.Center, 0f);
                returnToRange = false;
            }

            if (Projectile.Distance(player.Center) >= 1000f && !returnToPlayer && !remoteMode) //if suddenly stand is too far
            {
                arrayClear = true;
                returnToPlayer = true;
            }
            if (Projectile.Distance(player.Center) <= 10f)
                returnToPlayer = false;

            if (returnToPlayer || returnToRange)
            {
                if (returnToPlayer)
                    returnToRange = false;
                Projectile.tileCollide = false;
                MovementAI(player.Center, 20f + player.moveSpeed * 2);
            }
            if (player.teleporting)
                Projectile.position = player.position;
        }

        private void MovementAI(Vector2 target, float speed)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                if (target == Projectile.Center)
                    return;

                Projectile.velocity = target - Projectile.Center;
                Projectile.velocity.Normalize();
                Projectile.velocity *= speed;
            }
            Projectile.netUpdate = true;
        }

        private void AttackAI(Vector2 target)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            stinger = true;
            Vector2 shootVel = target - Projectile.Center;
            if (shootVel == Vector2.Zero)
                shootVel = new Vector2(0f, 1f);

            shootVel.Normalize();
            shootVel *= 0f;
            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newProjectileDamage * mPlayer.towerOfGrayDamageMult), 3f, Projectile.owner, FistID, TierNumber);
            Main.projectile[projIndex].netUpdate = true;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (returnToPlayer || returnToRange)
            {
                if (arrayClear)
                {
                    Array.Clear(Projectile.oldPos, 0, Projectile.oldPos.Length);
                    arrayClear = false;
                }
                if (!arrayClear)
                {
                    for (int oldPos = Projectile.oldPos.Length - 1; oldPos > 0; oldPos--)
                        Projectile.oldPos[oldPos] = Projectile.oldPos[oldPos - 1];
                    Projectile.oldPos[0] = Projectile.position;

                    SpriteEffects effects = (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 centerOffset = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2, TextureAssets.Projectile[Projectile.type].Value.Height / 4);
                    for (int oldPos = 0; oldPos < Projectile.oldPos.Length; oldPos++)
                    {
                        Vector2 drawPosition = Projectile.oldPos[oldPos] - Main.screenPosition + centerOffset + new Vector2(0f, Projectile.gfxOffY);
                        Color drawColor = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - oldPos) / (float)Projectile.oldPos.Length);
                        Rectangle animRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);
                        Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPosition, animRect, drawColor, Projectile.rotation, centerOffset, Projectile.scale, effects, 0.3f);
                    }
                }
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer) //AG, i'm pretty sure that i'm creating a mess here, but in this way everything works stable in multiplayer. Sorry in Advance. (C) Proos <3
        {
            writer.Write(returnToPlayer);
            writer.Write(returnToRange);
            writer.Write(mouseControlled);
            writer.Write(stinger);
            writer.Write(remoteMode);
            writer.Write(arrayClear);
            writer.Write(emoteTimer);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            returnToPlayer = reader.ReadBoolean();
            returnToRange = reader.ReadBoolean();
            mouseControlled = reader.ReadBoolean();
            stinger = reader.ReadBoolean();
            remoteMode = reader.ReadBoolean();
            arrayClear = reader.ReadBoolean();
            emoteTimer = reader.ReadInt32();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void SelectFrame()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (!stinger)
                {
                    if (Projectile.frame > 1)
                        Projectile.frame = 0;
                }
                else
                {
                    if (Projectile.frame > 3)
                        Projectile.frame = 2;
                }
            }
        }

        public override DrawData BuildStandDesummonDrawData()
        {
            standTexture = (Texture2D)ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad);
            int frameHeight = standTexture.Height / Main.projFrames[Projectile.type];
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Rectangle animRect = new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight);
            Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            return new DrawData(standTexture, drawPosition, animRect, Color.White, Projectile.rotation, standOrigin, 1f, effects, 0);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = Projectile.width - 18;
            height = Projectile.height - 8;
            fallThrough = true;
            return true;
        }
    }
}