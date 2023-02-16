using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesAct0 : StandItemClass
    {
        public override int StandSpeed => 2;
        public override int StandType => 1;
        public override string StandProjectileName => "Echoes";
        public override int StandTier => 1;
        public override int StandTierDisplayOffset => -1;
        public override Color StandTierDisplayColor => Color.LightGreen;
        public override string Texture
        {
            get { return Mod.Name + "/Items/EchoesAct0"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 0)");
            Tooltip.SetDefault("Left-click to... throw?\nOnly for those with the strongest of wills.");
        }

        public override void SetDefaults()
        {
            Item.damage = 108;
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
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
