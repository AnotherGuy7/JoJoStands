using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Seasonal.FreeDom
{ 
	public class FreeDomStandT1 : StandClass
	{
        public override float ProjectileSpeed => 100f;
        public override int HalfStandHeight => 34;
        public override StandAttackType StandType => StandAttackType.None;
        public override int ProjectileDamage => 0;
        public override int ShootTime => 40;
        public int MandomTimer = 0;
        public int MandomSavedCount;
        public WorldData[] savedWorldData = new WorldData[7];
        public struct PlayerData
        {
            public int playerIndex;
            public float PlayerX;
            public float PlayerY;
            public int playerMandomHealth;
            public int playerDirection;
            public int[] buffTypes;
            public int[] buffTimes;
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
            public int Index;
        }

        public struct WorldData
        {
            public int worldTime;
            public int playerLength;
            public PlayerData[] PlayerData;
            public int NPCLength;
            public NPCData[] npcData;
            public int MandomPlayerIndex;
        }

        public override void AI()
        {
            MandomTimer++;
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            StayBehind();
            currentAnimationState = AnimationState.Idle;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;
            //saving
            if (MandomTimer > 60)
            {
                MandomTimer = 0;
                MandomSavedCount++;
                for (int i = 6; i > 0;i--) 
                { 
                    if (i > 0)
                    {
                        savedWorldData[i] = savedWorldData[i -1];
                    }
                }
                savedWorldData[0] = new WorldData();
                savedWorldData[0].NPCLength = 0;
                savedWorldData[0].worldTime = (int)Main.time;
                savedWorldData[0].PlayerData = new PlayerData[Main.player.Length];

                for (int i = 0;i <Main.maxPlayers; i++)
                {
                    savedWorldData[0].PlayerData[i] = new PlayerData();
                    savedWorldData[0].PlayerData[i].playerIndex = i;
                    savedWorldData[0].PlayerData[i].playerMandomHealth = Main.player[i].statLife;
                    savedWorldData[0].PlayerData[i].PlayerX = Main.player[i].position.X;
                    savedWorldData[0].PlayerData[i].PlayerY = Main.player[i].position.Y;
                    savedWorldData[0].PlayerData[i].playerDirection = Main.player[i].direction;
                    int amountOfBuffs = Main.player[i].CountBuffs();
                    savedWorldData[0].PlayerData[i].buffTypes = new int[amountOfBuffs];
                    savedWorldData[0].PlayerData[i].buffTimes = new int[amountOfBuffs];
                    for (int i2 = 0; i2 < amountOfBuffs; i2++)
                    {
                        savedWorldData[0].PlayerData[i].buffTypes[i2] = Main.player[i].buffType[i2];
                        savedWorldData[0].PlayerData[i].buffTimes[i2] = Main.player[i].buffTime[i2];
                    }
                }
                savedWorldData[0].npcData = new NPCData[Main.npc.Length];
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].townNPC)
                    {
                        savedWorldData[0].npcData[i] = new NPCData
                        {
                            ai = Main.npc[i].ai,
                            active = true,
                            direction = Main.npc[i].direction,
                            health = Main.npc[i].life,
                            Index = i,
                            position = Main.npc[i].position,
                            type = Main.npc[i].type,
                            velocity = Main.npc[i].velocity
                        };
                        savedWorldData[0].NPCLength++;
                    }

                }
                Projectile.netUpdate = true;
            }
            if (MandomSavedCount < 7)
            {
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(1));
            }
            //activation
            if ((SpecialKeyPressed() || player.statLife <= 10) && Projectile.owner == Main.myPlayer && !playerHasAbilityCooldown)
            {
                Main.time = savedWorldData[5].worldTime;
                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/TimeRewind"));
                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/FreeDom-Eagle"));
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(7));
                player.AddBuff(ModContent.BuffType<DuelingSpirit>(),360);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < savedWorldData[6].PlayerData.Length;i++)
                    { 
                        PlayerData playerDataVar = savedWorldData[6].PlayerData[i];
                        if (playerDataVar.playerIndex == Main.myPlayer)
                        {
                            float PosX = playerDataVar.PlayerX;
                            float PosY = playerDataVar.PlayerY;
                            Vector2 position = new Vector2(PosX, PosY);
                            Main.player[i].position = position;
                            Main.player[i].statLife = playerDataVar.playerMandomHealth;
                            Main.player[i].ChangeDir(playerDataVar.playerDirection);

                        } else
                        {
                            SyncCall.SyncMandomActivation(Main.player[Main.myPlayer].whoAmI, playerDataVar.playerIndex, playerDataVar.PlayerX, playerDataVar.PlayerY, playerDataVar.playerMandomHealth, playerDataVar.playerDirection);
                        }
                    }
                } else
                {
                    for (int i = 0; i < savedWorldData[6].PlayerData.Length; i++)
                    {
                        PlayerData playerDataVar = savedWorldData[6].PlayerData[i];
                        if (playerDataVar.playerIndex == Projectile.owner)
                        {
                            float PosX = playerDataVar.PlayerX;
                            float PosY = playerDataVar.PlayerY;
                            Vector2 position = new Vector2(PosX, PosY);
                            Main.player[i].position = position;
                            Main.player[i].statLife = playerDataVar.playerMandomHealth;
                            Main.player[i].ChangeDir(playerDataVar.playerDirection);

                        }
                    }
                }
                for (int i = 0; i < savedWorldData[5].npcData.Length ;i++)
                {
                    NPCData savedData = savedWorldData[5].npcData[i];
                    if (Main.npc[savedData.Index].active && !Main.npc[savedData.Index].townNPC)
                    {
                        Main.npc[savedData.Index].position = savedData.position;
                        Main.npc[savedData.Index].velocity = savedData.velocity;
                        Main.npc[savedData.Index].ai = savedData.ai;
                        Main.npc[savedData.Index].life = savedData.health;
                    }
                    else if (!Main.npc[savedData.Index].active || Main.npc[savedData.Index].type != savedData.type && !Main.npc[savedData.Index].townNPC)
                    {
                        NPC remadeNPC = Main.npc[NPC.NewNPC(Projectile.GetSource_FromThis(), (int)savedData.position.X, (int)savedData.position.Y, savedData.type)];
                        remadeNPC.life = savedData.health;
                        remadeNPC.ai = savedData.ai;
                        remadeNPC.velocity = savedData.velocity;
                    }
                }
            }

        }
        public override void SelectAnimation()
        {
            if (currentAnimationState == AnimationState.Idle)
            {
                PlayAnimation("Idle");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Seasonal/FreeDom/FreeDom_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 15, true);
            }
        }
    }
}
