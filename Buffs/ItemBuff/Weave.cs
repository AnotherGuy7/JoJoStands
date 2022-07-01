using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Weave : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weave");
            Description.SetDefault("These strings create a sheet as strong as metal when woven together... and you're wrapped in it!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<MyPlayer>().stoneFreeWeaveAbilityActive)
                player.buffTime[buffIndex] = 2;
        }
    }
}