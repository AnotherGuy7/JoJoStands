using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using JoJoStands.Buffs.Debuffs;

namespace JoJoStands.Buffs.EffectBuff
{
    public class Whoosh : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Swiftness; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WOOOSH!");
            Description.SetDefault("Tailwind increases your speed!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 1.5f;
            if (player.HasBuff(ModContent.BuffType<WhooshDebuff>()))
                player.ClearBuff(ModContent.BuffType<WhooshDebuff>());
        }
    }
}