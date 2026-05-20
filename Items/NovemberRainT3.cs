using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class NovemberRainT3 : StandItemClass
    {
        public override int StandSpeed => 15;
        public override int StandType => 2;
        public override string StandIdentifierName => "NovemberRain";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.SkyBlue;

        public override string Texture => Mod.Name + "/Items/NovemberRainT1";

        public override void SetDefaults()
        {
            Item.damage = 84;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NovemberRainT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.SoulofNight, 6)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
