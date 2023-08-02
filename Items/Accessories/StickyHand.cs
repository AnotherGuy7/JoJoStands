using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class StickyHand : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sticky Hand");
            /* Tooltip.SetDefault("25% increased Stand dodge chance." +
                "\n100% increased stand defence bonus" +
                "\nHealth is slowly drained\nAllows you to make a last stand before death.\nDuring the last stand phase, damage and critial strike chances are doubled."); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 6);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().standDodgeChance += 15f;
            player.GetModPlayer<MyPlayer>().siliconLifeformCarapace = true;
            player.GetModPlayer<MyPlayer>().zippedHandEquipped = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SiliconLifeformCarapace>())
                .AddIngredient(ModContent.ItemType<ZippedHand>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}