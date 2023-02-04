using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesACT1 : StandItemClass
    {
        public override int StandSpeed => 16;
        public override int StandType => 1;
        public override string StandProjectileName => "Echoes";
        public override int StandTier => 2;
        public override int StandTierDisplayOffset => -1;
        public override Color StandTierDisplayColor => Color.LightGreen;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 1)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate! \nRight-click: Place sound mark at entity! \nSpecial: Remote Control \nPassive: Normal attack inflict debuff that get stronger with each hit! Sound mark increases damage of debuff!");
        }

        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.width = 38;
            Item.height = 36;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EchoesACT0>())
                .AddIngredient(ItemID.HellstoneBar, 18)
                .AddIngredient(ItemID.Gel, 36)
                .AddIngredient(ItemID.Emerald)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
