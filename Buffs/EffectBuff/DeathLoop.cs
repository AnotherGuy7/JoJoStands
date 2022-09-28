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
        public int deathTimes = 0;
        public int deathTimeAdd = 0;
        public int deathLoopTimer = 0;

        private readonly SoundStyle deathLoopSound = new SoundStyle("JoJoStands/Sounds/GameSounds/GEDeathLoop");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Loop");
            Description.SetDefault("The targetted enemy will go through endless deaths upon death...");
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

            if ((deathTimes < (3 + deathTimeAdd) && mPlayer.deathLoopNPCIsBoss) || (deathTimes < (10 + deathTimeAdd) && !mPlayer.deathLoopNPCIsBoss))
                player.buffTime[buffIndex] = 2;

            if (deathLoopTimer >= 60)
                deathLoopTimer = 0;

            if (!mPlayer.timestopActive)        //so the effect stops during a timestop
            {
                deathLoopTimer++;
                if (mPlayer.deathLoopNPCIsBoss)
                {
                    if (deathLoopTimer >= 60 && deathTimes <= (1 + deathTimeAdd))
                    {
                        int spawnedNPC = NPC.NewNPC(player.GetSource_FromThis(), (int)mPlayer.deathLoopPosition.X, (int)mPlayer.deathLoopPosition.Y, mPlayer.deathLoopNPCType);
                        Main.npc[spawnedNPC].GetGlobalNPC<NPCs.JoJoGlobalNPC>().spawnedByDeathLoop = true;
                        SoundEngine.PlaySound(deathLoopSound);
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                }
                else
                {
                    if (deathLoopTimer >= 30 && deathTimes <= (8 + deathTimeAdd))
                    {
                        int spawnedNPC = NPC.NewNPC(player.GetSource_FromThis(), (int)mPlayer.deathLoopPosition.X, (int)mPlayer.deathLoopPosition.Y, mPlayer.deathLoopNPCType);
                        Main.npc[spawnedNPC].GetGlobalNPC<NPCs.JoJoGlobalNPC>().spawnedByDeathLoop = true;
                        SoundEngine.PlaySound(deathLoopSound);
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                }
            }
            if ((deathTimes >= (2 + deathTimeAdd) && mPlayer.deathLoopNPCIsBoss) || (deathTimes >= (9 + deathTimeAdd) && !mPlayer.deathLoopNPCIsBoss) || mPlayer.deathLoopNPCWhoAmI == -1 || !mPlayer.standOut)
            {
                mPlayer.deathLoopActive = false;
                deathTimes = 0;
                deathLoopTimer = 0;
                mPlayer.deathLoopNPCIsBoss = false;
                player.ClearBuff(ModContent.BuffType<DeathLoop>());
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(60));
                mPlayer.deathLoopPosition = Vector2.Zero;
                mPlayer.deathLoopNPCType = 0;
                mPlayer.deathLoopNPCWhoAmI = -1;
            }
        }
    }
}