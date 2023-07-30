using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class AwakenedAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 8));
            // Tooltip.SetDefault("An amulet that perfectly represents and enchances the form of the soul.\n2 increased Stand Speed\n10% increased Stand dodge chance\n20% Stand Ability cooldown reduction\n10% increased Stand crit chance\n10 increased Stand armor penetration\nIncreased defense while the Stand is out");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(platinum: 1);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standSpeedBoosts += 2;
            mPlayer.standDodgeChance += 10f;
            mPlayer.standCooldownReduction += 0.2f;
            mPlayer.standCritChangeBoosts += 10f;
            mPlayer.standArmorPenetration += 10;
            mPlayer.standAccessoryDefense += 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldAmuletOfManipulation>())
                .AddIngredient(ModContent.ItemType<GoldAmuletOfServing>())
                .AddIngredient(ModContent.ItemType<GoldAmuletOfAdapting>())
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 5)
                .AddRecipeGroup(RecipeGroupID.Fragment, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PlatinumAmuletOfManipulation>())
                .AddIngredient(ModContent.ItemType<PlatinumAmuletOfServing>())
                .AddIngredient(ModContent.ItemType<PlatinumAmuletOfAdapting>())
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 5)
                .AddRecipeGroup(RecipeGroupID.Fragment, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}