using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class DeathLoop : ModBuff
    {
        public static int deathTimes = 0;
        public static bool Looping3x = false;
        public static bool Looping10x = false;
        public static float deathPositionX = 0f;
        public static float deathPositionY = 0f;
        public static int LoopNPC = 0;
        public int deathTimeAdd = 0;
        public int deathLoopTimer = 0;

        public override void SetDefaults()
        {
			DisplayName.SetDefault("Death Loop");
            Description.SetDefault("The next enemy you kill will go through endless deaths...");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that you can't cancel it or it breaks everything
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.deathLoopActive = true;
            if (Main.netMode == NetmodeID.Server)
            {
                deathTimeAdd = 1;
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                deathTimeAdd = 0;
            }
            if ((deathTimes < (3 + deathTimeAdd) && Looping3x) || (deathTimes < (10 + deathTimeAdd) && Looping10x))
            {
                player.AddBuff(mod.BuffType("DeathLoop"), 2);
            }
            if (deathLoopTimer >= 60)       //sometimes it w
            {
                deathLoopTimer = 0;
            }
            if (!player.GetModPlayer<MyPlayer>().timestopActive)        //so the effect stops during a timestop
            {
                if (Looping3x)
                {
                    deathLoopTimer++;
                    if (deathLoopTimer >= 60 && deathTimes <= (1 + deathTimeAdd))
                    {
                        int spawnedNPC = NPC.NewNPC((int)deathPositionX, (int)deathPositionY, LoopNPC);
                        Main.npc[spawnedNPC].GetGlobalNPC<NPCs.JoJoGlobalNPC>().spawnedByDeathLoop = true;
                        deathTimes += 1;
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/GEDeathLoop"));
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
                        int spawnedNPC = NPC.NewNPC((int)deathPositionX, (int)deathPositionY, LoopNPC);
                        Main.npc[spawnedNPC].GetGlobalNPC<NPCs.JoJoGlobalNPC>().spawnedByDeathLoop = true;
                        deathTimes += 1;
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/GEDeathLoop"));
                        deathLoopTimer = 0;
                    }
                    if (deathLoopTimer >= 30 && deathTimes == (9 + deathTimeAdd))
                    {
                        deathTimes += 1;
                        deathLoopTimer = 0;
                    }
                }
            }
            if ((deathTimes >= (3 + deathTimeAdd) && Looping3x) || (deathTimes >= (10 + deathTimeAdd) && Looping10x))
            {
                mPlayer.deathLoopActive = false;
                deathTimes = 0;
                Looping3x = false;
                deathLoopTimer = 0;
                Looping10x = false;
                player.ClearBuff(mod.BuffType("DeathLoop"));
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(60));
                deathPositionX = 0f;
                deathPositionY = 0f;
                LoopNPC = 0;
            }
        }
    }
}