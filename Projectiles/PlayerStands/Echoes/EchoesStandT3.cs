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
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandT3 : StandClass
    {
        public override int PunchDamage => 44;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 26;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 3;
        public override float MaxDistance => 148f;      //1.5x the normal range cause Koichi is really reliable guy (C) Proos <3
        public override int StandOffset => 20;
        public override int standYOffset => 30;
        public override StandAttackType StandType => StandAttackType.Melee;

        private int ACT = 2;
        private int changeActCooldown = 20;
        private int shoot = 0;
        private int holdSpecial = 0;
        private int echoesTailTipType = 1;

        private float remoteRange = 1200f;

        private bool remoteMode = false;
        private bool mouseControlled = false;
        private bool returnToPlayer = false;
        private bool returnTail = false;
        private bool changeACT = false;
        private bool evolve = false;

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
            if (shoot > 0)
                shoot--;
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

            if (mPlayer.usedEctoPearl && remoteRange == 1200f)
                remoteRange *= 1.5f;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (!remoteMode)
                {
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !mPlayer.posing && !returnToPlayer && !returnTail && shoot == 0)
                    {
                        Punch();
                    }
                    else
                    {
                        if (player.whoAmI == Main.myPlayer)
                            attackFrames = false;
                    }
                    if (Main.mouseRight && Projectile.owner == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<EchoesTailTip>()] == 0 && !returnToPlayer && shoot == 0 && !returnTail && shootCount == 0) //right-click ability 
                        holdSpecial++;
                    if (!Main.mouseRight && Projectile.owner == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<EchoesTailTip>()] == 0 && !returnToPlayer && shoot == 0 && !returnTail && holdSpecial > 0 && holdSpecial < 60 && shootCount == 0)
                    {
                        holdSpecial = 0;
                        if (echoesTailTipType == 1 && shootCount == 0)
                        {
                            echoesTailTipType = 2;
                            Main.NewText("KABOOM", Color.DeepSkyBlue);
                            shootCount = 30;
                        }
                        if (echoesTailTipType == 2 && shootCount == 0)
                        {
                            echoesTailTipType = 3;
                            Main.NewText("WOOOSH", Color.LightSkyBlue);
                            shootCount = 30;
                        }
                        if (echoesTailTipType == 3 && shootCount == 0)
                        {
                            echoesTailTipType = 4;
                            Main.NewText("SIZZLE", Color.IndianRed);
                            shootCount = 30;
                        }
                        if (echoesTailTipType == 4 && shootCount == 0)
                        {
                            echoesTailTipType = 1;
                            Main.NewText("BOING", Color.HotPink);
                            shootCount = 30;
                        }
                    }
                    if (holdSpecial >= 60)
                    {
                        holdSpecial = 0;
                        Projectile.frame = 0;
                        shoot += 60;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);
                        shootVel.Normalize();
                        shootVel *= 8f;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<EchoesTailTip>(), (int)(AltDamage * mPlayer.standDamageBoosts), 6f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier = mPlayer.echoesTier;
                        Main.projectile[proj].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType = echoesTailTipType;
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                    if (!attackFrames && !returnToPlayer && !returnTail)
                        StayBehind();
                }

                if (mPlayer.echoesTailTip != -1 && shoot == 0)
                {
                    if (Main.mouseRight && Projectile.owner == Main.myPlayer && !remoteMode && !returnToPlayer && !returnTail)
                    {
                        if (Main.projectile[mPlayer.echoesTailTip].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage != 2)
                        {
                            shootCount = 30;
                            Main.projectile[mPlayer.echoesTailTip].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage = 2;
                        }
                        if (Main.projectile[mPlayer.echoesTailTip].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 2 && shootCount == 0 && Vector2.Distance(Projectile.Center, Main.projectile[mPlayer.echoesTailTip].Center) <= 1200f)
                        {
                            shootCount = 30;
                            returnTail = true;
                        }
                        if (Main.projectile[mPlayer.echoesTailTip].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 2 && shootCount == 0 && Vector2.Distance(Projectile.Center, Main.projectile[mPlayer.echoesTailTip].Center) > 1200f)
                        {
                            shootCount = 30;
                            Main.NewText("Out of reach");
                        }
                    }
                }

                if (SpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer && !returnToPlayer && !returnTail && shoot == 0) //remote mode
                {
                    remoteMode = !remoteMode;
                    if (remoteMode)
                        Main.NewText("Remote Mode: Active");
                    else
                        Main.NewText("Remote Mode: Disabled");
                }

                if (SecondSpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer && changeActCooldown == 0 && mPlayer.echoesTier > 2 && !evolve)
                {
                    changeACT = true;
                    Projectile.Kill();
                }

                if (remoteMode && !returnToPlayer && !returnTail)
                {
                    float distance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
                    float halfScreenWidth = (float)Main.screenWidth / 2f;
                    float halfScreenHeight = (float)Main.screenHeight / 2f;
                    mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                    holdSpecial = 0;
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

            if (Vector2.Distance(player.Center, Projectile.Center) <= remoteRange * 0.9f)
            {
                if (remoteMode || returnToPlayer || returnTail)
                {
                    if (Projectile.velocity.X > 0)
                        Projectile.spriteDirection = 1;
                    if (Projectile.velocity.X < 0)
                        Projectile.spriteDirection = -1;
                }
            }

            if (mPlayer.echoesTailTip != -1)
            {
                if (returnTail)
                    MovementAI(Main.projectile[mPlayer.echoesTailTip].Center, 8f);
                if (Main.projectile[mPlayer.echoesTailTip].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 2 && Vector2.Distance(Projectile.Center, Main.projectile[mPlayer.echoesTailTip].Center) <= 20f)
                    Main.projectile[mPlayer.echoesTailTip].Kill();
            }

            if (mPlayer.echoesTailTip == -1 && returnTail)
            {
                returnTail = false;
                returnToPlayer = true;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !returnToPlayer && !returnTail) //automode
            {
                holdSpecial = 0;
                remoteMode = false;
                BasicPunchAI();
            }

            if (Projectile.Distance(player.Center) >= newMaxDistance + 10f && !returnToPlayer && !remoteMode && !returnTail) //if suddenly stand is too far
                returnToPlayer = true;
            if (Projectile.Distance(player.Center) <= 20f)
                returnToPlayer = false;

            if (returnToPlayer)
            {
                holdSpecial = 0;
                Projectile.tileCollide = false;
                MovementAI(player.Center, 8f + player.moveSpeed * 2);
            }

            if (returnTail)
            {
                Projectile.tileCollide = false;
                holdSpecial = 0;
            }

            if (player.teleporting)
            {
                holdSpecial = 0;
                Projectile.position = player.position;
                returnTail = false;
            }

            if (player.HasBuff(ModContent.BuffType<StrongWill>()) && mPlayer.echoesTier == 3 && mPlayer.echoesACT3Evolve >= 20000 && Main.hardMode && mPlayer.echoesTailTip != -1)
            {
                evolve = true;
                Projectile.Kill();
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
            if (shoot > 0)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Shoot");
            }

        }

        public override void PlayAnimation(string animationName)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            string pathAddition = "";
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<EchoesTailTip>()] > 0)
                pathAddition = "Tailless_";
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Echoes", "/EchoesACT2_" + pathAddition + animationName);

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
            if (animationName == "Shoot")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(ACT);
            writer.Write(shoot);

            writer.Write(remoteRange);

            writer.Write(remoteMode);
            writer.Write(mouseControlled);
            writer.Write(returnToPlayer);
            writer.Write(returnTail);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            ACT = reader.ReadInt32();
            shoot = reader.ReadInt32();

            remoteRange = reader.ReadSingle();

            remoteMode = reader.ReadBoolean();
            mouseControlled = reader.ReadBoolean();
            returnToPlayer = reader.ReadBoolean();
            returnTail = reader.ReadBoolean();
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
                if (mPlayer.echoesTier == 3)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT2>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
                if (mPlayer.echoesTier == 4)
                {
                    if (remoteModeOnSpawn == 1f)
                    {
                        if (Projectile.Distance(player.Center) >= newMaxDistance + 10f)
                            remoteModeOnSpawn = 2f;
                        else
                            remoteModeOnSpawn = 0f;
                    }
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandFinal>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
                }
            }
            if (evolve)
            {
                player.maxMinions += 1;
                mPlayer.echoesACT3Evolve = 0;
                mPlayer.StandSlot.SlotItem.type = ModContent.ItemType<EchoesACT3>();
                mPlayer.StandSlot.SlotItem.SetDefaults(ModContent.ItemType<EchoesACT3>());
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandFinal>(), 0, 0f, Main.myPlayer, 2f);
                Main.NewText("Oh? Echoes is evolving!");
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT2_Gore>(), 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
    }
}