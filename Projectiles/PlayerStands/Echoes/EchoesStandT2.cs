using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using JoJoStands.NPCs;
using JoJoStands.Items;
using JoJoStands.Gores.Echoes;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandT2 : StandClass
    {
        public override int PunchDamage => 4;
        public override int PunchTime => 16;
        public override int HalfStandHeight => 30;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 2;
        public override int standYOffset => 40;
        public override float MaxDistance => 148f;      //1.5x the normal range cause Koichi is really reliable guy (C) Proos <3
        public override StandAttackType StandType => StandAttackType.Melee;

        private int ACT = 1;
        private int changeActCooldown = 20;
        private int onlyOneTarget = 0;
        private int targetNPC = -1;
        private int targetPlayer = -1;
        private int rightClickCooldown = 0;

        private float remoteRange = 1200f;

        private bool remoteMode = false;
        private bool mouseControlled = false;
        private bool returnToPlayer = false;
        private bool changeACT = false;
        private bool evolve = false;

        private bool selectEntity = false;
        private bool selectEntity2 = false;
        private int messageCooldown = 0;
        private int messageCooldown2 = 0;

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 1f)
                remoteMode = true;
            if (Projectile.ai[0] == 2f)
                returnToPlayer = true;
            idleFrames = true;
        }
        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (changeActCooldown > 0)
                changeActCooldown--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (remoteMode)
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
            mPlayer.echoesACT = ACT;

            mouseControlled = false;

            Projectile.tileCollide = true;

            Rectangle rectangle = Rectangle.Empty;

            if (Projectile.owner == player.whoAmI)
                rectangle = new Rectangle((int)(Main.MouseWorld.X - 10), (int)(Main.MouseWorld.Y - 10), 20, 20);

            if (mPlayer.usedEctoPearl && remoteRange == 1200f)
                remoteRange *= 1.5f;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (!remoteMode)
                {
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !mPlayer.posing && !returnToPlayer)
                    {
                        Punch();
                    }
                    else
                    {
                        if (player.whoAmI == Main.myPlayer)
                            attackFrames = false;
                    }
                    bool message = false; //select target message
                    bool message2 = false; //target too far message
                    if (Projectile.owner == player.whoAmI)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal)
                            {
                                if (npc.Hitbox.Intersects(rectangle))
                                {
                                    message = true;
                                    if (Vector2.Distance(Projectile.Center, npc.Center) <= 200f)
                                        message2 = true;
                                }
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            for (int p = 0; p < Main.maxPlayers; p++)
                            {
                                Player otherPlayer = Main.player[p];
                                if (otherPlayer.active)
                                {
                                    if (otherPlayer.Hitbox.Intersects(rectangle))
                                    {
                                        message = true;
                                        if (Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 200f)
                                            message2 = true;
                                    }
                                }
                            }
                        }
                    }
                    if (Main.mouseRight && onlyOneTarget == 0 && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && rightClickCooldown == 0 && !returnToPlayer) //right-click ability activation
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal)
                            {
                                if (npc.Hitbox.Intersects(rectangle))
                                {
                                    if (Vector2.Distance(Projectile.Center, npc.Center) > 200f && messageCooldown2 == 0 && !message2)
                                        selectEntity2 = true;
                                    if (Vector2.Distance(Projectile.Center, npc.Center) <= 200f && onlyOneTarget < 1)
                                    {
                                        onlyOneTarget += 1;
                                        targetNPC = npc.whoAmI;
                                        SoundEngine.PlaySound(SoundID.Item4, npc.Center);
                                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                                    }
                                }
                                if (!npc.Hitbox.Intersects(rectangle) && messageCooldown == 0 && !message)
                                    selectEntity = true;
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            for (int p = 0; p < Main.maxPlayers; p++)
                            {
                                Player otherPlayer = Main.player[p];
                                if (otherPlayer.active)
                                {
                                    if (otherPlayer.Hitbox.Intersects(rectangle))
                                    {
                                        if (Vector2.Distance(Projectile.Center, otherPlayer.Center) > 200f && messageCooldown2 == 0 && !message2)
                                            selectEntity2 = true;
                                        if (Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 200f && onlyOneTarget < 1 && otherPlayer.whoAmI != player.whoAmI)
                                        {
                                            onlyOneTarget += 1;
                                            targetPlayer = otherPlayer.whoAmI;
                                            SoundEngine.PlaySound(SoundID.Item4, otherPlayer.Center);
                                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                                        }
                                    }
                                    if (!otherPlayer.Hitbox.Intersects(rectangle) && messageCooldown == 0 && !message)
                                        selectEntity = true;
                                }
                            }
                        }
                        rightClickCooldown += 90;
                    }
                    if (!attackFrames && !returnToPlayer)
                        StayBehind();
                }

                if (onlyOneTarget > 0) //right-click ability effect
                {
                    if (targetNPC != -1)
                    {
                        if (!Main.npc[targetNPC].townNPC)
                        {
                            Main.npc[targetNPC].AddBuff(ModContent.BuffType<SMACK>(), 900);
                            Main.npc[targetNPC].GetGlobalNPC<JoJoGlobalNPC>().echoesDebuffOwner = player.whoAmI;
                            onlyOneTarget = 0;
                        }
                        else
                        {
                            Main.npc[targetNPC].AddBuff(ModContent.BuffType<BelieveInMe>(), 1800);
                            onlyOneTarget = 0;
                        }
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                    }
                    if (targetPlayer != -1 && onlyOneTarget != 0)
                    {
                        if (Main.player[targetPlayer].hostile && player.hostile && player.InOpposingTeam(Main.player[targetPlayer]))
                        {
                            Main.player[targetPlayer].AddBuff(ModContent.BuffType<SMACK>(), 360);
                            SyncCall.SyncOtherPlayerDebuff(player.whoAmI, targetPlayer, ModContent.BuffType<SMACK>(), 360);
                            onlyOneTarget = 0;
                        }
                        if (!Main.player[targetPlayer].hostile || !player.hostile || Main.player[targetPlayer].hostile && player.hostile && !player.InOpposingTeam(Main.player[targetPlayer]))
                        {
                            Main.player[targetPlayer].AddBuff(ModContent.BuffType<BelieveInMe>(), 720);
                            SyncCall.SyncOtherPlayerDebuff(player.whoAmI, targetPlayer, ModContent.BuffType<BelieveInMe>(), 720);
                            onlyOneTarget = 0;
                        }
                    }
                }

                if (onlyOneTarget == 0)
                {
                    targetPlayer = -1;
                    targetNPC = -1;
                }

                if (selectEntity2)
                {
                    Main.NewText("Target too far");
                    selectEntity2 = false;
                    messageCooldown2 += 90;
                }
                if (selectEntity)
                {
                    Main.NewText("Select target with mouse");
                    selectEntity = false;
                    messageCooldown += 90;
                }
                if (messageCooldown > 0)
                    messageCooldown--;
                if (messageCooldown2 > 0)
                    messageCooldown2--;

                if (rightClickCooldown > 0)
                    rightClickCooldown--;

                if (SpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer && !returnToPlayer) //remote mode
                {
                    remoteMode = !remoteMode;
                    if (remoteMode)
                        Main.NewText("Remote Mode: Active");
                    else
                        Main.NewText("Remote Mode: Disabled");
                }

                if (SecondSpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer && mPlayer.echoesTier > 2 && changeActCooldown == 0)
                {
                    changeACT = true;
                    Projectile.Kill();
                }

                if (Vector2.Distance(player.Center, Projectile.Center) <= remoteRange * 0.9f)
                {
                    if (remoteMode || returnToPlayer)
                    {
                        if (Projectile.velocity.X > 0)
                            Projectile.spriteDirection = 1;
                        if (Projectile.velocity.X < 0)
                            Projectile.spriteDirection = -1;
                    }
                }
                if (remoteMode && !returnToPlayer)
                {
                    float distance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
                    float halfScreenWidth = (float)Main.screenWidth / 2f;
                    float halfScreenHeight = (float)Main.screenHeight / 2f;
                    mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                    Projectile.rotation = Projectile.velocity.X * 0.05f;
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !mPlayer.posing)
                    {
                        if (distance > 25f)
                            MovementAI(Main.MouseWorld, 8f + player.moveSpeed);
                        if (distance <= 25f)
                            MovementAI(Main.MouseWorld, (distance * (8f + player.moveSpeed)) / 25);
                        mouseControlled = true;
                    }
                    if (Main.mouseRight && Projectile.owner == Main.myPlayer && !mPlayer.posing)
                    {
                        attackFrames = true;
                        PlayPunchSound();
                        if (shootCount <= 0)
                        {
                            shootCount += newPunchTime;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                            Main.projectile[proj].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                    {
                        attackFrames = false;
                        idleFrames = true;
                    }
                    if (!mouseControlled)
                        MovementAI(Projectile.Center + new Vector2(100f * Projectile.spriteDirection, 0f), 0f);

                    LimitDistance(remoteRange);
                }
            }
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !returnToPlayer) //automode
            {
                remoteMode = false;
                BasicPunchAI();
            }

            if (Projectile.Distance(player.Center) >= newMaxDistance + 10f && !returnToPlayer && !remoteMode) //if suddenly stand is too far
                returnToPlayer = true;
            if (Projectile.Distance(player.Center) <= 20f)
                returnToPlayer = false;

            if (returnToPlayer)
            {
                Projectile.tileCollide = false;
                MovementAI(player.Center, 8f + player.moveSpeed * 2);
            }
            if (player.HasBuff(ModContent.BuffType<StrongWill>()) && mPlayer.echoesTier == 2 && Main.hardMode && mPlayer.echoesACT2Evolve >= 10000)
            {
                evolve = true;
                Projectile.Kill();
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

        public override void SelectAnimation()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (mPlayer.posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();

            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Echoes", "/EchoesACT1_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(ACT);
            writer.Write(onlyOneTarget);
            writer.Write(targetNPC);
            writer.Write(targetPlayer);
            writer.Write(rightClickCooldown);

            writer.Write(remoteRange);

            writer.Write(remoteMode);
            writer.Write(mouseControlled);
            writer.Write(returnToPlayer);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            ACT = reader.ReadInt32();
            onlyOneTarget = reader.ReadInt32();
            targetNPC = reader.ReadInt32();
            targetPlayer = reader.ReadInt32();
            rightClickCooldown = reader.ReadInt32();

            remoteRange = reader.ReadSingle();

            remoteMode = reader.ReadBoolean();
            mouseControlled = reader.ReadBoolean();
            returnToPlayer = reader.ReadBoolean();
        }

        public override void StandKillEffects()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            float remoteModeOnSpawn = 0f;
            if (remoteMode)
                remoteModeOnSpawn = 1f;
            if (!remoteMode && Projectile.Distance(player.Center) >= newMaxDistance + 10f)
                remoteModeOnSpawn = 2f;
            if (changeACT)
            {
                player.maxMinions += 1;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT3>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
            }
            if (evolve)
            {
                player.maxMinions += 1;
                mPlayer.echoesACT2Evolve = 0;
                mPlayer.StandSlot.SlotItem.type = ModContent.ItemType<EchoesACT2>();
                mPlayer.StandSlot.SlotItem.SetDefaults(ModContent.ItemType<EchoesACT2>());
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT3>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
                Main.NewText("Oh? Echoes is evolving!");
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT1_Gore_1>(), 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT1_Gore_2>(), 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
    }
}