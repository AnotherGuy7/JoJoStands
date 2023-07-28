using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Reinforcements : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Reinforcements");
            // Description.SetDefault("Reinforcements have arrived!");
        }

        public override void OnBuffEnd(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int abilityCooldownTime = (5 - (2 * (mPlayer.standTier - 3))) * 60;
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(abilityCooldownTime));
        }
    }
}