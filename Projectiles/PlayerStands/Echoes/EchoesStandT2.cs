using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Gores.Echoes;
using JoJoStands.Items;
using JoJoStands.Networking;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandT2 : StandClass
    {
        public override int PunchDamage => 4;
        public override int PunchTime => 16;
        public override int HalfStandHeight => 30;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 2;
        public override string PoseSoundName => "EchoesAct1";
        public override string SpawnSoundName => "Echoes Act 1";
        public override Vector2 StandOffset => new Vector2(-8, 0);
        public override Vector2 ManualIdleHoverOffset => new Vector2(-28, -40);
        public override float MaxDistance => 148f;      //1.5x the normal range cause Koichi is really reliable guy (C) Proos <3
        public override StandAttackType StandType => StandAttackType.Melee;
        private const float ManualRange = 40 * 16;
        private const float RemoteRange = 75 * 16;

        private int actNumber = 1;
        private int targetNPC = -1;
        private int targetPlayer = -1;
        private int rightClickCooldown = 0;
        private int actChangeCooldown = 30;

        private bool remoteMode = false;
        private bool mouseControlled = false;
        private bool returnToPlayer = false;
        private bool changeACT = false;
        private bool evolve = false;
        private bool targetFound;

        private int messageCooldown = 0;

        public override void ExtraSpawnEffects()
        {
            if (Projectile.ai[0] == 1f)
                remoteMode = true;
            if (Projectile.ai[0] == 2f)
                returnToPlayer = true;
            currentAnimationState = AnimationState.Idle;
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (messageCooldown > 0)
                messageCooldown--;
            if (rightClickCooldown > 0)
                rightClickCooldown--;
            if (actChangeCooldown > 0)
                actChangeCooldown--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (remoteMode)
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
            mPlayer.currentEchoesAct = actNumber;

            mouseControlled = false;
            Projectile.tileCollide = true;
            Rectangle mouseRect = Rectangle.Empty;
            if (Projectile.owner == player.whoAmI)
                mouseRect = new Rectangle((int)(Main.MouseWorld.X - 10), (int)(Main.MouseWorld.Y - 10), 20, 20);

            float controlRange = ManualRange;
            if (remoteMode)
                controlRange = RemoteRange;
            if (mPlayer.usedEctoPearl)
                controlRange *= 1.5f;
            controlRange += mPlayer.standRangeBoosts;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !mPlayer.posing && !returnToPlayer)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && rightClickCooldown <= 0 && !targetFound && !returnToPlayer) //right-click ability activation
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal)
                        {
                            if (npc.Hitbox.Intersects(mouseRect))
                            {
                                if (Vector2.Distance(Projectile.Center, npc.Center) <= 200f)
                                {
                                    if (!targetFound)
                                    {
                                        targetFound = true;
                                        targetNPC = npc.whoAmI;
                                        SoundEngine.PlaySound(SoundID.Item4, npc.Center);
                                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                                    }
                                }
                                else
                                {
                                    messageCooldown += 90;
                                    Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.CrazyDiamondTargetOOR").Value);
                                }
                            }
                            else
                            {
                                if (messageCooldown <= 0)
                                {
                                    messageCooldown += 90;
                                    Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.EchoesMouseHint").Value);
                                }
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
                                if (otherPlayer.Hitbox.Intersects(mouseRect))
                                {
                                    if (Vector2.Distance(Projectile.Center, otherPlayer.Center) <= 200f)
                                    {
                                        if (!targetFound && otherPlayer.whoAmI != player.whoAmI)
                                        {
                                            targetFound = true;
                                            targetPlayer = otherPlayer.whoAmI;
                                            SoundEngine.PlaySound(SoundID.Item4, otherPlayer.Center);
                                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                                        }
                                    }
                                    else
                                    {
                                        messageCooldown += 90;
                                        Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.CrazyDiamondTargetOOR").Value);
                                    }
                                }
                                else
                                {
                                    if (messageCooldown <= 0)
                                    {
                                        messageCooldown += 90;
                                        Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.EchoesMouseHint").Value);
                                    }
                                }
                            }
                        }
                    }
                    rightClickCooldown += 90;
                }
                if (!attacking && !returnToPlayer)
                    StayBehind();
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                if (!returnToPlayer)
                {
                    float distance = Vector2.Distance(Projectile.Center, Main.MouseWorld);
                    float halfScreenWidth = (float)Main.screenWidth / 2f;
                    float halfScreenHeight = (float)Main.screenHeight / 2f;
                    mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                    Projectile.rotation = Projectile.velocity.X * 0.05f;
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !mPlayer.posing)
                    {
                        if (distance <= 25f)
                            MovementAI(Main.MouseWorld, (distance * (8f + player.moveSpeed)) / 25);
                        else
                            MovementAI(Main.MouseWorld, 8f + player.moveSpeed);
                        mouseControlled = true;
                    }
                    if (Main.mouseRight && Projectile.owner == Main.myPlayer && !mPlayer.posing)
                    {
                        currentAnimationState = AnimationState.Attack;
                        PlayPunchSound();
                        if (shootCount <= 0)
                        {
                            shootCount += newPunchTime;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                    if (!mouseControlled)
                        MovementAI(Projectile.Center + new Vector2(100f * Projectile.spriteDirection, 0f), 0f);

                    LimitDistance(RemoteRange);
                }
            }
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !returnToPlayer) //automode
            {
                remoteMode = false;
                BasicPunchAI();
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual || mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                if (targetFound)      //right-click ability effect
                {
                    targetFound = false;
                    if (targetNPC != -1)
                    {
                        if (!Main.npc[targetNPC].townNPC)
                        {
                            Main.npc[targetNPC].AddBuff(ModContent.BuffType<SMACK>(), 900);
                            Main.npc[targetNPC].GetGlobalNPC<JoJoGlobalNPC>().echoesDebuffOwner = player.whoAmI;
                        }
                        else
                            Main.npc[targetNPC].AddBuff(ModContent.BuffType<BelieveInMe>(), 1800);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                    }
                    else
                    {
                        if (Main.player[targetPlayer].hostile && player.hostile && player.InOpposingTeam(Main.player[targetPlayer]))
                        {
                            Main.player[targetPlayer].AddBuff(ModContent.BuffType<SMACK>(), 360);
                            SyncCall.SyncOtherPlayerDebuff(player.whoAmI, targetPlayer, ModContent.BuffType<SMACK>(), 360);
                        }
                        else if (!Main.player[targetPlayer].hostile || !player.hostile || Main.player[targetPlayer].hostile && player.hostile && !player.InOpposingTeam(Main.player[targetPlayer]))
                        {
                            Main.player[targetPlayer].AddBuff(ModContent.BuffType<BelieveInMe>(), 720);
                            SyncCall.SyncOtherPlayerDebuff(player.whoAmI, targetPlayer, ModContent.BuffType<BelieveInMe>(), 720);
                        }
                    }
                    targetPlayer = -1;
                    targetNPC = -1;
                }

                if (SpecialKeyPressed(false) && !returnToPlayer && Projectile.owner == Main.myPlayer) //remote mode
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

                if (SecondSpecialKeyPressed(false) && mPlayer.echoesTier >= 3 && actChangeCooldown <= 0 && Projectile.owner == Main.myPlayer)
                {
                    changeACT = true;
                    Projectile.Kill();
                }

                if (Vector2.Distance(player.Center, Projectile.Center) <= controlRange * 0.9f)
                {
                    if (remoteMode || returnToPlayer)
                    {
                        if (Projectile.velocity.X > 0)
                            Projectile.spriteDirection = 1;
                        if (Projectile.velocity.X < 0)
                            Projectile.spriteDirection = -1;
                    }
                }
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
            if (player.HasBuff(ModContent.BuffType<StrongWill>()) && mPlayer.echoesTier == 2 && Main.hardMode && mPlayer.echoesACT2EvolutionProgress >= 10000)
            {
                evolve = true;
                Projectile.Kill();
            }
            if (player.teleporting)
                Projectile.position = player.position;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
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
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Echoes", "EchoesACT1_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(actNumber);
            writer.Write(targetNPC);
            writer.Write(targetPlayer);
            writer.Write(rightClickCooldown);

            writer.Write(RemoteRange);

            writer.Write(remoteMode);
            writer.Write(mouseControlled);
            writer.Write(returnToPlayer);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            actNumber = reader.ReadInt32();
            targetNPC = reader.ReadInt32();
            targetPlayer = reader.ReadInt32();
            rightClickCooldown = reader.ReadInt32();

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
                mPlayer.echoesACT2EvolutionProgress = 0;
                mPlayer.StandSlot.SlotItem.type = ModContent.ItemType<EchoesAct2>();
                mPlayer.StandSlot.SlotItem.SetDefaults(ModContent.ItemType<EchoesAct2>());
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT3>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
                Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.EchoesEvolve").Value);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT1_Gore_1>(), 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT1_Gore_2>(), 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
    }
}