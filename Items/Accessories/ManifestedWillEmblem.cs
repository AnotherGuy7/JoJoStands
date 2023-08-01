using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;

namespace JoJoStands.Items.Accessories
{
    public class ManifestedWillEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Manifested Will Emblem");
            // Tooltip.SetDefault("15% increased stand damage\n50% increased stand crit damage");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(gold: 6);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.15f;
            player.GetModPlayer<MyPlayer>().manifestedWillEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FightingSpiritEmblem>())
                .AddIngredient(ItemID.EyeoftheGolem)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 3)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 3)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 3)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 3)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 3)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 3)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}