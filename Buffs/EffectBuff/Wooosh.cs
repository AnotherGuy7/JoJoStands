using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class Whoosh : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Swiftness;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("WOOOSH!");
            // Description.SetDefault("Tailwind increases your speed!");
        }
        public override void UpdateBuffOnPlayer(Player player)
        {
            player.moveSpeed *= 1.5f;
            if (player.HasBuff(ModContent.BuffType<WoooshDebuff>()))
                player.ClearBuff(ModContent.BuffType<WoooshDebuff>());
        }
    }
}