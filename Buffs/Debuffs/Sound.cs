using Terraria;
using Terraria.ModLoader;
using JoJoStands.Buffs.PlayerBuffs;

namespace JoJoStands.Buffs.Debuffs
{
    public class Sound : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tinnitus");
            Description.SetDefault("The punch was weak, but that sound...");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                player.ClearBuff(buffIndex);
        }
    }
}