using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Stolen : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stolen");
            Description.SetDefault("Your stand disc has been stolen!");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().StandOut = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity.X = 0f;
            npc.lifeRegen = -6;
            npc.lifeRegenExpectedLossPerSecond = 6;
        }
    }
}