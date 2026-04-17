using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class NovemberRainFinal : StandItemClass
    {
        public override int StandSpeed => 3;
        public override int StandType => 1;
        public override string StandIdentifierName => "NovemberRain";
        public override int StandTier => 4;
        public override Color StandTierDisplayColor => Color.SkyBlue;

        public override string Texture => Mod.Name + "/Items/NovemberRainT1";

        public override void SetDefaults()
        {
            Item.damage = 95;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NovemberRainT3>())
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddIngredient(ItemID.ChlorophyteBar, 15)
                .AddIngredient(ItemID.ShroomiteBar, 10)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
