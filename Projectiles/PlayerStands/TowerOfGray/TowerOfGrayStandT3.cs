using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using JoJoStands.Buffs.Debuffs;
using Terraria.GameContent.UI;


namespace JoJoStands.Projectiles.PlayerStands.TowerOfGray
{
    public class TowerOfGrayStandT3 : StandClass
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

        public override int ProjectileDamage => 36;
        public override int ShootTime => 10;
        public override int FistWhoAmI => 13;
        public override int TierNumber => 3;
        public override StandAttackType StandType => StandAttackType.Ranged;
        private const int MovementSafetyDistance = 10;

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
        private int pause = 0;
        private int offset = 0;
        private int special = -1;
        private int standTier = 3;
        private int frameMultUpdate = 0;
        private int emote = 0;

        private float range = 250f;
        private float noRemoteRange = 350f; //250f 300f 350f 400f
        private float remoteRange = 1050f; //750f 900f 1050f 1200f

        private Vector2 dashPoint = Vector2.Zero;
        private Vector2 projPos = Vector2.Zero;

        private List<int> specialTargets = new List<int>();

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
            if (remoteMode)
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (Vector2.Distance(player.Center, Main.MouseWorld) <= range * 0.9f && mouseControlled || !mouseControlled || Vector2.Distance(player.Center, Projectile.Center) <= range * 0.9f && mouseControlled)
            {
                if (Projectile.velocity.X > 0.5f)
                    Projectile.spriteDirection = 1;
                if (Projectile.velocity.X < -0.5f)
                    Projectile.spriteDirection = -1;
            }

            if (mPlayer.usedEctoPearl && noRemoteRange == 350f)
                noRemoteRange *= 1.5f;
            if (mPlayer.usedEctoPearl && remoteRange == 1050f)
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
                player.aggro -= 1200;
                player.eyeHelper.BlinkBecausePlayerGotHurt();
                range = remoteRange;
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

            if (!dash && !returnToPlayer && !returnToRange && special == -1)
                Array.Clear(Projectile.oldPos, 0, Projectile.oldPos.Length);

            if (pause > 0)
                pause--;
            if (emote > 0)
                emote--;

            if (Vector2.Distance(player.Center, Projectile.Center) <= range * 0.9f)
                Projectile.rotation = Projectile.velocity.X * 0.05f;
            Projectile.tileCollide = true;

            NPC target = FindNearestTarget(range);

            if (!Main.mouseLeft && !dash && special == -1 && Projectile.owner == Main.myPlayer)
                mouseControlled = false;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && target == null || mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && !mouseControlled && !remoteMode)
                stinger = false;

            if (!returnToPlayer && !returnToRange) // basic stand control
            {
                if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
                {
                    if (Main.mouseLeft && !dash && special == -1 && Projectile.owner == Main.myPlayer)
                    {
                        mouseControlled = true;
                        float remote = 0f;
                        if (remoteMode)
                            remote = 0f;
                        if (!remoteMode)
                            remote = 20f;
                        float mouseDistance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
                        if (Vector2.Distance(Projectile.Center, player.Center) < range - remote + MovementSafetyDistance)
                        {
                            if (mouseDistance > 25f)
                                MovementAI(Main.MouseWorld, 18 + player.moveSpeed*2);
                            else
                                MovementAI(Main.MouseWorld, (mouseDistance * (18f + player.moveSpeed*2)) / 25);
                            if (Vector2.Distance(player.Center, Projectile.Center) > range * 0.9f && Vector2.Distance(player.Center, Main.MouseWorld) > range * 0.9f)
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
                        LimitDistance(range - remote);
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
                    if (SecondSpecialKeyPressed()) // special ability activation
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            arrayClear = true;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active && !npc.hide && !npc.immortal && Projectile.Distance(npc.Center) <= 900f && !npc.friendly & npc.lifeMax > 5)
                                    specialTargets.Add(npc.whoAmI);
                            }
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
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

                if (specialTargets.Count > 0)
                    special = specialTargets[0];
                if (specialTargets.Count == 0)
                    special = -1;

                if (special != -1 && Projectile.owner == Main.myPlayer) // special ability
                {
                    NPC targetSpecial = Main.npc[special];
                    if (Projectile.Distance(targetSpecial.Center) <= 1000f)
                    {
                        Projectile.tileCollide = false;
                        if (pause == 0)
                            MovementAI(targetSpecial.Center, 20f + player.moveSpeed);
                        if (pause > 0)
                        {
                            arrayClear = true;
                            MovementAI(targetSpecial.Center, 0f);
                        }
                        mPlayer.towerOfGrayDamageMult = 10f;
                        if (Projectile.Distance(targetSpecial.Center) <= 10f && pause == 0 && targetSpecial.active && specialTargets.Count == 1)
                        {
                            AttackAI(targetSpecial.Center);
                            specialTargets.Clear();
                            pause += 10;
                        }
                        if (Projectile.Distance(targetSpecial.Center) <= 10f && pause == 0 && specialTargets.Count > 1 && targetSpecial.active)
                        {
                            AttackAI(targetSpecial.Center);
                            specialTargets.Remove(specialTargets[0]);
                            pause += 10;
                        }
                        if (specialTargets.Count > 1 && !targetSpecial.active)
                            specialTargets.Remove(specialTargets[0]);
                        if (specialTargets.Count == 1 && !targetSpecial.active)
                            specialTargets.Clear();
                    }
                    if (Projectile.Distance(targetSpecial.Center) > 1000f && targetSpecial.active && specialTargets.Count > 1)
                        specialTargets.Remove(specialTargets[0]);
                    if (Projectile.Distance(targetSpecial.Center) > 1000f && targetSpecial.active && specialTargets.Count == 1)
                        specialTargets.Clear();
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

                if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !dash && special == -1) // automode attack ai (only half of original damage)  
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
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newProjectileDamage * 0.5f), 3f, Projectile.owner, FistWhoAmI, TierNumber);
                                Main.projectile[proj].netUpdate = true;
                                Projectile.netUpdate = true;
                            }
                        }
                    }
                }

                if (!dash && special == -1) //return to stand range after ability
                {
                    if (Vector2.Distance(Projectile.Center, player.Center) > range + 20f && !returnToPlayer && !returnToRange)
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

            if (Projectile.Distance(player.Center) >= 1000f && !dash && !returnToPlayer && special == -1 && !remoteMode) //if suddenly stand is too far
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
            if (player.teleporting)
            {
                Projectile.position = player.position;
                dash = false;
            }
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
            {
                shootVel = new Vector2(0f, 1f);
            }
            shootVel.Normalize();
            shootVel *= 0f;
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newProjectileDamage * mPlayer.towerOfGrayDamageMult), 3f, Projectile.owner, FistWhoAmI, TierNumber);
            Main.projectile[proj].netUpdate = true;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (dash || returnToPlayer || returnToRange || special != -1)
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

            writer.Write(special);
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

            special = reader.ReadInt32();
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