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
            Tooltip.SetDefault("An amulet that perfectly represents and enchances the form of the soul.\n30% increased Stand attack damage\n2 increased Stand Speed\n20% Stand Ability cooldown reduction\n30% increased Stand crit chance\nMakes melee stands inflict Infected on enemies.\nIncreased defense while the Stand is out");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts += 0.3f;
            mPlayer.standSpeedBoosts += 2;
            mPlayer.standCooldownReduction += 0.2f;
            mPlayer.standCritChangeBoosts += 30f;
            mPlayer.awakenedAmuletEquipped = true;
            if (mPlayer.standOut)
                player.statDefense += 12;
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