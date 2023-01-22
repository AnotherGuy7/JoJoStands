using Terraria;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class QuickThinking : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quick Thinking");
            Description.SetDefault("You are much quicker on knowing what to do and how to do it.\nStand Speed incrased by 1.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 1;
        }
    }
}