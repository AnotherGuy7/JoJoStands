using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueenBTD
{
    public class KillerQueenBTDStand : StandClass
    {
        public override float shootSpeed => 4f;
        public override int shootTime => 30;
        public override int halfStandHeight => 37;
        public override int standType => 2;
        public override int standOffset => -10;
        public override string poseSoundName => "IWouldntLose";
        public override string spawnSoundName => "Killer Queen";

        private int btdStartDelay = 0;
        private int bubbleDamage = 684;      //not using projectileDamage cause this one changes

        private int btdPlaceTimer = 0;
        private int btdPositionSaveTimer = 0;
        private int btdPositionIndex = 0;
        private int btdRevertTimer = 0;
        private int btdRevertTime = 0;
        private int amountOfSavedData = 0;
        private int currentRewindTime = 0;
        private int totalRewindTime = 0;
        private Vector2[] btdPlayerPositions;       //Positions of KQ:BTD's owner.
        private Vector2[] btdPlayerVelocities;
        private bool bitesTheDustActive;
        private bool bitesTheDustActivated;
        private PlayerData savedPlayerData;
        private WorldData savedWorldData;

        public struct PlayerData
        {
            public Vector2 playerBTDPos;
            public Item[] playerBTDInventory;
            public int playerBTDHealth;
            public int playerDirection;
            public int[] buffTypes;
            public int[] buffTimes;
        }

        public struct WorldData
        {
            public int worldTime;
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
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (Main.dayTime)
            {
                bubbleDamage = 684;
            }
            if (!Main.dayTime)
            {
                bubbleDamage = 456;
            }
            drawOriginOffsetY = -halfStandHeight;
            int newBubbleDamage = (int)(bubbleDamage * mPlayer.standDamageBoosts);

            if (!attackFrames)
                StayBehind();
            if (attackFrames)
                GoInFront();

            bitesTheDustActive = player.HasBuff(mod.BuffType("BitesTheDust"));
            if (SpecialKeyPressed() && !bitesTheDustActive)
            {
                btdPositionIndex = 0;
                amountOfSavedData =  0;
                btdPlayerPositions = new Vector2[500];
                btdPlayerVelocities = new Vector2[500];
                btdPlayerPositions[0] = player.position;
                btdPlayerVelocities[0] = player.velocity;
                currentRewindTime = 0;
                btdRevertTime = 35;

                savedPlayerData = new PlayerData();
                savedPlayerData.playerBTDHealth = player.statLife;
                savedPlayerData.playerBTDInventory = player.inventory.Clone() as Item[];
                savedPlayerData.playerBTDPos = player.position;
                savedPlayerData.playerDirection = player.direction;

                int amountOfBuffs = player.CountBuffs();
                savedPlayerData.buffTypes = new int[amountOfBuffs];
                savedPlayerData.buffTimes = new int[amountOfBuffs];
                for (int i = 0; i < amountOfBuffs; i++)
                {
                    savedPlayerData.buffTypes[i] = player.buffType[i];
                    savedPlayerData.buffTimes[i] = player.buffTime[i];
                }

                player.AddBuff(mod.BuffType("BitesTheDust"), 5 * 60 * 60);      //So it doesn't save
                mPlayer.standChangingLocked = true;

                savedWorldData = new WorldData();
                savedWorldData.worldTime = (int)Main.time;
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
            }

            if (SpecialKeyPressed() && bitesTheDustActive && btdStartDelay <= 0)
            {
                if (JoJoStands.SoundsLoaded)
                {
                    bitesTheDustActivated = true;
                    totalRewindTime = CalculateRewindTime();
                    player.AddBuff(mod.BuffType("BitesTheDust"), 10);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BiteTheDustEffect"));
                }
                else
                    btdStartDelay = 205;
            }
            if (JoJoStands.SoundsLoaded && btdStartDelay > 0)
            {
                btdStartDelay--;
                if (btdStartDelay <= 0)
                {
                    bitesTheDustActivated = true;
                    totalRewindTime = CalculateRewindTime();
                    Terraria.Audio.LegacySoundStyle biteTheDust = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/BiteTheDust");
                    biteTheDust.WithVolume(MyPlayer.ModSoundsVolume);
                    Main.PlaySound(biteTheDust, projectile.position);
                    btdStartDelay = 1;
                }
            }
            if (bitesTheDustActive && !bitesTheDustActivated)
            {
                btdPositionSaveTimer++;
                if (btdPositionSaveTimer >= 30)
                {
                    btdPositionSaveTimer = 0;
                    btdPositionIndex++;
                    btdPlayerPositions[btdPositionIndex] = player.position;
                    btdPlayerVelocities[btdPositionIndex] = -player.velocity;
                    amountOfSavedData++;
                }
            }
            if (bitesTheDustActivated)
            {
                btdRevertTimer++;
                currentRewindTime++;
                mPlayer.bitesTheDustActive = true;
                mPlayer.biteTheDustEffectProgress = (float)currentRewindTime / (float)totalRewindTime;
                if (btdRevertTimer >= btdRevertTime)
                {
                    btdRevertTime = (int)(btdRevertTime * 0.8f);
                    if (btdRevertTime < 2)
                        btdRevertTime = 2;

                    btdRevertTimer = 0;
                    player.position = btdPlayerPositions[btdPositionIndex];
                    player.velocity = btdPlayerVelocities[btdPositionIndex];
                    btdPositionIndex--;
                    if (btdPositionIndex <= 0)
                    {
                        bitesTheDustActivated = false;
                        mPlayer.bitesTheDustActive = false;
                        player.statLife = savedPlayerData.playerBTDHealth;
                        player.position = savedPlayerData.playerBTDPos;
                        player.inventory = savedPlayerData.playerBTDInventory;
                        player.ChangeDir(savedPlayerData.playerDirection);
                        //player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(120));
                        player.ClearBuff(mod.BuffType("BitesTheDust"));
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));

                        for (int i = 0; i < savedPlayerData.buffTypes.Length; i++)
                        {
                            player.buffType[i] = savedPlayerData.buffTypes[i];
                            player.buffTime[i] = savedPlayerData.buffTimes[i];
                        }
                        mPlayer.biteTheDustEffectProgress = 0f;
                        mPlayer.standChangingLocked = false;

                        Main.time = savedWorldData.worldTime;
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPCData savedData = savedWorldData.npcData[i];
                            if (!Main.npc[i].active)
                            {
                                NPC remadeNPC = Main.npc[NPC.NewNPC((int)savedData.position.X, (int)savedData.position.Y, savedData.type)];
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
                        }
                    }
                }
            }


            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
                {
                    attackFrames = true;
                    projectile.netUpdate = true;
                    if (projectile.frame == 4 && !mPlayer.standAutoMode)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = Main.MouseWorld - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Bubble"), newBubbleDamage, 6f, projectile.owner, 1f, projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                    if (projectile.frame >= 5)
                    {
                        attackFrames = false;
                    }
                }
                else if (!secondaryAbilityFrames)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
                {
                    secondaryAbilityFrames = true;
                    projectile.ai[0] = 1f;      //to detonate all bombos
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                }
                if (secondaryAbilityFrames && projectile.ai[0] == 1f)
                {
                    if (projectile.frame >= 2)
                    {
                        projectile.ai[0] = 0f;
                        normalFrames = true;
                        attackFrames = false;
                        secondaryAbilityFrames = false;
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(350f);
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;
                    projectile.direction = 1;
                    if (target.position.X - projectile.Center.X < 0)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    if (attackFrames && projectile.frame == 4 && shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Bubble"), newBubbleDamage, 6f, projectile.owner, 0f, projectile.whoAmI);
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

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/KillerQueenBTD/KQBTD_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 6, newShootTime / 2, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 4, newShootTime / 4, false);
            }
        }
    }
}