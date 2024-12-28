using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT3 : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandIdentifierName => "TheWorld";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.Yellow;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The World (Tier 3)");
            // Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw knives! \nSpecial: Stop time for 5 seconds!\nSecond Special: Stop time and surround an enemy with knives!\nNote: Hunter's Knives are made with 1 iron bar at a furnace and are required in order to throw knives.\nUsed in Stand Slot");
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StarPlatinumT3>();
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
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
                .AddIngredient(ModContent.ItemType<TheWorldT2>())
                .AddIngredient(ItemID.HallowedBar, 19)
                .AddRecipeGroup("JoJoStandsGold-TierBar", 15)
                .AddIngredient(ModContent.ItemType<SoulofTime>(), 2)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
