using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class DeathLoop : ModBuff
    {
        public static int deathTimes = 0;
        public static bool Looping3x = false;
        public static bool Looping10x = false;
        public static Vector2 deathPosition;
        public static int deathNPCType = 0;
        public static int targetNPCWhoAmI = -1;

        public int deathTimeAdd = 0;
        public int deathLoopTimer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Loop");
            Description.SetDefault("The next enemy you kill will go through endless deaths...");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that you can't cancel it or it breaks everything
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.deathLoopActive = true;
            if (Main.netMode == NetmodeID.Server)
                deathTimeAdd = 1;

            if (Main.netMode == NetmodeID.SinglePlayer)
                deathTimeAdd = 0;

            if ((deathTimes < (3 + deathTimeAdd) && Looping3x) || (deathTimes < (10 + deathTimeAdd) && Looping10x))
                player.AddBuff(ModContent.BuffType<DeathLoop>(), 2);

            if (deathLoopTimer >= 60)
                deathLoopTimer = 0;

            if (!mPlayer.timestopActive)        //so the effect stops during a timestop
            {
                if (Looping3x)
                {
                    deathLoopTimer++;
                    if (deathLoopTimer >= 60 && deathTimes <= (1 + deathTimeAdd))
                    {
                        int spawnedNPC = NPC.NewNPC(player.GetSource_FromThis(), (int)deathPosition.X, (int)deathPosition.Y, deathNPCType);
                        Main.npc[spawnedNPC].GetGlobalNPC<NPCs.JoJoGlobalNPC>().spawnedByDeathLoop = true;
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/GEDeathLoop"));
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                    if (deathLoopTimer >= 60 && deathTimes == (2 + deathTimeAdd))
                    {
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                }
                if (Looping10x)
                {
                    deathLoopTimer++;
                    if (deathLoopTimer >= 30 && deathTimes <= (8 + deathTimeAdd))
                    {
                        int spawnedNPC = NPC.NewNPC(player.GetSource_FromThis(), (int)deathPosition.X, (int)deathPosition.Y, deathNPCType);
                        Main.npc[spawnedNPC].GetGlobalNPC<NPCs.JoJoGlobalNPC>().spawnedByDeathLoop = true;
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/GEDeathLoop"));
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                    if (deathLoopTimer >= 30 && deathTimes == (9 + deathTimeAdd))
                    {
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                }
            }
            if ((deathTimes >= (3 + deathTimeAdd) && Looping3x) || (deathTimes >= (10 + deathTimeAdd) && Looping10x) || targetNPCWhoAmI == -1 || !mPlayer.standOut)
            {
                mPlayer.deathLoopActive = false;
                deathTimes = 0;
                deathLoopTimer = 0;
                Looping3x = false;
                Looping10x = false;
                player.ClearBuff(ModContent.BuffType<DeathLoop>());
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(60));
                deathPosition = Vector2.Zero;
                deathNPCType = 0;
                targetNPCWhoAmI = -1;
            }
        }
    }
}