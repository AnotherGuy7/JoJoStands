using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT2 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;
        public override string StandIdentifierName => "TheWorld";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.Yellow;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The World (Tier 2)");
            // Tooltip.SetDefault("Punch enemies at a really fast rate! \nSpecial: Stop time for 2 seconds!\nUsed in Stand Slot");
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StarPlatinumT2>();
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TheWorldT1>())
                .AddIngredient(ItemID.HellstoneBar, 25)
                .AddRecipeGroup("JoJoStandsWatch")
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
