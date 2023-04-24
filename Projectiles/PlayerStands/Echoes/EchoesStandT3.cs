using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Gores.Echoes;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandT3 : StandClass
    {
        public override int PunchDamage => 44;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 26;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 3;
        public override string PoseSoundName => "EchoesAct2";
        public override string SpawnSoundName => "Echoes Act 2";
        public override float MaxDistance => 148f;      //1.5x the normal range cause Koichi is really reliable guy (C) Proos <3
        public override Vector2 StandOffset => new Vector2(10, 0);
        public override Vector2 ManualIdleHoverOffset => new Vector2(0, -30);
        public override StandAttackType StandType => StandAttackType.Melee;

        private const int ActNumber = 2;
        private const float ManualRange = 25 * 16;
        private const float RemoteRange = 75 * 16;

        private int tailUseTimer = 0;
        private int holdSpecial = 0;
        private int echoesTailTipType = 1;
        private int actChangeCooldown = 30;

        public const int Effect_Boing = 1;
        public const int Effect_Kabooom = 2;
        public const int Effect_Wooosh = 3;
        public const int Effect_Sizzle = 4;
        private readonly string[] EffectNames = new string[4] { "BOING", "KABOOOM", "WOOOSH", "SIZZLE" };
        private readonly Color[] EffectColors = new Color[4] { Color.HotPink, Color.Magenta, Color.LightSkyBlue, Color.IndianRed };

        private bool remoteMode = false;
        private bool mouseControlled = false;
        private bool returnToPlayer = false;
        private bool returnTail = false;
        private bool changeACT = false;
        private bool evolve = false;

        public override void ExtraSpawnEffects()
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
            if (tailUseTimer > 0)
                tailUseTimer--;
            if (shootCount > 0)
                shootCount--;
            if (actChangeCooldown > 0)
                actChangeCooldown--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (remoteMode)
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
            mPlayer.currentEchoesAct = ActNumber;

            mouseControlled = false;
            Projectile.tileCollide = true;
            float controlRange = ManualRange;
            if (remoteMode)
                controlRange = RemoteRange;
            if (mPlayer.usedEctoPearl)
                controlRange *= 1.5f;
            controlRange += mPlayer.standRangeBoosts;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !mPlayer.posing && !returnToPlayer && !returnTail && tailUseTimer == 0)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Projectile.owner == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<EchoesTailTip>()] == 0 && !returnToPlayer && tailUseTimer == 0 && !returnTail && shootCount <= 0)
                {
                    if (Main.mouseRight) //right-click ability 
                        holdSpecial++;
                    else if (Projectile.owner == Main.myPlayer && holdSpecial > 0 && holdSpecial < 60)
                    {
                        holdSpecial = 0;
                        shootCount = 30;
                        echoesTailTipType++;
                        if (echoesTailTipType >= 5)
                            echoesTailTipType = 1;
                        Main.NewText(EffectNames[echoesTailTipType - 1], EffectColors[echoesTailTipType - 1]);

                    }
                }
                if (holdSpecial >= 60)
                {
                    holdSpecial = 0;
                    Projectile.frame = 0;
                    tailUseTimer += 60;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 8f;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<EchoesTailTip>(), (int)(AltDamage * mPlayer.standDamageBoosts), 6f, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[projIndex].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier = mPlayer.echoesTier;
                    Main.projectile[projIndex].GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType = echoesTailTipType;
                    Main.projectile[projIndex].netUpdate = true;
                    Projectile.netUpdate = true;
                }
                if (!attackFrames && !returnToPlayer && !returnTail)
                    StayBehind();

                if (mPlayer.echoesTailTip != -1 && tailUseTimer == 0)
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
                            Main.NewText("The tip is out of reach!");
                        }
                    }
                }

                if (SpecialKeyPressed(false) && Projectile.owner == Main.myPlayer && !returnToPlayer && !returnTail && tailUseTimer == 0) //remote mode
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

                if (SecondSpecialKeyPressed(false) && mPlayer.echoesTier >= 3 && actChangeCooldown <= 0 && !evolve && Projectile.owner == Main.myPlayer)
                {
                    changeACT = true;
                    Projectile.Kill();
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote && !returnToPlayer && !returnTail)
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
                    else
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
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI, TierNumber);
                        Main.projectile[projIndex].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }

                if (mPlayer.echoesTailTip != -1 && tailUseTimer == 0)
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
                            Main.NewText("The tip is out of reach!");
                        }
                    }
                }

                if (SpecialKeyPressed(false) && Projectile.owner == Main.myPlayer && !returnToPlayer && !returnTail && tailUseTimer == 0) //remote mode
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
                if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    attackFrames = false;
                    idleFrames = true;
                }
                if (!mouseControlled)
                    MovementAI(Projectile.Center + new Vector2(100f * Projectile.spriteDirection, 0f), 0f);

                LimitDistance(controlRange);
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto && !returnToPlayer && !returnTail)        //Automode
            {
                holdSpecial = 0;
                remoteMode = false;
                BasicPunchAI();
            }

            if (Vector2.Distance(player.Center, Projectile.Center) <= controlRange * 0.9f)
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

            if (player.HasBuff(ModContent.BuffType<StrongWill>()) && mPlayer.echoesTier == 3 && mPlayer.echoesACT3EvolutionProgress >= 20000 && Main.hardMode && mPlayer.echoesTailTip != -1)
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
            if (tailUseTimer > 0)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Shoot");
            }

        }

        public override void PlayAnimation(string animationName)
        {
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
            writer.Write(tailUseTimer);
            writer.Write(remoteMode);
            writer.Write(returnToPlayer);
            writer.Write(returnTail);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            tailUseTimer = reader.ReadInt32();
            remoteMode = reader.ReadBoolean();
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
                else if (mPlayer.echoesTier == 3)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT2>(), 0, 0f, Main.myPlayer, remoteModeOnSpawn);
            }
            if (evolve)
            {
                player.maxMinions += 1;
                mPlayer.echoesACT3EvolutionProgress = 0;
                mPlayer.StandSlot.SlotItem.type = ModContent.ItemType<EchoesAct3>();
                mPlayer.StandSlot.SlotItem.SetDefaults(ModContent.ItemType<EchoesAct3>());
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandFinal>(), 0, 0f, Main.myPlayer, 2f);
                Main.NewText("Oh? Echoes is evolving!");
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT2_Gore>(), 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
    }
}