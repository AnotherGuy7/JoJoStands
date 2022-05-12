using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EctoPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ecto Pearl");
            Tooltip.SetDefault("A otherworldly pearl that has been inherited by a trapped soul from the dungeon.\nPermanently incrases stand range radius by 0.5 tiles");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 0, 0, 60);
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<MyPlayer>().usedEctoPearl = true;
            player.ConsumeItem(ModContent.ItemType<EctoPearl>());
            return true;
        }
    }
}