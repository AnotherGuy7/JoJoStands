using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Items
{
    public class PokerChip : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A poker chip said to have a soul inside...");
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.buyPrice(gold: 40, silver: 45);
            Item.rare = ItemRarityID.LightPurple;
            Item.maxStack = 1;
        }

        public override void OnConsumeItem(Player player)
        {
            player.statLife += player.statLifeMax / 2;
        }
    }
}