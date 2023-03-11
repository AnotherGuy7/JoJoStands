using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueenBTD
{
    public class KillerQueenBTDStand : StandClass
    {
        public override float ProjectileSpeed => 4f;
        public override int ShootTime => 30;
        public override int HalfStandHeight => 37;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override Vector2 StandOffset => new Vector2(-5, 0);
        public override string PoseSoundName => "IWouldntLose";
        public override string SpawnSoundName => "Killer Queen";
        //public override bool CanUseSaladDye => true;
        public override bool CanUseRangeIndicators => false;
        private static readonly SoundStyle BtdWarpSoundEffect = new SoundStyle("JoJoStands/Sounds/GameSounds/BiteTheDustEffect")
        {
            Volume = JoJoStands.ModSoundsVolume
        };
        private static readonly SoundStyle BtdSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/BiteTheDust")
        {
            Volume = JoJoStands.ModSoundsVolume
        };
        private readonly SoundStyle kqClickSound = new SoundStyle("JoJoStands/Sounds/GameSounds/KQButtonClick");


        private int btdStartDelay = 0;
        private int bubbleDamage = 684;      //not using projectileDamage cause this one changes

        private int btdPositionSaveTimer = 0;
        private int btdPositionIndex = 0;
        private int btdRevertTimer = 0;
        private int btdRevertTime = 0;
        private int amountOfSavedData = 0;
        private int currentRewindTime = 0;
        private int totalRewindTime = 0;
        private List<Vector2> btdPlayerPositions;       //Positions of KQ:BTD's owner.
        private List<Vector2> btdPlayerVelocities;
        private bool bitesTheDustActive;
        private bool bitesTheDustActivated;
        private PlayerData[] savedPlayerDatas;
        private WorldData savedWorldData;
        private bool saveDataCreated = false;       //For use with all clients that aren't 

        public struct PlayerData
        {
            public bool active;
            public Vector2 playerBTDPos;
            public Item[] playerBTDInventory;
            public int playerBTDHealth;
            public int playerDirection;
            public int[] buffTypes;
            public int[] buffTimes;
            public byte whoAmI;
        }

        public struct WorldData
        {
            public double worldTime;
            public NPCData[] npcData;
        }

        public struct NPCData
        {
            public int type;
            public Vector2 position;
            public Vector2 velocity;
            public int health;
            public int direction;
            public float[] ai;
            public bool active;
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            bubbleDamage = Main.dayTime ? 684 : 456;
            DrawOriginOffsetY = -HalfStandHeight;
            int newBubbleDamage = (int)(bubbleDamage * mPlayer.standDamageBoosts);

            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            bitesTheDustActive = player.HasBuff(ModContent.BuffType<BitesTheDust>());
            if (!bitesTheDustActive && saveDataCreated)
                saveDataCreated = false;
            if (Main.netMode != NetmodeID.SinglePlayer && Main.myPlayer != Projectile.owner)
            {
                Player otherPlayer = Main.player[Main.myPlayer];
                if (otherPlayer.GetModPlayer<MyPlayer>().bitesTheDustActive && !bitesTheDustActivated)
                {
                    bitesTheDustActivated = true;
                    totalRewindTime = CalculateRewindTime();
                    if (JoJoStands.SoundsLoaded)
                        SoundEngine.PlaySound(BtdSound, Projectile.Center);
                }
            }

            if ((SpecialKeyPressed() && !bitesTheDustActive) || (bitesTheDustActive && !saveDataCreated && !playerHasAbilityCooldown))
            {
                btdPositionIndex = 0;
                amountOfSavedData = 0;
                btdPlayerPositions = new List<Vector2>() { player.position };
                btdPlayerVelocities = new List<Vector2>() { player.velocity };
                savedPlayerDatas = new PlayerData[Main.maxPlayers];
                currentRewindTime = 0;
                totalRewindTime = 0;
                btdRevertTime = 35;

                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    savedPlayerDatas[0] = new PlayerData();
                    savedPlayerDatas[0].playerBTDHealth = player.statLife;
                    savedPlayerDatas[0].playerBTDInventory = player.inventory.Clone() as Item[];
                    savedPlayerDatas[0].playerBTDPos = player.position;
                    savedPlayerDatas[0].playerDirection = player.direction;

                    int amountOfBuffs = player.CountBuffs();
                    savedPlayerDatas[0].buffTypes = new int[amountOfBuffs];
                    savedPlayerDatas[0].buffTimes = new int[amountOfBuffs];
                    for (int i = 0; i < amountOfBuffs; i++)
                    {
                        savedPlayerDatas[0].buffTypes[i] = player.buffType[i];
                        savedPlayerDatas[0].buffTimes[i] = player.buffTime[i];
                    }
                }
                else
                {
                    int activeIndex = 0;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active)
                        {
                            savedPlayerDatas[activeIndex].active = true;
                            savedPlayerDatas[activeIndex] = new PlayerData();
                            savedPlayerDatas[activeIndex].playerBTDHealth = otherPlayer.statLife;
                            if (p == Main.myPlayer)
                                savedPlayerDatas[activeIndex].playerBTDInventory = otherPlayer.inventory.Clone() as Item[];
                            savedPlayerDatas[activeIndex].playerBTDPos = otherPlayer.position;
                            savedPlayerDatas[activeIndex].playerDirection = otherPlayer.direction;

                            int amountOfBuffs = otherPlayer.CountBuffs();
                            savedPlayerDatas[activeIndex].buffTypes = new int[amountOfBuffs];
                            savedPlayerDatas[activeIndex].buffTimes = new int[amountOfBuffs];
                            for (int i = 0; i < amountOfBuffs; i++)
                            {
                                savedPlayerDatas[activeIndex].buffTypes[i] = otherPlayer.buffType[i];
                                savedPlayerDatas[activeIndex].buffTimes[i] = otherPlayer.buffTime[i];
                            }
                            savedPlayerDatas[activeIndex].whoAmI = (byte)p;
                            activeIndex++;
                        }
                    }
                }

                player.AddBuff(ModContent.BuffType<BitesTheDust>(), 5 * 60 * 60);      //So it doesn't save
                mPlayer.standChangingLocked = true;

                savedWorldData = new WorldData();
                savedWorldData.worldTime = Main.time;
                savedWorldData.npcData = new NPCData[Main.maxNPCs];
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    savedWorldData.npcData[i] = new NPCData();
                    if (npc.active)
                    {
                        savedWorldData.npcData[i].type = npc.type;
                        savedWorldData.npcData[i].position = npc.position;
                        savedWorldData.npcData[i].velocity = npc.velocity;
                        savedWorldData.npcData[i].health = npc.life;
                        savedWorldData.npcData[i].direction = npc.direction;
                        savedWorldData.npcData[i].ai = npc.ai;
                        savedWorldData.npcData[i].active = true;
                    }
                }
                Projectile.netUpdate = true;
                saveDataCreated = true;
            }

            if (SpecialKeyPressed() && bitesTheDustActive && btdStartDelay <= 0)
            {
                if (!JoJoStands.SoundsLoaded)
                {
                    bitesTheDustActivated = true;
                    totalRewindTime = CalculateRewindTime();
                    SoundEngine.PlaySound(BtdWarpSoundEffect);
                    SyncCall.SyncBitesTheDust(player.whoAmI, true);
                }
                else
                    btdStartDelay = 205;
                Projectile.netUpdate = true;
            }
            if (JoJoStands.SoundsLoaded && !bitesTheDustActivated && btdStartDelay > 0)
            {
                btdStartDelay--;
                if (btdStartDelay <= 0)
                {
                    bitesTheDustActivated = true;
                    totalRewindTime = CalculateRewindTime();
                    SoundEngine.PlaySound(BtdSound, Projectile.Center);
                    SyncCall.SyncBitesTheDust(player.whoAmI, true);
                    Projectile.netUpdate = true;
                }
            }
            if (bitesTheDustActive && !bitesTheDustActivated && saveDataCreated)       //Records
            {
                btdPositionSaveTimer++;
                if (btdPositionSaveTimer >= 30)
                {
                    btdPositionSaveTimer = 0;
                    btdPositionIndex++;
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        btdPlayerPositions.Add(player.position);
                        btdPlayerVelocities.Add(-player.velocity);
                    }
                    else
                    {
                        btdPlayerPositions.Add(Main.player[Main.myPlayer].position);
                        btdPlayerVelocities.Add(-Main.player[Main.myPlayer].velocity);
                    }
                    amountOfSavedData++;
                }
            }
            if (bitesTheDustActivated)      //Actual activation
            {
                if (totalRewindTime == 0 && Main.myPlayer != Projectile.owner)
                {
                    totalRewindTime = CalculateRewindTime();
                    if (JoJoStands.SoundsLoaded)
                        SoundEngine.PlaySound(BtdSound, Projectile.Center);
                }

                btdRevertTimer++;
                currentRewindTime++;
                mPlayer.bitesTheDustActive = true;
                mPlayer.biteTheDustEffectProgress = (float)currentRewindTime / (float)totalRewindTime;
                if (Main.netMode == NetmodeID.MultiplayerClient && Projectile.owner != Main.myPlayer)
                    Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().biteTheDustEffectProgress = (float)currentRewindTime / (float)totalRewindTime;
                Main.time = MathHelper.Lerp((float)Main.time, (float)savedWorldData.worldTime, mPlayer.biteTheDustEffectProgress);
                if (btdRevertTimer >= btdRevertTime)
                {
                    btdRevertTime = (int)(btdRevertTime * 0.8f);
                    if (btdRevertTime < 2)
                        btdRevertTime = 2;

                    btdRevertTimer = 0;
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        player.position = btdPlayerPositions[btdPositionIndex];
                        player.velocity = btdPlayerVelocities[btdPositionIndex];
                    }
                    else
                    {
                        if (btdPlayerPositions != null && btdPositionIndex < btdPlayerPositions.Count)
                        {
                            Main.player[Main.myPlayer].position = btdPlayerPositions[btdPositionIndex];
                            Main.player[Main.myPlayer].velocity = btdPlayerVelocities[btdPositionIndex];
                        }
                    }
                    btdPositionIndex--;
                    if (btdPositionIndex <= 0)
                    {
                        bitesTheDustActivated = false;
                        mPlayer.bitesTheDustActive = false;
                        mPlayer.biteTheDustEffectProgress = 0f;
                        mPlayer.standChangingLocked = false;
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            player.statLife = savedPlayerDatas[0].playerBTDHealth;
                            player.position = savedPlayerDatas[0].playerBTDPos;
                            player.velocity = Vector2.Zero;
                            player.inventory = savedPlayerDatas[0].playerBTDInventory;
                            player.ChangeDir(savedPlayerDatas[0].playerDirection);
                            for (int i = 0; i < savedPlayerDatas[0].buffTypes.Length; i++)
                            {
                                player.buffType[i] = savedPlayerDatas[0].buffTypes[i];
                                player.buffTime[i] = savedPlayerDatas[0].buffTimes[i];
                            }
                        }
                        else
                        {
                            for (int i = 0; i < savedPlayerDatas.Length; i++)
                            {
                                if (!savedPlayerDatas[i].active)
                                    continue;

                                Player otherPlayer = Main.player[savedPlayerDatas[i].whoAmI];
                                if (otherPlayer.active)
                                {
                                    otherPlayer.statLife = savedPlayerDatas[i].playerBTDHealth;
                                    otherPlayer.position = savedPlayerDatas[i].playerBTDPos;
                                    otherPlayer.velocity = Vector2.Zero;
                                    if (savedPlayerDatas[i].whoAmI == Main.myPlayer)
                                        otherPlayer.inventory = savedPlayerDatas[i].playerBTDInventory;
                                    otherPlayer.ChangeDir(savedPlayerDatas[i].playerDirection);
                                    otherPlayer.ClearBuff(ModContent.BuffType<BitesTheDust>());

                                    for (int b = 0; b < savedPlayerDatas[i].buffTypes.Length; b++)
                                    {
                                        otherPlayer.buffType[b] = savedPlayerDatas[i].buffTypes[b];
                                        otherPlayer.buffTime[b] = savedPlayerDatas[i].buffTimes[b];
                                    }
                                }
                            }
                        }

                        Main.time = savedWorldData.worldTime;
                        player.ClearBuff(ModContent.BuffType<BitesTheDust>());
                        SoundEngine.PlaySound(kqClickSound);
                        if (Projectile.owner == Main.myPlayer)
                        {
                            SyncCall.SyncBitesTheDust(player.whoAmI, false);
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(120));
                        }
                        if (Main.netMode != NetmodeID.SinglePlayer && Projectile.owner != Main.myPlayer)
                        {
                            Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().bitesTheDustActive = false;
                            Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().biteTheDustEffectProgress = 0f;
                            JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.BiteTheDustEffect, false);
                            JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.BiteTheDustEffect, 0f);
                        }
                        /*for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPCData savedData = savedWorldData.npcData[i];
                            if (!Main.npc[i].active || Main.npc[i].type != savedData.type)
                            {
                                NPC remadeNPC = Main.npc[NPC.NewNPC(Projectile.GetSource_FromThis(), (int)savedData.position.X, (int)savedData.position.Y, savedData.type)];
                                remadeNPC.life = savedData.health;
                                remadeNPC.ai = savedData.ai;
                                remadeNPC.velocity = savedData.velocity;
                            }
                            else
                            {
                                NPC npc = Main.npc[i];
                                npc.position = savedData.position;
                                npc.velocity = savedData.velocity;
                                npc.ai = savedData.ai;
                                npc.life = savedData.health;
                            }
                        }*/
                        saveDataCreated = false;
                    }
                }
            }


            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && Projectile.ai[0] == 0f)
                {
                    if (!mPlayer.canStandBasicAttack)
                    {
                        idleFrames = true;
                        attackFrames = false;
                        return;
                    }

                    attackFrames = true;
                    Projectile.netUpdate = true;
                    if (Projectile.frame == 4 && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<ExplosiveBubble>(), newBubbleDamage, 6f, Projectile.owner, 1f, Projectile.whoAmI);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (Projectile.frame >= 5)
                        Projectile.frame = 0;
                }
                else if (!secondaryAbilityFrames)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        idleFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && Projectile.ai[0] == 0f && shootCount <= 0)
                {
                    secondaryAbilityFrames = true;
                    Projectile.ai[0] = 1f;      //to detonate all bombos
                    SoundEngine.PlaySound(kqClickSound);
                    shootCount += 45;
                }
                if (secondaryAbilityFrames && Projectile.ai[0] == 1f)
                {
                    if (Projectile.frame >= 2)
                    {
                        Projectile.ai[0] = 0f;
                        idleFrames = true;
                        attackFrames = false;
                        secondaryAbilityFrames = false;
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                NPC target = FindNearestTarget(350f);
                if (target != null)
                {
                    attackFrames = true;
                    idleFrames = false;
                    Projectile.direction = 1;
                    if (target.position.X - Projectile.Center.X < 0)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;

                    if (attackFrames && Projectile.frame == 4 && shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.position - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<ExplosiveBubble>(), newBubbleDamage, 6f, Projectile.owner, 0f, Projectile.whoAmI);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    idleFrames = true;
                    attackFrames = false;
                }
            }
        }

        private int CalculateRewindTime()
        {
            int rewindTime = 0;
            int currentTimeAmount = 35;
            for (int i = 0; i < amountOfSavedData; i++)
            {
                rewindTime += currentTimeAmount;
                currentTimeAmount = (int)(currentTimeAmount * 0.8);
            }
            return rewindTime;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(bitesTheDustActive);
            writer.Write(bitesTheDustActivated);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            bitesTheDustActive = reader.ReadBoolean();
            bitesTheDustActivated = reader.ReadBoolean();
        }

        /*public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Attack")
                Projectile.frame = 5;
        }*/

        public override void SelectAnimation()
        {
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
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/KillerQueenBTD/KQBTD_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 6, newShootTime / 2, false);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 4, newShootTime / 4, false);
            }
        }
    }
}