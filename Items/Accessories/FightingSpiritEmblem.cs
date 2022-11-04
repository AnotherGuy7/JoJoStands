using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;

namespace JoJoStands.Items.Accessories
{
    public class FightingSpiritEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fighting Spirit Emblem");
            Tooltip.SetDefault("15% increased stand damage\nUp to 45% increased stand damage with attacks on same target");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(gold: 5);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.15f + 0.01f * player.GetModPlayer<MyPlayer>().fightingSpiritEmblemStack;
            player.GetModPlayer<MyPlayer>().fightingSpiritEmblem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandEmblem>())
                .AddRecipeGroup("JoJoStandsCursedIchor", 15)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ModContent.ItemType<WillToEscape>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}