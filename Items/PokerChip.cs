using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items
{
    public class PokerChip : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A poker chip said to have a soul inside...");
            SacrificeTotal = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 35, 72, 0);
            Item.rare = 8;
            Item.maxStack = 1;
        }

        public override void OnConsumeItem(Player player)
        {
            player.statLife += (player.statLifeMax / 2);
        }
    }
}