using JoJoStands.Buffs.PlayerBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Tinnitus : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Confused;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tinnitus");
            Description.SetDefault("The punch was weak, but that sound...");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                player.ClearBuff(buffIndex);
        }
    }
}