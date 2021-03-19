using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class Sunscreen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunscreen");
            Tooltip.SetDefault("For all the hot days you spend wandering.");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.rare = ItemRarityID.Green;
            item.buffTime = 3 * 60 * 60;
            item.buffType = mod.BuffType("SunscreenBuff");
        }
    }
}
