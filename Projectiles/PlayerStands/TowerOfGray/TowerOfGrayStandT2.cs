using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using JoJoStands.Buffs.Debuffs;
using Terraria.GameContent.UI;


namespace JoJoStands.Projectiles.PlayerStands.TowerOfGray
{
    public class TowerOfGrayStandT2 : StandClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/TowerOfGray/TowerOfGrayStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
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

        public override int punchDamage => 21;
        public override int punchTime => 9;
        public override float fistWhoAmI => 13f;    
        public override StandType standType => StandType.Melee;

        private bool returnToPlayer = false;
        private bool returnToRange = false;
        private bool mouseControlled = false;
        private bool dash = false;
        private bool stinger = false;
        private bool remoteMode = false;
        private bool arrayClear = false;

        private int travelPointChoose = 0;
        private int travelPointRandY = 30;
        private int travelPointRandX = 60;
        private int returnback = 0;
        private int offset = 0;
        private int standTier = 2;
        private int frameMultUpdate = 0;
        private int emote = 0;

        private float range = 250f; 
        private float noRemoteRange = 300f; //250f 300f 350f 400f
        private float remoteRange = 700f; //550f 700f 850f 1000f

        private Vector2 dashPoint = Vector2.Zero;
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

            mPlayer.towerOfGrayTier = standTier;
            mPlayer.standRemoteMode = remoteMode;

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (Vector2.Distance(player.Center, Main.MouseWorld) < range && mouseControlled || !mouseControlled || Vector2.Distance(player.Center, Projectile.Center) <= range * 0.9f && mouseControlled)
            {
                if (Projectile.velocity.X > 0)
                    Projectile.spriteDirection = 1;
                if (Projectile.velocity.X < 0)
                    Projectile.spriteDirection = -1;
            }

            if (mPlayer.usedEctoPearl && noRemoteRange == 250f)
                noRemoteRange *= 1.5f;
            if (mPlayer.usedEctoPearl && remoteRange == 550f)
                remoteRange *= 1.5f;

            if (!remoteMode)
                range = noRemoteRange;
            if (remoteMode)
            {
                if (emote == 0)
                {
                    emote += 180;
                    EmoteBubble.NewBubble(89, new WorldUIAnchor(player), emote);
                }
                range = remoteRange;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                if (!mouseControlled)
                    MovementAI(Projectile.Center + new Vector2(100f * Projectile.spriteDirection, 0f), 0f);
            }

            if (!dash && !returnToPlayer && !returnToRange)
                Array.Clear(Projectile.oldPos, 0, Projectile.oldPos.Length);

            if (emote > 0)
                emote--;

            Projectile.rotation = Projectile.velocity.X * 0.05f;
            Projectile.tileCollide = true;
            mouseControlled = false;

            float distance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
            NPC target = FindNearestTarget(range);
            if (!returnToPlayer && !returnToRange) // basic stand control
            {
                if (!mPlayer.standAutoMode)
                {
                    if (Main.mouseLeft && !dash && Projectile.owner == Main.myPlayer)
                    {
                        mouseControlled = true;
                        float remote = 0f;
                        if (remoteMode)
                            remote = 0f;
                        if (!remoteMode)
                            remote = 20f;
                        if (Vector2.Distance(Projectile.Center, player.Center) < range - remote)
                        {
                            if (distance > 25f)
                                MovementAI(Main.MouseWorld, 10f + player.moveSpeed);
                            if (distance <= 25f)
                                MovementAI(Main.MouseWorld, (distance * (10f + player.moveSpeed)) / 25);
                        }
                        if (shootCount <= 0)
                        {
                            shootCount += newPunchTime;
                            AttackAI(Main.MouseWorld);
                        }
                        LimitDistance(range);
                    }
                    if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>())) //right click abilty activation
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            arrayClear = true;
                            dash = true;
                            dashPoint = Main.MouseWorld;
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                        }
                    }

                    if (SpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer)
                    {
                        remoteMode = !remoteMode;
                        if (remoteMode)
                            Main.NewText("Remote Mode: Active");
                        else
                            Main.NewText("Remote Mode: Disabled");
                    }
                }

                if (dash && Projectile.owner == Main.myPlayer) //right click abilty
                {
                    Projectile.tileCollide = false;
                    MovementAI(dashPoint, 20f + player.moveSpeed);
                    mPlayer.towerOfGrayDamageMult = 5f;

                    AttackAI(dashPoint);

                    if (Projectile.Distance(dashPoint) <= 20f)
                    {
                        dash = false;
                        dashPoint = Vector2.Zero;
                    }
                }

                if (mPlayer.standAutoMode && !dash) // automode attack ai (only half of original damage)  
                {
                    remoteMode = false;
                    if (target != null)
                    {
                        stinger = true;
                        if (Projectile.Distance(target.Center) > 30f)
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
                                shootCount += newPunchTime;
                                Vector2 shootVel = target.Center - Projectile.Center;
                                if (shootVel == Vector2.Zero)
                                    shootVel = new Vector2(0f, 1f);

                                shootVel.Normalize();
                                shootVel *= shootSpeed;
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 0.5f), 3f, Projectile.owner, fistWhoAmI, tierNumber);
                                Main.projectile[proj].netUpdate = true;
                                Projectile.netUpdate = true;
                            }
                        }
                    }
                }

                if (!dash) //return to stand range after ability
                {
                    if (mPlayer.standAutoMode && target == null || !mPlayer.standAutoMode && !mouseControlled)
                        stinger = false;

                    if (Vector2.Distance(Projectile.Center, player.Center) > range + 20f && !returnToPlayer && !returnToRange)
                    {
                        arrayClear = true;
                        returnToRange = true;
                    }
                    if (!remoteMode)
                    {
                        if (!mouseControlled && !mPlayer.standAutoMode && Projectile.owner == Main.myPlayer || mPlayer.standAutoMode && target == null && Projectile.owner == Main.myPlayer) // Idle AI
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
                                Vector2 travelPoint1 = new Vector2(player.Center.X + travelPointRandX, player.Center.Y - travelPointRandY);
                                Vector2 travelPoint2 = new Vector2(player.Center.X - travelPointRandX, player.Center.Y - travelPointRandY);

                                if (Projectile.Distance(travelPoint1) < Projectile.Distance(travelPoint2) && travelPointChoose == 0)
                                    travelPointChoose = 1;
                                if (Projectile.Distance(travelPoint1) >= Projectile.Distance(travelPoint2) && travelPointChoose == 0)
                                    travelPointChoose = 2;
                                if (travelPointChoose > 0)
                                {
                                    if (Projectile.Distance(travelPoint1) < 3f)
                                    {
                                        travelPointChoose = 1;
                                        travelPointRandY = (int)Main.rand.NextFloat(-10, 30);
                                        travelPointRandX = (int)Main.rand.NextFloat(30, 60);
                                    }
                                    if (Projectile.Distance(travelPoint2) < 3f)
                                    {
                                        travelPointChoose = 2;
                                        travelPointRandY = (int)Main.rand.NextFloat(-10, 30);
                                        travelPointRandX = (int)Main.rand.NextFloat(30, 60);
                                    }
                                    if (travelPointChoose == 2)
                                        MovementAI(travelPoint1, 0.25f + player.moveSpeed);
                                    if (travelPointChoose == 1)
                                        MovementAI(travelPoint2, 0.25f + player.moveSpeed);
                                }
                            }
                        }
                    }
                }

                if (!mouseControlled && !remoteMode) //prevents stand from getting stuck
                {
                    if (Projectile.Distance(projPos) > 1f)
                    {
                        projPos = Projectile.Center;
                        returnback = 0;
                    }
                    if ((Projectile.Distance(projPos) <= 1f))
                        returnback++;

                    if (returnback >= 90)
                    {
                        arrayClear = true;
                        returnToPlayer = true;
                        returnback = 0;
                    }
                }
            }

            if (Projectile.Distance(player.Center) <= range * 0.9f && returnToRange) //if suddenly stand is out of range
            {
                MovementAI(player.Center, 0f);
                returnToRange = false;
            }

            if (Projectile.Distance(player.Center) >= 1000f && !dash && !returnToPlayer && !remoteMode) //if suddenly stand is too far
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
                dash = false;
                dashPoint = Vector2.Zero;
                MovementAI(player.Center, 20f + player.moveSpeed * 2);
            }
        }

        private void MovementAI(Vector2 target, float speed)
        {
            if (Projectile.owner == Main.myPlayer)
            {
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
            {
                shootVel = new Vector2(0f, 1f);
            }
            shootVel.Normalize();
            shootVel *= shootSpeed;
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * mPlayer.towerOfGrayDamageMult), 3f, Projectile.owner, fistWhoAmI, tierNumber);
            Main.projectile[proj].netUpdate = true;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (dash || returnToPlayer || returnToRange)
            {
                if (arrayClear)
                {
                    Array.Clear(Projectile.oldPos, 0, Projectile.oldPos.Length);
                    arrayClear = false;
                }
                if (!arrayClear)
                {
                    if (Projectile.frame == 0)
                        offset = 0;
                    if (Projectile.frame == 1)
                        offset = 24;
                    if (Projectile.frame == 2)
                        offset = 48;
                    if (Projectile.frame == 3)
                        offset = 72;

                    for (int oldPos2 = Projectile.oldPos.Length - 1; oldPos2 > 0; oldPos2--)
                        Projectile.oldPos[oldPos2] = Projectile.oldPos[oldPos2 - 1];
                    Projectile.oldPos[0] = Projectile.position;

                    SpriteEffects effects = (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 vector2 = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2, TextureAssets.Projectile[Projectile.type].Value.Height / 4);
                    for (int oldPos2 = 0; oldPos2 < Projectile.oldPos.Length; oldPos2++)
                    {
                        Vector2 vector2_2 = Projectile.oldPos[oldPos2] - Main.screenPosition + vector2 + new Vector2(0f, Projectile.gfxOffY);
                        Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - oldPos2) / (float)Projectile.oldPos.Length);
                        Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, vector2_2, new Rectangle(0, offset, Projectile.width, Projectile.height), color, Projectile.rotation, vector2, Projectile.scale, effects, 0.3f);
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
            writer.Write(dash);
            writer.Write(stinger);
            writer.Write(remoteMode);
            writer.Write(arrayClear);

            writer.Write(standTier);
            writer.Write(frameMultUpdate);
            writer.Write(emote);

            writer.Write(noRemoteRange);
            writer.Write(remoteRange);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            returnToPlayer = reader.ReadBoolean();
            returnToRange = reader.ReadBoolean();
            mouseControlled = reader.ReadBoolean();
            dash = reader.ReadBoolean();
            stinger = reader.ReadBoolean();
            remoteMode = reader.ReadBoolean();
            arrayClear = reader.ReadBoolean();

            standTier = reader.ReadInt32();
            frameMultUpdate = reader.ReadInt32();
            emote = reader.ReadInt32();

            noRemoteRange = reader.ReadSingle();
            remoteRange = reader.ReadSingle();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void SelectFrame()
        {
            Projectile.frame = frameMultUpdate;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                frameMultUpdate += 1;
                Projectile.frameCounter = 0;
                if (!stinger)
                {
                    if (frameMultUpdate > 1)
                        frameMultUpdate = 0;
                }
                if (stinger)
                {
                    if (frameMultUpdate > 3)
                        frameMultUpdate = 2;
                }
            }
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