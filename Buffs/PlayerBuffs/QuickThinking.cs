using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class QuickThinking : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quick Thinking");
            Description.SetDefault("You are much quicker on knowing what to do and how to do it.\nStand Speed incrased by 1.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 1;
        }
    }
}