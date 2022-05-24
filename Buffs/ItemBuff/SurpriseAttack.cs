using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SurpriseAttack : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surprise Attack");
            Description.SetDefault("You have the upper edge and you know just how to prove it.");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts += 0.15f;
            mPlayer.standCritChangeBoosts += 10f;
            mPlayer.standSpeedBoosts += 1;
        }
    }
}